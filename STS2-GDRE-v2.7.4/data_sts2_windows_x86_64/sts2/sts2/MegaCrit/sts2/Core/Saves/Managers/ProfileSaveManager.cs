using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Saves.Migrations;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(1)]
	[Nullable(0)]
	public class ProfileSaveManager
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000499E4 File Offset: 0x00047BE4
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x000499EC File Offset: 0x00047BEC
		public ProfileSave Profile { get; private set; }

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000499F5 File Offset: 0x00047BF5
		public ProfileSaveManager(ISaveStore saveStore, MigrationManager migrationManager)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00049A0B File Offset: 0x00047C0B
		public static string GetProfileSavePath(bool? forceModState = null)
		{
			return StringExtensions.PathJoin(UserDataPathProvider.GetAccountDir(forceModState), "profile.save");
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00049A20 File Offset: 0x00047C20
		public void SaveProfile()
		{
			this.Profile.SchemaVersion = this._migrationManager.GetLatestVersion<ProfileSave>();
			string text = JsonSerializationUtility.ToJson<ProfileSave>(this.Profile);
			this._saveStore.WriteFile(ProfileSaveManager.GetProfileSavePath(null), text);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00049A6C File Offset: 0x00047C6C
		public ReadSaveResult<ProfileSave> LoadProfile()
		{
			ReadSaveResult<ProfileSave> readSaveResult = this._migrationManager.LoadSave<ProfileSave>(ProfileSaveManager.GetProfileSavePath(null));
			if (!readSaveResult.Success || readSaveResult.SaveData == null)
			{
				this.Profile = this._migrationManager.CreateNewSave<ProfileSave>();
			}
			else
			{
				this.Profile = readSaveResult.SaveData;
			}
			return readSaveResult;
		}

		// Token: 0x040003BC RID: 956
		public const int maxProfileCount = 3;

		// Token: 0x040003BD RID: 957
		public const string profileSaveFileName = "profile.save";

		// Token: 0x040003BE RID: 958
		private readonly ISaveStore _saveStore;

		// Token: 0x040003BF RID: 959
		private readonly MigrationManager _migrationManager;
	}
}
