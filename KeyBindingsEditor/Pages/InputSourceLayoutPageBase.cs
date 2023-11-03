using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor.Pages
{
    public abstract class MousePageBase : InputSourceLayoutPageBase<MouseButtons> 
    {
    }
    public abstract class KeyboardPageBase : InputSourceLayoutPageBase<Keys> 
    { 
    }
    public abstract class GamepadPageBase : InputSourceLayoutPageBase<GamepadButtons> 
    {
    }

    public abstract class InputSourceLayoutPageBase<T> : Page where T : notnull
    {
        private Dictionary<T, (Button Button, string Text)> buttons = new();
        private Button? previousButton;

        internal abstract IEnumerable<IBindingsLayer> Layers { get; }

        internal protected abstract EditorInputType EditorInputType { get; }

        internal static EditorViewModel Editor => EditorViewModel.Instance;

        protected void Initialize()
        {
            buttons = FillButtons();
            Loaded += OnLoaded;
            Editor.PropertyChanged += Editor_PropertyChanged;
            Editor.ReloadTrigger += (s, e) => ReloadLayout();
            Editor.LayersContext = Layers;
        }

        protected abstract Dictionary<T, (Button Button, string Text)> FillButtons();

        protected internal virtual void ReloadLayout()
        {
            previousButton = null;
            BindingVisualizer.VisualizeLayout(buttons, Editor);
        }

        private void Editor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Editor.CurrentEditorType != EditorInputType)
                return;
            if (e.PropertyName == nameof(EditorViewModel.Configuration))
            {
                Editor.LayersContext = Layers;
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

        public void OnKeyClick(object sender)
        {
            var bindingButton = (Button)sender;
            var pair = buttons.First(x => x.Value.Button == bindingButton);
            var key = pair.Key;
            var bindings = (ICollection<KeyBinding<T>>)Editor.BindingsContext!;
            var binding = bindings.FirstOrDefault(x => x.Key.Equals(key));
            if (binding == null)
            {
                binding = new KeyBinding<T>() { Key = key, Parent = EditorViewModel.Instance.CombinationSource as KeyBinding<T> };
                bindings.Add(binding);
            }
            Editor.SelectedBinding = binding;
            if (previousButton != null && bindings != null)
            {
                var oldPair = buttons.First(x => x.Value.Button == previousButton);
                var previousBinding = bindings.First(x => x.Key.Equals(oldPair.Key));
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
