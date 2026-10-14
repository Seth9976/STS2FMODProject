using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Characters
{
	// Token: 0x020008EE RID: 2286
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Regent : CharacterModel
	{
		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x06006D35 RID: 27957 RVA: 0x00282EB2 File Offset: 0x002810B2
		public override Color NameColor
		{
			get
			{
				return StsColors.orange;
			}
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x06006D36 RID: 27958 RVA: 0x00282EB9 File Offset: 0x002810B9
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Masculine;
			}
		}

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x06006D37 RID: 27959 RVA: 0x00282EBC File Offset: 0x002810BC
		protected override CharacterModel UnlocksAfterRunAs
		{
			get
			{
				return ModelDb.Character<Silent>();
			}
		}

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x06006D38 RID: 27960 RVA: 0x00282EC3 File Offset: 0x002810C3
		public override int StartingHp
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x06006D39 RID: 27961 RVA: 0x00282EC7 File Offset: 0x002810C7
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x06006D3A RID: 27962 RVA: 0x00282ECB File Offset: 0x002810CB
		public override bool ShouldAlwaysShowStarCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x06006D3B RID: 27963 RVA: 0x00282ECE File Offset: 0x002810CE
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<RegentCardPool>();
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x06006D3C RID: 27964 RVA: 0x00282ED5 File Offset: 0x002810D5
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<RegentRelicPool>();
			}
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x06006D3D RID: 27965 RVA: 0x00282EDC File Offset: 0x002810DC
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<RegentPotionPool>();
			}
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x06006D3E RID: 27966 RVA: 0x00282EE4 File Offset: 0x002810E4
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeRegent>(),
					ModelDb.Card<StrikeRegent>(),
					ModelDb.Card<StrikeRegent>(),
					ModelDb.Card<StrikeRegent>(),
					ModelDb.Card<DefendRegent>(),
					ModelDb.Card<DefendRegent>(),
					ModelDb.Card<DefendRegent>(),
					ModelDb.Card<DefendRegent>(),
					ModelDb.Card<FallingStar>(),
					ModelDb.Card<Venerate>()
				});
			}
		}

		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x06006D3F RID: 27967 RVA: 0x00282F4E File Offset: 0x0028114E
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<DivineRight>());
			}
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x06006D40 RID: 27968 RVA: 0x00282F5A File Offset: 0x0028115A
		public override float AttackAnimDelay
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x06006D41 RID: 27969 RVA: 0x00282F61 File Offset: 0x00281161
		public override float CastAnimDelay
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x00282F68 File Offset: 0x00281168
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 5;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_starry_impact";
			num2++;
			*span[num2] = "vfx/vfx_attack_blunt";
			num2++;
			*span[num2] = "vfx/vfx_attack_slash";
			num2++;
			*span[num2] = "vfx/vfx_heavy_blunt";
			num2++;
			*span[num2] = "vfx/vfx_attack_lightning";
			return list;
		}

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x06006D43 RID: 27971 RVA: 0x00282FE3 File Offset: 0x002811E3
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("784000FF");
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x06006D44 RID: 27972 RVA: 0x00282FEF File Offset: 0x002811EF
		public override Color DialogueColor
		{
			get
			{
				return new Color("52371D");
			}
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x06006D45 RID: 27973 RVA: 0x00282FFB File Offset: 0x002811FB
		public override VfxColor SpeechBubbleColor
		{
			get
			{
				return VfxColor.Orange;
			}
		}

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x06006D46 RID: 27974 RVA: 0x00282FFE File Offset: 0x002811FE
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("935206");
			}
		}

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x06006D47 RID: 27975 RVA: 0x0028300A File Offset: 0x0028120A
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return new Color("BFA270FF");
			}
		}

		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x06006D48 RID: 27976 RVA: 0x00283016 File Offset: 0x00281216
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return new Color("784000FF");
			}
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x06006D49 RID: 27977 RVA: 0x00283022 File Offset: 0x00281222
		public override string CharacterTransitionSfx
		{
			get
			{
				return "event:/sfx/ui/wipe_ironclad";
			}
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x0028302C File Offset: 0x0028122C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("attack_sovereign", false);
			AnimState animState7 = new AnimState("relaxed_loop", true);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.NextState = animState;
			animState7.AddBranch("Idle", animState, null);
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("sovereignBladeTrigger", animState6, null);
			creatureAnimator.AddAnyState("Relaxed", animState7, null);
			creatureAnimator.AddAnyState("PowerUp", animState2, null);
			return creatureAnimator;
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x00283139 File Offset: 0x00281339
		public static string GetSovereignBladeAnimIfApplicable(CharacterModel character)
		{
			if (!(character is Regent))
			{
				return "Cast";
			}
			return "sovereignBladeTrigger";
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x0028314E File Offset: 0x0028134E
		public static float GetSovereignBladeDelayIfApplicable(CharacterModel character)
		{
			if (!(character is Regent))
			{
				return character.CastAnimDelay;
			}
			return 0.25f;
		}

		// Token: 0x04002841 RID: 10305
		public const string sovereignBladeTrigger = "sovereignBladeTrigger";

		// Token: 0x04002842 RID: 10306
		public const float sovereignBladeAnimDelay = 0.25f;

		// Token: 0x04002843 RID: 10307
		public const string energyColorName = "regent";
	}
}
