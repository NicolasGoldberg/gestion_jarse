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
    public partial class MessageError : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public MessageError(object Contenido, object Titulo)
        {
            InitializeComponent();
            this.Contenido = Contenido.ToStr();
            this.Titulo = Titulo.ToStr();
            OnPropertyChanged();
            KeyUp += (s, e) => {
                if (e.Key == Key.Enter || e.Key == Key.S)
                    IContinue.Execute(null);
            };
        }

        public string Contenido { get; }
        public string Titulo { get; }

        //public ICommand IClose => new Comando(() => { this.Close(); Environment.Exit(0); });
        public ICommand IContinue => new Comando(() => { this.Close(); });
        public ICommand IDetails => new Comando(() => {
            this.Details.Visibility = Visibility.Visible;
            this.YesDetails.Visibility = Visibility.Collapsed;
            this.NoDetails.Visibility = Visibility.Visible;
            this.Height = 500;
            this.Top = this.Top - 200;
        });
        public ICommand INoDetails => new Comando(() => {
            this.Details.Visibility = Visibility.Collapsed;
            this.YesDetails.Visibility = Visibility.Visible;
            this.NoDetails.Visibility = Visibility.Collapsed;
            this.Height = 150;
            this.Top = this.Top + 200;
        });
    }
}
