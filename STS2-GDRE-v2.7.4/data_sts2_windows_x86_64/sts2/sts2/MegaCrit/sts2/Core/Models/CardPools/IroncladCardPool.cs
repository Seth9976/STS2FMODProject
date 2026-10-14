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
	// Token: 0x02000B51 RID: 2897
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IroncladCardPool : CardPoolModel
	{
		// Token: 0x170021A5 RID: 8613
		// (get) Token: 0x06007A67 RID: 31335 RVA: 0x0029DBD3 File Offset: 0x0029BDD3
		public override string Title
		{
			get
			{
				return "ironclad";
			}
		}

		// Token: 0x170021A6 RID: 8614
		// (get) Token: 0x06007A68 RID: 31336 RVA: 0x0029DBDA File Offset: 0x0029BDDA
		public override string EnergyColorName
		{
			get
			{
				return "ironclad";
			}
		}

		// Token: 0x170021A7 RID: 8615
		// (get) Token: 0x06007A69 RID: 31337 RVA: 0x0029DBE1 File Offset: 0x0029BDE1
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_red";
			}
		}

		// Token: 0x170021A8 RID: 8616
		// (get) Token: 0x06007A6A RID: 31338 RVA: 0x0029DBE8 File Offset: 0x0029BDE8
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("D62000");
			}
		}

		// Token: 0x170021A9 RID: 8617
		// (get) Token: 0x06007A6B RID: 31339 RVA: 0x0029DBF4 File Offset: 0x0029BDF4
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("802020");
			}
		}

		// Token: 0x170021AA RID: 8618
		// (get) Token: 0x06007A6C RID: 31340 RVA: 0x0029DC00 File Offset: 0x0029BE00
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A6D RID: 31341 RVA: 0x0029DC04 File Offset: 0x0029BE04
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Aggression>(),
				ModelDb.Card<Anger>(),
				ModelDb.Card<Armaments>(),
				ModelDb.Card<AshenStrike>(),
				ModelDb.Card<Barricade>(),
				ModelDb.Card<Bash>(),
				ModelDb.Card<BattleTrance>(),
				ModelDb.Card<Blaze>(),
				ModelDb.Card<BloodWall>(),
				ModelDb.Card<Bloodletting>(),
				ModelDb.Card<Bludgeon>(),
				ModelDb.Card<BodySlam>(),
				ModelDb.Card<Brand>(),
				ModelDb.Card<Break>(),
				ModelDb.Card<Breakthrough>(),
				ModelDb.Card<Bully>(),
				ModelDb.Card<BurningPact>(),
				ModelDb.Card<Cascade>(),
				ModelDb.Card<Cinder>(),
				ModelDb.Card<Colossus>(),
				ModelDb.Card<Conflagration>(),
				ModelDb.Card<Corruption>(),
				ModelDb.Card<CrimsonMantle>(),
				ModelDb.Card<Cruelty>(),
				ModelDb.Card<DarkEmbrace>(),
				ModelDb.Card<DefendIronclad>(),
				ModelDb.Card<DemonForm>(),
				ModelDb.Card<DemonicShield>(),
				ModelDb.Card<Dismantle>(),
				ModelDb.Card<Dominate>(),
				ModelDb.Card<DrumOfBattle>(),
				ModelDb.Card<EvilEye>(),
				ModelDb.Card<ExpectAFight>(),
				ModelDb.Card<Feed>(),
				ModelDb.Card<FeelNoPain>(),
				ModelDb.Card<FiendFire>(),
				ModelDb.Card<FightMe>(),
				ModelDb.Card<FlameBarrier>(),
				ModelDb.Card<ForgottenRitual>(),
				ModelDb.Card<Havoc>(),
				ModelDb.Card<Headbutt>(),
				ModelDb.Card<Hellraiser>(),
				ModelDb.Card<Hemokinesis>(),
				ModelDb.Card<HowlFromBeyond>(),
				ModelDb.Card<Impervious>(),
				ModelDb.Card<InfernalBlade>(),
				ModelDb.Card<Inferno>(),
				ModelDb.Card<Inflame>(),
				ModelDb.Card<IronWave>(),
				ModelDb.Card<Juggernaut>(),
				ModelDb.Card<Juggling>(),
				ModelDb.Card<Mangle>(),
				ModelDb.Card<Midnight>(),
				ModelDb.Card<MoltenFist>(),
				ModelDb.Card<NotYet>(),
				ModelDb.Card<Offering>(),
				ModelDb.Card<OneTwoPunch>(),
				ModelDb.Card<Outrage>(),
				ModelDb.Card<PactsEnd>(),
				ModelDb.Card<PerfectedStrike>(),
				ModelDb.Card<Pillage>(),
				ModelDb.Card<PommelStrike>(),
				ModelDb.Card<PrimalForce>(),
				ModelDb.Card<Pyre>(),
				ModelDb.Card<Rage>(),
				ModelDb.Card<Rampage>(),
				ModelDb.Card<Rupture>(),
				ModelDb.Card<SecondWind>(),
				ModelDb.Card<SetupStrike>(),
				ModelDb.Card<ShrugItOff>(),
				ModelDb.Card<Spite>(),
				ModelDb.Card<Stampede>(),
				ModelDb.Card<Stoke>(),
				ModelDb.Card<Stomp>(),
				ModelDb.Card<StoneArmor>(),
				ModelDb.Card<StrikeIronclad>(),
				ModelDb.Card<SwordBoomerang>(),
				ModelDb.Card<Tank>(),
				ModelDb.Card<Taunt>(),
				ModelDb.Card<TearAsunder>(),
				ModelDb.Card<Thrash>(),
				ModelDb.Card<Thunderclap>(),
				ModelDb.Card<Tremble>(),
				ModelDb.Card<TrueGrit>(),
				ModelDb.Card<TwinStrike>(),
				ModelDb.Card<Unmovable>(),
				ModelDb.Card<Unrelenting>(),
				ModelDb.Card<Uppercut>(),
				ModelDb.Card<Vicious>(),
				ModelDb.Card<Whirlwind>()
			};
		}

		// Token: 0x06007A6E RID: 31342 RVA: 0x0029DF3C File Offset: 0x0029C13C
		protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			if (!unlockState.IsEpochRevealed<Ironclad2Epoch>())
			{
				list.RemoveAll((CardModel c) => Ironclad2Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Ironclad5Epoch>())
			{
				list.RemoveAll((CardModel c) => Ironclad5Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			if (!unlockState.IsEpochRevealed<Ironclad7Epoch>())
			{
				list.RemoveAll((CardModel c) => Ironclad7Epoch.Cards.Any<CardModel>((CardModel card) => card.Id == c.Id));
			}
			return list;
		}
	}
}
