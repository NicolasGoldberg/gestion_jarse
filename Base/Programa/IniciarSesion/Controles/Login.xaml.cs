using Extenciones;
using MahApps.Metro.SimpleChildWindow;
using System.ComponentModel;
using System.Windows.Input;

namespace Sistema.Base.Programa.IniciarSesion.Controles
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private string titulo;

        public Login()
        {
            InitializeComponent();
        }

        public string Titulo { get => titulo; set { titulo = value; OnPropertyChanged(); } }

        public void Show(string Contenido, string Titulo)
        {
            this.DataContext = this;
            this.Titulo = Titulo;
            IsOpen = true;
        }

        public ICommand IIniciar => new Comando(() => IsOpen = false);
    }
}
