using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    /// <summary>
    /// Custom control that validates and masks a phone number input.
    /// </summary>
    public partial class PhoneMaskTextBox : UserControl
    {
        public PhoneMaskTextBox()
        {
            InitializeComponent();
        }

        // DependencyProperty for PhoneNumber
        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PhoneMaskTextBox),
                new PropertyMetadata(string.Empty, OnPhoneNumberChanged));

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }

        // DependencyProperty for IsValid (read-only)
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(PhoneMaskTextBox),
                new PropertyMetadata(false));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidProperty, value); }
        }

        // Callback to handle phone number changes and update the IsValid state
        private static void OnPhoneNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhoneMaskTextBox)d;
            control.IsValid = Regex.IsMatch(control.PhoneNumber, @"^\d{9}$");
            control.UpdateBorderColor();
            control.ApplyMask();
        }

        // Method to update the border color based on the IsValid property
        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }

        // Method to apply a mask to the phone number
        private void ApplyMask()
        {
            if (PhoneNumber.Length > 0)
            {
                // Display as XXX-XXX-XXX format
                string maskedPhone = Regex.Replace(PhoneNumber, @"(\d{3})(\d{3})(\d{3})", "$1-$2-$3");
                // Update displayed text (for illustrative purposes, assuming TextBox is named phoneTextBox)
                phoneTextBox.Text = maskedPhone;
            }
        }
    }
}
