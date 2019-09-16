using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Factory;
using Sistema.Presentacion.Secciones.AreaShop;
using Sistema.Presentacion.Secciones.AreaShop.Ventas.Controles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Extenciones.Conexiones.API.ApiMercadoLibre;
using static Libreria.Clases.Objetos.Zona33;

namespace Sistema.Controladores
{

    public static partial class ControladoresDeSeccion
    {
        public partial class MAINAreaShop_Controlador : Notify, IControladorUI
        {
            private States state;

            public MAINAreaShop_Controlador()
            {
                UIMain = new UC_Ventas_AreaShop();
                UIMain.DataContext = this;

                var backPedidos = new BackgroundWorker();
                backPedidos.DoWork += (s, e) =>
                {
                    e.Result = Factory.Zona33.PreConsignacionActual();

                };
                backPedidos.RunWorkerCompleted += (s, e) =>
                {
                    PreConsignacion = e.Result as PreConsignacionMercadoLibre;
                    var backOrdenes = new BackgroundWorker();
                    backOrdenes.DoWork += (a, b) =>
                    {
                        b.Result = Factory.MercadoLibre.Ordenes.Recientes(CuentaML.HogarRespuestos);
                    };
                    backOrdenes.RunWorkerCompleted += (a, b) =>
                    {
                        Ordenes = b.Result as OCOrder;
                        foreach (var item in PreConsignacion.Items)
                        {
                            var Ord = Ordenes.Where(x => x.Id == item.OrdenAsociada).FirstOrDefault();
                            if (Ord.IsNotNull())
                            {
                                Omitidos.Add(Ord);
                                Ordenes.Remove(Ord);
                            }
                        }
                        LoadingPedidos = false;
                        LoadingOrdenes = false;
                        OnPropertyChanged();
                    };
                    backOrdenes.RunWorkerAsync();
                };
                backPedidos.RunWorkerAsync();
            }

            internal UC_Ventas_AreaShop UIMain { get; }
            public OCOrder Ordenes { get; set; }
            public OCOrder Omitidos { get; } = new OCOrder();
            public PreConsignacionMercadoLibre PreConsignacion { get; set; }
            public enum States { }
            public States State { get => state; set { state = value; OnPropertyChanged(); } }
            public bool LoadingOrdenes { get; set; } = true;
            public bool LoadingPedidos { get; set; } = true;
            public bool NotLoadingPedidos => !LoadingPedidos;
            public bool NotLoadingOrdenes => !LoadingOrdenes;

            #region EDICION
            public ICommand IEliminarTodo => new Comando(() =>
            {
                if (App.MessageController.YesNo("Se eliminarán todos los items.", "¿Eliminar pedido actual?"))
                {
                    if (PreConsignacion.Delete())
                    {
                        foreach (var item in Omitidos)
                        {
                            Ordenes.Add(item);
                        }
                        Omitidos.Clear();
                        OnPropertyChanged();
                    }
                }
            });

            public ICommand IConfirmar => new ComandoUnParametro(x =>
            {
                if (!(x is Order))
                    return;
                var Orden = x as Order;
                /*
                if (Orden.OrderItems.HasRows())
                {
                    var Articulo = new Articulo(Orden.OrderItems.First().View.Codigo);
                    if (Articulo.Existe)
                    {
                        var Item = new PreConsignacionMercadoLibre.Item(Articulo);
                        Item.Cantidad = Orden.OrderItems.First().Quantity;
                        Item.OrdenAsociada = Orden.Id;
                        var mm = new DW_ConfimarItem(Item);
                        if (mm.ShowDialog() == true)
                        {
                            PreConsignacion.Items.Add(Item);
                            PreConsignacion.Save();
                            Omitidos.Add(Orden);
                            Ordenes.Remove(Orden);
                            OnPropertyChanged();
                        }
                    }
                }
                */
            });
            public ICommand IEliminar => new ComandoUnParametro(x =>
            {
                if (!(x is PreConsignacionMercadoLibre.Item))
                    return;
                var Item = x as PreConsignacionMercadoLibre.Item;
                if (App.MessageController.YesNo("Se eliminará el codigo "+Item.Articulo.Codigo, "¿Eliminar item del pedido actual?"))
                {
                    var Ord = Omitidos.Where(s => s.Id == Item.OrdenAsociada).FirstOrDefault();
                    if (Ord.IsNotNull())
                    {
                        Ordenes.Add(Ord);
                        Omitidos.Remove(Ord);
                    }
                    Item.Delete();
                    OnPropertyChanged();

                }
            });
            public ICommand IEditar => new ComandoUnParametro(x =>
            {
                if (!(x is PreConsignacionMercadoLibre.Item))
                    return;
                var Item = x as PreConsignacionMercadoLibre.Item;
                var mm = new DW_ConfimarItem(Item);
                if (mm.ShowDialog() == true)
                {
                    Item.Save();
                    OnPropertyChanged();
                }
            });
            #endregion

            #region RESTORE

            #endregion

            #region LISTADO

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
