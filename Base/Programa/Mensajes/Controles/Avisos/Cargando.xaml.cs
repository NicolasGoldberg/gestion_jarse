using MahApps.Metro.SimpleChildWindow;
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

namespace Sistema.Base.Programa.Mensajes.Controles.Avisos
{
    /// <summary>
    /// Lógica de interacción para Cargando.xaml
    /// </summary>
    public partial class Cargando : UserControl, INotifyPropertyChanged
    {
        public Cargando()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Cargando), new PropertyMetadata(false, IsOpenPropertyChanged));
        public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set { SetValue(IsOpenProperty, value); } }
        private static void IsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Cargando)d).IsOpenPropertyChanged((bool)e.NewValue);
        private void IsOpenPropertyChanged(bool IsOpen) { this.Visibility = IsOpen ? Visibility.Visible : Visibility.Collapsed; }

        public static readonly DependencyProperty ContenidoProperty = DependencyProperty.Register("Contenido", typeof(string), typeof(Cargando), new PropertyMetadata("", ContenidoPropertyChanged));
        public string Contenido { get => (string)GetValue(ContenidoProperty); set { SetValue(ContenidoProperty, value); } }
        private static void ContenidoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Cargando)d).ContenidoPropertyChanged((string)e.NewValue);
        private void ContenidoPropertyChanged(string Contenido) { }
    }

}
