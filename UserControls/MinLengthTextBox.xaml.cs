using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    /// <summary>
    /// Custom control that validates the minimum length of a text input.
    /// </summary>
    public partial class MinLengthTextBox : UserControl
    {
        // Constructor
        public MinLengthTextBox()
        {
            InitializeComponent();
        }

        // DependencyProperty for Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MinLengthTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // DependencyProperty for MinLength
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(MinLengthTextBox),
            new PropertyMetadata(3));

        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        // Read-only property to indicate if the input is valid
        public bool IsValid
        {
            get { return (Text != null && Text.Length >= MinLength); }
        }

        // Callback to handle text changes and refresh IsValid state
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MinLengthTextBox)d;
            control.UpdateBorderColor();
        }

        // Updates the border color based on validation
        private void UpdateBorderColor()
        {
            BorderBrush = IsValid ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.Red);
        }
    }
}
