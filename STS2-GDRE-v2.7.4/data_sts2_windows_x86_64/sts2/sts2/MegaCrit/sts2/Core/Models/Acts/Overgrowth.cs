using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.Acts
{
	// Token: 0x02000B83 RID: 2947
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Overgrowth : ActModel
	{
		// Token: 0x06007B63 RID: 31587 RVA: 0x002A08C8 File Offset: 0x0029EAC8
		public override IEnumerable<EncounterModel> GenerateAllEncounters()
		{
			return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
			{
				ModelDb.Encounter<BygoneEffigyElite>(),
				ModelDb.Encounter<ByrdonisElite>(),
				ModelDb.Encounter<CeremonialBeastBoss>(),
				ModelDb.Encounter<CubexConstructNormal>(),
				ModelDb.Encounter<FlyconidNormal>(),
				ModelDb.Encounter<FogmogNormal>(),
				ModelDb.Encounter<FuzzyWurmCrawlerWeak>(),
				ModelDb.Encounter<InkletsNormal>(),
				ModelDb.Encounter<MawlerNormal>(),
				ModelDb.Encounter<NibbitsNormal>(),
				ModelDb.Encounter<NibbitsWeak>(),
				ModelDb.Encounter<OvergrowthCrawlers>(),
				ModelDb.Encounter<PhrogParasiteElite>(),
				ModelDb.Encounter<RubyRaidersNormal>(),
				ModelDb.Encounter<ShrinkerBeetleWeak>(),
				ModelDb.Encounter<SlimesNormal>(),
				ModelDb.Encounter<SlimesWeak>(),
				ModelDb.Encounter<SlitheringStranglerNormal>(),
				ModelDb.Encounter<SnappingJaxfruitNormal>(),
				ModelDb.Encounter<TheKinBoss>(),
				ModelDb.Encounter<VantomBoss>(),
				ModelDb.Encounter<VineShamblerNormal>()
			});
		}

		// Token: 0x17002232 RID: 8754
		// (get) Token: 0x06007B64 RID: 31588 RVA: 0x002A099E File Offset: 0x0029EB9E
		public override string ChestOpenSfx
		{
			get
			{
				return "event:/sfx/ui/treasure/treasure_act1";
			}
		}

		// Token: 0x17002233 RID: 8755
		// (get) Token: 0x06007B65 RID: 31589 RVA: 0x002A09A5 File Offset: 0x0029EBA5
		public override IEnumerable<EncounterModel> BossDiscoveryOrder
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
				{
					ModelDb.Encounter<VantomBoss>(),
					ModelDb.Encounter<CeremonialBeastBoss>(),
					ModelDb.Encounter<TheKinBoss>()
				});
			}
		}

		// Token: 0x17002234 RID: 8756
		// (get) Token: 0x06007B66 RID: 31590 RVA: 0x002A09CA File Offset: 0x0029EBCA
		public override IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<AncientEventModel>(ModelDb.AncientEvent<Neow>());
			}
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x002A09D8 File Offset: 0x0029EBD8
		public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
		{
			List<AncientEventModel> list = this.AllAncients.ToList<AncientEventModel>();
			if (!unlockState.IsEpochRevealed<NeowEpoch>())
			{
				list.Remove(ModelDb.AncientEvent<Neow>());
			}
			return list;
		}

		// Token: 0x17002235 RID: 8757
		// (get) Token: 0x06007B68 RID: 31592 RVA: 0x002A0A08 File Offset: 0x0029EC08
		public override IEnumerable<EventModel> AllEvents
		{
			get
			{
				return new <>z__ReadOnlyArray<EventModel>(new EventModel[]
				{
					ModelDb.Event<AromaOfChaos>(),
					ModelDb.Event<ByrdonisNest>(),
					ModelDb.Event<DenseVegetation>(),
					ModelDb.Event<JungleMazeAdventure>(),
					ModelDb.Event<LuminousChoir>(),
					ModelDb.Event<MorphicGrove>(),
					ModelDb.Event<SapphireSeed>(),
					ModelDb.Event<SunkenStatue>(),
					ModelDb.Event<TabletOfTruth>(),
					ModelDb.Event<UnrestSite>(),
					ModelDb.Event<Wellspring>(),
					ModelDb.Event<WhisperingHollow>(),
					ModelDb.Event<WoodCarvings>()
				});
			}
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x002A0A90 File Offset: 0x0029EC90
		protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
		{
			if (unlockState.NumberOfRuns == 0)
			{
				Log.Info("First run ever. Presenting rooms in a set order.", 2);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, NibbitsWeak>(this._rooms.normalEncounters, 0);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, SlimesWeak>(this._rooms.normalEncounters, 1);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, ShrinkerBeetleWeak>(this._rooms.normalEncounters, 2);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, InkletsNormal>(this._rooms.normalEncounters, 3);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, MawlerNormal>(this._rooms.normalEncounters, 4);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, RubyRaidersNormal>(this._rooms.normalEncounters, 5);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, NibbitsNormal>(this._rooms.normalEncounters, 6);
				RoomSet.SwapToOrCreateAtIndex<EventModel, ByrdonisNest>(this._rooms.events, 0);
				RoomSet.SwapToOrCreateAtIndex<EventModel, SapphireSeed>(this._rooms.events, 1);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, ByrdonisElite>(this._rooms.eliteEncounters, 0);
				RoomSet.SwapToOrCreateAtIndex<EncounterModel, PhrogParasiteElite>(this._rooms.eliteEncounters, 1);
			}
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x002A0B6E File Offset: 0x0029ED6E
		public override bool IsUnlocked(UnlockState unlockState)
		{
			return true;
		}

		// Token: 0x17002236 RID: 8758
		// (get) Token: 0x06007B6B RID: 31595 RVA: 0x002A0B71 File Offset: 0x0029ED71
		protected override int NumberOfWeakEncounters
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17002237 RID: 8759
		// (get) Token: 0x06007B6C RID: 31596 RVA: 0x002A0B74 File Offset: 0x0029ED74
		protected override int BaseNumberOfRooms
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17002238 RID: 8760
		// (get) Token: 0x06007B6D RID: 31597 RVA: 0x002A0B78 File Offset: 0x0029ED78
		public override int Index
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002239 RID: 8761
		// (get) Token: 0x06007B6E RID: 31598 RVA: 0x002A0B7B File Offset: 0x0029ED7B
		public override bool IsDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700223A RID: 8762
		// (get) Token: 0x06007B6F RID: 31599 RVA: 0x002A0B7E File Offset: 0x0029ED7E
		public override string[] BgMusicOptions
		{
			get
			{
				return new string[] { "event:/music/act1_a1_v1", "event:/music/act1_a2_v2" };
			}
		}

		// Token: 0x1700223B RID: 8763
		// (get) Token: 0x06007B70 RID: 31600 RVA: 0x002A0B96 File Offset: 0x0029ED96
		public override string[] MusicBankPaths
		{
			get
			{
				return new string[] { "res://banks/desktop/act1_a1.bank", "res://banks/desktop/act1_a2.bank" };
			}
		}

		// Token: 0x1700223C RID: 8764
		// (get) Token: 0x06007B71 RID: 31601 RVA: 0x002A0BAE File Offset: 0x0029EDAE
		public override string AmbientSfx
		{
			get
			{
				return "event:/sfx/ambience/act1_ambience";
			}
		}

		// Token: 0x1700223D RID: 8765
		// (get) Token: 0x06007B72 RID: 31602 RVA: 0x002A0BB5 File Offset: 0x0029EDB5
		public override string ChestSpineResourcePath
		{
			get
			{
				return "res://animations/backgrounds/treasure_room/chest_room_act_1_skel_data.tres";
			}
		}

		// Token: 0x1700223E RID: 8766
		// (get) Token: 0x06007B73 RID: 31603 RVA: 0x002A0BBC File Offset: 0x0029EDBC
		public override string ChestSpineSkinNameNormal
		{
			get
			{
				return "act1";
			}
		}

		// Token: 0x1700223F RID: 8767
		// (get) Token: 0x06007B74 RID: 31604 RVA: 0x002A0BC3 File Offset: 0x0029EDC3
		public override string ChestSpineSkinNameStroke
		{
			get
			{
				return "act1_stroke";
			}
		}

		// Token: 0x17002240 RID: 8768
		// (get) Token: 0x06007B75 RID: 31605 RVA: 0x002A0BCA File Offset: 0x0029EDCA
		public override Color MapTraveledColor
		{
			get
			{
				return new Color("28231D");
			}
		}

		// Token: 0x17002241 RID: 8769
		// (get) Token: 0x06007B76 RID: 31606 RVA: 0x002A0BD6 File Offset: 0x0029EDD6
		public override Color MapUntraveledColor
		{
			get
			{
				return new Color("877256");
			}
		}

		// Token: 0x17002242 RID: 8770
		// (get) Token: 0x06007B77 RID: 31607 RVA: 0x002A0BE2 File Offset: 0x0029EDE2
		public override Color MapBgColor
		{
			get
			{
				return new Color("A78A67");
			}
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x002A0BF0 File Offset: 0x0029EDF0
		public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
		{
			int num = mapRng.NextGaussianInt(7, 1, 6, 7);
			int num2 = MapPointTypeCounts.StandardRandomUnknownCount(mapRng);
			return new MapPointTypeCounts(num2, num);
		}
	}
}
