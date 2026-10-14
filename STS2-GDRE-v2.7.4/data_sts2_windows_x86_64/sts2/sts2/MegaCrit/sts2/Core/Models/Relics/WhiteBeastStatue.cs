using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000616 RID: 1558
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WhiteBeastStatue : RelicModel
	{
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06005635 RID: 22069 RVA: 0x0024A1EB File Offset: 0x002483EB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x0024A1EE File Offset: 0x002483EE
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x0024A1F6 File Offset: 0x002483F6
		public override bool ShouldForcePotionReward(Player player, RoomType roomType)
		{
			return player == base.Owner && roomType.IsCombatRoom();
		}
	}
}
