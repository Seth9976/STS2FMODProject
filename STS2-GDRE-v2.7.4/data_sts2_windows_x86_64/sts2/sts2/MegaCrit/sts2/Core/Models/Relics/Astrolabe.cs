using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F8 RID: 1272
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Astrolabe : RelicModel
	{
		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004EF0 RID: 20208 RVA: 0x0023C63F File Offset: 0x0023A83F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x0023C642 File Offset: 0x0023A842
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x0023C645 File Offset: 0x0023A845
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x0023C654 File Offset: 0x0023A854
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				CardModel cardModel2 = CardFactory.CreateRandomCardForTransform(cardModel, false, base.Owner.RunState.Rng.Niche);
				CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
				await CardCmd.Transform(cardModel, cardModel2, CardPreviewStyle.HorizontalLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}
	}
}
