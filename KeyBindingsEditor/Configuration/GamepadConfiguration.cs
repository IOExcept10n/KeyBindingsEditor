using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class GamepadConfiguration
    {
        public List<KeyBinding<GamepadButtons>> Bindings { get; set; }
    }
}
