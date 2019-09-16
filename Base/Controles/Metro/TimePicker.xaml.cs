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
    /// Lógica de interacción para TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    {
        public TimePicker()
        {
            InitializeComponent();
            this.DataContext = this;
            this.aControl.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter && Value.IsValid())
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
        private TimeSpan _value;
        private string inputValue = "";
        private bool isSelecting;
        private bool error = false;
        #endregion

        #region COMUNES 
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TimePicker), new PropertyMetadata("", TitlePropertyChanged));
        public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
        private static void TitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).TitlePropertyChanged((string)e.NewValue);
        private void TitlePropertyChanged(string Title) { }

        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(), SelectedTimePropertyChanged));
        public TimeSpan SelectedTime { get => (TimeSpan)GetValue(SelectedTimeProperty); set { SetValue(SelectedTimeProperty, value); } }
        private static void SelectedTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).SelectedTimePropertyChanged((TimeSpan)e.NewValue);
        private void SelectedTimePropertyChanged(TimeSpan SelectedTime) { Value = SelectedTime; OnPropertyChanged(); }

        public static readonly DependencyProperty RequiredProperty = DependencyProperty.Register("Required", typeof(bool), typeof(TimePicker), new PropertyMetadata(false, RequiredPropertyChanged));
        public bool Required { get => (bool)GetValue(RequiredProperty); set { SetValue(RequiredProperty, value); } }
        private static void RequiredPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).RequiredPropertyChanged((bool)e.NewValue);
        private void RequiredPropertyChanged(bool Required) { }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(TimePicker), new PropertyMetadata(false, ReadOnlyPropertyChanged));
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set { SetValue(ReadOnlyProperty, value); } }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).ReadOnlyPropertyChanged((bool)e.NewValue);
        private void ReadOnlyPropertyChanged(bool ReadOnly) { OnPropertyChanged("NotReadOnly"); }
        public bool NotReadOnly => !ReadOnly;

        public static readonly DependencyProperty ValidateProperty = DependencyProperty.Register("Validate", typeof(Func<TimeSpan, bool>), typeof(TimePicker), new PropertyMetadata(null, ValidatePropertyChanged));
        public Func<TimeSpan, bool> Validate { get => (Func<TimeSpan, bool>)GetValue(ValidateProperty); set { SetValue(ValidateProperty, value); } }
        private static void ValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).ValidatePropertyChanged((Func<TimeSpan, bool>)e.NewValue);
        private void ValidatePropertyChanged(Func<TimeSpan, bool> Validate) { }

        public static readonly DependencyProperty OnErrorProperty = DependencyProperty.Register("OnError", typeof(Action<object, string>), typeof(TimePicker), new PropertyMetadata(null, OnErrorPropertyChanged));
        public Action<object, string> OnError { get => (Action<object, string>)GetValue(OnErrorProperty); set { SetValue(OnErrorProperty, value); } }
        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).OnErrorPropertyChanged((Action<object, string>)e.NewValue);
        private void OnErrorPropertyChanged(Action<object, string> OnError) { }

        public static readonly DependencyProperty ControlWidthProperty = DependencyProperty.Register("ControlWidth", typeof(int), typeof(TimePicker), new PropertyMetadata(100, ControlWidthPropertyChanged));
        public int ControlWidth { get => (int)GetValue(ControlWidthProperty); set { SetValue(ControlWidthProperty, value); } }
        private static void ControlWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).ControlWidthPropertyChanged((int)e.NewValue);
        private void ControlWidthPropertyChanged(int ControlWidth) { }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(TimePicker), new PropertyMetadata("", WaterMarkPropertyChanged));
        public string WaterMark { get => (string)GetValue(WaterMarkProperty); set { SetValue(WaterMarkProperty, value); } }
        private static void WaterMarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TimePicker)d).WaterMarkPropertyChanged((string)e.NewValue);
        private void WaterMarkPropertyChanged(string WaterMark) { }
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
        public string TextValue => Value.IsValid() ? Value.ToStringTimeSpan() : "";
        public TimeSpan Value {
            get => _value;
            set {
                _value = value;
                if (value.IsValid())
                {
                    SelectedTime = value;
                    if (value.ToStringTime() != InputValue)
                    {
                        SetInputValue(value);
                    }
                    IsOpen = false;
                }
                OnPropertyChanged("Value");
                OnPropertyChanged("TextValue");
            }
        }
        public string InputValue {
            get => inputValue;
            set {
                var date = value.ToTime();
                inputValue = value;
                Error = (!date.IsValid() && Required) || (Validate.IsNotNull() && Validate?.Invoke(date) == false);
                if (date != Value)
                    SetValue(date);
                OnPropertyChanged("InputValue");
            }
        }
        #endregion

        #region CONTROL
        void SetValue(TimeSpan value)
        {
            if (value.TotalHours < 24)
                Value = value;
            OnPropertyChanged("Value");
        }
        void SetInputValue(TimeSpan value)
        {
            inputValue = value.ToStringTime();
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
        public bool IsControlFocused => aControl.IsNotNull() && aControl.IsFocused;
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
        public bool IsSelected => Value.IsValid();
        public bool IsOpen { get => isOpen; set { isOpen = value; if (value) OnPropertyChanged("BorderColor"); OnPropertyChanged("IsOpen"); } }
        public bool Error { get => error; set { error = value; OnPropertyChanged("Error"); OnPropertyChanged("BorderColor"); if (Error) OnError?.Invoke(this, InputValue); } }
        #endregion


    }
}
