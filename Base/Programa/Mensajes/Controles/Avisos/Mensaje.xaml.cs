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
    /// Lógica de interacción para Mensaje.xaml
    /// </summary>
    public partial class Mensaje : UserControl, INotifyPropertyChanged
    {
        public Mensaje()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }
        public enum Iconos { Ninguno, Alerta, Error, OK }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Mensaje), new PropertyMetadata(false, IsOpenPropertyChanged));
        public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set { SetValue(IsOpenProperty, value); } }
        private static void IsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Mensaje)d).IsOpenPropertyChanged((bool)e.NewValue);
        private void IsOpenPropertyChanged(bool IsOpen) { this.Visibility = IsOpen ? Visibility.Visible : Visibility.Collapsed; }

        public static readonly DependencyProperty TituloProperty = DependencyProperty.Register("Titulo", typeof(string), typeof(Mensaje), new PropertyMetadata("", TituloPropertyChanged));
        public string Titulo { get => (string)GetValue(TituloProperty); set { SetValue(TituloProperty, value); } }
        private static void TituloPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Mensaje)d).TituloPropertyChanged((string)e.NewValue);
        private void TituloPropertyChanged(string Titulo) { }

        public static readonly DependencyProperty ContenidoProperty = DependencyProperty.Register("Contenido", typeof(string), typeof(Mensaje), new PropertyMetadata("", ContenidoPropertyChanged));
        public string Contenido { get => (string)GetValue(ContenidoProperty); set { SetValue(ContenidoProperty, value); } }
        private static void ContenidoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Mensaje)d).ContenidoPropertyChanged((string)e.NewValue);
        private void ContenidoPropertyChanged(string Contenido) { }

        public static readonly DependencyProperty IconoProperty = DependencyProperty.Register("Icono", typeof(Iconos), typeof(Mensaje), new PropertyMetadata(Iconos.Ninguno, IconoPropertyChanged));
        public Iconos Icono { get => (Iconos)GetValue(IconoProperty); set { SetValue(IconoProperty, value); } }
        private static void IconoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Mensaje)d).IconoPropertyChanged((Iconos)e.NewValue);
        private void IconoPropertyChanged(Iconos Icono) { OnPropertyChanged(); }

        public bool iAlerta => Icono == Iconos.Alerta;
        public bool iError => Icono == Iconos.Error;
        public bool iOK => Icono == Iconos.OK;
    }

}
