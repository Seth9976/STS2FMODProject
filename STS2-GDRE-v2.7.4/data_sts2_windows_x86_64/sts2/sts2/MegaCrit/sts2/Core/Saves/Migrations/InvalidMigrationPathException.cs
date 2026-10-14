using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Base exception for migration path validation failures.
	/// </summary>
	// Token: 0x020000DD RID: 221
	public class InvalidMigrationPathException : Exception
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x000463CF File Offset: 0x000445CF
		[NullableContext(1)]
		protected InvalidMigrationPathException(string message)
			: base(message)
		{
		}
	}
}
