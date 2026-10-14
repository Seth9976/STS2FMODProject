using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000860 RID: 2144
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TinkerTime : EventModel
	{
		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x0027C2CB File Offset: 0x0027A4CB
		// (set) Token: 0x06006966 RID: 26982 RVA: 0x0027C2D3 File Offset: 0x0027A4D3
		private CardType ChosenCardType
		{
			get
			{
				return this._chosenCardType;
			}
			set
			{
				base.AssertMutable();
				this._chosenCardType = value;
			}
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x0027C2E2 File Offset: 0x0027A4E2
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.ChooseCardType), "TINKER_TIME.pages.INITIAL.options.CHOOSE_CARD_TYPE", Array.Empty<IHoverTip>()));
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x06006968 RID: 26984 RVA: 0x0027C308 File Offset: 0x0027A508
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
					new DynamicVar("CuriousReduction", 1m),
					new EnergyVar("energyPrefix", 1)
				});
			}
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x0027C3F4 File Offset: 0x0027A5F4
		private Task ChooseCardType()
		{
			IEnumerable<EventOption> enumerable = new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Attack), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.ATTACK", new IHoverTip[] { this.GetCardTypeHoverTip(CardType.Attack) }),
				new EventOption(this, new Func<Task>(this.Skill), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.SKILL", new IHoverTip[] { this.GetCardTypeHoverTip(CardType.Skill) }),
				new EventOption(this, new Func<Task>(this.Power), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.POWER", new IHoverTip[] { this.GetCardTypeHoverTip(CardType.Power) })
			});
			this.SetEventState(base.L10NLookup("TINKER_TIME.pages.CHOOSE_CARD_TYPE.description"), enumerable.TakeRandom<EventOption>(2, base.Rng));
			return Task.CompletedTask;
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x0027C4B0 File Offset: 0x0027A6B0
		private CardHoverTip GetCardTypeHoverTip(CardType cardType)
		{
			MadScience madScience = base.Owner.RunState.CreateCard<MadScience>(base.Owner);
			madScience.TinkerTimeType = cardType;
			madScience.TinkerTimeRider = TinkerTime.RiderEffect.None;
			return new CardHoverTip(madScience);
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x0027C4E8 File Offset: 0x0027A6E8
		private Task Attack()
		{
			this.ChosenCardType = CardType.Attack;
			return this.ChooseRiderEffect();
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x0027C4F7 File Offset: 0x0027A6F7
		private Task Skill()
		{
			this.ChosenCardType = CardType.Skill;
			return this.ChooseRiderEffect();
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x0027C506 File Offset: 0x0027A706
		private Task Power()
		{
			this.ChosenCardType = CardType.Power;
			return this.ChooseRiderEffect();
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x0027C518 File Offset: 0x0027A718
		private Task ChooseRiderEffect()
		{
			IEnumerable<TinkerTime.RiderEffect> enumerable;
			switch (this.ChosenCardType)
			{
			case CardType.Attack:
				enumerable = new <>z__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[]
				{
					TinkerTime.RiderEffect.Sapping,
					TinkerTime.RiderEffect.Violence,
					TinkerTime.RiderEffect.Choking
				});
				break;
			case CardType.Skill:
				enumerable = new <>z__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[]
				{
					TinkerTime.RiderEffect.Energized,
					TinkerTime.RiderEffect.Wisdom,
					TinkerTime.RiderEffect.Chaos
				});
				break;
			case CardType.Power:
				enumerable = new <>z__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[]
				{
					TinkerTime.RiderEffect.Expertise,
					TinkerTime.RiderEffect.Curious,
					TinkerTime.RiderEffect.Improvement
				});
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			IEnumerable<TinkerTime.RiderEffect> enumerable2 = enumerable;
			List<TinkerTime.RiderEffect> riders = enumerable2.TakeRandom<TinkerTime.RiderEffect>(2, base.Rng).ToList<TinkerTime.RiderEffect>();
			this.SetEventState(base.L10NLookup("TINKER_TIME.pages.CHOOSE_RIDER.description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, () => this.RiderChosen(riders[0]), TinkerTime.GetRiderLocKey(riders[0]), new IHoverTip[] { this.GetRiderHoverTip(riders[0]) }),
				new EventOption(this, () => this.RiderChosen(riders[1]), TinkerTime.GetRiderLocKey(riders[1]), new IHoverTip[] { this.GetRiderHoverTip(riders[1]) })
			}));
			return Task.CompletedTask;
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x0027C660 File Offset: 0x0027A860
		private CardHoverTip GetRiderHoverTip(TinkerTime.RiderEffect rider)
		{
			MadScience madScience = base.Owner.RunState.CreateCard<MadScience>(base.Owner);
			madScience.TinkerTimeType = this.ChosenCardType;
			madScience.TinkerTimeRider = rider;
			return new CardHoverTip(madScience);
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x0027C6A0 File Offset: 0x0027A8A0
		private async Task RiderChosen(TinkerTime.RiderEffect rider)
		{
			MadScience madScience = base.Owner.RunState.CreateCard<MadScience>(base.Owner);
			madScience.TinkerTimeType = this.ChosenCardType;
			madScience.TinkerTimeRider = rider;
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(madScience, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 3f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("TINKER_TIME.pages.DONE.description"));
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x0027C6EC File Offset: 0x0027A8EC
		private static string GetRiderLocKey(TinkerTime.RiderEffect rider)
		{
			string text;
			switch (rider)
			{
			case TinkerTime.RiderEffect.None:
				throw new ArgumentOutOfRangeException("rider", rider, "None is not a valid rider");
			case TinkerTime.RiderEffect.Sapping:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.SAPPING";
				break;
			case TinkerTime.RiderEffect.Violence:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.VIOLENCE";
				break;
			case TinkerTime.RiderEffect.Choking:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.CHOKING";
				break;
			case TinkerTime.RiderEffect.Energized:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.ENERGIZED";
				break;
			case TinkerTime.RiderEffect.Wisdom:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.WISDOM";
				break;
			case TinkerTime.RiderEffect.Chaos:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.CHAOS";
				break;
			case TinkerTime.RiderEffect.Expertise:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.EXPERTISE";
				break;
			case TinkerTime.RiderEffect.Curious:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.CURIOUS";
				break;
			case TinkerTime.RiderEffect.Improvement:
				text = "TINKER_TIME.pages.CHOOSE_RIDER.options.IMPROVEMENT";
				break;
			default:
				throw new ArgumentOutOfRangeException("rider", rider, null);
			}
			return text;
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x0027C79C File Offset: 0x0027A99C
		public static IHoverTip[] GetRiderHoverTips(TinkerTime.RiderEffect rider)
		{
			IHoverTip[] array;
			switch (rider)
			{
			case TinkerTime.RiderEffect.None:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Sapping:
				array = new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				};
				break;
			case TinkerTime.RiderEffect.Violence:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Choking:
				array = new IHoverTip[] { HoverTipFactory.FromPower<StranglePower>(null) };
				break;
			case TinkerTime.RiderEffect.Energized:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Wisdom:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Chaos:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Expertise:
				array = new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				};
				break;
			case TinkerTime.RiderEffect.Curious:
				array = Array.Empty<IHoverTip>();
				break;
			case TinkerTime.RiderEffect.Improvement:
				array = Array.Empty<IHoverTip>();
				break;
			default:
				throw new ArgumentOutOfRangeException("rider", rider, null);
			}
			return array;
		}

		// Token: 0x040027DE RID: 10206
		private CardType _chosenCardType;

		/// <summary>
		/// A "rider" is an additional effect that accompanies the card's base effect.
		/// The MadScience card has a base effect (deal damage, gain block, or apply a power)
		/// plus a rider effect chosen by the player during the TinkerTime event.
		/// Each card type (Attack/Skill/Power) has three possible riders to choose from.
		/// </summary>
		// Token: 0x02001F7F RID: 8063
		[NullableContext(0)]
		public enum RiderEffect
		{
			// Token: 0x040080AA RID: 32938
			None,
			// Token: 0x040080AB RID: 32939
			Sapping,
			// Token: 0x040080AC RID: 32940
			Violence,
			// Token: 0x040080AD RID: 32941
			Choking,
			// Token: 0x040080AE RID: 32942
			Energized,
			// Token: 0x040080AF RID: 32943
			Wisdom,
			// Token: 0x040080B0 RID: 32944
			Chaos,
			// Token: 0x040080B1 RID: 32945
			Expertise,
			// Token: 0x040080B2 RID: 32946
			Curious,
			// Token: 0x040080B3 RID: 32947
			Improvement
		}
	}
}
