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
			AddCommand(CommandKeys.ShortenNames, ExecShortenNames, QueryProcessingCommand);
			AddCommand(CommandKeys.FixProcs, ExecFixProcsPrintSettings, QueryProcessingCommand);
			AddCommand(CommandKeys.FixDateAttributes, ExecFixDateAttributes, QueryProcessingCommand);
			AddCommand(CommandKeys.FixObjectDateVariables, ExecFixObjectDateVariables, QueryProcessingCommand);
			AddCommand(CommandKeys.FixObjDescriptions, ExecFixObjDescriptions, QueryProcessingCommand);
			AddCommand(CommandKeys.FixTblDescriptions, ExecFixTblDescriptions, QueryProcessingCommand);
			AddCommand(CommandKeys.SetObjsNoGen, ExecSetObjsNoGen, QueryProcessingCommand);
		}

		private bool QueryProcessingCommand(CommandData commandData, ref CommandStatus status)
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

		public bool ExecShortenNames(CommandData commandData)
		{
			ShortenNames.Execute(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecFixProcsPrintSettings(CommandData commandData)
		{
			FixProcsPrintSettings.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecFixDateAttributes(CommandData commandData)
		{
			FixDateAttributes.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecFixObjectDateVariables(CommandData commandData)
		{
			FixObjectDateVariables.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecFixObjDescriptions(CommandData commandData)
		{
			FixObjectDescriptions.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecFixTblDescriptions(CommandData commandData)
		{
			FixTableDescriptions.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}

		public bool ExecSetObjsNoGen(CommandData commandData)
		{
			SetObjetsToNotGenerate.ExecuteTool(UIServices.KB.CurrentModel);
			return true;
		}
	}
}
