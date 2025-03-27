using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System;

namespace ToriFischerChallengeM8
{
    public class TextDocument
    {
        public string FilePath { get; private set; } // Stores the file path of the document
        public string Content { get; set; } // Stores the content of the document
        public bool IsModified { get; set; } // Tracks whether the document has been modified
        
        //constructor
        public TextDocument()
        {
            FilePath = string.Empty;
            Content = string.Empty;
            IsModified = false;
        }

        //opens a text file & loads contents
        public void Open(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                Content = reader.ReadToEnd();
            }
            FilePath = filePath;
            IsModified = false;
        }


        //saves current cotent to existing file path
        public void Save()
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(Content);
                }
                IsModified = false;
            }
        }


        //saves current content to new file path 
        public void SaveAs(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(Content);
            }
            FilePath = filePath;
            IsModified = false;
        }

    }

    //main window class for application
    public partial class MainWindow : Window
    {
        //instance of class to manage file
        private TextDocument document;

        public MainWindow()
        {
            InitializeComponent();
            document = new TextDocument();
            UpdateMenuItems();
        }

        //creates new file and clears existing content after confirming whether to save changes
        private void NewFile(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                document = new TextDocument();
                textBox.Text = string.Empty;

                document.IsModified = false;
                UpdateMenuItems();
            }
        }

        //opens an existing file after cinfirming whether to save changes
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt" }; //Create an OpenFileDialog instance to allow user to select a txt file.
                if (openFileDialog.ShowDialog() == true) //if user selects file, continue
                {
                    document.Open(openFileDialog.FileName); //load file
                    textBox.Text = document.Content; //display file

                    document.IsModified = false;
                    UpdateMenuItems();
                }
            }
        }

        //saves current file to current file path
        //if file path does not currently exist, acts as save as
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(document.FilePath))
            {
                document.Content = textBox.Text;
                document.Save();
            }
            else
            {
                SaveFileAs(sender, e);
            }
            UpdateMenuItems();
        }

        //prompts user to select file path and saves document as a new file
        private void SaveFileAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog //Create an OpenFileDialog instance to allow user to specify file path
            { 
                Filter = "Text Files (*.txt)|*.txt", //must save as txt
                FileName = string.IsNullOrEmpty(document.FilePath) ? "Untitled.txt" : System.IO.Path.GetFileName(document.FilePath), //set file name to untitled or load previously saved name
                //set directory to documents if file has not been saved before, otherwise use current directory
                InitialDirectory = string.IsNullOrEmpty(document.FilePath) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : System.IO.Path.GetDirectoryName(document.FilePath) 
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                document.Content = textBox.Text;
                document.SaveAs(saveFileDialog.FileName);
            }
            UpdateMenuItems();
        }

        //closes application after confirming whether to save changes
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                Application.Current.Shutdown();
            }
        }

        //displays about message box
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Plain Text Editor\nDeveloped by: Tori Fischer", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //checks for unsaved changes and if there are unsaved changes prompts user to save/not save/cancel
        private bool ConfirmSaveChanges()
        {
            if (document.IsModified && (!string.IsNullOrEmpty(document.Content)))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Save Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(null, null);
                }
                return result != MessageBoxResult.Cancel;
            }
            return true;
        }

        //marks document as modified when text is changed
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            document.IsModified = true;
            UpdateMenuItems();
        }

        //updates menu items based on current state
        private void UpdateMenuItems()
        {
            menuSave.IsEnabled = document.IsModified;
            menuSaveAs.IsEnabled = true;
        }
    }
}
