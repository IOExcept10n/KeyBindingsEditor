using System.Windows;
using System.Windows.Media;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для ColorPickerWindow.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        public Color SelectedColor => Picker.SelectedColor;

        public ColorPickerWindow()
        {
            InitializeComponent();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}