using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000609 RID: 1545
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnsettlingLamp : RelicModel
	{
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x060055DC RID: 21980 RVA: 0x0024963D File Offset: 0x0024783D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x060055DD RID: 21981 RVA: 0x00249640 File Offset: 0x00247840
		// (set) Token: 0x060055DE RID: 21982 RVA: 0x00249648 File Offset: 0x00247848
		[Nullable(2)]
		private CardModel TriggeringCard
		{
			[NullableContext(2)]
			get
			{
				return this._triggeringCard;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._triggeringCard = value;
			}
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x060055DF RID: 21983 RVA: 0x00249657 File Offset: 0x00247857
		private List<PowerModel> DoubledPowers
		{
			get
			{
				base.AssertMutable();
				if (this._doubledPowers == null)
				{
					this._doubledPowers = new List<PowerModel>();
				}
				return this._doubledPowers;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x060055E0 RID: 21984 RVA: 0x00249678 File Offset: 0x00247878
		// (set) Token: 0x060055E1 RID: 21985 RVA: 0x00249680 File Offset: 0x00247880
		private bool IsFinishedTriggering
		{
			get
			{
				return this._isFinishedTriggering;
			}
			set
			{
				base.AssertMutable();
				this._isFinishedTriggering = value;
			}
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x0024968F File Offset: 0x0024788F
		public override Task BeforeCombatStart()
		{
			this.TriggeringCard = null;
			this.DoubledPowers.Clear();
			this.IsFinishedTriggering = false;
			base.Status = RelicStatus.Active;
			return Task.CompletedTask;
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x002496B8 File Offset: 0x002478B8
		public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (this.TriggeringCard != null)
			{
				return Task.CompletedTask;
			}
			if (this.IsFinishedTriggering)
			{
				return Task.CompletedTask;
			}
			if (cardSource == null)
			{
				return Task.CompletedTask;
			}
			if (applier != base.Owner.Creature)
			{
				return Task.CompletedTask;
			}
			if (target.Side == base.Owner.Creature.Side)
			{
				return Task.CompletedTask;
			}
			if (!power.IsVisible)
			{
				return Task.CompletedTask;
			}
			if (power.GetTypeForAmount(amount) != PowerType.Debuff)
			{
				return Task.CompletedTask;
			}
			if (target.HasPower<ArtifactPower>())
			{
				return Task.CompletedTask;
			}
			this.TriggeringCard = cardSource;
			this.DoubledPowers.Add(power);
			return Task.CompletedTask;
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x00249764 File Offset: 0x00247964
		public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, [Nullable(2)] Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (this.TriggeringCard == null)
			{
				return 1m;
			}
			if (cardSource != this.TriggeringCard)
			{
				return 1m;
			}
			if (this.IsFinishedTriggering)
			{
				return 1m;
			}
			if (this.HasDoubledTemporaryPowerSource(power))
			{
				return 1m;
			}
			if (power.GetTypeForAmount(amount) != PowerType.Debuff)
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x002497C2 File Offset: 0x002479C2
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card != this.TriggeringCard)
			{
				return Task.CompletedTask;
			}
			if (this.IsFinishedTriggering)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			this.IsFinishedTriggering = true;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x002497FF File Offset: 0x002479FF
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.TriggeringCard = null;
			this.DoubledPowers.Clear();
			this.IsFinishedTriggering = false;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		/// <summary>
		/// If we've doubled an <see cref="T:MegaCrit.Sts2.Core.Models.ITemporaryPower" /> and now we're trying to double its internal debuff,
		/// skip it so we don't double-dip.
		/// </summary>
		/// <example>
		/// If we've doubled <see cref="T:MegaCrit.Sts2.Core.Models.Powers.PiercingWailPower" />, we don't also want to double negative <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DexterityPower" />,
		/// because that'll end up quadrupling it.
		/// </example>
		// Token: 0x060055E7 RID: 21991 RVA: 0x00249828 File Offset: 0x00247A28
		private bool HasDoubledTemporaryPowerSource(PowerModel power)
		{
			return this.DoubledPowers.OfType<ITemporaryPower>().Any<ITemporaryPower>((ITemporaryPower p) => p.InternallyAppliedPower.GetType() == power.GetType());
		}

		// Token: 0x040024DD RID: 9437
		[Nullable(2)]
		private CardModel _triggeringCard;

		// Token: 0x040024DE RID: 9438
		[Nullable(new byte[] { 2, 1 })]
		private List<PowerModel> _doubledPowers;

		// Token: 0x040024DF RID: 9439
		private bool _isFinishedTriggering;
	}
}
