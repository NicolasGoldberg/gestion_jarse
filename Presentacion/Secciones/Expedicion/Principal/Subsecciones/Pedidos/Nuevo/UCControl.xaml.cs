using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Clases.Objetos;
using Sistema.Controladores;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sistema.Presentacion.Secciones.Expedicion.Principal.Subsecciones.Pedidos.Nuevo
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
            TituloGeneral = "Nuevo pedido";
            UIMain = new UCControl();
            UIMain.DataContext = this;
        }

        internal UCControl UIMain { get; }

        #region COMANDOS
        override public List<ICommand> IComandos => new List<ICommand>
            {
                //0
                new Comando( () =>{
                    //var c = new Sistema.Presentacion.Secciones.Recepcion.Controles.Inicio.Controlador();
                    //c.StatusChanged += x => { };
                    //UIMain.MainContent.Content = c.GetUI();
                }),
                //1
                new Comando( () =>{
                    //var c = new Sistema.Presentacion.Secciones.Recepcion.Controles.Inicio.Controlador();
                    //c.StatusChanged += x => { };
                    //UIMain.MainContent.Content = c.GetUI();
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


