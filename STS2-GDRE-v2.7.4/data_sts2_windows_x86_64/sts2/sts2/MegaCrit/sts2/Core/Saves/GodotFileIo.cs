using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Implements the ISaveStore interface for managing game save files within Godot.
	/// Handles file operations including reading, writing, and managing save directories.
	/// All file I/O operations related to game saves should use this class to ensure
	/// proper path handling, atomic writes, and consistent error handling across the application.
	/// </summary>
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	[Nullable(0)]
	public class GodotFileIo : ISaveStore
	{
		/// <summary>
		/// WARNING: ONLY CHANGE THIS IN TESTS.
		/// The directory that your save files live at.
		/// On Windows, these will be at: C:\Users\{USER}\AppData\Roaming\SlayTheSpire2\{platform}\{userId}\profile{profileId}\saves
		/// </summary>
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x000174F0 File Offset: 0x000156F0
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x000174F8 File Offset: 0x000156F8
		public string SaveDir { get; set; }

		// Token: 0x06000508 RID: 1288 RVA: 0x00017501 File Offset: 0x00015701
		public GodotFileIo(string saveDir)
		{
			this.SaveDir = saveDir;
			this.CreateDirectory(this.SaveDir);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001751C File Offset: 0x0001571C
		public string GetFullPath(string filename)
		{
			if (filename.StartsWith(this.SaveDir))
			{
				return filename;
			}
			return this.SaveDir + "/" + filename;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00017540 File Offset: 0x00015740
		[return: Nullable(2)]
		public string ReadFile(string path)
		{
			path = this.GetFullPath(path);
			string text;
			using (FileAccess fileAccess = FileAccess.Open(path, 1L))
			{
				if (fileAccess == null)
				{
					Error openError = FileAccess.GetOpenError();
					if (openError != 7L)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Failed to open file for reading. path='");
						defaultInterpolatedStringHandler.AppendFormatted(path);
						defaultInterpolatedStringHandler.AppendLiteral("' error=");
						defaultInterpolatedStringHandler.AppendFormatted<Error>(openError);
						throw new SaveException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					Log.Warn("Tried to read file at " + path + ", but there was no such file", 2);
					text = null;
				}
				else
				{
					string asText = fileAccess.GetAsText(false);
					fileAccess.Close();
					text = asText;
				}
			}
			return text;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000175F4 File Offset: 0x000157F4
		[return: Nullable(new byte[] { 1, 2 })]
		public async Task<string> ReadFileAsync(string path)
		{
			path = this.GetFullPath(path);
			GodotFileIo.ValidateGodotFilePath(path);
			FileAccessStream stream = new FileAccessStream(path, 1L);
			object obj = null;
			int num = 0;
			string text;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					await stream.CopyToAsync(memoryStream);
					text = Encoding.UTF8.GetString(memoryStream.ToArray());
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			if (stream != null)
			{
				await stream.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			string text2;
			if (num == 1)
			{
				text2 = text;
			}
			else
			{
				obj = null;
				text = null;
				stream = null;
				MemoryStream memoryStream = null;
			}
			return text2;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001763F File Offset: 0x0001583F
		public DateTimeOffset GetLastModifiedTime(string path)
		{
			path = this.GetFullPath(path);
			return SaveTimestamps.FromUnixTimeSecondsOrEpoch((long)FileAccess.GetModifiedTime(path), path);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00017656 File Offset: 0x00015856
		public int GetFileSize(string path)
		{
			path = this.GetFullPath(path);
			return (int)FileAccess.GetSize(path);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00017668 File Offset: 0x00015868
		public void SetLastModifiedTime(string path, DateTimeOffset time)
		{
			path = this.GetFullPath(path);
			File.SetLastWriteTimeUtc(ProjectSettings.GlobalizePath(path), time.UtcDateTime);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00017685 File Offset: 0x00015885
		public void WriteFile(string path, string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				Log.Error("The content is empty for path='" + path + "'", 2);
				return;
			}
			this.WriteFile(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000176B8 File Offset: 0x000158B8
		public void WriteFile(string path, byte[] bytes)
		{
			path = this.GetFullPath(path);
			GodotFileIo.ValidateGodotFilePath(path);
			this.CopyBackup(path);
			string tempPath = path + ".tmp";
			FileWriteRetry.Run(tempPath, delegate
			{
				this.StoreBufferToFile(tempPath, bytes, path);
			}, null);
			GodotFileIo.FsyncFile(tempPath);
			this.RenameFile(tempPath, path);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Wrote ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(bytes.Length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes to path=");
			defaultInterpolatedStringHandler.AppendFormatted(path);
			defaultInterpolatedStringHandler.AppendLiteral(" save_dir=");
			defaultInterpolatedStringHandler.AppendFormatted(this.SaveDir);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000177B8 File Offset: 0x000159B8
		private void StoreBufferToFile(string tempPath, byte[] bytes, string path)
		{
			using (FileAccess fileAccess = FileAccess.Open(tempPath, 2L))
			{
				if (fileAccess == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to open file for writing. path='");
					defaultInterpolatedStringHandler.AppendFormatted(tempPath);
					defaultInterpolatedStringHandler.AppendLiteral("' error=");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
					throw new SaveException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!fileAccess.StoreBuffer(bytes))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to write ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(bytes.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" bytes to path=");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(" save_dir=");
					defaultInterpolatedStringHandler.AppendFormatted(this.SaveDir);
					defaultInterpolatedStringHandler.AppendLiteral(". Error: ");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(fileAccess.GetError());
					throw new SaveException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000178AC File Offset: 0x00015AAC
		public Task WriteFileAsync(string path, string content)
		{
			return this.WriteFileAsync(path, Encoding.UTF8.GetBytes(content));
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000178C0 File Offset: 0x00015AC0
		public async Task WriteFileAsync(string path, byte[] bytes)
		{
			GodotFileIo.<>c__DisplayClass15_0 CS$<>8__locals1 = new GodotFileIo.<>c__DisplayClass15_0();
			CS$<>8__locals1.bytes = bytes;
			path = this.GetFullPath(path);
			GodotFileIo.ValidateGodotFilePath(path);
			this.CopyBackup(path);
			CS$<>8__locals1.tempPath = path + ".tmp";
			CS$<>8__locals1.bytesWritten = 0L;
			await FileWriteRetry.RunAsync(CS$<>8__locals1.tempPath, delegate
			{
				GodotFileIo.<>c__DisplayClass15_0.<<WriteFileAsync>b__0>d <<WriteFileAsync>b__0>d;
				<<WriteFileAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<WriteFileAsync>b__0>d.<>4__this = CS$<>8__locals1;
				<<WriteFileAsync>b__0>d.<>1__state = -1;
				<<WriteFileAsync>b__0>d.<>t__builder.Start<GodotFileIo.<>c__DisplayClass15_0.<<WriteFileAsync>b__0>d>(ref <<WriteFileAsync>b__0>d);
				return <<WriteFileAsync>b__0>d.<>t__builder.Task;
			}, null);
			GodotFileIo.FsyncFile(CS$<>8__locals1.tempPath);
			this.RenameFile(CS$<>8__locals1.tempPath, path);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Wrote ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(CS$<>8__locals1.bytesWritten);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes to path=");
			defaultInterpolatedStringHandler.AppendFormatted(path);
			defaultInterpolatedStringHandler.AppendLiteral(" save_dir=");
			defaultInterpolatedStringHandler.AppendFormatted(this.SaveDir);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00017913 File Offset: 0x00015B13
		public bool FileExists(string path)
		{
			return FileAccess.FileExists(this.GetFullPath(path));
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00017921 File Offset: 0x00015B21
		public bool DirectoryExists(string path)
		{
			return DirAccess.DirExistsAbsolute(this.GetFullPath(path));
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00017930 File Offset: 0x00015B30
		public void DeleteFile(string path)
		{
			Error error = DirAccess.RemoveAbsolute(this.GetFullPath(path));
			if (error != null && error != 7L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error deleting path ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00017994 File Offset: 0x00015B94
		public void RenameFile(string sourcePath, string destinationPath)
		{
			if (!this.FileExists(sourcePath))
			{
				throw new SaveException("Cannot rename file: source does not exist. source=" + this.GetFullPath(sourcePath));
			}
			sourcePath = this.GetFullPath(sourcePath);
			destinationPath = this.GetFullPath(destinationPath);
			Error error = 1L;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			for (int i = 1; i <= 4; i++)
			{
				error = DirAccess.RenameAbsolute(sourcePath, destinationPath);
				if (error == null)
				{
					return;
				}
				if (FileAccess.FileExists(destinationPath) && !FileAccess.FileExists(sourcePath))
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Rename reported error=");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
					defaultInterpolatedStringHandler.AppendLiteral(" but destination exists, treating as success. source=");
					defaultInterpolatedStringHandler.AppendFormatted(sourcePath);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return;
				}
				if (i < 4)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Rename failed (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(4);
					defaultInterpolatedStringHandler.AppendLiteral("), retrying. error=");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
					defaultInterpolatedStringHandler.AppendLiteral(" source=");
					defaultInterpolatedStringHandler.AppendFormatted(sourcePath);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					Thread.Sleep(50);
				}
			}
			Thread.Sleep(100);
			if (FileAccess.FileExists(destinationPath) && !FileAccess.FileExists(sourcePath))
			{
				Log.Warn("Rename appeared to fail but destination exists after delay, treating as success. source=" + sourcePath, 2);
				return;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Failed to rename file. error=");
			defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
			defaultInterpolatedStringHandler.AppendLiteral(" source=");
			defaultInterpolatedStringHandler.AppendFormatted(sourcePath);
			defaultInterpolatedStringHandler.AppendLiteral(" destination=");
			defaultInterpolatedStringHandler.AppendFormatted(destinationPath);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("source_exists=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(FileAccess.FileExists(sourcePath));
			defaultInterpolatedStringHandler.AppendLiteral(" destination_exists=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(FileAccess.FileExists(destinationPath));
			throw new SaveException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00017B6F File Offset: 0x00015D6F
		public string[] GetFilesInDirectory(string directoryPath)
		{
			directoryPath = this.GetFullPath(directoryPath);
			return DirAccess.GetFilesAt(directoryPath);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00017B80 File Offset: 0x00015D80
		public string[] GetDirectoriesInDirectory(string directoryPath)
		{
			directoryPath = this.GetFullPath(directoryPath);
			return DirAccess.GetDirectoriesAt(directoryPath);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00017B91 File Offset: 0x00015D91
		public void CreateDirectory(string directoryPath)
		{
			directoryPath = this.GetFullPath(directoryPath);
			if (!DirAccess.DirExistsAbsolute(directoryPath))
			{
				DirAccess.MakeDirRecursiveAbsolute(directoryPath);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00017BAC File Offset: 0x00015DAC
		public void DeleteDirectory(string directoryPath)
		{
			directoryPath = this.GetFullPath(directoryPath);
			if (DirAccess.DirExistsAbsolute(directoryPath))
			{
				using (DirAccess dirAccess = DirAccess.Open(directoryPath))
				{
					dirAccess.IncludeHidden = true;
					foreach (string text in dirAccess.GetFiles())
					{
						Error error = dirAccess.Remove(text);
						if (error != null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Got error ");
							defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
							defaultInterpolatedStringHandler.AppendLiteral(" trying to delete file ");
							defaultInterpolatedStringHandler.AppendFormatted(text);
							defaultInterpolatedStringHandler.AppendLiteral(" in directory ");
							defaultInterpolatedStringHandler.AppendFormatted(directoryPath);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					foreach (string text2 in dirAccess.GetDirectories())
					{
						this.DeleteDirectory(directoryPath + "/" + text2);
					}
					Error error2 = dirAccess.Remove("");
					if (error2 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Got error ");
						defaultInterpolatedStringHandler.AppendFormatted<Error>(error2);
						defaultInterpolatedStringHandler.AppendLiteral(" trying to delete directory ");
						defaultInterpolatedStringHandler.AppendFormatted(directoryPath);
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00017D04 File Offset: 0x00015F04
		public void DeleteTemporaryFiles(string directoryPath)
		{
			directoryPath = this.GetFullPath(directoryPath);
			using (DirAccess dirAccess = DirAccess.Open(directoryPath))
			{
				if (dirAccess != null)
				{
					foreach (string text in dirAccess.GetFiles())
					{
						if (text.EndsWith(".tmp"))
						{
							Log.Info("Cleaning up orphaned " + text + " in " + directoryPath, 2);
							Error error = dirAccess.Remove(text);
							if (error != null)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Couldn't delete temporary file ");
								defaultInterpolatedStringHandler.AppendFormatted(text);
								defaultInterpolatedStringHandler.AppendLiteral(" in ");
								defaultInterpolatedStringHandler.AppendFormatted(directoryPath);
								defaultInterpolatedStringHandler.AppendLiteral(", error=");
								defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
								Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Copies the current save to a .backup file using temp+rename for crash safety.
		/// The .backup serves as a fallback for the non-atomic rename on Windows and for
		/// recovery when the primary save is corrupted. Writing to a .tmp file first ensures
		/// the old .backup is preserved if a crash occurs mid-write.
		/// </summary>
		// Token: 0x0600051D RID: 1309 RVA: 0x00017DE8 File Offset: 0x00015FE8
		private void CopyBackup(string fullPath)
		{
			if (!FileAccess.FileExists(fullPath))
			{
				return;
			}
			string text = fullPath + ".backup";
			string text2 = fullPath + ".backup.tmp";
			using (FileAccess fileAccess = FileAccess.Open(fullPath, 1L))
			{
				if (fileAccess == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to open source for backup copy. path=");
					defaultInterpolatedStringHandler.AppendFormatted(fullPath);
					defaultInterpolatedStringHandler.AppendLiteral(" error=");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					byte[] buffer = fileAccess.GetBuffer((long)fileAccess.GetLength());
					fileAccess.Close();
					using (FileAccess fileAccess2 = FileAccess.Open(text2, 2L))
					{
						if (fileAccess2 == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Failed to open backup for writing. path=");
							defaultInterpolatedStringHandler.AppendFormatted(text2);
							defaultInterpolatedStringHandler.AppendLiteral(" error=");
							defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						}
						else if (!fileAccess2.StoreBuffer(buffer))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Copying backup from ");
							defaultInterpolatedStringHandler.AppendFormatted(fullPath);
							defaultInterpolatedStringHandler.AppendLiteral(" to ");
							defaultInterpolatedStringHandler.AppendFormatted(text2);
							defaultInterpolatedStringHandler.AppendLiteral(" failed: ");
							defaultInterpolatedStringHandler.AppendFormatted<Error>(fileAccess2.GetError());
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						}
						else
						{
							fileAccess2.Close();
							try
							{
								GodotFileIo.FsyncFile(text2);
								this.RenameFile(text2, text);
							}
							catch (Exception ex)
							{
								Log.Warn("Failed to finalize backup for " + fullPath + ": " + ex.Message, 2);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Forces the OS to flush all buffered data for the specified file to the storage device.
		/// Without this, data may sit in the OS page cache and be lost on power loss or OS crash.
		///
		/// Why .NET FileStream instead of Godot's FileAccess.Flush()?
		/// Godot's Flush() calls fflush() (see file_access_unix.cpp:322 and file_access_windows.cpp:374)
		/// which only pushes data from the C stdio buffer to the OS kernel page cache. Godot does not
		/// expose fsync/fdatasync/FlushFileBuffers anywhere in its FileAccess API. .NET's Flush(flushToDisk: true)
		/// calls fsync (Linux) / FlushFileBuffers (Windows) which forces the kernel to write to the physical device.
		///
		/// This is a platform-dependent call and may not work on all OSes (e.g. consoles with sandboxed
		/// filesystems). On unsupported platforms, the catch block logs a warning and the save proceeds
		/// without durability guarantees (same as before this change). Console ports will need a
		/// platform-specific ISaveStore implementation in C# anyway, which can use that platform's
		/// native flush/commit API directly.
		/// </summary>
		// Token: 0x0600051E RID: 1310 RVA: 0x00017FD8 File Offset: 0x000161D8
		private static void FsyncFile(string godotPath)
		{
			try
			{
				string text = ProjectSettings.GlobalizePath(godotPath);
				using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
				{
					fileStream.Flush(true);
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Failed to fsync " + godotPath + ": " + ex.Message, 2);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00018048 File Offset: 0x00016248
		private static void ValidateGodotFilePath(string godotFilePath)
		{
			if (!godotFilePath.Contains("://"))
			{
				throw new SaveException("The path='" + godotFilePath + "' is not a godot file path");
			}
			string baseDir = StringExtensions.GetBaseDir(godotFilePath);
			if (!DirAccess.DirExistsAbsolute(baseDir))
			{
				DirAccess.MakeDirRecursiveAbsolute(baseDir);
			}
		}
	}
}
