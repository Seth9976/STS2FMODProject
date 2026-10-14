using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A3E RID: 2622
	[NullableContext(1)]
	[Nullable(0)]
	public class Midnight : CardModel
	{
		// Token: 0x06007459 RID: 29785 RVA: 0x002915EB File Offset: 0x0028F7EB
		public Midnight()
			: base(12, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F1B RID: 7963
		// (get) Token: 0x0600745A RID: 29786 RVA: 0x002915F9 File Offset: 0x0028F7F9
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001F1C RID: 7964
		// (get) Token: 0x0600745B RID: 29787 RVA: 0x002915FC File Offset: 0x0028F7FC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(60m, ValueProp.Move));
			}
		}

		// Token: 0x0600745C RID: 29788 RVA: 0x00291610 File Offset: 0x0028F810
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x00291664 File Offset: 0x0028F864
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			if (base.IsClone)
			{
				return Task.CompletedTask;
			}
			int num = CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Count<CardExhaustedEntry>();
			this.ReduceCostBy(num);
			return Task.CompletedTask;
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x002916AF File Offset: 0x0028F8AF
		public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			this.ReduceCostBy(1);
			return Task.CompletedTask;
		}

		// Token: 0x0600745F RID: 29791 RVA: 0x002916BD File Offset: 0x0028F8BD
		private void ReduceCostBy(int amount)
		{
			base.EnergyCost.AddThisCombat(-amount, false);
		}

		// Token: 0x06007460 RID: 29792 RVA: 0x002916CD File Offset: 0x0028F8CD
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(12m);
		}
	}
}
