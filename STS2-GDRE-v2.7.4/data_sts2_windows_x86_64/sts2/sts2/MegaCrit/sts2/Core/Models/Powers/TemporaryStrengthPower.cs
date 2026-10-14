using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This class represents a buff/debuff that gives/takes strength.
	/// This class is never instantiated directly. Instead, it is subclassed and those classes represent the buff/debuff.
	/// Every model that grants a temporary strength power creates an associated power that subclasses this. For example,
	/// FlexPotion has FlexPotionPower which grants strength and then takes it away at the end of the turn.
	/// This used to be called Strength Down (or Restore Strength for negative strength) and be its own power. We switched
	/// to this model so make the UX more clear for players; both Strength and Temporary Strength are a buff.
	/// </summary>
	// Token: 0x02000716 RID: 1814
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class TemporaryStrengthPower : PowerModel, ITemporaryPower
	{
		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06005C0F RID: 23567 RVA: 0x002559A7 File Offset: 0x00253BA7
		public override PowerType Type
		{
			get
			{
				if (!this.IsPositive)
				{
					return PowerType.Debuff;
				}
				return PowerType.Buff;
			}
		}

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06005C10 RID: 23568 RVA: 0x002559B4 File Offset: 0x00253BB4
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		/// <summary>
		/// The canonical model that applies this power. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FlexPotion" />.
		/// </summary>
		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06005C11 RID: 23569
		public abstract AbstractModel OriginModel { get; }

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06005C12 RID: 23570 RVA: 0x002559B7 File Offset: 0x00253BB7
		public PowerModel InternallyAppliedPower
		{
			get
			{
				return ModelDb.Power<StrengthPower>();
			}
		}

		/// <summary>
		/// If this power is supposed to apply negative strength, make this false
		/// </summary>
		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x06005C13 RID: 23571 RVA: 0x002559BE File Offset: 0x00253BBE
		protected virtual bool IsPositive
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Shorthand indicating the sign of the amount to apply
		/// </summary>
		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x06005C14 RID: 23572 RVA: 0x002559C1 File Offset: 0x00253BC1
		private int Sign
		{
			get
			{
				if (!this.IsPositive)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06005C15 RID: 23573 RVA: 0x002559D0 File Offset: 0x00253BD0
		public override LocString Title
		{
			get
			{
				AbstractModel originModel = this.OriginModel;
				CardModel cardModel = originModel as CardModel;
				LocString locString;
				if (cardModel == null)
				{
					PotionModel potionModel = originModel as PotionModel;
					if (potionModel == null)
					{
						RelicModel relicModel = originModel as RelicModel;
						if (relicModel == null)
						{
							throw new InvalidOperationException();
						}
						locString = relicModel.Title;
					}
					else
					{
						locString = potionModel.Title;
					}
				}
				else
				{
					locString = cardModel.TitleLocString;
				}
				return locString;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06005C16 RID: 23574 RVA: 0x00255A2A File Offset: 0x00253C2A
		public override LocString Description
		{
			get
			{
				return new LocString("powers", this.IsPositive ? "TEMPORARY_STRENGTH_POWER.description" : "TEMPORARY_STRENGTH_DOWN.description");
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x06005C17 RID: 23575 RVA: 0x00255A4A File Offset: 0x00253C4A
		protected override string SmartDescriptionLocKey
		{
			get
			{
				if (!this.IsPositive)
				{
					return "TEMPORARY_STRENGTH_DOWN.smartDescription";
				}
				return "TEMPORARY_STRENGTH_POWER.smartDescription";
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x06005C18 RID: 23576 RVA: 0x00255A60 File Offset: 0x00253C60
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				List<IHoverTip> list2 = list;
				AbstractModel originModel = this.OriginModel;
				CardModel cardModel = originModel as CardModel;
				IEnumerable<IHoverTip> enumerable;
				if (cardModel == null)
				{
					if (!(originModel is PotionModel))
					{
						RelicModel relicModel = originModel as RelicModel;
						if (relicModel == null)
						{
							throw new InvalidOperationException();
						}
						enumerable = HoverTipFactory.FromRelic(relicModel);
					}
					else
					{
						enumerable = Array.Empty<IHoverTip>();
					}
				}
				else
				{
					enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(cardModel, false));
				}
				list2.AddRange(enumerable);
				list.Add(HoverTipFactory.FromPower<StrengthPower>(null));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x00255AF0 File Offset: 0x00253CF0
		public override async Task BeforeApplied(Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, this.Sign * amount, applier, cardSource, true);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x00255B54 File Offset: 0x00253D54
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount == base.Amount))
			{
				if (power == this)
				{
					await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, this.Sign * amount, applier, cardSource, true);
				}
			}
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x00255BC4 File Offset: 0x00253DC4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -this.Sign * base.Amount, base.Owner, null, false);
			}
		}
	}
}
