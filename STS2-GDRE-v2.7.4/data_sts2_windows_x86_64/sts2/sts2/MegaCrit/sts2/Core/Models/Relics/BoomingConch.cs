using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200050D RID: 1293
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoomingConch : RelicModel
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06004F72 RID: 20338 RVA: 0x0023D5D3 File Offset: 0x0023B7D3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x0023D5D6 File Offset: 0x0023B7D6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06004F74 RID: 20340 RVA: 0x0023D5E3 File Offset: 0x0023B7E3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x0023D604 File Offset: 0x0023B804
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			if (base.Owner.PlayerCombatState.TurnNumber > 1)
			{
				return count;
			}
			AbstractRoom currentRoom = player.RunState.CurrentRoom;
			if (currentRoom == null || currentRoom.RoomType != RoomType.Elite)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.IntValue;
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x0023D670 File Offset: 0x0023B870
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					AbstractRoom currentRoom = combatState.RunState.CurrentRoom;
					if (currentRoom != null && currentRoom.RoomType == RoomType.Elite)
					{
						base.Flash();
						await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					}
				}
			}
		}
	}
}
