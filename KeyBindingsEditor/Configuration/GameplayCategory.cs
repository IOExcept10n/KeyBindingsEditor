using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace KeyBindingsEditor.Configuration
{
    public class GameplayCategory : INotifyPropertyChanged
    {
        private Color color;
        private string name = string.Empty;
        private ObservableCollection<ActionInfo> actions = new();

        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<ActionInfo> Actions
        {
            get => actions;
            set
            {
                actions = value;
                OnPropertyChanged(nameof(Actions));
                actions.CollectionChanged += OnActionsChanged;
            }
        }

        private void OnActionsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Actions) + "Content");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }

    public class ActionInfo : INotifyPropertyChanged
    {
        private string name = null!;
        private string title = null!;
        private string description = null!;
        private string? category;

        public string? Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public override string ToString()
        {
            return $"{Category}.{Name}";
        }

        internal GameplayCategory? GetCategory(CategoryManager manager)
        {
            return manager[Category];
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}