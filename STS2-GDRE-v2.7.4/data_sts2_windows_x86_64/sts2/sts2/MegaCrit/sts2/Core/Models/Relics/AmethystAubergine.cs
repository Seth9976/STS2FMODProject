using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F3 RID: 1267
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AmethystAubergine : RelicModel
	{
		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x0023C013 File Offset: 0x0023A213
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x0023C016 File Offset: 0x0023A216
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x0023C01E File Offset: 0x0023A21E
		public override bool IsAllowedInShops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0023C021 File Offset: 0x0023A221
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(15));
			}
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x0023C030 File Offset: 0x0023A230
		public override bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (room == null)
			{
				return false;
			}
			if (!room.RoomType.IsCombatRoom())
			{
				return false;
			}
			if (room.RoomType == RoomType.Boss && player.RunState.CurrentActIndex >= player.RunState.Acts.Count - 1)
			{
				return false;
			}
			rewards.Add(new GoldReward(base.DynamicVars.Gold.IntValue, player, false));
			return true;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x0023C0A4 File Offset: 0x0023A2A4
		public override Task AfterModifyingRewards()
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
