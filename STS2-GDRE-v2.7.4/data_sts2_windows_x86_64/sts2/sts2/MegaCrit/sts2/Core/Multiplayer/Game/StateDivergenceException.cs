using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x020004A8 RID: 1192
	public class StateDivergenceException : Exception
	{
		// Token: 0x060047E6 RID: 18406 RVA: 0x0021F485 File Offset: 0x0021D685
		[NullableContext(1)]
		public StateDivergenceException(string message)
			: base(message)
		{
		}
	}
}
