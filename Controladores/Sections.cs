using Extenciones;
using Extenciones.Clases.Abstractas;
using Extenciones.Clases.Modelos;
using Extenciones.Controladores;
using Libreria.Objetos;
using Notifications.Wpf;
using Sistema.Base.Controles.Ventanas;
using Sistema.Base.Programa.Pestañas;
using Sistema.Controladores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Sistema
{
    public static partial class Sections
    {

        public static GroupsController.SectionController PedidosProcesos => new GroupsController.SectionController() { Action = () => ControladoresDeSeccion.Expedicion.Pedidos.AddTab(), Condition = () => true };
        public static GroupsController.SectionController Articulos => new GroupsController.SectionController() { Action = () => ControladoresDeSeccion.Articulos.Principal.AddTab(), Condition = () => true };
        //public static GroupsController.SectionController Publicaciones => new GroupsController.SectionController() { Action = () => App.TabController.Create("Publicaciones", new Presentacion.Secciones.Publicaciones.UCControl.Controlador().GetUI()), Condition = () => true };

        //public static GroupsController.SectionController VentasDrLineablanca => new GroupsController.SectionController() { Action = () => App.TabController.Create("Ventas "+ CuentaML.DRLINEABLANCA.Descripcion, new MAINProcesarOrdenes_MercadoLibre_Controlador(CuentaML.DRLINEABLANCA).GetUI()), Condition = () => true};
        //public static GroupsController.SectionController VentasHogarRespuestos => new GroupsController.SectionController() { Action = () => App.TabController.Create("Ventas "+ CuentaML.HogarRespuestos.Descripcion, new MAINProcesarOrdenes_MercadoLibre_Controlador(CuentaML.HogarRespuestos).GetUI()), Condition = () => true};
        //
        //public static GroupsController.SectionController ConfiguracionGeneral => new GroupsController.SectionController() { Action = () => App.TabController.Create("Configuración", new MAINConfiguracionGeneral_Controlador().GetUI()), Condition = () => false };
        //public static GroupsController.SectionController VentasAreshop => new GroupsController.SectionController() { Action = () => App.TabController.Create("Ventas", new MAINAreaShop_Controlador().GetUI()), Condition = () => false };
        //public static GroupsController.SectionController VentasSilvina => new GroupsController.SectionController() { Action = () => App.TabController.Create("Ordenes web", new MAINWebSilvina_Controlador().GetUI()), Condition = () => false };
        //public static GroupsController.SectionController Preguntas => new GroupsController.SectionController() { Action = () => App.TabController.Create("Preguntas", new MAINPreguntas_Controlador().GetUI()), Condition = () => true };
        //public static GroupsController.SectionController Publicaciones => new GroupsController.SectionController() { Action = () => App.TabController.Create("Publicaciones", new MAINPublicaciones_Controlador().GetUI()), Condition = () => true };
    }
}

namespace Sistema.Controladores
{
    public static partial class ControladoresDeSeccion
    {
        public static class Articulos
        {
            public static class Principal
            {
                public static void AddTab()
                {
                    var a = new Sistema.Presentacion.Secciones.Articulos.Principal.Controlador();
                    App.Pestañas.Crear(a.GetUI(), a.TituloGeneral);
                }
                public static void ReplaceTab()
                {
                    var a = new Sistema.Presentacion.Secciones.Articulos.Principal.Controlador();
                    App.Pestañas.ReemplazarActual(a.GetUI(), a.TituloGeneral);
                }
            }

            public static class Gestion
            {
                public static void AddTab(Articulo Articulo)
                {
                    var a = new Sistema.Presentacion.Secciones.Articulos.GestionDeArticulos.Controlador(Articulo);
                    App.Pestañas.Crear(a.GetUI(), a.TituloGeneral);
                }
                public static void ReplaceTab(Articulo Articulo)
                {
                    var a = new Sistema.Presentacion.Secciones.Articulos.GestionDeArticulos.Controlador(Articulo);
                    App.Pestañas.ReemplazarActual(a.GetUI(), a.TituloGeneral);
                }
            }
        }

        public static class Expedicion
        {
            public class Pedidos
            {
                public Pedidos() => Controlador = new Presentacion.Secciones.Expedicion.Principal.Controlador();
                internal Sistema.Presentacion.Secciones.Expedicion.Principal.Controlador Controlador { get; }
                public UIElement GetUI() => Controlador.GetUI();
                public static void AddTab() => App.Pestañas.Crear(new Pedidos().GetUI(), "Expedición");
                public static void ReplaceTab() => App.Pestañas.ReemplazarActual(new Pedidos().GetUI(), "Expedición");

                public class Nuevo
                {
                    public Nuevo() => Controlador = new Presentacion.Secciones.Expedicion.Principal.Subsecciones.Pedidos.Nuevo.Controlador();
                    internal Presentacion.Secciones.Expedicion.Principal.Subsecciones.Pedidos.Nuevo.Controlador Controlador { get; }
                    public UIElement GetUI() => Controlador.GetUI();
                    public static void AddTab() => App.Pestañas.Crear(new Nuevo().GetUI(), "Nuevo pedido");
                    public static void ReplaceTab() => App.Pestañas.ReemplazarActual(new Nuevo().GetUI(), "Nuevo pedido");
                }

                public class Buscar
                {
                    public Buscar() => Controlador = new Presentacion.Secciones.Expedicion.Principal.Subsecciones.Pedidos.Nuevo.Controlador();
                    internal Presentacion.Secciones.Expedicion.Principal.Subsecciones.Pedidos.Nuevo.Controlador Controlador { get; }
                    public UIElement GetUI() => Controlador.GetUI();
                    public static void AddTab() => App.Pestañas.Crear(new Buscar().GetUI(), "Buscar pedido");
                    public static void ReplaceTab() => App.Pestañas.ReemplazarActual(new Buscar().GetUI(), "Buscar pedido");
                }
            }
        }
    }
}
