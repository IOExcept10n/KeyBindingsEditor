using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace KeyBindingsEditor.Configuration
{
    /// <summary>
    /// Represents a configuration for all input sources that can be loaded into a single file.
    /// </summary>
    public class InputConfiguration : INotifyPropertyChanged
    {
        private ObservableCollection<GameplayCategory> categories = null!;
        private KeyboardConfiguration keyboard = null!;
        private MouseConfiguration mouse = null!;
        private GamepadConfiguration gamepad = null!;

        internal CategoryManager CategoryManager { get; private set; } = null!;

        /// <summary>
        /// The configuration for the keyboard.
        /// </summary>
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

        /// <summary>
        /// The configuration for the mouse.
        /// </summary>
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

        /// <summary>
        /// The configuration for the gamepad.
        /// </summary>
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

        /// <summary>
        /// The list of categories used in the file.
        /// </summary>
        public ObservableCollection<GameplayCategory> Categories
        {
            get => categories;
            set
            {
                categories = value;
                CategoryManager = new(categories);
                foreach (var category in categories)
                {
                    category.Initialize();
                }
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
            foreach (var binding in Keyboard.Layers)
            {
                ConfigureLayer(binding);
            }
            foreach (var binding in Mouse.Layers)
            {
                ConfigureLayer(binding);
            }
            foreach (var binding in Gamepad.Layers)
            {
                ConfigureLayer(binding);
            }
            return this;
        }

        private void ConfigureLayer<T>(BindingsLayer<T> layer)
        {
            foreach (var binding in layer.Bindings)
            {
                binding.Activate(CategoryManager);
            }
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
                        b.Initialize();
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