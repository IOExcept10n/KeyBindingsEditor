using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesEditor.xaml
    /// </summary>
    public partial class CategoriesEditor : Page
    {
        private int currentCategoryIndex = -1;
        private GameplayCategory? currentCategory => EditorViewModel.Instance.Configuration.Categories[currentCategoryIndex];

        private bool currentCategorySelected => currentCategoryIndex >= 0 && currentCategory != null;

        public CategoriesEditor()
        {
            InitializeComponent();
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
            var categories = EditorViewModel.Instance.Configuration.Categories;
            categories.Add(new()
            {
                Name = $"Category {categories.Count + 1}",
                Color = Colors.BlueViolet
            });
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (CategoriesPanel.SelectedIndex != currentCategoryIndex)
            {
                if (currentCategorySelected)
                {
                    CloseCard(currentCategory);
                }
                currentCategoryIndex = CategoriesPanel.SelectedIndex;
                if (currentCategoryIndex < 0)
                    return;
                OpenCard(currentCategory);
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
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            var category = EditorViewModel.Instance.Configuration.Categories[currentCategoryIndex];
            category.Actions.Add(new()
            {
                Category = category.Name,
                Name = "New action",
                Description = "The description here"
            });
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Accept delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                EditorViewModel.Instance.Configuration.Categories.RemoveAt(currentCategoryIndex);
                currentCategoryIndex = -1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCategory != null)
            {
                ActionInfo context = (ActionInfo)((Button)sender).DataContext;
                currentCategory.Actions.Remove(context);
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
    }
}
