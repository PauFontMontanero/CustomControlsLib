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
        private static readonly char[] ValidDNILetters = "TRWAGMYFPDXBNJZSQVHLCKE".ToCharArray();

        public DNITextBox()
        {
            InitializeComponent();
            textBox.PreviewTextInput += TextBox_PreviewTextInput;
        }

        // DependencyProperty for DNI
        public static readonly DependencyProperty DNIProperty =
            DependencyProperty.Register("DNI", typeof(string), typeof(DNITextBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnDNIChanged));

        public string DNI
        {
            get { return (string)GetValue(DNIProperty); }
            set { SetValue(DNIProperty, value); }
        }

        // DependencyProperty for IsValid (read-only)
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(DNITextBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidProperty, value); }
        }

        // DependencyProperty for ValidationMessage
        public static readonly DependencyProperty ValidationMessageProperty =
            DependencyProperty.Register("ValidationMessage", typeof(string), typeof(DNITextBox),
                new PropertyMetadata(string.Empty));

        public string ValidationMessage
        {
            get { return (string)GetValue(ValidationMessageProperty); }
            private set { SetValue(ValidationMessageProperty, value); }
        }

        // DependencyProperty for ErrorBorderBrush
        public static readonly DependencyProperty ErrorBorderBrushProperty =
            DependencyProperty.Register("ErrorBorderBrush", typeof(Brush), typeof(DNITextBox),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public Brush ErrorBorderBrush
        {
            get { return (Brush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }

        private static void OnDNIChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DNITextBox)d;
            control.ValidateDNI();
        }

        private void ValidateDNI()
        {
            if (string.IsNullOrEmpty(DNI))
            {
                IsValid = false;
                ValidationMessage = "ID number cannot be empty";
                UpdateBorderColor();
                return;
            }

            if (!Regex.IsMatch(DNI, @"^\d{8}[A-Za-z]$"))
            {
                IsValid = false;
                ValidationMessage = "ID number must have 8 numbers followed by a letter";
                UpdateBorderColor();
                return;
            }

            string numbers = DNI.Substring(0, 8);
            char providedLetter = char.ToUpper(DNI[8]);

            if (int.TryParse(numbers, out int dniNumber))
            {
                char expectedLetter = ValidDNILetters[dniNumber % 23];
                IsValid = providedLetter == expectedLetter;
                ValidationMessage = IsValid ?
                    "Valid DNI" :
                    $"Incorrect letter. The correct letter should be {expectedLetter}";
            }
            else
            {
                IsValid = false;
                ValidationMessage = "Invalid ID number format";
            }

            UpdateBorderColor();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (textBox.Text.Length < 8)
            {
                // Only allow digits for the first 8 characters
                e.Handled = !char.IsDigit(e.Text[0]);
            }
            else if (textBox.Text.Length == 8)
            {
                // Only allow letters for the 9th character
                e.Handled = !char.IsLetter(e.Text[0]);
            }
            else
            {
                // Don't allow more than 9 characters
                e.Handled = true;
            }
        }

        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : ErrorBorderBrush;
        }
    }
}