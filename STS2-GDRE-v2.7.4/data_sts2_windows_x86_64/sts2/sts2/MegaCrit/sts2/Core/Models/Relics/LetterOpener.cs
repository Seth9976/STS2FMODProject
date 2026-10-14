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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057A RID: 1402
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LetterOpener : RelicModel
	{
		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x00242513 File Offset: 0x00240713
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06005221 RID: 21025 RVA: 0x00242516 File Offset: 0x00240716
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06005222 RID: 21026 RVA: 0x00242522 File Offset: 0x00240722
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.SkillsPlayedThisTurn % base.DynamicVars.Cards.IntValue;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x00242554 File Offset: 0x00240754
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new DamageVar(5m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x00242579 File Offset: 0x00240779
		// (set) Token: 0x06005225 RID: 21029 RVA: 0x00242581 File Offset: 0x00240781
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06005226 RID: 21030 RVA: 0x00242596 File Offset: 0x00240796
		// (set) Token: 0x06005227 RID: 21031 RVA: 0x0024259E File Offset: 0x0024079E
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
				this.UpdateDisplay();
			}
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x002425B4 File Offset: 0x002407B4
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars.Cards.IntValue;
				base.Status = ((this.SkillsPlayedThisTurn % intValue == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00242600 File Offset: 0x00240800
		public override Task BeforeCombatStart()
		{
			this.SkillsPlayedThisTurn = 0;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x00242618 File Offset: 0x00240818
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			if (base.Owner.PlayerCombatState.TurnNumber == 1)
			{
				return Task.CompletedTask;
			}
			this.SkillsPlayedThisTurn = 0;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x0024266C File Offset: 0x0024086C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					if (cardPlay.Card.Type == CardType.Skill)
					{
						int skillsPlayedThisTurn = this.SkillsPlayedThisTurn;
						this.SkillsPlayedThisTurn = skillsPlayedThisTurn + 1;
						int intValue = base.DynamicVars.Cards.IntValue;
						if (this.SkillsPlayedThisTurn % intValue == 0)
						{
							TaskHelper.RunSafely(this.DoActivateVisuals());
							await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
						}
					}
				}
			}
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x002426C0 File Offset: 0x002408C0
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x00242703 File Offset: 0x00240903
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.IsActivating = false;
			return Task.CompletedTask;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x00242718 File Offset: 0x00240918
		public int GetSkillsPlayedForTest()
		{
			return this.SkillsPlayedThisTurn;
		}

		// Token: 0x04002470 RID: 9328
		private bool _isActivating;

		// Token: 0x04002471 RID: 9329
		private int _skillsPlayedThisTurn;
	}
}
