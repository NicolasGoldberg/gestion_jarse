using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Factory;
using Sistema.Presentacion.Secciones.WebSilvina;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Extenciones.Conexiones.API.ApiMercadoLibre;

namespace Sistema.Controladores
{

    public static partial class ControladoresDeSeccion
    {
        public partial class MAINWebSilvina_Controlador : Notify, IControladorUI
        {
            private States state;

            public MAINWebSilvina_Controlador()
            {
                UIMain = new UCManejoDeOrdenes();
                UIMain.DataContext = this;

                var backPedidos = new BackgroundWorker();
                backPedidos.DoWork += (s, e) =>
                {
                    try
                    {
                        e.Result = Factory.Web.Otras.Recientes();
                    }
                    catch (Exception ex)
                    {
                        e.Result = ex;
                    }

                };
                backPedidos.RunWorkerCompleted += (s, e) =>
                {
                    if (e.Result is Exception)
                    {
                        App.MessageController.Error(e.Result.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
                        Ordenes = new OCOrdenWeb();
                    }
                    else
                        Ordenes = e.Result as OCOrdenWeb;
                    OnPropertyChanged();
                    Ordenes.Count.ToMessage();
                };
                backPedidos.RunWorkerAsync();
            }

            internal UCManejoDeOrdenes UIMain { get; }
            public OCOrdenWeb Ordenes { get; set; }
            public OCOrder Omitidos { get; } = new OCOrder();
            public enum States { }
            public States State { get => state; set { state = value; OnPropertyChanged(); } }
            public bool LoadingOrdenes { get; set; } = false;
            public bool LoadingPedidos { get; set; } = false;
            public bool NotLoadingPedidos => !LoadingPedidos;
            public bool NotLoadingOrdenes => !LoadingOrdenes;

            #region EDICION
            public ICommand ICargarNueva => new Comando(() =>
            {
                var m = new DWCargaDeOrden();
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
