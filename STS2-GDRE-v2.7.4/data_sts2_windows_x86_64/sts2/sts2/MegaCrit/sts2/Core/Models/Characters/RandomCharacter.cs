using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Characters
{
	// Token: 0x020008ED RID: 2285
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RandomCharacter : CharacterModel
	{
		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x06006D1F RID: 27935 RVA: 0x00282DB9 File Offset: 0x00280FB9
		public override bool IsPlayable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x06006D20 RID: 27936 RVA: 0x00282DBC File Offset: 0x00280FBC
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Neutral;
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x06006D21 RID: 27937 RVA: 0x00282DBF File Offset: 0x00280FBF
		[Nullable(2)]
		protected override CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x06006D22 RID: 27938 RVA: 0x00282DC2 File Offset: 0x00280FC2
		public override Color NameColor
		{
			get
			{
				return StsColors.gold;
			}
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x06006D23 RID: 27939 RVA: 0x00282DC9 File Offset: 0x00280FC9
		public override int StartingHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x06006D24 RID: 27940 RVA: 0x00282DCC File Offset: 0x00280FCC
		public override int StartingGold
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x00282DCF File Offset: 0x00280FCF
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<IroncladCardPool>();
			}
		}

		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x06006D26 RID: 27942 RVA: 0x00282DD6 File Offset: 0x00280FD6
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<IroncladPotionPool>();
			}
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x06006D27 RID: 27943 RVA: 0x00282DDD File Offset: 0x00280FDD
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<IroncladRelicPool>();
			}
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x06006D28 RID: 27944 RVA: 0x00282DE4 File Offset: 0x00280FE4
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<StrikeRegent>(),
					ModelDb.Card<StrikeNecrobinder>(),
					ModelDb.Card<StrikeDefect>(),
					ModelDb.Card<DefendIronclad>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<DefendRegent>(),
					ModelDb.Card<DefendNecrobinder>(),
					ModelDb.Card<DefendDefect>()
				});
			}
		}

		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x06006D29 RID: 27945 RVA: 0x00282E4E File Offset: 0x0028104E
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<Circlet>());
			}
		}

		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x00282E5A File Offset: 0x0028105A
		protected override string CharacterSelectIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/character_select/char_select_random.png");
			}
		}

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x06006D2B RID: 27947 RVA: 0x00282E66 File Offset: 0x00281066
		protected override string CharacterSelectLockedIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/character_select/char_select_random_locked.png");
			}
		}

		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x06006D2C RID: 27948 RVA: 0x00282E72 File Offset: 0x00281072
		public override float AttackAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x06006D2D RID: 27949 RVA: 0x00282E79 File Offset: 0x00281079
		public override float CastAnimDelay
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00282E80 File Offset: 0x00281080
		public override List<string> GetArchitectAttackVfx()
		{
			return new List<string>();
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x06006D2F RID: 27951 RVA: 0x00282E87 File Offset: 0x00281087
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return Colors.Magenta;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x06006D30 RID: 27952 RVA: 0x00282E8E File Offset: 0x0028108E
		public override Color DialogueColor
		{
			get
			{
				return Colors.Magenta;
			}
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x00282E95 File Offset: 0x00281095
		public override Color MapDrawingColor
		{
			get
			{
				return Colors.Magenta;
			}
		}

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x06006D32 RID: 27954 RVA: 0x00282E9C File Offset: 0x0028109C
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return Colors.Magenta;
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x06006D33 RID: 27955 RVA: 0x00282EA3 File Offset: 0x002810A3
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return Colors.Magenta;
			}
		}
	}
}
