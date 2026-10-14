using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200054D RID: 1357
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FragrantMushroom : RelicModel
	{
		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x060050E5 RID: 20709 RVA: 0x0023FDDB File Offset: 0x0023DFDB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x060050E6 RID: 20710 RVA: 0x0023FDDE File Offset: 0x0023DFDE
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x060050E7 RID: 20711 RVA: 0x0023FDE1 File Offset: 0x0023DFE1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(15m),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x0023FE08 File Offset: 0x0023E008
		public override async Task AfterObtained()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Niche)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.MessyLayout);
			}
		}

		// Token: 0x0400244B RID: 9291
		public const int hpLoss = 15;
	}
}
