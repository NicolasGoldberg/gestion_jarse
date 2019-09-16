using MahApps.Metro.Controls;
using Sistema.Base.Controles.ControlesDeUsuario;
using Sistema.Base.Listados.PDF;
using System;
using System.Windows;

namespace Sistema.Base.Programa.Interfaz
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class VentanaPrincipal : MetroWindow
    {
        public VentanaPrincipal()
        {
            InitializeComponent();
        }

        private void Home(object sender, RoutedEventArgs e) => App.Pestañas.IrAlInicio();

        public void Refresh()
        {
            UserButtonContent.Content = new BotonDeUsuario();
            NotificationButtonContent.Content = new BotonDeNotificaciones();
        }
        public new void Show()
        {
            Refresh();
            base.Show();
        }
    }
}
