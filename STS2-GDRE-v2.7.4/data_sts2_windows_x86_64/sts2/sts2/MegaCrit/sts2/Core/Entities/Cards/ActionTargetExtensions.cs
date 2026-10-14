using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CC8 RID: 3272
	public static class ActionTargetExtensions
	{
		// Token: 0x060084F1 RID: 34033 RVA: 0x002CB3D0 File Offset: 0x002C95D0
		public static bool IsSingleTarget(this TargetType targetType)
		{
			switch (targetType)
			{
			case TargetType.Self:
			case TargetType.AnyEnemy:
			case TargetType.AnyPlayer:
			case TargetType.AnyAlly:
			case TargetType.TargetedNoCreature:
				return true;
			}
			return false;
		}
	}
}
