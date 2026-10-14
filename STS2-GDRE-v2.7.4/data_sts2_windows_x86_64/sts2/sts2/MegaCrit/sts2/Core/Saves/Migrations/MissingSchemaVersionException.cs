using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Exception thrown when a schema version is missing from a save file.
	/// </summary>
	// Token: 0x020000DC RID: 220
	public class MissingSchemaVersionException : Exception
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x000463C6 File Offset: 0x000445C6
		[NullableContext(1)]
		public MissingSchemaVersionException(string message)
			: base(message)
		{
		}
	}
}
