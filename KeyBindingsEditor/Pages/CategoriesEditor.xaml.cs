using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesEditor.xaml
    /// </summary>
    public partial class CategoriesEditor : Page
    {
        private int currentCategoryIndex = -1;
        private GameplayCategory? CurrentCategory => EditorViewModel.Instance.Configuration.Categories[currentCategoryIndex];

        private bool CurrentCategorySelected => currentCategoryIndex >= 0 && currentCategoryIndex < EditorViewModel.Instance.Configuration.Categories.Count && CurrentCategory != null;

        public static RoutedCommand NewCategoryCommand = new("NewCategory", typeof(CategoriesEditor));

        public CategoriesEditor()
        {
            InitializeComponent();
            NewCategoryCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Control));
            Loaded += CategoriesEditor_Loaded;
        }

        private void CategoriesEditor_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var category in EditorViewModel.Instance.Configuration.Categories)
            {
                CloseCard(category);
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CreateNewCategory();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (CategoriesPanel.SelectedIndex != currentCategoryIndex)
            {
                if (CurrentCategorySelected)
                {
                    CloseCard(CurrentCategory);
                }
                currentCategoryIndex = CategoriesPanel.SelectedIndex;
                if (currentCategoryIndex < 0)
                    return;
                OpenCard(CurrentCategory);
            }
        }

        private void OpenCard(GameplayCategory? current)
        {
            ListBoxItem selectedItem = (ListBoxItem)CategoriesPanel.ItemContainerGenerator.ContainerFromItem(current);
            ContentPresenter presenter = FindVisualChild<ContentPresenter>(selectedItem);
            DataTemplate template = presenter.ContentTemplate;
            ListBox actionsPanel = (ListBox)template.FindName("ActionsPanel", presenter);
            TextBox categoryNameBox = (TextBox)template.FindName("CategoryNameBox", presenter);
            Button addAction = (Button)template.FindName("AddActionButton", presenter);
            Button removeCategory = (Button)template.FindName("RemoveCategoryButton", presenter);
            DoubleAnimation widthAnimation = new()
            {
                From = categoryNameBox.ActualWidth,
                To = selectedItem.ActualWidth - 120,
                Duration = TimeSpan.FromSeconds(0.5),
                DecelerationRatio = 0.2,
                FillBehavior = FillBehavior.HoldEnd,
            };
            categoryNameBox.IsEnabled = true;
            categoryNameBox.BeginAnimation(WidthProperty, widthAnimation);
            addAction.Visibility = removeCategory.Visibility = Visibility.Visible;
            double targetHeight = Math.Max(actionsPanel.Items.Count * 40 + (actionsPanel.Items.Count - 1) * 2, 0);
            DoubleAnimation heightAnimation = new()
            {
                From = 0,
                To = targetHeight,
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.8,
                FillBehavior = FillBehavior.Stop,
            };
            actionsPanel.BeginAnimation(HeightProperty, heightAnimation);
            actionsPanel.Visibility = Visibility.Visible;
            actionsPanel.IsEnabled = true;
        }

        private void CloseCard(GameplayCategory? current)
        {
            ListBoxItem selectedItem = (ListBoxItem)CategoriesPanel.ItemContainerGenerator.ContainerFromItem(current);
            ContentPresenter presenter = FindVisualChild<ContentPresenter>(selectedItem);
            DataTemplate template = presenter.ContentTemplate;
            ListBox actionsPanel = (ListBox)template.FindName("ActionsPanel", presenter);
            TextBox categoryNameBox = (TextBox)template.FindName("CategoryNameBox", presenter);
            Button addAction = (Button)template.FindName("AddActionButton", presenter);
            Button removeCategory = (Button)template.FindName("RemoveCategoryButton", presenter);
            DoubleAnimation widthAnimation = new()
            {
                From = categoryNameBox.ActualWidth,
                To = 300,
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.2,
                FillBehavior = FillBehavior.Stop,
            };
            categoryNameBox.IsEnabled = false;
            categoryNameBox.BeginAnimation(WidthProperty, widthAnimation);
            addAction.Visibility = removeCategory.Visibility = Visibility.Collapsed;
            actionsPanel.Visibility = Visibility.Collapsed;
            actionsPanel.IsEnabled = true;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem item)
                {
                    return item;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child!);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null!;
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            AddAndSelectNewAction();
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Accept delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var category = EditorViewModel.Instance.Configuration.Categories[currentCategoryIndex];
                category.Delete();
                EditorViewModel.Instance.Configuration.Categories.RemoveAt(currentCategoryIndex);
                currentCategoryIndex = -1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCategory != null)
            {
                ActionInfo context = (ActionInfo)((Button)sender).DataContext;
                context.Delete();
                CurrentCategory.Actions.Remove(context);
            }
        }

        private void PickColor_Click(object sender, RoutedEventArgs e)
        {
            var category = (GameplayCategory)((Button)sender).DataContext;
            if (category != null)
            {
                ColorPickerWindow picker = new();
                if (picker.ShowDialog() == true)
                {
                    category!.Color = picker.SelectedColor;
                }
            }
        }

        private void CategoryNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == 0)
            {
                AddAndSelectNewAction();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ActionInfo context = (ActionInfo)((TextBox)sender).DataContext;
            if (context.Description == "<Sample Description>" || string.IsNullOrEmpty(context.Description))
            {
                context.Description = context.Name;
            }
            if (context.Title == "<New Action>" || string.IsNullOrEmpty(context.Title))
            {
                context.Title = context.Name;
            }
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == 0)
            {
                AddAndSelectNewAction();
            }
        }

        private void AddAndSelectNewAction()
        {
            var category = EditorViewModel.Instance.Configuration.Categories[currentCategoryIndex];
            ActionInfo action = new()
            {
                Category = category.Name,
                Name = $"ActionKey{category.Actions.Count + 1}",
                Title = "<New Action>",
                Description = "<Sample Description>"
            };
            category.Actions.Add(action);
            ListBoxItem selectedItem = (ListBoxItem)CategoriesPanel.ItemContainerGenerator.ContainerFromItem(category);
            ContentPresenter presenter = FindVisualChild<ContentPresenter>(selectedItem);
            DataTemplate template = presenter.ContentTemplate;
            ListBox actionsPanel = (ListBox)template.FindName("ActionsPanel", presenter);
            actionsPanel.SelectedIndex = category.Actions.Count - 1;
            try
            {
                actionsPanel.UpdateLayout();
                selectedItem = (ListBoxItem)actionsPanel.ItemContainerGenerator.ContainerFromItem(action);
                presenter = FindVisualChild<ContentPresenter>(selectedItem);
                template = presenter.ContentTemplate;
                TextBox actionNameBox = (TextBox)template.FindName("ActionNameBox", presenter);
                actionNameBox.Focus();
                actionNameBox.SelectAll();
            }
            catch
            {
                // If the action was created until the animation is ended, the actions panel can't be updated.
                // It causes an exception while trying to find the control to focus because it's not yet generated.
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewCategory();
        }

        private void CreateNewCategory()
        {
            var categories = EditorViewModel.Instance.Configuration.Categories;
            categories.Add(new()
            {
                Name = $"Category{categories.Count + 1}",
                Color = Colors.BlueViolet
            });
            CategoriesPanel.UpdateLayout();
            CategoriesPanel.SelectedIndex = categories.Count - 1;
        }
    }
}