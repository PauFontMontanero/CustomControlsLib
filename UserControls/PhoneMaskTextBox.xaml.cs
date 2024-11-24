using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlsLib
{
    public partial class PhoneMaskTextBox : UserControl
    {
        public PhoneMaskTextBox()
        {
            InitializeComponent();
            Mask = "+34 ### ### ###";
            phoneTextBox.PreviewTextInput += PhoneTextBox_PreviewTextInput;
            phoneTextBox.PreviewKeyDown += PhoneTextBox_PreviewKeyDown;
            UpdateDisplay();
        }

        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PhoneMaskTextBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPhoneNumberChanged));

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register("Mask", typeof(string), typeof(PhoneMaskTextBox),
                new PropertyMetadata("+34 ### ### ###"));

        public string Mask
        {
            get { return (string)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(PhoneMaskTextBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty ValidationMessageProperty =
            DependencyProperty.Register("ValidationMessage", typeof(string), typeof(PhoneMaskTextBox),
                new PropertyMetadata(string.Empty));

        public string ValidationMessage
        {
            get { return (string)GetValue(ValidationMessageProperty); }
            private set { SetValue(ValidationMessageProperty, value); }
        }

        private static void OnPhoneNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhoneMaskTextBox)d;
            control._numericValue = control.PhoneNumber ?? string.Empty;
            control.UpdateDisplay();
            control.ValidateInput();
        }

        private string _numericValue = string.Empty;

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
                return;
            }

            int maxDigits = Mask.Count(c => c == '#');

            if (_numericValue.Length < maxDigits)
            {
                _numericValue += e.Text;
                UpdateDisplay();
                PhoneNumber = _numericValue;
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }

            ValidateInput();
        }

        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && _numericValue.Length > 0)
            {
                _numericValue = _numericValue.Substring(0, _numericValue.Length - 1);
                PhoneNumber = _numericValue;
                UpdateDisplay();
                ValidateInput();
                e.Handled = true;
            }
        }

        private void UpdateDisplay()
        {
            string display = Mask;
            int valueIndex = 0;

            for (int i = 0; i < display.Length && valueIndex < _numericValue.Length; i++)
            {
                if (display[i] == '#')
                {
                    display = display.Substring(0, i) + _numericValue[valueIndex] + display.Substring(i + 1);
                    valueIndex++;
                }
            }

            phoneTextBox.Text = display;
        }

        private void ValidateInput()
        {
            int requiredDigits = Mask.Count(c => c == '#');
            IsValid = _numericValue.Length == requiredDigits;
            ValidationMessage = IsValid
                ? "Valid Number"
                : $"Phone number must have {requiredDigits} digits ({_numericValue.Length}/{requiredDigits})";
            UpdateBorderColor();
        }

        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }
    }
}