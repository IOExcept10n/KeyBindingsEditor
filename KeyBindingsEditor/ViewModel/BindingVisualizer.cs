using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KeyBindingsEditor.ViewModel
{
    internal static class BindingVisualizer
    {
        public static void ApplyButtonLayout<T>(Button target, string text, KeyBinding<T> binding, EditorViewModel editor)
        {
            Color background = Color.FromArgb(0x88, 50, 50, 50);
            string bindingDesc = "";
            if (binding.ClickAction != null)
            {
                bindingDesc += $"Click: {binding.ClickAction.Title};\n";
                var category = binding.ClickAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    background = category.Color;
                }
            }
            target.Background = new SolidColorBrush(background);
            target.ClipToBounds = false;
            if (editor.SelectedBinding == binding)
            {
                target.BorderThickness = new Thickness(1);
                target.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                target.BorderThickness = default;
            }
            Grid grid = new()
            {
                ClipToBounds = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = target.Width,
                Height = target.Height
            };
            Color foreground = binding.ClickAction == null ? Colors.White : background.GetContrast(60);
            grid.Children.Add(new TextBlock()
            {
                Text = text,
                //FontSize = 20,
                Foreground = new SolidColorBrush(foreground),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            });
            if (binding.DoubleClickAction != null)
            {
                bindingDesc += $"Double click: {binding.DoubleClickAction.Title};\n";
                var category = binding.DoubleClickAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1,
                        Height = 10,
                        Width = 10,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    grid.Children.Add(notification);
                }
            }
            if (binding.HoldAction != null)
            {
                bindingDesc += $"Double click: {binding.HoldAction.Title};\n";
                var category = binding.HoldAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1,
                        Height = 10,
                        Width = 10,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Right,
                    };
                    grid.Children.Add(notification);
                }
            }
            if (binding.SequenceNextBindings.Count > 0)
            {
                bindingDesc += $"Next key combinations: {binding.SequenceNextBindings.Count};\n";
                Rectangle notification = new()
                {
                    Fill = new SolidColorBrush(Colors.Orange),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    Height = 10,
                    Width = 20,
                    RadiusX = 5,
                    RadiusY = 5,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                grid.Children.Add(notification);
                TextBlock notificationText = new()
                {
                    Text = binding.SequenceNextBindings.Count.ToString(),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 8,
                    Margin = new Thickness(6, 0, 0, 0)
                };
                grid.Children.Add(notificationText);
            }
            target.Content = grid;
            target.ToolTip = bindingDesc.Trim('\n', ';');
        }

        public static void VisualizeLayout<T>(Dictionary<T, (Button Button, string Text)> buttons, EditorViewModel editor) where T : notnull
        {
            foreach (var button in buttons)
            {
                var btn = button.Value.Button;
                btn.Content = button.Value.Text;
                btn.Background = new SolidColorBrush(Color.FromArgb(0x88, 50, 50, 50));
                btn.Foreground = new SolidColorBrush(Colors.White);
                btn.IsEnabled = !editor.CurrentCombinationContains(button.Key) && editor.LayersContext?.Any(x => x.Enabled) == true;
                btn.Padding = new Thickness();
                btn.ToolTip = null;
            }
            if (editor.CombinationSource != null)
            {
                foreach (var binding in editor.BindingsContext!)
                {
                    if (binding is KeyBinding<T> keyBinding && buttons.TryGetValue(keyBinding.Key, out var button))
                    {
                        ApplyButtonLayout(button.Button, button.Text, keyBinding, editor);
                    }
                }
            }
            else if (editor.LayersContext != null)
            {
                // Reverse layers to draw the first layer as the top-layer.
                foreach (var layer in editor.LayersContext.Reverse())
                {
                    if (layer.Enabled)
                        foreach (var binding in layer.Bindings)
                        {
                            if (binding is KeyBinding<T> keyBinding && buttons.TryGetValue(keyBinding.Key, out var button))
                            {
                                ApplyButtonLayout(button.Button, button.Text, keyBinding, editor);
                            }
                        }
                }
            }
        }
    }
}