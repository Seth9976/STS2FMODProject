using System;
using System.IO;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// TEMPORARY: This class handles the one-time migration of user data from before profiles existed.
	///
	/// This migrator is designed to help existing playtesters transition their save data, replays,
	/// and console history to the new platform-specific, user-scoped directory structure without
	/// losing their progress.
	///
	/// This class should be removed a few months after the user profile feature has been
	/// released to playtesters, once we're confident that all active users have migrated their data.
	///
	/// The migration moves all data from user://{platform}/{userId}/saves/ -&gt; user://{platform}/{userId}/profile1/saves/
	/// </summary>
	// Token: 0x020000A5 RID: 165
	public static class ProfileAccountScopeMigrator
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x0003C154 File Offset: 0x0003A354
		public static void MigrateToProfileScopedDirectories()
		{
			if (!ProfileAccountScopeMigrator.HasLegacyData())
			{
				Log.VeryDebug("No legacy unscoped data found, skipping migration", 2);
				return;
			}
			Log.Info("Starting migration of account-scoped data to profile-scoped directories", 2);
			string text = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath("", null, null));
			string text2 = ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1, null, null));
			Directory.CreateDirectory(text2);
			bool flag = MigrationUtil.MigrateFile("settings.save", text + "/saves", text);
			bool flag2 = MigrationUtil.MigrateDirectory("saves", text, text2);
			bool flag3 = MigrationUtil.MigrateDirectory("replays", text, text2);
			bool flag4 = MigrationUtil.MigrateFile("console_history.log", text, text2);
			ProfileAccountScopeMigrator._migrationPerformed = flag || flag2 || flag3 || flag4;
			if (ProfileAccountScopeMigrator._migrationPerformed)
			{
				Log.Info("Migration to profile-scoped directories completed successfully", 2);
				return;
			}
			Log.Info("No items were migrated (all destinations already existed)", 2);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0003C238 File Offset: 0x0003A438
		private static bool HasLegacyData()
		{
			return DirAccess.DirExistsAbsolute(UserDataPathProvider.GetAccountScopedBasePath("saves", null, null)) || DirAccess.DirExistsAbsolute(UserDataPathProvider.GetAccountScopedBasePath("replays", null, null));
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0003C28C File Offset: 0x0003A48C
		public static void ArchiveLegacyData()
		{
			if (!ProfileAccountScopeMigrator._migrationPerformed)
			{
				Log.VeryDebug("No migration was performed, skipping archival of legacy data", 2);
				return;
			}
			if (!ProfileAccountScopeMigrator.HasLegacyData())
			{
				return;
			}
			Log.Info("Archiving account-scoped data after successful profile-scoped migration", 2);
			string text = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath("", null, null));
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
			string text3 = ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1, null, null));
			File.Delete(Path.Combine(text3, "saves/settings.save"));
			Log.Info("Legacy data archived to 'legacy_backup' folder", 2);
		}

		// Token: 0x04000266 RID: 614
		private static bool _migrationPerformed;
	}
}
