using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    /// <summary>
    /// Custom control that validates if the input text is a valid DNI (8 digits followed by a letter).
    /// </summary>
    public partial class DNITextBox : UserControl
    {
        public DNITextBox()
        {
            InitializeComponent();
        }

        // DependencyProperty for DNI
        public static readonly DependencyProperty DNIProperty =
            DependencyProperty.Register("DNI", typeof(string), typeof(DNITextBox),
                new PropertyMetadata(string.Empty, OnDNIChanged));

        public string DNI
        {
            get { return (string)GetValue(DNIProperty); }
            set { SetValue(DNIProperty, value); }
        }

        // DependencyProperty for IsValid (read-only)
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(DNITextBox),
                new PropertyMetadata(false));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidProperty, value); }
        }

        // Callback to handle DNI changes and update the IsValid state
        private static void OnDNIChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DNITextBox)d;
            control.IsValid = Regex.IsMatch(control.DNI, @"^\d{8}[A-Za-z]$");
            control.UpdateBorderColor();
        }

        // Method to update the border color based on the IsValid property
        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }
    }
}
