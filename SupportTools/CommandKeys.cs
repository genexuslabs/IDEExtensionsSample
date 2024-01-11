using Artech.Common.Framework.Commands;

namespace GeneXus.Packages.SupportTools
{
	public class CommandKeys
	{
		public static CommandKey ShortenNames => new CommandKey(Package.Guid, "ShortenNames");
		public static CommandKey FixProcs => new CommandKey(Package.Guid, "FixProcs");
		public static CommandKey FixDateAttributes => new CommandKey(Package.Guid, "FixDateAttributes");
		public static CommandKey FixObjectDateVariables => new CommandKey(Package.Guid, "FixObjectDateVariables");
		public static CommandKey FixObjDescriptions => new CommandKey(Package.Guid, "FixObjDescriptions");
		public static CommandKey FixTblDescriptions => new CommandKey(Package.Guid, "FixTblDescriptions");
	}
}
