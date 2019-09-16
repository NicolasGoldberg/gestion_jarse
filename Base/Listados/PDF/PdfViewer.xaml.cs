using Extenciones;
using Microsoft.Win32;
using MoonPdfLib;
using Notifications.Wpf;
using PdfiumViewer;
using Sistema.Base.Controles.Ventanas;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sistema.Base.Listados.PDF
{
    /// <summary>
    /// Lógica de interacción para PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public PdfViewer()
        {
            InitializeComponent();
        }

        private MoonPdfPanel Moon;
        private string FileName { get; set; }

        public bool Active => Moon.IsNotNull() && File.Exists(FileName);
        public string TotalPages => Moon.IsNotNull() ? Moon.TotalPages.ToStr() : "";
        public string Zoom => Moon.IsNotNull() ? Moon.CurrentZoom.ToStr() : "";
        public string ActualPage => Moon.IsNotNull() ? Moon.GetCurrentPageNumber().ToStr() : "";
        public ICommand Print => new Comando(() =>
        {
            new Imprimir(FileName);
            return;
            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = true;
            bool? doPrint = printDialog.ShowDialog();
            if (doPrint != true)
            {
                return;
            }

            using (var document = PdfDocument.Load(FileName))
            {
                using (var printDocument = document.CreatePrintDocument())
                {
                    printDocument.PrinterSettings.PrinterName = printDialog.PrintQueue.Name;
                    printDocument.DocumentName = FileName;
                    printDocument.PrinterSettings.PrintFileName = FileName;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                }
            }



        });
        public ICommand Download => new Comando(() =>
        {

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Documento Pdf|*.pdf";
            save.Title = "Guardar archivo";
            save.ShowDialog();

            if (save.FileName != "")
            {
                try
                {
                    if (File.Exists(save.FileName))
                        File.Delete(save.FileName);
                    File.Copy(FileName, save.FileName, true);
                }
                catch (Exception)
                {
                    App.Notificaciones.Error("","Error al guardar");
                }
                finally
                {
                    App.Notificaciones.ArchivoGuardado(save.FileName);
                }
            }
        });
        public ICommand Maximize => new Comando(() => { if (Moon.IsNotNull()) Moon.ZoomToWidth(); OnPropertyChanged(); });
        public ICommand ZoomIn => new Comando(() => { if (Moon.IsNotNull()) Moon.ZoomIn(); OnPropertyChanged(); });
        public ICommand ZoomOut => new Comando(() => { if (Moon.IsNotNull()) Moon.ZoomOut(); OnPropertyChanged(); });

        public ICommand GotoNextPage => new Comando(() => { if (Moon.IsNotNull()) Moon.GotoNextPage(); OnPropertyChanged(); });
        public ICommand GotoPreviousPage => new Comando(() => { if (Moon.IsNotNull()) Moon.GotoPreviousPage(); OnPropertyChanged(); });


        public bool OpenFile(string File, string Password = null)
        {
            this.FileName = File;
            
            if (Archivos.CheckPDF(File))
            {
                try
                {
                    Moon = new MoonPdfPanel();
                    Moon.OpenFile(File, Password);
                    PdfContent.Children.Clear();
                    PdfContent.Children.Add(Moon);
                    OnPropertyChanged();
                    return true;
                }
                catch (Exception ex)
                {
                    App.Modales.Alerta(ex.ToStr(), "Error al abrir el documento.");
                }

            } else
            {
                App.Modales.Alerta("Archivo corrupto o inexistente.", "Error al abrir el documento.");
            }
            return false;
        }

        public void CloseFile()
        {
            PdfContent.Children.Clear();
            FileName = "";
            Moon = null;
            OnPropertyChanged();
        }
    }
}