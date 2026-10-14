using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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
	// Token: 0x020005EC RID: 1516
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StoneCracker : RelicModel
	{
		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06005527 RID: 21799 RVA: 0x00248127 File Offset: 0x00246327
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06005528 RID: 21800 RVA: 0x0024812A File Offset: 0x0024632A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x00248138 File Offset: 0x00246338
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				base.Flash();
				List<CardModel> list = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.CombatCardSelection)
					.Take<CardModel>(base.DynamicVars.Cards.IntValue)
					.ToList<CardModel>();
				CardCmd.Upgrade(list, CardPreviewStyle.HorizontalLayout);
				CardCmd.Preview(list, 1.2f, CardPreviewStyle.HorizontalLayout);
				await Cmd.CustomScaledWait(0.5f, 1f, false, default(CancellationToken));
			}
		}
	}
}
