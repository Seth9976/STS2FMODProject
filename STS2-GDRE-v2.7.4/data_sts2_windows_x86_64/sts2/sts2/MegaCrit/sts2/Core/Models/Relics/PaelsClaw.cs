using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A3 RID: 1443
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsClaw : RelicModel
	{
		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x0600531E RID: 21278 RVA: 0x002441AB File Offset: 0x002423AB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x002441AE File Offset: 0x002423AE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new StringVar("EnchantmentName", ModelDb.Enchantment<Goopy>().Title.GetFormattedText())
				});
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06005320 RID: 21280 RVA: 0x002441E0 File Offset: 0x002423E0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Goopy>(2);
			}
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x002441E8 File Offset: 0x002423E8
		public override Task AfterObtained()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.ToList<CardModel>();
			foreach (CardModel cardModel in enumerable)
			{
				if (ModelDb.Enchantment<Goopy>().CanEnchant(cardModel))
				{
					CardCmd.Enchant<Goopy>(cardModel, 1m);
					NRun instance = NRun.Instance;
					if (instance != null)
					{
						instance.GlobalUi.CardPreviewContainer.AddChildSafely(NCardEnchantVfx.Create(cardModel));
					}
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x04002485 RID: 9349
		public const int cardsCount = 3;

		// Token: 0x04002486 RID: 9350
		private const int _enchantmentAmount = 2;
	}
}
