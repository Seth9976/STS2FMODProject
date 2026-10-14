using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves.Migrations;
using Steamworks;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000FD RID: 253
	[NullableContext(1)]
	[Nullable(0)]
	public class SettingsSaveManager
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0004BDC5 File Offset: 0x00049FC5
		private string SettingsPath
		{
			get
			{
				return "settings.save";
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0004BDCC File Offset: 0x00049FCC
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0004BDD4 File Offset: 0x00049FD4
		public SettingsSave Settings { get; set; }

		// Token: 0x06000B0F RID: 2831 RVA: 0x0004BDDD File Offset: 0x00049FDD
		public SettingsSaveManager(ISaveStore saveStore, MigrationManager migrationManager)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0004BDF4 File Offset: 0x00049FF4
		public void SaveSettings()
		{
			this.Settings.SchemaVersion = this._migrationManager.GetLatestVersion<SettingsSave>();
			string text = JsonSerializationUtility.ToJson<SettingsSave>(this.Settings);
			this._saveStore.WriteFile(this.SettingsPath, text);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0004BE38 File Offset: 0x0004A038
		public ReadSaveResult<SettingsSave> LoadSettings()
		{
			ReadSaveResult<SettingsSave> readSaveResult = this._migrationManager.LoadSave<SettingsSave>(this.SettingsPath);
			if (!readSaveResult.Success || readSaveResult.SaveData == null)
			{
				this.Settings = this._migrationManager.CreateNewSave<SettingsSave>();
				this.ApplyPlatformDefaults(this.Settings);
				this.SaveSettings();
			}
			else
			{
				this.Settings = readSaveResult.SaveData;
			}
			return readSaveResult;
		}

		/// <summary>
		/// Applies platform-specific default settings when a settings file is recreated.
		/// </summary>
		/// <param name="settings">The settings object to apply defaults to</param>
		// Token: 0x06000B12 RID: 2834 RVA: 0x0004BE99 File Offset: 0x0004A099
		private void ApplyPlatformDefaults(SettingsSave settings)
		{
			if (SteamInitializer.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				settings.Fullscreen = true;
				settings.WindowPosition = new Vector2I(0, 0);
			}
		}

		// Token: 0x040003D4 RID: 980
		public const string settingsSaveFileName = "settings.save";

		// Token: 0x040003D5 RID: 981
		private readonly ISaveStore _saveStore;

		// Token: 0x040003D6 RID: 982
		private readonly MigrationManager _migrationManager;
	}
}
