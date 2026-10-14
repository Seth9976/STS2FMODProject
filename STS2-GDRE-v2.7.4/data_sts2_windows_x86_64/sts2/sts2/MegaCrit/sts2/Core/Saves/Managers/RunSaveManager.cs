using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Migrations;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000FC RID: 252
	[NullableContext(1)]
	[Nullable(0)]
	public class RunSaveManager
	{
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0004B748 File Offset: 0x00049948
		private string CurrentRunSavePath
		{
			get
			{
				return RunSaveManager.GetRunSavePath(this._profileIdProvider.CurrentProfileId, "current_run.save", null);
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0004B774 File Offset: 0x00049974
		private string CurrentMultiplayerRunSavePath
		{
			get
			{
				return RunSaveManager.GetRunSavePath(this._profileIdProvider.CurrentProfileId, "current_run_mp.save", null);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000AFC RID: 2812 RVA: 0x0004B7A0 File Offset: 0x000499A0
		// (remove) Token: 0x06000AFD RID: 2813 RVA: 0x0004B7D8 File Offset: 0x000499D8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Saved;

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0004B80D File Offset: 0x00049A0D
		public bool HasRunSave
		{
			get
			{
				return this._saveStore.FileExists(this.CurrentRunSavePath) || this._saveStore.FileExists(this.CurrentRunSavePath + ".backup");
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0004B83F File Offset: 0x00049A3F
		public bool HasMultiplayerRunSave
		{
			get
			{
				return this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath) || this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath + ".backup");
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0004B871 File Offset: 0x00049A71
		public int SchemaVersion
		{
			get
			{
				return this._migrationManager.GetLatestVersion<SerializableRun>();
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0004B87E File Offset: 0x00049A7E
		public RunSaveManager(int profileId, ISaveStore saveStore, MigrationManager migrationManager, bool forceSynchronous = false)
			: this(saveStore, migrationManager, new StaticProfileIdProvider(profileId), forceSynchronous)
		{
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0004B890 File Offset: 0x00049A90
		public RunSaveManager(ISaveStore saveStore, MigrationManager migrationManager, IProfileIdProvider profileIdProvider, bool forceSynchronous = false)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
			this._forceSynchronous = forceSynchronous;
			this._profileIdProvider = profileIdProvider;
		}

		/// <summary>
		/// Save a run while one is currently in progress.
		/// </summary>
		/// <param name="preFinishedRoom"></param>
		// Token: 0x06000B03 RID: 2819 RVA: 0x0004B8B8 File Offset: 0x00049AB8
		public async Task SaveRun([Nullable(2)] AbstractRoom preFinishedRoom)
		{
			if (RunManager.Instance.ShouldSave)
			{
				if (RunManager.Instance.NetService.Type == NetGameType.Singleplayer || RunManager.Instance.NetService.Type == NetGameType.Host)
				{
					SerializableRun serializableRun = RunManager.Instance.ToSave(preFinishedRoom);
					await this.SaveRun(serializableRun, RunManager.Instance.NetService.Type == NetGameType.Host);
				}
			}
		}

		/// <summary>
		/// Save a run when you just have a SerializableRun field, independent of whether run manager has been set up yet.
		/// WARNING: Only use this if you know what you are doing
		/// This exists so we can resave a serializable run without already having a run in progress
		/// Important for resaving SerializableRun.NumReloads right after we've started a run.
		/// in most cases you should call the other SaveRun overload.
		/// </summary>
		// Token: 0x06000B04 RID: 2820 RVA: 0x0004B904 File Offset: 0x00049B04
		public async Task SaveRun(SerializableRun save, bool isMultiplayer)
		{
			string savePath = (isMultiplayer ? this.CurrentMultiplayerRunSavePath : this.CurrentRunSavePath);
			using (MemoryStream stream = new MemoryStream())
			{
				if (!this._forceSynchronous)
				{
					await JsonSerializer.SerializeAsync<SerializableRun>(stream, save, JsonSerializationUtility.GetTypeInfo<SerializableRun>(), default(CancellationToken));
					stream.Seek(0L, SeekOrigin.Begin);
					await this._saveStore.WriteFileAsync(savePath, stream.ToArray());
				}
				else
				{
					JsonSerializer.Serialize<SerializableRun>(stream, save, JsonSerializationUtility.GetTypeInfo<SerializableRun>());
					stream.Seek(0L, SeekOrigin.Begin);
					this._saveStore.WriteFile(savePath, stream.ToArray());
				}
				Action saved = this.Saved;
				if (saved != null)
				{
					saved();
				}
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0004B958 File Offset: 0x00049B58
		public ReadSaveResult<SerializableRun> LoadRunSave()
		{
			ReadSaveResult<SerializableRun> readSaveResult = this._migrationManager.LoadSave<SerializableRun>(this.CurrentRunSavePath);
			if (readSaveResult.Success)
			{
				return readSaveResult;
			}
			if (readSaveResult.Status == ReadSaveStatus.FileNotFound)
			{
				Log.Info("Run save file not found at " + this.CurrentRunSavePath, 2);
			}
			else if (!readSaveResult.Status.IsRecoverable())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load run save: status=");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral(" msg=");
				defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Run save had recoverable issues: status=");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral(" msg=");
				defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return readSaveResult;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0004BA48 File Offset: 0x00049C48
		public ReadSaveResult<SerializableRun> LoadMultiplayerRunSave()
		{
			ReadSaveResult<SerializableRun> readSaveResult = this._migrationManager.LoadSave<SerializableRun>(this.CurrentMultiplayerRunSavePath);
			if (readSaveResult.Success)
			{
				return readSaveResult;
			}
			if (readSaveResult.Status == ReadSaveStatus.FileNotFound)
			{
				Log.Info("Multiplayer run save file not found at " + this.CurrentMultiplayerRunSavePath, 2);
			}
			else if (!readSaveResult.Status.IsRecoverable())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load multiplayer run save: status=");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral(" msg=");
				defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Multiplayer run save had recoverable issues: status=");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral(" msg=");
				defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return readSaveResult;
		}

		/// <summary>
		/// Loads the multiplayer run and canonicalizes it.
		/// In this case, "canonicalize" means that we load the save, detect any deprecated content, and replace it with
		/// the deprecated version of that model (e.g. <see cref="T:MegaCrit.Sts2.Core.Models.Events.DeprecatedEvent" />.)
		/// </summary>
		/// <param name="localPlayerId">The local player ID who is loading the save.</param>
		// Token: 0x06000B07 RID: 2823 RVA: 0x0004BB38 File Offset: 0x00049D38
		public ReadSaveResult<SerializableRun> LoadAndCanonicalizeMultiplayerRunSave(ulong localPlayerId)
		{
			ReadSaveResult<SerializableRun> readSaveResult = this.LoadMultiplayerRunSave();
			if (readSaveResult != null && readSaveResult.Success && readSaveResult.SaveData != null)
			{
				try
				{
					SerializableRun serializableRun = RunManager.CanonicalizeSave(readSaveResult.SaveData, localPlayerId);
					return new ReadSaveResult<SerializableRun>(serializableRun, ReadSaveStatus.Success, null);
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Multiplayer run save validation failed: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this.RenameBrokenMultiplayerRunSave(ReadSaveStatus.ValidationFailed);
					ReadSaveStatus readSaveStatus = ReadSaveStatus.ValidationFailed;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Save file validation failed: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					return new ReadSaveResult<SerializableRun>(readSaveStatus, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return readSaveResult;
			}
			return readSaveResult;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0004BBFC File Offset: 0x00049DFC
		public void DeleteCurrentRun()
		{
			this._saveStore.DeleteFile(this.CurrentRunSavePath);
			this._saveStore.DeleteFile(this.CurrentRunSavePath + ".backup");
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0004BC2A File Offset: 0x00049E2A
		public void DeleteCurrentMultiplayerRun()
		{
			this._saveStore.DeleteFile(this.CurrentMultiplayerRunSavePath);
			this._saveStore.DeleteFile(this.CurrentMultiplayerRunSavePath + ".backup");
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0004BC58 File Offset: 0x00049E58
		public void RenameBrokenMultiplayerRunSave(ReadSaveStatus status)
		{
			try
			{
				if (this.HasMultiplayerRunSave)
				{
					if (this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath))
					{
						string text = CorruptFileHandler.GenerateCorruptFilePath(this.CurrentMultiplayerRunSavePath, status);
						this._saveStore.RenameFile(this.CurrentMultiplayerRunSavePath, text);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Corrupt multiplayer run save detected: Renamed '");
						defaultInterpolatedStringHandler.AppendFormatted(this.CurrentMultiplayerRunSavePath);
						defaultInterpolatedStringHandler.AppendLiteral("' to '");
						defaultInterpolatedStringHandler.AppendFormatted(text);
						defaultInterpolatedStringHandler.AppendLiteral("'");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
					string text2 = this.CurrentMultiplayerRunSavePath + ".backup";
					if (this._saveStore.FileExists(text2))
					{
						string text3 = CorruptFileHandler.GenerateCorruptFilePath(text2, status);
						this._saveStore.RenameFile(text2, text3);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Corrupt multiplayer run backup detected: Renamed '");
						defaultInterpolatedStringHandler.AppendFormatted(text2);
						defaultInterpolatedStringHandler.AppendLiteral("' to '");
						defaultInterpolatedStringHandler.AppendFormatted(text3);
						defaultInterpolatedStringHandler.AppendLiteral("'");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to rename broken multiplayer run save: " + ex.Message, 2);
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0004BDAC File Offset: 0x00049FAC
		public static string GetRunSavePath(int profileId, string fileName, bool? forceModState = null)
		{
			return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), fileName);
		}

		// Token: 0x040003CD RID: 973
		public const string runSaveFileName = "current_run.save";

		// Token: 0x040003CE RID: 974
		public const string multiplayerRunSaveFileName = "current_run_mp.save";

		// Token: 0x040003CF RID: 975
		private readonly ISaveStore _saveStore;

		// Token: 0x040003D0 RID: 976
		private readonly MigrationManager _migrationManager;

		// Token: 0x040003D1 RID: 977
		private readonly bool _forceSynchronous;

		// Token: 0x040003D2 RID: 978
		private readonly IProfileIdProvider _profileIdProvider;
	}
}
