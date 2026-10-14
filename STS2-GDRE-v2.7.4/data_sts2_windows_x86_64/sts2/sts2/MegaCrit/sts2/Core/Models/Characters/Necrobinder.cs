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

namespace MegaCrit.Sts2.Core.Models.Characters
{
	// Token: 0x020008EC RID: 2284
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Necrobinder : CharacterModel
	{
		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x06006D07 RID: 27911 RVA: 0x00282AF0 File Offset: 0x00280CF0
		public override Color NameColor
		{
			get
			{
				return StsColors.purple;
			}
		}

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x06006D08 RID: 27912 RVA: 0x00282AF7 File Offset: 0x00280CF7
		public override CharacterGender Gender
		{
			get
			{
				return CharacterGender.Feminine;
			}
		}

		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x06006D09 RID: 27913 RVA: 0x00282AFA File Offset: 0x00280CFA
		protected override CharacterModel UnlocksAfterRunAs
		{
			get
			{
				return ModelDb.Character<Regent>();
			}
		}

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x06006D0A RID: 27914 RVA: 0x00282B01 File Offset: 0x00280D01
		public override int StartingHp
		{
			get
			{
				return 66;
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x06006D0B RID: 27915 RVA: 0x00282B05 File Offset: 0x00280D05
		public override int StartingGold
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x06006D0C RID: 27916 RVA: 0x00282B09 File Offset: 0x00280D09
		public override CardPoolModel CardPool
		{
			get
			{
				return ModelDb.CardPool<NecrobinderCardPool>();
			}
		}

		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x06006D0D RID: 27917 RVA: 0x00282B10 File Offset: 0x00280D10
		public override RelicPoolModel RelicPool
		{
			get
			{
				return ModelDb.RelicPool<NecrobinderRelicPool>();
			}
		}

		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x06006D0E RID: 27918 RVA: 0x00282B17 File Offset: 0x00280D17
		public override PotionPoolModel PotionPool
		{
			get
			{
				return ModelDb.PotionPool<NecrobinderPotionPool>();
			}
		}

		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x06006D0F RID: 27919 RVA: 0x00282B1E File Offset: 0x00280D1E
		protected override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					SceneHelper.GetScenePath("vfx/vfx_heal_osty"),
					SceneHelper.GetScenePath("creature_visuals/osty")
				});
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x06006D10 RID: 27920 RVA: 0x00282B48 File Offset: 0x00280D48
		public override IEnumerable<CardModel> StartingDeck
		{
			get
			{
				return new <>z__ReadOnlyArray<CardModel>(new CardModel[]
				{
					ModelDb.Card<StrikeNecrobinder>(),
					ModelDb.Card<StrikeNecrobinder>(),
					ModelDb.Card<StrikeNecrobinder>(),
					ModelDb.Card<StrikeNecrobinder>(),
					ModelDb.Card<DefendNecrobinder>(),
					ModelDb.Card<DefendNecrobinder>(),
					ModelDb.Card<DefendNecrobinder>(),
					ModelDb.Card<DefendNecrobinder>(),
					ModelDb.Card<Bodyguard>(),
					ModelDb.Card<Unleash>()
				});
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x06006D11 RID: 27921 RVA: 0x00282BB2 File Offset: 0x00280DB2
		public override IReadOnlyList<RelicModel> StartingRelics
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<BoundPhylactery>());
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x06006D12 RID: 27922 RVA: 0x00282BBE File Offset: 0x00280DBE
		public override float AttackAnimDelay
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x06006D13 RID: 27923 RVA: 0x00282BC5 File Offset: 0x00280DC5
		public override float CastAnimDelay
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x00282BCC File Offset: 0x00280DCC
		public unsafe override List<string> GetArchitectAttackVfx()
		{
			int num = 4;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "vfx/vfx_thrash";
			num2++;
			*span[num2] = "vfx/vfx_heavy_blunt";
			num2++;
			*span[num2] = "vfx/vfx_attack_slash";
			num2++;
			*span[num2] = "vfx/vfx_bloody_impact";
			return list;
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x06006D15 RID: 27925 RVA: 0x00282C35 File Offset: 0x00280E35
		public override Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("702D6FFF");
			}
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x06006D16 RID: 27926 RVA: 0x00282C41 File Offset: 0x00280E41
		public override Color DialogueColor
		{
			get
			{
				return new Color("6B4658");
			}
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x06006D17 RID: 27927 RVA: 0x00282C4D File Offset: 0x00280E4D
		public override Color MapDrawingColor
		{
			get
			{
				return new Color("AC0486");
			}
		}

		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x06006D18 RID: 27928 RVA: 0x00282C59 File Offset: 0x00280E59
		public override Color RemoteTargetingLineColor
		{
			get
			{
				return new Color("FD98C9FF");
			}
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x06006D19 RID: 27929 RVA: 0x00282C65 File Offset: 0x00280E65
		public override Color RemoteTargetingLineOutline
		{
			get
			{
				return new Color("702D6FFF");
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x06006D1A RID: 27930 RVA: 0x00282C71 File Offset: 0x00280E71
		public override string CharacterTransitionSfx
		{
			get
			{
				return "event:/sfx/ui/wipe_ironclad";
			}
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x00282C78 File Offset: 0x00280E78
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("cast_mighty", false);
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
			creatureAnimator.AddAnyState("summonTrigger", animState2, null);
			creatureAnimator.AddAnyState("Cast", animState6, null);
			creatureAnimator.AddAnyState("Relaxed", animState7, null);
			creatureAnimator.AddAnyState("PowerUp", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x00282D86 File Offset: 0x00280F86
		public static string GetSummonAnimIfApplicable(CharacterModel character)
		{
			if (!(character is Necrobinder))
			{
				return "Cast";
			}
			return "summonTrigger";
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x00282D9B File Offset: 0x00280F9B
		public static float GetSummonDelayIfApplicable(CharacterModel character)
		{
			if (!(character is Necrobinder))
			{
				return character.CastAnimDelay;
			}
			return 0.25f;
		}

		// Token: 0x0400283D RID: 10301
		private const string _summonTrigger = "summonTrigger";

		// Token: 0x0400283E RID: 10302
		public const string energyColorName = "necrobinder";

		// Token: 0x0400283F RID: 10303
		public const string healOstyPath = "vfx/vfx_heal_osty";

		// Token: 0x04002840 RID: 10304
		private const string _ostyVisualsPath = "creature_visuals/osty";
	}
}
