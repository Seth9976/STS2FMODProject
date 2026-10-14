using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000500 RID: 1280
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BigMushroom : RelicModel
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06004F24 RID: 20260 RVA: 0x0023CC8B File Offset: 0x0023AE8B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06004F25 RID: 20261 RVA: 0x0023CC8E File Offset: 0x0023AE8E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06004F26 RID: 20262 RVA: 0x0023CC91 File Offset: 0x0023AE91
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new MaxHpVar(20m),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x0023CCB8 File Offset: 0x0023AEB8
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			this.Grow();
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x0023CCFB File Offset: 0x0023AEFB
		public override Task AfterRoomEntered(AbstractRoom _)
		{
			this.Grow();
			return Task.CompletedTask;
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x0023CD08 File Offset: 0x0023AF08
		public override decimal ModifyHandDraw(Player player, decimal cardsToDraw)
		{
			if (player != base.Owner)
			{
				return cardsToDraw;
			}
			if (base.Owner.PlayerCombatState.TurnNumber != 1)
			{
				return cardsToDraw;
			}
			return cardsToDraw - base.DynamicVars.Cards.IntValue;
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x0023CD45 File Offset: 0x0023AF45
		private void Grow()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(base.Owner.Creature);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.ScaleTo(1.5f, 0.0);
		}
	}
}
