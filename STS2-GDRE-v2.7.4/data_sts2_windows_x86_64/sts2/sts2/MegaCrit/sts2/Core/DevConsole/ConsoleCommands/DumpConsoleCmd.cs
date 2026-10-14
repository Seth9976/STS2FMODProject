using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CEC RID: 3308
	[NullableContext(1)]
	[Nullable(0)]
	public class DumpConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024B7 RID: 9399
		// (get) Token: 0x060085D6 RID: 34262 RVA: 0x002CED97 File Offset: 0x002CCF97
		public override string CmdName
		{
			get
			{
				return "dump";
			}
		}

		// Token: 0x170024B8 RID: 9400
		// (get) Token: 0x060085D7 RID: 34263 RVA: 0x002CED9E File Offset: 0x002CCF9E
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170024B9 RID: 9401
		// (get) Token: 0x060085D8 RID: 34264 RVA: 0x002CEDA5 File Offset: 0x002CCFA5
		public override string Description
		{
			get
			{
				return "Dumps Model ID database to console & logs.";
			}
		}

		// Token: 0x170024BA RID: 9402
		// (get) Token: 0x060085D9 RID: 34265 RVA: 0x002CEDAC File Offset: 0x002CCFAC
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060085DA RID: 34266 RVA: 0x002CEDAF File Offset: 0x002CCFAF
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			Log.Info(ModelIdSerializationCache.Dump(), 2);
			return new CmdResult(true, "Model ID database dumped to console & logs");
		}
	}
}
