using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200051B RID: 1307
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CentennialPuzzle : RelicModel
	{
		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x0023E224 File Offset: 0x0023C424
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06004FD6 RID: 20438 RVA: 0x0023E227 File Offset: 0x0023C427
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x0023E22E File Offset: 0x0023C42E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x0023E23B File Offset: 0x0023C43B
		// (set) Token: 0x06004FD9 RID: 20441 RVA: 0x0023E243 File Offset: 0x0023C443
		public bool UsedThisCombat
		{
			get
			{
				return this._usedThisCombat;
			}
			private set
			{
				if (this._usedThisCombat == value)
				{
					return;
				}
				base.AssertMutable();
				this._usedThisCombat = value;
			}
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x0023E25C File Offset: 0x0023C45C
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				if (target == base.Owner.Creature)
				{
					if (result.UnblockedDamage > 0)
					{
						if (!this.UsedThisCombat)
						{
							base.Flash();
							this.UsedThisCombat = true;
							int i = 0;
							while (i < base.DynamicVars.Cards.BaseValue)
							{
								await CardPileCmd.Draw(choiceContext, base.Owner);
								i++;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x0023E2B7 File Offset: 0x0023C4B7
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.UsedThisCombat = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002437 RID: 9271
		private bool _usedThisCombat;
	}
}
