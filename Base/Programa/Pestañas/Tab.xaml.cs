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

namespace Sistema.Base.Programa.Pestañas
{
    /// <summary>
    /// Lógica de interacción para Tab.xaml
    /// </summary>
    public partial class Tab : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public Tab()
        {

        }

        public Tab(ControladorDePestañas.Item Item)
        {
            InitializeComponent();
            this.Item = Item;
            Item.OnSeleccionado += x =>
            {
                if (x)
                {
                    Btn.Height = 30;
                    Btn.Background = Misc.ConvertFromString("#6F0000");
                }
                else
                {
                    Btn.Height = 28;
                    Btn.Background = Misc.ConvertFromString("#9F5555");
                }
                    
            };
        }

        private ControladorDePestañas.Item item;
        public ControladorDePestañas.Item Item { get => item; set { item = value; OnPropertyChanged(); } }
        public ICommand ICerrar => new Comando(() => Item.Cerrar());
        public ICommand ISeleccionar => new Comando(() => Item.Seleccionar());

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
                ICerrar.Execute(null);
        }
    }
}
