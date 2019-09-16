using Extenciones;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sistema.Base.Controles.Metro
{
    /// <summary>
    /// Lógica de interacción para SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl, INotifyPropertyChanged
    {
        public SearchTextBox()
        {
            InitializeComponent();
            this.DataContext = this;

            aControl.GotFocus += (s, e) => OnFocus?.Invoke();
            this.MouseLeftButtonUp += (s, e) => OnFocus?.Invoke();
            aControl.MouseLeftButtonUp += (s, e) => OnFocus?.Invoke();
            this.aControl.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    OnFechaAbajo?.Invoke();
                }
            };
            //aControl.PreviewMouseDown += (s, e) => { if (aControl.IsFocused) OnFocus?.Invoke(); };
        }

        public void SetFocus()
        {
            aControl.Focus();
        }

        public delegate void OnFocusDelegate();
        public event OnFocusDelegate OnFocus;

        public delegate void FechaAbajoDelegate();
        public event FechaAbajoDelegate OnFechaAbajo;

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

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(SearchTextBox), new PropertyMetadata("", ValuePropertyChanged));
        public string Value { get => (string)GetValue(ValueProperty); set { SetValue(ValueProperty, value); } }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).ValuePropertyChanged((string)e.NewValue);
        private void ValuePropertyChanged(string Value) { InputValue = Value.IsNotNull() ? Value : ""; }


        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { }

        public static readonly DependencyProperty CargandoProperty = DependencyProperty.Register("Cargando", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false, CargandoPropertyChanged));
        public bool Cargando { get => (bool)GetValue(CargandoProperty); set { SetValue(CargandoProperty, value); } }
        private static void CargandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).CargandoPropertyChanged((bool)e.NewValue);
        private void CargandoPropertyChanged(bool Cargando) { OnPropertyChanged("Cargando"); }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SearchTextBox), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register("TitleWidth", typeof(int), typeof(SearchTextBox), new PropertyMetadata(100, TitleWidthPropertyChanged));
        public int TitleWidth { get => (int)GetValue(TitleWidthProperty); set { SetValue(TitleWidthProperty, value); } }
        private static void TitleWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).TitleWidthPropertyChanged((int)e.NewValue);
        private void TitleWidthPropertyChanged(int TitleWidth) { }

        public static readonly DependencyProperty ValueSelectedProperty = DependencyProperty.Register("ValueSelected", typeof(string), typeof(SearchTextBox), new PropertyMetadata("", ValueSelectedPropertyChanged));
        public string ValueSelected { get => (string)GetValue(ValueSelectedProperty); set { SetValue(ValueSelectedProperty, value); } }
        private static void ValueSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).ValueSelectedPropertyChanged((string)e.NewValue);
        private void ValueSelectedPropertyChanged(string ValueSelected) { OnPropertyChanged("ValueSelected"); }

        public static readonly DependencyProperty SeleccionadoProperty = DependencyProperty.Register("Seleccionado", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false, SeleccionadoPropertyChanged));
        public bool Seleccionado { get => (bool)GetValue(SeleccionadoProperty); set { SetValue(SeleccionadoProperty, value); } }
        private static void SeleccionadoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).SeleccionadoPropertyChanged((bool)e.NewValue);
        private void SeleccionadoPropertyChanged(bool Seleccionado) { OnPropertyChanged("Seleccionado"); }


        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<string, bool>), typeof(SearchTextBox), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<string, bool> Validate { get => (Func<string, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).ValidatePropertyChanged((Func<string, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<string, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, string>), typeof(SearchTextBox), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, string> OnError { get => (Action<object, string>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).OnErrorPropertyChanged((Action<object, string>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, string> OnError) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(SearchTextBox), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(SearchTextBox), new PropertyMetadata(255, MaxLengthPropertyChanged));
        public int MaxLength { get => (int)GetValue(MaxLengthProperty); set { SetValue(MaxLengthProperty, value); } }
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).MaxLengthPropertyChanged((int)e.NewValue);
        private void MaxLengthPropertyChanged(int MaxLength) { }

        public static readonly DependencyProperty EsCampoProperty = DependencyProperty.Register("EsCampo", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false, EsCampoPropertyChanged));
        public bool EsCampo { get => (bool)GetValue(EsCampoProperty); set { SetValue(EsCampoProperty, value); } }
        private static void EsCampoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((SearchTextBox)d).EsCampoPropertyChanged((bool)e.NewValue);
        private void EsCampoPropertyChanged(bool EsCampo)
        {
            if (EsCampo)
            {
                cBorder.BorderThickness = new Thickness(0, 0, 0, 0);
                cBorder.Background = LibConfig.Configuracion.Colores.Transparente;
            }
            else
            {
                cBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                cBorder.Background = LibConfig.Configuracion.Colores.Blanco;
            }
        }
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
                Error = false;
                OnPropertyChanged("InputValue");
                if (Timer.IsNotNull())
                    Timer.Stop();
                Timer = new DispatcherTimer();
                Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                Timer.Tick += (s, e) => SetValue(value);
                Timer.Start();
            }
        }
        #endregion
        public TimeSpan LastEdit { get; set; } = DateTime.Now.TimeOfDay;
        public DispatcherTimer Timer { get; set; }
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
        public bool IsControlFocused => (aControl.IsNotNull() && aControl.IsFocused);
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
                            return Misc.ConvertFromString("#C6C6C6");
                    }
                }

            }
        }
        public ICommand IUnSelect => new Comando(() => UnSelect?.Invoke());
        public delegate void UnSelectDelegate();
        public event UnSelectDelegate UnSelect;

        public bool IsSelected => Value.IsNotEmpty();
        public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged("IsOpen"); } }
        public bool Error { get => error; set { error = value; OnPropertyChanged("Error"); OnPropertyChanged("BorderColor"); if (Error) OnError?.Invoke(this, InputValue); } }
        #endregion
    }
}
