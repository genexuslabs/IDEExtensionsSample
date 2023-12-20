using Artech.Architecture.UI.Framework.Helper;
using Artech.Architecture.UI.Framework.Services;
using Artech.Common.Framework.Commands;
using Artech.Genexus.Common.Services;
using GeneXus.Packages.SupportTools.ShortNames;
using GeneXus.Packages.SupportTools.Fixing;

namespace GeneXus.Packages.SupportTools
{
	class CommandManager : CommandDelegator
	{
		public CommandManager()
		{
			AddCommand(CommandKeys.ShortenNames, ExecShortenNames, QueryShortenNames);
			AddCommand(CommandKeys.FixProcs, ExecFixProcs, QueryFixProcs);
		}

		public bool ExecShortenNames(CommandData commandData)
		{
			ShortenNames.Execute(UIServices.KB.CurrentModel);
			return true;
		}

		private bool QueryShortenNames(CommandData commandData, ref CommandStatus status) => QueryProcessingCommand(status);

		public bool ExecFixProcs(CommandData commandData)
		{
			FixProcs.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		private bool QueryFixProcs(CommandData commandData, ref CommandStatus status) => QueryProcessingCommand(status);

		private static bool QueryProcessingCommand(CommandStatus status)
		{
			IKBService kbserv = UIServices.KB;

			if (kbserv.CurrentModel is null)
				status.State = CommandState.Invisible;
			else if (GenexusUIServices.Build.IsBuilding || kbserv.CurrentModel.IsReadOnly)
				status.State = CommandState.Disabled;
			else
				status.State = CommandState.Enabled;

			return true;
		}
	}
}
