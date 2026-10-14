using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF5 RID: 3317
	[NullableContext(1)]
	[Nullable(0)]
	public class InstantConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024DD RID: 9437
		// (get) Token: 0x0600861E RID: 34334 RVA: 0x002D077C File Offset: 0x002CE97C
		public override string CmdName
		{
			get
			{
				return "instant";
			}
		}

		// Token: 0x170024DE RID: 9438
		// (get) Token: 0x0600861F RID: 34335 RVA: 0x002D0783 File Offset: 0x002CE983
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170024DF RID: 9439
		// (get) Token: 0x06008620 RID: 34336 RVA: 0x002D078A File Offset: 0x002CE98A
		public override string Description
		{
			get
			{
				return "Turns instant mode on.";
			}
		}

		// Token: 0x170024E0 RID: 9440
		// (get) Token: 0x06008621 RID: 34337 RVA: 0x002D0791 File Offset: 0x002CE991
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008622 RID: 34338 RVA: 0x002D0794 File Offset: 0x002CE994
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
				return new CmdResult(true, "Instant mode off");
			}
			SaveManager.Instance.PrefsSave.FastMode = FastModeType.Instant;
			return new CmdResult(true, "INSTANT MODE ACTIVE");
		}
	}
}
