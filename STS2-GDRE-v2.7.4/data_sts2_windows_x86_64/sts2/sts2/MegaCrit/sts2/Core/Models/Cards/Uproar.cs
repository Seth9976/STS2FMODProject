using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B2F RID: 2863
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Uproar : CardModel
	{
		// Token: 0x0600796B RID: 31083 RVA: 0x0029BC33 File Offset: 0x00299E33
		public Uproar()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700212F RID: 8495
		// (get) Token: 0x0600796C RID: 31084 RVA: 0x0029BC40 File Offset: 0x00299E40
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x0029BC54 File Offset: 0x00299E54
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).WithHitCount(2)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			CardModel cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Shuffle)
				.FirstOrDefault<CardModel>();
			if (cardModel == null)
			{
				cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Shuffle)
					.FirstOrDefault<CardModel>();
			}
			if (cardModel != null)
			{
				await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.Default, false, false);
			}
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x0029BCA7 File Offset: 0x00299EA7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
