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
    /// Lógica de interacción para TextBox.xaml
    /// </summary>
    public partial class TextBox : UserControl, INotifyPropertyChanged
    {
        public TextBox()
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

        new public void Focus()
        {
            App.FocusHelper.Focus(aControl);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #region CAMPOS
        private bool isOpen = false;
        private SolidColorBrush borderColor = App.Configuracion.Colores.Gris;
        private string inputValue = "";
        private bool isSelecting;
        private bool error = false;
        #endregion

        #region COMUNES 
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TextBox), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TextBox), new PropertyMetadata("", ValuePropertyChanged));
        public string Value { get => (string)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).ValuePropertyChanged((string)e.NewValue);
        private void ValuePropertyChanged(string Value) { InputValue = Value.IsNotNull() ? Value : ""; }

        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register("TitleWidth", typeof(int), typeof(TextBox), new PropertyMetadata(100, TitleWidthPropertyChanged));
        public int TitleWidth { get => (int)GetValue(TitleWidthProperty); set { SetValue(TitleWidthProperty, value); } }
        private static void TitleWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).TitleWidthPropertyChanged((int)e.NewValue);
        private void TitleWidthPropertyChanged(int TitleWidth) { }

        public static readonly DependencyProperty RequiredProperty = DependencyProperty.Register("Required", typeof(bool), typeof(TextBox), new PropertyMetadata(false, RequiredPropertyChanged));
        public bool Required { get => (bool)GetValue(RequiredProperty); set { SetValue(RequiredProperty, value); } }
        private static void RequiredPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).RequiredPropertyChanged((bool)e.NewValue);
        private void RequiredPropertyChanged(bool Required) { }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(TextBox), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { }

        public static readonly DependencyProperty CasingProperty = DependencyProperty.Register("Casing", typeof(System.Windows.Controls.CharacterCasing), typeof(TextBox), new PropertyMetadata(System.Windows.Controls.CharacterCasing.Upper, CasingPropertyChanged));
        public System.Windows.Controls.CharacterCasing Casing { get => (System.Windows.Controls.CharacterCasing)GetValue(CasingProperty); set { SetValue(CasingProperty, value); } }
        private static void CasingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).CasingPropertyChanged((System.Windows.Controls.CharacterCasing)e.NewValue);
        private void CasingPropertyChanged(System.Windows.Controls.CharacterCasing Casing) { }
        
        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<string, bool>), typeof(TextBox), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<string, bool> Validate { get => (Func<string, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).ValidatePropertyChanged((Func<string, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<string, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, string>), typeof(TextBox), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, string> OnError { get => (Action<object, string>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).OnErrorPropertyChanged((Action<object, string>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, string> OnError) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(TextBox), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(TextBox), new PropertyMetadata("", MaskPropertyChanged));
        public string Mask { get => (string)GetValue(MaskProperty); set { SetValue(MaskProperty, value); } }
        private static void MaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).MaskPropertyChanged((string)e.NewValue);
        private void MaskPropertyChanged(string Mask) { }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBox), new PropertyMetadata(255, MaxLengthPropertyChanged));
        public int MaxLength { get => (int)GetValue(MaxLengthProperty); set { SetValue(MaxLengthProperty, value); } }
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBox)d).MaxLengthPropertyChanged((int)e.NewValue);
        private void MaxLengthPropertyChanged(int MaxLength) { }
        #endregion

        #region EVENTOS COMUNES
        private void eMouseEnter(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeave(object sender, MouseEventArgs e) => OnPropertyChanged("BorderColor");
        private void eMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Select(true);
        private void eControlGetFocus(object sender, RoutedEventArgs e) => Select(true);
        private void Button_Click(object sender, RoutedEventArgs e) => IsOpen = !IsOpen;
        private void ControlLostFocus(object sender, RoutedEventArgs e) { IsOpen = false; OnPropertyChanged("BorderColor"); Select(false); }
        private void ControlGetFocus(object sender, RoutedEventArgs e) { SetInputValue(Value); OnPropertyChanged("BorderColor"); aControl.SelectAll(); }
        //private void ControlKeyUp(object sender, KeyEventArgs e) => Field?.KeyUp?.Invoke(e.Key);
        //private void ControlKeyDown(object sender, KeyEventArgs e) => Field?.KeyDown?.Invoke(e.Key);
        #endregion

        #region VALUE
        public string InputValue {
            get => inputValue;
            set {
                inputValue = value;
                Error = (value.IsEmpty() && Required) || (Validate.IsNotNull() && Validate?.Invoke(value) == false);
                OnPropertyChanged("InputValue");
                SetValue(value);
            }
        }
        #endregion


        #region CONTROL
        void SetValue(string value)
        {
            if (value != Value)
            {
                Value = value;
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
        public bool IsSelected => Value.IsNotEmpty();
        public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged("IsOpen"); } }
        public bool Error { get => error; set { error = value; OnPropertyChanged("Error"); OnPropertyChanged("BorderColor"); if (Error) OnError?.Invoke(this, InputValue); } }
        #endregion
    }
}
