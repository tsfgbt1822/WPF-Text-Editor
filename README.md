# WPF-Text-Editor
This program is a plain text editor with file manipulation for Windows, built using WPF (Windows Presentation Foundation).

Main Features
- New File: Clears the current text box for a new document and prompts to save changes if the current one was modified.
- Open File: Opens a .txt file and loads its contents into the editor.
- Save File: Saves the current contents to the existing file path.
- Save As: Lets the user choose a new path to save the current document.
- Exit: Closes the application, prompting the user to save any unsaved changes.
 - About: Displays a message box with a basic "About" message.

Core Components
- TextDocument class: Manages the text file's path, content, and modification state.
- MainWindow class: Manages UI interactions like button/menu clicks and text editing events.
