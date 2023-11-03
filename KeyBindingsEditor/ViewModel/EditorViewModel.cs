using KeyBindingsEditor.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace KeyBindingsEditor.ViewModel
{
    public enum EditorInputType
    {
        None,
        Keyboard,
        Mouse,
        Gamepad
    }

    /// <summary>
    /// Represents the view model for the editor.
    /// </summary>
    internal class EditorViewModel : INotifyPropertyChanged
    {
        private string? filePath;
        private bool hasUnsavedChanges;
        private IKeyBinding? selectedBinding;
        private InputConfiguration configuration = null!;
        private IEnumerable<IKeyBinding>? bindingsContext;
        private EditorInputType currentEditorType;
        private IKeyBinding? combinationSource;
        private IKeyBinding? sequenceThird;
        private IKeyBinding? sequenceFirst;
        private IKeyBinding? sequenceSecond;
        private IEnumerable<IBindingsLayer>? layersContext;
        private IBindingsLayer? selectedLayer;

        public static EditorViewModel Instance { get; set; } = null!;

        /// <summary>
        /// The configuration loaded into an editor.
        /// </summary>
        public InputConfiguration Configuration
        {
            get => configuration;
            set
            {
                configuration = value;
                if (configuration != null)
                {
                    configuration.PropertyChanged += Configuration_PropertyChanged;
                }
                OnPropertyChanged(nameof(Configuration));
            }
        }

        /// <summary>
        /// The selected binding to vizualize.
        /// </summary>
        public IKeyBinding? SelectedBinding
        {
            get => selectedBinding;
            set
            {
                selectedBinding = value;
                OnPropertyChanged(nameof(SelectedBinding));
            }
        }

        public IBindingsLayer? SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                if (combinationSource == null)
                {
                    BindingsContext = selectedLayer?.Bindings;
                }
                OnPropertyChanged(nameof(SelectedLayer));
            }
        }

        /// <summary>
        /// The first key combination binding selected in the current view.
        /// </summary>
        public IKeyBinding? SequenceFirst
        {
            get => sequenceFirst;
            set
            {
                sequenceFirst = value;
                OnPropertyChanged(nameof(SequenceFirst));
            }
        }

        /// <summary>
        /// The second key combination binding selected in the current view.
        /// </summary>
        public IKeyBinding? SequenceSecond
        {
            get => sequenceSecond;
            set
            {
                sequenceSecond = value;
                OnPropertyChanged(nameof(SequenceSecond));
            }
        }

        /// <summary>
        /// The third key combination binding selected in the current view.
        /// </summary>
        public IKeyBinding? SequenceThird
        {
            get => sequenceThird;
            set
            {
                sequenceThird = value;
                OnPropertyChanged(nameof(SequenceThird));
            }
        }

        /// <summary>
        /// The source of the combination to visualize input context.
        /// </summary>
        public IKeyBinding? CombinationSource
        {
            get => combinationSource;
            set
            {
                combinationSource = value;
                if (combinationSource != null)
                {
                    BindingsContext = combinationSource.NextBindings;
                }
                else
                {
                    LayersContext = currentEditorType switch
                    {
                        EditorInputType.Keyboard => Configuration.Keyboard.Layers,
                        EditorInputType.Mouse => Configuration.Mouse.Layers,
                        EditorInputType.Gamepad => Configuration.Gamepad.Layers,
                        _ => null,
                    };
                }
                OnPropertyChanged(nameof(CombinationSource));
            }
        }

        public IEnumerable<IBindingsLayer>? LayersContext
        {
            get => layersContext;
            set
            {
                layersContext = value;
                if (combinationSource == null)
                {
                    BindingsContext = SelectedLayer?.Bindings ?? layersContext?.First().Bindings;
                }
                OnPropertyChanged(nameof(LayersContext));
            }
        }

        /// <summary>
        /// The context of the input device to visualize.
        /// </summary>
        public IEnumerable<IKeyBinding>? BindingsContext
        {
            get => bindingsContext;
            set
            {
                bindingsContext = value;
                OnPropertyChanged(nameof(BindingsContext));
            }
        }

        /// <summary>
        /// Determines whether the editor has unsaved changes.
        /// </summary>
        public bool HasUnsavedChanges
        {
            get => hasUnsavedChanges;
            set
            {
                hasUnsavedChanges = value;
                OnPropertyChanged(nameof(HasUnsavedChanges));
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        /// <summary>
        /// The title of the window to display.
        /// </summary>
        public string WindowTitle => (FilePath ?? "Untitled Configuration") + (HasUnsavedChanges ? "*" : "");

        /// <summary>
        /// The path to the opened file to display and save into.
        /// </summary>
        public string? FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        /// <summary>
        /// Current editor input source selected for the display.
        /// </summary>
        public EditorInputType CurrentEditorType
        {
            get => currentEditorType;
            set
            {
                currentEditorType = value;
                OnPropertyChanged(nameof(CurrentEditorType));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler? ReloadTrigger;

        public EditorViewModel()
        {
            CreateConfiguration();
        }

        public void AddLayer()
        {
            if (LayersContext != null)
            {
                switch (CurrentEditorType)
                {
                    case EditorInputType.Keyboard:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<Keys>>;
                            keyLayers?.Insert(0, new());
                        }
                        break;
                    case EditorInputType.Mouse:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<MouseButtons>>;
                            keyLayers?.Insert(0, new());
                        }
                        break;
                    case EditorInputType.Gamepad:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<GamepadButtons>>;
                            keyLayers?.Insert(0, new());
                        }
                        break;
                }
                SelectedLayer = LayersContext.First();
            }
        }

        public void SwapLayers(int layerPosition, int shift)
        {
            if (LayersContext != null)
            {
                switch (CurrentEditorType)
                {
                    case EditorInputType.Keyboard:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<Keys>>;
                            SwapLayers(keyLayers, layerPosition, shift);
                        }
                        break;
                    case EditorInputType.Mouse:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<MouseButtons>>;
                            SwapLayers(keyLayers, layerPosition, shift);
                        }
                        break;
                    case EditorInputType.Gamepad:
                        {
                            var keyLayers = LayersContext as ObservableCollection<BindingsLayer<GamepadButtons>>;
                            SwapLayers(keyLayers, layerPosition, shift);
                        }
                        break;
                }
            }
        }
        
        private static void SwapLayers<T>(ObservableCollection<BindingsLayer<T>>? layers, int position, int shift)
        {
            if (layers == null || position < 0 || position >= layers.Count || position + shift < 0 || position + shift >= layers.Count)
                return;
            (layers[position], layers[position + shift]) = (layers[position + shift], layers[position]);
        }

        public void DeleteLayer(int index)
        {
            if (LayersContext == null || index < 0 || index >= LayersContext.Count())
                return;
            switch (CurrentEditorType)
            {
                case EditorInputType.Keyboard:
                    {
                        (LayersContext as ObservableCollection<BindingsLayer<Keys>>)?.RemoveAt(index);
                    }
                    break;
                case EditorInputType.Mouse:
                    {
                        (LayersContext as ObservableCollection<BindingsLayer<MouseButtons>>)?.RemoveAt(index);
                    }
                    break;
                case EditorInputType.Gamepad:
                    {
                        (LayersContext as ObservableCollection<BindingsLayer<GamepadButtons>>)?.RemoveAt(index);
                    }
                    break;
            }
        }

        public void ReloadLayout()
        {
            ReloadTrigger?.Invoke(this, EventArgs.Empty);
        }

        public bool CurrentCombinationContains<T>(T keys)
        {
            return (SequenceFirst as KeyBinding<T>)?.Key?.Equals(keys) == true ||
                   (SequenceSecond as KeyBinding<T>)?.Key?.Equals(keys) == true ||
                   (SequenceThird as KeyBinding<T>)?.Key?.Equals(keys) == true;
        }

        public bool CreateConfiguration()
        {
            if (SaveIfUnsaved())
            {
                SelectedBinding = SequenceFirst = SequenceSecond = SequenceThird = CombinationSource = null;
                Configuration = new();
                HasUnsavedChanges = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Opens the input configuration file.
        /// </summary>
        /// <returns><see langword="true"/> if the file has been opened successfully, <see langword="false"/> otherwise.</returns>
        public bool OpenConfiguration()
        {
            OpenFileDialog dialog = new()
            {
                DefaultExt = "keymap",
                Title = "Select the input configuration file",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "JSON input configuration file|*.keymap|JSON files|*.json|All files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                Configuration = InputConfiguration.LoadFrom(dialog.FileName);
                HasUnsavedChanges = false;
                FilePath = dialog.FileName;
                return true;
            }
            return false;
        }

        public bool ExportConfiguration()
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "json",
                OverwritePrompt = true,
                Title = "Save the categories",
                Filter = "JSON categories configuration file|*.json|All files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(Configuration.Categories, Formatting.Indented));
                return true;
            }
            else return false;
        }

        public bool ImportConfiguration()
        {
            OpenFileDialog dialog = new()
            {
                DefaultExt = "json",
                Title = "Select the categories configuration file",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "JSON categories configuration file|*.json|All files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                Configuration.Categories = JsonConvert.DeserializeObject<ObservableCollection<GameplayCategory>>(File.ReadAllText(dialog.FileName))!;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves the world if it has unsaved changes.
        /// </summary>
        /// <returns><see langword="true"/> if the actions that asks user to save should be continued,
        /// <see langword="false"/> if user cancels the action until the fle will be saved.</returns>
        public bool SaveIfUnsaved()
        {
            if (!HasUnsavedChanges) return true;
            var result = MessageBox.Show("Your configuration has unsaved changes. Do you want to save them?", "Save changes", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                return Save();
            }
            else if (result == MessageBoxResult.No)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves the world into the last selected file or new file if it isn't selected.
        /// </summary>
        /// <returns><see langword="true"/> if the file was saved, <see langword="false"/> otherwise.</returns>
        public bool Save()
        {
            if (FilePath != null)
            {
                Configuration.Save(FilePath);
                HasUnsavedChanges = false;
                return true;
            }
            return SaveAs();
        }

        /// <summary>
        /// Saves the world as the new file.
        /// </summary>
        /// <returns><see langword="true"/> if the file was saved, <see langword="false"/> otherwise.</returns>
        public bool SaveAs()
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "keymap",
                OverwritePrompt = true,
                Title = "Save the configuration",
                Filter = "JSON input configuration file|*.keymap|JSON files|*.json|All files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                Configuration.Save(dialog.FileName);
                FilePath = dialog.FileName;
                HasUnsavedChanges = false;
                return true;
            }
            else return false;
        }

        public bool CreateMarkdown()
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "md",
                OverwritePrompt = true,
                Title = "Save the table",
                Filter = "Markdown file|*.md|All files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                StringBuilder markdown = new();
                markdown
                    .AppendLine("# Input configuration")
                    .AppendLine()
                    .AppendLine($"*Made with KeyBindingsEditor v.{Assembly.GetExecutingAssembly().GetName().Version}*")
                    .AppendLine();
                // Add keyboard definition.
                markdown
                    .AppendLine("## Keyboard")
                    .AppendLine()
                    .AppendLine("---")
                    .AppendLine()
                    .AppendLine("|Name|Binding Type|Key|Category|Title|Description|")
                    .AppendLine("|---|---|:---:|---|---|---|");
                MarkupLayer(markdown, Configuration.Keyboard.Layers);
                markdown.AppendLine();
                // Add mouse definition.
                markdown
                    .AppendLine("## Mouse")
                    .AppendLine()
                    .AppendLine("---")
                    .AppendLine()
                    .AppendLine("|Name|Binding Type|Key|Category|Title|Description|")
                    .AppendLine("|---|---|:---:|---|---|---|");
                MarkupLayer(markdown, Configuration.Mouse.Layers);
                markdown.AppendLine();
                // Add gamepad definition.
                markdown
                    .AppendLine("## Gamepad")
                    .AppendLine()
                    .AppendLine("---")
                    .AppendLine()
                    .AppendLine("|Name|Binding Type|Key|Category|Title|Description|")
                    .AppendLine("|---|---|:---:|---|---|---|");
                MarkupLayer(markdown, Configuration.Gamepad.Layers);
                markdown.AppendLine();
                File.WriteAllText(dialog.FileName, markdown.ToString());
                return true;
            }
            else return false;
        }
        
        private static void MarkupLayer<T>(StringBuilder markdown, IEnumerable<BindingsLayer<T>> layers)
        {
            foreach (var layer in layers)
            {
                MarkupSource(markdown, layer.Bindings);
                markdown.AppendLine("| --- | --- | --- | --- | --- | --- |");
            }
        }

        private static void MarkupSource<T>(StringBuilder markdown, IEnumerable<KeyBinding<T>> bindings)
        {
            foreach (var binding in bindings)
            {
                if (binding.ClickAction != null && !binding.ClickAction.IsDeleted)
                    AppendAction(markdown, binding.ClickAction, binding.KeysSequence, "Click");
                if (binding.HoldAction != null && !binding.HoldAction.IsDeleted)
                    AppendAction(markdown, binding.HoldAction, binding.KeysSequence, "Hold");
                if (binding.DoubleClickAction != null && !binding.DoubleClickAction.IsDeleted)
                    AppendAction(markdown, binding.DoubleClickAction, binding.KeysSequence, "DoubleClick");
                if (binding.SequenceNextBindings.Any())
                {
                    MarkupSource(markdown, binding.SequenceNextBindings);
                }
            }
        }

        private static void AppendAction(StringBuilder builder, ActionInfo action, string keys, string type)
        {
            builder
                .Append('|').Append(action.Name)
                .Append('|').Append(type)
                .Append('|').Append(keys)
                .Append('|').Append('#').Append(action.Category)
                .Append('|').Append(action.Title)
                .Append('|').Append(action.Description).AppendLine("|");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        private void Configuration_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            HasUnsavedChanges = true;
        }
    }
}