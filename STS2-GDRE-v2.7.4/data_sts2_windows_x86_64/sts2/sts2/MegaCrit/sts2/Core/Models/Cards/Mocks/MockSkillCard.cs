using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B49 RID: 2889
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockSkillCard : MockCardModel
	{
		// Token: 0x1700217D RID: 8573
		// (get) Token: 0x06007A1E RID: 31262 RVA: 0x0029CDED File Offset: 0x0029AFED
		public override CardType Type
		{
			get
			{
				return CardType.Skill;
			}
		}

		// Token: 0x1700217E RID: 8574
		// (get) Token: 0x06007A1F RID: 31263 RVA: 0x0029CDF0 File Offset: 0x0029AFF0
		public override TargetType TargetType
		{
			get
			{
				return this._targetType;
			}
		}

		// Token: 0x1700217F RID: 8575
		// (get) Token: 0x06007A20 RID: 31264 RVA: 0x0029CDF8 File Offset: 0x0029AFF8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(5m, ValueProp.Move),
					new CardsVar("Draw", 0),
					new CardsVar("Discard", 0),
					new EnergyVar(0),
					new ForgeVar(0),
					new StarsVar(0),
					new SummonVar(0m)
				});
			}
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x0029CE63 File Offset: 0x0029B063
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this._powerApplications = this._powerApplications.ToList<MockSkillCard.PowerApplication>();
			this._cardCreations = this._cardCreations.ToList<MockSkillCard.CardCreation>();
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x0029CE8D File Offset: 0x0029B08D
		[PreserveBaseOverrides]
		public new MockSkillCard MockBlock(int block)
		{
			base.AssertMutable();
			base.DynamicVars.Block.BaseValue = block;
			return this;
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x0029CEAC File Offset: 0x0029B0AC
		public MockSkillCard MockBlockCount(int blockCount)
		{
			base.AssertMutable();
			this._blockCount = blockCount;
			return this;
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x0029CEBC File Offset: 0x0029B0BC
		public MockSkillCard MockDraw(int cards)
		{
			base.AssertMutable();
			base.DynamicVars["Draw"].BaseValue = cards;
			return this;
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x0029CEE0 File Offset: 0x0029B0E0
		public MockSkillCard MockSummon(int summons)
		{
			base.AssertMutable();
			base.DynamicVars.Summon.BaseValue = summons;
			return this;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x0029CEFF File Offset: 0x0029B0FF
		public MockSkillCard MockDiscard(int cards)
		{
			base.AssertMutable();
			base.DynamicVars["Discard"].BaseValue = cards;
			return this;
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x0029CF23 File Offset: 0x0029B123
		public MockSkillCard MockForge(decimal forge)
		{
			base.AssertMutable();
			base.DynamicVars.Forge.BaseValue = forge;
			return this;
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x0029CF3D File Offset: 0x0029B13D
		public MockSkillCard MockStarGain(decimal stars)
		{
			base.AssertMutable();
			base.DynamicVars.Stars.BaseValue = stars;
			return this;
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x0029CF57 File Offset: 0x0029B157
		public MockSkillCard MockEnergyGain(decimal energy)
		{
			base.AssertMutable();
			base.DynamicVars.Energy.BaseValue = energy;
			return this;
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x0029CF74 File Offset: 0x0029B174
		public MockSkillCard MockPower<[Nullable(0)] TPower>(int amount, TargetType targetType) where TPower : PowerModel
		{
			base.AssertMutable();
			if (this._powerApplications.Any<MockSkillCard.PowerApplication>((MockSkillCard.PowerApplication a) => a.targetType != targetType))
			{
				throw new InvalidOperationException("Cannot have multiple power applications with different target types.");
			}
			this._targetType = targetType;
			this._powerApplications.Add(new MockSkillCard.PowerApplication
			{
				powerType = typeof(TPower),
				amount = amount,
				targetType = targetType
			});
			return this;
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x0029D000 File Offset: 0x0029B200
		public MockSkillCard MockCreateCards<[Nullable(0)] TCard>(int amount) where TCard : CardModel
		{
			base.AssertMutable();
			this._cardCreations.Add(new MockSkillCard.CardCreation
			{
				canonicalCard = ModelDb.Card<TCard>(),
				amount = amount
			});
			return this;
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x0029D041 File Offset: 0x0029B241
		protected override int GetBaseBlock()
		{
			return base.DynamicVars.Block.IntValue;
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x0029D054 File Offset: 0x0029B254
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this._mockSelfHpLoss > 0)
			{
				await CreatureCmd.Damage(choiceContext, base.Owner.Creature, this._mockSelfHpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			}
			for (int i = 0; i < this._blockCount; i++)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
			if (base.DynamicVars["Draw"].IntValue > 0)
			{
				await CardPileCmd.Draw(choiceContext, base.DynamicVars["Draw"].IntValue, base.Owner, false);
			}
			if (base.DynamicVars["Discard"].IntValue > 0)
			{
				CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(new LocString("cards", "MOCK_SKILL_CARD.discardSelectionPrompt"), base.DynamicVars["Discard"].IntValue);
				await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, cardSelectorPrefs, null, this));
			}
			if (base.DynamicVars.Forge.BaseValue > 0m)
			{
				await ForgeCmd.Forge(base.DynamicVars.Forge.BaseValue, base.Owner, this);
			}
			if (base.DynamicVars.Stars.BaseValue > 0m)
			{
				await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			}
			if (base.DynamicVars.Summon.BaseValue > 0m)
			{
				await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			}
			if (base.DynamicVars.Energy.BaseValue > 0m)
			{
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
			}
			foreach (MockSkillCard.PowerApplication application in this._powerApplications)
			{
				IReadOnlyList<Creature> readOnlyList;
				switch (application.targetType)
				{
				case TargetType.Self:
					readOnlyList = new <>z__ReadOnlySingleElementList<Creature>(base.Owner.Creature);
					break;
				case TargetType.AnyEnemy:
					readOnlyList = new <>z__ReadOnlySingleElementList<Creature>(cardPlay.Target);
					break;
				case TargetType.AllEnemies:
					readOnlyList = base.CombatState.Enemies;
					break;
				default:
					throw new ArgumentOutOfRangeException("targetType", application.targetType, null);
				}
				foreach (Creature creature in readOnlyList)
				{
					await PowerCmd.Apply(choiceContext, ModelDb.GetById<PowerModel>(ModelDb.GetId(application.powerType)).ToMutable(0), creature, application.amount, base.Owner.Creature, this, false);
				}
				IEnumerator<Creature> enumerator2 = null;
				application = default(MockSkillCard.PowerApplication);
			}
			List<MockSkillCard.PowerApplication>.Enumerator enumerator = default(List<MockSkillCard.PowerApplication>.Enumerator);
			foreach (MockSkillCard.CardCreation cardCreation in this._cardCreations)
			{
				CardModel canonicalCard = cardCreation.canonicalCard;
				if (!(canonicalCard is Shiv))
				{
					if (!(canonicalCard is Soul))
					{
						List<CardModel> list = new List<CardModel>();
						for (int j = 0; j < cardCreation.amount; j++)
						{
							list.Add(base.CombatState.CreateCard(cardCreation.canonicalCard, base.Owner));
						}
						await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
					}
					else
					{
						await Soul.CreateInHand(base.Owner, cardCreation.amount, base.CombatState, null);
					}
				}
				else
				{
					await Shiv.CreateInHand(base.Owner, cardCreation.amount, base.CombatState, null);
				}
			}
			List<MockSkillCard.CardCreation>.Enumerator enumerator3 = default(List<MockSkillCard.CardCreation>.Enumerator);
			if (this._mockExtraLogic != null)
			{
				await this._mockExtraLogic(this);
			}
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x0029D0A7 File Offset: 0x0029B2A7
		protected override void OnUpgrade()
		{
			if (this._mockUpgradeLogic != null)
			{
				this._mockUpgradeLogic(this);
				return;
			}
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x040028EB RID: 10475
		private const string _drawKey = "Draw";

		// Token: 0x040028EC RID: 10476
		private const string _discardKey = "Discard";

		// Token: 0x040028ED RID: 10477
		private int _blockCount = 1;

		// Token: 0x040028EE RID: 10478
		private TargetType _targetType = TargetType.Self;

		// Token: 0x040028EF RID: 10479
		private List<MockSkillCard.PowerApplication> _powerApplications = new List<MockSkillCard.PowerApplication>();

		// Token: 0x040028F0 RID: 10480
		private List<MockSkillCard.CardCreation> _cardCreations = new List<MockSkillCard.CardCreation>();

		// Token: 0x0200229E RID: 8862
		[NullableContext(0)]
		private struct PowerApplication
		{
			// Token: 0x040091DF RID: 37343
			[Nullable(1)]
			public Type powerType;

			// Token: 0x040091E0 RID: 37344
			public int amount;

			// Token: 0x040091E1 RID: 37345
			public TargetType targetType;
		}

		// Token: 0x0200229F RID: 8863
		[NullableContext(0)]
		private struct CardCreation
		{
			// Token: 0x040091E2 RID: 37346
			[Nullable(1)]
			public CardModel canonicalCard;

			// Token: 0x040091E3 RID: 37347
			public int amount;
		}
	}
}
