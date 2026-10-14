using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C7 RID: 1479
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RainbowRing : RelicModel
	{
		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06005435 RID: 21557 RVA: 0x00246227 File Offset: 0x00244427
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06005436 RID: 21558 RVA: 0x0024622C File Offset: 0x0024442C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06005437 RID: 21559 RVA: 0x00246266 File Offset: 0x00244466
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005438 RID: 21560 RVA: 0x0024628D File Offset: 0x0024448D
		// (set) Token: 0x06005439 RID: 21561 RVA: 0x00246295 File Offset: 0x00244495
		private int AttacksPlayedThisTurn
		{
			get
			{
				return this._attacksPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._attacksPlayedThisTurn = value;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x0600543A RID: 21562 RVA: 0x002462A4 File Offset: 0x002444A4
		// (set) Token: 0x0600543B RID: 21563 RVA: 0x002462AC File Offset: 0x002444AC
		private int SkillsPlayedThisTurn
		{
			get
			{
				return this._skillsPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._skillsPlayedThisTurn = value;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x0600543C RID: 21564 RVA: 0x002462BB File Offset: 0x002444BB
		// (set) Token: 0x0600543D RID: 21565 RVA: 0x002462C3 File Offset: 0x002444C3
		private int PowersPlayedThisTurn
		{
			get
			{
				return this._powersPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._powersPlayedThisTurn = value;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x0600543E RID: 21566 RVA: 0x002462D2 File Offset: 0x002444D2
		// (set) Token: 0x0600543F RID: 21567 RVA: 0x002462DA File Offset: 0x002444DA
		private int ActivationCountThisTurn
		{
			get
			{
				return this._activationCountThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._activationCountThisTurn = value;
				base.Status = ((this._activationCountThisTurn > 0) ? RelicStatus.Active : RelicStatus.Normal);
			}
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x002462FC File Offset: 0x002444FC
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.AttacksPlayedThisTurn = 0;
			this.SkillsPlayedThisTurn = 0;
			this.PowersPlayedThisTurn = 0;
			this.ActivationCountThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x00246338 File Offset: 0x00244538
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					if (this.ActivationCountThisTurn < 1)
					{
						this.AttacksPlayedThisTurn += ((cardPlay.Card.Type == CardType.Attack) ? 1 : 0);
						this.SkillsPlayedThisTurn += ((cardPlay.Card.Type == CardType.Skill) ? 1 : 0);
						this.PowersPlayedThisTurn += ((cardPlay.Card.Type == CardType.Power) ? 1 : 0);
						if (this.AttacksPlayedThisTurn > 0 && this.SkillsPlayedThisTurn > 0 && this.PowersPlayedThisTurn > 0)
						{
							base.Flash();
							await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
							await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
							this.ActivationCountThisTurn++;
						}
					}
				}
			}
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x0024638B File Offset: 0x0024458B
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.AttacksPlayedThisTurn = 0;
			this.SkillsPlayedThisTurn = 0;
			this.PowersPlayedThisTurn = 0;
			this.ActivationCountThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x040024AD RID: 9389
		private int _attacksPlayedThisTurn;

		// Token: 0x040024AE RID: 9390
		private int _skillsPlayedThisTurn;

		// Token: 0x040024AF RID: 9391
		private int _powersPlayedThisTurn;

		// Token: 0x040024B0 RID: 9392
		private int _activationCountThisTurn;
	}
}
