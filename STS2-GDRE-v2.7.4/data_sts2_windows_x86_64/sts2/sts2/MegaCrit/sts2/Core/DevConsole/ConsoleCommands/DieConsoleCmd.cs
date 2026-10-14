using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Console commands which kills the player
	/// </summary>
	// Token: 0x02000CEA RID: 3306
	[NullableContext(1)]
	[Nullable(0)]
	public class DieConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024AF RID: 9391
		// (get) Token: 0x060085C9 RID: 34249 RVA: 0x002CEC27 File Offset: 0x002CCE27
		public override string CmdName
		{
			get
			{
				return "die";
			}
		}

		// Token: 0x170024B0 RID: 9392
		// (get) Token: 0x060085CA RID: 34250 RVA: 0x002CEC2E File Offset: 0x002CCE2E
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170024B1 RID: 9393
		// (get) Token: 0x060085CB RID: 34251 RVA: 0x002CEC35 File Offset: 0x002CCE35
		public override string Description
		{
			get
			{
				return "You die";
			}
		}

		// Token: 0x170024B2 RID: 9394
		// (get) Token: 0x060085CC RID: 34252 RVA: 0x002CEC3C File Offset: 0x002CCE3C
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085CD RID: 34253 RVA: 0x002CEC40 File Offset: 0x002CCE40
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run does not appear to be in progress");
			}
			Task task = CreatureCmd.Kill(issuingPlayer.Creature, false);
			return new CmdResult(task, true, "You died.");
		}
	}
}
