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
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000613 RID: 1555
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WarPaint : RelicModel
	{
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06005623 RID: 22051 RVA: 0x00249EF8 File Offset: 0x002480F8
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06005624 RID: 22052 RVA: 0x00249EFB File Offset: 0x002480FB
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06005625 RID: 22053 RVA: 0x00249EFE File Offset: 0x002480FE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x00249F0C File Offset: 0x0024810C
		public override Task AfterObtained()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.Type == CardType.Skill && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Niche)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			return Task.CompletedTask;
		}
	}
}
