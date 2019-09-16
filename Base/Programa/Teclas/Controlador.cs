using Extenciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sistema.Base.Programa.Teclas
{
    public class ControladorDeTeclas
    {
        public ControladorDeTeclas(Window Ventana)
        {
            this.Ventana = Ventana;
            this.Ventana.KeyDown += (s, e) =>
            {
                if (e.Key == Key.F1) App.Pestañas.IrAlInicio();
                if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) App.Pestañas.IrAlSiguiente();
                if (e.Key == Key.F4 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) App.Pestañas.CerrarActual();



                var ea = AsignacionesDown.Where(x => x.Key == e.Key).FirstOrDefault();
                if (ea.IsNotNull() && ea.Value.IsNotNull())
                {
                    ea.Value?.Invoke();
                }
            };

            this.Ventana.KeyUp += (s, e) =>
            {
                var ea = AsignacionesUp.Where(x => x.Key == e.Key).FirstOrDefault();
                if (ea.IsNotNull() && ea.Value.IsNotNull())
                {
                    ea.Value?.Invoke();
                }
            };
        }
        internal Dictionary<Key, Action> AsignacionesUp { get; } = new Dictionary<Key, Action>();
        internal Dictionary<Key, Action> AsignacionesDown { get; } = new Dictionary<Key, Action>();

        public void AñadirUp(Key Tecla, Action Accion)
        {
            var ea = AsignacionesUp.Where(x => x.Key == Tecla).FirstOrDefault();
            if (ea.IsNotNull())
                AsignacionesUp[Tecla] = Accion;
            else
                AsignacionesUp.Add(Tecla, Accion);
        }

        public void EliminarUp(Key Tecla)
        {
            var ea = AsignacionesUp.Where(x => x.Key == Tecla).FirstOrDefault();
            if (ea.IsNotNull())
                AsignacionesUp.Remove(Tecla);
        }

        public void AñadirDown(Key Tecla, Action Accion)
        {
            var ea = AsignacionesDown.Where(x => x.Key == Tecla).FirstOrDefault();
            if (ea.IsNotNull())
                AsignacionesDown[Tecla] = Accion;
            else
                AsignacionesDown.Add(Tecla, Accion);
        }

        public void EliminarDown(Key Tecla)
        {
            var ea = AsignacionesDown.Where(x => x.Key == Tecla).FirstOrDefault();
            if (ea.IsNotNull())
                AsignacionesDown.Remove(Tecla);
        }

        public Window Ventana { get; }
    }
}
