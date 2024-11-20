using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    /// <summary>
    /// Custom control that validates if the input text is a valid email.
    /// </summary>
    public partial class EmailTextBox : UserControl
    {
        // Constructor
        public EmailTextBox()
        {
            InitializeComponent();
        }

        // DependencyProperty for Email
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(EmailTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEmailChanged));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        // DependencyProperty for IsValid (read-only)
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(EmailTextBox),
                new PropertyMetadata(false));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidProperty, value); }
        }

        // Callback to handle email changes and update the IsValid state
        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailTextBox)d;
            control.IsValid = Regex.IsMatch(control.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            control.UpdateBorderColor();
        }

        // Method to update the border color based on the IsValid property
        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }
    }
}
