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
	// Token: 0x020006CE RID: 1742
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PossessSpeedPower : PowerModel
	{
		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06005A44 RID: 23108 RVA: 0x00251F83 File Offset: 0x00250183
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06005A45 RID: 23109 RVA: 0x00251F86 File Offset: 0x00250186
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x00251F89 File Offset: 0x00250189
		protected override object InitInternalData()
		{
			return new PossessSpeedPower.Data();
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06005A47 RID: 23111 RVA: 0x00251F90 File Offset: 0x00250190
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06005A48 RID: 23112 RVA: 0x00251FB0 File Offset: 0x002501B0
		private Dictionary<Creature, decimal> StolenDexterity
		{
			get
			{
				return base.GetInternalData<PossessSpeedPower.Data>().stolenDexterity;
			}
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x00251FC0 File Offset: 0x002501C0
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
			if (!(power is DexterityPower))
			{
				return Task.CompletedTask;
			}
			if (amount >= 0m)
			{
				return Task.CompletedTask;
			}
			if (!this.StolenDexterity.ContainsKey(power.Owner))
			{
				this.StolenDexterity.Add(power.Owner, 0m);
			}
			Dictionary<Creature, decimal> stolenDexterity = this.StolenDexterity;
			Creature owner = power.Owner;
			stolenDexterity[owner] += amount;
			return Task.CompletedTask;
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x00252064 File Offset: 0x00250264
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					foreach (KeyValuePair<Creature, decimal> keyValuePair in this.StolenDexterity)
					{
						await PowerCmd.Apply<DexterityPower>(choiceContext, keyValuePair.Key, -keyValuePair.Value, null, null, false);
					}
					Dictionary<Creature, decimal>.Enumerator enumerator = default(Dictionary<Creature, decimal>.Enumerator);
				}
			}
		}

		// Token: 0x02001C2E RID: 7214
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x040070BA RID: 28858
			[Nullable(1)]
			public Dictionary<Creature, decimal> stolenDexterity = new Dictionary<Creature, decimal>();
		}
	}
}
