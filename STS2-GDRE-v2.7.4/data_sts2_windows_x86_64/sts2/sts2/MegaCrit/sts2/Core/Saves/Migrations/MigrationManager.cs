using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Manages save schema migrations and version tracking.
	/// This class combines functionality previously spread across MigrationRegistry, SaveSystemBootstrap,
	/// and migration logic from SaveManager.
	/// </summary>
	// Token: 0x020000E0 RID: 224
	[NullableContext(1)]
	[Nullable(0)]
	public class MigrationManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager" /> class.
		/// </summary>
		/// <param name="saveStore">The save store to use for file operations</param>
		// Token: 0x06000A65 RID: 2661 RVA: 0x000463EA File Offset: 0x000445EA
		public MigrationManager(ISaveStore saveStore)
		{
			this._saveStore = saveStore;
			this.Initialize();
		}

		/// <summary>
		/// Initializes the migration system, registering migrations and setting current versions.
		/// </summary>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.InvalidMigrationPathException">Thrown when migration paths are invalid</exception>
		// Token: 0x06000A66 RID: 2662 RVA: 0x00046420 File Offset: 0x00044620
		private void Initialize()
		{
			this._registry.RegisterAllMigrations(this);
			this.DeriveAndSetLatestVersions();
			this.ValidateMigrationPaths();
			this.SetMinimumSupportedVersion<SerializableRun>(13);
			this.SetMinimumSupportedVersion<SerializableProgress>(21);
			this.SetMinimumSupportedVersion<SettingsSave>(4);
			this.SetMinimumSupportedVersion<RunHistory>(8);
			this.SetMinimumSupportedVersion<PrefsSave>(2);
			this.SetMinimumSupportedVersion<ProfileSave>(2);
		}

		/// <summary>
		/// Derives the latest version numbers dynamically from registered migrations.
		/// </summary>
		// Token: 0x06000A67 RID: 2663 RVA: 0x00046474 File Offset: 0x00044674
		private void DeriveAndSetLatestVersions()
		{
			foreach (Type type in this._registry.Migrations.Keys)
			{
				if (this._registry.Migrations[type].Count == 0)
				{
					this._latestVersions[type] = 1;
				}
				else
				{
					int num = this._registry.Migrations[type].Select<IMigration, int>((IMigration m) => m.ToVersion).Max();
					this._latestVersions[type] = num;
				}
			}
			Log.Info("Current save versions: " + string.Join("; ", this._latestVersions.Select<KeyValuePair<Type, int>, string>(delegate(KeyValuePair<Type, int> p)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(p.Key.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" v");
				defaultInterpolatedStringHandler.AppendFormatted<int>(p.Value);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			})), 2);
			this.EnsureVersionSet<SerializableRun>();
			this.EnsureVersionSet<SerializableProgress>();
			this.EnsureVersionSet<SettingsSave>();
			this.EnsureVersionSet<RunHistory>();
			this.EnsureVersionSet<PrefsSave>();
			this.EnsureVersionSet<ProfileSave>();
		}

		/// <summary>
		/// Ensures that a version is set for the specified save type.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		// Token: 0x06000A68 RID: 2664 RVA: 0x000465A4 File Offset: 0x000447A4
		[NullableContext(0)]
		private void EnsureVersionSet<T>() where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			this._latestVersions.TryAdd(typeFromHandle, 1);
		}

		/// <summary>
		/// Validates migration paths for gaps and duplicates.
		/// </summary>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MigrationPathGapException">Thrown when migration paths have gaps</exception>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.DuplicateMigrationException">Thrown when migration paths have duplicates</exception>
		// Token: 0x06000A69 RID: 2665 RVA: 0x000465CC File Offset: 0x000447CC
		private void ValidateMigrationPaths()
		{
			foreach (Type type in this._registry.Migrations.Keys)
			{
				this.IsMigrationPathValid(type);
			}
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0004662C File Offset: 0x0004482C
		[NullableContext(2)]
		public int GetLatestVersion<T>()
		{
			Type typeFromHandle = typeof(T);
			int num;
			if (!this._latestVersions.TryGetValue(typeFromHandle, out num))
			{
				Log.Warn("No version found for " + typeFromHandle.Name + ", defaulting to 1", 2);
				return 1;
			}
			return num;
		}

		/// <summary>
		/// Gets a list of version numbers that represent gaps in the migration path.
		/// A gap is defined as a version that is a target in one migration but not a source in any migration,
		/// except for the highest version which is expected to be only a target.
		/// </summary>
		/// <param name="saveType">The save type to check</param>
		/// <returns>A list of version numbers that represent gaps</returns>
		// Token: 0x06000A6B RID: 2667 RVA: 0x00046674 File Offset: 0x00044874
		private List<int> GetGapsInMigrationPath(Type saveType)
		{
			List<int> list = new List<int>();
			List<IMigration> list2;
			if (!this._registry.Migrations.TryGetValue(saveType, out list2))
			{
				return list;
			}
			HashSet<int> hashSet = new HashSet<int>(list2.Select<IMigration, int>((IMigration m) => m.FromVersion));
			HashSet<int> hashSet2 = new HashSet<int>(list2.Select<IMigration, int>((IMigration m) => m.ToVersion));
			int num;
			bool flag = this._latestVersions.TryGetValue(saveType, out num);
			foreach (int num2 in hashSet2)
			{
				if ((!flag || num2 != num) && !hashSet.Contains(num2))
				{
					list.Add(num2);
				}
			}
			return list;
		}

		/// <summary>
		/// Gets a list of version numbers that are sources for multiple migrations.
		/// This indicates a conflict in the migration path.
		/// </summary>
		/// <param name="saveType">The save type to check</param>
		/// <returns>A list of version numbers that are sources for multiple migrations</returns>
		// Token: 0x06000A6C RID: 2668 RVA: 0x00046760 File Offset: 0x00044960
		private List<int> GetDuplicateMigrationSources(Type saveType)
		{
			List<IMigration> list;
			if (!this._registry.Migrations.TryGetValue(saveType, out list))
			{
				return new List<int>();
			}
			return (from m in list
				group m by m.FromVersion into g
				where g.Count<IMigration>() > 1
				select g.Key).ToList<int>();
		}

		/// <summary>
		/// Checks if the migration path for a specific save type is valid.
		/// Throws the same exceptions as ValidateMigrationPaths for consistency.
		/// </summary>
		/// <param name="saveType">The save type to check</param>
		/// <returns>True if the migration path is valid</returns>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MigrationPathGapException">Thrown when migration paths have gaps</exception>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.DuplicateMigrationException">Thrown when migration paths have duplicates</exception>
		// Token: 0x06000A6D RID: 2669 RVA: 0x000467FC File Offset: 0x000449FC
		private void IsMigrationPathValid(Type saveType)
		{
			List<int> duplicateMigrationSources = this.GetDuplicateMigrationSources(saveType);
			if (duplicateMigrationSources.Count > 0)
			{
				int num = duplicateMigrationSources[0];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Multiple migrations from version ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" for ");
				defaultInterpolatedStringHandler.AppendFormatted(saveType.Name);
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				throw new DuplicateMigrationException(text);
			}
			List<int> gapsInMigrationPath = this.GetGapsInMigrationPath(saveType);
			if (gapsInMigrationPath.Count > 0)
			{
				int num2 = gapsInMigrationPath[0];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Gap in migration path for ");
				defaultInterpolatedStringHandler.AppendFormatted(saveType.Name);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendLiteral("version ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(" is a target but not a source in any migration");
				string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
				throw new MigrationPathGapException(text2);
			}
		}

		/// <summary>
		/// Registers a migration.
		/// </summary>
		/// <param name="migration">The migration to register</param>
		// Token: 0x06000A6E RID: 2670 RVA: 0x000468E4 File Offset: 0x00044AE4
		public void RegisterMigration(IMigration migration)
		{
			Type saveType = migration.SaveType;
			List<IMigration> list;
			if (!this._registry.Migrations.TryGetValue(saveType, out list))
			{
				list = new List<IMigration>();
				this._registry.Migrations[saveType] = list;
			}
			list.Add(migration);
		}

		/// <summary>
		/// Sets the minimum supported schema version for a specific save type.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <param name="version">The minimum supported version</param>
		// Token: 0x06000A6F RID: 2671 RVA: 0x0004692C File Offset: 0x00044B2C
		[NullableContext(0)]
		private void SetMinimumSupportedVersion<T>(int version) where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			this._minimumSupportedVersions[typeFromHandle] = version;
		}

		/// <summary>
		/// Gets the current schema version for a specific save type.
		/// </summary>
		/// <typeparam name="T">The save type that implements the ISaveSchema interface</typeparam>
		/// <returns>The current version number for the specified save type</returns>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MigrationException">Thrown when no migrations are found for the specified save type</exception>
		// Token: 0x06000A70 RID: 2672 RVA: 0x00046954 File Offset: 0x00044B54
		[NullableContext(0)]
		private int GetCurrentVersion<T>() where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			int num;
			if (!this._latestVersions.TryGetValue(typeFromHandle, out num))
			{
				throw new MigrationException("No migrations found for " + typeFromHandle.Name);
			}
			return num;
		}

		/// <summary>
		/// Gets the minimum supported schema version for a specific save type.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <returns>The minimum supported version</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown when no minimum supported version has been set</exception>
		// Token: 0x06000A71 RID: 2673 RVA: 0x00046994 File Offset: 0x00044B94
		[NullableContext(0)]
		private int GetMinimumSupportedVersion<T>() where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			int num;
			if (!this._minimumSupportedVersions.TryGetValue(typeFromHandle, out num))
			{
				throw new InvalidOperationException("No minimum supported version found for " + typeFromHandle.Name + ". Each save schema type should have a minimum supported version set and a base migration implemented.");
			}
			return num;
		}

		/// <summary>
		/// Gets a migration for a specific save type and version.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <param name="fromVersion">The version to migrate from</param>
		/// <param name="toVersion">The version to migrate to (used only for validation)</param>
		/// <returns>The migration, or null if no migration exists</returns>
		// Token: 0x06000A72 RID: 2674 RVA: 0x000469D8 File Offset: 0x00044BD8
		[NullableContext(0)]
		[return: Nullable(2)]
		private IMigration GetMigration<T>(int fromVersion, int toVersion) where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			List<IMigration> list;
			if (!this._registry.Migrations.TryGetValue(typeFromHandle, out list))
			{
				return null;
			}
			return list.FirstOrDefault<IMigration>((IMigration m) => m.FromVersion == fromVersion && m.ToVersion == toVersion);
		}

		/// <summary>
		/// Gets the next available version in the migration chain.
		/// </summary>
		/// <typeparam name="T">The save type that implements the ISaveSchema interface</typeparam>
		/// <param name="currentVersion">The current version number to find the next version for</param>
		/// <returns>The next version number if a migration exists, or null if no migration is available</returns>
		// Token: 0x06000A73 RID: 2675 RVA: 0x00046A30 File Offset: 0x00044C30
		[NullableContext(0)]
		private int? GetNextVersion<T>(int currentVersion) where T : ISaveSchema
		{
			Type typeFromHandle = typeof(T);
			List<IMigration> list;
			if (!this._registry.Migrations.TryGetValue(typeFromHandle, out list))
			{
				return null;
			}
			int versionToFind = currentVersion;
			IMigration migration = list.FirstOrDefault<IMigration>((IMigration m) => m.FromVersion == versionToFind);
			if (migration == null)
			{
				return null;
			}
			return new int?(migration.ToVersion);
		}

		/// <summary>
		/// Gets all registered save types that have migrations.
		/// </summary>
		/// <returns>The save types</returns>
		// Token: 0x06000A74 RID: 2676 RVA: 0x00046AA1 File Offset: 0x00044CA1
		public IEnumerable<Type> GetRegisteredSaveTypes()
		{
			return this._registry.Migrations.Keys;
		}

		/// <summary>
		/// Gets all registered migrations for a specific save type.
		/// </summary>
		/// <param name="saveType">The save type</param>
		/// <returns>The migrations</returns>
		// Token: 0x06000A75 RID: 2677 RVA: 0x00046AB4 File Offset: 0x00044CB4
		public IEnumerable<IMigration> GetMigrationsForType(Type saveType)
		{
			List<IMigration> list;
			if (!this._registry.Migrations.TryGetValue(saveType, out list))
			{
				return Array.Empty<IMigration>();
			}
			return list;
		}

		/// <summary>
		/// Extracts the schema version from a JsonObject.
		/// </summary>
		/// <param name="json">The JsonObject to extract the version from</param>
		/// <returns>The schema version</returns>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MissingSchemaVersionException">Thrown when the schema version is not found</exception>
		// Token: 0x06000A76 RID: 2678 RVA: 0x00046AE0 File Offset: 0x00044CE0
		private static int ExtractSchemaVersion(MigratingData json)
		{
			if (json.Has("schema_version"))
			{
				return json.GetInt("schema_version");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Schema version not found in JSON: ");
			defaultInterpolatedStringHandler.AppendFormatted<MigratingData>(json);
			throw new MissingSchemaVersionException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Creates a new save instance with the latest schema version for the type.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <returns>New save instance with current schema version</returns>
		// Token: 0x06000A77 RID: 2679 RVA: 0x00046B30 File Offset: 0x00044D30
		public T CreateNewSave<[Nullable(0)] T>() where T : ISaveSchema, new()
		{
			T t = new T();
			ref T ptr = ref t;
			if (default(T) == null)
			{
				T t2 = t;
				ptr = ref t2;
			}
			ptr.SchemaVersion = this.GetLatestVersion<T>();
			return t;
		}

		/// <summary>
		/// Preserves a corrupted save file using standardized naming convention.
		/// </summary>
		/// <param name="savePath">The path to the corrupt save file</param>
		/// <param name="status">The reason for corruption</param>
		// Token: 0x06000A78 RID: 2680 RVA: 0x00046B70 File Offset: 0x00044D70
		private void PreserveCorruptFile(string savePath, ReadSaveStatus status)
		{
			try
			{
				if (savePath.EndsWith(".corrupt"))
				{
					Log.Warn("File '" + savePath + "' is already marked as corrupt, skipping rename", 2);
				}
				else
				{
					string text = CorruptFileHandler.GenerateCorruptFilePath(savePath, status);
					this._saveStore.RenameFile(savePath, text);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Corrupt save detected (");
					defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(status);
					defaultInterpolatedStringHandler.AppendLiteral("): Renamed '");
					defaultInterpolatedStringHandler.AppendFormatted(savePath);
					defaultInterpolatedStringHandler.AppendLiteral("' to '");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral("'");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to preserve corrupt save file '" + savePath + "': " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Determines if a corrupt file should be preserved based on the status.
		/// </summary>
		/// <param name="status">The read save status</param>
		/// <returns>True if the file should be preserved as corrupt</returns>
		// Token: 0x06000A79 RID: 2681 RVA: 0x00046C4C File Offset: 0x00044E4C
		private static bool ShouldPreserveCorrupt(ReadSaveStatus status)
		{
			return status != ReadSaveStatus.FileNotFound && status != ReadSaveStatus.Success && status != ReadSaveStatus.MigrationRequired;
		}

		/// <summary>
		/// Loads save data with aggressive recovery attempts, falling back to a .backup if the primary file
		/// is missing or corrupt. This covers the case where a non-atomic rename on Windows (delete-then-move)
		/// lost the primary file during a power failure.
		/// Does NOT create new saves or modify files - that's the caller's responsibility.
		/// All recovery operations happen in memory only, preserving original files for debugging.
		/// </summary>
		/// <typeparam name="T">The type of save data</typeparam>
		/// <param name="filePath">The path to the save file</param>
		/// <returns>ReadSaveResult with save data or error status</returns>
		// Token: 0x06000A7A RID: 2682 RVA: 0x00046C60 File Offset: 0x00044E60
		public ReadSaveResult<T> LoadSave<[Nullable(0), DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string filePath) where T : ISaveSchema, new()
		{
			ReadSaveResult<T> readSaveResult = this.LoadSaveFromPath<T>(filePath);
			if (readSaveResult.Success)
			{
				return readSaveResult;
			}
			string text = filePath + ".backup";
			if (this._saveStore.FileExists(text))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Primary save failed (");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral("), attempting .backup fallback: ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				ReadSaveResult<T> readSaveResult2 = this.LoadSaveFromPath<T>(text);
				if (readSaveResult2.Success)
				{
					return readSaveResult2;
				}
			}
			return readSaveResult;
		}

		/// <summary>
		/// Loads save data from a single file path with aggressive recovery attempts.
		/// </summary>
		// Token: 0x06000A7B RID: 2683 RVA: 0x00046CF0 File Offset: 0x00044EF0
		private ReadSaveResult<T> LoadSaveFromPath<[Nullable(0), DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string filePath) where T : ISaveSchema, new()
		{
			if (!this._saveStore.FileExists(filePath))
			{
				return new ReadSaveResult<T>(ReadSaveStatus.FileNotFound, null);
			}
			ReadSaveResult<T> readSaveResult;
			try
			{
				string text = this._saveStore.ReadFile(filePath);
				if (string.IsNullOrWhiteSpace(text))
				{
					Log.Warn("Empty save file found at " + filePath, 2);
					this.PreserveCorruptFile(filePath, ReadSaveStatus.FileEmpty);
					readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.FileEmpty, null);
				}
				else
				{
					readSaveResult = this.LoadWithAggressiveRecovery<T>(filePath, text);
				}
			}
			catch (Exception ex)
			{
				Log.Error("File access error loading " + filePath + ": " + ex.Message, 2);
				this.PreserveCorruptFile(filePath, ReadSaveStatus.FileAccessError);
				readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.FileAccessError, ex.Message);
			}
			return readSaveResult;
		}

		/// <summary>
		/// Attempts to load save data with aggressive recovery attempts, including migration and repair.
		/// Uses multiple fallback strategies: schema version inference, sequential migrations, JSON repair, and data scavenging.
		/// </summary>
		/// <typeparam name="T">The type of save data</typeparam>
		/// <param name="filePath">The path to the save file (for logging)</param>
		/// <param name="content">The file content to parse</param>
		/// <returns>ReadSaveResult with save data or error status</returns>
		// Token: 0x06000A7C RID: 2684 RVA: 0x00046DA0 File Offset: 0x00044FA0
		private ReadSaveResult<T> LoadWithAggressiveRecovery<[Nullable(0), DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string filePath, string content) where T : ISaveSchema, new()
		{
			ReadSaveResult<T> readSaveResult;
			try
			{
				using (JsonDocument jsonDocument = JsonDocument.Parse(content, default(JsonDocumentOptions)))
				{
					MigratingData migratingData = new MigratingData(jsonDocument);
					bool flag = false;
					int num;
					try
					{
						num = MigrationManager.ExtractSchemaVersion(migratingData);
					}
					catch (MissingSchemaVersionException)
					{
						Log.Warn("Missing schema version in " + filePath + ", attempting to infer...", 2);
						flag = true;
						num = this.InferSchemaVersionFromStructure<T>(migratingData).GetValueOrDefault();
						migratingData.Set<int>("schema_version", num);
					}
					int currentVersion = this.GetCurrentVersion<T>();
					int minimumSupportedVersion = this.GetMinimumSupportedVersion<T>();
					if (num > currentVersion)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Save version ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler.AppendLiteral(" is newer than current ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(currentVersion);
						defaultInterpolatedStringHandler.AppendLiteral(", attempting recovery...");
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						T t = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
						if (t != null)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Successfully recovered data from future save version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							T t2 = t;
							ReadSaveStatus readSaveStatus = ReadSaveStatus.RecoveredWithDataLoss;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Data recovered from future version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" but newer fields were discarded");
							readSaveResult = new ReadSaveResult<T>(t2, readSaveStatus, defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Save file version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" is newer than current version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(currentVersion);
							string text = defaultInterpolatedStringHandler.ToStringAndClear();
							Log.Error(text + ": " + filePath, 2);
							this.PreserveCorruptFile(filePath, ReadSaveStatus.FutureVersion);
							readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.FutureVersion, text);
						}
					}
					else if (num < minimumSupportedVersion)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Save version ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler.AppendLiteral(" is below minimum ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(minimumSupportedVersion);
						defaultInterpolatedStringHandler.AppendLiteral(", attempting data scavenging...");
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						T t3 = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
						if (t3 != null)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Successfully scavenged data from old save version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" (recovery data not persisted)");
							Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							T t4 = t3;
							ReadSaveStatus readSaveStatus2 = ReadSaveStatus.RecoveredWithDataLoss;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Data recovered from version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" but some information may be lost");
							readSaveResult = new ReadSaveResult<T>(t4, readSaveStatus2, defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Save file version ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" is too old and couldn't be scavenged");
							string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
							Log.Error(text2 + ": " + filePath, 2);
							this.PreserveCorruptFile(filePath, ReadSaveStatus.VersionTooOld);
							readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.VersionTooOld, text2);
						}
					}
					else
					{
						if (num < currentVersion)
						{
							try
							{
								MigratingData migratingData2 = this.MigrateDataSequentially<T>(migratingData);
								T t5 = migratingData2.ToObject<T>();
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Successfully migrated ");
								defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
								defaultInterpolatedStringHandler.AppendLiteral(" from v");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num);
								defaultInterpolatedStringHandler.AppendLiteral(" to v");
								defaultInterpolatedStringHandler.AppendFormatted<int>(t5.SchemaVersion);
								defaultInterpolatedStringHandler.AppendLiteral(" (migration not persisted)");
								Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
								T t6 = t5;
								ReadSaveStatus readSaveStatus3 = ReadSaveStatus.MigrationRequired;
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Save was migrated from version ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num);
								defaultInterpolatedStringHandler.AppendLiteral(" to ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(t5.SchemaVersion);
								return new ReadSaveResult<T>(t6, readSaveStatus3, defaultInterpolatedStringHandler.ToStringAndClear());
							}
							catch (Exception ex)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Migration failed for ");
								defaultInterpolatedStringHandler.AppendFormatted(filePath);
								defaultInterpolatedStringHandler.AppendLiteral(" with exception: ");
								defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
								T t7 = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
								if (t7 != null)
								{
									Log.Info("Migration failed but data scavenging succeeded", 2);
									T t8 = t7;
									ReadSaveStatus readSaveStatus4 = ReadSaveStatus.RecoveredWithDataLoss;
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
									defaultInterpolatedStringHandler.AppendLiteral("Migration failed, recovered partial data from version ");
									defaultInterpolatedStringHandler.AppendFormatted<int>(num);
									return new ReadSaveResult<T>(t8, readSaveStatus4, defaultInterpolatedStringHandler.ToStringAndClear());
								}
								this.PreserveCorruptFile(filePath, ReadSaveStatus.MigrationFailed);
								return new ReadSaveResult<T>(ReadSaveStatus.MigrationFailed, ex.Message);
							}
						}
						ReadSaveResult<T> readSaveResult2 = JsonSerializationUtility.FromJson<T>(content);
						if (!readSaveResult2.Success || readSaveResult2.SaveData == null)
						{
							Log.Error("Failed to deserialize " + filePath + ": " + readSaveResult2.ErrorMessage, 2);
							T t9 = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
							if (t9 != null)
							{
								Log.Info("Deserialization failed but data scavenging succeeded", 2);
								readSaveResult = new ReadSaveResult<T>(t9, ReadSaveStatus.RecoveredWithDataLoss, "Save file was corrupt but partial data was recovered");
							}
							else if (flag)
							{
								this.PreserveCorruptFile(filePath, ReadSaveStatus.MissingSchemaVersion);
								readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.MissingSchemaVersion, "Save file is missing schema version and cannot be deserialized");
							}
							else
							{
								if (MigrationManager.ShouldPreserveCorrupt(readSaveResult2.Status))
								{
									this.PreserveCorruptFile(filePath, readSaveResult2.Status);
								}
								readSaveResult = new ReadSaveResult<T>(readSaveResult2.Status, readSaveResult2.ErrorMessage);
							}
						}
						else
						{
							readSaveResult = readSaveResult2;
						}
					}
				}
			}
			catch (JsonException ex2)
			{
				string text3 = ex2.Path ?? "unknown";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 4);
				defaultInterpolatedStringHandler.AppendLiteral("JSON parse error in ");
				defaultInterpolatedStringHandler.AppendFormatted(filePath);
				defaultInterpolatedStringHandler.AppendLiteral(" at path=");
				defaultInterpolatedStringHandler.AppendFormatted(text3);
				defaultInterpolatedStringHandler.AppendLiteral(", line=");
				defaultInterpolatedStringHandler.AppendFormatted<long?>(ex2.LineNumber);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(ex2.Message);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				string text4 = this.RepairCommonJsonErrors(content);
				if (text4 != null)
				{
					Log.Info("JSON repair succeeded, retrying load...", 2);
					ReadSaveResult<T> readSaveResult3 = this.LoadWithAggressiveRecovery<T>(filePath, text4);
					if (readSaveResult3.Success)
					{
						this._saveStore.WriteFile(filePath + ".pre-repair", content);
						readSaveResult = new ReadSaveResult<T>(readSaveResult3.SaveData, ReadSaveStatus.JsonRepaired, "Save file had JSON errors that were automatically repaired");
					}
					else
					{
						readSaveResult = readSaveResult3;
					}
				}
				else
				{
					this.PreserveCorruptFile(filePath, ReadSaveStatus.JsonParseError);
					ReadSaveStatus readSaveStatus5 = ReadSaveStatus.JsonParseError;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
					defaultInterpolatedStringHandler.AppendLiteral("JSON error at ");
					defaultInterpolatedStringHandler.AppendFormatted(text3);
					defaultInterpolatedStringHandler.AppendLiteral(" (line ");
					defaultInterpolatedStringHandler.AppendFormatted<long?>(ex2.LineNumber);
					defaultInterpolatedStringHandler.AppendLiteral("): ");
					defaultInterpolatedStringHandler.AppendFormatted(ex2.Message);
					readSaveResult = new ReadSaveResult<T>(readSaveStatus5, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			catch (Exception ex3)
			{
				Log.Error("Unexpected error loading " + filePath + ": " + ex3.Message, 2);
				this.PreserveCorruptFile(filePath, ReadSaveStatus.Unrecoverable);
				readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.Unrecoverable, ex3.Message);
			}
			return readSaveResult;
		}

		/// <summary>
		/// Attempts to infer schema version from save structure
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <param name="data">The save data to examine</param>
		/// <returns>Inferred version or null if can't infer</returns>
		// Token: 0x06000A7D RID: 2685 RVA: 0x0004750C File Offset: 0x0004570C
		private int? InferSchemaVersionFromStructure<[Nullable(0)] T>(MigratingData data) where T : ISaveSchema
		{
			return null;
		}

		/// <summary>
		/// Attempts to scavenge usable data from corrupted/old saves by stamping the current schema version
		/// and deserializing directly. Unknown fields are silently skipped; missing fields get C# defaults.
		/// SerializableRun is excluded because defaulted combat state (0 HP, empty deck) causes gameplay bugs.
		/// </summary>
		/// <typeparam name="T">The save type</typeparam>
		/// <param name="data">The save data to scavenge from</param>
		/// <returns>Scavenged save object or null if nothing can be salvaged</returns>
		// Token: 0x06000A7E RID: 2686 RVA: 0x00047524 File Offset: 0x00045724
		[return: Nullable(2)]
		private T RecoverPartialDataFromCorruptSave<[Nullable(0)] T>(MigratingData data) where T : ISaveSchema, new()
		{
			T t;
			if (typeof(T) == typeof(SerializableRun))
			{
				t = default(T);
				return t;
			}
			int currentVersion = this.GetCurrentVersion<T>();
			try
			{
				data.Set<int>("schema_version", currentVersion);
				T t2 = data.ToObject<T>();
				t2.SchemaVersion = currentVersion;
				Log.Info("Data scavenging succeeded for " + typeof(T).Name, 2);
				t = t2;
			}
			catch (Exception ex)
			{
				Log.Warn("Data scavenging failed for " + typeof(T).Name + ": " + ex.Message, 2);
				t = default(T);
			}
			return t;
		}

		/// <summary>
		/// Attempts basic JSON repair (fix common issues like trailing commas, missing brackets).
		/// Limited to safe, conservative repairs only.
		/// </summary>
		/// <param name="json">The JSON content to repair</param>
		/// <returns>Repaired JSON string or null if repair failed</returns>
		// Token: 0x06000A7F RID: 2687 RVA: 0x000475EC File Offset: 0x000457EC
		[return: Nullable(2)]
		private string RepairCommonJsonErrors(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}
			string text2;
			try
			{
				string text = Regex.Replace(json, ",\\s*([}\\]])", "$1");
				if (!text.EndsWith("}") && !text.EndsWith("]"))
				{
					int num = text.Count<char>((char c) => c == '{') - text.Count<char>((char c) => c == '}');
					int num2 = text.Count<char>((char c) => c == '[') - text.Count<char>((char c) => c == ']');
					if (num == 1 && num2 == 0)
					{
						text += "}";
					}
					else if (num2 == 1 && num == 0)
					{
						text += "]";
					}
				}
				using (JsonDocument.Parse(text, default(JsonDocumentOptions)))
				{
					text2 = text;
				}
			}
			catch
			{
				text2 = null;
			}
			return text2;
		}

		/// <summary>
		/// Migrates save data using JsonObject.
		/// </summary>
		/// <typeparam name="T">The type of save data to migrate</typeparam>
		/// <param name="jsonObj">The JsonObject to migrate</param>
		/// <returns>The migrated JsonObject</returns>
		/// <exception cref="T:MegaCrit.Sts2.Core.Saves.Migrations.MigrationException">Thrown when a migration cannot be found or fails</exception>
		// Token: 0x06000A80 RID: 2688 RVA: 0x00047754 File Offset: 0x00045954
		private MigratingData MigrateDataSequentially<[Nullable(0)] T>(MigratingData jsonObj) where T : ISaveSchema
		{
			int i = MigrationManager.ExtractSchemaVersion(jsonObj);
			int currentVersion = this.GetCurrentVersion<T>();
			while (i < currentVersion)
			{
				int? nextVersion = this.GetNextVersion<T>(i);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (nextVersion == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Missing migration path for ");
					defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
					defaultInterpolatedStringHandler.AppendLiteral(" from v");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" to latest version");
					throw new MigrationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int value = nextVersion.Value;
				IMigration migration = this.GetMigration<T>(i, value);
				if (migration == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Missing migration implementation for ");
					defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
					defaultInterpolatedStringHandler.AppendLiteral(" from v");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" to v");
					defaultInterpolatedStringHandler.AppendFormatted<int>(value);
					throw new MigrationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Migrating ");
				defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
				defaultInterpolatedStringHandler.AppendLiteral(" from v");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral(" to v");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				try
				{
					jsonObj = migration.Migrate(jsonObj);
					i = value;
				}
				catch (Exception ex) when (!(ex is MigrationException))
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Error migrating ");
					defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
					defaultInterpolatedStringHandler.AppendLiteral(" from v");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" to v");
					defaultInterpolatedStringHandler.AppendFormatted<int>(value);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
					throw new MigrationException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
				}
			}
			return jsonObj;
		}

		// Token: 0x04000391 RID: 913
		private readonly Dictionary<Type, int> _latestVersions = new Dictionary<Type, int>();

		// Token: 0x04000392 RID: 914
		private readonly Dictionary<Type, int> _minimumSupportedVersions = new Dictionary<Type, int>();

		// Token: 0x04000393 RID: 915
		private readonly MigrationRegistry _registry = new MigrationRegistry();

		// Token: 0x04000394 RID: 916
		private readonly ISaveStore _saveStore;
	}
}
