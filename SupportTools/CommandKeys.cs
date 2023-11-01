using Artech.Common.Framework.Commands;

namespace Artech.Packages.SupportTools
{
	public class CommandKeys
	{
		private static CommandKey shortenNames = new CommandKey(Package.guid, "ShortenNames");

		public static CommandKey ShortenNames { get { return shortenNames; } }
	}
}
