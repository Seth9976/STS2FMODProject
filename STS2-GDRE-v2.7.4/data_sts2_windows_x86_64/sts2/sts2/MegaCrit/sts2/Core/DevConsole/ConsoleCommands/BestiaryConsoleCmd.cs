using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE5 RID: 3301
	[NullableContext(1)]
	[Nullable(0)]
	public class BestiaryConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700249A RID: 9370
		// (get) Token: 0x060085A6 RID: 34214 RVA: 0x002CE459 File Offset: 0x002CC659
		public override string CmdName
		{
			get
			{
				return "bestiary";
			}
		}

		// Token: 0x1700249B RID: 9371
		// (get) Token: 0x060085A7 RID: 34215 RVA: 0x002CE460 File Offset: 0x002CC660
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700249C RID: 9372
		// (get) Token: 0x060085A8 RID: 34216 RVA: 0x002CE467 File Offset: 0x002CC667
		public override string Description
		{
			get
			{
				return "Opens the bestiary (WIP)";
			}
		}

		// Token: 0x1700249D RID: 9373
		// (get) Token: 0x060085A9 RID: 34217 RVA: 0x002CE46E File Offset: 0x002CC66E
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060085AA RID: 34218 RVA: 0x002CE474 File Offset: 0x002CC674
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			NCompendiumSubmenu ncompendiumSubmenu = NGame.Instance.MainMenu.OpenCompendiumSubmenu();
			ncompendiumSubmenu.OpenBestiary();
			return new CmdResult(true, "Opened bestiary submenu");
		}
	}
}
