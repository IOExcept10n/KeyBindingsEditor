using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamepadLayout.xaml
    /// </summary>
    public partial class GamepadLayout : Page
    {
        private readonly Dictionary<GamepadButtons, (Button Button, string Text)> buttons;
        private Button? previousButton;

        public GamepadLayout()
        {
            InitializeComponent();
            buttons = new()
            {
                [GamepadButtons.A] = (A, "A"),
                [GamepadButtons.B] = (B, "B"),
                [GamepadButtons.X] = (X, "X"),
                [GamepadButtons.Y] = (Y, "Y"),
                [GamepadButtons.Back] = (Back_Button, "BACK"),
                [GamepadButtons.Start] = (Start_Button, "START"),
                [GamepadButtons.LeftShoulder] = (Left_Button, "LB"),
                [GamepadButtons.LeftTrigger] = (Left_Trigger, "LT"),
                [GamepadButtons.RightShoulder] = (Right_Button, "RB"),
                [GamepadButtons.RightTrigger] = (Right_Trigger, "RT"),
                [GamepadButtons.LeftThumb] = (Left_Stick, "LS"),
                [GamepadButtons.RightThumb] = (Right_Stick, "RS"),
                [GamepadButtons.PadRight] = (Right_DPad, "→"),
                [GamepadButtons.PadLeft] = (Left_DPad, "←"),
                [GamepadButtons.PadUp] = (Up_DPad, "↑"),
                [GamepadButtons.PadDown] = (Down_DPad, "↓"),
            };
            EditorViewModel.Instance.PropertyChanged += ViewModel_PropertyChanged;
            EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Gamepad.Bindings;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (EditorViewModel.Instance.CurrentEditorType != EditorInputType.Gamepad)
                return;
            if (e.PropertyName == nameof(EditorViewModel.Configuration))
            {
                EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Gamepad.Bindings;
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
            previousButton = null;
            BindingVisualizer.VisualizeLayout(buttons, EditorViewModel.Instance);
        }

        private void GamepadKey_Click(object sender, RoutedEventArgs e)
        {
            var bindingButton = (Button)sender;
            var instance = EditorViewModel.Instance;
            var pair = buttons.First(x => x.Value.Button == bindingButton);
            var key = pair.Key;
            var bindings = (ICollection<KeyBinding<GamepadButtons>>)instance.BindingsContext!;
            var binding = bindings.FirstOrDefault(x => x.Key == key);
            if (binding == null)
            {
                binding = new KeyBinding<GamepadButtons>() { Key = key, Parent = EditorViewModel.Instance.CombinationSource as KeyBinding<GamepadButtons> };
                bindings.Add(binding);
            }
            instance.SelectedBinding = binding;
            if (previousButton != null && bindings != null)
            {
                var oldPair = buttons.First(x => x.Value.Button == previousButton);
                var previousBinding = bindings.First(x => x.Key == oldPair.Key);
                if (previousBinding != null)
                    BindingVisualizer.ApplyButtonLayout(previousButton, oldPair.Value.Text, previousBinding, EditorViewModel.Instance);
            }
            BindingVisualizer.ApplyButtonLayout(bindingButton, pair.Value.Text, binding, EditorViewModel.Instance);
            binding.PropertyChanged += (s, e) =>
            {
                BindingVisualizer.ApplyButtonLayout(bindingButton, pair.Value.Text, binding, EditorViewModel.Instance);
            };
            previousButton = bindingButton;
        }
    }
}