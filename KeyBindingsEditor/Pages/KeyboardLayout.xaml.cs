using KeyBindingsEditor.Configuration;
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
        private readonly Dictionary<Keys, (Button Button, string Text)> buttons;

        public KeyboardLayout()
        {
            InitializeComponent();
            buttons = new Dictionary<Keys, (Button, string)>()
            {
                [Keys.A] = (A_key, "A"),
                [Keys.B] = (B_key, "B"),
                [Keys.C] = (C_key, "C"),
                [Keys.D] = (D_key, "D"),
                [Keys.E] = (E_key, "E"),
                [Keys.F] = (F_key, "F"),
                [Keys.G] = (G_key, "G"),
                [Keys.H] = (H_key, "H"),
                [Keys.I] = (I_key, "I"),
                [Keys.J] = (J_key, "J"),
                [Keys.K] = (K_key, "K"),
                [Keys.L] = (L_key, "L"),
                [Keys.M] = (M_key, "M"),
                [Keys.N] = (N_key, "N"),
                [Keys.O] = (O_key, "O"),
                [Keys.P] = (P_key, "P"),
                [Keys.Q] = (Q_key, "Q"),
                [Keys.R] = (R_key, "R"),
                [Keys.S] = (S_key, "S"),
                [Keys.T] = (T_key, "T"),
                [Keys.U] = (U_key, "U"),
                [Keys.V] = (V_key, "V"),
                [Keys.W] = (W_key, "W"),
                [Keys.X] = (X_key, "X"),
                [Keys.Y] = (Y_key, "Y"),
                [Keys.Z] = (Z_key, "Z"),
                [Keys.Escape] = (Escape_key, "Esc"),
                [Keys.F1] = (F1_key, "F1"),
                [Keys.F2] = (F2_key, "F2"),
                [Keys.F3] = (F3_key, "F3"),
                [Keys.F4] = (F4_key, "F4"),
                [Keys.F5] = (F5_key, "F5"),
                [Keys.F6] = (F6_key, "F6"),
                [Keys.F7] = (F7_key, "F7"),
                [Keys.F8] = (F8_key, "F8"),
                [Keys.F9] = (F9_key, "F9"),
                [Keys.F10] = (F10_key, "F10"),
                [Keys.F11] = (F11_key, "F11"),
                [Keys.F12] = (F12_key, "F12"),
                [Keys.PrintScreen] = (PrintScreen_key, "PrSc"),
                [Keys.Scroll] = (ScrollLock_key, "ScLk"),
                [Keys.Pause] = (Pause_key, "Pause"),
                [Keys.Insert] = (Insert_key, "Ins"),
                [Keys.Delete] = (Delete_key, "Del"),
                [Keys.PageUp] = (PageUp_key, "PgUp"),
                [Keys.PageDown] = (PageDown_key, "PgDn"),
                [Keys.OemTilde] = (Tilde_key, "~"),
                [Keys.D1] = (D1_key, "1"),
                [Keys.D2] = (D2_key, "2"),
                [Keys.D3] = (D3_key, "3"),
                [Keys.D4] = (D4_key, "4"),
                [Keys.D5] = (D5_key, "5"),
                [Keys.D6] = (D6_key, "6"),
                [Keys.D7] = (D7_key, "7"),
                [Keys.D8] = (D8_key, "8"),
                [Keys.D9] = (D9_key, "9"),
                [Keys.D0] = (D0_key, "0"),
                [Keys.OemMinus] = (Minus_key, "-"),
                [Keys.OemPlus] = (Sum_key, "="),
                [Keys.BackSpace] = (BackSpace_key, "BackSpace"),
                [Keys.NumLock] = (NumLock_key, "NumLk"),
                [Keys.Divide] = (NumpadSlash_key, "/"),
                [Keys.Multiply] = (NumpadStar_key, "*"),
                [Keys.Subtract] = (NumpadMinus_key, "-"),
                [Keys.Add] = (NumpadSum_key, "+"),
                [Keys.Tab] = (TAB_key, "Tab"),
                [Keys.OemBackslash] = (Backslash_key, "\\"),
                [Keys.NumPad0] = (Numpad0_key, "0"),
                [Keys.NumPad1] = (Numpad1_key, "1"),
                [Keys.NumPad2] = (Numpad2_key, "2"),
                [Keys.NumPad3] = (Numpad3_key, "3"),
                [Keys.NumPad4] = (Numpad4_key, "4"),
                [Keys.NumPad5] = (Numpad5_key, "5"),
                [Keys.NumPad6] = (Numpad6_key, "6"),
                [Keys.NumPad7] = (Numpad7_key, "7"),
                [Keys.NumPad8] = (Numpad8_key, "8"),
                [Keys.NumPad9] = (Numpad9_key, "9"),
                [Keys.Decimal] = (Point_key, "."),
                [Keys.NumPadEnter] = (NumpadEnter_key, "Enter"),
                [Keys.Space] = (Space_key, "Space"),
                [Keys.CapsLock] = (CapsLock_key, "CapsLk"),
                [Keys.Enter] = (Enter_key, "Enter"),
                [Keys.LeftShift] = (LeftShift_key, "Shift"),
                [Keys.RightShift] = (RightShift_key, "Shift"),
                [Keys.LeftCtrl] = (LeftControl_key, "CTRL"),
                [Keys.RightCtrl] = (RightControl_key, "CTRL"),
                [Keys.LeftAlt] = (LeftAlt_key, "ALT"),
                [Keys.RightAlt] = (RightAlt_key, "ALT"),
                [Keys.LeftWin] = (LeftStart_key, "Start"),
                [Keys.RightWin] = (RightStart_key, "Start"),
                [Keys.KanjiMode] = (Fn_key, "Fn"),
                [Keys.Home] = (Home_key, "Home"),
                [Keys.End] = (End_key, "End"),
                [Keys.OemComma] = (Less_key, "<"),
                [Keys.OemPeriod] = (Greater_key, ">"),
                [Keys.OemSemicolon] = (PointComma_key, ";"),
                [Keys.OemQuotes] = (QuotationMark_key, "'"),
                [Keys.OemQuestion] = (QuestionMark_key, "?"),
                [Keys.OemOpenBrackets] = (OpenSquareBracket_key, "["),
                [Keys.OemCloseBrackets] = (CloseSquareBracket_key, "]"),
                [Keys.Right] = (ArrowRight_key, "→"),
                [Keys.Left] = (ArrowLeft_key, "←"),
                [Keys.Up] = (ArrowUp_key, "↑"),
                [Keys.Down] = (ArrowDown_key, "↓"),
            };
            Loaded += OnLoaded;
            EditorViewModel.Instance.PropertyChanged += ViewModel_PropertyChanged;
            EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Keyboard.Bindings;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (EditorViewModel.Instance.CurrentEditorType != EditorInputType.Keyboard)
                return;
            if (e.PropertyName == nameof(EditorViewModel.Configuration))
            {
                EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Keyboard.Bindings;
            }
            if (e.PropertyName == nameof(EditorViewModel.Configuration) || e.PropertyName == nameof(EditorViewModel.BindingsContext))
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
            BindingVisualizer.VisualizeLayout(buttons, EditorViewModel.Instance);
        }

        public void Key_Click(object sender, RoutedEventArgs e)
        {
            var bindingButton = (Button)sender;
            var instance = EditorViewModel.Instance;
            var pair = buttons.First(x => x.Value.Button == bindingButton);
            var key = pair.Key;
            var bindings = (ICollection<KeyBinding<Keys>>)instance.BindingsContext;
            var binding = bindings.FirstOrDefault(x => x.Key == key);
            if (binding == null)
            {
                binding = new KeyBinding<Keys>() { Key = key, Parent = EditorViewModel.Instance.CombinationSource as KeyBinding<Keys> };
                bindings.Add(binding);
            }
            instance.SelectedBinding = binding;
            binding.PropertyChanged += (s, e) =>
            {
                BindingVisualizer.ApplyButtonLayout(bindingButton, pair.Value.Text, binding, EditorViewModel.Instance);
            };
        }
    }
}
