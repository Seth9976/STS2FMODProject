using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Cards.Mocks;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B4F RID: 2895
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprivedCardPool : CardPoolModel
	{
		// Token: 0x1700219A RID: 8602
		// (get) Token: 0x06007A57 RID: 31319 RVA: 0x0029D9CE File Offset: 0x0029BBCE
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700219B RID: 8603
		// (get) Token: 0x06007A58 RID: 31320 RVA: 0x0029D9D1 File Offset: 0x0029BBD1
		public override string Title
		{
			get
			{
				return "test";
			}
		}

		// Token: 0x1700219C RID: 8604
		// (get) Token: 0x06007A59 RID: 31321 RVA: 0x0029D9D8 File Offset: 0x0029BBD8
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x1700219D RID: 8605
		// (get) Token: 0x06007A5A RID: 31322 RVA: 0x0029D9DF File Offset: 0x0029BBDF
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x1700219E RID: 8606
		// (get) Token: 0x06007A5B RID: 31323 RVA: 0x0029D9E6 File Offset: 0x0029BBE6
		public override Color DeckEntryCardColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x1700219F RID: 8607
		// (get) Token: 0x06007A5C RID: 31324 RVA: 0x0029D9ED File Offset: 0x0029BBED
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A5D RID: 31325 RVA: 0x0029D9F0 File Offset: 0x0029BBF0
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Common),
				DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Uncommon),
				DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Rare),
				DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Common),
				DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Uncommon),
				DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Rare),
				DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Common),
				DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Uncommon),
				DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Rare),
				DeprivedCardPool.MockCard<MockQuestCard>(CardRarity.Quest),
				DeprivedCardPool.MockCard<MockCurseCard>(CardRarity.Curse),
				DeprivedCardPool.MockCard<MockStatusCard>(CardRarity.Status)
			};
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x0029DA75 File Offset: 0x0029BC75
		private static MockCardModel MockCard<[Nullable(0)] T>(CardRarity rarity) where T : MockCardModel
		{
			return ((MockCardModel)ModelDb.Card<T>().ToMutable()).MockRarity(rarity).MockCanonical();
		}
	}
}
