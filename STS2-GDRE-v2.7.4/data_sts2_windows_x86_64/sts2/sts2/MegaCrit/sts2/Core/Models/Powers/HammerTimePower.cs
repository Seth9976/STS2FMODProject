using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200068A RID: 1674
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HammerTimePower : PowerModel
	{
		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x060058B4 RID: 22708 RVA: 0x0024F25F File Offset: 0x0024D45F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x060058B5 RID: 22709 RVA: 0x0024F262 File Offset: 0x0024D462
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x060058B6 RID: 22710 RVA: 0x0024F265 File Offset: 0x0024D465
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x0024F26C File Offset: 0x0024D46C
		public override async Task AfterForge(decimal amount, Player forger, [Nullable(2)] AbstractModel source)
		{
			if (!(source is HammerTimePower))
			{
				if (forger == base.Owner.Player)
				{
					IEnumerable<Player> enumerable = base.CombatState.Players.Where<Player>((Player p) => p.Creature.IsAlive && p != forger);
					foreach (Player player in enumerable)
					{
						await ForgeCmd.Forge(amount, player, this);
					}
					IEnumerator<Player> enumerator = null;
				}
			}
		}
	}
}
