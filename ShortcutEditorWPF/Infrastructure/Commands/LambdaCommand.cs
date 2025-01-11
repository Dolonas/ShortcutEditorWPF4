using System;
using ShortcutEditorWPF.Infrastructure.Commands.Base;

namespace ShortcutEditorWPF.Infrastructure.Commands
{
	public class LambdaCommand: Command
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;

		public LambdaCommand(Action<object> execute, Func<object, bool>canExecute = null)
		{
			_execute = execute ?? throw new ArgumentException(nameof(Execute));
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

		public override void Execute(object parameter) => _execute(parameter);
	}
}