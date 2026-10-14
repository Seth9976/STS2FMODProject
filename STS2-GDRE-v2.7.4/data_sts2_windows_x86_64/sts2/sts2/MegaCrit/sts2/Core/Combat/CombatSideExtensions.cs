using System;

namespace MegaCrit.Sts2.Core.Combat
{
	// Token: 0x02000D41 RID: 3393
	internal static class CombatSideExtensions
	{
		// Token: 0x06008923 RID: 35107 RVA: 0x002E1ED0 File Offset: 0x002E00D0
		public static CombatSide GetOppositeSide(this CombatSide side)
		{
			CombatSide combatSide;
			switch (side)
			{
			case CombatSide.None:
				combatSide = CombatSide.None;
				break;
			case CombatSide.Player:
				combatSide = CombatSide.Enemy;
				break;
			case CombatSide.Enemy:
				combatSide = CombatSide.Player;
				break;
			default:
				throw new ArgumentOutOfRangeException("side", side, null);
			}
			return combatSide;
		}
	}
}
