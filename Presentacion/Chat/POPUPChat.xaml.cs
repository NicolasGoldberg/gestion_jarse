using Extenciones;
using Libreria.Clases.Objetos.Web.Ventas;
using Libreria.Clases.Objetos.MercadoLibre;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Sistema.Controladores.ControladoresDeSeccion;

namespace Sistema.Presentacion.ChatControl
{
    /// <summary>
    /// Lógica de interacción para UCChat.xaml
    /// </summary>
    public partial class POPUPChat : Popup, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public POPUPChat()
        {
            InitializeComponent();

            this.Opened += (a, b) =>
            {
                if(DataContext is OrdenWeb)
                {
                    var Controlador = new MAINChat_Controlador(DataContext as OrdenWeb);
                    cContent.Content = Controlador.GetUI();
                }
            };
        }
    }
}
