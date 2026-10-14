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
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000612 RID: 1554
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WarHammer : RelicModel
	{
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x00249E14 File Offset: 0x00248014
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06005620 RID: 22048 RVA: 0x00249E17 File Offset: 0x00248017
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(4));
			}
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x00249E24 File Offset: 0x00248024
		public override Task AfterCombatVictory(CombatRoom room)
		{
			if (room.RoomType != RoomType.Elite)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Niche)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			return Task.CompletedTask;
		}
	}
}
