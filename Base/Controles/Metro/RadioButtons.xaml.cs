using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Extenciones;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Libreria;
using Extenciones.Tipos;

namespace Sistema.Base.Controles.Metro
{
    /// <summary>
    /// Lógica de interacción para RadioButtons.xaml
    /// </summary>
    public partial class RadioButtons : UserControl, INotifyPropertyChanged
    {
        public enum Tipo { Ninguno, Logico, Sexo, Documento, Nacionalidad }
        public string SelectedKey { get; set; }
        public RadioButtons()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #region CAMPOS
        #endregion

        #region COMUNES 
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(RadioButtons), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RadioButtons)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(RadioButtons), new PropertyMetadata(null, ValuePropertyChanged));
        public object Value { get => (object)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RadioButtons)d).ValuePropertyChanged((object)e.NewValue);
        private void ValuePropertyChanged(object Value)
        {
            var val = ItemsSource.Where(x => x.Value == Value).FirstOrDefault();
            if (val.IsNotNull() && val.Key != SelectedKey)
                SelectedKey = val.Key;
            foreach (var item in wrapContent.Children)
            {
                if (item is RadioButton)
                {
                    var radio = item as RadioButton;
                    if (radio.IsChecked != true && radio.Tag.ToStr() == SelectedKey)
                        radio.IsChecked = true;
                }
            }
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(Tipo), typeof(RadioButtons), new PropertyMetadata(Tipo.Ninguno, TypePropertyChanged));
        public Tipo Type { get => (Tipo)GetValue(TypeProperty); set { SetValue(TypeProperty, Type); } }
        private static void TypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RadioButtons)d).TypePropertyChanged((Tipo)e.NewValue);
        private void TypePropertyChanged(Tipo Type)
        {
            if (Type == Tipo.Sexo)
            {
                ItemsSource = new Tipos.Sexo.Items();
            }

            if (Type == Tipo.Logico)
            {

            }
            else if (Type == Tipo.Documento)
            {
                ItemsSource = new Tipos.Documento.Items();
            }
            else if (Type == Tipo.Nacionalidad)
            {
                ItemsSource = new Tipos.Nacionalidad.Items();

            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(BindingItems), typeof(RadioButtons), new PropertyMetadata(new BindingItems(), ItemsSourcePropertyChanged));
        public BindingItems ItemsSource { get => (BindingItems)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }
        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RadioButtons)d).ItemsSourcePropertyChanged((BindingItems)e.NewValue);

        private void ItemsSourcePropertyChanged(BindingItems ItemsSource)
        {
            wrapContent.Children.Clear();
            foreach (var item in ItemsSource)
            {
                var radio = new RadioButton() { Content = item.Label };
                radio.Margin = new Thickness(3, 0, 0, 0);
                radio.Tag = item.Key;
                radio.Checked += (s, e) =>
                {
                    SelectedKey = item.Key;
                    item.Selected = true;
                    Value = item.Value;
                };
                wrapContent.Children.Add(radio);
            }
        }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(RadioButtons), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RadioButtons)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { OnPropertyChanged("NotReadOnly"); }
        public bool NotReadOnly => !ReadOnly;

        #endregion

        #region EVENTOS COMUNES
        #endregion

        #region VALUE
        #endregion

        #region CONTROL
        #endregion
    }
}
