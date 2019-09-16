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
    /// Lógica de interacción para Message.xaml
    /// </summary>
    public partial class Message : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        public Message(object Contenido, object Titulo)
        {
            InitializeComponent();
            this.Contenido = Contenido.ToStr();
            this.Titulo = Titulo.ToStr();
            OnPropertyChanged();
            KeyUp += (s, e) => {
                if (e.Key == Key.Enter || e.Key == Key.S)
                    IClose.Execute(null);
            };
        }

        public string Contenido { get; }
        public string Titulo { get; }

        public ICommand IClose => new Comando(() => { this.Close(); });
    }
}
