using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;
using TextureAtlasLib;

namespace Assignment3
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _projectName = "SpriteSheet";
        private string _outputDirectory;
        private string _outputFile = "SpriteSheet.png";
        private int _columns = 6;
        private bool _includeMetaData;
        private string _currentFilePath;
        private bool _isDirty = false;
        private bool _isGenerating = false;

        public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

        // Undo/Redo (Bonus)
        private Stack<List<string>> _undoStack = new Stack<List<string>>();
        private Stack<List<string>> _redoStack = new Stack<List<string>>();

        public MainViewModel()
        {
            BrowseCommand = new RelayCommand(BrowseOutput);
            AddCommand = new RelayCommand(AddImages);
            RemoveCommand = new RelayCommand(RemoveImage, (o) => SelectedImage != null);
            GenerateCommand = new RelayCommand(GenerateSpriteSheet, CanGenerate);

            NewCommand = new RelayCommand(NewProject);
            OpenCommand = new RelayCommand(OpenProject);
            SaveCommand = new RelayCommand(SaveProject, (o) => !string.IsNullOrEmpty(_currentFilePath) || _isDirty);
            SaveAsCommand = new RelayCommand(SaveProjectAs);
            ExitCommand = new RelayCommand(ExitApp);

            // Edit Menu Commands (Bonus)
            UndoCommand = new RelayCommand(Undo, (o) => _undoStack.Count > 0);
            RedoCommand = new RelayCommand(Redo, (o) => _redoStack.Count > 0);
            RemoveAllCommand = new RelayCommand((o) => { RecordState(); Images.Clear(); });
        }

        public string ProjectNameTitle => $"{_projectName}.xml" + (_isDirty ? "*" : "");

        public string OutputDirectory
        {
            get => _outputDirectory;
            set { _outputDirectory = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanGenerate)); }
        }

        public string OutputFile
        {
            get => _outputFile;
            set { _outputFile = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanGenerate)); }
        }

        public int Columns
        {
            get => _columns;
            set { _columns = value; OnPropertyChanged(); }
        }

        public bool IncludeMetaData
        {
            get => _includeMetaData;
            set { _includeMetaData = value; OnPropertyChanged(); }
        }

        public bool IsGenerating
        {
            get => _isGenerating;
            set { _isGenerating = value; OnPropertyChanged(); }
        }

        public string SelectedImage { get; set; }

        public ICommand BrowseCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand GenerateCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand RemoveAllCommand { get; }

        private void BrowseOutput(object obj)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = OutputFile;
            if (sfd.ShowDialog() == true)
            {
                OutputDirectory = Path.GetDirectoryName(sfd.FileName);
                OutputFile = Path.GetFileName(sfd.FileName);
            }
        }

        private void AddImages(object obj)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg";

            if (ofd.ShowDialog() == true)
            {
                RecordState(); // Save for Undo
                foreach (string file in ofd.FileNames)
                {
                    if (!Images.Contains(file))
                    {
                        Images.Add(file);
                    }
                }
                _isDirty = true;
                OnPropertyChanged(nameof(ProjectNameTitle));
            }
        }

        private void RemoveImage(object obj)
        {
            if (SelectedImage != null)
            {
                RecordState(); // Save for Undo
                Images.Remove(SelectedImage);
                _isDirty = true;
                OnPropertyChanged(nameof(ProjectNameTitle));
            }
        }

        private bool CanGenerate(object obj)
        {
            // Validation Logic (Bonus)
            return !string.IsNullOrEmpty(OutputDirectory) && !string.IsNullOrEmpty(OutputFile) && Images.Count > 0 &&
                   Columns > 0;
        }

        private async void GenerateSpriteSheet(object obj)
        {
            IsGenerating = true;
            try
            {
                await Task.Run(() =>
                {
                    Spritesheet sheet = new Spritesheet
                    {
                        InputPaths = Images.ToList(),
                        OutputDirectory = OutputDirectory,
                        OutputFile = OutputFile,
                        Columns = Columns,
                        IncludeMetaData = IncludeMetaData
                    };
                    sheet.Generate(true);
                });

                var result = MessageBox.Show("Generated successfully! Would you like to view the output?",
                    "Success", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    if (Directory.Exists(OutputDirectory))
                    {
                        Process.Start("explorer.exe", OutputDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating spritesheet: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsGenerating = false;
            }
        }

        // --- File Menu Logic ---

        private void NewProject(object obj)
        {
            if (CheckUnsavedChanges())
            {
                Images.Clear();
                OutputDirectory = "";
                OutputFile = "SpriteSheet.png";
                Columns = 6;
                IncludeMetaData = false;
                _projectName = "Untitled";
                _currentFilePath = null;
                _isDirty = false;
                _undoStack.Clear();
                _redoStack.Clear();
                OnPropertyChanged(nameof(ProjectNameTitle));
            }
        }

        private void OpenProject(object obj)
        {
            if (!CheckUnsavedChanges())
            {
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SpriteSheetProject));
                    using (StreamReader reader = new StreamReader(ofd.FileName))
                    {
                        var project = (SpriteSheetProject)serializer.Deserialize(reader);

                        OutputDirectory = project.OutputDirectory;
                        OutputFile = project.OutputFile;
                        Columns = project.Columns;
                        IncludeMetaData = project.IncludeMetaData;

                        Images.Clear();
                        List<string> missingFiles = new List<string>();

                        foreach (var img in project.ImagePaths)
                        {
                            if (File.Exists(img))
                            {
                                Images.Add(img);
                            }
                            else
                            {
                                missingFiles.Add(img);
                            }
                        }

                        if (missingFiles.Count > 0)
                        {
                            string msg = "The following images are missing and were skipped:\n" + 
                                         string.Join("\n", missingFiles);
                            MessageBox.Show(msg, "Warning", MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
                        }

                        _currentFilePath = ofd.FileName;
                        _projectName = Path.GetFileNameWithoutExtension(_currentFilePath);
                        _isDirty = false;
                        _undoStack.Clear();
                        OnPropertyChanged(nameof(ProjectNameTitle));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load: {ex.Message}");
                }
            }
        }

        private void SaveProject(object obj)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveProjectAs(obj);
            }
            else
            {
                SaveToXml(_currentFilePath);
            }
        }

        private void SaveProjectAs(object obj)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML Files|*.xml";
            sfd.FileName = _projectName;
            if (sfd.ShowDialog() == true)
            {
                SaveToXml(sfd.FileName);
                _currentFilePath = sfd.FileName;
                _projectName = Path.GetFileNameWithoutExtension(_currentFilePath);
                OnPropertyChanged(nameof(ProjectNameTitle));
            }
        }

        private void SaveToXml(string path)
        {
            var project = new SpriteSheetProject
            {
                OutputDirectory = OutputDirectory,
                OutputFile = OutputFile,
                Columns = Columns,
                IncludeMetaData = IncludeMetaData,
                ImagePaths = Images.ToList()
            };

            XmlSerializer serializer = new XmlSerializer(typeof(SpriteSheetProject));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, project);
            }
            _isDirty = false;
            OnPropertyChanged(nameof(ProjectNameTitle));
        }

        private void ExitApp(object obj)
        {
            if (CheckUnsavedChanges())
            {
                Application.Current.Shutdown();
            }
        }

        private bool CheckUnsavedChanges()
        {
            if (_isDirty)
            {
                var result = MessageBox.Show("Do you want to save changes?", "Unsaved Changes", 
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveProject(null);
                    return true;
                }
                return result == MessageBoxResult.No;
            }
            return true;
        }

        // --- Undo / Redo Logic ---
        private void RecordState()
        {
            _undoStack.Push(new List<string>(Images));
            _redoStack.Clear();
        }

        private void Undo(object obj)
        {
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new List<string>(Images));
                var previousState = _undoStack.Pop();
                RestoreList(previousState);
            }
        }

        private void Redo(object obj)
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new List<string>(Images));
                var nextState = _redoStack.Pop();
                RestoreList(nextState);
            }
        }

        private void RestoreList(List<string> state)
        {
            Images.Clear();
            foreach (var s in state) Images.Add(s);
            _isDirty = true;
            OnPropertyChanged(nameof(ProjectNameTitle));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}