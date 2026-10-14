using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000576 RID: 1398
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LavaRock : RelicModel
	{
		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x0600520C RID: 21004 RVA: 0x0024231C File Offset: 0x0024051C
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600520D RID: 21005 RVA: 0x0024231F File Offset: 0x0024051F
		public override bool ShowCounter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600520E RID: 21006 RVA: 0x00242322 File Offset: 0x00240522
		// (set) Token: 0x0600520F RID: 21007 RVA: 0x0024232A File Offset: 0x0024052A
		[SavedProperty]
		public bool HasTriggered
		{
			get
			{
				return this._hasTriggered;
			}
			set
			{
				base.AssertMutable();
				this._hasTriggered = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005210 RID: 21008 RVA: 0x0024233F File Offset: 0x0024053F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 2m));
			}
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x00242358 File Offset: 0x00240558
		public override bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (room == null || room.RoomType != RoomType.Boss)
			{
				return false;
			}
			if (base.Owner.RunState.CurrentActIndex != 0)
			{
				return false;
			}
			if (this.HasTriggered)
			{
				return false;
			}
			base.Flash();
			for (int i = 0; i < base.DynamicVars["Relics"].IntValue; i++)
			{
				rewards.Add(new RelicReward(player));
			}
			this.HasTriggered = true;
			base.Status = RelicStatus.Disabled;
			return true;
		}

		// Token: 0x0400246E RID: 9326
		private const string _relicsKey = "Relics";

		// Token: 0x0400246F RID: 9327
		private bool _hasTriggered;
	}
}
