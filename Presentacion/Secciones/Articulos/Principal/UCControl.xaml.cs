using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Sistema.Controladores;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sistema.Presentacion.Secciones.Articulos.Principal
{
    /// <summary>
    /// Lógica de interacción para UCControl.xaml
    /// </summary>
    public partial class UCControl : UserControl
    {
        public UCControl()
        {
            InitializeComponent();
        }
    }

    public class Controlador : ControladorDeSeccion
    {
        public Controlador()
        {
            TituloGeneral = "Artículos";
            UIMain = new UCControl();
            UIMain.DataContext = this;
        }

        internal UCControl UIMain { get; }

        #region COMANDOS
        override public List<ICommand> IComandos => new List<ICommand>
            {
                //0
                new Comando( () =>{
                    //var c = new Sistema.Presentacion.Secciones.Articulos.GestionDeArticulos.Controlador(new Libreria.Objetos.Articulo("22715/7"));
                    //var c = new Sistema.Presentacion.Secciones.Articulos.Principal.Subsecciones.Buscar.Controlador();
                    //UIMain.MainContent.Content = c.GetUI();
                    ControladoresDeSeccion.Articulos.Gestion.AddTab(new Libreria.Objetos.Articulo("227157"));
                }),
                //1
                new Comando( () =>{
                     new WorkerHelper(
                           (s, e) =>
                           {
                                try
                                {
                                    //var sin1 = new Libreria.Procedimientos.Sincronizaciones.Lineas(); sin1.AgregarTodo(); sin1.Sincronizar();
                                    //var sin2 = new Libreria.Procedimientos.Sincronizaciones.Sectores(); sin2.AgregarTodo(); sin2.Sincronizar();
                                    //var sin3 = new Libreria.Procedimientos.Sincronizaciones.Marcas(); sin3.AgregarTodo(); sin3.Sincronizar();
                                    //var sin4 = new Libreria.Procedimientos.Sincronizaciones.Titulos(); sin4.AgregarTodo(); sin4.Sincronizar();
                                    //var sin5 = new Libreria.Procedimientos.Sincronizaciones.Unidades(); sin5.AgregarTodo(); sin5.Sincronizar();
                                    //var sin6 = new Libreria.Procedimientos.Sincronizaciones.Articulos(); sin6.AgregarTodo(); sin6.Sincronizar();
                                }
                                catch (System.Exception ex )
                                {
                                    ex.ToLog();
                                }

                           },
                           (s, e) =>
                           {
                               MessageBox.Show("OK");
                           }
                           ).Run();


                }),
                //1
                new Comando( () =>{
                    //var c = new Sistema.Presentacion.Secciones.Administracion.Principal.Feriados.Controlador();
                    //c.StatusChanged += x => { };
                    //UIMain.MainContent.Content = c.GetUI();
                }),
            };

        #endregion

        public UIElement GetUI()
        {
            if (UIMain.IsNull())
                return new UIElement();
            var Container = UIMain.Content as Grid;
            UIMain.Content = null;
            Container.DataContext = this;
            Container.Loaded += (s, e) =>
            {
                if (!Loaded)
                {
                    Loaded = true;
                    Selected = 0;
                }
            };
            return Container;
        }

    }
}


