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
	// Token: 0x02000B4B RID: 2891
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColorlessCardPool : CardPoolModel
	{
		// Token: 0x17002185 RID: 8581
		// (get) Token: 0x06007A38 RID: 31288 RVA: 0x0029D126 File Offset: 0x0029B326
		public override string Title
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x17002186 RID: 8582
		// (get) Token: 0x06007A39 RID: 31289 RVA: 0x0029D12D File Offset: 0x0029B32D
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x17002187 RID: 8583
		// (get) Token: 0x06007A3A RID: 31290 RVA: 0x0029D134 File Offset: 0x0029B334
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x17002188 RID: 8584
		// (get) Token: 0x06007A3B RID: 31291 RVA: 0x0029D13B File Offset: 0x0029B33B
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("A3A3A3FF");
			}
		}

		// Token: 0x17002189 RID: 8585
		// (get) Token: 0x06007A3C RID: 31292 RVA: 0x0029D147 File Offset: 0x0029B347
		public override bool IsColorless
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x0029D14C File Offset: 0x0029B34C
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Alchemize>(),
				ModelDb.Card<Anointed>(),
				ModelDb.Card<Automation>(),
				ModelDb.Card<BeaconOfHope>(),
				ModelDb.Card<BeatDown>(),
				ModelDb.Card<BelieveInYou>(),
				ModelDb.Card<Bolas>(),
				ModelDb.Card<Calamity>(),
				ModelDb.Card<Catastrophe>(),
				ModelDb.Card<Coordinate>(),
				ModelDb.Card<DarkShackles>(),
				ModelDb.Card<Discovery>(),
				ModelDb.Card<DramaticEntrance>(),
				ModelDb.Card<Entropy>(),
				ModelDb.Card<Equilibrium>(),
				ModelDb.Card<EternalArmor>(),
				ModelDb.Card<Fasten>(),
				ModelDb.Card<Finesse>(),
				ModelDb.Card<Fisticuffs>(),
				ModelDb.Card<FlashOfSteel>(),
				ModelDb.Card<GangUp>(),
				ModelDb.Card<GoldAxe>(),
				ModelDb.Card<HandOfGreed>(),
				ModelDb.Card<HiddenGem>(),
				ModelDb.Card<HuddleUp>(),
				ModelDb.Card<Impatience>(),
				ModelDb.Card<Intercept>(),
				ModelDb.Card<JackOfAllTrades>(),
				ModelDb.Card<Jackpot>(),
				ModelDb.Card<Knockdown>(),
				ModelDb.Card<Lift>(),
				ModelDb.Card<MasterOfStrategy>(),
				ModelDb.Card<Mayhem>(),
				ModelDb.Card<Mimic>(),
				ModelDb.Card<MindBlast>(),
				ModelDb.Card<Nostalgia>(),
				ModelDb.Card<Omnislice>(),
				ModelDb.Card<Panache>(),
				ModelDb.Card<PanicButton>(),
				ModelDb.Card<PrepTime>(),
				ModelDb.Card<Production>(),
				ModelDb.Card<Prolong>(),
				ModelDb.Card<Prowess>(),
				ModelDb.Card<Purity>(),
				ModelDb.Card<Rally>(),
				ModelDb.Card<Rend>(),
				ModelDb.Card<Restlessness>(),
				ModelDb.Card<RollingBoulder>(),
				ModelDb.Card<Salvo>(),
				ModelDb.Card<Scrawl>(),
				ModelDb.Card<SecretTechnique>(),
				ModelDb.Card<SecretWeapon>(),
				ModelDb.Card<SeekerStrike>(),
				ModelDb.Card<Shockwave>(),
				ModelDb.Card<Splash>(),
				ModelDb.Card<Stratagem>(),
				ModelDb.Card<TagTeam>(),
				ModelDb.Card<TheBall>(),
				ModelDb.Card<TheBomb>(),
				ModelDb.Card<TheGambit>(),
				ModelDb.Card<ThinkingAhead>(),
				ModelDb.Card<ThrummingHatchet>(),
				ModelDb.Card<UltimateDefend>(),
				ModelDb.Card<UltimateStrike>(),
				ModelDb.Card<Volley>()
			};
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x0029D3A0 File Offset: 0x0029B5A0
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Colorless1Epoch>())
			{
				list.RemoveAll((CardModel c) => Colorless1Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Colorless2Epoch>())
			{
				list.RemoveAll((CardModel c) => Colorless2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Colorless3Epoch>())
			{
				list.RemoveAll((CardModel c) => Colorless3Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Colorless4Epoch>())
			{
				list.RemoveAll((CardModel c) => Colorless4Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Colorless5Epoch>())
			{
				list.RemoveAll((CardModel c) => Colorless5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}

		// Token: 0x040028F1 RID: 10481
		public const string energyColorName = "colorless";
	}
}
