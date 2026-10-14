using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0D RID: 2573
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Impatience : CardModel
	{
		// Token: 0x0600734A RID: 29514 RVA: 0x0028F16B File Offset: 0x0028D36B
		public Impatience()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EAF RID: 7855
		// (get) Token: 0x0600734B RID: 29515 RVA: 0x0028F178 File Offset: 0x0028D378
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return PileType.Hand.GetPile(base.Owner).Cards.All<CardModel>((CardModel c) => c.Type != CardType.Attack);
			}
		}

		// Token: 0x17001EB0 RID: 7856
		// (get) Token: 0x0600734C RID: 29516 RVA: 0x0028F1AF File Offset: 0x0028D3AF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x0028F1BC File Offset: 0x0028D3BC
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x0028F1D4 File Offset: 0x0028D3D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!PileType.Hand.GetPile(base.Owner).Cards.Any<CardModel>((CardModel c) => c.Type == CardType.Attack))
			{
				await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			}
		}
	}
}
