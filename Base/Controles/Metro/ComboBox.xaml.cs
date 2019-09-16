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

namespace Sistema.Base.Controles.Metro
{
    /// <summary>
    /// Lógica de interacción para ComboBox.xaml
    /// </summary>
    public partial class ComboBox : UserControl, INotifyPropertyChanged
    {
        public ComboBox()
        {
            InitializeComponent();
            this.DataContext = this;
            this.aControl.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (EnterSetValue && ComboItems.HasRows())
                    {
                        SetValue(ComboItems[0]);
                        EnterSetValue = false;
                    }
                    TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                    UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;
                    if (keyboardFocus != null) { keyboardFocus.MoveFocus(tRequest); }
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #region CAMPOS
        private bool isOpen = false;
        private SolidColorBrush borderColor = App.Configuracion.Colores.Gris;
        private string inputValue = "";
        private bool isSelecting;
        private bool error = false;
        private List<ComboBoxModelItem> comboItems;
        #endregion

        #region COMUNES 
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ComboBox), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(ComboBoxModelItem), typeof(ComboBox), new PropertyMetadata(null, ValuePropertyChanged));
        public ComboBoxModelItem Value { get => (ComboBoxModelItem)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).ValuePropertyChanged((ComboBoxModelItem)e.NewValue);
        private void ValuePropertyChanged(ComboBoxModelItem Value) { InputValue = Value.IsNotNull() ? Value.Label : ""; }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ComboBoxModel), typeof(ComboBox), new PropertyMetadata(new ComboBoxModel(), ItemsSourcePropertyChanged));
        public ComboBoxModel ItemsSource { get => (ComboBoxModel)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }
        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).ItemsSourcePropertyChanged((ComboBoxModel)e.NewValue);
        private void ItemsSourcePropertyChanged(ComboBoxModel ItemsSource) { ComboItems = ItemsSource.ToList(); }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(ComboBox), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { OnPropertyChanged("NotReadOnly"); }
        public bool NotReadOnly => !ReadOnly;
        public static readonly DependencyProperty RequiredProperty = DependencyProperty.Register("Required", typeof(bool), typeof(ComboBox), new PropertyMetadata(false, RequiredPropertyChanged));
        public bool Required { get => (bool)GetValue(RequiredProperty); set { SetValue(RequiredProperty, value); } }
        private static void RequiredPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).RequiredPropertyChanged((bool)e.NewValue);
        private void RequiredPropertyChanged(bool Required) { }

        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<ComboBoxModelItem, bool>), typeof(ComboBox), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<ComboBoxModelItem, bool> Validate { get => (Func<ComboBoxModelItem, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).ValidatePropertyChanged((Func<ComboBoxModelItem, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<ComboBoxModelItem, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, string>), typeof(ComboBox), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, string> OnError { get => (Action<object, string>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).OnErrorPropertyChanged((Action<object, string>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, string> OnError) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(ComboBox), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(ComboBox), new PropertyMetadata("", MaskPropertyChanged));
        public string Mask { get => (string)GetValue(MaskProperty); set { SetValue(MaskProperty, value); } }
        private static void MaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).MaskPropertyChanged((string)e.NewValue);
        private void MaskPropertyChanged(string Mask) { }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(ComboBox), new PropertyMetadata(255, MaxLengthPropertyChanged));
        public int MaxLength { get => (int)GetValue(MaxLengthProperty); set { SetValue(MaxLengthProperty, value); } }
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ComboBox)d).MaxLengthPropertyChanged((int)e.NewValue);
        private void MaxLengthPropertyChanged(int MaxLength) { }
        #endregion

        #region EVENTOS COMUNES
        private void eMouseEnter(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeave(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Select(true);
        private void eControlGetFocus(object sender, RoutedEventArgs e) => Select(true);
        private void Button_Click(object sender, RoutedEventArgs e) => IsOpen = !IsOpen;
        private void ControlLostFocus(object sender, RoutedEventArgs e) { IsOpen = false; OnPropertyChanged("BorderColor"); Select(false); }
        private void ControlGetFocus(object sender, RoutedEventArgs e) { SetInputValue(Value.Label); OnPropertyChanged("BorderColor"); aControl.SelectAll(); }
        //private void ControlKeyUp(object sender, KeyEventArgs e) => Field?.KeyUp?.Invoke(e.Key);
        //private void ControlKeyDown(object sender, KeyEventArgs e) => Field?.KeyDown?.Invoke(e.Key);
        #endregion

        #region VALUE
        public string InputValue {
            get => inputValue;
            set {
                EnterSetValue = false;
                if (value.IsNotEmpty())
                {
                    ComboItems = ItemsSource.Where(x => x.Label.ToUpper().Contiene(value.ToUpper())).ToList();
                    if (ComboItems.HasRows())
                    {
                        if (ComboItems.Count == 1)
                            SetValue(ComboItems[0]);
                        else
                        {
                            var a = ComboItems[0].Label.Remove(0, value.Length);
                            inputValue = (value + a).ToUpper();
                            OnPropertyChanged("InputValue");
                            aControl.SelectedText = a;
                            EnterSetValue = true;
                            IsOpen = true;
                        }
                    }
                    else
                    {
                        inputValue = "";
                        OnPropertyChanged("InputValue");
                        ComboItems = ItemsSource.ToList();
                        Error = !IsSelected && Required;
                    }
                }
                else
                {
                    inputValue = "";
                    OnPropertyChanged("InputValue");
                    ComboItems = ItemsSource.ToList();
                    Error = !IsSelected && Required;
                }
            }
        }
        #endregion


        #region CONTROL
        void SetValue(ComboBoxModelItem value)
        {
            if (value != Value && value.IsNotNull())
            {
                Value = value;
                Error = !IsSelected && Required;
                inputValue = Value.Label;
                IsOpen = false;
                OnPropertyChanged("InputValue");
                OnPropertyChanged("Value");
            }
        }
        void SetInputValue(string value)
        {
            inputValue = value;
            OnPropertyChanged("InputValue");
        }
        void Select(bool v)
        {
            if (v)
            {
                IsSelecting = true;
                App.FocusHelper.Focus(aControl);
            }
            else
            {
                IsSelecting = false;
            }
        }
        public bool IsSelecting { get => isSelecting; set { isSelecting = value; OnPropertyChanged(); } }
        public bool IsNotSelecting => !IsSelecting;
        public bool IsControlFocused => (aControl.IsNotNull() && aControl.IsFocused) || IsOpen;
        public bool IsNotControlFocused => !IsControlFocused;
        public SolidColorBrush BorderColor {
            get {
                if (IsControlFocused)
                {
                    if (Error)
                        return App.Configuracion.Colores.Rojo;
                    else
                        return App.Configuracion.Colores.Negro;
                }
                else
                {
                    if (IsMouseOver)
                    {
                        if (Error)
                            return App.Configuracion.Colores.Rojo;
                        else
                            return App.Configuracion.Colores.GrisOscuro;
                    }
                    else
                    {
                        if (Error)
                            return App.Configuracion.Colores.RojoOscuro;
                        else
                            return App.Configuracion.Colores.Gris;
                    }
                }

            }
        }
        public List<ComboBoxModelItem> ComboItems { get => comboItems; set { comboItems = value; OnPropertyChanged("ComboItems"); } }
        public bool IsSelected => Value.IsNotNull();

        private bool EnterSetValue { get; set; }

        public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged("IsOpen"); OnPropertyChanged("IsNotOpen"); } }
        public bool IsNotOpen => !IsOpen;
        public bool Error { get => error; set { error = value; OnPropertyChanged("Error"); OnPropertyChanged("BorderColor"); if (Error) OnError?.Invoke(this, InputValue); } }
        #endregion

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var g = (DataGrid)sender;
                if (g.SelectedItem.IsNotNull() && g.SelectedItem is ComboBoxModelItem)
                {
                    SetValue(g.SelectedItem as ComboBoxModelItem);
                }
            }
            else if (e.Key == Key.Escape)
            {
                IsOpen = false;
            }
        }
        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var g = (DataGrid)sender;
            if (g.SelectedItem.IsNotNull() && g.SelectedItem is ComboBoxModelItem)
            {
                SetValue(g.SelectedItem as ComboBoxModelItem);
            }
        }
    }
}
