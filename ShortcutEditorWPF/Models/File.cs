using System.IO;
using ShortcutEditorWPF.Models.Interfaces;

namespace ShortcutEditorWPF.Models
{
	public class File: IEntity
	{
		public int Id { get; set; }

		public string ShortName
		{
			get
			{
				if (FullName is null)
					return string.Empty;
				return FullName.Length > 0 ? Path.GetFileName(FullName) : string.Empty;
			}
		}
		public string FullName { get; set; }
		
		public File(string fullName)
		{
			Id = 0;
			FullName = fullName;
		}

		public File() : this(string.Empty)
		{
		}
		
		public override string ToString()
		{
			if (ShortName != null) return ShortName;
			return string.Empty;
		}
	}
}