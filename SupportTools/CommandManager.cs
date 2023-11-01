using Artech.Architecture.UI.Framework.Helper;
using Artech.Architecture.UI.Framework.Services;
using Artech.Common.Framework.Commands;
using Artech.Genexus.Common.Services;
using GeneXus.Packages.SupportTools.ShortNames;

namespace GeneXus.Packages.SupportTools
{
    class CommandManager : CommandDelegator
    {
        public CommandManager()
        {
            AddCommand(CommandKeys.ShortenNames, ExecShortenNames, QueryShortenNames);
        }

        public bool ExecShortenNames(CommandData commandData)
        {
            ShortenNames.Execute(UIServices.KB.CurrentModel);
            return true;
        }

        private bool QueryShortenNames(CommandData commandData, ref CommandStatus status)
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
