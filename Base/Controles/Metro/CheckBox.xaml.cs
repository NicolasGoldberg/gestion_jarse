using Extenciones;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sistema.Base.Controles.Metro
{
    /// <summary>
    /// Lógica de interacción para CheckBox.xaml
    /// </summary>
    public partial class CheckBox : UserControl, INotifyPropertyChanged
    {
        public CheckBox()
        {
            InitializeComponent();
            this.DataContext = this;
            this.aControl.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
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
        private bool inputValue = false;
        private bool isSelecting;
        private bool error = false;
        #endregion

        #region COMUNES 
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CheckBox), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(bool), typeof(CheckBox), new PropertyMetadata(false, ValuePropertyChanged));
        public bool Value { get =>  (bool)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).ValuePropertyChanged((bool)e.NewValue);
        private void ValuePropertyChanged(bool Value) { InputValue = Value.IsNotNull() ? Value : false; }

        public static readonly DependencyProperty ControlWidthProperty = DependencyProperty.Register("ControlWidth", typeof(int), typeof(CheckBox), new PropertyMetadata(100, ControlWidthPropertyChanged));
        public int ControlWidth { get => (int)GetValue(ControlWidthProperty); set { SetValue(ControlWidthProperty, value); } }
        private static void ControlWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).ControlWidthPropertyChanged((int)e.NewValue);
        private void ControlWidthPropertyChanged(int ControlWidth) { }

        public static readonly DependencyProperty RequiredProperty = DependencyProperty.Register("Required", typeof(bool), typeof(CheckBox), new PropertyMetadata(false, RequiredPropertyChanged));
        public bool Required { get => (bool)GetValue(RequiredProperty); set { SetValue(RequiredProperty, value); } }
        private static void RequiredPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).RequiredPropertyChanged((bool)e.NewValue);
        private void RequiredPropertyChanged(bool Required) { }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(CheckBox), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { OnPropertyChanged("NotReadOnly"); }
        public bool NotReadOnly => !ReadOnly;

        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<bool, bool>), typeof(CheckBox), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<bool, bool> Validate { get => (Func<bool, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).ValidatePropertyChanged((Func<bool, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<bool, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, bool>), typeof(CheckBox), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, bool> OnError { get => (Action<object, bool>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).OnErrorPropertyChanged((Action<object, bool>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, bool> OnError) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(CheckBox), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(CheckBox), new PropertyMetadata("", MaskPropertyChanged));
        public string Mask { get => (string)GetValue(MaskProperty); set { SetValue(MaskProperty, value); } }
        private static void MaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).MaskPropertyChanged((string)e.NewValue);
        private void MaskPropertyChanged(string Mask) { }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(CheckBox), new PropertyMetadata(255, MaxLengthPropertyChanged));
        public int MaxLength { get => (int)GetValue(MaxLengthProperty); set { SetValue(MaxLengthProperty, value); } }
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CheckBox)d).MaxLengthPropertyChanged((int)e.NewValue);
        private void MaxLengthPropertyChanged(int MaxLength) { }
        #endregion

        #region EVENTOS COMUNES
        private void eMouseEnter(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeave(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Select(true);
        private void eControlGetFocus(object sender, RoutedEventArgs e) => Select(true);
        private void Button_Click(object sender, RoutedEventArgs e) => IsOpen = !IsOpen;
        private void ControlLostFocus(object sender, RoutedEventArgs e) { IsOpen = false; OnPropertyChanged("BorderColor"); Select(false); }
        private void ControlGetFocus(object sender, RoutedEventArgs e) { SetInputValue(Value); OnPropertyChanged("BorderColor"); }
        //private void ControlKeyUp(object sender, KeyEventArgs e) => Field?.KeyUp?.Invoke(e.Key);
        //private void ControlKeyDown(object sender, KeyEventArgs e) => Field?.KeyDown?.Invoke(e.Key);
        #endregion

        #region VALUE
        public bool InputValue {
            get => inputValue;
            set {
                inputValue = value;
                Error = (value.IsNull() && Required) || (Validate.IsNotNull() && Validate?.Invoke(value) == false);
                OnPropertyChanged("InputValue");
                SetValue(value);
            }
        }
        #endregion


        #region CONTROL
        void SetValue(bool value)
        {
            if (value != Value)
            {
                Value = value;
                OnPropertyChanged("Value");
            }
        }
        void SetInputValue(bool value)
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
        public bool IsSelected => Value.IsNotNull();
        public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged("IsOpen"); } }
        public bool Error { get => error; set { error = value; OnPropertyChanged("Error"); OnPropertyChanged("BorderColor"); if (Error) OnError?.Invoke(this, InputValue); } }
        #endregion
    }
}
