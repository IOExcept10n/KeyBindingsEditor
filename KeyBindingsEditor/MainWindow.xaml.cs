using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using ModernWpf.Controls;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public static RoutedCommand SaveCommand = new("SaveConfig", typeof(MainWindow));
        public static RoutedCommand OpenCommand = new("OpenConfig", typeof(MainWindow));
        public static RoutedCommand NewFileCommand = new("NewConfig", typeof(MainWindow));
        public static RoutedCommand SaveAsCommand = new("SaveConfigAs", typeof(MainWindow));
        public static RoutedCommand RemoveSelectionCommand = new("RemoveSelection", typeof(MainWindow));

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
            context.SelectedLayer = null;
            context.LayersContext = null;
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
            ClearSelection();
        }

        private void ClearSelection()
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

        private void LayerEnabled_Checked(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            context.ReloadLayout();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearSelection();
            context.SelectedLayer = (IBindingsLayer)LayersListBox.SelectedItem;
        }

        private void AddLayer_Click(object sender, RoutedEventArgs e)
        {
            context.AddLayer();
            LayersListBox.SelectedIndex = 0;
            ClearSelection();
            context.ReloadLayout();
        }

        private void RenameLayer_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)LayersListBox.ItemContainerGenerator.ContainerFromItem(context.SelectedLayer);
            ContentPresenter presenter = FindVisualChild<ContentPresenter>(selectedItem);
            DataTemplate template = presenter.ContentTemplate;
            TextBox layerName = (TextBox)template.FindName("LayerName", presenter);
            layerName.IsEnabled = true;
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

        private void MoveLayerUp_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            var current = context.SelectedLayer;
            if (context.LayersContext is not IList list) return;
            int index = list.IndexOf(current);
            context.SwapLayers(index, -1);
            context.ReloadLayout();
            LayersListBox.SelectedIndex = index - 1;
        }

        private void MoveLayerDown_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            var current = context.SelectedLayer;
            if (context.LayersContext is not IList list) return;
            int index = list.IndexOf(current);
            context.SwapLayers(index, 1);
            context.ReloadLayout();
            LayersListBox.SelectedIndex = index + 1;
        }

        private void RemoveLayer_Click(object sender, RoutedEventArgs e)
        {
            var current = context.SelectedLayer;
            if (context.LayersContext is not IList list) return;
            int index = list.IndexOf(current);
            context.DeleteLayer(index);
            context.ReloadLayout();
            LayersListBox.SelectedIndex = 0;
        }

        private void LayerName_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).IsEnabled = false;
        }

        private void LayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Kill logical focus
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope((TextBox)sender), null);
                // Kill keyboard focus
                Keyboard.ClearFocus();
            }
        }
    }
}