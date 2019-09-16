using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Factory;
using Sistema.Base.Listados.PDF;
using Sistema.Presentacion.Chat;
using Sistema.Presentacion.Secciones.Preguntas;
using Sistema.Presentacion.Secciones.Publicaciones;
using Sistema.Presentacion.Secciones.Publicaciones.Controles;
using Sistema.Presentacion.Secciones.Ventas;
using Sistema.Presentacion.Secciones.Ventas.Controles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        public partial class MAINPublicaciones_Controlador : Notify, IControladorUI
        {
            public MAINPublicaciones_Controlador()
            {
                UIMain = new UC_Publicaciones();
                UIMain.DataContext = this;

                //Factory.MercadoLibre.Servicios.DescargarOrdenes(CuentaML.JarseIndustrial);
                //Factory.MercadoLibre.Servicios.DescargarPublicaciones(CuentaML.JarseIndustrial);
                //var e = new Publicacion("MLA791376142");
                //e.ModificarStock(10).ToMessage();

                //var p = new Publicacion("MLA792110826", CuentaML.DRLINEABLANCA);
                //Factory.MercadoLibre.Publicaciones.Robarazona33();

                Find();
                PropertyChanged += (a, b) => { if (b.PropertyName == "FindText") Find(); };

            }

            void Find()
            {
                new WorkerHelper(
                (s, e) =>
                {
                    try
                    {
                        if (FindText.IsNotEmpty())
                        {
                            if (FindText.Length > 3)
                                e.Result = Factory.MercadoLibre.Publicaciones.BuscarPaginado(ItemsPorPagina.Value.ToInt(), FindText, CuentaML.JarseIndustrial, CuentaML.DRLINEABLANCA);
                            else
                                e.Result = null;
                        }
                        else
                        {
                            e.Result = Factory.MercadoLibre.Publicaciones.BuscarPaginado(ItemsPorPagina.Value.ToInt(), CuentaML.JarseIndustrial, CuentaML.DRLINEABLANCA);
                        }
                    }
                    catch (Exception ex)
                    {
                        e.Result = ex;
                    }
                },
                (s, e) =>
                {
                    if (e.Result is Exception)
                    {
                        App.MessageController.Error(e.Result.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
                        Pagina = 0;
                    }
                    else if (e.Result is List<DataTable>)
                    {
                        TotalItems = e.Result as List<DataTable>;
                        if (TotalItems.HasRows())
                            Pagina = 1;
                        else
                            Pagina = 0;
                        OnPropertyChanged();
                    }

                }).Run();
            }

            internal UC_Publicaciones UIMain { get; }
            public List<DataTable> TotalItems { get; set; } = new List<DataTable>();
            public OCPublicacion Items { get; set; } = new OCPublicacion();

            #region BUSQUEDA
            string findText = "";
            public string FindText { get => findText; set { findText = value.ToUpper(); OnPropertyChanged("FindText"); } }
            #endregion

            #region SELECCION
            public bool SeleccionarTodos {
                get => seleccionarTodos; set {
                    seleccionarTodos = value;
                    if (seleccionarTodos)
                    {
                        CargandoSeleccion = true;
                        OnPropertyChanged();
                        new WorkerHelper(
                            (s, e) =>
                            {
                                try
                                { 
                                    var result = new OCPublicacion();
                                    foreach (DataTable data in TotalItems)
                                    {
                                        foreach (DataRow item in data.Rows)
                                        {
                                            if (!IdSeleccionados.Contains(item["id"].ToStr()))
                                            {
                                                result.Add(new Publicacion(item));
                                            }
                                        }
                                    }
                                    e.Result = result;
                                }
                                catch (Exception ex)
                                {
                                    e.Result = ex;
                                }
                            },
                            (s, e) =>
                            {
                                if (e.Result is OCPublicacion && SeleccionarTodos)
                                {
                                    foreach (var item in e.Result as OCPublicacion)
                                    {
                                        Seleccionados.Add(item);
                                    }
                                    foreach (UCPublicacion child in UIMain.tStack.Children)
                                        child.Selected = IdSeleccionados.Contains(child.Model.Id);
                                    CargandoSeleccion = false;
                                    OnPropertyChanged();
                                }
                            }
                            ).Run();
                    }
                    else
                    {
                        Seleccionados.Clear();
                        foreach (UCPublicacion child in UIMain.tStack.Children)
                            child.Selected = IdSeleccionados.Contains(child.Model.Id);
                        OnPropertyChanged();
                    }
                }
            }
            public List<string> IdSeleccionados => Seleccionados.Select(x => x.Id).ToList();
            public OCPublicacion Seleccionados { get; } = new OCPublicacion();
            public int TotalSeleccionados => Seleccionados.Count;
            public bool HaySeleccionados => TotalSeleccionados > 0 || CargandoSeleccion;
            public bool CargandoSeleccion { get; set; }
            public bool NoCargandoSeleccion => !CargandoSeleccion;

            public ICommand IUnSelect => new ComandoUnParametro(x =>
            {
                if (x is Publicacion)
                {
                    var publicacion = x as Publicacion;
                    if (IdSeleccionados.Contains(publicacion.Id))
                    {
                        var I = Seleccionados.Where(a => a.Id == publicacion.Id).FirstOrDefault();
                        if (I.IsNotNull())
                        {
                            Seleccionados.Remove(I);
                            foreach (UCPublicacion child in UIMain.tStack.Children)
                                child.Selected = IdSeleccionados.Contains(child.Model.Id);
                            OnPropertyChanged();
                        }
                    }
                }
            });
            #endregion

            #region PAGINACION
            public int Total => TotalItems.Sum(x => x.Rows.Count).ToInt();
            public ComboBoxModelItem ItemsPorPagina {
                get => itemsPorPagina;
                set {
                    if (value.IsNotNull())
                    {
                        itemsPorPagina = value;
                        Find();
                    }
                }
            }
            public ComboBoxModel ComboItemsPorPagina { get; } = new ComboBoxModel() {
                new ComboBoxModelItem() { Label = "5 por página", Value = 5 },
                new ComboBoxModelItem() { Label = "10 por página", Value = 10 },
                new ComboBoxModelItem() { Label = "50 por página", Value = 50 },
                new ComboBoxModelItem() { Label = "100 por página", Value = 100 },
            };
            private int pagina = 0;
            private ComboBoxModelItem itemsPorPagina = new ComboBoxModelItem() { Label = "5 por página", Value = 5 };
            private bool seleccionarTodos;
            public int Pagina {
                get => pagina; set {
                    if (value > 0 && value <= TotalPaginas)
                    {
                        new WorkerHelper(
                        (s, e) =>
                        {
                            try
                            {
                                e.Result = new OCPublicacion(TotalItems[value - 1]);
                            }
                            catch (Exception ex)
                            {
                                e.Result = ex;
                            }
                        },
                        (s, e) =>
                        {
                            if (e.Result is Exception)
                            {
                                App.MessageController.Error(e.Result.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
                                Items = new OCPublicacion();
                            }
                            else
                            {
                                pagina = value;
                                Items = e.Result as OCPublicacion;
                                UIMain.tStack.Children.Clear();
                                foreach (var item in Items)
                                {
                                    var uc = new UCPublicacion(item, IdSeleccionados.Contains(item.Id));
                                    uc.OnSelect += x =>
                                    {
                                        if (!IdSeleccionados.Contains(item.Id))
                                            Seleccionados.Add(item);
                                        OnPropertyChanged();
                                    };
                                    uc.OnUnSelect += x =>
                                    {
                                        if (IdSeleccionados.Contains(item.Id))
                                        {
                                            var I = Seleccionados.Where(a => a.Id == item.Id).FirstOrDefault();
                                            if (I.IsNotNull()) Seleccionados.Remove(I);
                                            OnPropertyChanged();
                                        }
                                    };
                                    UIMain.tStack.Children.Add(uc);
                                }
                            }
                            OnPropertyChanged();
                        }).Run();
                    }
                    else if (value == 0)
                    {
                        pagina = 0;
                        TotalItems = new List<DataTable>();
                        Items = new OCPublicacion();
                        UIMain.tStack.Children.Clear();
                    }
                }
            }
            public int TotalPaginas => TotalItems.Count;
            public ICommand IPaginaAnterior => new Comando(() => Pagina--, () => Pagina > 1);
            public ICommand IPaginaSiguiente => new Comando(() => Pagina++, () => Pagina < TotalPaginas);
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
