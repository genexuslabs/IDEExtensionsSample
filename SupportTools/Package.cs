using System;
using System.Runtime.InteropServices;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Packages;
using Artech.Common.Properties;
using Artech.Genexus.Common;
using Artech.Udm.Framework.References;

namespace Artech.Packages.SupportTools
{
	[Guid("616AD12C-C14D-4c5d-9A80-CE96B5AD6D23")]
	public class Package : AbstractPackageUI
	{
		public static Guid guid = typeof(Package).GUID;

		public override string Name
		{
			get { return "Support Tools"; }
		}

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
