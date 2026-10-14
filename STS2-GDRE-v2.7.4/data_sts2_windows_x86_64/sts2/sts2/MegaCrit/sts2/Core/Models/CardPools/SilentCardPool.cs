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
	// Token: 0x02000B56 RID: 2902
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilentCardPool : CardPoolModel
	{
		// Token: 0x170021C3 RID: 8643
		// (get) Token: 0x06007A95 RID: 31381 RVA: 0x0029E92B File Offset: 0x0029CB2B
		public override string Title
		{
			get
			{
				return "silent";
			}
		}

		// Token: 0x170021C4 RID: 8644
		// (get) Token: 0x06007A96 RID: 31382 RVA: 0x0029E932 File Offset: 0x0029CB32
		public override string EnergyColorName
		{
			get
			{
				return "silent";
			}
		}

		// Token: 0x170021C5 RID: 8645
		// (get) Token: 0x06007A97 RID: 31383 RVA: 0x0029E939 File Offset: 0x0029CB39
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_green";
			}
		}

		// Token: 0x170021C6 RID: 8646
		// (get) Token: 0x06007A98 RID: 31384 RVA: 0x0029E940 File Offset: 0x0029CB40
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("5EBD00");
			}
		}

		// Token: 0x170021C7 RID: 8647
		// (get) Token: 0x06007A99 RID: 31385 RVA: 0x0029E94C File Offset: 0x0029CB4C
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("1A6625");
			}
		}

		// Token: 0x170021C8 RID: 8648
		// (get) Token: 0x06007A9A RID: 31386 RVA: 0x0029E958 File Offset: 0x0029CB58
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A9B RID: 31387 RVA: 0x0029E95C File Offset: 0x0029CB5C
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Abrasive>(),
				ModelDb.Card<Accelerant>(),
				ModelDb.Card<Accuracy>(),
				ModelDb.Card<Acrobatics>(),
				ModelDb.Card<Adrenaline>(),
				ModelDb.Card<Afterimage>(),
				ModelDb.Card<Anticipate>(),
				ModelDb.Card<Assassinate>(),
				ModelDb.Card<Backflip>(),
				ModelDb.Card<Backstab>(),
				ModelDb.Card<BladeOfInk>(),
				ModelDb.Card<BladeDance>(),
				ModelDb.Card<BladeSymphony>(),
				ModelDb.Card<Blur>(),
				ModelDb.Card<BouncingFlask>(),
				ModelDb.Card<BubbleBubble>(),
				ModelDb.Card<BulletTime>(),
				ModelDb.Card<Burst>(),
				ModelDb.Card<CalculatedGamble>(),
				ModelDb.Card<CloakAndDagger>(),
				ModelDb.Card<CorrosiveWave>(),
				ModelDb.Card<Concoct>(),
				ModelDb.Card<DaggerSpray>(),
				ModelDb.Card<DaggerThrow>(),
				ModelDb.Card<Dash>(),
				ModelDb.Card<DeadlyPoison>(),
				ModelDb.Card<DefendSilent>(),
				ModelDb.Card<Deflect>(),
				ModelDb.Card<DodgeAndRoll>(),
				ModelDb.Card<EchoingSlash>(),
				ModelDb.Card<Envenom>(),
				ModelDb.Card<EscapePlan>(),
				ModelDb.Card<Expertise>(),
				ModelDb.Card<Expose>(),
				ModelDb.Card<Fade>(),
				ModelDb.Card<FanOfKnives>(),
				ModelDb.Card<Finisher>(),
				ModelDb.Card<Flanking>(),
				ModelDb.Card<Flechettes>(),
				ModelDb.Card<FlickFlack>(),
				ModelDb.Card<Scare>(),
				ModelDb.Card<Footwork>(),
				ModelDb.Card<GrandFinale>(),
				ModelDb.Card<HandTrick>(),
				ModelDb.Card<Haze>(),
				ModelDb.Card<HiddenDaggers>(),
				ModelDb.Card<InfiniteBlades>(),
				ModelDb.Card<KnifeTrap>(),
				ModelDb.Card<LeadingStrike>(),
				ModelDb.Card<LegSweep>(),
				ModelDb.Card<Malaise>(),
				ModelDb.Card<MasterPlanner>(),
				ModelDb.Card<MementoMori>(),
				ModelDb.Card<Mirage>(),
				ModelDb.Card<Murder>(),
				ModelDb.Card<Neutralize>(),
				ModelDb.Card<Nightmare>(),
				ModelDb.Card<NoxiousFumes>(),
				ModelDb.Card<Outbreak>(),
				ModelDb.Card<PhantomBlades>(),
				ModelDb.Card<PiercingWail>(),
				ModelDb.Card<Pinpoint>(),
				ModelDb.Card<PoisonedStab>(),
				ModelDb.Card<Pounce>(),
				ModelDb.Card<PreciseCut>(),
				ModelDb.Card<Predator>(),
				ModelDb.Card<Prepared>(),
				ModelDb.Card<Reflex>(),
				ModelDb.Card<Ricochet>(),
				ModelDb.Card<SerpentForm>(),
				ModelDb.Card<ShadowStep>(),
				ModelDb.Card<Shadowmeld>(),
				ModelDb.Card<Skewer>(),
				ModelDb.Card<Slice>(),
				ModelDb.Card<Snakebite>(),
				ModelDb.Card<Sneaky>(),
				ModelDb.Card<Speedster>(),
				ModelDb.Card<StormOfSteel>(),
				ModelDb.Card<Strangle>(),
				ModelDb.Card<StrikeSilent>(),
				ModelDb.Card<SuckerPunch>(),
				ModelDb.Card<Suppress>(),
				ModelDb.Card<Survivor>(),
				ModelDb.Card<Tactician>(),
				ModelDb.Card<TheHunt>(),
				ModelDb.Card<ToolsOfTheTrade>(),
				ModelDb.Card<Tracking>(),
				ModelDb.Card<Untouchable>(),
				ModelDb.Card<UpMySleeve>(),
				ModelDb.Card<WellLaidPlans>(),
				ModelDb.Card<WraithForm>()
			};
		}

		// Token: 0x06007A9C RID: 31388 RVA: 0x0029EC9C File Offset: 0x0029CE9C
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Silent2Epoch>())
			{
				list.RemoveAll((CardModel c) => Silent2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Silent5Epoch>())
			{
				list.RemoveAll((CardModel c) => Silent5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Silent7Epoch>())
			{
				list.RemoveAll((CardModel c) => Silent7Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}
	}
}
