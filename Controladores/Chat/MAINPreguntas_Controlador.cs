using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Libreria.Clases.Objetos.MercadoLibre;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Factory;
using Sistema.Base.Listados.PDF;
using Sistema.Presentacion.Chat;
using Sistema.Presentacion.Secciones.Preguntas;
using Sistema.Presentacion.Secciones.Preguntas.Controles;
using Sistema.Presentacion.Secciones.Ventas;
using Sistema.Presentacion.Secciones.Ventas.Controles;
using System;
using System.ComponentModel;
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
        public partial class MAINPreguntas_Controlador : Notify, IControladorUI
        {
            public MAINPreguntas_Controlador()
            {
                UIMain = new UC_Preguntas();
                UIMain.DataContext = this;
                Set();
            }

            internal UC_Preguntas UIMain { get; }
            public OCPregunta Preguntas { get; set; } = new OCPregunta();

            #region LOADING 
            private bool loading = true;
            public bool Loading { get => loading; set { loading = value; OnPropertyChanged("Loading"); OnPropertyChanged("NotLoading"); } }
            public bool NotLoading => !Loading;
            #endregion

            void Set()
            {
                Loading = true;

                new WorkerHelper(
                (s, e) =>
                {
                    try
                    {
                        e.Result = Factory.MercadoLibre.Preguntas.NoContestadas(CuentaML.JarseIndustrial, CuentaML.DRLINEABLANCA);
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
                        Preguntas = new OCPregunta();
                    }
                    else
                        Preguntas = e.Result as OCPregunta;
                    foreach (var item in Preguntas)
                    {
                        UIMain.tStack.Children.Add(new UCPregunta(item));
                    }
                    Loading = false;
                    OnPropertyChanged();
                }).Run();
            }


            #region EDICION
            public ICommand ILimpiarRespondidas => new Comando(() => {

                
                foreach (var item in Preguntas.Where(x=> x.Respondida).ToList())
                {
                    UIMain.tStack.Children.RemoveAt(Preguntas.IndexOf(item));
                    Preguntas.Remove(item);
                }
                OnPropertyChanged();
            });
            public ICommand IRefrescar => new Comando(() => {
                Set();
            });
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
