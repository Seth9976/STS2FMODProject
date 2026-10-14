using System;
using System.IO;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// TEMPORARY: This class handles the one-time migration of user data from the legacy unscoped
	/// directory structure to the new user-scoped directory structure introduced for playtesters.
	///
	/// This migrator is designed to help existing playtesters transition their save data, replays,
	/// and console history to the new platform-specific, user-scoped directory structure without
	/// losing their progress.
	///
	/// This class should be removed a few months after the user-scoped directory feature has been
	/// released to playtesters, once we're confident that all active users have migrated their data.
	///
	/// The migration moves data from:
	///   - user://saves/       -&gt; user://{platform}/{userId}/profile1/saves/
	///   - user://replays/     -&gt; user://{platform}/{userId}/profile1/replays/
	///   - user://console_history.log -&gt; user://{platform}/{userId}/profile1/console_history.log
	///
	/// Note: Logs are intentionally NOT migrated as they remain in the legacy location (user://logs/)
	/// since Godot controls the log file creation.
	/// </summary>
	// Token: 0x0200008B RID: 139
	public static class AccountScopeUserDataMigrator
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00015D1C File Offset: 0x00013F1C
		public static void MigrateToUserScopedDirectories()
		{
			if (!AccountScopeUserDataMigrator.HasLegacyData())
			{
				Log.VeryDebug("No legacy unscoped data found, skipping migration", 2);
				return;
			}
			Log.Info("Starting migration of legacy unscoped data to user-scoped directories", 2);
			string text = ProjectSettings.GlobalizePath("user://");
			string text2 = ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1, null, null));
			Directory.CreateDirectory(text2);
			bool flag = MigrationUtil.MigrateDirectory("saves", text, text2);
			bool flag2 = MigrationUtil.MigrateDirectory("replays", text, text2);
			bool flag3 = MigrationUtil.MigrateFile("console_history.log", text, text2);
			AccountScopeUserDataMigrator._migrationPerformed = flag || flag2 || flag3;
			if (AccountScopeUserDataMigrator._migrationPerformed)
			{
				Log.Info("Migration to user-scoped directories completed successfully", 2);
				return;
			}
			Log.Info("No items were migrated (all destinations already existed)", 2);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00015DCC File Offset: 0x00013FCC
		private static bool HasLegacyData()
		{
			string text = ProjectSettings.GlobalizePath("user://");
			return Directory.Exists(Path.Combine(text, "saves")) || Directory.Exists(Path.Combine(text, "replays")) || File.Exists(Path.Combine(text, "console_history.log"));
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00015E1C File Offset: 0x0001401C
		public static void ArchiveLegacyData()
		{
			if (!AccountScopeUserDataMigrator._migrationPerformed)
			{
				Log.VeryDebug("No migration was performed, skipping archival of legacy data", 2);
				return;
			}
			if (!AccountScopeUserDataMigrator.HasLegacyData())
			{
				return;
			}
			Log.Info("Archiving legacy unscoped data after successful migration", 2);
			string text = ProjectSettings.GlobalizePath("user://");
			string text2 = Path.Combine(text, "legacy_backup");
			if (Directory.Exists(text2))
			{
				Log.Warn("Deleting legacy data archive that already exists", 2);
				Directory.Delete(text2, true);
			}
			Directory.CreateDirectory(text2);
			MigrationUtil.ArchiveLegacyDirectory(Path.Combine(text, "saves"), text2);
			MigrationUtil.ArchiveLegacyDirectory(Path.Combine(text, "replays"), text2);
			MigrationUtil.ArchiveLegacyFile(Path.Combine(text, "console_history.log"), text2);
			Log.Info("Legacy data archived to 'legacy_backup' folder", 2);
		}

		// Token: 0x04000179 RID: 377
		private static bool _migrationPerformed;
	}
}
