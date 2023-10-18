using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using ModernWpf.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EditorViewModel context;
        private bool suppressCurrentUpdate;
        private int bindingPosition = 1;

        public MainWindow()
        {
            InitializeComponent();
            context = EditorViewModel.Instance = (EditorViewModel)DataContext;
            context.PropertyChanged += Instance_PropertyChanged;
            Closing += MainWindow_Closing;
            UpdateBindingState();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !context.SaveIfUnsaved();
        }

        private void Instance_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditorViewModel.SelectedBinding))
            {
                UpdateBindingState();
            }
        }

        private void UpdateBindingState()
        {
            var selected = context.SelectedBinding;
            suppressCurrentUpdate = true;
            Sequence1.IsEnabled = true;
            Sequence2.IsEnabled = Sequence3.IsEnabled = false;
            if (selected != null)
            {
                if (bindingPosition == 3)
                {
                    context.SequenceThird = selected;
                }
                else if (bindingPosition == 2)
                {
                    context.SequenceSecond = selected;
                }
                else if (bindingPosition == 1)
                {
                    context.SequenceFirst = selected;
                }
                ClickCategory.SelectedItem = selected.ClickAction?.GetCategory(context.Configuration.CategoryManager);
                ClickAction.SelectedItem = selected.ClickAction;
                IsClickEnabled.IsChecked = selected.ClickAction != null;
                DoubleClickCategory.SelectedItem = selected.DoubleClickAction?.GetCategory(context.Configuration.CategoryManager);
                DoubleClickAction.SelectedItem = selected.DoubleClickAction;
                IsDoubleClickEnabled.IsChecked = selected.DoubleClickAction != null;
                HoldCategory.SelectedItem = selected.HoldAction?.GetCategory(context.Configuration.CategoryManager);
                HoldAction.SelectedItem = selected.HoldAction;
                IsHoldEnabled.IsChecked = selected.HoldAction != null;
            }
            if (context.SequenceFirst != null)
            {
                if (context.SequenceSecond != null)
                {
                    Sequence3.IsEnabled = true;
                }
                Sequence2.IsEnabled = true;
            }
            suppressCurrentUpdate = false;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            OpenSelectedPage();
        }

        public void OpenSelectedPage()
        {
            context.SequenceFirst = context.SequenceSecond = context.SequenceThird = null;
            switch (NavView.MenuItems.IndexOf(NavView.SelectedItem))
            {
                // Keyboard
                case 0:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/KeyboardLayout.xaml", UriKind.Relative));
                    context.CurrentEditorType = EditorInputType.Keyboard;
                    break;

                case 1:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/MouseLayout.xaml", UriKind.Relative));
                    context.CurrentEditorType = EditorInputType.Mouse;
                    break;

                case 2:
                    Properties.Visibility = Visibility.Visible;
                    CurrentPageFrame.Navigate(new Uri("Pages/GamepadLayout.xaml", UriKind.Relative));
                    context.CurrentEditorType = EditorInputType.Gamepad;
                    break;

                case 3:
                    Properties.Visibility = Visibility.Hidden;
                    CurrentPageFrame.Navigate(new Uri("Pages/CategoriesEditor.xaml", UriKind.Relative));
                    context.CurrentEditorType = EditorInputType.None;
                    break;
            }
            context.SelectedBinding = null;
            context.CombinationSource = null;
        }

        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            if (context.CreateConfiguration())
            {
                // Maybe handle it.
            }
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

        private void Sequence1_Click(object sender, RoutedEventArgs e)
        {
            context.SequenceFirst = context.SequenceSecond = context.SequenceThird = null;
            context.SelectedBinding = null;
            context.CombinationSource = null;
            bindingPosition = 1;
        }

        private void Sequence2_Click(object sender, RoutedEventArgs e)
        {
            context.SequenceSecond = context.SequenceThird = null;
            context.SelectedBinding = null;
            context.CombinationSource = context.SequenceFirst;
            bindingPosition = 2;
        }

        private void Sequence3_Click(object sender, RoutedEventArgs e)
        {
            context.SequenceThird = null;
            context.SelectedBinding = null;
            context.CombinationSource = context.SequenceSecond;
            bindingPosition = 3;
        }

        private void ExportConfiguration_Click(object sender, RoutedEventArgs e)
        {
            context.ExportConfiguration();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            context.ImportConfiguration();
        }
    }
}