using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B45 RID: 2885
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MockCardModel : CardModel
	{
		// Token: 0x060079E7 RID: 31207 RVA: 0x0029CAB6 File Offset: 0x0029ACB6
		protected MockCardModel()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002163 RID: 8547
		// (get) Token: 0x060079E8 RID: 31208 RVA: 0x0029CAD8 File Offset: 0x0029ACD8
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002164 RID: 8548
		// (get) Token: 0x060079E9 RID: 31209 RVA: 0x0029CADB File Offset: 0x0029ACDB
		protected override int CanonicalEnergyCost
		{
			get
			{
				return this._mockEnergyCost;
			}
		}

		// Token: 0x17002165 RID: 8549
		// (get) Token: 0x060079EA RID: 31210 RVA: 0x0029CAE3 File Offset: 0x0029ACE3
		protected override bool HasEnergyCostX
		{
			get
			{
				return this._mockEnergyCostX;
			}
		}

		// Token: 0x17002166 RID: 8550
		// (get) Token: 0x060079EB RID: 31211 RVA: 0x0029CAEB File Offset: 0x0029ACEB
		public override int CanonicalStarCost
		{
			get
			{
				return this._mockStarCost;
			}
		}

		// Token: 0x17002167 RID: 8551
		// (get) Token: 0x060079EC RID: 31212 RVA: 0x0029CAF3 File Offset: 0x0029ACF3
		public override bool HasStarCostX
		{
			get
			{
				return this._mockStarCostX;
			}
		}

		// Token: 0x17002168 RID: 8552
		// (get) Token: 0x060079ED RID: 31213 RVA: 0x0029CAFB File Offset: 0x0029ACFB
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return this._mockMultiplayerConstraint;
			}
		}

		// Token: 0x17002169 RID: 8553
		// (get) Token: 0x060079EE RID: 31214 RVA: 0x0029CB03 File Offset: 0x0029AD03
		public override bool GainsBlock
		{
			get
			{
				return this.GetBaseBlock() > 0;
			}
		}

		// Token: 0x1700216A RID: 8554
		// (get) Token: 0x060079EF RID: 31215 RVA: 0x0029CB0E File Offset: 0x0029AD0E
		public override int MaxUpgradeLevel
		{
			get
			{
				return this._mockMaxUpgradeLevel;
			}
		}

		// Token: 0x1700216B RID: 8555
		// (get) Token: 0x060079F0 RID: 31216 RVA: 0x0029CB16 File Offset: 0x0029AD16
		public override CardRarity Rarity
		{
			get
			{
				return this._mockRarity;
			}
		}

		// Token: 0x1700216C RID: 8556
		// (get) Token: 0x060079F1 RID: 31217 RVA: 0x0029CB1E File Offset: 0x0029AD1E
		public override CardPoolModel Pool
		{
			get
			{
				return this._mockPool ?? ModelDb.CardPool<DeprivedCardPool>();
			}
		}

		// Token: 0x1700216D RID: 8557
		// (get) Token: 0x060079F2 RID: 31218 RVA: 0x0029CB2F File Offset: 0x0029AD2F
		public override IEnumerable<CardTag> Tags
		{
			get
			{
				return this._mockTags ?? new HashSet<CardTag>();
			}
		}

		// Token: 0x060079F3 RID: 31219
		public abstract MockCardModel MockBlock(int block);

		// Token: 0x060079F4 RID: 31220 RVA: 0x0029CB40 File Offset: 0x0029AD40
		public MockCardModel MockCanonical()
		{
			base.AssertMutable();
			ICombatState combatState = base.CombatState;
			if (combatState != null)
			{
				combatState.RemoveCard(this);
			}
			base.NeverEverCallThisOutsideOfTests_ClearOwner();
			base.NeverEverCallThisOutsideOfTests_SetIsMutable(false);
			return this;
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x0029CB68 File Offset: 0x0029AD68
		public MockCardModel MockEnergyCost(int cost)
		{
			base.AssertMutable();
			this._mockEnergyCost = cost;
			this._mockEnergyCostX = false;
			base.MockSetEnergyCost(new CardEnergyCost(this, cost, false));
			return this;
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x0029CB8D File Offset: 0x0029AD8D
		public MockCardModel MockMultiplayerType(CardMultiplayerConstraint constraint)
		{
			base.AssertMutable();
			this._mockMultiplayerConstraint = constraint;
			return this;
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x0029CB9D File Offset: 0x0029AD9D
		public MockCardModel MockEnergyCostX()
		{
			base.AssertMutable();
			this._mockEnergyCostX = true;
			this._mockEnergyCost = 0;
			base.MockSetEnergyCost(new CardEnergyCost(this, 0, true));
			return this;
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x0029CBC2 File Offset: 0x0029ADC2
		public MockCardModel MockStarCost(int cost)
		{
			base.AssertMutable();
			this._mockStarCost = cost;
			this._mockStarCostX = false;
			return this;
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x0029CBD9 File Offset: 0x0029ADD9
		public MockCardModel MockStarCostX()
		{
			base.AssertMutable();
			this._mockStarCostX = true;
			this._mockStarCost = 0;
			return this;
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x0029CBF0 File Offset: 0x0029ADF0
		public MockCardModel MockExtraLogic(Func<CardModel, Task> extraLogic)
		{
			base.AssertMutable();
			this._mockExtraLogic = extraLogic;
			return this;
		}

		// Token: 0x060079FB RID: 31227 RVA: 0x0029CC00 File Offset: 0x0029AE00
		public MockCardModel MockKeyword(CardKeyword keyword)
		{
			base.AssertMutable();
			base.AddKeyword(keyword);
			return this;
		}

		// Token: 0x060079FC RID: 31228 RVA: 0x0029CC10 File Offset: 0x0029AE10
		public MockCardModel MockReplay(int count)
		{
			base.AssertMutable();
			base.BaseReplayCount = count;
			return this;
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x0029CC20 File Offset: 0x0029AE20
		public MockCardModel MockRarity(CardRarity rarity)
		{
			base.AssertMutable();
			this._mockRarity = rarity;
			return this;
		}

		// Token: 0x060079FE RID: 31230 RVA: 0x0029CC30 File Offset: 0x0029AE30
		public MockCardModel MockPool<[Nullable(0)] T>() where T : CardPoolModel
		{
			base.AssertMutable();
			this._mockPool = ModelDb.CardPool<T>();
			return this;
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x0029CC49 File Offset: 0x0029AE49
		public MockCardModel MockTag(CardTag tag)
		{
			base.AssertMutable();
			if (this._mockTags == null)
			{
				this._mockTags = new HashSet<CardTag>();
			}
			this._mockTags.Add(tag);
			return this;
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x0029CC72 File Offset: 0x0029AE72
		public MockCardModel MockSelfHpLoss(int hpLoss)
		{
			base.AssertMutable();
			this._mockSelfHpLoss = hpLoss;
			return this;
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x0029CC82 File Offset: 0x0029AE82
		public MockCardModel MockUnUpgradable()
		{
			base.AssertMutable();
			this._mockMaxUpgradeLevel = 0;
			return this;
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x0029CC92 File Offset: 0x0029AE92
		public MockCardModel MockUpgradeLogic(Action<CardModel> upgradeLogic)
		{
			base.AssertMutable();
			this._mockUpgradeLogic = upgradeLogic;
			return this;
		}

		// Token: 0x06007A03 RID: 31235
		protected abstract int GetBaseBlock();

		// Token: 0x040028DF RID: 10463
		protected int _mockEnergyCost = 1;

		// Token: 0x040028E0 RID: 10464
		protected CardMultiplayerConstraint _mockMultiplayerConstraint;

		// Token: 0x040028E1 RID: 10465
		protected bool _mockEnergyCostX;

		// Token: 0x040028E2 RID: 10466
		protected int _mockStarCost;

		// Token: 0x040028E3 RID: 10467
		protected bool _mockStarCostX;

		// Token: 0x040028E4 RID: 10468
		[Nullable(new byte[] { 2, 1, 1 })]
		protected Func<CardModel, Task> _mockExtraLogic;

		// Token: 0x040028E5 RID: 10469
		protected int _mockMaxUpgradeLevel = 1;

		// Token: 0x040028E6 RID: 10470
		protected CardRarity _mockRarity = CardRarity.Common;

		// Token: 0x040028E7 RID: 10471
		protected int _mockSelfHpLoss;

		// Token: 0x040028E8 RID: 10472
		[Nullable(2)]
		protected HashSet<CardTag> _mockTags;

		// Token: 0x040028E9 RID: 10473
		[Nullable(2)]
		protected CardPoolModel _mockPool;

		// Token: 0x040028EA RID: 10474
		[Nullable(new byte[] { 2, 1 })]
		protected Action<CardModel> _mockUpgradeLogic;
	}
}
