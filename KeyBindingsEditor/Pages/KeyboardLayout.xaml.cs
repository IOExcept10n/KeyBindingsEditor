using KeyBindingsEditor.ViewModel;
using System;
using System.Collections.Generic;
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

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для KeyboardLayout.xaml
    /// </summary>
    public partial class KeyboardLayout : Page
    {
        BindingVisualizer visualizer = new();
        Dictionary<Keys, Button> buttons;

        public KeyboardLayout()
        {
            InitializeComponent();
            buttons = new Dictionary<Keys, Button>()
            {
                [Keys.A] = A_key,
                [Keys.B] = B_key,
                [Keys.C] = C_key,
                [Keys.D] = D_key,
                [Keys.E] = E_key,
                [Keys.F] = F_key,
                [Keys.G] = G_key,
                [Keys.H] = H_key,
                [Keys.I] = I_key,
                [Keys.J] = J_key,
                [Keys.K] = K_key,
                [Keys.L] = L_key,
                [Keys.M] = M_key,
                [Keys.N] = N_key,
                [Keys.O] = O_key,
                [Keys.P] = P_key,
                [Keys.Q] = Q_key,
                [Keys.R] = R_key,
                [Keys.S] = S_key,
                [Keys.T] = T_key,
                [Keys.U] = U_key,
                [Keys.V] = V_key,
                [Keys.W] = W_key,
                [Keys.X] = X_key,
                [Keys.Y] = Y_key,
                [Keys.Z] = Z_key,
                [Keys.Escape] = Escape_key,
                [Keys.F1] = F1_key,
                [Keys.F2] = F2_key,
                [Keys.F3] = F3_key,
                [Keys.F4] = F4_key,
                [Keys.F5] = F5_key,
                [Keys.F6] = F6_key,
                [Keys.F7] = F7_key,
                [Keys.F8] = F8_key,
                [Keys.F9] = F9_key,
                [Keys.F10] = F10_key,
                [Keys.F11] = F11_key,
                [Keys.F12] = F12_key,
                [Keys.PrintScreen] = PrintScreen_key,
                [Keys.Scroll] = ScrollLock_key,
                [Keys.Pause] = Pause_key,
                [Keys.Insert] = Insert_key,
                [Keys.Delete] = Delete_key,
                [Keys.PageUp] = PageUp_key,
                [Keys.PageDown] = PageDown_key,
                [Keys.OemTilde] = Tilde_key,
                [Keys.D1] = D1_key,
                [Keys.D2] = D2_key,
                [Keys.D3] = D3_key,
                [Keys.D4] = D4_key,
                [Keys.D5] = D5_key,
                [Keys.D6] = D6_key,
                [Keys.D7] = D7_key,
                [Keys.D8] = D8_key,
                [Keys.D9] = D9_key,
                [Keys.D0] = D0_key,
                [Keys.OemMinus] = Minus_key,
                [Keys.OemPlus] = Sum_key,
                [Keys.BackSpace] = BackSpace_key,
                [Keys.NumLock] = NumLock_key,
                [Keys.Divide] = NumpadSlash_key,
                [Keys.Multiply] = NumpadStar_key,
                [Keys.Subtract] = NumpadMinus_key,
                [Keys.Add] = NumpadSum_key,
                [Keys.Tab] = TAB_key,
                [Keys.OemBackslash] = Backslash_key,
                [Keys.NumPad0] = Numpad0_key,
                [Keys.NumPad1] = Numpad1_key,
                [Keys.NumPad2] = Numpad2_key,
                [Keys.NumPad3] = Numpad3_key,
                [Keys.NumPad4] = Numpad4_key,
                [Keys.NumPad5] = Numpad5_key,
                [Keys.NumPad6] = Numpad6_key,
                [Keys.NumPad7] = Numpad7_key,
                [Keys.NumPad8] = Numpad8_key,
                [Keys.NumPad9] = Numpad9_key,
                [Keys.Decimal] = Point_key,
                [Keys.NumPadEnter] = NumpadEnter_key,
                [Keys.Space] = Space_key,
                [Keys.CapsLock] = CapsLock_key,
                [Keys.Enter] = Enter_key,
                [Keys.LeftShift] = LeftShift_key,
                [Keys.RightShift] = RightShift_key,
                [Keys.LeftCtrl] = LeftControl_key,
                [Keys.RightCtrl] = RightControl_key,
                [Keys.LeftAlt] = LeftAlt_key,
                [Keys.RightAlt] = RightAlt_key,
                [Keys.LeftWin] = LeftStart_key,
                [Keys.RightWin] = RightStart_key,
                [Keys.KanjiMode] = Fn_key,
                [Keys.Home] = Home_key,
                [Keys.End] = End_key,
                [Keys.OemComma] = Less_key,
                [Keys.OemPeriod] = Greater_key,
                [Keys.OemSemicolon] = PointComma_key,
                [Keys.OemQuotes] = QuotationMark_key,
                [Keys.OemQuestion] = QuestionMark_key,
                [Keys.OemOpenBrackets] = OpenSquareBracket_key,
                [Keys.OemCloseBrackets] = CloseSquareBracket_key,
                [Keys.Right] = ArrowRight_key,
                [Keys.Left] = ArrowLeft_key,
                [Keys.Up] = ArrowUp_key,
                [Keys.Down] = ArrowDown_key,
            };
            Loaded += OnLoaded;
            EditorViewModel.Instance.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditorViewModel.Configuration))
            {
                ReloadLayout();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ReloadLayout();
        }

        private void ReloadLayout()
        {
            visualizer.VisualizeLayout(buttons, EditorViewModel.Instance.Configuration.Keyboard, EditorViewModel.Instance.Configuration.CategoryManager);
        }

        public void Key_Click(object sender, RoutedEventArgs e)
        {
            var bindingButton = (Button)sender;
            var instance = EditorViewModel.Instance;
            var key = buttons.First(x => x.Value == bindingButton).Key;
            var bindings = instance.Configuration.Keyboard.Bindings;
            var binding = bindings.FirstOrDefault(x => x.Key == key);
            if (binding == null)
            {
                binding = new Configuration.KeyBinding<Keys>() { Key = key };
                bindings.Add(binding);
            }
            instance.SelectedBinding = binding;
            binding.PropertyChanged += (s, e) =>
            {
                visualizer.ApplyButtonLayout(bindingButton, binding, EditorViewModel.Instance.Configuration.CategoryManager);
            };
        }
    }
}
