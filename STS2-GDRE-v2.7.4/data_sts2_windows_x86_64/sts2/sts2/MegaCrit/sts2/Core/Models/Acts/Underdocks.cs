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
	// Token: 0x02000B84 RID: 2948
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Underdocks : ActModel
	{
		// Token: 0x06007B7A RID: 31610 RVA: 0x002A0C20 File Offset: 0x0029EE20
		public override IEnumerable<EncounterModel> GenerateAllEncounters()
		{
			return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
			{
				ModelDb.Encounter<CorpseSlugsNormal>(),
				ModelDb.Encounter<CorpseSlugsWeak>(),
				ModelDb.Encounter<CultistsNormal>(),
				ModelDb.Encounter<FossilStalkerNormal>(),
				ModelDb.Encounter<GremlinMercNormal>(),
				ModelDb.Encounter<HauntedShipNormal>(),
				ModelDb.Encounter<LagavulinMatriarchBoss>(),
				ModelDb.Encounter<LivingFogNormal>(),
				ModelDb.Encounter<PhantasmalGardenersElite>(),
				ModelDb.Encounter<PunchConstructNormal>(),
				ModelDb.Encounter<SeapunkNormal>(),
				ModelDb.Encounter<SeapunkWeak>(),
				ModelDb.Encounter<SewerClamNormal>(),
				ModelDb.Encounter<SkulkingColonyElite>(),
				ModelDb.Encounter<SludgeSpinnerWeak>(),
				ModelDb.Encounter<SoulFyshBoss>(),
				ModelDb.Encounter<TerrorEelElite>(),
				ModelDb.Encounter<ToadpolesWeak>(),
				ModelDb.Encounter<TwoTailedRatsNormal>(),
				ModelDb.Encounter<WaterfallGiantBoss>()
			});
		}

		// Token: 0x17002243 RID: 8771
		// (get) Token: 0x06007B7B RID: 31611 RVA: 0x002A0CE4 File Offset: 0x0029EEE4
		public override IEnumerable<EncounterModel> BossDiscoveryOrder
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
				{
					ModelDb.Encounter<WaterfallGiantBoss>(),
					ModelDb.Encounter<SoulFyshBoss>(),
					ModelDb.Encounter<LagavulinMatriarchBoss>()
				});
			}
		}

		// Token: 0x17002244 RID: 8772
		// (get) Token: 0x06007B7C RID: 31612 RVA: 0x002A0D09 File Offset: 0x0029EF09
		public override IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<AncientEventModel>(ModelDb.AncientEvent<Neow>());
			}
		}

		// Token: 0x17002245 RID: 8773
		// (get) Token: 0x06007B7D RID: 31613 RVA: 0x002A0D15 File Offset: 0x0029EF15
		public override string ChestOpenSfx
		{
			get
			{
				return "event:/sfx/ui/treasure/treasure_act1";
			}
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x002A0D1C File Offset: 0x0029EF1C
		public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
		{
			List<AncientEventModel> list = this.AllAncients.ToList<AncientEventModel>();
			if (!unlockState.IsEpochRevealed<NeowEpoch>())
			{
				list.Remove(ModelDb.AncientEvent<Neow>());
			}
			return list;
		}

		// Token: 0x17002246 RID: 8774
		// (get) Token: 0x06007B7F RID: 31615 RVA: 0x002A0D4C File Offset: 0x0029EF4C
		public override IEnumerable<EventModel> AllEvents
		{
			get
			{
				return new <>z__ReadOnlyArray<EventModel>(new EventModel[]
				{
					ModelDb.Event<AbyssalBaths>(),
					ModelDb.Event<DrowningBeacon>(),
					ModelDb.Event<EndlessConveyor>(),
					ModelDb.Event<PunchOff>(),
					ModelDb.Event<SpiralingWhirlpool>(),
					ModelDb.Event<SunkenStatue>(),
					ModelDb.Event<SunkenTreasury>(),
					ModelDb.Event<DoorsOfLightAndDark>(),
					ModelDb.Event<TrashHeap>(),
					ModelDb.Event<WaterloggedScriptorium>()
				});
			}
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x002A0DB6 File Offset: 0x0029EFB6
		protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
		{
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x002A0DB8 File Offset: 0x0029EFB8
		public override bool IsUnlocked(UnlockState unlockState)
		{
			return unlockState.IsEpochRevealed<UnderdocksEpoch>();
		}

		// Token: 0x17002247 RID: 8775
		// (get) Token: 0x06007B82 RID: 31618 RVA: 0x002A0DC0 File Offset: 0x0029EFC0
		protected override int NumberOfWeakEncounters
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17002248 RID: 8776
		// (get) Token: 0x06007B83 RID: 31619 RVA: 0x002A0DC3 File Offset: 0x0029EFC3
		protected override int BaseNumberOfRooms
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17002249 RID: 8777
		// (get) Token: 0x06007B84 RID: 31620 RVA: 0x002A0DC7 File Offset: 0x0029EFC7
		public override int Index
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700224A RID: 8778
		// (get) Token: 0x06007B85 RID: 31621 RVA: 0x002A0DCA File Offset: 0x0029EFCA
		public override bool IsDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700224B RID: 8779
		// (get) Token: 0x06007B86 RID: 31622 RVA: 0x002A0DCD File Offset: 0x0029EFCD
		public override string[] BgMusicOptions
		{
			get
			{
				return new string[] { "event:/music/act1_b1_v1" };
			}
		}

		// Token: 0x1700224C RID: 8780
		// (get) Token: 0x06007B87 RID: 31623 RVA: 0x002A0DDD File Offset: 0x0029EFDD
		public override string[] MusicBankPaths
		{
			get
			{
				return new string[] { "res://banks/desktop/act1_b1.bank" };
			}
		}

		// Token: 0x1700224D RID: 8781
		// (get) Token: 0x06007B88 RID: 31624 RVA: 0x002A0DED File Offset: 0x0029EFED
		public override string AmbientSfx
		{
			get
			{
				return "event:/sfx/ambience/act3_ambience";
			}
		}

		// Token: 0x1700224E RID: 8782
		// (get) Token: 0x06007B89 RID: 31625 RVA: 0x002A0DF4 File Offset: 0x0029EFF4
		public override string ChestSpineResourcePath
		{
			get
			{
				return "res://animations/backgrounds/treasure_room/chest_room_act_1_skel_data.tres";
			}
		}

		// Token: 0x1700224F RID: 8783
		// (get) Token: 0x06007B8A RID: 31626 RVA: 0x002A0DFB File Offset: 0x0029EFFB
		public override string ChestSpineSkinNameNormal
		{
			get
			{
				return "act1";
			}
		}

		// Token: 0x17002250 RID: 8784
		// (get) Token: 0x06007B8B RID: 31627 RVA: 0x002A0E02 File Offset: 0x0029F002
		public override string ChestSpineSkinNameStroke
		{
			get
			{
				return "act1_stroke";
			}
		}

		// Token: 0x17002251 RID: 8785
		// (get) Token: 0x06007B8C RID: 31628 RVA: 0x002A0E09 File Offset: 0x0029F009
		public override Color MapTraveledColor
		{
			get
			{
				return new Color("180F24");
			}
		}

		// Token: 0x17002252 RID: 8786
		// (get) Token: 0x06007B8D RID: 31629 RVA: 0x002A0E15 File Offset: 0x0029F015
		public override Color MapUntraveledColor
		{
			get
			{
				return new Color("534A62");
			}
		}

		// Token: 0x17002253 RID: 8787
		// (get) Token: 0x06007B8E RID: 31630 RVA: 0x002A0E21 File Offset: 0x0029F021
		public override Color MapBgColor
		{
			get
			{
				return new Color("9F95A5");
			}
		}

		// Token: 0x06007B8F RID: 31631 RVA: 0x002A0E30 File Offset: 0x0029F030
		public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
		{
			int num = mapRng.NextGaussianInt(7, 1, 6, 7);
			int num2 = MapPointTypeCounts.StandardRandomUnknownCount(mapRng);
			return new MapPointTypeCounts(num2, num);
		}
	}
}
