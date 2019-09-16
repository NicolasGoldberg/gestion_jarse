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
    /// Lógica de interacción para TextArea.xaml
    /// </summary>
    public partial class TextArea : UserControl, INotifyPropertyChanged
    {
        public TextArea()
        {
            InitializeComponent();
            this.DataContext = this;
           
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
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TextArea), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TextArea), new PropertyMetadata("", ValuePropertyChanged));
        public string Value { get => (string)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).ValuePropertyChanged((string)e.NewValue);
        private void ValuePropertyChanged(string Value) { InputValue = Value.IsNotNull() ? Value : ""; }

        public static readonly DependencyProperty ControlWidthProperty = DependencyProperty.Register("ControlWidth", typeof(int), typeof(TextArea), new PropertyMetadata(100, ControlWidthPropertyChanged));
        public int ControlWidth { get => (int)GetValue(ControlWidthProperty); set { SetValue(ControlWidthProperty, value); } }
        private static void ControlWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).ControlWidthPropertyChanged((int)e.NewValue);
        private void ControlWidthPropertyChanged(int ControlWidth) { }

        public static readonly DependencyProperty TextBoxMinHeightProperty = DependencyProperty.Register("TextBoxMinHeight", typeof(int), typeof(TextArea), new PropertyMetadata(30, TextBoxMinHeightPropertyChanged));
        public int TextBoxMinHeight { get => (int)GetValue(TextBoxMinHeightProperty); set { SetValue(TextBoxMinHeightProperty, value); } }
        private static void TextBoxMinHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).TextBoxMinHeightPropertyChanged((int)e.NewValue);
        private void TextBoxMinHeightPropertyChanged(int ControlWidth) { }

        public static readonly DependencyProperty RequiredProperty = DependencyProperty.Register("Required", typeof(bool), typeof(TextArea), new PropertyMetadata(false, RequiredPropertyChanged));
        public bool Required { get => (bool)GetValue(RequiredProperty); set { SetValue(RequiredProperty, value); } }
        private static void RequiredPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).RequiredPropertyChanged((bool)e.NewValue);
        private void RequiredPropertyChanged(bool Required) { }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(TextArea), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { }

        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<string, bool>), typeof(TextArea), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<string, bool> Validate { get => (Func<string, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).ValidatePropertyChanged((Func<string, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<string, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, string>), typeof(TextArea), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, string> OnError { get => (Action<object, string>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).OnErrorPropertyChanged((Action<object, string>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, string> OnError) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(TextArea), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(TextArea), new PropertyMetadata(2000, MaxLengthPropertyChanged));
        public int MaxLength { get => (int)GetValue(MaxLengthProperty); set { SetValue(MaxLengthProperty, value); } }
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextArea)d).MaxLengthPropertyChanged((int)e.NewValue);
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
