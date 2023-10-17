using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (binding.ClickAction != null)
            {
                var category = binding.ClickAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    background = category.Color;
                }
            }
            target.Background = new SolidColorBrush(background);
            target.ClipToBounds = false;
            Grid grid = new()
            {
                ClipToBounds = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = target.Width, 
                Height = target.Height
            };
            Color foreground = binding.ClickAction == null ? Colors.White : background.GetContrast();
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
                var category = binding.DoubleClickAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
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
                var category = binding.HoldAction.GetCategory(editor.Configuration.CategoryManager);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
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
                Rectangle notification = new()
                {
                    Fill = new SolidColorBrush(Colors.Orange),
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
        }

        public static void VisualizeLayout<T>(Dictionary<T, (Button Button, string Text)> buttons, EditorViewModel editor) where T : notnull
        {
            foreach (var button in buttons)
            {
                var btn = button.Value.Button;
                btn.Content = button.Value.Text;
                btn.Background = new SolidColorBrush(Color.FromArgb(0x88, 50, 50, 50));
                btn.Foreground = new SolidColorBrush(Colors.White);
                btn.IsEnabled = !editor.CurrentCombinationContains(button.Key);
                btn.Padding = new Thickness();
            }
            foreach (var binding in editor.BindingsContext)
            {
                var keyBinding = binding as KeyBinding<T>;
                if (keyBinding != null && buttons.TryGetValue(keyBinding.Key, out var button))
                {
                    ApplyButtonLayout(button.Button, button.Text, keyBinding, editor);
                }
            }
        }
    }
}
