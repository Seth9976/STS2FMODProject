using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.Acts
{
	// Token: 0x02000B81 RID: 2945
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glory : ActModel
	{
		// Token: 0x06007B35 RID: 31541 RVA: 0x002A0440 File Offset: 0x0029E640
		public override IEnumerable<EncounterModel> GenerateAllEncounters()
		{
			return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
			{
				ModelDb.Encounter<AxebotsNormal>(),
				ModelDb.Encounter<ConstructMenagerieNormal>(),
				ModelDb.Encounter<DevotedSculptorWeak>(),
				ModelDb.Encounter<AeonglassBoss>(),
				ModelDb.Encounter<FabricatorNormal>(),
				ModelDb.Encounter<FrogKnightNormal>(),
				ModelDb.Encounter<GlobeHeadNormal>(),
				ModelDb.Encounter<KnightsElite>(),
				ModelDb.Encounter<MechaKnightElite>(),
				ModelDb.Encounter<OwlMagistrateNormal>(),
				ModelDb.Encounter<QueenBoss>(),
				ModelDb.Encounter<ScrollsOfBitingNormal>(),
				ModelDb.Encounter<ScrollsOfBitingWeak>(),
				ModelDb.Encounter<SlimedBerserkerNormal>(),
				ModelDb.Encounter<SoulNexusElite>(),
				ModelDb.Encounter<TestSubjectBoss>(),
				ModelDb.Encounter<TheLostAndForgottenNormal>(),
				ModelDb.Encounter<TurretOperatorWeak>()
			});
		}

		// Token: 0x17002210 RID: 8720
		// (get) Token: 0x06007B36 RID: 31542 RVA: 0x002A04F2 File Offset: 0x0029E6F2
		public override string ChestOpenSfx
		{
			get
			{
				return "event:/sfx/ui/treasure/treasure_act3";
			}
		}

		// Token: 0x17002211 RID: 8721
		// (get) Token: 0x06007B37 RID: 31543 RVA: 0x002A04F9 File Offset: 0x0029E6F9
		public override IEnumerable<EncounterModel> BossDiscoveryOrder
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
				{
					ModelDb.Encounter<QueenBoss>(),
					ModelDb.Encounter<TestSubjectBoss>(),
					ModelDb.Encounter<AeonglassBoss>()
				});
			}
		}

		// Token: 0x17002212 RID: 8722
		// (get) Token: 0x06007B38 RID: 31544 RVA: 0x002A051E File Offset: 0x0029E71E
		public override IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return new <>z__ReadOnlyArray<AncientEventModel>(new AncientEventModel[]
				{
					ModelDb.AncientEvent<Nonupeipe>(),
					ModelDb.AncientEvent<Tanx>(),
					ModelDb.AncientEvent<Vakuu>()
				});
			}
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x002A0544 File Offset: 0x0029E744
		public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
		{
			return this.AllAncients.ToList<AncientEventModel>();
		}

		// Token: 0x17002213 RID: 8723
		// (get) Token: 0x06007B3A RID: 31546 RVA: 0x002A0560 File Offset: 0x0029E760
		public override IEnumerable<EventModel> AllEvents
		{
			get
			{
				return new <>z__ReadOnlyArray<EventModel>(new EventModel[]
				{
					ModelDb.Event<BattlewornDummy>(),
					ModelDb.Event<GraveOfTheForgotten>(),
					ModelDb.Event<HungryForMushrooms>(),
					ModelDb.Event<Reflections>(),
					ModelDb.Event<RoundTeaParty>(),
					ModelDb.Event<Trial>(),
					ModelDb.Event<TinkerTime>()
				});
			}
		}

		// Token: 0x17002214 RID: 8724
		// (get) Token: 0x06007B3B RID: 31547 RVA: 0x002A05B0 File Offset: 0x0029E7B0
		protected override int NumberOfWeakEncounters
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17002215 RID: 8725
		// (get) Token: 0x06007B3C RID: 31548 RVA: 0x002A05B3 File Offset: 0x0029E7B3
		protected override int BaseNumberOfRooms
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x17002216 RID: 8726
		// (get) Token: 0x06007B3D RID: 31549 RVA: 0x002A05B7 File Offset: 0x0029E7B7
		public override int Index
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17002217 RID: 8727
		// (get) Token: 0x06007B3E RID: 31550 RVA: 0x002A05BA File Offset: 0x0029E7BA
		public override bool IsDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002218 RID: 8728
		// (get) Token: 0x06007B3F RID: 31551 RVA: 0x002A05BD File Offset: 0x0029E7BD
		public override string[] BgMusicOptions
		{
			get
			{
				return new string[] { "event:/music/act3_a1_v1", "event:/music/act3_a2_v1" };
			}
		}

		// Token: 0x17002219 RID: 8729
		// (get) Token: 0x06007B40 RID: 31552 RVA: 0x002A05D5 File Offset: 0x0029E7D5
		public override string[] MusicBankPaths
		{
			get
			{
				return new string[] { "res://banks/desktop/act3_a1.bank", "res://banks/desktop/act3_a2.bank" };
			}
		}

		// Token: 0x1700221A RID: 8730
		// (get) Token: 0x06007B41 RID: 31553 RVA: 0x002A05ED File Offset: 0x0029E7ED
		public override string AmbientSfx
		{
			get
			{
				return "event:/sfx/ambience/act3_ambience";
			}
		}

		// Token: 0x1700221B RID: 8731
		// (get) Token: 0x06007B42 RID: 31554 RVA: 0x002A05F4 File Offset: 0x0029E7F4
		public override string ChestSpineResourcePath
		{
			get
			{
				return "res://animations/backgrounds/treasure_room/chest_room_act_3_skel_data.tres";
			}
		}

		// Token: 0x1700221C RID: 8732
		// (get) Token: 0x06007B43 RID: 31555 RVA: 0x002A05FB File Offset: 0x0029E7FB
		public override string ChestSpineSkinNameNormal
		{
			get
			{
				return "act3";
			}
		}

		// Token: 0x1700221D RID: 8733
		// (get) Token: 0x06007B44 RID: 31556 RVA: 0x002A0602 File Offset: 0x0029E802
		public override string ChestSpineSkinNameStroke
		{
			get
			{
				return "act3_stroke";
			}
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x002A0609 File Offset: 0x0029E809
		protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
		{
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x002A060B File Offset: 0x0029E80B
		public override bool IsUnlocked(UnlockState unlockState)
		{
			return true;
		}

		// Token: 0x1700221E RID: 8734
		// (get) Token: 0x06007B47 RID: 31559 RVA: 0x002A060E File Offset: 0x0029E80E
		public override Color MapTraveledColor
		{
			get
			{
				return new Color("1D1E2F");
			}
		}

		// Token: 0x1700221F RID: 8735
		// (get) Token: 0x06007B48 RID: 31560 RVA: 0x002A061A File Offset: 0x0029E81A
		public override Color MapUntraveledColor
		{
			get
			{
				return new Color("60717C");
			}
		}

		// Token: 0x17002220 RID: 8736
		// (get) Token: 0x06007B49 RID: 31561 RVA: 0x002A0626 File Offset: 0x0029E826
		public override Color MapBgColor
		{
			get
			{
				return new Color("819A97");
			}
		}

		// Token: 0x06007B4A RID: 31562 RVA: 0x002A0634 File Offset: 0x0029E834
		public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
		{
			int num = mapRng.NextInt(5, 7);
			int num2 = MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1;
			return new MapPointTypeCounts(num2, num);
		}
	}
}
