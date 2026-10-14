using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// A command that toggles the 0 - 9 hotkeys for hiding UI and +/- for controlling speed
	/// </summary>
	// Token: 0x02000D01 RID: 3329
	[NullableContext(1)]
	[Nullable(0)]
	public class TrailerConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700250F RID: 9487
		// (get) Token: 0x0600867C RID: 34428 RVA: 0x002D1FC9 File Offset: 0x002D01C9
		public override string CmdName
		{
			get
			{
				return "trailer";
			}
		}

		// Token: 0x17002510 RID: 9488
		// (get) Token: 0x0600867D RID: 34429 RVA: 0x002D1FD0 File Offset: 0x002D01D0
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002511 RID: 9489
		// (get) Token: 0x0600867E RID: 34430 RVA: 0x002D1FD7 File Offset: 0x002D01D7
		public override string Description
		{
			get
			{
				return "Toggles the ability to show and hide UI elements via 0 - 9 and +- keys.";
			}
		}

		// Token: 0x17002512 RID: 9490
		// (get) Token: 0x0600867F RID: 34431 RVA: 0x002D1FDE File Offset: 0x002D01DE
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x002D1FE4 File Offset: 0x002D01E4
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			NGame.ToggleTrailerMode();
			string text = (NGame.IsTrailerMode ? "enabled" : "disabled");
			return new CmdResult(true, "Trailer mode " + text);
		}
	}
}
