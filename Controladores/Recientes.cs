using Libreria.Objetos;
using System.Windows;

namespace Sistema
{
    public partial class App : Application
    {
        public static BusquedasRecientes BusquedasRecientes { get; set; }

    }

    public class BusquedasRecientes
    {
        public Articulo.Coleccion Articulos { get; } = new Articulo.Coleccion();
    }
}
