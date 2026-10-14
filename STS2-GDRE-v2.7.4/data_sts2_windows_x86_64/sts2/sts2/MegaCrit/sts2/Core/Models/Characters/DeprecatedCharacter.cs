using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MegaCrit.Sts2.Core.Models.Characters
{
	/// <summary>
	/// Placeholder character for if we can't find the character we are looking for (like if it's been deprecated).
	/// </summary>
	// Token: 0x020008E9 RID: 2281
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedCharacter : CharacterModel
	{
		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x06006CC8 RID: 27848 RVA: 0x0028265B File Offset: 0x0028085B
		public override bool IsPlayable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x0028265E File Offset: 0x0028085E
		public override Color NameColor
		{
			get
			{
				return StsColors.gold;
			}
		}

		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x06006CCA RID: 27850 RVA: 0x00282665 File Offset: 0x00280865
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Neutral;
			}
		}

		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x06006CCB RID: 27851 RVA: 0x00282668 File Offset: 0x00280868
		[Nullable(2)]
		protected override CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x06006CCC RID: 27852 RVA: 0x0028266B File Offset: 0x0028086B
		protected override string IconPath
		{
			get
			{
				return SceneHelper.GetScenePath("ui/character_icons/ironclad_icon");
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x06006CCD RID: 27853 RVA: 0x00282677 File Offset: 0x00280877
		public override int StartingHp
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x06006CCE RID: 27854 RVA: 0x0028267E File Offset: 0x0028087E
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x06006CCF RID: 27855 RVA: 0x00282682 File Offset: 0x00280882
		public override int MaxEnergy
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x06006CD0 RID: 27856 RVA: 0x00282686 File Offset: 0x00280886
		public override CardPoolModel CardPool
		{
			get
			{
				return this._mockCardPool ?? ModelDb.CardPool<MockCardPool>();
			}
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x00282697 File Offset: 0x00280897
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<IroncladRelicPool>();
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x0028269E File Offset: 0x0028089E
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<IroncladPotionPool>();
			}
		}

		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002826A5 File Offset: 0x002808A5
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return Array.Empty<CardModel>();
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x002826AC File Offset: 0x002808AC
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return Array.Empty<RelicModel>();
			}
		}

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x06006CD5 RID: 27861 RVA: 0x002826B3 File Offset: 0x002808B3
		public override float AttackAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x06006CD6 RID: 27862 RVA: 0x002826BA File Offset: 0x002808BA
		public override float CastAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x002826C4 File Offset: 0x002808C4
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_attack_blunt";
			return list;
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x002826F7 File Offset: 0x002808F7
		public void ResetMockCardPool()
		{
			this._mockCardPool = null;
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x00282700 File Offset: 0x00280900
		public void AddToPool(CardModel card)
		{
			card.AssertCanonical();
			if (this._mockCardPool == null)
			{
				this._mockCardPool = (MockCardPool)ModelDb.CardPool<MockCardPool>().ToMutable();
			}
			this._mockCardPool.Add(card);
		}

		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x06006CDA RID: 27866 RVA: 0x00282731 File Offset: 0x00280931
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("462996");
			}
		}

		// Token: 0x04002839 RID: 10297
		[Nullable(2)]
		private MockCardPool _mockCardPool;
	}
}
