using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// A command that lets you jump to any room on the map.
	/// </summary>
	// Token: 0x02000D02 RID: 3330
	[NullableContext(1)]
	[Nullable(0)]
	public class TravelConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002513 RID: 9491
		// (get) Token: 0x06008682 RID: 34434 RVA: 0x002D2023 File Offset: 0x002D0223
		public override string CmdName
		{
			get
			{
				return "travel";
			}
		}

		// Token: 0x17002514 RID: 9492
		// (get) Token: 0x06008683 RID: 34435 RVA: 0x002D202A File Offset: 0x002D022A
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002515 RID: 9493
		// (get) Token: 0x06008684 RID: 34436 RVA: 0x002D2031 File Offset: 0x002D0231
		public override string Description
		{
			get
			{
				return "Enables you to jump to any room on the map.";
			}
		}

		// Token: 0x17002516 RID: 9494
		// (get) Token: 0x06008685 RID: 34437 RVA: 0x002D2038 File Offset: 0x002D0238
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008686 RID: 34438 RVA: 0x002D203C File Offset: 0x002D023C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress...");
			}
			NMapScreen.Instance.SetDebugTravelEnabled(!NMapScreen.Instance.IsDebugTravelEnabled);
			string text = (NMapScreen.Instance.IsDebugTravelEnabled ? "enabled" : "disabled");
			return new CmdResult(true, "Travel mode " + text);
		}
	}
}
