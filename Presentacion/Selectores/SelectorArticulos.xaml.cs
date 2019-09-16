using Extenciones;
using Libreria.Objetos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sistema.Presentacion.Selectores
{
    /// <summary>
    /// Lógica de interacción para SelectorArticuloss .xaml
    /// </summary>
    public partial class SelectorArticulos : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public SelectorArticulos()
        {
            InitializeComponent();
            this.DataContext = this;
            PropertyChanged += Find;
            OnPropertyChanged();
            cFinder.OnFocus += () => CheckPopup();
            cFinder.UnSelect += () => { Selected = null; cFinder.SetFocus(); };

            this.OnSelect += (e) =>
            {
                if (IsSelected)
                    if (Recientes.Where(x => x.Codigo == e.Codigo).Select(x => x.Codigo).Count() == 0)
                    {
                        Recientes.Insert(0, e);
                        if (Recientes.Count > 5)
                            Recientes.RemoveAt(Recientes.Count - 1);
                        OnPropertyChanged("Recientes");
                    }
            };
            IsEnabledChanged += (s, e) =>
            {
                cFinder.ReadOnly = !IsEnabled;
            };
            cFinder.OnFechaAbajo += () =>
            {
                CheckPopup();
                if (PopupOpen)
                {
                    if (HayResultados)
                    {
                        gridResultados.SelectedItem = gridResultados.Items[0];
                        gridResultados.ScrollIntoView(gridResultados.Items[0]);
                        DataGridRow dgrow = (DataGridRow)gridResultados.ItemContainerGenerator.ContainerFromItem(gridResultados.Items[0]);
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                    else if (MuestraRecientes)
                    {
                        gridRecientes.SelectedItem = gridRecientes.Items[0];
                        gridRecientes.ScrollIntoView(gridRecientes.Items[0]);
                        DataGridRow dgrow = (DataGridRow)gridRecientes.ItemContainerGenerator.ContainerFromItem(gridRecientes.Items[0]);
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            };

            gridResultados.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    if (gridResultados.SelectedIndex == 0)
                    {
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    if (gridResultados.SelectedIndex == gridResultados.Items.Count - 1 && MuestraRecientes)
                    {
                        e.Handled = true;
                    }
                }
            };

            gridRecientes.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    if (gridRecientes.SelectedIndex == 0)
                    {
                        e.Handled = true;
                    }
                }

            };

            gridResultados.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (gridResultados.SelectedItem is Articulo)
                    {
                        Selected = gridResultados.SelectedItem as Articulo;
                    }
                }
                else if (e.Key == Key.Escape)
                {
                    e.Handled = true;
                    cFinder.SetFocus();
                }
                else if (e.Key == Key.Up)
                {
                    e.Handled = true;
                    if (gridResultados.SelectedIndex == 0)
                    {
                        cFinder.SetFocus();
                    }
                }
                else if (e.Key == Key.Down)
                {
                    if (gridResultados.SelectedIndex == gridResultados.Items.Count - 1 && MuestraRecientes)
                    {
                        e.Handled = true;
                        gridRecientes.SelectedItem = gridRecientes.Items[0];
                        gridRecientes.ScrollIntoView(gridRecientes.Items[0]);
                        DataGridRow dgrow = (DataGridRow)gridRecientes.ItemContainerGenerator.ContainerFromItem(gridRecientes.Items[0]);
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            };

            gridRecientes.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (gridRecientes.SelectedItem is Articulo)
                    {
                        Selected = gridRecientes.SelectedItem as Articulo;
                    }
                }
                else if (e.Key == Key.Escape)
                {
                    e.Handled = true;
                    cFinder.SetFocus();
                }
                else if (e.Key == Key.Up)
                {
                    if (gridRecientes.SelectedIndex == 0 && HayResultados)
                    {
                        e.Handled = true;
                        gridResultados.SelectedItem = gridResultados.Items[gridResultados.Items.Count - 1];
                        gridResultados.ScrollIntoView(gridResultados.Items[gridResultados.Items.Count - 1]);
                        DataGridRow dgrow = (DataGridRow)gridResultados.ItemContainerGenerator.ContainerFromItem(gridResultados.Items[gridResultados.Items.Count - 1]);
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                    else if (gridRecientes.SelectedIndex == 0 && !HayResultados)
                    {
                        e.Handled = true;
                        cFinder.SetFocus();
                    }
                }
            };
        }

        #region PROPS



        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SelectorArticulos), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(SelectorArticulos), new PropertyMetadata("Ingrese búsqueda", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MostrarRecientesProperty = DependencyProperty.Register("MostrarRecientes", typeof(bool), typeof(SelectorArticulos), new PropertyMetadata(true, MostrarRecientesPropertyChanged));
        public bool MostrarRecientes { get => (bool)GetValue(MostrarRecientesProperty); set { SetValue(MostrarRecientesProperty, value); } }
        private static void MostrarRecientesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).MostrarRecientesPropertyChanged((bool)e.NewValue);
        private void MostrarRecientesPropertyChanged(bool MostrarRecientes) { OnPropertyChanged("MuestraRecientes"); }

        public static readonly DependencyProperty MostrarResultadosProperty = DependencyProperty.Register("MostrarResultados", typeof(bool), typeof(SelectorArticulos), new PropertyMetadata(true, MostrarResultadosPropertyChanged));
        public bool MostrarResultados { get => (bool)GetValue(MostrarResultadosProperty); set { SetValue(MostrarResultadosProperty, value); } }
        private static void MostrarResultadosPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).MostrarResultadosPropertyChanged((bool)e.NewValue);
        private void MostrarResultadosPropertyChanged(bool MostrarResultados) { OnPropertyChanged("MuestraResultados"); }

        public static readonly DependencyProperty PermiteSeleccionarProperty = DependencyProperty.Register("PermiteSeleccionar", typeof(bool), typeof(SelectorArticulos), new PropertyMetadata(true, PermiteSeleccionarPropertyChanged));
        public bool PermiteSeleccionar { get => (bool)GetValue(PermiteSeleccionarProperty); set { SetValue(PermiteSeleccionarProperty, value); } }
        private static void PermiteSeleccionarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).PermiteSeleccionarPropertyChanged((bool)e.NewValue);
        private void PermiteSeleccionarPropertyChanged(bool PermiteSeleccionar) { OnPropertyChanged("MuestraSeleccion"); }

        public static readonly DependencyProperty BloqueadoProperty = DependencyProperty.Register("Bloqueado", typeof(bool), typeof(SelectorArticulos), new PropertyMetadata(false, BloqueadoPropertyChanged));
        public bool Bloqueado { get => (bool)GetValue(BloqueadoProperty); set { SetValue(BloqueadoProperty, value); } }
        private static void BloqueadoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SelectorArticulos)d).BloqueadoPropertyChanged((bool)e.NewValue);
        private void BloqueadoPropertyChanged(bool Bloqueado) { cFinder.ReadOnly = Bloqueado; }
        #endregion

        #region DATOS
        public Libreria.Procedimientos.Busquedas.Articulos.Busqueda Busqueda = new Libreria.Procedimientos.Busquedas.Articulos.Busqueda();
        public Articulo Selected {
            get => selected;
            set {
                FindText = "";
                selected = value;
                if (IsSelected)
                {
                    PopupOpen = false;
                    if (PermiteSeleccionar)
                    {
                        cFinder.ValueSelected = selected.Descripcion;
                        cFinder.Seleccionado = true;
                    }
                }
                else
                {
                    if (PermiteSeleccionar)
                    {
                        cFinder.ValueSelected = "";
                        cFinder.Seleccionado = false;
                    }
                    CheckPopup();
                }
                OnPropertyChanged();
                OnSelect?.Invoke(selected);
            }
        }
        public Articulo.Coleccion Recientes => App.BusquedasRecientes.Articulos;
        public Articulo.Coleccion Resultados { get; set; } = new Articulo.Coleccion();
        #endregion

        #region ESTADOS
        void CheckPopup()
        {
            if (!cFinder.IsControlFocused || !IsEnabled || Bloqueado)
                PopupOpen = false;
            else if (!MostrarResultados && !MostrarRecientes)
                PopupOpen = false;
            else if (IsSelected)
                PopupOpen = false;
            else
            {
                if (HayResultados || NoHayResultados || MuestraRecientes)
                    PopupOpen = true;
                else
                    PopupOpen = false;
            }
        }
        public bool IsSelected => Selected.IsNotNull() && Selected.Existe;
        public bool HayResultados => Resultados.HasRows();
        public bool NoHayResultados => Vacio && !HayResultados && FindText.IsNotEmpty() && (FindText.Length > 3 || FindText.ToInt() > 0);
        public bool MuestraResultados => MostrarResultados && HayResultados;
        public bool MuestraRecientes => MostrarRecientes && Recientes.HasRows();
        public bool MuestraSeleccion => PermiteSeleccionar && Selected.IsNotNull();

        public bool popupOpen = false;
        public bool PopupOpen { get => popupOpen; set { popupOpen = value; OnPropertyChanged("PopupOpen"); } }

        public enum Estados { EnEspera, Cargando, Error, Vacio }
        private Estados estado = Estados.EnEspera;
        public Estados Estado {
            get => estado; set {
                estado = value;
                cFinder.Cargando = Cargando;
                OnPropertyChanged();
                StatusChanged?.Invoke(value);
            }
        }
        public bool Cargando => Estado == Estados.Cargando;
        public bool EnEspera => Estado == Estados.EnEspera;
        public bool Error => Estado == Estados.Error;
        public bool Vacio => Estado == Estados.Vacio;
        #endregion

        #region BUSCADOR 
        private string findText = "";
        private Articulo selected;

        public string FindText { get => findText; set { findText = value.ToUpper(); Busqueda.Filtros.TextoABuscar = findText; OnPropertyChanged("FindText"); } }
        private void Find(object sender, PropertyChangedEventArgs b)
        {
            if (b.PropertyName == "FindText")
            {
                if (FindText.CheckBusqueda())
                {
                    Estado = Estados.Cargando;
                    Resultados.Clear();
                    new WorkerHelper(
                           (s, e) =>
                           {
                               Busqueda.Buscar();
                           },
                           (s, e) =>
                           {
                               Resultados = Busqueda.Items;
                               if (Resultados.HasRows())
                                   Estado = Estados.EnEspera;
                               else
                                   Estado = Estados.Vacio;
                               CheckPopup();
                           }
                           ).Run();
                }
                else
                {
                    Estado = Estados.EnEspera;
                    Resultados.Clear();
                    CheckPopup();
                }
            }
        }
        #endregion


        public delegate void StatusChangedDelegate(Estados Estado);
        public event StatusChangedDelegate StatusChanged;

        public ICommand ISelect => new ComandoUnParametro(x => Selected = (x as Articulo));


        public delegate void OnSelectDelegate(Articulo Selected);
        public event OnSelectDelegate OnSelect;


    }
}
