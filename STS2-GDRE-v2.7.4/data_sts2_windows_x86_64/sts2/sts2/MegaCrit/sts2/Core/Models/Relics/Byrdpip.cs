using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000516 RID: 1302
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Byrdpip : RelicModel
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x0023DD88 File Offset: 0x0023BF88
		public override bool AddsPet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x0023DD8B File Offset: 0x0023BF8B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x0023DD8E File Offset: 0x0023BF8E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x0023DD91 File Offset: 0x0023BF91
		public override bool SpawnsPets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x0023DD94 File Offset: 0x0023BF94
		public static string[] SkinOptions
		{
			get
			{
				return new string[] { "version1", "version2", "version3", "version4" };
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x0023DDBC File Offset: 0x0023BFBC
		// (set) Token: 0x06004FB7 RID: 20407 RVA: 0x0023DDC4 File Offset: 0x0023BFC4
		[SavedProperty]
		public string Skin
		{
			get
			{
				return this._skin;
			}
			set
			{
				base.AssertMutable();
				this._skin = value;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x0023DDD3 File Offset: 0x0023BFD3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<ByrdSwoop>(false);
			}
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x0023DDDC File Offset: 0x0023BFDC
		public override async Task AfterObtained()
		{
			this.Skin = new Rng(base.Owner, base.Id, 0UL).NextItem<string>(Byrdpip.SkinOptions);
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c is ByrdonisEgg).ToList<CardModel>();
			if (CombatManager.Instance.IsInProgress)
			{
				list.AddRange(base.Owner.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c is ByrdonisEgg));
			}
			foreach (CardModel cardModel in list)
			{
				await CardCmd.TransformTo<ByrdSwoop>(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			if (CombatManager.Instance.IsInProgress)
			{
				await this.SummonPet();
			}
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x0023DE20 File Offset: 0x0023C020
		public override async Task BeforeCombatStart()
		{
			await this.SummonPet();
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x0023DE64 File Offset: 0x0023C064
		private async Task SummonPet()
		{
			await PlayerCmd.AddPet<Byrdpip>(base.Owner);
		}

		// Token: 0x04002432 RID: 9266
		private string _skin = Byrdpip.SkinOptions[0];
	}
}
