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
	// Token: 0x020008EB RID: 2283
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ironclad : CharacterModel
	{
		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x06006CF0 RID: 27888 RVA: 0x00282844 File Offset: 0x00280A44
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Masculine;
			}
		}

		/// <remarks>
		/// Ironclad starts out unlocked.
		/// </remarks>
		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x06006CF1 RID: 27889 RVA: 0x00282847 File Offset: 0x00280A47
		[Nullable(2)]
		protected override CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x06006CF2 RID: 27890 RVA: 0x0028284A File Offset: 0x00280A4A
		public override Color NameColor
		{
			get
			{
				return StsColors.red;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x06006CF3 RID: 27891 RVA: 0x00282851 File Offset: 0x00280A51
		public override int StartingHp
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x06006CF4 RID: 27892 RVA: 0x00282855 File Offset: 0x00280A55
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x06006CF5 RID: 27893 RVA: 0x00282859 File Offset: 0x00280A59
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<IroncladCardPool>();
			}
		}

		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x00282860 File Offset: 0x00280A60
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<IroncladPotionPool>();
			}
		}

		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x06006CF7 RID: 27895 RVA: 0x00282867 File Offset: 0x00280A67
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<IroncladRelicPool>();
			}
		}

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x06006CF8 RID: 27896 RVA: 0x00282870 File Offset: 0x00280A70
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<StrikeIronclad>(),
					ModelDb.Card<DefendIronclad>(),
					ModelDb.Card<DefendIronclad>(),
					ModelDb.Card<DefendIronclad>(),
					ModelDb.Card<DefendIronclad>(),
					ModelDb.Card<Bash>()
				});
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x06006CF9 RID: 27897 RVA: 0x002828DA File Offset: 0x00280ADA
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<BurningBlood>());
			}
		}

		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x06006CFA RID: 27898 RVA: 0x002828E6 File Offset: 0x00280AE6
		public override float AttackAnimDelay
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x06006CFB RID: 27899 RVA: 0x002828ED File Offset: 0x00280AED
		public override float CastAnimDelay
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x06006CFC RID: 27900 RVA: 0x002828F4 File Offset: 0x00280AF4
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 5;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_attack_blunt";
			num2++;
			*span[num2] = "vfx/vfx_heavy_blunt";
			num2++;
			*span[num2] = "vfx/vfx_attack_slash";
			num2++;
			*span[num2] = "vfx/vfx_bloody_impact";
			num2++;
			*span[num2] = "vfx/vfx_rock_shatter";
			return list;
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x06006CFD RID: 27901 RVA: 0x0028296F File Offset: 0x00280B6F
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("801212FF");
			}
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x06006CFE RID: 27902 RVA: 0x0028297B File Offset: 0x00280B7B
		public override Color DialogueColor
		{
			get
			{
				return new Color("590700");
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x06006CFF RID: 27903 RVA: 0x00282987 File Offset: 0x00280B87
		public override VfxColor SpeechBubbleColor
		{
			get
			{
				return VfxColor.Red;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x06006D00 RID: 27904 RVA: 0x0028298A File Offset: 0x00280B8A
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("CB282B");
			}
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x06006D01 RID: 27905 RVA: 0x00282996 File Offset: 0x00280B96
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return new Color("E15847FF");
			}
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x06006D02 RID: 27906 RVA: 0x002829A2 File Offset: 0x00280BA2
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return new Color("801212FF");
			}
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x002829B0 File Offset: 0x00280BB0
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("attack_heavy", false);
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
			creatureAnimator.AddAnyState("heavyAttack", animState6, null);
			creatureAnimator.AddAnyState("PowerUp", animState2, null);
			creatureAnimator.AddAnyState("Relaxed", animState7, null);
			return creatureAnimator;
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x00282ABD File Offset: 0x00280CBD
		public static string GetHeavyAnimIfApplicable(CharacterModel character)
		{
			if (!(character is Ironclad))
			{
				return "Attack";
			}
			return "heavyAttack";
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x00282AD2 File Offset: 0x00280CD2
		public static float GetHeavyAttackDelayIfApplicable(CharacterModel character)
		{
			if (!(character is Ironclad))
			{
				return character.AttackAnimDelay;
			}
			return 0.2f;
		}

		// Token: 0x0400283B RID: 10299
		public const string heavyAttackTrigger = "heavyAttack";

		// Token: 0x0400283C RID: 10300
		public const string energyColorName = "ironclad";
	}
}
