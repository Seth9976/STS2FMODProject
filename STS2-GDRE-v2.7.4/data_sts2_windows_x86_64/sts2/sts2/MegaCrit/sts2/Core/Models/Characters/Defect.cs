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
	// Token: 0x020008E8 RID: 2280
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Defect : CharacterModel
	{
		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x06006CAF RID: 27823 RVA: 0x002823C2 File Offset: 0x002805C2
		public override Color NameColor
		{
			get
			{
				return StsColors.blue;
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x06006CB0 RID: 27824 RVA: 0x002823C9 File Offset: 0x002805C9
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Neutral;
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x06006CB1 RID: 27825 RVA: 0x002823CC File Offset: 0x002805CC
		protected override CharacterModel UnlocksAfterRunAs
		{
			get
			{
				return ModelDb.Character<Necrobinder>();
			}
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x06006CB2 RID: 27826 RVA: 0x002823D3 File Offset: 0x002805D3
		public override int StartingHp
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x06006CB3 RID: 27827 RVA: 0x002823D7 File Offset: 0x002805D7
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x06006CB4 RID: 27828 RVA: 0x002823DB File Offset: 0x002805DB
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<DefectCardPool>();
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x06006CB5 RID: 27829 RVA: 0x002823E2 File Offset: 0x002805E2
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<DefectRelicPool>();
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x06006CB6 RID: 27830 RVA: 0x002823E9 File Offset: 0x002805E9
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<DefectPotionPool>();
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x06006CB7 RID: 27831 RVA: 0x002823F0 File Offset: 0x002805F0
		public static Vector2 EyelineOffset
		{
			get
			{
				return new Vector2(34f, -30f);
			}
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x06006CB8 RID: 27832 RVA: 0x00282404 File Offset: 0x00280604
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeDefect>(),
					ModelDb.Card<StrikeDefect>(),
					ModelDb.Card<StrikeDefect>(),
					ModelDb.Card<StrikeDefect>(),
					ModelDb.Card<DefendDefect>(),
					ModelDb.Card<DefendDefect>(),
					ModelDb.Card<DefendDefect>(),
					ModelDb.Card<DefendDefect>(),
					ModelDb.Card<Zap>(),
					ModelDb.Card<Dualcast>()
				});
			}
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x06006CB9 RID: 27833 RVA: 0x0028246E File Offset: 0x0028066E
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<CrackedCore>());
			}
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x06006CBA RID: 27834 RVA: 0x0028247A File Offset: 0x0028067A
		public override float AttackAnimDelay
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x06006CBB RID: 27835 RVA: 0x00282481 File Offset: 0x00280681
		public override float CastAnimDelay
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x06006CBC RID: 27836 RVA: 0x00282488 File Offset: 0x00280688
		public override float PowerUpAnimDelay
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x00282490 File Offset: 0x00280690
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 5;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_attack_lightning";
			num2++;
			*span[num2] = "vfx/vfx_attack_blunt";
			num2++;
			*span[num2] = "vfx/vfx_scratch";
			num2++;
			*span[num2] = "vfx/vfx_attack_slash";
			num2++;
			*span[num2] = "vfx/vfx_heavy_blunt";
			return list;
		}

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x06006CBE RID: 27838 RVA: 0x0028250B File Offset: 0x0028070B
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("163E64FF");
			}
		}

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x06006CBF RID: 27839 RVA: 0x00282517 File Offset: 0x00280717
		public override int BaseOrbSlotCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x06006CC0 RID: 27840 RVA: 0x0028251A File Offset: 0x0028071A
		public override Color DialogueColor
		{
			get
			{
				return new Color("13446B");
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x06006CC1 RID: 27841 RVA: 0x00282526 File Offset: 0x00280726
		public override VfxColor SpeechBubbleColor
		{
			get
			{
				return VfxColor.Blue;
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x06006CC2 RID: 27842 RVA: 0x00282529 File Offset: 0x00280729
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("0D638C");
			}
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x06006CC3 RID: 27843 RVA: 0x00282535 File Offset: 0x00280735
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return new Color("70B6EDFF");
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x06006CC4 RID: 27844 RVA: 0x00282541 File Offset: 0x00280741
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return new Color("163E64FF");
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x06006CC5 RID: 27845 RVA: 0x0028254D File Offset: 0x0028074D
		public override string CharacterTransitionSfx
		{
			get
			{
				return "event:/sfx/ui/wipe_ironclad";
			}
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x00282554 File Offset: 0x00280754
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("process", false);
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
			creatureAnimator.AddAnyState("PowerUp", animState6, null);
			creatureAnimator.AddAnyState("Relaxed", animState7, null);
			return creatureAnimator;
		}

		// Token: 0x04002838 RID: 10296
		public const string energyColorName = "defect";
	}
}
