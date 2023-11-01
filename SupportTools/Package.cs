using System;
using System.Runtime.InteropServices;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Packages;

namespace GeneXus.Packages.SupportTools
{
    [Guid("616AD12C-C14D-4c5d-9A80-CE96B5AD6D23")]
    public class Package : AbstractPackageUI
    {
        public static Guid Guid => typeof(Package).GUID;

        public override string Name => "Support Tools";

        public override void Initialize(IGxServiceProvider services)
        {
            base.Initialize(services);

            LoadCommandTargets();
        }

        private void LoadCommandTargets()
        {
            AddCommandTarget(new CommandManager());
        }
    }
}
