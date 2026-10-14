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
	// Token: 0x020008EF RID: 2287
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Silent : CharacterModel
	{
		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x06006D4E RID: 27982 RVA: 0x0028316C File Offset: 0x0028136C
		public override Color NameColor
		{
			get
			{
				return StsColors.green;
			}
		}

		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x06006D4F RID: 27983 RVA: 0x00283173 File Offset: 0x00281373
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Feminine;
			}
		}

		/// <remarks>
		/// You'll always unlock Silent by doing a run with Ironclad since he's the only character you start out with, but
		/// technically you unlock her by doing a run with any character.
		/// </remarks>
		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x06006D50 RID: 27984 RVA: 0x00283176 File Offset: 0x00281376
		[Nullable(2)]
		protected override CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x06006D51 RID: 27985 RVA: 0x00283179 File Offset: 0x00281379
		public override int StartingHp
		{
			get
			{
				return 70;
			}
		}

		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x06006D52 RID: 27986 RVA: 0x0028317D File Offset: 0x0028137D
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x06006D53 RID: 27987 RVA: 0x00283181 File Offset: 0x00281381
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<SilentCardPool>();
			}
		}

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x06006D54 RID: 27988 RVA: 0x00283188 File Offset: 0x00281388
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<SilentRelicPool>();
			}
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x06006D55 RID: 27989 RVA: 0x0028318F File Offset: 0x0028138F
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<SilentPotionPool>();
			}
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x06006D56 RID: 27990 RVA: 0x00283198 File Offset: 0x00281398
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<StrikeSilent>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<DefendSilent>(),
					ModelDb.Card<Neutralize>(),
					ModelDb.Card<Survivor>()
				});
			}
		}

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x06006D57 RID: 27991 RVA: 0x00283214 File Offset: 0x00281414
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<RingOfTheSnake>());
			}
		}

		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x06006D58 RID: 27992 RVA: 0x00283220 File Offset: 0x00281420
		public override float AttackAnimDelay
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x06006D59 RID: 27993 RVA: 0x00283227 File Offset: 0x00281427
		public override float CastAnimDelay
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x06006D5A RID: 27994 RVA: 0x00283230 File Offset: 0x00281430
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 4;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_dagger_spray";
			num2++;
			*span[num2] = "vfx/vfx_flying_slash";
			num2++;
			*span[num2] = "vfx/vfx_dramatic_stab";
			num2++;
			*span[num2] = "vfx/vfx_dagger_throw";
			return list;
		}

		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x06006D5B RID: 27995 RVA: 0x00283299 File Offset: 0x00281499
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("004f04FF");
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x06006D5C RID: 27996 RVA: 0x002832A5 File Offset: 0x002814A5
		public override Color DialogueColor
		{
			get
			{
				return new Color("284719");
			}
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x06006D5D RID: 27997 RVA: 0x002832B1 File Offset: 0x002814B1
		public override VfxColor SpeechBubbleColor
		{
			get
			{
				return VfxColor.Swamp;
			}
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x06006D5E RID: 27998 RVA: 0x002832B5 File Offset: 0x002814B5
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("2F6729");
			}
		}

		// Token: 0x17001C22 RID: 7202
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x002832C1 File Offset: 0x002814C1
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return new Color("2EBD5EFF");
			}
		}

		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x002832CD File Offset: 0x002814CD
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return new Color("004f04FF");
			}
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x002832DC File Offset: 0x002814DC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("shiv", false);
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
			creatureAnimator.AddAnyState("Shiv", animState6, null);
			creatureAnimator.AddAnyState("Relaxed", animState7, null);
			creatureAnimator.AddAnyState("PowerUp", animState2, null);
			return creatureAnimator;
		}

		// Token: 0x04002844 RID: 10308
		public const string shivTrigger = "Shiv";

		// Token: 0x04002845 RID: 10309
		public const string energyColorName = "silent";
	}
}
