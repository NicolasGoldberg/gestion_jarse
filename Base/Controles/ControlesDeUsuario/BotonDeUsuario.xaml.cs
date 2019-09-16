using Extenciones;
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

namespace Sistema.Base.Controles.ControlesDeUsuario
{
    /// <summary>
    /// Lógica de interacción para BotonDeUsuario.xaml
    /// </summary>
    public partial class BotonDeUsuario : UserControl, INotifyPropertyChanged
    {
        public BotonDeUsuario()
        {
            InitializeComponent();
        }

        public ICommand IOpen => new Comando(() => { PopupOpen = true; OnPropertyChanged(); });
        public bool PopupOpen { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
