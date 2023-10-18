using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KeyBindingsEditor.Configuration
{
    public class InputSourceConfigurationBase<T> : INotifyPropertyChanged
    {
        private ObservableCollection<KeyBinding<T>> bindings = null!;

        public ObservableCollection<KeyBinding<T>> Bindings
        {
            get => bindings;
            set
            {
                bindings = value;
                OnChanged(true);
                bindings.CollectionChanged += BindingsUpdated;
            }
        }

        public InputSourceConfigurationBase()
        {
            Bindings = new();
        }

        private void BindingsUpdated(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (KeyBinding<T> b in e.NewItems!)
                    {
                        b.PropertyChanged += BindingUpdated;
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (KeyBinding<T> b in e.OldItems!)
                    {
                        b.PropertyChanged -= BindingUpdated;
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (KeyBinding<T> b in Bindings)
                    {
                        b.PropertyChanged -= BindingUpdated;
                    }
                    break;
            }
            OnChanged(true);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnChanged(bool values)
        {
            PropertyChanged?.Invoke(this, new(nameof(Bindings) + (values ? "Values" : "")));
        }

        private void BindingUpdated(object? source, PropertyChangedEventArgs e)
        {
            OnChanged(true);
        }
    }
}