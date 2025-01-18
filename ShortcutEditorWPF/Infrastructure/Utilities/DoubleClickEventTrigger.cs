using System;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace ShortcutEditorWPF.Infrastructure.Utilities;

public class DoubleClickEventTrigger : EventTrigger
{
	private bool _clicked;
	private TimeOnly _tPreviousClick;
	protected override void OnEvent(EventArgs eventArgs)
	{
		var e = eventArgs as MouseButtonEventArgs;
		if (e == null)
			return;
		
		var tCurrentClick = TimeOnly.FromDateTime(DateTime.Now);
		var tDifference = (tCurrentClick - _tPreviousClick).TotalMilliseconds;
		
		if (!_clicked && tDifference > 300)
		{
			_clicked = true;
		}
		else if (_clicked && tDifference < 300)
		{
			base.OnEvent(eventArgs);
			_clicked = false;
		}
		_tPreviousClick = TimeOnly.FromDateTime(DateTime.Now);
	}
}