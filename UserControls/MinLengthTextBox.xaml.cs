using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    public partial class MinLengthTextBox : UserControl
    {
        public MinLengthTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MinLengthTextBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(MinLengthTextBox),
                new PropertyMetadata(3));

        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(MinLengthTextBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty ValidationMessageProperty =
            DependencyProperty.Register("ValidationMessage", typeof(string), typeof(MinLengthTextBox),
                new PropertyMetadata(string.Empty));

        public string ValidationMessage
        {
            get { return (string)GetValue(ValidationMessageProperty); }
            private set { SetValue(ValidationMessageProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MinLengthTextBox)d;
            control.ValidateText();
        }

        private void ValidateText()
        {
            if (string.IsNullOrEmpty(Text))
            {
                IsValid = false;
                ValidationMessage = $"Text must be at least {MinLength} characters long (current: 0)";
            }
            else
            {
                IsValid = Text.Length >= MinLength;
                ValidationMessage = IsValid
                    ? "Valid Name"
                    : $"Text must be at least {MinLength} characters long (current: {Text.Length})";
            }
            UpdateBorderColor();
        }

        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }
    }
}