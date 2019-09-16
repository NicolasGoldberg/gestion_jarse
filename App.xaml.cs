using Extenciones;
using Extenciones.Configuraciones;
using MahApps.Metro;
using Sistema.Base.Listados.PDF;
using Sistema.Base.Programa.Interfaz;
using Sistema.Base.Programa.Mensajes;
using Sistema.Base.Programa.Notificaciones;
using Sistema.Base.Programa.Pestañas;
using Sistema.Base.Programa.Teclas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sistema
{


    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Init();
            App.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Exception.ToStr().ToLog();
                //App.Modales.Error(e.Exception.ToStr(), "Intentá continuar o informá del error si este vuelve a ocurrir.");
            };

            App.VentanaPrincipal = new VentanaPrincipal();
            App.Current.MainWindow = App.VentanaPrincipal;
            App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            if (Misc.CheckMiMaquina())
            {

                //LoginController.Usuario = "nicolas.goldberg";
                //LoginController.Password = "Nico123**";
                //LoginController.ILogin.Execute(null);
            }
            else
            {
                //LoginController.Usuario = "nicolas.goldberg";
                //LoginController.Password = "Nico123**";
                //LoginController.ILogin.Execute(null);
                LoginWindow.Show();
            }

            App.Pestañas = new ControladorDePestañas(App.VentanaPrincipal.TabMain, App.VentanaPrincipal.UniformTabs, new InterfazDeInicio());
            App.Teclas = new ControladorDeTeclas(App.VentanaPrincipal);
            App.Modales = new ControladorDeMensajes(App.VentanaPrincipal);
            App.VentanaPrincipal.Show();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            // add custom accent and theme resource dictionaries to the ThemeManager
            // you should replace MahAppsMetroThemesSample with your application name
            // and correct place where your custom accent lives
            ThemeManager.AddAccent("CustomAccent", new Uri("pack://application:,,,/Base/Programa/Estilos/Colores.xaml"));
            // get the current app style (theme and accent) from the application
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            // now change app style to the custom accent and current theme
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("CustomAccent"), theme.Item1);
            base.OnStartup(e);
        }
    }
}
