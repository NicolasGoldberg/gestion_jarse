using Extenciones;
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

namespace Sistema.Base.Controles.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Mensaje.xaml
    /// </summary>
    public partial class Mensaje : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private string contenido;
        private string titulo;

        public Mensaje()
        {
            InitializeComponent();
        }

        public string Contenido { get => contenido; set { contenido = value; OnPropertyChanged(); } }
        public string Titulo { get => titulo; set { titulo = value; OnPropertyChanged(); } }

        public void Show(string Contenido,string Titulo)
        {
            this.Titulo = Titulo;
            this.Contenido = Contenido;
            IsOpen = true;
        }

        public ICommand ICerrar => new Comando(() => IsOpen = false);
    }
}
