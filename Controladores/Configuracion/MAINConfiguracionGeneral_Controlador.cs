using Extenciones.Clases.Abstractas;
using Sistema.Presentacion.Secciones.Configuracion;
using Sistema.Presentacion.Secciones.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sistema.Controladores
{
    public static partial class ControladoresDeSeccion
    {
        public class MAINConfiguracionGeneral_Controlador : Notify, IControladorUI
        {
            private States state;

            public MAINConfiguracionGeneral_Controlador()
            {
                UIMain = new UCConfiguracionGeneral();
                UIMain.DataContext = this;
            }

            internal UCConfiguracionGeneral UIMain { get; }
            public enum States { }
            public States State { get => state; set { state = value; OnPropertyChanged(); } }
            public bool LoadingOrdenes { get; set; } = false;
            public bool LoadingPedidos { get; set; } = false;
            public bool NotLoadingPedidos => !LoadingPedidos;
            public bool NotLoadingOrdenes => !LoadingOrdenes;

            #region EDICION

            #endregion

            #region RESTORE

            #endregion

            #region LISTADO

            #endregion

            public UIElement GetUI()
            {
                var Container = UIMain.Content as Grid;
                UIMain.Content = null;
                Container.DataContext = this;
                return Container;
            }
        }
    }
}
