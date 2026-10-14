using System;

namespace MegaCrit.Sts2.Core.ValueProps
{
	// Token: 0x02000033 RID: 51
	public static class ValuePropExtensions
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00011320 File Offset: 0x0000F520
		public static bool IsPoweredAttack(this ValueProp props)
		{
			return props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0001134B File Offset: 0x0000F54B
		public static bool IsPoweredCardOrMonsterMoveBlock(this ValueProp props)
		{
			return props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00011376 File Offset: 0x0000F576
		public static bool IsCardOrMonsterMove(this ValueProp props)
		{
			return props.HasFlag(ValueProp.Move);
		}
	}
}
