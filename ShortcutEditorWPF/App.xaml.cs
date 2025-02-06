
using System;
using System.Windows;

namespace ShortcutEditorWPF
{
	
	public partial class App : Application
	{
		[STAThread]
		protected override void OnStartup(StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += (Exception);
		}
		
		static void Exception(object sender, UnhandledExceptionEventArgs e)
		{
			var excetionText = e.ExceptionObject.ToString()?.Substring(0, 590);
			MessageBox.Show(excetionText, "Ошибка!",  MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}