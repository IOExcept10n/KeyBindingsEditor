﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
                if (clickAction?.IsDeleted == true)
                    clickAction = null;
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
                if (holdAction?.IsDeleted == true)
                    holdAction = null;
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
                if (doubleClickAction?.IsDeleted == null)
                    doubleClickAction = null;
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

        [JsonIgnore]
        public string KeysSequence
        {
            get
            {
                if (Parent != null)
                {
                    Stack<KeyBinding<TKeyType>> parents = new();
                    var test = this;
                    while (test != null)
                    {
                        parents.Push(test);
                        test = test.Parent;
                    }
                    return string.Join('+', parents.Select(x => x.Key));
                }
                return Key?.ToString() ?? string.Empty;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void DeleteSequence()
        {
            SequenceNextBindings.Clear();
        }

        public bool AnyParents(Func<KeyBinding<TKeyType>, bool> predicate)
        {
            var test = this;
            do
            {
                if (!predicate(test))
                    return false;
                test = test.Parent;
            } while (test != null);
            return true;
        }

        public override string ToString()
        {
            return $"{Key}: [Click:{ClickAction}, DoubleClick:{DoubleClickAction}, Hold:{HoldAction}], Next: {SequenceNextBindings.Count}";
        }

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