using ShortcutEditorWPF.Models.Interfaces;

namespace ShortcutEditorWPF.Models
{
	public class File: IEntity
	{
		public int Id { get; set; }
		
		public string ShortName { get; set; }
		public string FullName { get; set; }
	}
}