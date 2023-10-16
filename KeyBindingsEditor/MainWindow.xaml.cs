using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyBindingsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EditorViewModel context;
        private bool suppressCurrentUpdate;

        public MainWindow()
        {
            InitializeComponent();
            context = EditorViewModel.Instance = (EditorViewModel)DataContext;
            context.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditorViewModel.SelectedBinding))
            {
                var selected = context.SelectedBinding;
                suppressCurrentUpdate = true;
                if (selected != null)
                {
                    ClickCategory.SelectedItem = selected.ClickAction?.GetCategory(context.Configuration.CategoryManager);
                    ClickAction.SelectedItem = selected.ClickAction;
                    IsClickEnabled.IsChecked = selected.ClickAction != null;
                    DoubleClickCategory.SelectedItem = selected.DoubleClickAction?.GetCategory(context.Configuration.CategoryManager);
                    DoubleClickAction.SelectedItem = selected.DoubleClickAction;
                    IsDoubleClickEnabled.IsChecked = selected.DoubleClickAction != null;
                    HoldCategory.SelectedItem = selected.HoldAction?.GetCategory(context.Configuration.CategoryManager);
                    HoldAction.SelectedItem = selected.HoldAction;
                    IsHoldEnabled.IsChecked = selected.HoldAction != null;
                    BindingParentAsRoot.IsEnabled = selected.Parent != null;
                }
                ResetBindingRoot.IsEnabled = !(OpenAsBindingRoot.IsEnabled = selected != null);
                suppressCurrentUpdate = false;
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            OpenSelectedPage();
        }

        public void OpenSelectedPage()
        {
            context.SelectedBinding = null;
            switch (NavView.MenuItems.IndexOf(NavView.SelectedItem))
            {
                // Keyboard
                case 0:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/KeyboardLayout.xaml", UriKind.Relative));
                    break;
                case 1:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/MouseLayout.xaml", UriKind.Relative));
                    break;
                case 2:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/GamepadLayout.xaml", UriKind.Relative));
                    break;
                case 3:
                    Properties.Visibility = Visibility.Hidden;
                    CurrentPageFrame.Navigate(new Uri("Pages/CategoriesEditor.xaml", UriKind.Relative));
                    break;
            }
        }

        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (context.OpenConfiguration())
            {
                // Maybe handle it.
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (context.Save())
            {
                // Maybe handle it.
            }
        }

        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            if (context.SaveAs())
            {
                // Maybe handle it.
            }
        }

        private void ClickAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                context.SelectedBinding.ClickAction = (ActionInfo)ClickAction.SelectedItem;
            }
        }

        private void ClickCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void IsClickEnabled_Checked(object sender, RoutedEventArgs e)
        {
            ClickAction.IsEnabled = ClickCategory.IsEnabled = IsClickEnabled.IsChecked == true;
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                if (IsClickEnabled.IsChecked == false)
                {
                    context.SelectedBinding.ClickAction = null;
                }
            }
        }

        private void DoubleClickAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                context.SelectedBinding.DoubleClickAction = (ActionInfo)DoubleClickAction.SelectedItem;
            }
        }

        private void DoubleClickCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IsDoubleClickEnabled_Checked(object sender, RoutedEventArgs e)
        {
            DoubleClickAction.IsEnabled = DoubleClickCategory.IsEnabled = IsDoubleClickEnabled.IsChecked == true;
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                if (IsDoubleClickEnabled.IsChecked == false)
                {
                    context.SelectedBinding.ClickAction = null;
                }
            }
        }

        private void IsHoldEnabled_Checked(object sender, RoutedEventArgs e)
        {
            HoldAction.IsEnabled = HoldCategory.IsEnabled = IsHoldEnabled.IsChecked == true;
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                if (IsHoldEnabled.IsChecked == false)
                {
                    context.SelectedBinding.HoldAction = null;
                }
            }
        }

        private void HoldCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HoldAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (context.SelectedBinding != null && !suppressCurrentUpdate)
            {
                context.SelectedBinding.HoldAction = (ActionInfo)HoldAction.SelectedItem;
            }
        }
    }
}
