using Artech.Common.Framework.Commands;

namespace GeneXus.Packages.SupportTools
{
	public class CommandKeys
	{
		public static CommandKey ShortenNames => new CommandKey(Package.Guid, "ShortenNames");
		public static CommandKey FixProcs => new CommandKey(Package.Guid, "FixProcs");
	}
}
