using System.Collections.Generic;
using System.ComponentModel;

namespace KeyBindingsEditor.Configuration
{
    internal interface IKeyBinding : INotifyPropertyChanged
    {
        public IKeyBinding? Parent { get; }

        public ActionInfo? ClickAction { get; set; }
        public ActionInfo? HoldAction { get; set; }
        public ActionInfo? DoubleClickAction { get; set; }

        public IEnumerable<IKeyBinding> NextBindings { get; }
    }
}