using System.Collections.Generic;

namespace Machine
{
	/// <summary>
	/// Provides the base class of option items
	/// </summary>
	class OptionsItem
	{
		public string ID { get; set; }

		public OptionsItem()
		{
			this.ID = "";
		}
	}

	/// <summary>
	/// Represents string[] option item
	/// </summary>
	class StringsOptionsItem : OptionsItem
	{
		public List<string> Args { get; set; }
		public StringsOptionsItem()
			: base()
		{
			this.Args = new List<string>();
		}
	}
	
	/// <summary>
	/// Provides the base class I/O option items
	/// </summary>
	class DataOptionsItem : OptionsItem
	{
		public List<string> Paths { get; set; }
		public List<List<string>> Content { get; set; }
		public List<byte[]> BinaryContent { get; set; }

		public DataOptionsItem()
			: base()
		{
			this.Paths = new List<string>();
			this.Content = new List<List<string>>();
			this.BinaryContent = new List<byte[]>();
		}
	}

	/// <summary>
	/// Represents output option item
	/// </summary>
	class OutputOptionsItem : DataOptionsItem
	{
		public Options.OutputMode Mode { get; set; }
		public OutputOptionsItem()
			: base()
		{
			this.Mode = Options.OutputMode.FileOnly;
		}
	}

	/// <summary>
	/// Represents input option item
	/// </summary>
	class InputOptionsItem : DataOptionsItem
	{
		public Options.InputMode Mode { get; set; }
		public InputOptionsItem()
			: base()
		{
			this.Mode = Options.InputMode.File;
		}
	}
}
