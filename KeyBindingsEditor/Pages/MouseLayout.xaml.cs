using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для MouseLayout.xaml
    /// </summary>
    public partial class MouseLayout : MousePageBase
    {
        public MouseLayout()
        {
            InitializeComponent();
            Initialize();
        }

        internal override IEnumerable<IBindingsLayer> Layers => Editor.Configuration.Mouse.Layers;

        protected internal override EditorInputType EditorInputType => EditorInputType.Mouse;

        protected override Dictionary<MouseButtons, (Button Button, string Text)> FillButtons()
        {
            return new()
            {
                [MouseButtons.Left] = (Mouse_1, "LMB"),
                [MouseButtons.Right] = (Mouse_2, "RMB"),
                [MouseButtons.Middle] = (Mouse_3, "M"),
                [MouseButtons.Extended1] = (Mouse_4, "Mouse4"),
                [MouseButtons.Extended2] = (Mouse_5, "Mouse5")
            };
        }

        public void Key_Click(object sender, RoutedEventArgs e)
        {
            OnKeyClick(sender);
        }
    }
}