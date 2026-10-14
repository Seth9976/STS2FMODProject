using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Saves;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	/// <summary>
	/// Implementation of ISaveStore which saves files to Steam remote storage for syncing across devices.
	///
	/// Note that the usage of Steam API in here can be confusing. Here's what I've actually observed happens when we write
	/// files:
	/// - Steam ALWAYS writes the file to a directory on the local computer (C:/Program Files (x86)/Steam/userdata/[appid])
	/// - If the user has enabled steam cloud saves, then Steam additionally uploads them to the cloud
	///
	/// In addition, if the user re-enables steam cloud saves after having them disabled, then Steam immediately looks into
	/// the local userdata directory and tries to upload those files if they're there. So, while we always write to this
	/// save store while Steam is initialized. Whether those saves get uploaded to the cloud is up to the user's settings.
	///
	/// EXCEPTION BEHAVIOR: Methods in this class can fail in two ways:
	/// 1. Managed exceptions: Our wrapper code throws InvalidOperationException (write/read failures),
	///    FileNotFoundException (missing files), or SteamRemoteSaveStoreException (async read with non-OK EResult).
	///    The Steam API can return arbitrary EResult codes (see https://partner.steamgames.com/doc/api/steam_api#EResult),
	///    including unexpected ones after Cloud conflict resolution (e.g. k_EResultRemoteFileConflict, k_EResultBusy).
	/// 2. Native exceptions: Every method delegates through Steamworks.NET's NativeMethods P/Invoke layer into Valve's
	///    native steam_api DLL. If the native code faults, the CLR raises SEHException, which cannot be predicted or
	///    prevented from managed code.
	/// Callers must not assume a fixed set of exception types. All callers on startup-critical or gameplay paths
	/// should catch broadly to avoid fatal errors.
	/// </summary>
	// Token: 0x02000163 RID: 355
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamRemoteSaveStore : ICloudSaveStore, ISaveStore
	{
		// Token: 0x060010CC RID: 4300 RVA: 0x000637D4 File Offset: 0x000619D4
		public string ReadFile(string path)
		{
			path = this.CanonicalizePath(path);
			int fileSize = SteamRemoteStorage.GetFileSize(path);
			if (fileSize == 0)
			{
				if (!SteamRemoteStorage.FileExists(path))
				{
					throw new FileNotFoundException("Steam remote storage file not found: " + path);
				}
				return string.Empty;
			}
			else
			{
				byte[] array = new byte[fileSize];
				int num = SteamRemoteStorage.FileRead(path, array, fileSize);
				if (num == 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(109, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam remote storage read failed for ");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(". Expected ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(fileSize);
					defaultInterpolatedStringHandler.AppendLiteral(" bytes, got 0. Steam storage may be corrupted or unavailable.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (num != fileSize)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Steam read returned ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" bytes but expected ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(fileSize);
					defaultInterpolatedStringHandler.AppendLiteral(" for ");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(". Using partial data.");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				return Encoding.UTF8.GetString(array);
			}
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000638E0 File Offset: 0x00061AE0
		[return: Nullable(new byte[] { 1, 2 })]
		public async Task<string> ReadFileAsync(string path)
		{
			path = this.CanonicalizePath(path);
			int byteCount = SteamRemoteStorage.GetFileSize(path);
			string text;
			if (byteCount == 0)
			{
				text = string.Empty;
			}
			else
			{
				SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileReadAsync(path, 0U, (uint)byteCount);
				if (steamAPICall_t == SteamAPICall_t.Invalid)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam remote storage async read request returned invalid API call for path ");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(" (bytes ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(byteCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				using (SteamCallResult<RemoteStorageFileReadAsyncComplete_t> callResult = new SteamCallResult<RemoteStorageFileReadAsyncComplete_t>(steamAPICall_t, SteamInitializer.DisconnectToken))
				{
					RemoteStorageFileReadAsyncComplete_t remoteStorageFileReadAsyncComplete_t = await callResult.Task;
					RemoteStorageFileReadAsyncComplete_t remoteStorageFileReadAsyncComplete_t2 = remoteStorageFileReadAsyncComplete_t;
					if (remoteStorageFileReadAsyncComplete_t2.m_eResult != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Steam remote storage async read request returned error ");
						defaultInterpolatedStringHandler.AppendFormatted<EResult>(remoteStorageFileReadAsyncComplete_t2.m_eResult);
						defaultInterpolatedStringHandler.AppendLiteral(" for path ");
						defaultInterpolatedStringHandler.AppendFormatted(path);
						throw new SteamRemoteSaveStoreException(defaultInterpolatedStringHandler.ToStringAndClear(), remoteStorageFileReadAsyncComplete_t2.m_eResult);
					}
					byte[] array = new byte[byteCount];
					SteamRemoteStorage.FileReadAsyncComplete(remoteStorageFileReadAsyncComplete_t2.m_hFileReadAsync, array, remoteStorageFileReadAsyncComplete_t2.m_cubRead);
					text = Encoding.UTF8.GetString(array);
				}
			}
			return text;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0006392B File Offset: 0x00061B2B
		public void WriteFile(string path, string content)
		{
			this.WriteFile(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00063940 File Offset: 0x00061B40
		public void WriteFile(string path, byte[] bytes)
		{
			path = this.CanonicalizePath(path);
			if (!SteamRemoteStorage.FileWrite(path, bytes, bytes.Length))
			{
				throw new InvalidOperationException("Steam Cloud write failed. See ISteamRemoteStorage documentation for possible reasons.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Wrote ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(bytes.Length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes to ");
			defaultInterpolatedStringHandler.AppendFormatted(path);
			defaultInterpolatedStringHandler.AppendLiteral(" in steam remote store");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x000639BC File Offset: 0x00061BBC
		public Task WriteFileAsync(string path, string content)
		{
			return this.WriteFileAsync(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x000639D0 File Offset: 0x00061BD0
		public async Task WriteFileAsync(string path, byte[] bytes)
		{
			path = this.CanonicalizePath(path);
			SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileWriteAsync(path, bytes, (uint)bytes.Length);
			if (steamAPICall_t == SteamAPICall_t.Invalid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Steam remote storage async write request returned invalid API call for path ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(bytes.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes)");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			using (SteamCallResult<RemoteStorageFileWriteAsyncComplete_t> callResult = new SteamCallResult<RemoteStorageFileWriteAsyncComplete_t>(steamAPICall_t, SteamInitializer.DisconnectToken))
			{
				RemoteStorageFileWriteAsyncComplete_t remoteStorageFileWriteAsyncComplete_t = await callResult.Task;
				RemoteStorageFileWriteAsyncComplete_t remoteStorageFileWriteAsyncComplete_t2 = remoteStorageFileWriteAsyncComplete_t;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (remoteStorageFileWriteAsyncComplete_t2.m_eResult != 1)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Steam remote storage async write request returned error ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(remoteStorageFileWriteAsyncComplete_t2.m_eResult);
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Wrote ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(bytes.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes to ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(" in steam remote store");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00063A23 File Offset: 0x00061C23
		public bool FileExists(string path)
		{
			return SteamRemoteStorage.FileExists(this.CanonicalizePath(path));
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00063A31 File Offset: 0x00061C31
		public bool DirectoryExists(string path)
		{
			return true;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00063A34 File Offset: 0x00061C34
		public void DeleteFile(string path)
		{
			path = this.CanonicalizePath(path);
			bool flag = SteamRemoteStorage.FileExists(path);
			bool flag2 = SteamRemoteStorage.FileDelete(path);
			if (!flag2 && flag)
			{
				Log.Error("Steam remote storage delete FAILED for " + path + ". File existed but could not be deleted.", 2);
				return;
			}
			if (!flag2 && !flag)
			{
				Log.Debug("Steam delete called on non-existent file " + path + " (no-op)", 2);
				return;
			}
			Log.Debug("Deleted " + path + " from Steam remote storage", 2);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00063AAC File Offset: 0x00061CAC
		public void RenameFile(string sourcePath, string destinationPath)
		{
			sourcePath = this.CanonicalizePath(sourcePath);
			destinationPath = this.CanonicalizePath(destinationPath);
			string text = this.ReadFile(sourcePath);
			this.WriteFile(destinationPath, text);
			this.DeleteFile(sourcePath);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00063AE4 File Offset: 0x00061CE4
		public string[] GetFilesInDirectory(string directoryPath)
		{
			directoryPath = this.CanonicalizePath(directoryPath);
			int fileCount = SteamRemoteStorage.GetFileCount();
			List<string> list = new List<string>();
			for (int i = 0; i < fileCount; i++)
			{
				int num;
				string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, ref num);
				if (fileNameAndSize.StartsWith(directoryPath))
				{
					string text = fileNameAndSize.Substring(directoryPath.Length + 1);
					if (!text.Contains('/') && !text.Contains('\\'))
					{
						list.Add(text);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00063B58 File Offset: 0x00061D58
		public string[] GetDirectoriesInDirectory(string directoryPath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00063B5F File Offset: 0x00061D5F
		public void CreateDirectory(string directoryPath)
		{
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00063B61 File Offset: 0x00061D61
		public void DeleteDirectory(string directoryPath)
		{
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00063B63 File Offset: 0x00061D63
		public void DeleteTemporaryFiles(string directoryPath)
		{
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00063B65 File Offset: 0x00061D65
		public string GetFullPath(string filename)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00063B6C File Offset: 0x00061D6C
		public DateTimeOffset GetLastModifiedTime(string path)
		{
			path = this.CanonicalizePath(path);
			long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(path);
			return SaveTimestamps.FromUnixTimeSecondsOrEpoch(fileTimestamp, path);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00063B90 File Offset: 0x00061D90
		public int GetFileSize(string path)
		{
			path = this.CanonicalizePath(path);
			return SteamRemoteStorage.GetFileSize(path);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00063BA1 File Offset: 0x00061DA1
		public void SetLastModifiedTime(string path, DateTimeOffset time)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00063BA8 File Offset: 0x00061DA8
		public string CanonicalizePath(string path)
		{
			return path.Replace("user://", "").Replace("\\", "/");
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00063BC9 File Offset: 0x00061DC9
		public bool HasCloudFiles()
		{
			return SteamRemoteStorage.GetFileCount() > 0;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00063BD4 File Offset: 0x00061DD4
		public void ForgetFile(string path)
		{
			path = this.CanonicalizePath(path);
			if (!SteamRemoteStorage.FileForget(path))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to forget file at path ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(" from steam storage, but false was returned from ");
				defaultInterpolatedStringHandler.AppendFormatted("FileForget");
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00063C43 File Offset: 0x00061E43
		public bool IsFilePersisted(string path)
		{
			path = this.CanonicalizePath(path);
			return SteamRemoteStorage.FilePersisted(path);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00063C54 File Offset: 0x00061E54
		public void BeginSaveBatch()
		{
			if (!SteamRemoteStorage.BeginFileWriteBatch())
			{
				Log.Warn("SteamRemoteStorage.BeginFileWriteBatch returned false (a batch may already be in progress)", 2);
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00063C78 File Offset: 0x00061E78
		public void EndSaveBatch()
		{
			if (!SteamRemoteStorage.EndFileWriteBatch())
			{
				Log.Warn("SteamRemoteStorage.EndFileWriteBatch returned false (no batch was in progress)", 2);
			}
		}

		/// <summary>
		/// This controls whether files are synced from cloud to local.
		/// When the user disables steam cloud:
		///  - We still want to write files to the user's local steam cloud cache, to handle cloud conflicts later
		///  - But we never want to sync files from the user's steam cloud cache to our file storage.
		///
		/// In essence: If the user has enabled cloud sync, then Steam cloud is the authority. If the user has disabled
		/// cloud sync, then our local storage becomes the authority. But in both instances, we wish to keep the two
		/// storages in sync.
		///
		/// This allows players to turn off steam cloud sync in order to modify their save files.
		/// </summary>
		// Token: 0x060010E5 RID: 4325 RVA: 0x00063C99 File Offset: 0x00061E99
		public bool HasUserEnabledCloudSync()
		{
			return SteamRemoteStorage.IsCloudEnabledForAccount() && SteamRemoteStorage.IsCloudEnabledForApp();
		}
	}
}
