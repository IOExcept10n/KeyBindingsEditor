﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace KeyBindingsEditor.Utils
{
    /// <summary>
    /// Captures and eats MouseWheel events so that a nested ListBox does not
    /// prevent an outer scrollable control from scrolling.
    /// </summary>
    internal class IgnoreMouseWheelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            base.OnDetaching();
        }
        void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            AssociatedObject.RaiseEvent(e2);
        }
    }
}
