using Extenciones;
using Notifications.Wpf;
using Sistema.Base.Programa.Interfaz;
using System;
using System.Diagnostics;

namespace Sistema.Base.Programa.Mensajes
{
    public class ControladorDeMensajes
    {
        VentanaPrincipal Ventana { get; }

        public ControladorDeMensajes(VentanaPrincipal Ventana)
        {
            this.Ventana = Ventana;
        }

        public void Mensaje(string Mensaje = "", string Titulo = "", Action OnClose = null, int DuracionEnSegundos = 0)
        {
            Ventana.UIMensaje.Show(Mensaje, Titulo);
        }

        public void Alerta(string Mensaje = "", string Titulo = "", Action OnClose = null, int DuracionEnSegundos = 0)
        {
            Ventana.UIAlerta.Show(Mensaje, Titulo);
        }

        public void Error(string Mensaje = "", string Titulo = "", Action OnClose = null, int DuracionEnSegundos = 0)
        {
            Ventana.UIError.Show(Mensaje, Titulo);
        }

        public void Completado(string Mensaje = "", string Titulo = "", Action OnClose = null, int DuracionEnSegundos = 0)
        {
            Ventana.UICompletado.Show(Mensaje, Titulo);
        }

        public void Cargando(int DuracionEnSegundos = 0)
        {
            Ventana.UICargando.Show("","");
        }

    }
}
