using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Base class for migrations that automatically derives migration properties from the Migration attribute
	/// and uses JsonObject for migration operations.
	/// </summary>
	/// <typeparam name="T">The save type to migrate</typeparam>
	// Token: 0x020000DA RID: 218
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MigrationBase<[Nullable(0)] T> : IMigration<T>, IMigration where T : ISaveSchema
	{
		// Token: 0x06000A58 RID: 2648 RVA: 0x000462F5 File Offset: 0x000444F5
		protected MigrationBase()
		{
			this._migrationAttribute = new Lazy<MigrationAttribute>(delegate
			{
				object[] customAttributes = base.GetType().GetCustomAttributes(typeof(MigrationAttribute), false);
				if (customAttributes.Length == 0)
				{
					throw new InvalidOperationException(base.GetType().Name + " is missing the [Migration] attribute");
				}
				return (MigrationAttribute)customAttributes[0];
			});
		}

		/// <summary>
		/// The version to migrate from. Derived from the Migration attribute.
		/// </summary>
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00046314 File Offset: 0x00044514
		public int FromVersion
		{
			get
			{
				return this._migrationAttribute.Value.FromVersion;
			}
		}

		/// <summary>
		/// The version to migrate to. Derived from the Migration attribute.
		/// </summary>
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00046326 File Offset: 0x00044526
		public int ToVersion
		{
			get
			{
				return this._migrationAttribute.Value.ToVersion;
			}
		}

		/// <summary>
		/// The type of save that this migration operates on. Derived from the Migration attribute.
		/// </summary>
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00046338 File Offset: 0x00044538
		public Type SaveType
		{
			get
			{
				return this._migrationAttribute.Value.SaveType;
			}
		}

		/// <summary>
		/// Migrates the JSON object from the old version to the new version.
		/// </summary>
		/// <param name="saveData">The JSON object to migrate</param>
		/// <returns>The migrated JSON object</returns>
		// Token: 0x06000A5C RID: 2652 RVA: 0x0004634A File Offset: 0x0004454A
		public MigratingData Migrate(MigratingData saveData)
		{
			this.ApplyMigration(saveData);
			saveData.Set<int>("schema_version", this.ToVersion);
			return saveData;
		}

		/// <summary>
		/// Implement this in derived classes to modify the JSON object.
		/// </summary>
		/// <param name="saveData">The JSON object to modify</param>
		// Token: 0x06000A5D RID: 2653
		protected abstract void ApplyMigration(MigratingData saveData);

		// Token: 0x04000390 RID: 912
		private readonly Lazy<MigrationAttribute> _migrationAttribute;
	}
}
