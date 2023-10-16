using KeyBindingsEditor.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyBindingsEditor.ViewModel
{
    internal class EditorViewModel : INotifyPropertyChanged
    {
        private string? filePath;
        private bool hasUnsavedChanges;
        private IKeyBinding? selectedBinding;
        private InputConfiguration configuration = new();

        public static EditorViewModel Instance { get; set; }

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

        public IKeyBinding? SelectedBinding
        {
            get => selectedBinding;
            set
            {
                selectedBinding = value;
                OnPropertyChanged(nameof(SelectedBinding));
            }
        }

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

        public string WindowTitle => (FilePath ?? "Untitled Configuration") + (HasUnsavedChanges ? "*" : "");

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

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Opens the input configuration file.
        /// </summary>
        /// <returns><see langword="true"/> if the file has been opened successfully, <see langword="false"/> otherwise.</returns>
        public bool OpenConfiguration()
        {
            OpenFileDialog dialog = new()
            {
                DefaultExt = "json",
                Title = "Select the input configuration file",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "JSON input configuration file|*.json|All files|*.*"
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
                DefaultExt = "json",
                OverwritePrompt = true,
                Title = "Save the configuration",
                Filter = "JSON input configuration file|*.json|All files|*.*"
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
