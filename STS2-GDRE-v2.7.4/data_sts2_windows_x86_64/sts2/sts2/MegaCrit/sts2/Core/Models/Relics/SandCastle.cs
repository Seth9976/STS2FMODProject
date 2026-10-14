using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D8 RID: 1496
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SandCastle : RelicModel
	{
		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06005498 RID: 21656 RVA: 0x00246D2F File Offset: 0x00244F2F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06005499 RID: 21657 RVA: 0x00246D32 File Offset: 0x00244F32
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x0600549A RID: 21658 RVA: 0x00246D35 File Offset: 0x00244F35
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(6));
			}
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x00246D44 File Offset: 0x00244F44
		public override Task AfterObtained()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Niche)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.GridCardPreviewContainer.ForceMaxColumnsUntilEmpty(3);
			}
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.GridLayout);
			}
			return Task.CompletedTask;
		}
	}
}
