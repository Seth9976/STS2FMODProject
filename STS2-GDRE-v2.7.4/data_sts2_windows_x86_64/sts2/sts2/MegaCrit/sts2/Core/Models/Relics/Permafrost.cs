using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B4 RID: 1460
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Permafrost : RelicModel
	{
		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x060053BA RID: 21434 RVA: 0x002454F3 File Offset: 0x002436F3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x002454F6 File Offset: 0x002436F6
		// (set) Token: 0x060053BC RID: 21436 RVA: 0x002454FE File Offset: 0x002436FE
		private bool ActivatedThisCombat
		{
			get
			{
				return this._activatedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._activatedThisCombat = value;
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x060053BD RID: 21437 RVA: 0x0024550D File Offset: 0x0024370D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Unpowered));
			}
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x00245520 File Offset: 0x00243720
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			this.ActivatedThisCombat = false;
			return Task.CompletedTask;
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x0024553C File Offset: 0x0024373C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				if (cardPlay.Card.Owner == base.Owner)
				{
					if (cardPlay.Card.Type == CardType.Power)
					{
						if (!this.ActivatedThisCombat)
						{
							base.Flash();
							await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
							this.ActivatedThisCombat = true;
						}
					}
				}
			}
		}

		// Token: 0x0400249D RID: 9373
		private bool _activatedThisCombat;
	}
}
