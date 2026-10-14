using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AB3 RID: 2739
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SecondWind : CardModel
	{
		// Token: 0x060076C3 RID: 30403 RVA: 0x002965A1 File Offset: 0x002947A1
		public SecondWind()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x1700201D RID: 8221
		// (get) Token: 0x060076C4 RID: 30404 RVA: 0x002965AE File Offset: 0x002947AE
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700201E RID: 8222
		// (get) Token: 0x060076C5 RID: 30405 RVA: 0x002965B1 File Offset: 0x002947B1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x1700201F RID: 8223
		// (get) Token: 0x060076C6 RID: 30406 RVA: 0x002965C4 File Offset: 0x002947C4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x002965D4 File Offset: 0x002947D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (CardModel cardModel in this.GetCards().ToList<CardModel>())
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x00296627 File Offset: 0x00294827
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x00296640 File Offset: 0x00294840
		private IEnumerable<CardModel> GetCards()
		{
			CardPile pile = PileType.Hand.GetPile(base.Owner);
			return pile.Cards.Where<CardModel>((CardModel c) => c.Type != CardType.Attack);
		}
	}
}
