using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Objetos;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sistema.Presentacion.Secciones.Articulos.GestionDeArticulos.Principal
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
        public Controlador(Articulo Articulo)
        {
            TituloGeneral = "";
            UIMain = new UCControl();
            this.Articulo = Articulo;
            UIMain.DataContext = this;

        }

        internal UCControl UIMain { get; }
        public Articulo Articulo { get; }

        #region ESTADOS
        public enum Estados { EnEspera, Cargando, Error, Vacio }
        private Estados estado = Estados.EnEspera;
        public Estados Estado { get => estado; set { estado = value; OnPropertyChanged(); StatusChanged?.Invoke(value); } }
        public bool Cargando => Estado == Estados.Cargando;
        public bool EnEspera => Estado == Estados.EnEspera;
        public bool Error => Estado == Estados.Error;
        public bool Vacio => Estado == Estados.Vacio;
        public override List<ICommand> IComandos => new List<ICommand> { };
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
                    //Selected = 0;
                }
            };
            return Container;
        }

        public delegate void StatusChangedDelegate(Estados Estado);
        public event StatusChangedDelegate StatusChanged;
    }
}


