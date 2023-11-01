using Artech.Architecture.UI.Framework.Helper;
using Artech.Architecture.UI.Framework.Services;
using Artech.Common.Framework.Commands;
using Artech.Genexus.Common.Services;
using Artech.Packages.SupportTools.ShortNames;

namespace Artech.Packages.SupportTools
{
	class CommandManager : CommandDelegator
	{
		public CommandManager()
		{
			AddCommand(CommandKeys.ShortenNames, new ExecHandler(ExecShortenNames), new QueryHandler(QueryShortenNames));
		}

		public bool ExecShortenNames(CommandData commandData)
		{
			IKBService kbService = UIServices.KB;
			if (kbService == null)
				return false;

			ShortenNames.Execute(kbService.CurrentModel);
			return true;
		}

		private bool QueryShortenNames(CommandData commandData, ref CommandStatus status)
		{
			IKBService kbserv = UIServices.KB;

			if (kbserv == null || kbserv.CurrentModel == null)
				status.State = CommandState.Invisible;
			else if (GenexusUIServices.Build.IsBuilding || kbserv.CurrentModel.IsReadOnly)
				status.State = CommandState.Disabled;
			else
				status.State = CommandState.Enabled;

			return true;
		}
	}
}
