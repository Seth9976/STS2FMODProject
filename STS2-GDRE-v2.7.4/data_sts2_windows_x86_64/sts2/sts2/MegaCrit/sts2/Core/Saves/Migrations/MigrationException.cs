using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Exception thrown when a migration fails.
	/// </summary>
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	public class MigrationException : Exception
	{
		// Token: 0x06000A5F RID: 2655 RVA: 0x000463B3 File Offset: 0x000445B3
		public MigrationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000463BC File Offset: 0x000445BC
		public MigrationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
