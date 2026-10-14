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
	/// This class represents a buff/debuff that gives/takes focus.
	/// We never instantiate this directly. See <see cref="T:MegaCrit.Sts2.Core.Models.Powers.TemporaryStrengthPower" /> for context around how this is used.
	/// </summary>
	// Token: 0x02000715 RID: 1813
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class TemporaryFocusPower : PowerModel, ITemporaryPower
	{
		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06005C01 RID: 23553 RVA: 0x00255727 File Offset: 0x00253927
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

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06005C02 RID: 23554 RVA: 0x00255734 File Offset: 0x00253934
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		/// <summary>
		/// The canonical model that applies this power. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Hotfix" />.
		/// </summary>
		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06005C03 RID: 23555
		public abstract AbstractModel OriginModel { get; }

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06005C04 RID: 23556 RVA: 0x00255737 File Offset: 0x00253937
		public PowerModel InternallyAppliedPower
		{
			get
			{
				return ModelDb.Power<FocusPower>();
			}
		}

		/// <summary>
		/// If this power is supposed to apply negative Focus, make this false
		/// </summary>
		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06005C05 RID: 23557 RVA: 0x0025573E File Offset: 0x0025393E
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
		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06005C06 RID: 23558 RVA: 0x00255741 File Offset: 0x00253941
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

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x06005C07 RID: 23559 RVA: 0x00255750 File Offset: 0x00253950
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

		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x06005C08 RID: 23560 RVA: 0x002557AA File Offset: 0x002539AA
		public override LocString Description
		{
			get
			{
				return new LocString("powers", this.IsPositive ? "TEMPORARY_FOCUS_POWER.description" : "TEMPORARY_FOCUS_DOWN.description");
			}
		}

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x06005C09 RID: 23561 RVA: 0x002557CA File Offset: 0x002539CA
		protected override string SmartDescriptionLocKey
		{
			get
			{
				if (!this.IsPositive)
				{
					return "TEMPORARY_FOCUS_DOWN.smartDescription";
				}
				return "TEMPORARY_FOCUS_POWER.smartDescription";
			}
		}

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x06005C0A RID: 23562 RVA: 0x002557E0 File Offset: 0x002539E0
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
					PotionModel potionModel = originModel as PotionModel;
					if (potionModel == null)
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
						enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPotion(potionModel));
					}
				}
				else
				{
					enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(cardModel, false));
				}
				list2.AddRange(enumerable);
				list.Add(HoverTipFactory.FromPower<FocusPower>(null));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x00255878 File Offset: 0x00253A78
		public override async Task BeforeApplied(Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), target, this.Sign * amount, applier, cardSource, true);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x002558DC File Offset: 0x00253ADC
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount == base.Amount))
			{
				if (power == this)
				{
					await PowerCmd.Apply<FocusPower>(choiceContext, base.Owner, this.Sign * amount, applier, cardSource, true);
				}
			}
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x0025594C File Offset: 0x00253B4C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
				await PowerCmd.Apply<FocusPower>(choiceContext, base.Owner, -this.Sign * base.Amount, base.Owner, null, false);
			}
		}
	}
}
