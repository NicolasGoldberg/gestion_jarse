using Extenciones;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Sistema.Base.Controles.Ventanas
{
    /// <summary>
    /// Lógica de interacción para MessageFormError.xaml
    /// </summary>
    public partial class MessageFormError : MetroWindow, INotifyPropertyChanged
    {
        public MessageFormError(List<string> Errores)
        {
            InitializeComponent();
            this.Errores = Errores;
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public List<string> Errores { get; }

        public ICommand IContinue => new Comando(() => this.Close());
    }
}
