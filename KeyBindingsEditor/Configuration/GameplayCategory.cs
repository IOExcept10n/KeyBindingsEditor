using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeyBindingsEditor.Configuration
{
    public class GameplayCategory
    {
        public Color Color { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ActionInfo> Actions { get; set; } = new();
    }

    public struct ActionInfo
    {
        public static ActionInfo Empty { get; } = default;

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
