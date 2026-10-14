using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006CF RID: 1743
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PossessStrengthPower : PowerModel
	{
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06005A4C RID: 23116 RVA: 0x002520C7 File Offset: 0x002502C7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06005A4D RID: 23117 RVA: 0x002520CA File Offset: 0x002502CA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x002520CD File Offset: 0x002502CD
		protected override object InitInternalData()
		{
			return new PossessStrengthPower.Data();
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06005A4F RID: 23119 RVA: 0x002520D4 File Offset: 0x002502D4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06005A50 RID: 23120 RVA: 0x002520F4 File Offset: 0x002502F4
		private Dictionary<Creature, decimal> StolenStrength
		{
			get
			{
				return base.GetInternalData<PossessStrengthPower.Data>().stolenStrength;
			}
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x00252104 File Offset: 0x00250304
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (applier != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!power.Owner.IsPlayer)
			{
				return Task.CompletedTask;
			}
			if (!(power is StrengthPower))
			{
				return Task.CompletedTask;
			}
			if (amount >= 0m)
			{
				return Task.CompletedTask;
			}
			if (!this.StolenStrength.ContainsKey(power.Owner))
			{
				this.StolenStrength.Add(power.Owner, 0m);
			}
			Dictionary<Creature, decimal> stolenStrength = this.StolenStrength;
			Creature owner = power.Owner;
			stolenStrength[owner] += amount;
			return Task.CompletedTask;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x002521A8 File Offset: 0x002503A8
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					foreach (KeyValuePair<Creature, decimal> keyValuePair in this.StolenStrength)
					{
						await PowerCmd.Apply<StrengthPower>(choiceContext, keyValuePair.Key, -keyValuePair.Value, null, null, false);
					}
					Dictionary<Creature, decimal>.Enumerator enumerator = default(Dictionary<Creature, decimal>.Enumerator);
				}
			}
		}

		// Token: 0x02001C30 RID: 7216
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x040070C3 RID: 28867
			[Nullable(1)]
			public Dictionary<Creature, decimal> stolenStrength = new Dictionary<Creature, decimal>();
		}
	}
}
