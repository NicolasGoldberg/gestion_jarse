using Extenciones;
using MahApps.Metro.SimpleChildWindow;
using System.ComponentModel;
using System.Windows.Input;

namespace Sistema.Base.Programa.Mensajes.Controles
{
    /// <summary>
    /// Lógica de interacción para Cargando.xaml
    /// </summary>
    public partial class Cargando : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        
        public Cargando()
        {
            InitializeComponent();
        }


        public void Show(string Contenido, string Titulo)
        {
            this.DataContext = this;
            IsOpen = true;
        }

        public ICommand ICerrar => new Comando(() => IsOpen = false);
    }
}
