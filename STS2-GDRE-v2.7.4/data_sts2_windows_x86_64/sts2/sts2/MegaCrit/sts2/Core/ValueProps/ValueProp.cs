using System;

namespace MegaCrit.Sts2.Core.ValueProps
{
	/// <summary>
	/// Aka damage type:
	///   - Unblockable = HP loss like Poison
	///   - Unpowered = Damage from relics, potions, and powers
	///   - Move = Attack damage from Attack cards and Enemy creatures attacking
	/// </summary>
	// Token: 0x02000032 RID: 50
	[Flags]
	public enum ValueProp
	{
		// Token: 0x040000DC RID: 220
		Unblockable = 2,
		// Token: 0x040000DD RID: 221
		Unpowered = 4,
		// Token: 0x040000DE RID: 222
		Move = 8,
		// Token: 0x040000DF RID: 223
		SkipHurtAnim = 16
	}
}
