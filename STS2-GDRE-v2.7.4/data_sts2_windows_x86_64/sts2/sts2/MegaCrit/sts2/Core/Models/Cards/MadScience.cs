using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A30 RID: 2608
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MadScience : CardModel
	{
		// Token: 0x060073F9 RID: 29689 RVA: 0x00290774 File Offset: 0x0028E974
		public MadScience()
			: base(1, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy, false)
		{
		}

		// Token: 0x17001EF5 RID: 7925
		// (get) Token: 0x060073FA RID: 29690 RVA: 0x00290781 File Offset: 0x0028E981
		public override string PortraitPath
		{
			get
			{
				return this.GetPortraitPath(this.TinkerTimeType);
			}
		}

		// Token: 0x17001EF6 RID: 7926
		// (get) Token: 0x060073FB RID: 29691 RVA: 0x0029078F File Offset: 0x0028E98F
		public override string BetaPortraitPath
		{
			get
			{
				return CardModel.MissingPortraitPath;
			}
		}

		// Token: 0x17001EF7 RID: 7927
		// (get) Token: 0x060073FC RID: 29692 RVA: 0x00290796 File Offset: 0x0028E996
		public string[] AllPortraitPaths
		{
			[PreserveBaseOverrides]
			get
			{
				return new string[]
				{
					this.GetPortraitPath(CardType.Attack),
					this.GetPortraitPath(CardType.Skill),
					this.GetPortraitPath(CardType.Power)
				};
			}
		}

		// Token: 0x060073FD RID: 29693 RVA: 0x002907BC File Offset: 0x0028E9BC
		private string GetPortraitPath(CardType cardType)
		{
			return ImageHelper.GetImagePath("atlases/card_atlas.sprites/event/" + this.GetPortraitFilename(cardType) + ".tres");
		}

		// Token: 0x060073FE RID: 29694 RVA: 0x002907DC File Offset: 0x0028E9DC
		private string GetPortraitFilename(CardType cardType)
		{
			string text;
			switch (cardType)
			{
			case CardType.None:
				text = "mad_science_attack";
				break;
			case CardType.Attack:
				text = "mad_science_attack";
				break;
			case CardType.Skill:
				text = "mad_science_skill";
				break;
			case CardType.Power:
				text = "mad_science_power";
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Mad Science is invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<CardType>(this.TinkerTimeType);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return text;
		}

		// Token: 0x17001EF8 RID: 7928
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x0029085E File Offset: 0x0028EA5E
		public override CardType Type
		{
			get
			{
				return this.TinkerTimeType;
			}
		}

		// Token: 0x17001EF9 RID: 7929
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x00290866 File Offset: 0x0028EA66
		public override TargetType TargetType
		{
			get
			{
				if (this.TinkerTimeType != CardType.Attack)
				{
					return TargetType.Self;
				}
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001EFA RID: 7930
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x00290874 File Offset: 0x0028EA74
		public override bool GainsBlock
		{
			get
			{
				return this.TinkerTimeType == CardType.Skill;
			}
		}

		// Token: 0x17001EFB RID: 7931
		// (get) Token: 0x06007402 RID: 29698 RVA: 0x00290880 File Offset: 0x0028EA80
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(12m, ValueProp.Move),
					new BlockVar(8m, ValueProp.Move),
					new PowerVar<WeakPower>("SappingWeak", 2m),
					new PowerVar<VulnerablePower>("SappingVulnerable", 2m),
					new DynamicVar("ViolenceHits", 3m),
					new PowerVar<StranglePower>("ChokingDamage", 6m),
					new EnergyVar("EnergizedEnergy", 2),
					new CardsVar("WisdomCards", 3),
					new PowerVar<StrengthPower>("ExpertiseStrength", 2m),
					new PowerVar<DexterityPower>("ExpertiseDexterity", 2m),
					new DynamicVar("CuriousReduction", 1m)
				});
			}
		}

		// Token: 0x17001EFC RID: 7932
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x0029095A File Offset: 0x0028EB5A
		// (set) Token: 0x06007404 RID: 29700 RVA: 0x00290962 File Offset: 0x0028EB62
		[SavedProperty(SerializationCondition.AlwaysSave, -1)]
		public CardType TinkerTimeType
		{
			get
			{
				return this._tinkerTimeType;
			}
			set
			{
				base.AssertMutable();
				this._tinkerTimeType = value;
			}
		}

		// Token: 0x17001EFD RID: 7933
		// (get) Token: 0x06007405 RID: 29701 RVA: 0x00290971 File Offset: 0x0028EB71
		// (set) Token: 0x06007406 RID: 29702 RVA: 0x00290979 File Offset: 0x0028EB79
		[SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
		public TinkerTime.RiderEffect TinkerTimeRider
		{
			get
			{
				return this._tinkerTimeRider;
			}
			set
			{
				base.AssertMutable();
				this._tinkerTimeRider = value;
			}
		}

		/// <summary>
		/// Used for tests.
		/// </summary>
		// Token: 0x17001EFE RID: 7934
		// (get) Token: 0x06007407 RID: 29703 RVA: 0x00290988 File Offset: 0x0028EB88
		// (set) Token: 0x06007408 RID: 29704 RVA: 0x00290990 File Offset: 0x0028EB90
		[Nullable(2)]
		private CardModel MockedChaosCard
		{
			[NullableContext(2)]
			get
			{
				return this._mockedChaosCard;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._mockedChaosCard = value;
			}
		}

		// Token: 0x17001EFF RID: 7935
		// (get) Token: 0x06007409 RID: 29705 RVA: 0x0029099F File Offset: 0x0028EB9F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				if (this.TinkerTimeRider != TinkerTime.RiderEffect.None)
				{
					return TinkerTime.GetRiderHoverTips(this.TinkerTimeRider);
				}
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x0600740A RID: 29706 RVA: 0x002909BC File Offset: 0x0028EBBC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.TargetType == TargetType.AnyEnemy && cardPlay.Target == null)
			{
				ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			}
			switch (this.TinkerTimeType)
			{
			case CardType.Attack:
				await this.ExecuteAttack(choiceContext, cardPlay.Target, cardPlay);
				break;
			case CardType.Skill:
				await this.ExecuteSkill(cardPlay);
				break;
			case CardType.Power:
				await this.ExecutePower(choiceContext);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			TinkerTime.RiderEffect tinkerTimeRider = this.TinkerTimeRider;
			if (tinkerTimeRider == TinkerTime.RiderEffect.Sapping || tinkerTimeRider - TinkerTime.RiderEffect.Choking <= 3)
			{
				await this.ExecuteRider(this.TinkerTimeRider, cardPlay.Target, choiceContext);
			}
		}

		// Token: 0x0600740B RID: 29707 RVA: 0x00290A10 File Offset: 0x0028EC10
		private async Task ExecuteAttack(PlayerChoiceContext choiceContext, Creature target, CardPlay cardPlay)
		{
			int num = ((this.TinkerTimeRider == TinkerTime.RiderEffect.Violence) ? base.DynamicVars["ViolenceHits"].IntValue : 1);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, cardPlay)
				.Targeting(target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600740C RID: 29708 RVA: 0x00290A6C File Offset: 0x0028EC6C
		private async Task ExecuteSkill(CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x00290AB8 File Offset: 0x0028ECB8
		private async Task ExecutePower(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			switch (this.TinkerTimeRider)
			{
			case TinkerTime.RiderEffect.Expertise:
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ExpertiseStrength"].BaseValue, base.Owner.Creature, this, false);
				await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ExpertiseDexterity"].BaseValue, base.Owner.Creature, this, false);
				break;
			case TinkerTime.RiderEffect.Curious:
				await PowerCmd.Apply<CuriousPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CuriousReduction"].BaseValue, base.Owner.Creature, this, false);
				break;
			case TinkerTime.RiderEffect.Improvement:
				await PowerCmd.Apply<ImprovementPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
				break;
			}
		}

		// Token: 0x0600740E RID: 29710 RVA: 0x00290B03 File Offset: 0x0028ED03
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}

		// Token: 0x0600740F RID: 29711 RVA: 0x00290B0C File Offset: 0x0028ED0C
		protected override void AddExtraArgsToDescription(LocString description)
		{
			description.Add("CardType", this.TinkerTimeType.ToString());
			description.Add("HasRider", this.TinkerTimeRider > TinkerTime.RiderEffect.None);
			foreach (TinkerTime.RiderEffect riderEffect in Enum.GetValues<TinkerTime.RiderEffect>())
			{
				description.Add(riderEffect.ToString(), this.TinkerTimeRider == riderEffect);
			}
		}

		// Token: 0x06007410 RID: 29712 RVA: 0x00290B80 File Offset: 0x0028ED80
		private async Task ExecuteRider(TinkerTime.RiderEffect rider, [Nullable(2)] Creature target, PlayerChoiceContext choiceContext)
		{
			switch (rider)
			{
			case TinkerTime.RiderEffect.Sapping:
				await PowerCmd.Apply<WeakPower>(choiceContext, target, base.DynamicVars["SappingWeak"].BaseValue, base.Owner.Creature, this, false);
				await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.DynamicVars["SappingVulnerable"].BaseValue, base.Owner.Creature, this, false);
				return;
			case TinkerTime.RiderEffect.Choking:
				await PowerCmd.Apply<StranglePower>(choiceContext, target, base.DynamicVars["ChokingDamage"].BaseValue, base.Owner.Creature, this, false);
				return;
			case TinkerTime.RiderEffect.Energized:
				await PlayerCmd.GainEnergy(base.DynamicVars["EnergizedEnergy"].IntValue, base.Owner);
				return;
			case TinkerTime.RiderEffect.Wisdom:
				await CardPileCmd.Draw(choiceContext, base.DynamicVars["WisdomCards"].IntValue, base.Owner, false);
				return;
			case TinkerTime.RiderEffect.Chaos:
			{
				CardModel cardModel;
				if (this.MockedChaosCard != null)
				{
					cardModel = this.MockedChaosCard;
				}
				else
				{
					cardModel = CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), 1, base.Owner.RunState.Rng.CombatCardGeneration).First<CardModel>();
				}
				cardModel.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
				return;
			}
			}
			throw new ArgumentOutOfRangeException("rider", rider, null);
		}

		/// <summary>
		/// ONLY USE THIS IN TESTS!
		/// </summary>
		// Token: 0x06007411 RID: 29713 RVA: 0x00290BDB File Offset: 0x0028EDDB
		public void MockChaosCard(CardModel card)
		{
			base.AssertMutable();
			card.AssertMutable();
			this.MockedChaosCard = card;
		}

		// Token: 0x04002889 RID: 10377
		public const int attackDamage = 12;

		// Token: 0x0400288A RID: 10378
		public const int skillBlock = 8;

		// Token: 0x0400288B RID: 10379
		public const string sappingWeakKey = "SappingWeak";

		// Token: 0x0400288C RID: 10380
		public const int sappingWeakValue = 2;

		// Token: 0x0400288D RID: 10381
		public const string sappingVulnerableKey = "SappingVulnerable";

		// Token: 0x0400288E RID: 10382
		public const int sappingVulnerableValue = 2;

		// Token: 0x0400288F RID: 10383
		public const string violenceHitsKey = "ViolenceHits";

		// Token: 0x04002890 RID: 10384
		public const int violenceHitsValue = 3;

		// Token: 0x04002891 RID: 10385
		public const string chokingDamageKey = "ChokingDamage";

		// Token: 0x04002892 RID: 10386
		public const int chokingDamageValue = 6;

		// Token: 0x04002893 RID: 10387
		public const string energizedEnergyKey = "EnergizedEnergy";

		// Token: 0x04002894 RID: 10388
		public const int energizedEnergyValue = 2;

		// Token: 0x04002895 RID: 10389
		public const string wisdomCardsKey = "WisdomCards";

		// Token: 0x04002896 RID: 10390
		public const int wisdomCardsValue = 3;

		// Token: 0x04002897 RID: 10391
		public const string expertiseStrengthKey = "ExpertiseStrength";

		// Token: 0x04002898 RID: 10392
		public const int expertiseStrengthValue = 2;

		// Token: 0x04002899 RID: 10393
		public const string expertiseDexterityKey = "ExpertiseDexterity";

		// Token: 0x0400289A RID: 10394
		public const int expertiseDexterityValue = 2;

		// Token: 0x0400289B RID: 10395
		public const string curiousReductionKey = "CuriousReduction";

		// Token: 0x0400289C RID: 10396
		public const int curiousReductionValue = 1;

		// Token: 0x0400289D RID: 10397
		private CardType _tinkerTimeType;

		// Token: 0x0400289E RID: 10398
		private TinkerTime.RiderEffect _tinkerTimeRider;

		// Token: 0x0400289F RID: 10399
		[Nullable(2)]
		private CardModel _mockedChaosCard;
	}
}
