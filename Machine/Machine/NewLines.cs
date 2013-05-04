
namespace Machine
{
	/// <summary>
	/// Provides static properties represent types of newline character
	/// </summary>
	sealed class NewLines
	{
		public static string Win
		{
			get
			{
				return "\r\n";
			}
		}

		public static string Mac
		{
			get
			{
				return "\r";
			}
		}

		public static string Unix
		{
			get
			{
				return "\n";
			}
		}
	}
}
