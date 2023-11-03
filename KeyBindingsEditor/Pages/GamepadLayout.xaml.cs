using KeyBindingsEditor.Configuration;
using KeyBindingsEditor.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeyBindingsEditor.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamepadLayout.xaml
    /// </summary>
    public partial class GamepadLayout : GamepadPageBase
    {
        public GamepadLayout()
        {
            InitializeComponent();
            Initialize();
        }

        protected internal override EditorInputType EditorInputType => EditorInputType.Gamepad;

        internal override IEnumerable<IBindingsLayer> Layers => Editor.Configuration.Gamepad.Layers;

        public void GamepadKey_Click(object sender, RoutedEventArgs e)
        {
            OnKeyClick(sender);
        }

        protected override Dictionary<GamepadButtons, (Button Button, string Text)> FillButtons()
        {
            return new()
            {
                [GamepadButtons.A] = (A, "A"),
                [GamepadButtons.B] = (B, "B"),
                [GamepadButtons.X] = (X, "X"),
                [GamepadButtons.Y] = (Y, "Y"),
                [GamepadButtons.Back] = (Back_Button, "BACK"),
                [GamepadButtons.Start] = (Start_Button, "START"),
                [GamepadButtons.LeftShoulder] = (Left_Button, "LB"),
                [GamepadButtons.LeftTrigger] = (Left_Trigger, "LT"),
                [GamepadButtons.RightShoulder] = (Right_Button, "RB"),
                [GamepadButtons.RightTrigger] = (Right_Trigger, "RT"),
                [GamepadButtons.LeftThumb] = (Left_Stick, "LS"),
                [GamepadButtons.RightThumb] = (Right_Stick, "RS"),
                [GamepadButtons.PadRight] = (Right_DPad, "→"),
                [GamepadButtons.PadLeft] = (Left_DPad, "←"),
                [GamepadButtons.PadUp] = (Up_DPad, "↑"),
                [GamepadButtons.PadDown] = (Down_DPad, "↓"),
            };
        }
    }
}