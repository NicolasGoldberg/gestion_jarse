using Extenciones;
using MahApps.Metro.SimpleChildWindow;
using System.ComponentModel;
using System.Windows.Input;

namespace Sistema.Base.Programa.Mensajes.Controles
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

        public void Show(string Contenido, string Titulo)
        {
            this.DataContext = this;
            this.Titulo = Titulo;
            this.Contenido = Contenido;
            IsOpen = true;
        }

        public ICommand ICerrar => new Comando(() => IsOpen = false);
    }
}
