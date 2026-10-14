using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// An implementation of ISaveStore which saves files to both local and cloud storage.
	/// This is the main entry point for cloud saves. You should not use any cloud-enabled save stores directly - instead,
	/// write and read through this abstraction layer.
	/// When files are read, they are always read from local storage. SyncCloudToLocal should be called once at the beginning
	/// of the game's lifecycle before any files are read so that you are reading the up-to-date data.
	///
	/// CLOUD ERROR POLICY: All cloud operations are best-effort. A cloud failure must never prevent local saves from
	/// working or the game from starting. Exception handlers catch broadly (Exception, not specific types) because
	/// SteamRemoteSaveStore methods are P/Invoke calls into Valve's native steam_api DLL, which can throw SEHException
	/// at any time in addition to the managed exceptions (InvalidOperationException, SteamRemoteSaveStoreException)
	/// that our wrapper code throws.
	/// </summary>
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	[Nullable(0)]
	public class CloudSaveStore : ICloudSaveStore, ISaveStore
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001627D File Offset: 0x0001447D
		public ISaveStore LocalStore { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00016285 File Offset: 0x00014485
		public ICloudSaveStore CloudStore { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="localStore">The class which will be used to save to local storage.</param>
		/// <param name="cloudStore">The class which will be used to save to cloud storage.</param>
		// Token: 0x060004B4 RID: 1204 RVA: 0x0001628D File Offset: 0x0001448D
		public CloudSaveStore(ISaveStore localStore, ICloudSaveStore cloudStore)
		{
			this.LocalStore = localStore;
			this.CloudStore = cloudStore;
		}

		/// <summary>
		/// Reads a file from local storage.
		/// </summary>
		// Token: 0x060004B5 RID: 1205 RVA: 0x000162AE File Offset: 0x000144AE
		[return: Nullable(2)]
		public string ReadFile(string path)
		{
			return this.LocalStore.ReadFile(path);
		}

		/// <summary>
		/// Reads a file asynchronously from local storage.
		/// </summary>
		// Token: 0x060004B6 RID: 1206 RVA: 0x000162BC File Offset: 0x000144BC
		[return: Nullable(new byte[] { 1, 2 })]
		public Task<string> ReadFileAsync(string path)
		{
			return this.LocalStore.ReadFileAsync(path);
		}

		/// <summary>
		/// Checks if a file exists in local storage.
		/// </summary>
		// Token: 0x060004B7 RID: 1207 RVA: 0x000162CA File Offset: 0x000144CA
		public bool FileExists(string path)
		{
			return this.LocalStore.FileExists(path);
		}

		/// <summary>
		/// Checks if a directory exists in local storage.
		/// </summary>
		// Token: 0x060004B8 RID: 1208 RVA: 0x000162D8 File Offset: 0x000144D8
		public bool DirectoryExists(string path)
		{
			return this.LocalStore.DirectoryExists(path);
		}

		/// <summary>
		/// Writes a file synchronously to both local and remote storage.
		/// </summary>
		// Token: 0x060004B9 RID: 1209 RVA: 0x000162E8 File Offset: 0x000144E8
		public void WriteFile(string path, string content)
		{
			this.LocalStore.WriteFile(path, content);
			try
			{
				this.CloudStore.WriteFile(path, content);
				this.SyncLocalTimestamp(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud write failed for " + path + ", local file preserved: " + ex.Message, 2);
				this.SyncLocalTimestamp(path);
			}
		}

		/// <summary>
		/// Writes a file synchronously to both local and remote storage.
		/// </summary>
		// Token: 0x060004BA RID: 1210 RVA: 0x00016350 File Offset: 0x00014550
		public void WriteFile(string path, byte[] bytes)
		{
			this.LocalStore.WriteFile(path, bytes);
			try
			{
				this.CloudStore.WriteFile(path, bytes);
				this.SyncLocalTimestamp(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud write failed for " + path + ", local file preserved: " + ex.Message, 2);
				this.SyncLocalTimestamp(path);
			}
		}

		/// <summary>
		/// Writes a file asynchronously to both local and remote storage.
		/// </summary>
		// Token: 0x060004BB RID: 1211 RVA: 0x000163B8 File Offset: 0x000145B8
		public async Task WriteFileAsync(string path, string content)
		{
			await this.LocalStore.WriteFileAsync(path, content);
			try
			{
				await this.CloudStore.WriteFileAsync(path, content);
				this.SyncLocalTimestamp(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud write failed for " + path + ", local file preserved: " + ex.Message, 2);
				this.SyncLocalTimestamp(path);
			}
		}

		/// <summary>
		/// Writes a file asynchronously to both local and remote storage.
		/// </summary>
		// Token: 0x060004BC RID: 1212 RVA: 0x0001640C File Offset: 0x0001460C
		public async Task WriteFileAsync(string path, byte[] bytes)
		{
			await this.LocalStore.WriteFileAsync(path, bytes);
			try
			{
				await this.CloudStore.WriteFileAsync(path, bytes);
				this.SyncLocalTimestamp(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud write failed for " + path + ", local file preserved: " + ex.Message, 2);
				this.SyncLocalTimestamp(path);
			}
		}

		/// <summary>
		/// Deletes a file from both local and remote storage.
		/// </summary>
		// Token: 0x060004BD RID: 1213 RVA: 0x00016460 File Offset: 0x00014660
		public void DeleteFile(string path)
		{
			this.LocalStore.DeleteFile(path);
			try
			{
				this.CloudStore.DeleteFile(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud delete failed for " + path + ", local delete preserved: " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Renames a file in both local and remote storage.
		/// Try to avoid using this, Steam's remote storage doesn't allow atomic renaming.
		/// </summary>
		// Token: 0x060004BE RID: 1214 RVA: 0x000164B8 File Offset: 0x000146B8
		public void RenameFile(string sourcePath, string destinationPath)
		{
			this.LocalStore.RenameFile(sourcePath, destinationPath);
			try
			{
				this.CloudStore.RenameFile(sourcePath, destinationPath);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Cloud rename failed for ");
				defaultInterpolatedStringHandler.AppendFormatted(sourcePath);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted(destinationPath);
				defaultInterpolatedStringHandler.AppendLiteral(", local rename preserved: ");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Returns the files in the directory from local storage.
		/// </summary>
		// Token: 0x060004BF RID: 1215 RVA: 0x0001654C File Offset: 0x0001474C
		public string[] GetFilesInDirectory(string directoryPath)
		{
			return this.LocalStore.GetFilesInDirectory(directoryPath);
		}

		/// <summary>
		/// Returns the directories in the directory read from local storage.
		/// </summary>
		// Token: 0x060004C0 RID: 1216 RVA: 0x0001655A File Offset: 0x0001475A
		public string[] GetDirectoriesInDirectory(string directoryPath)
		{
			return this.LocalStore.GetDirectoriesInDirectory(directoryPath);
		}

		/// <summary>
		/// Creates a directory in both local and remote storage.
		/// </summary>
		// Token: 0x060004C1 RID: 1217 RVA: 0x00016568 File Offset: 0x00014768
		public void CreateDirectory(string directoryPath)
		{
			this.LocalStore.CreateDirectory(directoryPath);
			try
			{
				this.CloudStore.CreateDirectory(directoryPath);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud create directory failed for " + directoryPath + ": " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Deletes a directory and any remaining contents in both local and remote storage.
		/// </summary>
		// Token: 0x060004C2 RID: 1218 RVA: 0x000165C0 File Offset: 0x000147C0
		public void DeleteDirectory(string directoryPath)
		{
			this.LocalStore.DeleteDirectory(directoryPath);
			try
			{
				this.CloudStore.DeleteDirectory(directoryPath);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud delete directory failed for " + directoryPath + ": " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Deletes temporary files from both local and remote storage.
		/// </summary>
		// Token: 0x060004C3 RID: 1219 RVA: 0x00016618 File Offset: 0x00014818
		public void DeleteTemporaryFiles(string directoryPath)
		{
			this.LocalStore.DeleteTemporaryFiles(directoryPath);
			try
			{
				this.CloudStore.DeleteTemporaryFiles(directoryPath);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud delete temporary files failed for " + directoryPath + ": " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Gets the last modified time of a file in local storage.
		/// </summary>
		// Token: 0x060004C4 RID: 1220 RVA: 0x00016670 File Offset: 0x00014870
		public DateTimeOffset GetLastModifiedTime(string path)
		{
			return this.LocalStore.GetLastModifiedTime(path);
		}

		/// <summary>
		/// Gets the size of a file without reading the entire file.
		/// </summary>
		// Token: 0x060004C5 RID: 1221 RVA: 0x0001667E File Offset: 0x0001487E
		public int GetFileSize(string path)
		{
			return this.LocalStore.GetFileSize(path);
		}

		/// <summary>
		/// Sets the last modified time of a file in local storage.
		/// </summary>
		// Token: 0x060004C6 RID: 1222 RVA: 0x0001668C File Offset: 0x0001488C
		public void SetLastModifiedTime(string path, DateTimeOffset time)
		{
			this.LocalStore.SetLastModifiedTime(path, time);
		}

		/// <summary>
		/// Returns the full path of a file in local storage.
		/// </summary>
		// Token: 0x060004C7 RID: 1223 RVA: 0x0001669B File Offset: 0x0001489B
		public string GetFullPath(string filename)
		{
			return this.LocalStore.GetFullPath(filename);
		}

		/// <summary>
		/// Checks if the cloud storage has any files stored.
		/// </summary>
		// Token: 0x060004C8 RID: 1224 RVA: 0x000166A9 File Offset: 0x000148A9
		public bool HasCloudFiles()
		{
			return this.CloudStore.HasCloudFiles();
		}

		/// <summary>
		/// Removes a file from remote storage, but keeps it in the local storage.
		/// </summary>
		// Token: 0x060004C9 RID: 1225 RVA: 0x000166B8 File Offset: 0x000148B8
		public void ForgetFile(string path)
		{
			try
			{
				this.CloudStore.ForgetFile(path);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud forget failed for " + path + ": " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Returns true if the file at the path is in remote storage.
		/// The file can also be in local storage, but this is disregarded.
		/// </summary>
		// Token: 0x060004CA RID: 1226 RVA: 0x00016704 File Offset: 0x00014904
		public bool IsFilePersisted(string path)
		{
			return this.CloudStore.IsFilePersisted(path);
		}

		/// <inheritdoc />
		// Token: 0x060004CB RID: 1227 RVA: 0x00016712 File Offset: 0x00014912
		public void BeginSaveBatch()
		{
			this.CloudStore.BeginSaveBatch();
		}

		/// <inheritdoc />
		// Token: 0x060004CC RID: 1228 RVA: 0x0001671F File Offset: 0x0001491F
		public void EndSaveBatch()
		{
			this.CloudStore.EndSaveBatch();
		}

		/// <summary>
		/// Synchronizes a file from the remote to local storage.
		/// If the file exists on remote storage and the last-modified timestamp differs at all from the local one, then the
		/// local file will be overwritten with the one from remote storage.
		/// If the file doesn't exist on remote storage but does locally, then the local file will be deleted.
		/// </summary>
		// Token: 0x060004CD RID: 1229 RVA: 0x0001672C File Offset: 0x0001492C
		public async Task SyncCloudToLocal(string path)
		{
			try
			{
				await this.SyncCloudToLocalInternal(path);
			}
			catch (Exception ex)
			{
				Log.Warn("SteamRemoteStorage: Failed to sync " + path + " from cloud, skipping: " + ex.Message, 2);
				if (this._syncFailureReporter.ShouldReport(ex))
				{
					SentryService.CaptureException(ex);
				}
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00016778 File Offset: 0x00014978
		private async Task SyncCloudToLocalInternal(string path)
		{
			bool flag = this.CloudStore.FileExists(path);
			bool flag2 = this.LocalStore.FileExists(path);
			if (flag)
			{
				DateTimeOffset lastModifiedTime = this.CloudStore.GetLastModifiedTime(path);
				DateTimeOffset? dateTimeOffset = (flag2 ? new DateTimeOffset?(this.LocalStore.GetLastModifiedTime(path)) : null);
				bool flag3 = !flag2 || lastModifiedTime != dateTimeOffset;
				if (!flag3)
				{
					string text = this.LocalStore.ReadFile(path);
					if (string.IsNullOrWhiteSpace(text))
					{
						Log.Warn("Local file " + path + " appears corrupt (empty content) despite matching cloud timestamp, forcing re-download from cloud", 2);
						flag3 = true;
					}
				}
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Copying ");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(" from cloud to local. Local file exists: ");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(flag2);
					defaultInterpolatedStringHandler.AppendLiteral(" Cloud save time: ");
					defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset>(lastModifiedTime);
					defaultInterpolatedStringHandler.AppendLiteral(" Local save time: ");
					defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset?>(dateTimeOffset);
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					string text2 = await this.CloudStore.ReadFileAsync(path);
					string text3 = text2;
					if (string.IsNullOrWhiteSpace(text3) || text3[0] == '\0')
					{
						Log.Warn("Cloud file " + path + " has empty content, skipping download", 2);
					}
					else
					{
						await this.LocalStore.WriteFileAsync(path, text3);
						this.SyncLocalTimestamp(path);
					}
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Skipping sync for ");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(", last modified time matches on local and remote (");
					defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset>(lastModifiedTime);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			else if (flag2)
			{
				Log.Info("Deleting " + path + " because it does not exist on remote", 2);
				this.LocalStore.DeleteFile(path);
				this.LocalStore.DeleteFile(path + ".backup");
			}
			else
			{
				Log.Debug("Skipping sync for " + path + ", it doesn't exist on either local or cloud", 2);
			}
		}

		/// <summary>
		/// Synchronizes an entire directory from cloud to local storage.
		/// The rules for <see cref="M:MegaCrit.Sts2.Core.Saves.CloudSaveStore.SyncCloudToLocal(System.String)" /> are followed for every file found in the given directory, first for
		/// those found on the cloud, then for those found on the local side (if any files existed locally but not in the
		/// cloud).
		/// </summary>
		// Token: 0x060004CF RID: 1231 RVA: 0x000167C3 File Offset: 0x000149C3
		public IEnumerable<Task> SyncCloudToLocalDirectory(string directoryPath)
		{
			CloudSaveStore.<SyncCloudToLocalDirectory>d__34 <SyncCloudToLocalDirectory>d__ = new CloudSaveStore.<SyncCloudToLocalDirectory>d__34(-2);
			<SyncCloudToLocalDirectory>d__.<>4__this = this;
			<SyncCloudToLocalDirectory>d__.<>3__directoryPath = directoryPath;
			return <SyncCloudToLocalDirectory>d__;
		}

		/// <summary>
		/// Overwrites the state of a file in the cloud with the state of a file on the local side.
		/// This is used when the player launches the game for the first time with cloud sync enabled.
		/// If the file exists locally, then the file on the cloud is replaced unconditionally. If the file doesn't exist
		/// locally but exists on the cloud, the file is deleted from the cloud.
		/// </summary>
		/// <param name="path">The relative path to the file.</param>
		/// <param name="forgetImmediately">If this is true, then after the file is written to the cloud, then it is forgotten
		/// immediately. See comments in <see cref="M:MegaCrit.Sts2.Core.Saves.CloudSaveStore.OverwriteCloudWithLocalDirectory(System.String,System.Nullable{System.Int32},System.Nullable{System.Int32})" /> for when you might use this.</param>
		// Token: 0x060004D0 RID: 1232 RVA: 0x000167DC File Offset: 0x000149DC
		public async Task OverwriteCloudWithLocal(string path, bool forgetImmediately = false)
		{
			if (this.LocalStore.FileExists(path))
			{
				Log.Debug("Writing file " + path + " to cloud", 2);
				string text = await this.LocalStore.ReadFileAsync(path);
				string text2 = text;
				try
				{
					await this.CloudStore.WriteFileAsync(path, text2);
					if (forgetImmediately)
					{
						try
						{
							Log.Debug("Immediately forgetting " + path, 2);
							this.CloudStore.ForgetFile(path);
						}
						catch (Exception ex)
						{
							Log.Warn("Cloud forget failed for " + path + ": " + ex.Message, 2);
						}
					}
					this.SyncLocalTimestamp(path);
					return;
				}
				catch (Exception ex2)
				{
					Log.Warn("Cloud write failed for " + path + ", local file preserved: " + ex2.Message, 2);
					this.SyncLocalTimestamp(path);
					return;
				}
			}
			try
			{
				if (this.CloudStore.FileExists(path))
				{
					Log.Debug("Deleting file " + path + " from cloud because it doesn't exist on local", 2);
					this.CloudStore.DeleteFile(path);
				}
			}
			catch (Exception ex3)
			{
				Log.Warn("Cloud delete failed for " + path + ": " + ex3.Message, 2);
			}
		}

		/// <summary>
		/// Overwrites the state of a directory in the cloud with the state of a directory on the local side.
		/// This is used when the player launches the game for the first time with cloud sync enabled.
		/// The rules for <see cref="M:MegaCrit.Sts2.Core.Saves.CloudSaveStore.SyncCloudToLocal(System.String)" /> are followed for every file found in the given directory, first for
		/// those found on the cloud, then for those found on the local side (if any files existed locally but not in the
		/// cloud).
		///
		/// Files are written in order of last-modified time. Once we exceed either byteLimit or fileLimit, then files are
		/// written to the cloud storage, but they are forgotten from the remote cloud storage (not deleted, just forgotten).
		///
		/// So why write-and-forget? Why not just... not write? Later, when we go to sync cloud files to the local storage,
		/// we need the files to exist in the remote storage. Otherwise, we'll delete the files in the local storage.
		/// </summary>
		/// <param name="directoryPath">The directory to sync.</param>
		/// <param name="byteLimit">The maximum number of bytes that can be written to storage.</param>
		/// <param name="fileLimit">The maximum number of files that can be written to storage.</param>
		// Token: 0x060004D1 RID: 1233 RVA: 0x0001682F File Offset: 0x00014A2F
		public IEnumerable<Task> OverwriteCloudWithLocalDirectory(string directoryPath, int? byteLimit, int? fileLimit)
		{
			CloudSaveStore.<OverwriteCloudWithLocalDirectory>d__36 <OverwriteCloudWithLocalDirectory>d__ = new CloudSaveStore.<OverwriteCloudWithLocalDirectory>d__36(-2);
			<OverwriteCloudWithLocalDirectory>d__.<>4__this = this;
			<OverwriteCloudWithLocalDirectory>d__.<>3__directoryPath = directoryPath;
			<OverwriteCloudWithLocalDirectory>d__.<>3__byteLimit = byteLimit;
			<OverwriteCloudWithLocalDirectory>d__.<>3__fileLimit = fileLimit;
			return <OverwriteCloudWithLocalDirectory>d__;
		}

		/// <summary>
		/// Forgets the oldest files that would cause us to go over the byte/file limit quotas.
		/// This is used when the player is writing a new run history file, which might exceed a limit that we set on the
		/// count/size of run histories.
		/// </summary>
		/// <param name="directoryPath">The directory to sync.</param>
		/// <param name="bytesToBeWritten">The number bytes that will be written to the new run history file.</param>
		/// <param name="byteLimit">The maximum number of bytes that can be written to storage.</param>
		/// <param name="fileLimit">The maximum number of files that can be written to storage.</param>
		// Token: 0x060004D2 RID: 1234 RVA: 0x00016854 File Offset: 0x00014A54
		public void ForgetFilesInDirectoryBeforeWritingIfNecessary(string directoryPath, int bytesToBeWritten, int byteLimit, int fileLimit)
		{
			try
			{
				this.ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal(directoryPath, bytesToBeWritten, byteLimit, fileLimit);
			}
			catch (Exception ex)
			{
				Log.Warn("Cloud quota management failed for " + directoryPath + ": " + ex.Message, 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000168A4 File Offset: 0x00014AA4
		private void ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal(string directoryPath, int bytesToBeWritten, int byteLimit, int fileLimit)
		{
			int num = bytesToBeWritten;
			int num2 = 1;
			string[] filesInDirectory = this.CloudStore.GetFilesInDirectory(directoryPath);
			List<string> list = new List<string>();
			foreach (string text in filesInDirectory)
			{
				if (CloudSaveStore.ShouldSyncFileToCloud(text))
				{
					string text2 = directoryPath + "/" + text;
					if (this.CloudStore.IsFilePersisted(text2))
					{
						list.Add(text2);
						num += this.CloudStore.GetFileSize(text2);
						num2++;
					}
				}
			}
			if (num > byteLimit || num2 > fileLimit)
			{
				list.Sort((string p1, string p2) => this.GetLastModifiedTime(p2).CompareTo(this.GetLastModifiedTime(p1)));
				while (num > byteLimit || num2 > fileLimit)
				{
					List<string> list2 = list;
					string text3 = list2[list2.Count - 1];
					num -= this.CloudStore.GetFileSize(text3);
					num2--;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(116, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Forgetting file ");
					defaultInterpolatedStringHandler.AppendFormatted(text3);
					defaultInterpolatedStringHandler.AppendLiteral(" from cloud storage because we're past our quota. ");
					defaultInterpolatedStringHandler.AppendLiteral("Bytes after forgetting: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(". Files after forgetting: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this.CloudStore.ForgetFile(text3);
					list.RemoveAt(list.Count - 1);
				}
			}
		}

		/// <summary>
		/// Returns true if the file should be included in cloud sync operations. Filters out .backup files,
		/// which are local crash-recovery artifacts created by CopyBackup. These should never be uploaded to
		/// or synced from cloud storage.
		/// </summary>
		// Token: 0x060004D4 RID: 1236 RVA: 0x000169F9 File Offset: 0x00014BF9
		private static bool ShouldSyncFileToCloud(string fileName)
		{
			return !fileName.EndsWith(".backup");
		}

		/// <summary>
		/// Removes a .backup file that should never have been uploaded to cloud storage. These are local-only
		/// artifacts that were uploaded before OverwriteCloudWithLocalDirectory filtered them. Cleaning them
		/// up frees cloud quota.
		/// </summary>
		// Token: 0x060004D5 RID: 1237 RVA: 0x00016A0C File Offset: 0x00014C0C
		private void DeleteStaleBackupFromCloud(string directoryPath, string cloudPath)
		{
			string text = directoryPath + "/" + cloudPath;
			Log.Info("Removing stale .backup file from cloud: " + text, 2);
			try
			{
				this.CloudStore.DeleteFile(text);
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to remove .backup from cloud " + text + ": " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Syncs the local file's last modified time to the cloud file's last modified time.
		/// This is best-effort: on Windows, the file can become temporarily unavailable (antivirus, cloud sync tools, file
		/// system contention) between the write and the timestamp update. A failed sync just means the next cloud sync might
		/// redundantly re-copy the file, which is harmless.
		///
		/// Called in both the success path (to sync timestamps after a cloud write) and the failure path (to prevent
		/// SyncCloudToLocal from overwriting local with stale cloud data on next startup).
		///
		/// Catches Exception broadly (not just IOException) because CloudStore.GetLastModifiedTime is a P/Invoke call
		/// that can throw several exception types (SEHException, InvalidOperationException, etc.). The consequence of
		/// not catching broadly here is a game crash, which is not acceptable for a timestamp sync failure.
		/// </summary>
		// Token: 0x060004D6 RID: 1238 RVA: 0x00016A74 File Offset: 0x00014C74
		private void SyncLocalTimestamp(string path)
		{
			try
			{
				this.LocalStore.SetLastModifiedTime(path, this.CloudStore.GetLastModifiedTime(path));
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to sync timestamp for " + path + ", will re-sync on next launch: " + ex.Message, 2);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00016ACC File Offset: 0x00014CCC
		public bool HasUserEnabledCloudSync()
		{
			return this.CloudStore.HasUserEnabledCloudSync();
		}

		// Token: 0x04000193 RID: 403
		private readonly CloudSyncFailureReporter _syncFailureReporter = new CloudSyncFailureReporter();
	}
}
