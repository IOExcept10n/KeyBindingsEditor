using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using ModernWpf.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static RoutedCommand SaveCommand = new RoutedCommand("SaveConfig", typeof(MainWindow));
        public static RoutedCommand OpenCommand = new RoutedCommand("OpenConfig", typeof(MainWindow));
        public static RoutedCommand NewFileCommand = new RoutedCommand("NewConfig", typeof(MainWindow));
        public static RoutedCommand SaveAsCommand = new RoutedCommand("SaveConfigAs", typeof(MainWindow));
        public static RoutedCommand RemoveSelectionCommand = new RoutedCommand("RemoveSelection", typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            SaveAsCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            RemoveSelectionCommand.InputGestures.Add(new KeyGesture(Key.Escape));
            NewFileCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
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
                IsClickEnabled.IsChecked = true;
                DoubleClickCategory.SelectedItem = selected.DoubleClickAction?.GetCategory(context.Configuration.CategoryManager);
                DoubleClickAction.SelectedItem = selected.DoubleClickAction;
                IsDoubleClickEnabled.IsChecked = selected.DoubleClickAction != null;
                HoldCategory.SelectedItem = selected.HoldAction?.GetCategory(context.Configuration.CategoryManager);
                HoldAction.SelectedItem = selected.HoldAction;
                IsHoldEnabled.IsChecked = selected.HoldAction != null;
            }
            else
            {
                IsClickEnabled.IsChecked = false;
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
            bindingPosition = 1;
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
            if (context.ExportConfiguration())
            {
                MessageBox.Show("The configuration has been successfully exported.");
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (context.ImportConfiguration())
            {
                MessageBox.Show("The configuration has been successfully imported.");
            }
        }

        private void DeleteSequence_Click(object sender, RoutedEventArgs e)
        {
            switch (bindingPosition)
            {
                case 1:
                    {
                        context.SequenceFirst?.DeleteSequence();
                        context.SequenceFirst = context.SequenceSecond = context.SequenceThird = null;
                        context.SelectedBinding = null;
                        context.CombinationSource = null;
                        bindingPosition = 1;
                    }
                    break;
                case 2:
                    {
                        context.SequenceSecond?.DeleteSequence();
                        context.SequenceFirst = context.SequenceSecond = context.SequenceThird = null;
                        context.SelectedBinding = null;
                        context.CombinationSource = null;
                        bindingPosition = 1;
                    }
                    break;
                case 3:
                    {
                        context.SequenceThird?.DeleteSequence();
                        context.SequenceSecond = context.SequenceThird = null;
                        context.SelectedBinding = null;
                        context.CombinationSource = context.SequenceFirst;
                        bindingPosition = 2;
                    }
                    break;
            }
        }

        private void OpenConfigHandled(object sender, ExecutedRoutedEventArgs e)
        {
            context.OpenConfiguration();
        }

        private void SaveConfigHandled(object sender, ExecutedRoutedEventArgs e)
        {
            context.Save();
        }

        private void SaveConfigAsHandled(object sender, ExecutedRoutedEventArgs e)
        {
            context.SaveAs();
        }

        private void NewFileHandled(object sender, ExecutedRoutedEventArgs e)
        {
            context.CreateConfiguration();
        }

        private void RemoveSelectionHandled(object sender, ExecutedRoutedEventArgs e)
        {
            context.SequenceFirst = context.SequenceSecond = context.SequenceThird = null;
            context.SelectedBinding = null;
            context.CombinationSource = null;
            bindingPosition = 1;
        }

        private void CreateMarkdown_Click(object sender, RoutedEventArgs e)
        {
            if (EditorViewModel.Instance.CreateMarkdown())
            {
                MessageBox.Show("Markdown creation has been completed successfully.");
            }
        }
    }
}