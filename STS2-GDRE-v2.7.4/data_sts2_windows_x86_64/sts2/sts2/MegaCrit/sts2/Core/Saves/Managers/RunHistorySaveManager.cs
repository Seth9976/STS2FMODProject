using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Migrations;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	[Nullable(0)]
	public class RunHistorySaveManager
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0004B444 File Offset: 0x00049644
		private string HistoryPath
		{
			get
			{
				return RunHistorySaveManager.GetHistoryPath(this._profileIdProvider.CurrentProfileId, null);
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0004B46A File Offset: 0x0004966A
		public RunHistorySaveManager(int profileId, ISaveStore saveStore, MigrationManager migrationManager)
			: this(saveStore, migrationManager, new StaticProfileIdProvider(profileId))
		{
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0004B47A File Offset: 0x0004967A
		public RunHistorySaveManager(ISaveStore saveStore, MigrationManager migrationManager, IProfileIdProvider profileIdProvider)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
			this._profileIdProvider = profileIdProvider;
		}

		/// <summary>
		/// This should be called right after the profile ID is initialized so that there is a directory we can save run
		/// histories to.
		/// </summary>
		// Token: 0x06000AF3 RID: 2803 RVA: 0x0004B497 File Offset: 0x00049697
		public void CreateRunHistoryDirectory()
		{
			this._saveStore.CreateDirectory(this.HistoryPath);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0004B4AA File Offset: 0x000496AA
		public static string GetHistoryPath(int profileId, bool? forceModState = null)
		{
			return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "history");
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0004B4C8 File Offset: 0x000496C8
		public void SaveHistory(RunHistory history)
		{
			history.SchemaVersion = this._migrationManager.GetLatestVersion<RunHistory>();
			string text = JsonSerializationUtility.ToJson<RunHistory>(history);
			string historyPath = this.HistoryPath;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
			defaultInterpolatedStringHandler.AppendFormatted<long>(history.StartTime);
			defaultInterpolatedStringHandler.AppendLiteral(".run");
			string text2 = Path.Combine(historyPath, defaultInterpolatedStringHandler.ToStringAndClear());
			this.SaveHistoryInternal(text2, text);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Saved run history: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(history.StartTime);
			defaultInterpolatedStringHandler.AppendLiteral(".run");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0004B568 File Offset: 0x00049768
		private void SaveHistoryInternal(string path, string content)
		{
			CloudSaveStore cloudSaveStore = this._saveStore as CloudSaveStore;
			if (cloudSaveStore != null)
			{
				int byteCount = Encoding.UTF8.GetByteCount(content);
				cloudSaveStore.ForgetFilesInDirectoryBeforeWritingIfNecessary(this.HistoryPath, byteCount, 5242880, 100);
			}
			this._saveStore.WriteFile(path, content);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0004B5B1 File Offset: 0x000497B1
		public int GetHistoryCount()
		{
			if (!this._saveStore.DirectoryExists(this.HistoryPath))
			{
				return 0;
			}
			return this._saveStore.GetFilesInDirectory(this.HistoryPath).Length;
		}

		/// <summary>
		/// Loads a single run history file with migration support.
		/// </summary>
		/// <param name="fileName">The history file name (e.g., "1234567890.run")</param>
		/// <returns>The result of the load operation</returns>
		// Token: 0x06000AF8 RID: 2808 RVA: 0x0004B5DC File Offset: 0x000497DC
		public ReadSaveResult<RunHistory> LoadHistory(string fileName)
		{
			string text = Path.Combine(this.HistoryPath, fileName);
			ReadSaveResult<RunHistory> readSaveResult = this._migrationManager.LoadSave<RunHistory>(text);
			if (readSaveResult.Success)
			{
				Log.Info("Successfully loaded run history: " + fileName, 2);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load run history ");
				defaultInterpolatedStringHandler.AppendFormatted(fileName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return readSaveResult;
		}

		/// <summary>
		/// Returns a list of all the run histories that we have available to load.
		/// To actually load the history, use <see cref="M:MegaCrit.Sts2.Core.Saves.Managers.RunHistorySaveManager.LoadHistory(System.String)" />.
		/// </summary>
		// Token: 0x06000AF9 RID: 2809 RVA: 0x0004B664 File Offset: 0x00049864
		public List<string> LoadAllRunHistoryNames()
		{
			string[] filesInDirectory = this._saveStore.GetFilesInDirectory(this.HistoryPath);
			int num = 0;
			List<string> list = new List<string>();
			foreach (string text in filesInDirectory)
			{
				if (text.EndsWith(".corrupt"))
				{
					num++;
				}
				else if (!text.EndsWith(".backup"))
				{
					list.Add(text);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num > 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" corrupt save files in history directory");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Found ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" run history files");
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return list;
		}

		// Token: 0x040003C7 RID: 967
		public const int maxCloudBytes = 5242880;

		// Token: 0x040003C8 RID: 968
		public const int maxCloudFileCount = 100;

		// Token: 0x040003C9 RID: 969
		private const string _historyDirName = "history";

		// Token: 0x040003CA RID: 970
		private readonly ISaveStore _saveStore;

		// Token: 0x040003CB RID: 971
		private readonly MigrationManager _migrationManager;

		// Token: 0x040003CC RID: 972
		private readonly IProfileIdProvider _profileIdProvider;
	}
}
