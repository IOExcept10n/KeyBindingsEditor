using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class KeyBinding<TKeyType> : IKeyBinding
    {
        private string? click;
        private string? hold;
        private string? doubleClick;
        private bool activated = true;
        private ActionInfo? clickAction;
        private ActionInfo? holdAction;
        private ActionInfo? doubleClickAction;
        private ObservableCollection<KeyBinding<TKeyType>> sequenceNextBindings = new();

        [JsonConverter(typeof(StringEnumConverter))]
        public TKeyType Key { get; set; } = default!;

        IKeyBinding? IKeyBinding.Parent => Parent;

        public KeyBinding<TKeyType>? Parent { get; set; }

        [JsonIgnore]
        public ActionInfo? ClickAction
        {
            get
            {
                ThrowIfNotActivated();
                return clickAction;
            }

            set
            {
                clickAction = value;
                OnPropertyChanged(nameof(ClickAction));
            }
        }

        [JsonIgnore]
        public ActionInfo? HoldAction
        {
            get
            {
                ThrowIfNotActivated();
                return holdAction;
            }
            set
            {
                holdAction = value;
                OnPropertyChanged(nameof(HoldAction));
            }
        }

        [JsonIgnore]
        public ActionInfo? DoubleClickAction
        {
            get
            {
                ThrowIfNotActivated();
                return doubleClickAction;
            }
            set 
            {
                doubleClickAction = value;
                OnPropertyChanged(nameof(DoubleClickAction));
            }
        }

        IEnumerable<IKeyBinding> IKeyBinding.NextBindings => SequenceNextBindings;

        public ObservableCollection<KeyBinding<TKeyType>> SequenceNextBindings
        {
            get => sequenceNextBindings;
            set
            {
                sequenceNextBindings = value;
                OnPropertyChanged(nameof(SequenceNextBindings));
                sequenceNextBindings.CollectionChanged += OnSequenceChanged;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("This property is only needed for the serialization process. Don't use it directly.")]
        public string? Click
        {
            get => ClickAction != null ? $"{ClickAction.Category}.{ClickAction.Name}" : null;
            set
            {
                activated = false;
                click = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("This property is only needed for the serialization process. Don't use it directly.")]
        public string? DoubleClick
        {
            get => DoubleClickAction != null ? $"{DoubleClickAction.Category}.{DoubleClickAction.Name}" : null;
            set
            {
                activated = false;
                doubleClick = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("This property is only needed for the serialization process. Don't use it directly.")]
        public string? Hold
        {
            get => HoldAction != null ? $"{HoldAction.Category}.{HoldAction.Name}" : null;
            set
            {
                activated = false;
                hold = value;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        internal void Activate(CategoryManager categories)
        {
            if (activated) return;
            if (!string.IsNullOrEmpty(click))
            {
                ClickAction = categories.GetAction(click);
            }
            else
            {
                ClickAction = null;
            }
            if (!string.IsNullOrEmpty(hold))
            {
                HoldAction = categories.GetAction(hold);
            }
            else
            {
                HoldAction = null;
            }
            if (!string.IsNullOrEmpty(doubleClick))
            {
                DoubleClickAction = categories.GetAction(doubleClick);
            }
            else
            {
                DoubleClickAction = null;
            }
            activated = true;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }

        private void OnSequenceChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (KeyBinding<TKeyType> b in e.NewItems!)
                    {
                        b.PropertyChanged += PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (KeyBinding<TKeyType> b in e.OldItems!)
                    {
                        b.PropertyChanged -= PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (KeyBinding<TKeyType> b in SequenceNextBindings)
                    {
                        b.PropertyChanged -= PropertyChanged;
                    }
                    break;
            }
            OnPropertyChanged(nameof(SequenceNextBindings));
        }

        private void ThrowIfNotActivated()
        {
            if (!activated)
            {
                throw new InvalidOperationException("Some of configuration values have been changed since the last binding opening.");
            }
        }
    }
}
