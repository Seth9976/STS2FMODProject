using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MegaCrit.Sts2.Core.Models.Characters
{
	// Token: 0x020008EA RID: 2282
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deprived : CharacterModel
	{
		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x06006CDC RID: 27868 RVA: 0x00282745 File Offset: 0x00280945
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x06006CDD RID: 27869 RVA: 0x00282748 File Offset: 0x00280948
		public override bool IsPlayable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x06006CDE RID: 27870 RVA: 0x0028274B File Offset: 0x0028094B
		public override Color NameColor
		{
			get
			{
				return StsColors.gold;
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x06006CDF RID: 27871 RVA: 0x00282752 File Offset: 0x00280952
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Neutral;
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x06006CE0 RID: 27872 RVA: 0x00282755 File Offset: 0x00280955
		[Nullable(2)]
		protected override CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x06006CE1 RID: 27873 RVA: 0x00282758 File Offset: 0x00280958
		public override int StartingHp
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x06006CE2 RID: 27874 RVA: 0x0028275F File Offset: 0x0028095F
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x06006CE3 RID: 27875 RVA: 0x00282763 File Offset: 0x00280963
		public override int MaxEnergy
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x06006CE4 RID: 27876 RVA: 0x00282767 File Offset: 0x00280967
		public override CardPoolModel CardPool
		{
			get
			{
				if (this._mockCardPool != null)
				{
					return this._mockCardPool;
				}
				return ModelDb.CardPool<DeprivedCardPool>();
			}
		}

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x06006CE5 RID: 27877 RVA: 0x0028277D File Offset: 0x0028097D
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<IroncladRelicPool>();
			}
		}

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x06006CE6 RID: 27878 RVA: 0x00282784 File Offset: 0x00280984
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<IroncladPotionPool>();
			}
		}

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x06006CE7 RID: 27879 RVA: 0x0028278B File Offset: 0x0028098B
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return Array.Empty<CardModel>();
			}
		}

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x06006CE8 RID: 27880 RVA: 0x00282792 File Offset: 0x00280992
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return Array.Empty<RelicModel>();
			}
		}

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x06006CE9 RID: 27881 RVA: 0x00282799 File Offset: 0x00280999
		public override float AttackAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x06006CEA RID: 27882 RVA: 0x002827A0 File Offset: 0x002809A0
		public override float CastAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x002827A7 File Offset: 0x002809A7
		public override List<string> GetArchitectAttackVfx()
		{
			return new List<string>();
		}

		// Token: 0x06006CEC RID: 27884 RVA: 0x002827AE File Offset: 0x002809AE
		public void ResetMockCardPool()
		{
			this._mockCardPool = null;
		}

		// Token: 0x06006CED RID: 27885 RVA: 0x002827B8 File Offset: 0x002809B8
		public void SetMockCardPool(IEnumerable<CardModel> cards)
		{
			if (this._mockCardPool != null)
			{
				throw new InvalidOperationException("Mock card pool already initialized");
			}
			this._mockCardPool = (MockCardPool)ModelDb.CardPool<MockCardPool>().ToMutable();
			foreach (CardModel cardModel in cards)
			{
				this._mockCardPool.Add(cardModel);
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x06006CEE RID: 27886 RVA: 0x00282830 File Offset: 0x00280A30
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("462996");
			}
		}

		// Token: 0x0400283A RID: 10298
		[Nullable(2)]
		private MockCardPool _mockCardPool;
	}
}
