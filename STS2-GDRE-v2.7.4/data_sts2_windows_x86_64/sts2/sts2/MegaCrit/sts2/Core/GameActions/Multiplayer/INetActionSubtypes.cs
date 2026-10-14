using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.DevConsole;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C42 RID: 3138
	public static class INetActionSubtypes
	{
		// Token: 0x17002359 RID: 9049
		// (get) Token: 0x0600816F RID: 33135 RVA: 0x002BE344 File Offset: 0x002BC544
		public static int Count
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x1700235A RID: 9050
		// (get) Token: 0x06008170 RID: 33136 RVA: 0x002BE348 File Offset: 0x002BC548
		public static IReadOnlyList<Type> All
		{
			get
			{
				return INetActionSubtypes._subtypes;
			}
		}

		// Token: 0x06008171 RID: 33137 RVA: 0x002BE34F File Offset: 0x002BC54F
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		public static Type Get(int i)
		{
			return INetActionSubtypes._subtypes[i];
		}

		// Token: 0x04002B59 RID: 11097
		private static readonly Type _t0 = typeof(NetConsoleCmdGameAction);

		// Token: 0x04002B5A RID: 11098
		private static readonly Type _t1 = typeof(NetDiscardPotionGameAction);

		// Token: 0x04002B5B RID: 11099
		private static readonly Type _t2 = typeof(NetEndPlayerTurnAction);

		// Token: 0x04002B5C RID: 11100
		private static readonly Type _t3 = typeof(NetMoveToMapCoordAction);

		// Token: 0x04002B5D RID: 11101
		private static readonly Type _t4 = typeof(NetPickRelicAction);

		// Token: 0x04002B5E RID: 11102
		private static readonly Type _t5 = typeof(NetPlayCardAction);

		// Token: 0x04002B5F RID: 11103
		private static readonly Type _t6 = typeof(NetReadyToBeginEnemyTurnAction);

		// Token: 0x04002B60 RID: 11104
		private static readonly Type _t7 = typeof(NetUndoEndPlayerTurnAction);

		// Token: 0x04002B61 RID: 11105
		private static readonly Type _t8 = typeof(NetUsePotionAction);

		// Token: 0x04002B62 RID: 11106
		private static readonly Type _t9 = typeof(NetVoteForMapCoordAction);

		// Token: 0x04002B63 RID: 11107
		private static readonly Type _t10 = typeof(NetVoteToMoveToNextActAction);

		// Token: 0x04002B64 RID: 11108
		private static readonly Type[] _subtypes = new Type[]
		{
			INetActionSubtypes._t0,
			INetActionSubtypes._t1,
			INetActionSubtypes._t2,
			INetActionSubtypes._t3,
			INetActionSubtypes._t4,
			INetActionSubtypes._t5,
			INetActionSubtypes._t6,
			INetActionSubtypes._t7,
			INetActionSubtypes._t8,
			INetActionSubtypes._t9,
			INetActionSubtypes._t10
		};
	}
}
