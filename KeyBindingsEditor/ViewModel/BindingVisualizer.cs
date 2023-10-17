using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KeyBindingsEditor.ViewModel
{
    internal class BindingVisualizer
    {
        public void ApplyButtonLayout<T>(Button target, string text, KeyBinding<T> binding, CategoryManager categories)
        {
            Color background = Color.FromArgb(0x88, 50, 50, 50);
            if (binding.ClickAction != null)
            {
                var category = binding.ClickAction.GetCategory(categories);
                if (category != null)
                {
                    background = category.Color;
                }
            }
            target.Background = new SolidColorBrush(background);
            target.ClipToBounds = false;
            Grid grid = new()
            {
                ClipToBounds = false
            };
            grid.Children.Add(new TextBlock()
            {
                Text = text,
                //FontSize = 20,
                Foreground = new SolidColorBrush(background.GetContrast())
            });
            if (binding.DoubleClickAction != null)
            {
                var category = binding.DoubleClickAction.GetCategory(categories);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
                        Height = 10,
                        Width = 10,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Right
                    };
                    grid.Children.Add(notification);
                }
            }
            if (binding.HoldAction != null) 
            {
                var category = binding.HoldAction.GetCategory(categories);
                if (category != null)
                {
                    Ellipse notification = new()
                    {
                        Fill = new SolidColorBrush(category.Color),
                        Height = 10,
                        Width = 10,
                        VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Right
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
                    Width = 40,
                    RadiusX = 15,
                    RadiusY = 15,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                };
                grid.Children.Add(notification);
            }
            target.Content = grid;
        }

        public void VisualizeLayout<T>(Dictionary<T, (Button Button, string Text)> buttons, EditorViewModel editor, CategoryManager categories) where T : notnull
        {
            foreach (var button in buttons)
            {
                button.Value.Button.Background = new SolidColorBrush(Color.FromArgb(0x88, 50, 50, 50));
            }
            foreach (var binding in editor.BindingsContext)
            {
                var keyBinding = binding as KeyBinding<T>;
                if (keyBinding != null && buttons.TryGetValue(keyBinding.Key, out var button))
                {
                    ApplyButtonLayout(button.Button, button.Text, keyBinding, categories);
                }
            }
        }
    }
}
