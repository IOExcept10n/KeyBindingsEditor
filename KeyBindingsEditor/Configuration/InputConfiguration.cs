using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class InputConfiguration : INotifyPropertyChanged
    {
        private ObservableCollection<GameplayCategory> categories = null!;
        private KeyboardConfiguration keyboard = null!;
        private MouseConfiguration mouse = null!;
        private GamepadConfiguration gamepad = null!;

        internal CategoryManager CategoryManager { get; private set; } = null!;

        public KeyboardConfiguration Keyboard
        {
            get => keyboard;
            set
            {
                keyboard = value;
                keyboard.PropertyChanged += OnInputUpdated;
                OnPropertyChanged(nameof(Keyboard));
            }
        }

        public MouseConfiguration Mouse
        {
            get => mouse;
            set
            {
                mouse = value;
                mouse.PropertyChanged += OnInputUpdated;
                OnPropertyChanged(nameof(Mouse));
            }
        }

        public GamepadConfiguration Gamepad
        {
            get => gamepad;
            set
            {
                gamepad = value;
                gamepad.PropertyChanged += OnInputUpdated;
                OnPropertyChanged(nameof(Gamepad));
            }
        }

        public ObservableCollection<GameplayCategory> Categories
        {
            get => categories;
            set
            {
                categories = value;
                CategoryManager = new(categories);
                OnPropertyChanged(nameof(Categories));
                categories.CollectionChanged += OnCategoriesUpdated;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public InputConfiguration()
        {
            Categories = new();
            Keyboard = new();
            Mouse = new();
            Gamepad = new();
        }

        public InputConfiguration Configure()
        {
            foreach (var binding in Keyboard.Bindings)
            {
                binding.Activate(CategoryManager);
            }
            foreach (var binding in Mouse.Bindings)
            {
                binding.Activate(CategoryManager);
            }
            foreach (var binding in Gamepad.Bindings)
            {
                binding.Activate(CategoryManager);
            }
            return this;
        }

        public void Save(string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static InputConfiguration LoadFrom(string fileName)
        {
            return JsonConvert.DeserializeObject<InputConfiguration>(File.ReadAllText(fileName))!.Configure();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        private void OnInputUpdated(object? source, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("InputContents");
        }

        private void OnCategoriesUpdated(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (GameplayCategory b in e.NewItems!)
                    {
                        b.PropertyChanged += PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (GameplayCategory b in e.OldItems!)
                    {
                        b.PropertyChanged -= PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (GameplayCategory b in Categories)
                    {
                        b.PropertyChanged -= PropertyChanged;
                    }
                    break;
            }
            OnPropertyChanged(nameof(Categories));
        }
    }
}
