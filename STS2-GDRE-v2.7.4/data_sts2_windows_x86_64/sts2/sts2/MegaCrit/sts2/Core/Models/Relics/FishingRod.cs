using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200054B RID: 1355
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FishingRod : RelicModel
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x0023FC52 File Offset: 0x0023DE52
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x0023FC55 File Offset: 0x0023DE55
		public override int DisplayAmount
		{
			get
			{
				return this.CombatsSeen % base.DynamicVars["Combats"].IntValue;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x060050DB RID: 20699 RVA: 0x0023FC73 File Offset: 0x0023DE73
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x0023FC76 File Offset: 0x0023DE76
		// (set) Token: 0x060050DD RID: 20701 RVA: 0x0023FC7E File Offset: 0x0023DE7E
		[SavedProperty]
		public int CombatsSeen
		{
			get
			{
				return this._combatsSeen;
			}
			set
			{
				base.AssertMutable();
				this._combatsSeen = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x060050DE RID: 20702 RVA: 0x0023FC93 File Offset: 0x0023DE93
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 3m));
			}
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x0023FCAC File Offset: 0x0023DEAC
		public override Task AfterCombatEnd(CombatRoom room)
		{
			if (room.Encounter.RoomType != RoomType.Monster)
			{
				return Task.CompletedTask;
			}
			int combatsSeen = this.CombatsSeen;
			this.CombatsSeen = combatsSeen + 1;
			if (this.CombatsSeen % base.DynamicVars["Combats"].IntValue == 0)
			{
				base.Flash();
				IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable);
				CardModel cardModel = base.Owner.RunState.Rng.Niche.NextItem<CardModel>(enumerable);
				if (cardModel != null)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x04002449 RID: 9289
		private const string _combatsKey = "Combats";

		// Token: 0x0400244A RID: 9290
		private int _combatsSeen;
	}
}
