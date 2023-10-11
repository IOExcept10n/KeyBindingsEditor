using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class KeyboardConfiguration
    {
        public List<KeyBinding<Keys>> Bindings { get; set; }
    }
}
