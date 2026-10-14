using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200094F RID: 2383
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Cinder : CardModel
	{
		// Token: 0x06006F3E RID: 28478 RVA: 0x0028702C File Offset: 0x0028522C
		public Cinder()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x06006F3F RID: 28479 RVA: 0x00287039 File Offset: 0x00285239
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(18m, ValueProp.Move));
			}
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x06006F40 RID: 28480 RVA: 0x0028704D File Offset: 0x0028524D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x0028705C File Offset: 0x0028525C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.Targeting(cardPlay.Target)
				.WithHitVfxNode((Creature t) => NFireBurstVfx.Create(t, 0.75f))
				.Execute(choiceContext);
			CardPile pile = PileType.Hand.GetPile(base.Owner);
			CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(pile.Cards);
			if (cardModel != null)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
		}

		// Token: 0x06006F42 RID: 28482 RVA: 0x002870AF File Offset: 0x002852AF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
