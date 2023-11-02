using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для MouseLayout.xaml
    /// </summary>
    public partial class MouseLayout : Page
    {
        private readonly Dictionary<MouseButtons, (Button Button, string Text)> buttons;
        private Button? previousButton;

        public MouseLayout()
        {
            InitializeComponent();
            buttons = new()
            {
                [MouseButtons.Left] = (Mouse_1, "LMB"),
                [MouseButtons.Right] = (Mouse_2, "RMB"),
                [MouseButtons.Middle] = (Mouse_3, "M"),
                [MouseButtons.Extended1] = (Mouse_4, "Mouse4"),
                [MouseButtons.Extended2] = (Mouse_5, "Mouse5")
            };
            Loaded += OnLoaded;
            EditorViewModel.Instance.PropertyChanged += ViewModel_PropertyChanged;
            EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Mouse.Bindings;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ReloadLayout();
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (EditorViewModel.Instance.CurrentEditorType != EditorInputType.Mouse)
                return;
            if (e.PropertyName == nameof(EditorViewModel.Configuration))
            {
                EditorViewModel.Instance.BindingsContext = EditorViewModel.Instance.Configuration.Mouse.Bindings;
            }
            if (e.PropertyName == nameof(EditorViewModel.Configuration) || e.PropertyName == nameof(EditorViewModel.BindingsContext))
            {
                ReloadLayout();
            }
        }

        private void ReloadLayout()
        {
            previousButton = null;
            BindingVisualizer.VisualizeLayout(buttons, EditorViewModel.Instance);
        }

        public void Key_Click(object sender, RoutedEventArgs e)
        {
            var bindingButton = (Button)sender;
            var instance = EditorViewModel.Instance;
            var pair = buttons.First(x => x.Value.Button == bindingButton);
            var key = pair.Key;
            var bindings = (ICollection<KeyBinding<MouseButtons>>)instance.BindingsContext!;
            var binding = bindings.FirstOrDefault(x => x.Key == key);
            if (binding == null)
            {
                binding = new KeyBinding<MouseButtons>() { Key = key, Parent = EditorViewModel.Instance.CombinationSource as KeyBinding<MouseButtons> };
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