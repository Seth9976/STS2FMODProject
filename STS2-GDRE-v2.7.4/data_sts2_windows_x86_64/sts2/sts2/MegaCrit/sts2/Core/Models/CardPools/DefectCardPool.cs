using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B4D RID: 2893
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefectCardPool : CardPoolModel
	{
		// Token: 0x1700218F RID: 8591
		// (get) Token: 0x06007A47 RID: 31303 RVA: 0x0029D57D File Offset: 0x0029B77D
		public override string Title
		{
			get
			{
				return "defect";
			}
		}

		// Token: 0x17002190 RID: 8592
		// (get) Token: 0x06007A48 RID: 31304 RVA: 0x0029D584 File Offset: 0x0029B784
		public override string EnergyColorName
		{
			get
			{
				return "defect";
			}
		}

		// Token: 0x17002191 RID: 8593
		// (get) Token: 0x06007A49 RID: 31305 RVA: 0x0029D58B File Offset: 0x0029B78B
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_blue";
			}
		}

		// Token: 0x17002192 RID: 8594
		// (get) Token: 0x06007A4A RID: 31306 RVA: 0x0029D592 File Offset: 0x0029B792
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("3EB3ED");
			}
		}

		// Token: 0x17002193 RID: 8595
		// (get) Token: 0x06007A4B RID: 31307 RVA: 0x0029D59E File Offset: 0x0029B79E
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("1D5673");
			}
		}

		// Token: 0x17002194 RID: 8596
		// (get) Token: 0x06007A4C RID: 31308 RVA: 0x0029D5AA File Offset: 0x0029B7AA
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A4D RID: 31309 RVA: 0x0029D5B0 File Offset: 0x0029B7B0
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<AdaptiveStrike>(),
				ModelDb.Card<AllForOne>(),
				ModelDb.Card<BallLightning>(),
				ModelDb.Card<Barrage>(),
				ModelDb.Card<BeamCell>(),
				ModelDb.Card<BiasedCognition>(),
				ModelDb.Card<BoostAway>(),
				ModelDb.Card<BootSequence>(),
				ModelDb.Card<Buffer>(),
				ModelDb.Card<BulkUp>(),
				ModelDb.Card<Capacitor>(),
				ModelDb.Card<Chaos>(),
				ModelDb.Card<ChargeBattery>(),
				ModelDb.Card<Chill>(),
				ModelDb.Card<Claw>(),
				ModelDb.Card<ColdSnap>(),
				ModelDb.Card<Compact>(),
				ModelDb.Card<CompileDriver>(),
				ModelDb.Card<ConsumingShadow>(),
				ModelDb.Card<Coolant>(),
				ModelDb.Card<Coolheaded>(),
				ModelDb.Card<CreativeAi>(),
				ModelDb.Card<Darkness>(),
				ModelDb.Card<DefendDefect>(),
				ModelDb.Card<Defragment>(),
				ModelDb.Card<DoubleEnergy>(),
				ModelDb.Card<Dualcast>(),
				ModelDb.Card<EchoForm>(),
				ModelDb.Card<EnergySurge>(),
				ModelDb.Card<Feral>(),
				ModelDb.Card<FightThrough>(),
				ModelDb.Card<FlakCannon>(),
				ModelDb.Card<FocusedStrike>(),
				ModelDb.Card<Ftl>(),
				ModelDb.Card<Fusion>(),
				ModelDb.Card<GeneticAlgorithm>(),
				ModelDb.Card<Glacier>(),
				ModelDb.Card<Glasswork>(),
				ModelDb.Card<GoForTheEyes>(),
				ModelDb.Card<GunkUp>(),
				ModelDb.Card<Hailstorm>(),
				ModelDb.Card<HelixDrill>(),
				ModelDb.Card<Hibernate>(),
				ModelDb.Card<Hologram>(),
				ModelDb.Card<Hotfix>(),
				ModelDb.Card<Hyperbeam>(),
				ModelDb.Card<IceLance>(),
				ModelDb.Card<Ignition>(),
				ModelDb.Card<ImitationLearning>(),
				ModelDb.Card<Iteration>(),
				ModelDb.Card<Leap>(),
				ModelDb.Card<LightningRod>(),
				ModelDb.Card<Loop>(),
				ModelDb.Card<MachineLearning>(),
				ModelDb.Card<MeteorStrike>(),
				ModelDb.Card<Modded>(),
				ModelDb.Card<MomentumStrike>(),
				ModelDb.Card<MultiCast>(),
				ModelDb.Card<Null>(),
				ModelDb.Card<OneForAll>(),
				ModelDb.Card<Overclock>(),
				ModelDb.Card<Quadcast>(),
				ModelDb.Card<Rainbow>(),
				ModelDb.Card<Reboot>(),
				ModelDb.Card<Refract>(),
				ModelDb.Card<RocketPunch>(),
				ModelDb.Card<Scavenge>(),
				ModelDb.Card<Scrape>(),
				ModelDb.Card<ShadowShield>(),
				ModelDb.Card<Shatter>(),
				ModelDb.Card<SignalBoost>(),
				ModelDb.Card<Skim>(),
				ModelDb.Card<Smokestack>(),
				ModelDb.Card<Spinner>(),
				ModelDb.Card<Storm>(),
				ModelDb.Card<StrikeDefect>(),
				ModelDb.Card<Subroutine>(),
				ModelDb.Card<Sunder>(),
				ModelDb.Card<Supercritical>(),
				ModelDb.Card<SweepingBeam>(),
				ModelDb.Card<Synchronize>(),
				ModelDb.Card<Synthesis>(),
				ModelDb.Card<Tempest>(),
				ModelDb.Card<TeslaCoil>(),
				ModelDb.Card<Thunder>(),
				ModelDb.Card<TrashToTreasure>(),
				ModelDb.Card<Turbo>(),
				ModelDb.Card<Uproar>(),
				ModelDb.Card<Voltaic>(),
				ModelDb.Card<WhiteNoise>(),
				ModelDb.Card<Zap>()
			};
		}

		// Token: 0x06007A4E RID: 31310 RVA: 0x0029D8F0 File Offset: 0x0029BAF0
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Defect2Epoch>())
			{
				list.RemoveAll((CardModel c) => Defect2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Defect5Epoch>())
			{
				list.RemoveAll((CardModel c) => Defect5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Defect7Epoch>())
			{
				list.RemoveAll((CardModel c) => Defect7Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}
	}
}
