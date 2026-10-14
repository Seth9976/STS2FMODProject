using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B8 RID: 1720
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoDrawPower : PowerModel
	{
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x060059BA RID: 22970 RVA: 0x00250F2C File Offset: 0x0024F12C
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x060059BB RID: 22971 RVA: 0x00250F2F File Offset: 0x0024F12F
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x00250F32 File Offset: 0x0024F132
		public override bool ShouldDraw(Player player, bool fromHandDraw)
		{
			if (fromHandDraw)
			{
				return true;
			}
			if (player != base.Owner.Player)
			{
				return true;
			}
			base.Flash();
			return false;
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x00250F50 File Offset: 0x0024F150
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
