using Extenciones;
using Notifications.Wpf;
using System;
using System.Diagnostics;

namespace Sistema.Base.Programa.Notificaciones
{
    public static class Notificaciones
    {
        
    }

    public class ControladorDeNotificaciones
    {
        NotificationManager Manager { get; }
        public ControladorDeNotificaciones()
        {
            this.Manager = new NotificationManager();
            
        }

        public void Mensaje(string Mensaje = "", string Titulo = "", Action OnClick = null, Action OnClose = null, int DuracionEnSegundos = 5)
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Information
            }, "",
            new TimeSpan(0, 0, 0, DuracionEnSegundos),
            OnClick,
            OnClose);
        }

        public void Completado(string Mensaje , string Titulo = "Completado correctamente", Action OnClick = null, Action OnClose = null, int DuracionEnSegundos = 5)
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Success
            }, "",
            new TimeSpan(0, 0, 0, DuracionEnSegundos),
            OnClick,
            OnClose);
        }

        public void Alerta(string Mensaje, string Titulo = "Atención", Action OnClick = null, Action OnClose = null, int DuracionEnSegundos = 5)
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Warning
            }, "",
            new TimeSpan(0, 0, 0, DuracionEnSegundos),
            OnClick,
            OnClose);
        }

        public void Error(string Mensaje, string Titulo = "Ocurrió un error", Action OnClick = null, Action OnClose = null, int DuracionEnSegundos = 5)
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Error
            }, "",
            new TimeSpan(0, 0, 0, DuracionEnSegundos),
            OnClick,
            OnClose);
        }

        public void ArchivoGenerado(string RutaDelArchivo, string Mensaje = "Haga click aquí para abrirlo", string Titulo = "Archivo generado")
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Success
            }, "",
            new TimeSpan(0, 0, 0, 10),
            () => { if (RutaDelArchivo.FileExist()) Process.Start(RutaDelArchivo); });
        }

        public void ArchivoDescargado(string RutaDelArchivo, string Mensaje = "Haga click aquí para abrirlo", string Titulo = "Archivo descargado")
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Success
            }, "",
            new TimeSpan(0, 0, 0, 10),
            () => { if (RutaDelArchivo.FileExist()) Process.Start(RutaDelArchivo); });
        }

        public void ArchivoGuardado(string RutaDelArchivo, string Mensaje = "Haga click aquí para abrirlo", string Titulo = "Archivo guardado")
        {
            Manager.Show(new NotificationContent
            {
                Title = Titulo,
                Message = Mensaje,
                Type = NotificationType.Success
            }, "",
            new TimeSpan(0, 0, 0, 10),
            () => { if (RutaDelArchivo.FileExist()) Process.Start(RutaDelArchivo); });
        }

    }
}
