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
	// Token: 0x02000B53 RID: 2899
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NecrobinderCardPool : CardPoolModel
	{
		// Token: 0x170021B1 RID: 8625
		// (get) Token: 0x06007A7B RID: 31355 RVA: 0x0029E099 File Offset: 0x0029C299
		public override string Title
		{
			get
			{
				return "necrobinder";
			}
		}

		// Token: 0x170021B2 RID: 8626
		// (get) Token: 0x06007A7C RID: 31356 RVA: 0x0029E0A0 File Offset: 0x0029C2A0
		public override string EnergyColorName
		{
			get
			{
				return "necrobinder";
			}
		}

		// Token: 0x170021B3 RID: 8627
		// (get) Token: 0x06007A7D RID: 31357 RVA: 0x0029E0A7 File Offset: 0x0029C2A7
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_pink";
			}
		}

		// Token: 0x170021B4 RID: 8628
		// (get) Token: 0x06007A7E RID: 31358 RVA: 0x0029E0AE File Offset: 0x0029C2AE
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("CD4EED");
			}
		}

		// Token: 0x170021B5 RID: 8629
		// (get) Token: 0x06007A7F RID: 31359 RVA: 0x0029E0BA File Offset: 0x0029C2BA
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("803367");
			}
		}

		// Token: 0x170021B6 RID: 8630
		// (get) Token: 0x06007A80 RID: 31360 RVA: 0x0029E0C6 File Offset: 0x0029C2C6
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x0029E0CC File Offset: 0x0029C2CC
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Afterlife>(),
				ModelDb.Card<BansheesCry>(),
				ModelDb.Card<BlightStrike>(),
				ModelDb.Card<Bodyguard>(),
				ModelDb.Card<BoneShards>(),
				ModelDb.Card<BorrowedTime>(),
				ModelDb.Card<Bury>(),
				ModelDb.Card<Cacophony>(),
				ModelDb.Card<Calcify>(),
				ModelDb.Card<CallOfTheVoid>(),
				ModelDb.Card<CaptureSpirit>(),
				ModelDb.Card<Cleanse>(),
				ModelDb.Card<Countdown>(),
				ModelDb.Card<DanseMacabre>(),
				ModelDb.Card<DeathMarch>(),
				ModelDb.Card<Deathbringer>(),
				ModelDb.Card<DeathsDoor>(),
				ModelDb.Card<Debilitate>(),
				ModelDb.Card<DefendNecrobinder>(),
				ModelDb.Card<Defile>(),
				ModelDb.Card<Defy>(),
				ModelDb.Card<Delay>(),
				ModelDb.Card<Demesne>(),
				ModelDb.Card<DevourLife>(),
				ModelDb.Card<Dirge>(),
				ModelDb.Card<DrainPower>(),
				ModelDb.Card<Dredge>(),
				ModelDb.Card<Eidolon>(),
				ModelDb.Card<EndOfDays>(),
				ModelDb.Card<EnfeeblingTouch>(),
				ModelDb.Card<Eradicate>(),
				ModelDb.Card<Fear>(),
				ModelDb.Card<Fetch>(),
				ModelDb.Card<Flatten>(),
				ModelDb.Card<ForbiddenGrimoire>(),
				ModelDb.Card<Friendship>(),
				ModelDb.Card<GlimpseBeyond>(),
				ModelDb.Card<GraveWarden>(),
				ModelDb.Card<Graveblast>(),
				ModelDb.Card<Hang>(),
				ModelDb.Card<Haunt>(),
				ModelDb.Card<HighFive>(),
				ModelDb.Card<Invoke>(),
				ModelDb.Card<LegionOfBone>(),
				ModelDb.Card<Lethality>(),
				ModelDb.Card<Melancholy>(),
				ModelDb.Card<Misery>(),
				ModelDb.Card<NecroMastery>(),
				ModelDb.Card<NegativePulse>(),
				ModelDb.Card<Neurosurge>(),
				ModelDb.Card<NoEscape>(),
				ModelDb.Card<Oblivion>(),
				ModelDb.Card<Pagestorm>(),
				ModelDb.Card<Parse>(),
				ModelDb.Card<Poke>(),
				ModelDb.Card<Protector>(),
				ModelDb.Card<PullAggro>(),
				ModelDb.Card<PullFromBelow>(),
				ModelDb.Card<Putrefy>(),
				ModelDb.Card<Rattle>(),
				ModelDb.Card<Reanimate>(),
				ModelDb.Card<Reap>(),
				ModelDb.Card<ReaperForm>(),
				ModelDb.Card<Reave>(),
				ModelDb.Card<RightHandHand>(),
				ModelDb.Card<Sacrifice>(),
				ModelDb.Card<Scourge>(),
				ModelDb.Card<SculptingStrike>(),
				ModelDb.Card<Seance>(),
				ModelDb.Card<SentryMode>(),
				ModelDb.Card<Severance>(),
				ModelDb.Card<SharedFate>(),
				ModelDb.Card<Shroud>(),
				ModelDb.Card<SicEm>(),
				ModelDb.Card<SleightOfFlesh>(),
				ModelDb.Card<Snap>(),
				ModelDb.Card<SoulStorm>(),
				ModelDb.Card<Soulbound>(),
				ModelDb.Card<Sow>(),
				ModelDb.Card<SpiritOfAsh>(),
				ModelDb.Card<Spur>(),
				ModelDb.Card<Squeeze>(),
				ModelDb.Card<StrikeNecrobinder>(),
				ModelDb.Card<TheScythe>(),
				ModelDb.Card<TimesUp>(),
				ModelDb.Card<Transfigure>(),
				ModelDb.Card<Undeath>(),
				ModelDb.Card<Underworld>(),
				ModelDb.Card<Unleash>(),
				ModelDb.Card<Veilpiercer>(),
				ModelDb.Card<Wisp>()
			};
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x0029E40C File Offset: 0x0029C60C
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Necrobinder2Epoch>())
			{
				list.RemoveAll((CardModel c) => Necrobinder2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Necrobinder5Epoch>())
			{
				list.RemoveAll((CardModel c) => Necrobinder5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Necrobinder7Epoch>())
			{
				list.RemoveAll((CardModel c) => Necrobinder7Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}
	}
}
