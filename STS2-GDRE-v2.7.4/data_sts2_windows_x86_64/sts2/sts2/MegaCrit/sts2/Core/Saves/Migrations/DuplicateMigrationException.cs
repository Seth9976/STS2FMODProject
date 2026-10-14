using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Exception thrown when there are duplicate migrations from the same version.
	/// </summary>
	// Token: 0x020000DF RID: 223
	public class DuplicateMigrationException : InvalidMigrationPathException
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x000463E1 File Offset: 0x000445E1
		[NullableContext(1)]
		public DuplicateMigrationException(string message)
			: base(message)
		{
		}
	}
}
