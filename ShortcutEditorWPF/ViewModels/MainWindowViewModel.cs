using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using ShellLink;
using ShortcutEditorWPF.Infrastructure.Commands;
using ShortcutEditorWPF.Models;
using ShortcutEditorWPF.ViewModels.Base.Base;
using File = ShortcutEditorWPF.Models.File;
using Path = System.IO.Path;

namespace ShortcutEditorWPF.ViewModels
{
	internal class MainWindowViewModel: ViewModel
	{
		private string _title = "ShortcutEditor";
		private string? _currentDirectory;
		private ObservableCollection<File>? _fileList;
		private ShortcutNative _currentShortCut;
		private File? _selectedFile;
		private string? _shortCutData;
		private string? _searchingString;
		private string? _newPartOfString;
		public ObservableCollection<File>? Files 
		{
			get => _fileList ?? null;
			set
			{
				_fileList = value;
				OnPropertyChanged();
			}
		}
		
		public File? SelectedFile 
		{
			get => _selectedFile ?? null;
			set
			{
				_selectedFile = value;
				OnPropertyChanged();
			}
		}
		public ShortcutNative CurrentShortCut 
		{
			get => _currentShortCut;
			private set
			{
				_currentShortCut = value;
				OnPropertyChanged();
			}
		}
		public string? ShortCutData 
		{
			get => _shortCutData ?? null;
			private set
			{
				_shortCutData = value;
				OnPropertyChanged();
			}
		}
		public string? SearchingString 
		{
			get => _searchingString ?? null;
			set
			{
				_searchingString = value;
				OnPropertyChanged();
			}
		}
		
		public string? NewPartOfString 
		{
			get => _newPartOfString ?? null;
			set
			{
				_newPartOfString = value;
				OnPropertyChanged();
			}
		}
		
		public string? CurrentDirectory 
		{
			get => String.Format("Current directory: " + _currentDirectory);
			set
			{
				_currentDirectory = value;
				OnPropertyChanged();
			}
		}

		public string Title
		{
			get => _title;
			set => Set(ref _title, value);
		}

		#region Commands

		#region CloseApplicationCommand
		public ICommand CloseApplicationCommand { get; }
		private bool CanCloseApplicationCommandExecute(object p) => true;
		private void OnCloseApplicationCommandExecuted(object p)
		{
			System.Windows.Application.Current.Shutdown();
		}
		#endregion
		
		#region OpenDirectoryCommand
		public ICommand OpenDirectoryCommand { get; }
		private bool CanOpenDirectoryCommandExecute(object p) => true;

		private void OnOpenDirectoryExecuted(object p)
		{
			if(_fileList is { Count: > 0 })
				return;
			var ofd = new CommonOpenFileDialog("Выберите папку")
			{
				InitialDirectory = _currentDirectory,
				IsFolderPicker = true,
				Multiselect = true
			};
			if (ofd.ShowDialog() != CommonFileDialogResult.Ok) return;
			CurrentDirectory = Path.GetFullPath(ofd.FileName);
			Files = SearchFiles(_currentDirectory, new string [] {".lnk"});
		}
		#endregion
		
		#region ClearListCommand
		public ICommand ClearListCommand { get; }
		private bool CanClearListExecute(object p) => true;

		private void OnClearListExecuted(object p)
		{
			Files = new ObservableCollection<File>();
			CurrentShortCut = new ShortcutNative(new Shortcut());
		}
		#endregion
		
		#region OpenSelectedShortCutCommand
		public ICommand OpenSelectedShortCutCommand { get; }
		private bool CanOpenSelectedShortCutExecute(object p) => true;

		private void OnOpenSelectedShortCutExecuted(object p)
		{
			if (_fileList is { Count: > 0 } && SelectedFile != null)
			{
				CurrentShortCut = new ShortcutNative(Shortcut.ReadFromFile(SelectedFile.FullName));
				ShortCutData = CurrentShortCut.InternalShortcut.ToString();
			}
		}
		#endregion
		
		#region FindReplaceAndRewriteLinkCommand
		public ICommand FindReplaceAndRewriteLinkCommand { get; }
		private bool CanFindReplaceAndRewriteLinkExecute(object p) => true;

		private void OnFindReplaceAndRewriteLinkExecuted(object p)
		{
			if (!string.IsNullOrEmpty(_searchingString) && !string.IsNullOrEmpty(_newPartOfString))
			{
				ReplaceFieldsInShortcut(CurrentShortCut.InternalShortcut, _searchingString, _newPartOfString);
			}
		}
		#endregion
		
		#endregion
		
		public MainWindowViewModel()
		{
			CurrentShortCut = new ShortcutNative(new Shortcut());
			_currentDirectory = GetStartDirectory();
			ShortCutData = string.Empty;
			SearchingString = string.Empty;
			NewPartOfString = string.Empty;
			#region Commands
			OpenDirectoryCommand =
				new LambdaCommand(OnOpenDirectoryExecuted, CanOpenDirectoryCommandExecute);
			
			ClearListCommand =
				new LambdaCommand(OnClearListExecuted, CanClearListExecute);
			
			OpenSelectedShortCutCommand =
				new LambdaCommand(OnOpenSelectedShortCutExecuted, CanOpenSelectedShortCutExecute);
			
			FindReplaceAndRewriteLinkCommand =
				new LambdaCommand(OnFindReplaceAndRewriteLinkExecuted, CanFindReplaceAndRewriteLinkExecute);
			
			CloseApplicationCommand =
				new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
			#endregion
		}
		
		private string? GetStartDirectory()
		{
			var startDirectoryVar1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Develop_tests_link\shortcuts_work");
			if (Directory.Exists(startDirectoryVar1))
				return startDirectoryVar1;
			else
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
		
		private static ObservableCollection<File>? SearchFiles(string? directoryPath, string[] fileExtensions)
		{
			if (directoryPath is null)
				return null;
			try
			{
				var files = Directory.EnumerateFiles(directoryPath, "*.*",
						new EnumerationOptions{IgnoreInaccessible = true, RecurseSubdirectories = true}) //игнорируем папки и файлы к которым у нас нет доступа
					.Where(s => fileExtensions.Any(e => e == Path.GetExtension(s)))
					.Select(f =>
						new File()
						{
							FullName = Path.GetFullPath(f)
						});
			
				var result = new ObservableCollection<File>(files);
				return result;
			}
			catch (UnauthorizedAccessException uAEx)
			{
				throw new Exception(uAEx.Message);
			}
			catch (PathTooLongException pathEx)
			{
				throw new Exception(pathEx.Message);
			}
		}

		private void ReplaceFieldsInShortcut(object obj, string searchingString, string newPartOfString)
		{
			var thisType = obj.GetType();
			Console.WriteLine(thisType);
			var props = thisType.GetProperties();
			foreach (var p in props)
			{
				var conColorDefault = Console.ForegroundColor;
				if (!p.CanRead || !p.CanWrite)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("!!!");
				}
				Console.WriteLine($"Prop name: {p.Name}, can write: {p.CanWrite}");
				Console.ForegroundColor = conColorDefault;
				// var subProp = p.GetP;
				// if (subProp != null) ReplaceFieldsInShortcut(subProp, searchingString, newPartOfString);
			}
			// for (int i = 0; i < props.Length; i++)
			// {
			// 	// if (!props[i].CanRead && !props[i].CanWrite && props[i].PropertyType.)
			// 	// {
			// 	// 	var subType = props[i].PropertyType;
			// 	// 	var propValue = props[i].GetValue(subType);
			// 	// 	if (propValue != null) ReplaceFieldsInShortcut(propValue, searchingString, newPartOfString);
			// 	// }
			// 	
			// 	if (props[i].PropertyType == typeof(string))
			// 	{
			// 		//var fieldName = fields[i].Name;
			// 		var propValue = props[i].GetValue(obj);
			// 		Console.WriteLine($"Can write: {props[i].CanWrite}, field value: {propValue}");
			// 		if (propValue == null) continue;
			// 		var newValue = propValue.ToString()?.Replace(searchingString, newPartOfString);
			// 		props[i].SetValue(obj, newValue);
			// 	}
			// }
		}
	}
}