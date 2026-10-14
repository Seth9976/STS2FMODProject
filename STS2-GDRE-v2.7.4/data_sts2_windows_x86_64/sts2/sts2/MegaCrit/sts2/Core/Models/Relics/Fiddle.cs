using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200054A RID: 1354
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fiddle : RelicModel
	{
		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x0023FBD3 File Offset: 0x0023DDD3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x0023FBD6 File Offset: 0x0023DDD6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x0023FBE3 File Offset: 0x0023DDE3
		public override decimal ModifyHandDrawLate(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.IntValue;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x0023FC0B File Offset: 0x0023DE0B
		public override bool ShouldDraw(Player player, bool fromHandDraw)
		{
			return fromHandDraw || player != base.Owner || player.Creature.Side != player.Creature.CombatState.CurrentSide;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x0023FC3D File Offset: 0x0023DE3D
		public override Task AfterPreventingDraw()
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
