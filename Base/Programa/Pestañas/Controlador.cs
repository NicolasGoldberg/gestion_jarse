using Extenciones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sistema.Base.Programa.Pestañas
{
    public class ControladorDePestañas
    {
        TabControl TabMain;
        UniformGrid UniformControl;
        public ControladorDePestañas(TabControl TabMain, UniformGrid UniformControl, UIElement Inicio)
        {
            this.TabMain = TabMain;
            this.UniformControl = UniformControl;
            if (Inicio.IsNotNull())
            {
                this.Inicio = new Item(this, Inicio, "");
                this.Inicio.RepresentarComoInicio();
                Items.Add(this.Inicio);
                IrAlInicio();
            }
        }

        public Item Inicio { get; set; }
        public Item Actual => Items.Where(x => x.TabItem.GetHashCode() == TabMain.SelectedItem.GetHashCode()).FirstOrDefault();

        public ObservableCollection<Item> Items = new ObservableCollection<Item>();
        public Item ReemplazarActual(UIElement Contenido, string Titulo, bool PermiteCerrar = true)
        {
            return Reemplazar(Actual, Contenido, Titulo, PermiteCerrar);
        }
        public Item Crear(UIElement Contenido, string Titulo, bool PermiteCerrar = true)
        {
            var I = Items.Where(x => x.Contenido.GetHashCode() == Contenido.GetHashCode()).FirstOrDefault();
            if (I.IsNotNull())
            {
                Seleccionar(I);
                return I;
            }
            else
            {
                Item item = new Item(this, Contenido, Titulo.ToStr(), PermiteCerrar);
                item.Representar();
                Items.Add(item);
                Seleccionar(item);
                return item;
            }
        }

        public Item Reemplazar(Item Item, UIElement Contenido, string Titulo, bool PermiteCerrar = true)
        {
            var I = Items.Where(x => x.GetHashCode() == Item.GetHashCode()).FirstOrDefault();
            if (I.IsNotNull())
            {
                I.Cerrar();
                Item item = new Item(this, Contenido, Titulo.ToStr(), PermiteCerrar);
                item.Representar();
                Items.Add(item);
                Seleccionar(item);
                return item;
            }
            else
            {
                return Crear(Contenido, Titulo, PermiteCerrar);
            }
        }

        public void Cerrar(Item Item)
        {
            if (Item.IsNotNull())
            {
                var idx = Items.IndexOf(Item);
                if (idx == 0)
                    return;
                TabMain.Items.Remove(Item.TabItem);
                UniformControl.Children.Remove(Item.TabButton);
                Items.Remove(Item);
                var I = Items.ElementAtOrDefault(idx);
                if (I.IsNotNull())
                {
                    I.Seleccionar();
                    return;
                }
                I = Items.ElementAtOrDefault(idx - 1);
                if (I.IsNotNull())
                {
                    I.Seleccionar();
                    return;
                }
                I = Items.ElementAtOrDefault(idx + 1);
                if (I.IsNotNull())
                {
                    I.Seleccionar();
                    return;
                }
                IrAlInicio();
            }
        }

        public void Seleccionar(Item Item)
        {
            if (Item.IsNotNull())
            {
                TabMain.SelectedItem = Item.TabItem;
                Item.EstaSeleccionado = true;
                foreach (var item in Items)
                {
                    if (item.GetHashCode() != Item.GetHashCode())
                        item.EstaSeleccionado = false;
                }
            }
        }

        public void IrAlInicio() => Seleccionar(Inicio);
        public void IrAlSiguiente()
        {
            if (Actual.IsNotNull())
            {
                var idx = Items.IndexOf(Actual);
                var I = Items.ElementAtOrDefault(idx + 1);
                if (I.IsNotNull())
                {
                    I.Seleccionar();
                    return;
                }
                else
                {
                    IrAlInicio();
                }
            }
            else
            {
                IrAlInicio();
            }

        }
        public void IrAlAnterior()
        {
            if (Actual.IsNotNull())
            {
                var idx = Items.IndexOf(Actual);
                if (idx == 1)
                {
                    IrAlInicio();
                    return;
                }
                var I = Items.ElementAtOrDefault(idx - 1);
                if (I.IsNotNull())
                {
                    I.Seleccionar();
                    return;
                }
            }
        }
        public void CerrarActual() => Cerrar(Actual);
        public class Item
        {
            private bool estaSeleccionado;

            public Item(ControladorDePestañas Controlador, UIElement Contenido, string Titulo, bool PermiteCerrar = true)
            {
                this.Controlador = Controlador;
                this.Contenido = Contenido;
                this.Titulo = Titulo;
                this.PermiteCerrar = PermiteCerrar;
            }

            public string Titulo { get; set; }
            public bool PermiteCerrar { get; }
            public UIElement Contenido { get; set; }
            public TabItem TabItem { get; set; }
            public Tab TabButton { get; set; }
            public ControladorDePestañas Controlador { get; }

            internal void Representar()
            {
                Controlador.TabMain.Items.Add(TabItem = new TabItem()
                {
                    Content = Contenido,
                    Header = Titulo
                });
                Controlador.UniformControl.Children.Add(TabButton = new Tab(this));
            }

            internal void RepresentarComoInicio()
            {
                Controlador.TabMain.Items.Add(TabItem = new TabItem()
                {
                    Content = Contenido,
                    Header = Titulo
                });
            }

            internal void Cerrar() => Controlador.Cerrar(this);
            internal void Seleccionar() => Controlador.Seleccionar(this);

            public bool EstaSeleccionado {
                get => estaSeleccionado;
                set {
                    estaSeleccionado = value;
                    OnSeleccionado?.Invoke(estaSeleccionado);
                }
            }
            public delegate void Seleccionado(bool EstaSeleccionado);
            public event Seleccionado OnSeleccionado;

        }
    }
}
