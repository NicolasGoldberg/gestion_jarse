using Extenciones;
using System;
using System.Collections.Generic;
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

namespace Sistema.Base.Programa.Interfaz
{
    /// <summary>
    /// Lógica de interacción para InterfazDeInicio.xaml
    /// </summary>
    public partial class InterfazDeInicio : UserControl
    {
        public InterfazDeInicio()
        {
            InitializeComponent();
        }

        public ICommand IA => new Comando(() => App.Modales.Mensaje("pestaña nueva",""));
        public ICommand IB => new Comando(() => App.Modales.Alerta("Esta es una alerta)"));
        public ICommand IC => new Comando(() => App.Modales.Error("Esta es una alerta)"));
        public ICommand ID => new Comando(() => App.Modales.Completado("Esta es una alerta)"));
        public ICommand IE => new Comando(() => App.Modales.Cargando());
    }
}
