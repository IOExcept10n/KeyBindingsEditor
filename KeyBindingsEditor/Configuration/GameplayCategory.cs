using KeyBindingsEditor.ViewModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace KeyBindingsEditor.Configuration
{
    public class GameplayCategory : INotifyPropertyChanged, IDataErrorInfo
    {
        private Color color;
        private string name = string.Empty;
        private ObservableCollection<ActionInfo> actions = new();
        private bool isDeleted;

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
                foreach (var action in actions)
                {
                    action.Category = name;
                }
            }
        }

        [JsonIgnore]
        public bool IsDeleted
        {
            get => isDeleted;
            private set
            {
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
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

        [JsonIgnore]
        public string Error { get; } = null!;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name) || Name.Contains(' '))
                            error = "The category name is incorrect. Please, don't use whitespaces in name.";
                        break;
                }
                return error;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public void Initialize()
        {
            foreach (var action in Actions)
            {
                action.Category = Name;
            }
        }

        private void OnActionsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Actions) + "Content");
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        public void Delete()
        {
            Actions.Clear();
            IsDeleted = true;
        }
    }

    public class ActionInfo : INotifyPropertyChanged, IDataErrorInfo
    {
        private string name = null!;
        private string title = null!;
        private string description = null!;
        private string? category;
        private bool isDeleted;

        [JsonIgnore]
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

        [JsonIgnore]
        public bool IsDeleted
        {
            get => isDeleted;
            private set
            {
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        [JsonIgnore]
        public string Error { get; } = null!;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name) || Name.Contains(' '))
                            error = "The action name is incorrect. Please, don't use whitespaces in name.";
                        else if (GetCategory(EditorViewModel.Instance.Configuration.CategoryManager)?.Actions.Any(x => x != this && x.Name.Equals(Name)) == true)
                            error = "There is another action with the same name in this category. You shouldn't create multiple actions with the same name.";
                        break;
                }
                return error;
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

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}