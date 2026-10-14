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
	// Token: 0x020009D5 RID: 2517
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ftl : CardModel
	{
		// Token: 0x0600721A RID: 29210 RVA: 0x0028CA93 File Offset: 0x0028AC93
		public Ftl()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E2E RID: 7726
		// (get) Token: 0x0600721B RID: 29211 RVA: 0x0028CAA0 File Offset: 0x0028ACA0
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.CanDrawCard;
			}
		}

		// Token: 0x17001E2F RID: 7727
		// (get) Token: 0x0600721C RID: 29212 RVA: 0x0028CAA8 File Offset: 0x0028ACA8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(5m, ValueProp.Move),
					new IntVar("PlayMax", 3m),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17001E30 RID: 7728
		// (get) Token: 0x0600721D RID: 29213 RVA: 0x0028CAE0 File Offset: 0x0028ACE0
		private bool CanDrawCard
		{
			get
			{
				int num = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Player == base.Owner);
				return num < base.DynamicVars["PlayMax"].IntValue;
			}
		}

		// Token: 0x0600721E RID: 29214 RVA: 0x0028CB28 File Offset: 0x0028AD28
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			if (this.CanDrawCard)
			{
				await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			}
		}

		// Token: 0x0600721F RID: 29215 RVA: 0x0028CB7B File Offset: 0x0028AD7B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars["PlayMax"].UpgradeValueBy(1m);
		}

		// Token: 0x04002872 RID: 10354
		private const string _playMaxKey = "PlayMax";
	}
}
