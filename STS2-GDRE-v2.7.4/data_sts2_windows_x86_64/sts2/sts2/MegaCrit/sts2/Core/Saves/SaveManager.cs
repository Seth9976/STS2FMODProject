using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.Saves.Migrations;
using MegaCrit.Sts2.Core.Saves.Test;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using Steamworks;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Manages saving and loading game data, including settings, progress, current runs, and run history.
	/// Implements the Singleton pattern for global access to save functionality.
	/// </summary>
	/// <remarks>
	/// The SaveManager coordinates multiple specialized save managers:
	/// - <see cref="T:MegaCrit.Sts2.Core.Saves.Managers.SettingsSaveManager" />: Handles user settings (not cloud synced)
	/// - <see cref="T:MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager" />: Manages global game progress and statistics
	/// - <see cref="T:MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager" />: Handles saving/loading of active runs
	/// - <see cref="T:MegaCrit.Sts2.Core.Saves.Managers.RunHistorySaveManager" />: Manages historical run data
	///
	/// This specialized manager architecture improves testability by allowing each save manager
	/// to be tested independently with appropriate mocks and stubs.
	///
	/// Save files are stored in a user-scoped, platform-specific location:
	/// - In editor: C:\Users\{USER}\AppData\Roaming\SlayTheSpire2\{platform}\{userId}\saves
	/// - In builds: C:\Users\{USER}\AppData\Roaming\Godot\app_userdata\sts2\{platform}\{userId}\saves
	/// </remarks>
	// Token: 0x020000AA RID: 170
	[NullableContext(1)]
	[Nullable(0)]
	public class SaveManager : IProfileIdProvider
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0003EDC9 File Offset: 0x0003CFC9
		public static SaveManager Instance
		{
			get
			{
				if (SaveManager._mockInstance != null)
				{
					return SaveManager._mockInstance;
				}
				if (SaveManager._instance == null)
				{
					SaveManager._instance = SaveManager.ConstructDefault();
				}
				return SaveManager._instance;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0003EDEE File Offset: 0x0003CFEE
		public SettingsSave SettingsSave
		{
			get
			{
				return this._settingsSaveManager.Settings;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0003EDFB File Offset: 0x0003CFFB
		public PrefsSave PrefsSave
		{
			get
			{
				return this._prefsSaveManager.Prefs;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0003EE08 File Offset: 0x0003D008
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0003EE15 File Offset: 0x0003D015
		public ProgressState Progress
		{
			get
			{
				return this._progressSaveManager.Progress;
			}
			set
			{
				this._progressSaveManager.Progress = value;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0003EE23 File Offset: 0x0003D023
		public bool HasRunSave
		{
			get
			{
				return this._runSaveManager.HasRunSave;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0003EE30 File Offset: 0x0003D030
		public bool HasMultiplayerRunSave
		{
			get
			{
				return this._runSaveManager.HasMultiplayerRunSave;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0003EE3D File Offset: 0x0003D03D
		public bool IsProfileInitialized
		{
			get
			{
				return this._currentProfileId != null;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0003EE4C File Offset: 0x0003D04C
		public int CurrentProfileId
		{
			get
			{
				int? currentProfileId = this._currentProfileId;
				if (currentProfileId == null)
				{
					throw new InvalidOperationException("InitProfileId must be called on SaveManager!");
				}
				return currentProfileId.GetValueOrDefault();
			}
		}

		/// <summary>
		/// If the SaveManager is in the middle of saving the run, then this is non-null and not completed.
		/// </summary>
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0003EE7B File Offset: 0x0003D07B
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x0003EE83 File Offset: 0x0003D083
		[Nullable(2)]
		public Task CurrentRunSaveTask
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600079D RID: 1949 RVA: 0x0003EE8C File Offset: 0x0003D08C
		// (remove) Token: 0x0600079E RID: 1950 RVA: 0x0003EE9A File Offset: 0x0003D09A
		[Nullable(2)]
		public event Action Saved
		{
			[NullableContext(2)]
			add
			{
				this._runSaveManager.Saved += value;
			}
			[NullableContext(2)]
			remove
			{
				this._runSaveManager.Saved -= value;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600079F RID: 1951 RVA: 0x0003EEA8 File Offset: 0x0003D0A8
		// (remove) Token: 0x060007A0 RID: 1952 RVA: 0x0003EEE0 File Offset: 0x0003D0E0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int> ProfileIdChanged;

		// Token: 0x060007A1 RID: 1953 RVA: 0x0003EF15 File Offset: 0x0003D115
		public static void MockInstanceForTesting(SaveManager saveManager)
		{
			SaveManager._mockInstance = saveManager;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0003EF1D File Offset: 0x0003D11D
		public static void ClearInstanceForTesting()
		{
			SaveManager._mockInstance = null;
		}

		/// <summary>
		/// Constructor with dependency injection support.
		/// </summary>
		/// <param name="saveStore">The file I/O backend to use.</param>
		/// <param name="forceSynchronous">Force all operations to be performed synchronously. Only use in tests.</param>
		// Token: 0x060007A3 RID: 1955 RVA: 0x0003EF25 File Offset: 0x0003D125
		public SaveManager(ISaveStore saveStore, bool forceSynchronous = false)
			: this(saveStore, saveStore, forceSynchronous)
		{
		}

		/// <summary>
		/// Constructor with a separate local-only store for settings.
		/// Settings are machine-specific (display, controller, window) and must not be cloud-synced.
		/// </summary>
		// Token: 0x060007A4 RID: 1956 RVA: 0x0003EF30 File Offset: 0x0003D130
		public SaveManager(ISaveStore saveStore, ISaveStore localOnlyStore, bool forceSynchronous = false)
		{
			this._saveStore = saveStore;
			this._migrationManager = new MigrationManager(saveStore);
			this._settingsSaveManager = new SettingsSaveManager(localOnlyStore, this._migrationManager);
			this._profileSaveManager = new ProfileSaveManager(saveStore, this._migrationManager);
			this._prefsSaveManager = new PrefsSaveManager(saveStore, this._migrationManager, this);
			this._progressSaveManager = new ProgressSaveManager(saveStore, this._migrationManager, this);
			this._runSaveManager = new RunSaveManager(saveStore, this._migrationManager, this, forceSynchronous);
			this._runHistorySaveManager = new RunHistorySaveManager(saveStore, this._migrationManager, this);
		}

		/// <summary>
		/// Constructs the default SaveManager for this platform and configuration.
		///  - If we are currently in test mode, then this returns a SaveManager that doesn't actually save to disk.
		///  - If cloud saves are enabled, then this returns a SaveManager that saves both to cloud and to disk for the
		///    appropriate save backend.
		/// </summary>
		// Token: 0x060007A5 RID: 1957 RVA: 0x0003EFC8 File Offset: 0x0003D1C8
		private static SaveManager ConstructDefault()
		{
			ISaveStore saveStore;
			if (TestMode.IsOn)
			{
				saveStore = new MockGodotFileIo("user://test");
				return new SaveManager(saveStore, false);
			}
			ISaveStore saveStore2 = new GodotFileIo(UserDataPathProvider.GetAccountScopedBasePath(null, null, null));
			saveStore = saveStore2;
			if (SteamInitializer.Initialized)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Steam is enabled, we will write saves to steam storage. Enabled for account: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(SteamRemoteStorage.IsCloudEnabledForAccount());
				defaultInterpolatedStringHandler.AppendLiteral(", app: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(SteamRemoteStorage.IsCloudEnabledForApp());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SteamRemoteSaveStore steamRemoteSaveStore = new SteamRemoteSaveStore();
				CloudSaveStore cloudSaveStore = new CloudSaveStore(saveStore2, steamRemoteSaveStore);
				saveStore = cloudSaveStore;
			}
			return new SaveManager(saveStore, saveStore2, false);
		}

		/// <summary>
		/// Sets CurrentProfileId.
		/// This is called during the initialization of the game. It must be called after cloud sync has completed to allow
		/// the profile save to sync.
		/// </summary>
		/// <param name="profileId">The profile ID to use. If null, it will be read from file.</param>
		// Token: 0x060007A6 RID: 1958 RVA: 0x0003F088 File Offset: 0x0003D288
		public void InitProfileId(int? profileId = null)
		{
			this.CleanupTemporaryFiles();
			if (profileId == null)
			{
				this._profileSaveManager.LoadProfile();
				this._currentProfileId = new int?(this._profileSaveManager.Profile.LastProfileId);
			}
			else
			{
				this._currentProfileId = new int?(profileId.Value);
			}
			string profileScopedBasePath = UserDataPathProvider.GetProfileScopedBasePath(this.CurrentProfileId, null, null);
			Log.Info("Profile-scoped data path initialized: " + profileScopedBasePath, 2);
			this._runHistorySaveManager.CreateRunHistoryDirectory();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0003F119 File Offset: 0x0003D319
		[NullableContext(2)]
		public int GetLatestSchemaVersion<T>()
		{
			return this._migrationManager.GetLatestVersion<T>();
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0003F126 File Offset: 0x0003D326
		public string GetProfileScopedPath(string userData)
		{
			return this._saveStore.GetFullPath(Path.Combine(UserDataPathProvider.GetProfileDir(this.CurrentProfileId), userData));
		}

		/// <summary>
		/// Switches the current profile ID and saves it to the profile save file.
		/// Methods on the SaveManager will start reporting new data right after, but you will must call InitPrefsData and
		/// InitProgressData so that PrefsData and ProgressData will report old data.
		/// </summary>
		/// <param name="profileId">The profile ID to switch to.</param>
		// Token: 0x060007A9 RID: 1961 RVA: 0x0003F144 File Offset: 0x0003D344
		public void SwitchProfileId(int profileId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Switching save profiles to ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._currentProfileId = new int?(profileId);
			this._profileSaveManager.Profile.LastProfileId = profileId;
			this.SaveProfile();
			this._runHistorySaveManager.CreateRunHistoryDirectory();
			Action<int> profileIdChanged = this.ProfileIdChanged;
			if (profileIdChanged == null)
			{
				return;
			}
			profileIdChanged(profileId);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0003F1BC File Offset: 0x0003D3BC
		public SaveBatchScope BeginSaveBatch()
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				cloudSaveStore.BeginSaveBatch();
			}
			return new SaveBatchScope(this);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		public void EndSaveBatch()
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				cloudSaveStore.EndSaveBatch();
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003F208 File Offset: 0x0003D408
		public async Task SaveRun([Nullable(2)] AbstractRoom preFinishedRoom, bool saveProgress = true)
		{
			if (this.CurrentRunSaveTask != null)
			{
				await this.CurrentRunSaveTask;
			}
			using (this.BeginSaveBatch())
			{
				if (saveProgress)
				{
					this.SaveProgressFile();
				}
				try
				{
					this.CurrentRunSaveTask = this._runSaveManager.SaveRun(preFinishedRoom);
					await this.CurrentRunSaveTask;
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to save run: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					SentryService.CaptureException(ex);
				}
				finally
				{
					this.CurrentRunSaveTask = null;
				}
			}
			SaveBatchScope saveBatchScope = default(SaveBatchScope);
		}

		/// <summary>
		/// Increments and saves the NumReloads field of a save file.
		/// </summary>
		// Token: 0x060007AD RID: 1965 RVA: 0x0003F25C File Offset: 0x0003D45C
		public async Task IncrementNumReloads(SerializableRun save, NetGameType type, bool forceInTest = false)
		{
			int numReloads = save.NumReloads;
			save.NumReloads = numReloads + 1;
			bool flag = forceInTest || TestMode.IsOff;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = type - NetGameType.Singleplayer <= 1;
				flag2 = flag3;
			}
			if (flag2)
			{
				if (this.CurrentRunSaveTask != null)
				{
					await this.CurrentRunSaveTask;
				}
				using (this.BeginSaveBatch())
				{
					try
					{
						this.CurrentRunSaveTask = this._runSaveManager.SaveRun(save, type.IsMultiplayer());
						await this.CurrentRunSaveTask;
					}
					finally
					{
						this.CurrentRunSaveTask = null;
					}
				}
				SaveBatchScope saveBatchScope = default(SaveBatchScope);
			}
		}

		/// <summary>
		/// Called whenever the player wins, loses, or abandons run.
		/// Updates the progress.save file using the current_run.save file or similar.
		/// </summary>
		/// <param name="serializableRun">The serialized run data.</param>
		/// <param name="victory">Whether or not the run ended in a victory.</param>
		// Token: 0x060007AE RID: 1966 RVA: 0x0003F2B7 File Offset: 0x0003D4B7
		public void UpdateProgressWithRunData(SerializableRun serializableRun, bool victory)
		{
			this._progressSaveManager.UpdateWithRunData(serializableRun, victory);
		}

		/// <summary>
		/// Called when the player wins a combat.
		/// </summary>
		// Token: 0x060007AF RID: 1967 RVA: 0x0003F2C6 File Offset: 0x0003D4C6
		public void UpdateProgressAfterCombatWon(Player localPlayer, CombatRoom combatRoom)
		{
			this._progressSaveManager.UpdateAfterCombatWon(localPlayer, combatRoom);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0003F2D5 File Offset: 0x0003D4D5
		public void DeleteCurrentRun()
		{
			this._runSaveManager.DeleteCurrentRun();
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0003F2E2 File Offset: 0x0003D4E2
		public void DeleteCurrentMultiplayerRun()
		{
			this._runSaveManager.DeleteCurrentMultiplayerRun();
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0003F2F0 File Offset: 0x0003D4F0
		public void DeleteProfile(int profileId)
		{
			string profileScopedBasePath = UserDataPathProvider.GetProfileScopedBasePath(profileId, null, null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
			defaultInterpolatedStringHandler.AppendLiteral("DELETING the profile id ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
			defaultInterpolatedStringHandler.AppendLiteral(" at path ");
			defaultInterpolatedStringHandler.AppendFormatted(profileScopedBasePath);
			defaultInterpolatedStringHandler.AppendLiteral("!!");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this.DeleteDirectoryRecursive(UserDataPathProvider.GetProfileDir(profileId));
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0003F36D File Offset: 0x0003D56D
		public void DeleteDirectoryRecursive(string directory)
		{
			this.DeleteInDirectoryRecursive(directory);
			Log.Info("Deleting directory at " + directory, 2);
			this._saveStore.DeleteDirectory(directory);
		}

		/// <summary>
		/// Recursively deletes all files and subdirectories within the given directory.
		/// </summary>
		/// <remarks>
		/// PRG-5240: When using cloud storage, we must also delete cloud-only files.
		/// Cloud-only files can exist when:
		/// - Old run history files were "forgotten" from cloud quota tracking but still exist on Steam servers
		/// - Files were uploaded from another device but not yet synced locally
		///
		/// If we only deleted local files, cloud-only files would be restored on next game launch
		/// by SyncCloudToLocal, causing deleted profile data (like old run history) to reappear.
		/// </remarks>
		// Token: 0x060007B4 RID: 1972 RVA: 0x0003F394 File Offset: 0x0003D594
		private void DeleteInDirectoryRecursive(string directory)
		{
			foreach (string text in this._saveStore.GetDirectoriesInDirectory(directory))
			{
				string text2 = directory + "/" + text;
				this.DeleteInDirectoryRecursive(text2);
				Log.Info("Deleting directory at " + text2, 2);
				this._saveStore.DeleteDirectory(text2);
			}
			foreach (string text3 in this._saveStore.GetFilesInDirectory(directory))
			{
				string text4 = directory + "/" + text3;
				Log.Info("Deleting file at " + text4, 2);
				this._saveStore.DeleteFile(text4);
			}
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				foreach (string text5 in cloudSaveStore.CloudStore.GetFilesInDirectory(directory))
				{
					string text6 = directory + "/" + text5;
					Log.Info("Deleting cloud-only file at " + text6, 2);
					cloudSaveStore.CloudStore.DeleteFile(text6);
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0003F4B0 File Offset: 0x0003D6B0
		public void SaveSettings()
		{
			try
			{
				this._settingsSaveManager.SaveSettings();
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to save settings: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0003F510 File Offset: 0x0003D710
		public void SaveProfile()
		{
			try
			{
				this._profileSaveManager.SaveProfile();
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to save profile: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
		}

		/// <summary>
		/// Initializes a default settings file for testing purposes.
		/// This is separate from InitSettingsData because that is used directly in tests.
		/// </summary>
		// Token: 0x060007B7 RID: 1975 RVA: 0x0003F570 File Offset: 0x0003D770
		public ReadSaveResult<SettingsSave> InitSettingsDataForTest()
		{
			this._settingsSaveManager.Settings = new SettingsSave();
			return new ReadSaveResult<SettingsSave>(this._settingsSaveManager.Settings);
		}

		/// <summary>
		/// Initializes a default prefs file for testing purposes.
		/// This is separate from InitPrefsData because that is used directly in tests.
		/// </summary>
		// Token: 0x060007B8 RID: 1976 RVA: 0x0003F592 File Offset: 0x0003D792
		public ReadSaveResult<PrefsSave> InitPrefsDataForTest()
		{
			this._prefsSaveManager.Prefs = new PrefsSave();
			return new ReadSaveResult<PrefsSave>(this._prefsSaveManager.Prefs);
		}

		/// <summary>
		/// Loads the settings file for the first time. This should only be called once early on in the lifetime of the game.
		/// If the settings save could not be read because it was corrupt or did not exist, a new one is created.
		/// </summary>
		/// <returns>The result of reading the settings save file.</returns>
		// Token: 0x060007B9 RID: 1977 RVA: 0x0003F5B4 File Offset: 0x0003D7B4
		public ReadSaveResult<SettingsSave> InitSettingsData()
		{
			return this._settingsSaveManager.LoadSettings();
		}

		/// <summary>
		/// Loads the prefs file for the first time.
		/// This should be called once early on in the lifetime of the game, and then only when the player switches profiles
		/// after that. If the progress save could not be read because it was corrupt or did not exist, a new one is created.
		/// </summary>
		/// <returns>The result of reading the prefs save file.</returns>
		// Token: 0x060007BA RID: 1978 RVA: 0x0003F5C1 File Offset: 0x0003D7C1
		public ReadSaveResult<PrefsSave> InitPrefsData()
		{
			return this._prefsSaveManager.LoadPrefs();
		}

		/// <summary>
		/// Loads the progress file for the first time.
		/// This should be called once early on in the lifetime of the game, and then only when the player switches profiles
		/// after that. If the progress save could not be read because it was corrupt or did not exist, a new one is created.
		/// </summary>
		/// <returns>The result of reading the progress save file.</returns>
		// Token: 0x060007BB RID: 1979 RVA: 0x0003F5CE File Offset: 0x0003D7CE
		public ReadSaveResult<SerializableProgress> InitProgressData()
		{
			return this._progressSaveManager.LoadProgress();
		}

		/// <summary>
		/// Synchronizes files that are cloud-saved in the background.
		/// When this is called, we copy all newer files from the cloud save backend to the user:// directory, overwriting
		/// the saves that are there.
		/// If there is no active cloud save backend, then this method does nothing.
		/// </summary>
		// Token: 0x060007BC RID: 1980 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		public async Task SyncCloudToLocal()
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				Log.Info("Syncing cloud save files to the local save directory", 2);
				SaveManager.DeleteSettingsFromCloud(cloudSaveStore);
				List<Task> tasks = new List<Task>();
				foreach (Task task in this.EnumerateCloudSyncTasks(cloudSaveStore))
				{
					tasks.Add(task);
					if (tasks.Count >= 8)
					{
						await Task.WhenAll(tasks);
						tasks.Clear();
					}
				}
				IEnumerator<Task> enumerator = null;
				if (tasks.Count > 0)
				{
					await Task.WhenAll(tasks);
				}
				this.CleanupStaleCurrentRunSaves();
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0003F61F File Offset: 0x0003D81F
		private IEnumerable<Task> EnumerateCloudSyncTasks(CloudSaveStore cloudStore)
		{
			SaveManager.<EnumerateCloudSyncTasks>d__68 <EnumerateCloudSyncTasks>d__ = new SaveManager.<EnumerateCloudSyncTasks>d__68(-2);
			<EnumerateCloudSyncTasks>d__.<>3__cloudStore = cloudStore;
			return <EnumerateCloudSyncTasks>d__;
		}

		/// <summary>
		/// Deletes settings.save from cloud storage if it exists. Settings are machine-specific (display,
		/// controller, window) and were never intended to be cloud-synced. A stale cloud copy was uploaded
		/// by earlier versions and can cause sync conflicts when the local settings legitimately differ
		/// (e.g., different platform, different display configuration).
		/// </summary>
		// Token: 0x060007BE RID: 1982 RVA: 0x0003F630 File Offset: 0x0003D830
		private static void DeleteSettingsFromCloud(CloudSaveStore cloudStore)
		{
			try
			{
				if (cloudStore.CloudStore.FileExists("settings.save"))
				{
					Log.Info("Deleting stale settings.save from cloud storage", 2);
					cloudStore.CloudStore.DeleteFile("settings.save");
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to delete settings from cloud: " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Removes current_run.save files that have already been saved to run history.
		/// This prevents stale saves from being restored after cloud sync issues (e.g., Steam Deck suspend).
		/// </summary>
		// Token: 0x060007BF RID: 1983 RVA: 0x0003F698 File Offset: 0x0003D898
		private void CleanupStaleCurrentRunSaves()
		{
			for (int i = 1; i <= 3; i++)
			{
				this.CleanupStaleCurrentRunSaveForProfile(i, "current_run.save");
				this.CleanupStaleCurrentRunSaveForProfile(i, "current_run_mp.save");
			}
		}

		/// <summary>
		/// Checks if a current_run.save file for a specific profile is stale (already exists in history) and deletes it.
		/// </summary>
		// Token: 0x060007C0 RID: 1984 RVA: 0x0003F6CC File Offset: 0x0003D8CC
		private void CleanupStaleCurrentRunSaveForProfile(int profileId, string runSaveFileName)
		{
			string runSavePath = RunSaveManager.GetRunSavePath(profileId, runSaveFileName, null);
			string text = runSavePath + ".backup";
			string text2 = null;
			if (this._saveStore.FileExists(runSavePath))
			{
				text2 = runSavePath;
			}
			else if (this._saveStore.FileExists(text))
			{
				text2 = text;
			}
			if (text2 == null)
			{
				return;
			}
			try
			{
				string text3 = this._saveStore.ReadFile(text2);
				if (text3 == null)
				{
					Log.Warn("Could not read " + text2 + ", skipping staleness check", 2);
				}
				else
				{
					long? num = SaveManager.ExtractStartTimeFromRunSave(text3);
					if (num == null)
					{
						Log.Warn("Could not extract start_time from " + text2 + ", skipping staleness check", 2);
					}
					else
					{
						string historyPath = RunHistorySaveManager.GetHistoryPath(profileId, null);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
						defaultInterpolatedStringHandler.AppendFormatted<long?>(num);
						defaultInterpolatedStringHandler.AppendLiteral(".run");
						string text4 = Path.Combine(historyPath, defaultInterpolatedStringHandler.ToStringAndClear());
						if (this._saveStore.FileExists(text4))
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 4);
							defaultInterpolatedStringHandler.AppendLiteral("Deleting stale ");
							defaultInterpolatedStringHandler.AppendFormatted(runSaveFileName);
							defaultInterpolatedStringHandler.AppendLiteral(" for profile ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
							defaultInterpolatedStringHandler.AppendLiteral(": run with StartTime ");
							defaultInterpolatedStringHandler.AppendFormatted<long?>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" already exists in history at ");
							defaultInterpolatedStringHandler.AppendFormatted(text4);
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							this._saveStore.DeleteFile(runSavePath);
							this._saveStore.DeleteFile(runSavePath + ".backup");
						}
					}
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error checking for stale current_run.save in profile ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Removes orphaned .tmp files from all save directories.
		/// These can be left behind if the game crashes or is force-killed during an async write.
		/// </summary>
		// Token: 0x060007C1 RID: 1985 RVA: 0x0003F8B8 File Offset: 0x0003DAB8
		private void CleanupTemporaryFiles()
		{
			this._saveStore.DeleteTemporaryFiles("");
			for (int i = 1; i <= 3; i++)
			{
				string text = Path.Combine(UserDataPathProvider.GetProfileDir(i), UserDataPathProvider.SavesDir);
				this._saveStore.DeleteTemporaryFiles(text);
				this._saveStore.DeleteTemporaryFiles(RunHistorySaveManager.GetHistoryPath(i, null));
			}
		}

		/// <summary>
		/// Extracts the start_time field from a serialized run save JSON without full deserialization.
		/// Returns null if the field cannot be extracted.
		/// </summary>
		// Token: 0x060007C2 RID: 1986 RVA: 0x0003F918 File Offset: 0x0003DB18
		private static long? ExtractStartTimeFromRunSave(string json)
		{
			try
			{
				using (JsonDocument jsonDocument = JsonDocument.Parse(json, default(JsonDocumentOptions)))
				{
					JsonElement jsonElement;
					if (jsonDocument.RootElement.TryGetProperty("start_time", out jsonElement))
					{
						return new long?(jsonElement.GetInt64());
					}
				}
			}
			catch
			{
			}
			return null;
		}

		/// <summary>
		/// This returns true under the following circumstances:
		/// - If there is a cloud save backend with no files uploaded and we have save files locally
		/// - If the cloud save backend is present and has a local cache, but cloud sync is not enabled
		///
		/// If true is returned, then OverwriteCloudWithLocal should be called.
		/// If false is returned, then SyncCloudToLocal should be called.
		/// </summary>
		// Token: 0x060007C3 RID: 1987 RVA: 0x0003F998 File Offset: 0x0003DB98
		public bool ShouldOverwriteCloudWithLocal()
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			return cloudSaveStore != null && (!cloudSaveStore.HasUserEnabledCloudSync() || (this._saveStore.FileExists(ProfileSaveManager.GetProfileSavePath(null)) && !cloudSaveStore.HasCloudFiles()));
		}

		/// <summary>
		/// If any cloud backend is enabled, but there are no files uploaded to that backend and we have save files on our
		/// end, then this uploads all of our files to the cloud.
		/// If there is no active cloud save backend, then this method does nothing.
		/// This is used to upload all save files to the cloud for the first time when we enable a cloud save backend.
		/// </summary>
		// Token: 0x060007C4 RID: 1988 RVA: 0x0003F9E8 File Offset: 0x0003DBE8
		public async Task OverwriteCloudWithLocal()
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				Log.Info("OVERWRITING cloud saves with local saves.", 2);
				List<Task> tasks = new List<Task>();
				foreach (Task task in this.EnumerateOverwriteCloudWithLocalTasks(cloudSaveStore))
				{
					tasks.Add(task);
					if (tasks.Count >= 8)
					{
						await Task.WhenAll(tasks);
						tasks.Clear();
					}
				}
				IEnumerator<Task> enumerator = null;
				if (tasks.Count > 0)
				{
					await Task.WhenAll(tasks);
				}
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003FA2B File Offset: 0x0003DC2B
		private IEnumerable<Task> EnumerateOverwriteCloudWithLocalTasks(CloudSaveStore cloudStore)
		{
			SaveManager.<EnumerateOverwriteCloudWithLocalTasks>d__76 <EnumerateOverwriteCloudWithLocalTasks>d__ = new SaveManager.<EnumerateOverwriteCloudWithLocalTasks>d__76(-2);
			<EnumerateOverwriteCloudWithLocalTasks>d__.<>3__cloudStore = cloudStore;
			return <EnumerateOverwriteCloudWithLocalTasks>d__;
		}

		/// <summary>
		/// Load the current run save.
		/// </summary>
		/// <returns>The current run save, wrapped in a result object that contains some status info.</returns>
		// Token: 0x060007C6 RID: 1990 RVA: 0x0003FA3B File Offset: 0x0003DC3B
		public ReadSaveResult<SerializableRun> LoadRunSave()
		{
			return this._runSaveManager.LoadRunSave();
		}

		/// <summary>
		/// Load and validate the current multiplayer run save.
		/// Performs deep validation to ensure all save content is valid and handles corruption automatically.
		/// If the save contains deprecated content, the model IDs are automatically replaced with the deprecated versions.
		/// </summary>
		/// <param name="localPlayerId">The local player ID to validate against</param>
		/// <returns>The current multiplayer run save with validation status</returns>
		// Token: 0x060007C7 RID: 1991 RVA: 0x0003FA48 File Offset: 0x0003DC48
		public ReadSaveResult<SerializableRun> LoadAndCanonicalizeMultiplayerRunSave(ulong localPlayerId)
		{
			return this._runSaveManager.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0003FA58 File Offset: 0x0003DC58
		public void SaveRunHistory(RunHistory history)
		{
			try
			{
				this._runHistorySaveManager.SaveHistory(history);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to save run history: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0003FAB8 File Offset: 0x0003DCB8
		public int GetRunHistoryCount()
		{
			return this._runHistorySaveManager.GetHistoryCount();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0003FAC5 File Offset: 0x0003DCC5
		public List<string> GetAllRunHistoryNames()
		{
			return this._runHistorySaveManager.LoadAllRunHistoryNames();
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0003FAD2 File Offset: 0x0003DCD2
		public ReadSaveResult<RunHistory> LoadRunHistory(string fileName)
		{
			return this._runHistorySaveManager.LoadHistory(fileName);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0003FAE0 File Offset: 0x0003DCE0
		public static string ToJson<[Nullable(0), DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(T obj) where T : ISaveSchema
		{
			return JsonSerializationUtility.ToJson<T>(obj);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0003FAE8 File Offset: 0x0003DCE8
		public static ReadSaveResult<T> FromJson<[Nullable(0), DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string json) where T : ISaveSchema, new()
		{
			return JsonSerializationUtility.FromJson<T>(json);
		}

		/// <summary>
		/// Returns true if all ftues are disabled OR if the given ftue key exists (seen by the player before).
		/// Is also disabled if there's no game (test mode)
		/// </summary>
		// Token: 0x060007CE RID: 1998 RVA: 0x0003FAF0 File Offset: 0x0003DCF0
		public bool SeenFtue(string ftueKey)
		{
			return this._progressSaveManager.SeenFtue(ftueKey);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0003FAFE File Offset: 0x0003DCFE
		public bool SeenPopup(string popupKey)
		{
			return this._progressSaveManager.SeenPopup(popupKey);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0003FB0C File Offset: 0x0003DD0C
		public void SaveProgressFile()
		{
			this._progressSaveManager.SaveProgress();
		}

		/// <summary>
		/// Generates an unlock state from the current progress save.
		/// This object can be used to query what the player has unlocked.
		/// This should only be called from the main menu. When in a run, you should use the unlock state on <see cref="T:MegaCrit.Sts2.Core.Runs.RunManager" />
		/// or on Player. See the documentation in UnlockState for why.
		/// </summary>
		// Token: 0x060007D1 RID: 2001 RVA: 0x0003FB19 File Offset: 0x0003DD19
		public UnlockState GenerateUnlockStateFromProgress()
		{
			return this._progressSaveManager.GenerateUnlockState();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0003FB28 File Offset: 0x0003DD28
		public void SavePrefsFile()
		{
			try
			{
				this._prefsSaveManager.SavePrefs();
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to save prefs: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0003FB88 File Offset: 0x0003DD88
		public void MarkFtueAsComplete(string ftueId)
		{
			this._progressSaveManager.MarkFtueAsComplete(ftueId);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0003FB96 File Offset: 0x0003DD96
		public void SetFtuesEnabled(bool enabled)
		{
			this._progressSaveManager.SetFtuesEnabled(enabled);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0003FBA4 File Offset: 0x0003DDA4
		public void ResetFtues()
		{
			this._progressSaveManager.ResetFtues();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0003FBB1 File Offset: 0x0003DDB1
		public void MarkPotionAsSeen(PotionModel potion)
		{
			this._progressSaveManager.MarkPotionAsSeen(potion);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0003FBBF File Offset: 0x0003DDBF
		public void MarkCardAsSeen(CardModel card)
		{
			this._progressSaveManager.MarkCardAsSeen(card);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0003FBCD File Offset: 0x0003DDCD
		public void MarkRelicAsSeen(RelicModel relic)
		{
			this._progressSaveManager.MarkRelicAsSeen(relic);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0003FBDB File Offset: 0x0003DDDB
		public bool IsRelicSeen(RelicModel relic)
		{
			return this.Progress.DiscoveredRelics.Contains(relic.Id);
		}

		/// <summary>
		/// Sets an Epoch Slot to be available but not revealed/obtained.
		/// </summary>
		// Token: 0x060007DA RID: 2010 RVA: 0x0003FBF3 File Offset: 0x0003DDF3
		public void UnlockSlot(string epochId)
		{
			this.Progress.UnlockSlot(epochId);
		}

		/// <summary>
		/// Sets or creates an Epoch to any EpochState we wish.
		/// Used by TimelineExpansions for overriding behaviors.
		/// </summary>
		// Token: 0x060007DB RID: 2011 RVA: 0x0003FC01 File Offset: 0x0003DE01
		public void ObtainEpoch(string epochId)
		{
			this.Progress.ObtainEpoch(epochId);
		}

		/// <summary>
		/// Sets or creates an Epoch to any EpochState we wish.
		/// Used by TimelineExpansions for overriding behaviors.
		/// </summary>
		// Token: 0x060007DC RID: 2012 RVA: 0x0003FC0F File Offset: 0x0003DE0F
		public void ObtainEpochOverride(string epochId, EpochState state)
		{
			this.Progress.ObtainEpochOverride(epochId, state);
		}

		/// <summary>
		/// Reveals an Epoch. Sets an Epoch to IsComplete.
		/// Occurs when the player clicks on an Obtained Epoch in the Timeline screen.
		/// </summary>
		// Token: 0x060007DD RID: 2013 RVA: 0x0003FC1E File Offset: 0x0003DE1E
		public void RevealEpoch(string epochId, bool isDebug = false)
		{
			this.Progress.RevealEpoch(epochId);
			if (!isDebug)
			{
				MetricUtilities.UploadEpochMetric(epochId);
			}
		}

		/// <summary>
		/// Called by the debug Reset Progress button in the Timeline screen.
		/// </summary>
		// Token: 0x060007DE RID: 2014 RVA: 0x0003FC35 File Offset: 0x0003DE35
		public void ResetTimelineProgress()
		{
			this.Progress.ResetEpochs();
			this.ObtainEpochOverride(EpochModel.GetId<NeowEpoch>(), EpochState.Obtained);
			this.SaveProgressFile();
		}

		/// <summary>
		/// Checks if an epoch has been revealed on the timeline.
		/// You should only be querying this from the main menu. When you are inside of a run, you should be using
		/// <see cref="P:MegaCrit.Sts2.Core.Runs.RunState.UnlockState" /> or <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.UnlockState" />.
		/// </summary>
		// Token: 0x060007DF RID: 2015 RVA: 0x0003FC54 File Offset: 0x0003DE54
		[NullableContext(0)]
		public bool IsEpochRevealed<T>() where T : EpochModel
		{
			return this.Progress.IsEpochRevealed(EpochModel.GetId<T>());
		}

		/// <summary>
		/// Checks if an epoch has been revealed on the timeline.
		/// You should only be querying this from the main menu. When you are inside of a run, you should be using
		/// <see cref="P:MegaCrit.Sts2.Core.Runs.RunState.UnlockState" /> or <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.UnlockState" />.
		/// </summary>
		// Token: 0x060007E0 RID: 2016 RVA: 0x0003FC66 File Offset: 0x0003DE66
		public bool IsEpochRevealed(string id)
		{
			return this.Progress.IsEpochRevealed(id);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0003FC74 File Offset: 0x0003DE74
		public int GetTotalUnlockedCards()
		{
			return SaveManager.GetCardUnlockEpochIds().Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0003FC8E File Offset: 0x0003DE8E
		public static int GetUnlockableCardCount()
		{
			return SaveManager.GetCardUnlockEpochIds().Length * 3;
		}

		/// <summary>
		/// Helper method which returns every Epoch that unlocks cards in the game.
		/// Modify this list to affect our total card unlock statistics.
		/// </summary>
		// Token: 0x060007E3 RID: 2019 RVA: 0x0003FC9C File Offset: 0x0003DE9C
		private static string[] GetCardUnlockEpochIds()
		{
			return new string[]
			{
				EpochModel.GetId<Colorless1Epoch>(),
				EpochModel.GetId<Colorless2Epoch>(),
				EpochModel.GetId<Colorless3Epoch>(),
				EpochModel.GetId<Colorless4Epoch>(),
				EpochModel.GetId<Colorless5Epoch>(),
				EpochModel.GetId<Ironclad2Epoch>(),
				EpochModel.GetId<Ironclad5Epoch>(),
				EpochModel.GetId<Ironclad7Epoch>(),
				EpochModel.GetId<Silent2Epoch>(),
				EpochModel.GetId<Silent5Epoch>(),
				EpochModel.GetId<Silent7Epoch>(),
				EpochModel.GetId<Regent2Epoch>(),
				EpochModel.GetId<Regent5Epoch>(),
				EpochModel.GetId<Regent7Epoch>(),
				EpochModel.GetId<Defect2Epoch>(),
				EpochModel.GetId<Defect5Epoch>(),
				EpochModel.GetId<Defect7Epoch>(),
				EpochModel.GetId<Necrobinder2Epoch>(),
				EpochModel.GetId<Necrobinder5Epoch>(),
				EpochModel.GetId<Necrobinder7Epoch>()
			};
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0003FD5D File Offset: 0x0003DF5D
		public int GetTotalUnlockedRelics()
		{
			return SaveManager.GetRelicUnlockEpochIds().Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0003FD77 File Offset: 0x0003DF77
		public static int GetUnlockableRelicCount()
		{
			return SaveManager.GetRelicUnlockEpochIds().Length * 3;
		}

		/// <summary>
		/// Helper method which returns every Epoch that unlocks relics in the game.
		/// Modify this list to affect our total relic unlock statistics.
		/// </summary>
		// Token: 0x060007E6 RID: 2022 RVA: 0x0003FD84 File Offset: 0x0003DF84
		private static string[] GetRelicUnlockEpochIds()
		{
			return new string[]
			{
				EpochModel.GetId<Relic1Epoch>(),
				EpochModel.GetId<Relic2Epoch>(),
				EpochModel.GetId<Relic3Epoch>(),
				EpochModel.GetId<Relic4Epoch>(),
				EpochModel.GetId<Relic5Epoch>(),
				EpochModel.GetId<Ironclad3Epoch>(),
				EpochModel.GetId<Ironclad6Epoch>(),
				EpochModel.GetId<Silent3Epoch>(),
				EpochModel.GetId<Silent6Epoch>(),
				EpochModel.GetId<Regent3Epoch>(),
				EpochModel.GetId<Regent6Epoch>(),
				EpochModel.GetId<Defect3Epoch>(),
				EpochModel.GetId<Defect6Epoch>(),
				EpochModel.GetId<Necrobinder3Epoch>(),
				EpochModel.GetId<Necrobinder6Epoch>()
			};
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0003FE18 File Offset: 0x0003E018
		public int GetTotalUnlockedPotions()
		{
			return SaveManager.GetPotionUnlockEpochIds().Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0003FE32 File Offset: 0x0003E032
		public static int GetUnlockablePotionCount()
		{
			return SaveManager.GetPotionUnlockEpochIds().Length * 3;
		}

		/// <summary>
		/// Helper method which returns every Epoch that unlocks relics in the game.
		/// Modify this list to affect our total relic unlock statistics.
		/// </summary>
		// Token: 0x060007E9 RID: 2025 RVA: 0x0003FE40 File Offset: 0x0003E040
		private static string[] GetPotionUnlockEpochIds()
		{
			return new string[]
			{
				EpochModel.GetId<Potion1Epoch>(),
				EpochModel.GetId<Potion2Epoch>(),
				EpochModel.GetId<Ironclad4Epoch>(),
				EpochModel.GetId<Silent4Epoch>(),
				EpochModel.GetId<Regent4Epoch>(),
				EpochModel.GetId<Defect4Epoch>(),
				EpochModel.GetId<Necrobinder4Epoch>()
			};
		}

		/// <summary>
		/// Returns the sum of the Ascension progress of every character the player has.
		/// </summary>
		// Token: 0x060007EA RID: 2026 RVA: 0x0003FE8D File Offset: 0x0003E08D
		public int GetAggregateAscensionProgress()
		{
			return this.Progress.CharacterStats.Values.Sum<CharacterStats>((CharacterStats stat) => stat.MaxAscension);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0003FEC3 File Offset: 0x0003E0C3
		public static int GetAggregateAscensionCount()
		{
			return ModelDb.AllCharacters.Count<CharacterModel>() * 10;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0003FED2 File Offset: 0x0003E0D2
		public int GetTotalKills()
		{
			return this.Progress.EnemyStats.Values.Sum<EnemyStats>((EnemyStats enemy) => enemy.TotalWins);
		}

		/// <summary>
		/// <see cref="M:MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager.GetRevealableEpochs" />
		/// </summary>
		// Token: 0x060007ED RID: 2029 RVA: 0x0003FF08 File Offset: 0x0003E108
		public IEnumerable<SerializableEpoch> GetRevealableEpochs()
		{
			return this._progressSaveManager.GetRevealableEpochs();
		}

		/// <summary>
		/// Returns the number of Epochs we have that the player has discovered but not yet revealed in the Timeline.
		/// </summary>
		// Token: 0x060007EE RID: 2030 RVA: 0x0003FF15 File Offset: 0x0003E115
		public int GetDiscoveredEpochCount()
		{
			return this.GetRevealableEpochs().Count<SerializableEpoch>();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0003FF24 File Offset: 0x0003E124
		public bool IsNeowDiscovered()
		{
			SerializableEpoch serializableEpoch = this.Progress.Epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == EpochModel.GetId<NeowEpoch>());
			return serializableEpoch != null && serializableEpoch.State != EpochState.Revealed;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0003FF72 File Offset: 0x0003E172
		public int GetUnlocksRemaining()
		{
			return 18 - this.Progress.TotalUnlocks;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0003FF82 File Offset: 0x0003E182
		public int GetCurrentScore()
		{
			return this.Progress.CurrentScore;
		}

		/// <summary>
		/// Called whenever the score bar is filled.
		/// Increments TotalUnlocks and grants an Epoch.
		/// </summary>
		// Token: 0x060007F2 RID: 2034 RVA: 0x0003FF90 File Offset: 0x0003E190
		[NullableContext(2)]
		public string IncrementUnlock()
		{
			ProgressState progress = this.Progress;
			int totalUnlocks = progress.TotalUnlocks;
			progress.TotalUnlocks = totalUnlocks + 1;
			return this.GetEpochIdForUnlock();
		}

		/// <summary>
		/// Look-up function to get an Epoch ID based on the
		/// player's TotalUnlocks (the score bar system at the end of run).
		/// Must match <see cref="M:MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NGameOverScreen.GetScoreThreshold(System.Int32)" />.
		/// </summary>
		// Token: 0x060007F3 RID: 2035 RVA: 0x0003FFB8 File Offset: 0x0003E1B8
		[NullableContext(2)]
		private string GetEpochIdForUnlock()
		{
			int num = this.Progress.TotalUnlocks - 1;
			if (num < 0 || num >= SaveManager._agnosticEpochUnlockOrder.Length)
			{
				return null;
			}
			return SaveManager._agnosticEpochUnlockOrder[num];
		}

		/// <summary>
		/// The player needs to complete any run to access the compendium or be a dev.
		/// </summary>
		// Token: 0x060007F4 RID: 2036 RVA: 0x0003FFEA File Offset: 0x0003E1EA
		public bool IsCompendiumAvailable()
		{
			return this.Progress.NumberOfRuns > 0 || !NGame.IsReleaseGame();
		}

		// Token: 0x0400029F RID: 671
		[Nullable(2)]
		private static SaveManager _mockInstance;

		// Token: 0x040002A0 RID: 672
		[Nullable(2)]
		private static SaveManager _instance;

		// Token: 0x040002A1 RID: 673
		private readonly SettingsSaveManager _settingsSaveManager;

		// Token: 0x040002A2 RID: 674
		private readonly ProgressSaveManager _progressSaveManager;

		// Token: 0x040002A3 RID: 675
		private readonly RunSaveManager _runSaveManager;

		// Token: 0x040002A4 RID: 676
		private readonly RunHistorySaveManager _runHistorySaveManager;

		// Token: 0x040002A5 RID: 677
		private readonly PrefsSaveManager _prefsSaveManager;

		// Token: 0x040002A6 RID: 678
		private readonly ProfileSaveManager _profileSaveManager;

		// Token: 0x040002A7 RID: 679
		private readonly ISaveStore _saveStore;

		// Token: 0x040002A8 RID: 680
		private readonly MigrationManager _migrationManager;

		// Token: 0x040002A9 RID: 681
		private int? _currentProfileId;

		// Token: 0x040002AA RID: 682
		public const int totalAgnosticUnlocks = 18;

		/// <summary>
		/// The agnostic epoch IDs in unlock order.
		/// Used by <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.GetEpochIdForUnlock" /> and <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.GetRevealableEpochs" /> to enforce prerequisite ordering.
		/// </summary>
		// Token: 0x040002AB RID: 683
		private static readonly string[] _agnosticEpochUnlockOrder = new string[]
		{
			EpochModel.GetId<Colorless1Epoch>(),
			EpochModel.GetId<Relic1Epoch>(),
			EpochModel.GetId<Potion1Epoch>(),
			EpochModel.GetId<UnderdocksEpoch>(),
			EpochModel.GetId<Colorless2Epoch>(),
			EpochModel.GetId<Relic2Epoch>(),
			EpochModel.GetId<Potion2Epoch>(),
			EpochModel.GetId<Act2BEpoch>(),
			EpochModel.GetId<Colorless3Epoch>(),
			EpochModel.GetId<Relic3Epoch>(),
			EpochModel.GetId<Act3BEpoch>(),
			EpochModel.GetId<Colorless4Epoch>(),
			EpochModel.GetId<Relic4Epoch>(),
			EpochModel.GetId<Event1Epoch>(),
			EpochModel.GetId<Colorless5Epoch>(),
			EpochModel.GetId<Relic5Epoch>(),
			EpochModel.GetId<Event2Epoch>(),
			EpochModel.GetId<Event3Epoch>()
		};
	}
}
