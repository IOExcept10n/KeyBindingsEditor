using KeyBindingsEditor.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyBindingsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // HACK:
            var categories = new List<GameplayCategory>()
            {
                new()
                {
                    Name = "Movement",
                    Color = Colors.LightBlue,
                    Actions = new()
                    {
                        new() { Name="MoveForward" },
                        new() { Name = "MoveBackward" },
                        new() { Name = "MoveRight" },
                        new() { Name = "MoveLeft" },
                        new() { Name = "Jump" }
                    }
                },
                new()
                {
                    Name = "Fight",
                    Color = Colors.OrangeRed,
                    Actions = new()
                    {
                        new() { Name = "Hit" },
                        new() { Name = "ComboHit" },
                        new() { Name = "Shot" },
                        new() { Name = "CastSpell" }
                    }
                },
                new()
                {
                    Name = "Skills",
                    Color = Colors.BlueViolet,
                    Actions = new()
                    {
                        new() { Name = "Heal" },
                        new() { Name = "UseManaPotion" },
                    }
                },
                new()
                {
                    Name = "Menus",
                    Color = Colors.Green,
                    Actions = new()
                    {
                        new() { Name = "OpenPause" },
                        new() { Name = "OpenInventory" }
                    }
                }
            };
            var manager = new CategoryManager(categories);
            InputConfiguration config = new()
            {
                Categories = categories,
                Keyboard = new()
                {
                    Bindings = new()
                    {
                        new KeyBinding<Keys>()
                        {
                           Key = Keys.W,
                           ClickAction = manager.GetAction("Movement", "MoveForward")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.A,
                            ClickAction = manager.GetAction("Movement", "MoveLeft")
                        },
                        new KeyBinding<Keys>()
                        {
                           Key = Keys.S,
                           ClickAction = manager.GetAction("Movement", "MoveForward")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.D,
                            ClickAction = manager.GetAction("Movement", "MoveRight")
                        },
                        new KeyBinding<Keys>()
                        {
                           Key = Keys.Space,
                           ClickAction = manager.GetAction("Movement", "Jump")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.F,
                            ClickAction = manager.GetAction("Skills", "Heal")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.G,
                            ClickAction = manager.GetAction("Skills", "UseManaPotion")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.E,
                            ClickAction = manager.GetAction("Menus", "OpenPause")
                        },
                        new KeyBinding<Keys>()
                        {
                            Key = Keys.Escape,
                            ClickAction = manager.GetAction("Menus", "OpenInventory")
                        },
                    }
                },
                Mouse = new()
                {
                    Bindings = new()
                    {
                        new KeyBinding<MouseButtons>() 
                        {
                            Key = MouseButtons.Left,
                            ClickAction = manager.GetAction("Fight", "Hit"),
                            HoldAction = manager.GetAction("Fight", "Shot"),
                            NClickActions = new()
                            {
                                default,
                                default,
                                manager.GetAction("Fight", "ComboHit")
                            },
                        },
                        new KeyBinding<MouseButtons>()
                        {
                            Key = MouseButtons.Right,
                            ClickAction = manager.GetAction("Fight", "CastSpell")
                        }
                    }
                },
                Gamepad = new()
                {
                    Bindings = new()
                    {
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.PadUp,
                            ClickAction = manager.GetAction("Movement", "MoveForward")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.PadDown,
                            ClickAction = manager.GetAction("Movement", "MoveBackward")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.PadLeft,
                            ClickAction = manager.GetAction("Movement", "MoveLeft")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.PadRight,
                            ClickAction = manager.GetAction("Movement", "MoveRight")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.RightShoulder,
                            ClickAction = manager.GetAction("Fight", "Hit")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.LeftShoulder,
                            ClickAction = manager.GetAction("Fight", "CastSpell")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.Start,
                            ClickAction = manager.GetAction("Menus", "OpenPause")
                        },
                        new KeyBinding<GamepadButtons>()
                        {
                            Key = GamepadButtons.A,
                            ClickAction = manager.GetAction("Menus", "OpenInventory")
                        },
                    }
                }
            };
            System.IO.File.WriteAllText("TestBindings.json", JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }
}
