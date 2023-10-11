using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class InputConfiguration
    {
        public KeyboardConfiguration Keyboard { get; set; } = new();

        public MouseConfiguration Mouse { get; set; } = new();

        public GamepadConfiguration Gamepad { get; set; } = new();

        public List<GameplayCategory> Categories { get; set; } = new();
    }
}
