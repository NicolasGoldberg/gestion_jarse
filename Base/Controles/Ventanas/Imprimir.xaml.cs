using Extenciones;
using Extenciones.Clases.Modelos;
using MahApps.Metro.Controls;
using Sistema.Base.Listados.PDF;
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
using System.Windows.Shapes;

namespace Sistema.Base.Controles.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Imprimir.xaml
    /// </summary>
    public partial class Imprimir : MetroWindow, INotifyPropertyChanged
    {
        private Impresora seleccionada;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public Imprimir(params string[] Archivos)
        {
            InitializeComponent();
            Impresoras = new OCImpresora();
            if (App.DefaultPrinter.IsNotNull())
            {
                var def = Impresoras.Where(x => x.Nombre == App.DefaultPrinter.Nombre).FirstOrDefault();
                if (def.IsNotNull())
                {
                    Seleccionada = def;
                }
            }

            foreach (var item in Archivos)
                this.Archivos.Add(item);
            OnPropertyChanged();
            this.ShowDialog();
        }

        public Imprimir(List<string> Archivos)
        {
            InitializeComponent();
            Impresoras = new OCImpresora();
            if (App.DefaultPrinter.IsNotNull())
            {
                var def = Impresoras.Where(x => x.Nombre == App.DefaultPrinter.Nombre).FirstOrDefault();
                if (def.IsNotNull())
                {
                    Seleccionada = def;
                }
            }
            this.Archivos = Archivos;
            OnPropertyChanged();
            this.ShowDialog();
        }

        public OCImpresora Impresoras { get; }
        public Impresora Seleccionada { get => seleccionada; set { seleccionada = value; OnPropertyChanged(); } }
        public List<string> Archivos { get; } = new List<string>();

        public ICommand IImprimir => new Comando(() =>
        {
            Seleccionada.Imprimir(Archivos);
            this.Close();
        }, () => Seleccionada.IsNotNull());

        public ICommand ICancelar => new Comando(() =>
        {
            if (this.IsActive)
                this.Close();
        });

        public ICommand IAbrir => new Comando(() =>
        {
            int i = 0;
            foreach (var item in Archivos)
            {
                i++;
                var lit = new PdfViewer();
                if (lit.OpenFile(item))
                    App.Pestañas.Crear(lit, "Archivo " + i);
            }
            this.Close();
        });
    }
}
