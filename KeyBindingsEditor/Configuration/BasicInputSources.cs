using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    public class GamepadConfiguration : InputSourceConfigurationBase<GamepadButtons>
    {
    }

    public class MouseConfiguration : InputSourceConfigurationBase<MouseButtons>
    {
    }

    public class KeyboardConfiguration : InputSourceConfigurationBase<Keys>
    {
    }
}
