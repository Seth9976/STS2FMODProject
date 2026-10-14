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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200090D RID: 2317
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BansheesCry : CardModel
	{
		// Token: 0x06006DF5 RID: 28149 RVA: 0x0028451B File Offset: 0x0028271B
		public BansheesCry()
			: base(9, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x06006DF6 RID: 28150 RVA: 0x00284529 File Offset: 0x00282729
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x06006DF7 RID: 28151 RVA: 0x00284536 File Offset: 0x00282736
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(33m, ValueProp.Move),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x06006DF8 RID: 28152 RVA: 0x0028455C File Offset: 0x0028275C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06006DF9 RID: 28153 RVA: 0x002845AF File Offset: 0x002827AF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-2);
		}

		// Token: 0x06006DFA RID: 28154 RVA: 0x002845C0 File Offset: 0x002827C0
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
			int num = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.WasEthereal && e.CardPlay.Player == base.Owner);
			base.EnergyCost.AddThisCombat(-num * base.DynamicVars.Energy.IntValue, false);
			return Task.CompletedTask;
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x0028462C File Offset: 0x0028282C
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal))
			{
				return Task.CompletedTask;
			}
			base.EnergyCost.AddThisCombat(-base.DynamicVars.Energy.IntValue, false);
			return Task.CompletedTask;
		}
	}
}
