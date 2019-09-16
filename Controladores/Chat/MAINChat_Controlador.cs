using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Factory;
using Sistema.Base.Listados.PDF;
using Sistema.Presentacion.Chat;
using Sistema.Presentacion.Secciones.Ventas;
using Sistema.Presentacion.Secciones.Ventas.Controles;
using System;
using System.ComponentModel;
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

        public partial class MAINChat_Controlador : Notify, IControladorUI
        {
            public MAINChat_Controlador(OrdenWeb Orden)
            {
                UIMain = new UCChat();
                UIMain.DataContext = this;

                if (Orden.IsNotNull())
                {
                    new WorkerHelper(
                     (s, e) =>
                     {
                         try
                         {
                             e.Result = Orden.MercadoLibre.GetChat();
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
                             Chats = new Chat();
                         }
                         else
                             Chats = e.Result as Chat;

                         OnPropertyChanged();
                     }).Run();
                }

            }

            internal UCChat UIMain { get; }
            public Chat Chats { get; set; }


            #region EDICION
          
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
