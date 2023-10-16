using KeyBindingsEditor.Configuration;
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
        public void ApplyButtonLayout<T>(Button target, KeyBinding<T> binding, CategoryManager categories)
        {
            var backgroundBrush = new SolidColorBrush(Color.FromArgb(0x88, 50, 50, 50));
            if (binding.ClickAction != null)
            {
                var category = binding.ClickAction.GetCategory(categories);
                if (category != null)
                {
                    backgroundBrush = new SolidColorBrush(category.Color);
                }
            }
            target.Background = backgroundBrush;
            target.ClipToBounds = false;
            Grid grid = new()
            {
                ClipToBounds = false
            };
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

        public void VisualizeLayout<T>(Dictionary<T, Button> buttons, InputSourceConfigurationBase<T> layout, CategoryManager categories) where T : notnull
        {
            foreach (var button in buttons)
            {
                button.Value.Background = new SolidColorBrush(Color.FromArgb(0x88, 50, 50, 50));
            }
            foreach (var binding in layout.Bindings)
            {
                if (buttons.TryGetValue(binding.Key, out var button))
                {
                    ApplyButtonLayout(button, binding, categories);
                }
            }
        }
    }
}
