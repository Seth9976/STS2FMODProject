using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA5 RID: 2725
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RocketPunch : CardModel
	{
		// Token: 0x0600767B RID: 30331 RVA: 0x00295C97 File Offset: 0x00293E97
		public RocketPunch()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002000 RID: 8192
		// (get) Token: 0x0600767C RID: 30332 RVA: 0x00295CA4 File Offset: 0x00293EA4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(13m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x00295CCC File Offset: 0x00293ECC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x00295D20 File Offset: 0x00293F20
		public override Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (creator != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (card.Type != CardType.Status)
			{
				return Task.CompletedTask;
			}
			base.EnergyCost.SetUntilPlayed(0, false);
			return Task.CompletedTask;
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x00295D71 File Offset: 0x00293F71
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
