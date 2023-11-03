using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KeyBindingsEditor.Configuration
{
    public class InputSourceConfigurationBase<T> : INotifyPropertyChanged
    {
        private ObservableCollection<BindingsLayer<T>> layers = null!;

        public ObservableCollection<BindingsLayer<T>> Layers
        {
            get => layers;
            set
            {
                layers = value;
                OnChanged(true);
                layers.CollectionChanged += BindingsUpdated;
            }
        }

        public InputSourceConfigurationBase()
        {
            Layers = new()
            {
                new()
                {
                    Name = "Default",
                    Enabled = true
                }
            };
        }

        private void BindingsUpdated(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (BindingsLayer<T> b in e.NewItems!)
                    {
                        b.PropertyChanged += BindingUpdated;
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (BindingsLayer<T> b in e.OldItems!)
                    {
                        b.PropertyChanged -= BindingUpdated;
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (BindingsLayer<T> b in Layers)
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
            PropertyChanged?.Invoke(this, new(nameof(Layers) + (values ? "Values" : "")));
        }

        private void BindingUpdated(object? source, PropertyChangedEventArgs e)
        {
            OnChanged(true);
        }
    }

    internal interface IBindingsLayer
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<IKeyBinding> Bindings { get; }
    }

    public class BindingsLayer<T> : INotifyPropertyChanged, IBindingsLayer
    {
        private string name;
        private ObservableCollection<KeyBinding<T>> bindings = null!;
        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                PropertyChanged?.Invoke(this, new(nameof(Enabled)));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new(nameof(Name)));
            }
        }

        public ObservableCollection<KeyBinding<T>> Bindings
        {
            get => bindings;
            set
            {
                bindings = value;
                OnChanged(false);
                bindings.CollectionChanged += BindingsUpdated;
            }
        }

        IEnumerable<IKeyBinding> IBindingsLayer.Bindings => Bindings;

        public BindingsLayer()
        {
            Enabled = true;
            name = "New Layer";
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

        public override string ToString()
        {
            return $"{(Enabled ? '#' : '\0')} {Name}: {Bindings.Count} bindings";
        }
    }
}