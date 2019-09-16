using Extenciones;
using Extenciones.Clases.Abstractas;
using Extenciones.Clases.Modelos;
using Extenciones.Controladores;
using Libreria;
using Notifications.Wpf;
using Sistema.Base.Controles.Ventanas;
using Sistema.Base.Programa.IniciarSesion;
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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Sistema
{
    public partial class App : Application
    {
        public static VentanaPrincipal VentanaPrincipal { get; private set; }
        public static ConfiguracionDelSistema Configuracion { get; set; } = new ConfiguracionDelSistema(Assembly.GetExecutingAssembly());
        public static Impresora DefaultPrinter { get; set; }
        
        public static MailController MailController { get; } = new MailController();
        public static LogController LogController { get; } = new LogController();
        public static LoginController LoginController { get; } = new LoginController();
        
        public static GroupsController GroupsController { get; } = new GroupsController();
        public static ExceptionManager ExceptionManager { get; } = new ExceptionManager();

        public static readonly Databases Servers = new Databases();

        public static InterfazPrincipal InterfazPrincipal { get; } = new InterfazPrincipal();

        public static DispatcherTimer Timer { get; } = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        public static ControladorDeSesiones Sesion { get; } = new ControladorDeSesiones();

        public static ControladorDeMensajes Modales { get; private set; } 
        public static ControladorDeNotificaciones Notificaciones { get; } = new ControladorDeNotificaciones();
        public static ControladorDePestañas Pestañas { get; private set; }
        public static ControladorDeTeclas Teclas { get; private set; }

        public static Usuario Usuario { get; private set; }
        internal static LoginWindow LoginWindow { get; private set; }

        public void Init()
        {
            LibConfig.CurrentApp = this;
            Configuracion.NombreDelSistema = "Gestion";
            Configuracion.Empresa = "Empresa de Ejemplo";
            Configuracion.DataMode = ConfiguracionDelSistema.DataModes.DBF;

            InitBase();

            Configuracion.Colores.Principal = Misc.ConvertFromString("#103F91");
            Configuracion.Colores.PrincipalFocus = Misc.ConvertFromString("#185ABD");
            Configuracion.Colores.PrincipalPressed = Misc.ConvertFromString("#2B7CD3");

            Configuracion.Colores.Acento = Misc.ConvertFromString("#103F91");
            Configuracion.Colores.AcentoFocus = Misc.ConvertFromString("#185ABD");
            Configuracion.Colores.AcentoPressed = Misc.ConvertFromString("#2B7CD3");

            Configuracion.Colores.Activo = Misc.ConvertFromString("#103F91");
            Configuracion.Colores.Inactivo = Misc.ConvertFromString("#185ABD");

            Configuracion.Colores.Seleccionado = Misc.ConvertFromString("#103F91");
            Configuracion.Colores.NoSeleccionado = Misc.ConvertFromString("#185ABD");

            InterfazMySQL.ServidorPredeterminado = JarseConfig.ServidoresMysql.Principal;
        }

        public static void InitBase()
        {
            Configuracion.Carpetas = new Extenciones.Configuraciones.ConfiguracionDeCarpetas(Configuracion.NombreDelSistema, Extenciones.Configuraciones.ConfiguracionDeCarpetas.Configuracion.ProgramData);

            Application.Current.Resources["Version"] = Configuracion.Version.Codigo;
            Application.Current.Resources["Compilacion"] = Configuracion.Version.Compilacion;
            Application.Current.Resources["SistemaOperativo"] = Configuracion.Version.SistemaOperativo;

            Application.Current.Resources["Licencia"] = DateTime.Today.Month.ToMesMinuscula() + " " + DateTime.Today.Year;
            Application.Current.Resources["ProximoVencimiento"] = DateTime.Today.AddMonths(1).Month.ToMesMinuscula() + " " + DateTime.Today.AddMonths(1).Year;

            Application.Current.Resources["TituloDelSistema"] = Configuracion.NombreDelSistema;
            Application.Current.Resources["Empresa"] = Configuracion.Empresa;

            Configuracion.DebugMode = ConfiguracionDelSistema.DebugModes.File;

            if (!Misc.CheckMiMaquina())
                JarseConfig.Carpetas.ImagenesArticulos = @"F:\APLI\JARSE\FOTOS\";

            LibConfig.PrincipalAssembly = Assembly.GetExecutingAssembly();
            LibConfig.Configuracion = App.Configuracion;
            LibConfig.Servers = Servers;
            LibConfig.LogController = LogController;
            LibConfig.MailController = App.MailController;
            LibConfig.ExceptionManager = ExceptionManager;
            //LibConfig.MessageController = App.MessageController;
            LibConfig.DefaultPrinter = App.DefaultPrinter;

            LoginWindow = new LoginWindow();
            Timer.Start(); Timer.Tick += (s, e) => { App.InterfazPrincipal.Refresh(); };
            LoginController.OnLogin += s =>
            {
                App.Usuario = s;
                LibConfig.Usuario = s;
                Application.Current.Resources["Usuario"] = App.Usuario;
                LoginWindow.Close();
            };

            LoginController.OnLogout += () =>
            {
                App.Usuario = null;
                LibConfig.Usuario = null;
                Application.Current.Resources["Usuario"] = null;
                LoginWindow = new LoginWindow();
                LoginWindow.Show();
            };

        }

        public static class FocusHelper
        {
            public static void Focus(UIElement element)
            {
                bool c = false;
                try
                {
                    if (element.IsNotNull() && !element.IsFocused)
                    {
                        c = element.Focus();
                    }
                    else
                    {
                        c = true;
                    }
                }
                catch (System.Exception ex) { }
                if (!c)
                {
                    try
                    {
                        element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate () { element.Focus(); }));

                    }
                    catch (System.Exception ex) { }
                }
            }
        }
    }

    public class InterfazPrincipal : Notify
    {
        public string Hora => DateTime.Now.ToLongTimeString();
        public string Fecha => DateTime.Now.ToLongDateString();

        public ICommand IAcercaDe => new Comando(() => new AcercaDeWindow().Show());

        public void Refresh() => OnPropertyChanged();



    }
}
