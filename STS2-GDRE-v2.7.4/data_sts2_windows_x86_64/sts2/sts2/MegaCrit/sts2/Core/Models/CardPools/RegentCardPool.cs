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
	// Token: 0x02000B55 RID: 2901
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegentCardPool : CardPoolModel
	{
		// Token: 0x170021BD RID: 8637
		// (get) Token: 0x06007A8C RID: 31372 RVA: 0x0029E513 File Offset: 0x0029C713
		public override string Title
		{
			get
			{
				return "regent";
			}
		}

		// Token: 0x170021BE RID: 8638
		// (get) Token: 0x06007A8D RID: 31373 RVA: 0x0029E51A File Offset: 0x0029C71A
		public override string EnergyColorName
		{
			get
			{
				return "regent";
			}
		}

		// Token: 0x170021BF RID: 8639
		// (get) Token: 0x06007A8E RID: 31374 RVA: 0x0029E521 File Offset: 0x0029C721
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_orange";
			}
		}

		// Token: 0x170021C0 RID: 8640
		// (get) Token: 0x06007A8F RID: 31375 RVA: 0x0029E528 File Offset: 0x0029C728
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("E36600");
			}
		}

		// Token: 0x170021C1 RID: 8641
		// (get) Token: 0x06007A90 RID: 31376 RVA: 0x0029E534 File Offset: 0x0029C734
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("803D0E");
			}
		}

		// Token: 0x170021C2 RID: 8642
		// (get) Token: 0x06007A91 RID: 31377 RVA: 0x0029E540 File Offset: 0x0029C740
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A92 RID: 31378 RVA: 0x0029E544 File Offset: 0x0029C744
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Alignment>(),
				ModelDb.Card<Arsenal>(),
				ModelDb.Card<AstralPulse>(),
				ModelDb.Card<BeatIntoShape>(),
				ModelDb.Card<Begone>(),
				ModelDb.Card<BigBang>(),
				ModelDb.Card<BlackHole>(),
				ModelDb.Card<Bombardment>(),
				ModelDb.Card<Bulwark>(),
				ModelDb.Card<BundleOfJoy>(),
				ModelDb.Card<CelestialMight>(),
				ModelDb.Card<Charge>(),
				ModelDb.Card<ChildOfTheStars>(),
				ModelDb.Card<CloakOfStars>(),
				ModelDb.Card<CollisionCourse>(),
				ModelDb.Card<Comet>(),
				ModelDb.Card<Conqueror>(),
				ModelDb.Card<Constellation>(),
				ModelDb.Card<Convergence>(),
				ModelDb.Card<CosmicIndifference>(),
				ModelDb.Card<CrashLanding>(),
				ModelDb.Card<CrescentSpear>(),
				ModelDb.Card<CrushUnder>(),
				ModelDb.Card<DecisionsDecisions>(),
				ModelDb.Card<DefendRegent>(),
				ModelDb.Card<Devastate>(),
				ModelDb.Card<DyingStar>(),
				ModelDb.Card<FallingStar>(),
				ModelDb.Card<ForegoneConclusion>(),
				ModelDb.Card<Furnace>(),
				ModelDb.Card<GammaBlast>(),
				ModelDb.Card<GatherLight>(),
				ModelDb.Card<Genesis>(),
				ModelDb.Card<Glimmer>(),
				ModelDb.Card<Glitterstream>(),
				ModelDb.Card<Glow>(),
				ModelDb.Card<Guards>(),
				ModelDb.Card<GuidingStar>(),
				ModelDb.Card<HammerTime>(),
				ModelDb.Card<HeavenlyDrill>(),
				ModelDb.Card<Hegemony>(),
				ModelDb.Card<HeirloomHammer>(),
				ModelDb.Card<HiddenCache>(),
				ModelDb.Card<IAmInvincible>(),
				ModelDb.Card<KinglyKick>(),
				ModelDb.Card<KinglyPunch>(),
				ModelDb.Card<KnockoutBlow>(),
				ModelDb.Card<KnowThyPlace>(),
				ModelDb.Card<Largesse>(),
				ModelDb.Card<LunarBlast>(),
				ModelDb.Card<MakeItSo>(),
				ModelDb.Card<ManifestAuthority>(),
				ModelDb.Card<MeteorShower>(),
				ModelDb.Card<MonarchsGaze>(),
				ModelDb.Card<Monologue>(),
				ModelDb.Card<NeutronAegis>(),
				ModelDb.Card<Orbit>(),
				ModelDb.Card<PaleBlueDot>(),
				ModelDb.Card<Parry>(),
				ModelDb.Card<ParticleWall>(),
				ModelDb.Card<Patter>(),
				ModelDb.Card<PhotonCut>(),
				ModelDb.Card<Plot>(),
				ModelDb.Card<PillarOfCreation>(),
				ModelDb.Card<Prophesize>(),
				ModelDb.Card<Quasar>(),
				ModelDb.Card<Radiate>(),
				ModelDb.Card<RefineBlade>(),
				ModelDb.Card<Reflect>(),
				ModelDb.Card<Resonance>(),
				ModelDb.Card<RoyalGamble>(),
				ModelDb.Card<Royalties>(),
				ModelDb.Card<SeekingEdge>(),
				ModelDb.Card<SevenStars>(),
				ModelDb.Card<ShiningStrike>(),
				ModelDb.Card<SolarStrike>(),
				ModelDb.Card<SpectrumShift>(),
				ModelDb.Card<SpoilsOfBattle>(),
				ModelDb.Card<Stardust>(),
				ModelDb.Card<StrikeRegent>(),
				ModelDb.Card<SummonForth>(),
				ModelDb.Card<Supermassive>(),
				ModelDb.Card<SwordSage>(),
				ModelDb.Card<Terraforming>(),
				ModelDb.Card<TheSealedThrone>(),
				ModelDb.Card<TheSmith>(),
				ModelDb.Card<Tutor>(),
				ModelDb.Card<Tyranny>(),
				ModelDb.Card<Venerate>(),
				ModelDb.Card<VoidForm>(),
				ModelDb.Card<WroughtInWar>()
			};
		}

		// Token: 0x06007A93 RID: 31379 RVA: 0x0029E884 File Offset: 0x0029CA84
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Regent2Epoch>())
			{
				list.RemoveAll((CardModel c) => Regent2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Regent5Epoch>())
			{
				list.RemoveAll((CardModel c) => Regent5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Regent7Epoch>())
			{
				list.RemoveAll((CardModel c) => Regent7Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}
	}
}
