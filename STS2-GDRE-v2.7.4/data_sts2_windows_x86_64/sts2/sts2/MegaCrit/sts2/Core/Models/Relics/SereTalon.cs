using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DE RID: 1502
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SereTalon : RelicModel
	{
		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060054BC RID: 21692 RVA: 0x00247427 File Offset: 0x00245627
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060054BD RID: 21693 RVA: 0x0024742A File Offset: 0x0024562A
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060054BE RID: 21694 RVA: 0x0024742D File Offset: 0x0024562D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(9m),
					new DynamicVar("Wishes", 3m)
				});
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060054BF RID: 21695 RVA: 0x0024745C File Offset: 0x0024565C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Wish>(false);
			}
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x00247464 File Offset: 0x00245664
		public override async Task AfterObtained()
		{
			await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, false);
			List<CardPileAddResult> wishResults = new List<CardPileAddResult>();
			for (int i = 0; i < base.DynamicVars["Wishes"].IntValue; i++)
			{
				wishResults.Add(await CardPileCmd.Add(base.Owner.RunState.CreateCard(ModelDb.Card<Wish>(), base.Owner), PileType.Deck, CardPilePosition.Bottom, null, false));
			}
			CardCmd.PreviewCardPileAdd(wishResults, 2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.75f, false);
		}

		// Token: 0x040024B9 RID: 9401
		public const int maxHpLoss = 9;

		// Token: 0x040024BA RID: 9402
		private const string _wishesKey = "Wishes";
	}
}
