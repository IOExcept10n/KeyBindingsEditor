using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class KeyBinding<TKeyType>
    {
        public TKeyType Key { get; set; }

        public ActionInfo ClickAction { get; set; }

        public ActionInfo HoldAction { get; set; }

        public List<ActionInfo> NClickActions { get; set; }

        public List<KeyBinding<TKeyType>> SequenceNextBindings { get; set; }
    }
}
