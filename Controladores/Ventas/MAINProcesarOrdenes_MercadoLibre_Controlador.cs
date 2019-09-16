using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Clases.Objetos.MercadoLibre.Ordenes;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Factory;
using Sistema.Base.Listados.PDF;
using Sistema.Presentacion.Secciones.Ventas;
using Sistema.Presentacion.Secciones.Ventas.Controles;
using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Extenciones.Conexiones.API.ApiMercadoLibre;
using static Libreria.Factory.Factory;
using static Libreria.JarseConfig;

namespace Sistema.Controladores
{

    public static partial class ControladoresDeSeccion
    {

        public partial class MAINProcesarOrdenes_MercadoLibre_Controlador : Notify, IControladorUI
        {

            public MAINProcesarOrdenes_MercadoLibre_Controlador(CuentaML Cuenta)
            {
                this.Cuenta = Cuenta;
                UIMain = new UC_Ventas_MercadoLibre();
                UIMain.DataContext = this;
                this.PropertyChanged += Find;
                this.PropertyChanged += Find2;
                Pendientes.CollectionChanged += (s, e) => Pendientes_CollectionChanged(Pendientes, e, UIMain.StackContent);
                Procesadas.CollectionChanged += (s, e) => Procesadas_CollectionChanged(Procesadas, e, UIMain.StackContent2);
                Buscador.CollectionChanged += (s, e) => Pendientes_CollectionChanged(Buscador, e, UIMain.StackSearch);
                Buscador2.CollectionChanged += (s, e) => Procesadas_CollectionChanged(Buscador2, e, UIMain.StackSearch2);
                Set();

            }

            private void Procesadas_CollectionChanged(OCOrden oc, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, StackPanel Stack)
            {

                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var ui = new UCProcesadas(oc[e.NewStartingIndex]);
                    //ui.Orden.ActivateReloadTimer(10);
                    ui.Orden.StatusChanged += (a, b) =>
                    {
                        if (a == Orden.Objetos.Estado.Cancelada) { oc.Remove(ui.Orden); }
                    };
                    Stack.Children.Add(ui);
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    Stack.Children.RemoveAt(e.OldStartingIndex);
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                    Stack.Children.Clear();
            }
            private void Pendientes_CollectionChanged(OCOrden oc, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, StackPanel Stack)
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var ui = new UCOrden(oc[e.NewStartingIndex]);
                    //ui.Orden.ActivateReloadTimer();
                    ui.Orden.StatusChanged += (a, b) =>
                    {
                        if (a.Id == Orden.Objetos.Estado.Enproceso.Id) { oc.Remove(ui.Orden); Procesadas.Add(ui.Orden); }
                        if (a == Orden.Objetos.Estado.Facturada) { oc.Remove(ui.Orden); Procesadas.Add(ui.Orden); }
                        if (a == Orden.Objetos.Estado.Despachada) { oc.Remove(ui.Orden); Procesadas.Add(ui.Orden); }
                        if (a == Orden.Objetos.Estado.Cancelada) { oc.Remove(ui.Orden); }
                    };
                    Stack.Children.Add(ui);
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    Stack.Children.RemoveAt(e.OldStartingIndex);
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                    Stack.Children.Clear();
            }

            internal UC_Ventas_MercadoLibre UIMain { get; }
            public OCOrden Pendientes { get; } = new OCOrden();
            public OCOrden Procesadas { get; } = new OCOrden();

            #region SET
            void Set()
            {
                Loading = true;
                Loading2 = true;
                Pendientes.Clear();
                Procesadas.Clear();
                new WorkerHelper(
                (s, e) =>
                {
                    try
                    {
                        e.Result = Factory.MercadoLibre.Ordenes.Pendientes(Cuenta);
                    }
                    catch (Exception ex)
                    {
                        e.Result = ex;
                    }

                },
                (s, e) =>
                {
                    if (e.Result is Exception)
                        App.MessageController.Error(e.Result.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
                    else
                        foreach (var item in e.Result as OCOrden)
                            Pendientes.Add(item);

                    Loading = false;
                    OnPropertyChanged();
                }).Run();

                new WorkerHelper(
                    (s, e) =>
                    {
                        try
                        {
                            e.Result = Factory.MercadoLibre.Ordenes.EnProceso(Cuenta);
                        }
                        catch (Exception ex)
                        {
                            e.Result = ex;
                        }

                    },
                    (s, e) =>
                    {
                        if (e.Result is Exception)
                            App.MessageController.Error(e.Result.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
                        else
                            foreach (var item in e.Result as OCOrden)
                                Procesadas.Add(item);

                        Loading2 = false;
                        OnPropertyChanged();
                    }).Run();
            }
            #endregion

            #region INTERFAZ
            public ICommand IRefrescar => new Comando(() => Set());
            #endregion

            #region LOADING 
            private bool loading = true;
            private bool loading2 = true;
            public bool Loading { get => loading; set { loading = value; OnPropertyChanged("Loading"); OnPropertyChanged("NotLoading"); } }
            public bool Loading2 { get => loading2; set { loading2 = value; OnPropertyChanged("Loading2"); OnPropertyChanged("NotLoading2"); } }
            public bool NotLoading => !Loading;
            public bool NotLoading2 => !Loading2;
            #endregion

            #region BUSCADOR 1
            public bool searching = false;
            public bool noResult = false;
            public bool NoResult { get => noResult; set { noResult = value; OnPropertyChanged("NoResult"); } }
            public bool Searching { get => searching; set { searching = value; OnPropertyChanged("Searching"); OnPropertyChanged("NotSearching"); } }
            public bool NotSearching => !Searching;
            public OCOrden Buscador { get; } = new OCOrden();
            private string findText = "";
            public string FindText { get => findText; set { findText = value.ToUpper(); OnPropertyChanged("FindText"); } }
            private void Find(object sender, PropertyChangedEventArgs b)
            {
                if (b.PropertyName == "FindText")
                {
                    if (FindText.IsNotEmpty() && (FindText.Length > 3 || FindText.ToInt() > 0))
                    {
                        Buscador.Clear();
                        Loading = true;
                        NoResult = false;
                        Pendientes.Where(x =>
                                x.Id.ToStr().Contains(FindText) ||
                                x.Items.Where(a => a.Publicacion.Articulo.Codigo.ToStr().Contains(FindText.FormatearCodigoArticulo())).ToList().HasRows() ||
                                x.Items.Where(a => a.Publicacion.Titulo.ToStr().ToUpper().Contains(FindText.ToUpper())).ToList().HasRows() ||
                                x.Cliente.Nombre.ToStr().ToUpper().Contains(FindText.ToUpper()) ||
                                x.Cliente.Usuario.ToStr().ToUpper().Contains(FindText.ToUpper())
                                ).ToList().ForEach(x => Buscador.Add(x));
                        if (!Buscador.HasRows())
                            NoResult = true;
                        Searching = true;
                        Loading = false;
                    }
                    else
                    {
                        Searching = false;
                        Buscador.Clear();
                    }
                }
            }
            #endregion
            #region BUSCADOR 2
            public bool searching2 = false;
            public bool noResult2 = false;
            public bool NoResult2 { get => noResult2; set { noResult2 = value; OnPropertyChanged("NoResult2"); } }
            public bool Searching2 { get => searching2; set { searching2 = value; OnPropertyChanged("Searching2"); OnPropertyChanged("NotSearching2"); } }
            public bool NotSearching2 => !Searching2;
            public OCOrden Buscador2 { get; } = new OCOrden();
            private string findText2 = "";
            public string FindText2 { get => findText2; set { findText2 = value.ToUpper(); OnPropertyChanged("FindText2"); } }
            private void Find2(object sender, PropertyChangedEventArgs b)
            {
                if (b.PropertyName == "FindText2")
                {
                    if (FindText2.IsNotEmpty() && (FindText2.Length > 3 || FindText2.ToInt() > 0))
                    {
                        Buscador2.Clear();
                        Loading2 = true;
                        NoResult2 = false;
                        Procesadas.Where(x =>
                                x.Id.ToStr().Contains(FindText2) ||
                                x.Items.Where(a => a.Publicacion.Articulo.Codigo.ToStr().Contains(FindText2.FormatearCodigoArticulo())).ToList().HasRows() ||
                                x.Items.Where(a => a.Publicacion.Titulo.ToStr().ToUpper().Contains(FindText2.ToUpper())).ToList().HasRows() ||
                                x.Cliente.Nombre.ToStr().ToUpper().Contains(FindText2.ToUpper()) ||
                                x.Cliente.Usuario.ToStr().ToUpper().Contains(FindText2.ToUpper())
                                ).ToList().ForEach(x => Buscador2.Add(x));
                        if (!Buscador2.HasRows())
                            NoResult2 = true;
                        Searching2 = true;
                        Loading2 = false;
                    }
                    else
                    {
                        Searching2 = false;
                        Buscador2.Clear();
                    }
                }
            }
            #endregion

            #region EDICION
            public ICommand IEnviarExpedicion => new ComandoUnParametro(x =>
            {
                if (!(x is Orden))
                    return;
                var Orden = x as Orden;
                if (Orden.Items.HasRows())
                {
                    /*
                    var m = new DW_ConfirmarVenta(new OrdenWeb.Edit(Orden));
                    if (m.ShowDialog() == true)
                    {
                        Pendientes.Remove(Orden);
                        Procesadas.Add(Orden);
                        OnPropertyChanged();
                    }
                    */
                }
            });


            public ICommand IGenerarNotaDeVenta => new ComandoUnParametro(x =>
            {
                if (!(x is OrdenWeb))
                    return;
                var Orden = x as OrdenWeb;

                var lit = new PdfViewer();
                var listado = Orden.NotaDeVenta();
                lit.OpenFile(listado.InformacionDelDocumento.RutaCompleta);
                App.TabController.Create(listado.InformacionDelDocumento.Titulo, lit);

            });

            public CuentaML Cuenta { get; }
            #endregion

            public UIElement GetUI()
            {
                var Container = UIMain.Content as Grid;
                UIMain.Content = null;
                Container.DataContext = this;
                return Container;
            }
        }
    }
}
