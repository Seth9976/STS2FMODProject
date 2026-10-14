using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FA RID: 1530
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThrowingAxe : RelicModel
	{
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06005572 RID: 21874 RVA: 0x0024893E File Offset: 0x00246B3E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06005573 RID: 21875 RVA: 0x00248941 File Offset: 0x00246B41
		// (set) Token: 0x06005574 RID: 21876 RVA: 0x00248949 File Offset: 0x00246B49
		private bool UsedThisCombat
		{
			get
			{
				return this._usedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._usedThisCombat = value;
			}
		}

		// Token: 0x06005575 RID: 21877 RVA: 0x00248958 File Offset: 0x00246B58
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			this.UsedThisCombat = false;
			base.Status = RelicStatus.Active;
			return Task.CompletedTask;
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x0024897B File Offset: 0x00246B7B
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (this.UsedThisCombat)
			{
				return playCount;
			}
			if (card.Owner != base.Owner)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x0024899A File Offset: 0x00246B9A
		public override Task AfterModifyingCardPlayCount(CardModel card)
		{
			this.UsedThisCombat = true;
			base.Flash();
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x002489B5 File Offset: 0x00246BB5
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.UsedThisCombat = false;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x040024CF RID: 9423
		private bool _usedThisCombat;
	}
}
