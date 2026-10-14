using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Exception thrown when there are gaps in the migration path.
	/// </summary>
	// Token: 0x020000DE RID: 222
	public class MigrationPathGapException : InvalidMigrationPathException
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x000463D8 File Offset: 0x000445D8
		[NullableContext(1)]
		public MigrationPathGapException(string message)
			: base(message)
		{
		}
	}
}
