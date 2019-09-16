using Extenciones;
using Extenciones.Clases.Abstractas;
using Libreria.Objetos;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sistema.Presentacion.Secciones.Articulos.Principal.Subsecciones.Buscar
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
            TituloGeneral = "Buscar artículo";
            UIMain = new UCControl();
            UIMain.DataContext = this;

            UIMain.Selector.StatusChanged += (s) =>
            {
                switch (s)
                {
                    case Selectores.SelectorArticulos.Estados.EnEspera:
                        Estado = Estados.EnEspera;
                        break;
                    case Selectores.SelectorArticulos.Estados.Cargando:
                        Estado = Estados.Cargando;
                        break;
                    case Selectores.SelectorArticulos.Estados.Error:
                        Estado = Estados.Error;
                        break;
                    case Selectores.SelectorArticulos.Estados.Vacio:
                        Estado = Estados.Vacio;
                        break;
                    default:
                        break;
                }
            };
        }

        internal UCControl UIMain { get; }

        public ICommand ISelect => new ComandoUnParametro(x => UIMain.Selector.ISelect.Execute(x));
        public Articulo.Coleccion Recientes => UIMain.Selector.Recientes;
        public Articulo.Coleccion Resultados => UIMain.Selector.Resultados;
        public bool HayResultados => Resultados.HasRows();
        public bool NoHayResultados => UIMain.Selector.NoHayResultados;
        public bool MuestraRecientes => Recientes.HasRows();

        #region ESTADOS
        public enum Estados { EnEspera, Cargando, Error, Vacio }
        private Estados estado = Estados.EnEspera;

        public Estados Estado { get => estado; set { estado = value; OnPropertyChanged(); StatusChanged?.Invoke(value); } }
        public bool Cargando => Estado == Estados.Cargando;
        public bool Inicio => EnEspera && !HayResultados;
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


