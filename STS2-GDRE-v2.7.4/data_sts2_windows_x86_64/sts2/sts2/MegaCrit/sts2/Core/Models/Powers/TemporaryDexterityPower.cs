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
	/// This class represents a buff/debuff that gives/takes dexterity.
	/// We never instantiate this directly. See <see cref="T:MegaCrit.Sts2.Core.Models.Powers.TemporaryStrengthPower" /> for context around how this is used.
	/// </summary>
	// Token: 0x02000714 RID: 1812
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class TemporaryDexterityPower : PowerModel, ITemporaryPower
	{
		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x06005BF3 RID: 23539 RVA: 0x002554B0 File Offset: 0x002536B0
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

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x002554BD File Offset: 0x002536BD
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		/// <summary>
		/// The canonical model that applies this power. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Anticipate" />.
		/// </summary>
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06005BF5 RID: 23541
		public abstract AbstractModel OriginModel { get; }

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x002554C0 File Offset: 0x002536C0
		public PowerModel InternallyAppliedPower
		{
			get
			{
				return ModelDb.Power<DexterityPower>();
			}
		}

		/// <summary>
		/// If this power is supposed to apply negative dexterity, make this false
		/// </summary>
		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x002554C7 File Offset: 0x002536C7
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
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x002554CA File Offset: 0x002536CA
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

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x002554D8 File Offset: 0x002536D8
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

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x00255532 File Offset: 0x00253732
		public override LocString Description
		{
			get
			{
				return new LocString("powers", this.IsPositive ? "TEMPORARY_DEXTERITY_POWER.description" : "TEMPORARY_DEXTERITY_DOWN.description");
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x00255552 File Offset: 0x00253752
		protected override string SmartDescriptionLocKey
		{
			get
			{
				if (!this.IsPositive)
				{
					return "TEMPORARY_DEXTERITY_DOWN.smartDescription";
				}
				return "TEMPORARY_DEXTERITY_POWER.smartDescription";
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x00255568 File Offset: 0x00253768
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
				list.Add(HoverTipFactory.FromPower<DexterityPower>(null));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x002555F8 File Offset: 0x002537F8
		public override async Task BeforeApplied(Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target, this.Sign * amount, applier, cardSource, true);
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x0025565C File Offset: 0x0025385C
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount == base.Amount))
			{
				if (power == this)
				{
					await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, this.Sign * amount, applier, cardSource, true);
				}
			}
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x002556CC File Offset: 0x002538CC
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
				await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, -this.Sign * base.Amount, base.Owner, null, false);
			}
		}
	}
}
