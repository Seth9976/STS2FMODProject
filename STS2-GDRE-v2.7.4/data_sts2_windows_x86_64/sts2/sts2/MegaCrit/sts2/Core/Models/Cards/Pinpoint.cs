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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A75 RID: 2677
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pinpoint : CardModel
	{
		// Token: 0x0600757C RID: 30076 RVA: 0x00293AF3 File Offset: 0x00291CF3
		public Pinpoint()
			: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F97 RID: 8087
		// (get) Token: 0x0600757D RID: 30077 RVA: 0x00293B00 File Offset: 0x00291D00
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(15m, ValueProp.Move));
			}
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x00293B14 File Offset: 0x00291D14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x00293B67 File Offset: 0x00291D67
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x00293B80 File Offset: 0x00291D80
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
			int num = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == base.Owner && e.HappenedThisTurn(base.CombatState));
			this.ReduceCostBy(num);
			return Task.CompletedTask;
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x00293BD2 File Offset: 0x00291DD2
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Skill)
			{
				return Task.CompletedTask;
			}
			this.ReduceCostBy(1);
			return Task.CompletedTask;
		}

		// Token: 0x06007582 RID: 30082 RVA: 0x00293C0D File Offset: 0x00291E0D
		private void ReduceCostBy(int amount)
		{
			base.EnergyCost.AddThisTurn(-amount, false);
		}
	}
}
