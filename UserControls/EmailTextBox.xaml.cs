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
        private const string EMAIL_PATTERN = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        // Constructor
        public EmailTextBox()
        {
            InitializeComponent();
        }

        // DependencyProperty for Email
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(EmailTextBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnEmailChanged));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        // DependencyProperty for IsValid (read-only)
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(EmailTextBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // DependencyProperty for ValidationMessage
        public static readonly DependencyProperty ValidationMessageProperty =
            DependencyProperty.Register("ValidationMessage", typeof(string), typeof(EmailTextBox),
                new PropertyMetadata(string.Empty));

        public string ValidationMessage
        {
            get { return (string)GetValue(ValidationMessageProperty); }
            private set { SetValue(ValidationMessageProperty, value); }
        }

        // DependencyProperty for ErrorBorderBrush
        public static readonly DependencyProperty ErrorBorderBrushProperty =
            DependencyProperty.Register("ErrorBorderBrush", typeof(Brush), typeof(EmailTextBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public Brush ErrorBorderBrush
        {
            get { return (Brush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }

        // Callback to handle email changes and update the IsValid state
        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailTextBox)d;
            control.ValidateEmail();
        }

        // Method to validate email and update validation state
        private void ValidateEmail()
        {
            if (string.IsNullOrEmpty(Email))
            {
                IsValid = false;
                ValidationMessage = "Email cannot be empty";
                UpdateBorderColor();
                return;
            }

            if (Email.Length > 254)
            {
                IsValid = false;
                ValidationMessage = "Email is too long (maximum 254 characters)";
                UpdateBorderColor();
                return;
            }

            if (!Email.Contains("@"))
            {
                IsValid = false;
                ValidationMessage = "Email must contain @";
                UpdateBorderColor();
                return;
            }

            if (!Regex.IsMatch(Email, EMAIL_PATTERN))
            {
                IsValid = false;
                ValidationMessage = "Invalid email format";
                UpdateBorderColor();
                return;
            }

            IsValid = true;
            ValidationMessage = "Email correct";
            UpdateBorderColor();
        }

        // Method to update the border color based on the IsValid property
        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : ErrorBorderBrush;
        }
    }
}