using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Saves.Migrations;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(1)]
	[Nullable(0)]
	public class PrefsSaveManager
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000498CB File Offset: 0x00047ACB
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x000498D3 File Offset: 0x00047AD3
		public PrefsSave Prefs { get; set; }

		// Token: 0x06000AC2 RID: 2754 RVA: 0x000498DC File Offset: 0x00047ADC
		public PrefsSaveManager(int profileId, ISaveStore saveStore, MigrationManager migrationManager)
			: this(saveStore, migrationManager, new StaticProfileIdProvider(profileId))
		{
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000498EC File Offset: 0x00047AEC
		public PrefsSaveManager(ISaveStore saveStore, MigrationManager migrationManager, IProfileIdProvider profileIdProvider)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
			this._profileIdProvider = profileIdProvider;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00049909 File Offset: 0x00047B09
		public static string GetPrefsPath(int profileId, bool? forceModState = null)
		{
			return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "prefs.save");
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00049928 File Offset: 0x00047B28
		public void SavePrefs()
		{
			this.Prefs.SchemaVersion = this._migrationManager.GetLatestVersion<PrefsSave>();
			string text = JsonSerializationUtility.ToJson<PrefsSave>(this.Prefs);
			this._saveStore.WriteFile(PrefsSaveManager.GetPrefsPath(this._profileIdProvider.CurrentProfileId, null), text);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0004997C File Offset: 0x00047B7C
		public ReadSaveResult<PrefsSave> LoadPrefs()
		{
			ReadSaveResult<PrefsSave> readSaveResult = this._migrationManager.LoadSave<PrefsSave>(PrefsSaveManager.GetPrefsPath(this._profileIdProvider.CurrentProfileId, null));
			if (!readSaveResult.Success || readSaveResult.SaveData == null)
			{
				this.Prefs = this._migrationManager.CreateNewSave<PrefsSave>();
				this.SavePrefs();
			}
			else
			{
				this.Prefs = readSaveResult.SaveData;
			}
			return readSaveResult;
		}

		// Token: 0x040003B7 RID: 951
		public const string fileName = "prefs.save";

		// Token: 0x040003B8 RID: 952
		private readonly ISaveStore _saveStore;

		// Token: 0x040003B9 RID: 953
		private readonly MigrationManager _migrationManager;

		// Token: 0x040003BA RID: 954
		private readonly IProfileIdProvider _profileIdProvider;
	}
}
