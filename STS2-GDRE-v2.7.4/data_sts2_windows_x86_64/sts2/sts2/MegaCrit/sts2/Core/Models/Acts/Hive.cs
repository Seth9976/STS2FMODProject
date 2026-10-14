using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.Acts
{
	// Token: 0x02000B82 RID: 2946
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hive : ActModel
	{
		// Token: 0x06007B4C RID: 31564 RVA: 0x002A0664 File Offset: 0x0029E864
		public override IEnumerable<EncounterModel> GenerateAllEncounters()
		{
			return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
			{
				ModelDb.Encounter<BowlbugsNormal>(),
				ModelDb.Encounter<BowlbugsWeak>(),
				ModelDb.Encounter<ChompersNormal>(),
				ModelDb.Encounter<DecimillipedeElite>(),
				ModelDb.Encounter<EntomancerElite>(),
				ModelDb.Encounter<ExoskeletonsNormal>(),
				ModelDb.Encounter<ExoskeletonsWeak>(),
				ModelDb.Encounter<HunterKillerNormal>(),
				ModelDb.Encounter<KaiserCrabBoss>(),
				ModelDb.Encounter<InfestedPrismsElite>(),
				ModelDb.Encounter<KnowledgeDemonBoss>(),
				ModelDb.Encounter<LouseProgenitorNormal>(),
				ModelDb.Encounter<MytesNormal>(),
				ModelDb.Encounter<OvicopterNormal>(),
				ModelDb.Encounter<SlumberingBeetleNormal>(),
				ModelDb.Encounter<SpinyToadNormal>(),
				ModelDb.Encounter<TheInsatiableBoss>(),
				ModelDb.Encounter<TheObscuraNormal>(),
				ModelDb.Encounter<ThievingHopperWeak>(),
				ModelDb.Encounter<TunnelerWeak>()
			});
		}

		// Token: 0x17002221 RID: 8737
		// (get) Token: 0x06007B4D RID: 31565 RVA: 0x002A0728 File Offset: 0x0029E928
		public override string ChestOpenSfx
		{
			get
			{
				return "event:/sfx/ui/treasure/treasure_act2";
			}
		}

		// Token: 0x17002222 RID: 8738
		// (get) Token: 0x06007B4E RID: 31566 RVA: 0x002A072F File Offset: 0x0029E92F
		public override IEnumerable<EncounterModel> BossDiscoveryOrder
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
				{
					ModelDb.Encounter<TheInsatiableBoss>(),
					ModelDb.Encounter<KnowledgeDemonBoss>(),
					ModelDb.Encounter<KaiserCrabBoss>()
				});
			}
		}

		// Token: 0x17002223 RID: 8739
		// (get) Token: 0x06007B4F RID: 31567 RVA: 0x002A0754 File Offset: 0x0029E954
		public override IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return new <>z__ReadOnlyArray<AncientEventModel>(new AncientEventModel[]
				{
					ModelDb.AncientEvent<Orobas>(),
					ModelDb.AncientEvent<Pael>(),
					ModelDb.AncientEvent<Tezcatara>()
				});
			}
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x002A077C File Offset: 0x0029E97C
		public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
		{
			List<AncientEventModel> list = this.AllAncients.ToList<AncientEventModel>();
			if (!unlockState.IsEpochRevealed<OrobasEpoch>())
			{
				list.Remove(ModelDb.AncientEvent<Orobas>());
			}
			return list;
		}

		// Token: 0x17002224 RID: 8740
		// (get) Token: 0x06007B51 RID: 31569 RVA: 0x002A07AC File Offset: 0x0029E9AC
		public override IEnumerable<EventModel> AllEvents
		{
			get
			{
				return new <>z__ReadOnlyArray<EventModel>(new EventModel[]
				{
					ModelDb.Event<Amalgamator>(),
					ModelDb.Event<Bugslayer>(),
					ModelDb.Event<ColorfulPhilosophers>(),
					ModelDb.Event<ColossalFlower>(),
					ModelDb.Event<FieldOfManSizedHoles>(),
					ModelDb.Event<InfestedAutomaton>(),
					ModelDb.Event<LostWisp>(),
					ModelDb.Event<SpiritGrafter>(),
					ModelDb.Event<TheLanternKey>(),
					ModelDb.Event<ZenWeaver>()
				});
			}
		}

		// Token: 0x17002225 RID: 8741
		// (get) Token: 0x06007B52 RID: 31570 RVA: 0x002A0816 File Offset: 0x0029EA16
		protected override int NumberOfWeakEncounters
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17002226 RID: 8742
		// (get) Token: 0x06007B53 RID: 31571 RVA: 0x002A0819 File Offset: 0x0029EA19
		protected override int BaseNumberOfRooms
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17002227 RID: 8743
		// (get) Token: 0x06007B54 RID: 31572 RVA: 0x002A081D File Offset: 0x0029EA1D
		public override int Index
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17002228 RID: 8744
		// (get) Token: 0x06007B55 RID: 31573 RVA: 0x002A0820 File Offset: 0x0029EA20
		public override bool IsDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002229 RID: 8745
		// (get) Token: 0x06007B56 RID: 31574 RVA: 0x002A0823 File Offset: 0x0029EA23
		public override string[] BgMusicOptions
		{
			get
			{
				return new string[] { "event:/music/act2_a1_v2", "event:/music/act2_a2_v2" };
			}
		}

		// Token: 0x1700222A RID: 8746
		// (get) Token: 0x06007B57 RID: 31575 RVA: 0x002A083B File Offset: 0x0029EA3B
		public override string[] MusicBankPaths
		{
			get
			{
				return new string[] { "res://banks/desktop/act2_a1.bank", "res://banks/desktop/act2_a2.bank" };
			}
		}

		// Token: 0x1700222B RID: 8747
		// (get) Token: 0x06007B58 RID: 31576 RVA: 0x002A0853 File Offset: 0x0029EA53
		public override string AmbientSfx
		{
			get
			{
				return "event:/sfx/ambience/act2_ambience";
			}
		}

		// Token: 0x1700222C RID: 8748
		// (get) Token: 0x06007B59 RID: 31577 RVA: 0x002A085A File Offset: 0x0029EA5A
		public override string ChestSpineResourcePath
		{
			get
			{
				return "res://animations/backgrounds/treasure_room/chest_room_act_2_skel_data.tres";
			}
		}

		// Token: 0x1700222D RID: 8749
		// (get) Token: 0x06007B5A RID: 31578 RVA: 0x002A0861 File Offset: 0x0029EA61
		public override string ChestSpineSkinNameNormal
		{
			get
			{
				return "act2";
			}
		}

		// Token: 0x1700222E RID: 8750
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x002A0868 File Offset: 0x0029EA68
		public override string ChestSpineSkinNameStroke
		{
			get
			{
				return "act2_stroke";
			}
		}

		// Token: 0x06007B5C RID: 31580 RVA: 0x002A086F File Offset: 0x0029EA6F
		protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
		{
		}

		// Token: 0x06007B5D RID: 31581 RVA: 0x002A0871 File Offset: 0x0029EA71
		public override bool IsUnlocked(UnlockState unlockState)
		{
			return true;
		}

		// Token: 0x1700222F RID: 8751
		// (get) Token: 0x06007B5E RID: 31582 RVA: 0x002A0874 File Offset: 0x0029EA74
		public override Color MapTraveledColor
		{
			get
			{
				return new Color("27221C");
			}
		}

		// Token: 0x17002230 RID: 8752
		// (get) Token: 0x06007B5F RID: 31583 RVA: 0x002A0880 File Offset: 0x0029EA80
		public override Color MapUntraveledColor
		{
			get
			{
				return new Color("6E7750");
			}
		}

		// Token: 0x17002231 RID: 8753
		// (get) Token: 0x06007B60 RID: 31584 RVA: 0x002A088C File Offset: 0x0029EA8C
		public override Color MapBgColor
		{
			get
			{
				return new Color("9B9562");
			}
		}

		// Token: 0x06007B61 RID: 31585 RVA: 0x002A0898 File Offset: 0x0029EA98
		public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
		{
			int num = mapRng.NextGaussianInt(6, 1, 6, 7);
			int num2 = MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1;
			return new MapPointTypeCounts(num2, num);
		}
	}
}
