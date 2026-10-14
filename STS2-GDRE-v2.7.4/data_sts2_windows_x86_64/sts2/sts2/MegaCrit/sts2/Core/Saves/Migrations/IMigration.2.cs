using System;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Strongly typed interface for migrations that operate on a specific save type.
	/// </summary>
	/// <typeparam name="T">The save type to migrate</typeparam>
	// Token: 0x020000D7 RID: 215
	public interface IMigration<T> : IMigration where T : ISaveSchema
	{
	}
}
