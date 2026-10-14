using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF1 RID: 3313
	[NullableContext(1)]
	[Nullable(0)]
	public class GetLogsConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024CC RID: 9420
		// (get) Token: 0x060085F8 RID: 34296 RVA: 0x002CF3EC File Offset: 0x002CD5EC
		public override string CmdName
		{
			get
			{
				return "getlogs";
			}
		}

		// Token: 0x170024CD RID: 9421
		// (get) Token: 0x060085F9 RID: 34297 RVA: 0x002CF3F3 File Offset: 0x002CD5F3
		public override string Args
		{
			get
			{
				return "[test-feedback] <name:string>";
			}
		}

		// Token: 0x170024CE RID: 9422
		// (get) Token: 0x060085FA RID: 34298 RVA: 0x002CF3FA File Offset: 0x002CD5FA
		public override string Description
		{
			get
			{
				return "Gathers logs, automatically zips them to a file containing 'name', and opens the directory containing the zip file.";
			}
		}

		// Token: 0x170024CF RID: 9423
		// (get) Token: 0x060085FB RID: 34299 RVA: 0x002CF401 File Offset: 0x002CD601
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024D0 RID: 9424
		// (get) Token: 0x060085FC RID: 34300 RVA: 0x002CF404 File Offset: 0x002CD604
		public override bool DebugOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x002CF408 File Offset: 0x002CD608
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			string text = "";
			bool flag = false;
			if (args.Length != 0)
			{
				if (args[0] == "test-feedback")
				{
					flag = true;
				}
				else
				{
					text = "-" + args[0];
				}
			}
			string bugReportPath = GetLogsConsoleCmd.GetBugReportPath(text);
			if (flag)
			{
				using (FileAccessStream fileAccessStream = new FileAccessStream(bugReportPath, 2L))
				{
					GetLogsConsoleCmd.ZipFeedbackLogs(fileAccessStream, SaveManager.Instance.CurrentProfileId);
					goto IL_0068;
				}
			}
			TaskHelper.RunSafely(GetLogsConsoleCmd.GrabLogs(bugReportPath));
			IL_0068:
			return new CmdResult(true, "Zipping files to '" + bugReportPath + "'...");
		}

		// Token: 0x060085FE RID: 34302 RVA: 0x002CF4A4 File Offset: 0x002CD6A4
		public static string GetBugReportPath(string extraName = "")
		{
			string userDataDir = OS.GetUserDataDir();
			if (userDataDir == null)
			{
				throw new InvalidOperationException("Unable to open the user data directory.");
			}
			string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
			string text2 = userDataDir;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("BugReport");
			defaultInterpolatedStringHandler.AppendFormatted(extraName);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(".zip");
			return Path.Combine(text2, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x002CF528 File Offset: 0x002CD728
		public static async Task GrabLogs(string bugReportPath)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				bugReportPath = bugReportPath.Replace('/', '\\');
			}
			bool consoleVisible = NDevConsole.Instance.Visible;
			if (consoleVisible)
			{
				NDevConsole.Instance.HideConsole();
				await NDevConsole.Instance.AwaitProcessFrame(default(CancellationToken));
				await NDevConsole.Instance.AwaitProcessFrame(default(CancellationToken));
			}
			Image image = NDevConsole.Instance.GetViewport().GetTexture().GetImage();
			if (consoleVisible)
			{
				NDevConsole.Instance.ShowConsole();
			}
			FileStream fileStream = new FileStream(bugReportPath, FileMode.CreateNew);
			object obj = null;
			try
			{
				GetLogsConsoleCmd.ZipFiles(fileStream, image.SavePngToBuffer());
			}
			catch (object obj)
			{
			}
			if (fileStream != null)
			{
				await fileStream.DisposeAsync();
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
			obj = null;
			Error error = OS.ShellShowInFileManager(ProjectSettings.GlobalizePath(bugReportPath), true);
			if (error != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
				defaultInterpolatedStringHandler.AppendLiteral(": Cannot open OS file manager. Files zipped to '");
				defaultInterpolatedStringHandler.AppendFormatted(bugReportPath);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				Log.Info("Files zipped to '" + bugReportPath + "'", 2);
			}
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x002CF56C File Offset: 0x002CD76C
		public static void ZipFiles(Stream outputStream, byte[] screenshotBytes)
		{
			if (RunManager.Instance.IsInProgress && RunManager.Instance.CombatReplayWriter.IsRecordingReplay)
			{
				RunManager.Instance.WriteReplay(false);
			}
			string baseDir = StringExtensions.GetBaseDir(OS.GetExecutablePath());
			string text = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath("", null, null));
			string text2 = ProjectSettings.GlobalizePath("user://");
			string text3 = Path.Combine(text2, "logs");
			string text4 = Path.Combine(baseDir, "release_info.json");
			string text5 = Path.Combine(text2, "sentry", "reports");
			using (ZipArchive zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
			{
				if (Directory.Exists(text3))
				{
					foreach (string text6 in Directory.EnumerateFiles(text3, "*", SearchOption.AllDirectories))
					{
						string relativePath = Path.GetRelativePath(text2, text6);
						GetLogsConsoleCmd.ArchiveLogFile(text6, zipArchive, relativePath, 8388608L);
					}
				}
				List<string> list = new List<string>();
				if (Directory.Exists(text5))
				{
					list.AddRange(Directory.GetFiles(text5));
				}
				if (OS.GetName() == "Windows")
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					string text7 = Path.Combine(folderPath, "CrashDumps");
					if (Directory.Exists(text7))
					{
						string exeName = Path.GetFileNameWithoutExtension(OS.GetExecutablePath());
						list.AddRange(from f in Directory.GetFiles(text7)
							where Path.GetFileName(f).StartsWith(exeName, StringComparison.OrdinalIgnoreCase)
							select f);
					}
				}
				if (list.Count > 0)
				{
					IEnumerable<string> enumerable = list;
					Func<string, DateTime> func;
					if ((func = GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime) == null)
					{
						func = (GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime));
					}
					string text8 = enumerable.OrderByDescending<string, DateTime>(func).First<string>();
					DateTimeOffset dateTimeOffset = File.GetLastWriteTime(text8);
					string text9 = "yyyy-MM-dd_HH-mm-ss";
					GetLogsConsoleCmd.ArchiveFile(text8, zipArchive, "crashes/crash_" + dateTimeOffset.ToString(text9) + Path.GetExtension(text8));
				}
				GetLogsConsoleCmd.TryCollectLinuxCoreDump(zipArchive);
				foreach (string text10 in GetLogsConsoleCmd.GetAllSaveFiles(text))
				{
					string text11 = "saves/" + Path.GetRelativePath(text, text10);
					if (Path.GetExtension(text10).Equals(".json", StringComparison.OrdinalIgnoreCase))
					{
						GetLogsConsoleCmd.ArchiveLogFile(text10, zipArchive, text11, 0L);
					}
					else
					{
						GetLogsConsoleCmd.ArchiveFile(text10, zipArchive, text11);
					}
				}
				if (File.Exists(text4))
				{
					GetLogsConsoleCmd.ArchiveFile(text4, zipArchive, "release_info.json");
				}
				GetLogsConsoleCmd.ArchiveBytes(screenshotBytes, zipArchive, "screenshot.png");
			}
		}

		/// <summary>
		/// Builds a ZIP of logs and saves tailored for the feedback upload path.
		/// Unlike <see cref="M:MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.GetLogsConsoleCmd.ZipFiles(System.IO.Stream,System.Byte[])" />, this excludes the screenshot (already sent as a separate multipart part)
		/// and inactive profiles. Only the most recent run history file is included.
		/// </summary>
		// Token: 0x06008601 RID: 34305 RVA: 0x002CF850 File Offset: 0x002CDA50
		public unsafe static void ZipFeedbackLogs(Stream outputStream, int profileId)
		{
			if (RunManager.Instance.IsInProgress && RunManager.Instance.CombatReplayWriter.IsRecordingReplay)
			{
				RunManager.Instance.WriteReplay(false);
			}
			string baseDir = StringExtensions.GetBaseDir(OS.GetExecutablePath());
			string text = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath("", null, null));
			string text2 = ProjectSettings.GlobalizePath("user://");
			string text3 = Path.Combine(text2, "logs");
			string text4 = Path.Combine(baseDir, "release_info.json");
			string text5 = Path.Combine(text2, "sentry", "reports");
			using (ZipArchive zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
			{
				if (Directory.Exists(text3))
				{
					foreach (string text6 in Directory.EnumerateFiles(text3, "*", SearchOption.AllDirectories))
					{
						string relativePath = Path.GetRelativePath(text2, text6);
						GetLogsConsoleCmd.ArchiveLogFile(text6, zipArchive, relativePath, 8388608L);
					}
				}
				List<string> list = new List<string>();
				if (Directory.Exists(text5))
				{
					list.AddRange(Directory.GetFiles(text5));
				}
				if (OS.GetName() == "Windows")
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					string text7 = Path.Combine(folderPath, "CrashDumps");
					if (Directory.Exists(text7))
					{
						string exeName = Path.GetFileNameWithoutExtension(OS.GetExecutablePath());
						list.AddRange(from f in Directory.GetFiles(text7)
							where Path.GetFileName(f).StartsWith(exeName, StringComparison.OrdinalIgnoreCase)
							select f);
					}
				}
				if (list.Count > 0)
				{
					IEnumerable<string> enumerable = list;
					Func<string, DateTime> func;
					if ((func = GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime) == null)
					{
						func = (GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime));
					}
					string text8 = enumerable.OrderByDescending<string, DateTime>(func).First<string>();
					DateTimeOffset dateTimeOffset = File.GetLastWriteTime(text8);
					string text9 = "yyyy-MM-dd_HH-mm-ss";
					long length = new FileInfo(text8).Length;
					if (length <= 52428800L)
					{
						GetLogsConsoleCmd.ArchiveFile(text8, zipArchive, "crashes/crash_" + dateTimeOffset.ToString(text9) + Path.GetExtension(text8));
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Crash dump too large for feedback upload (");
						defaultInterpolatedStringHandler.AppendFormatted<long>(length / 1048576L);
						defaultInterpolatedStringHandler.AppendLiteral(" MB), skipping: ");
						defaultInterpolatedStringHandler.AppendFormatted(text8);
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
				GetLogsConsoleCmd.TryCollectLinuxCoreDump(zipArchive);
				int num = 7;
				List<string> list2 = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list2, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list2);
				int num2 = 0;
				*span[num2] = ProfileSaveManager.GetProfileSavePath(null);
				num2++;
				*span[num2] = "settings.save";
				num2++;
				*span[num2] = ProgressSaveManager.GetProgressPathForProfile(profileId, null);
				num2++;
				*span[num2] = RunSaveManager.GetRunSavePath(profileId, "current_run.save", null);
				num2++;
				*span[num2] = RunSaveManager.GetRunSavePath(profileId, "current_run_mp.save", null);
				num2++;
				*span[num2] = PrefsSaveManager.GetPrefsPath(profileId, null);
				num2++;
				*span[num2] = Path.Combine(UserDataPathProvider.GetProfileDir(profileId), "replays/latest.mcr");
				List<string> list3 = list2;
				string text10 = Path.Combine(text, RunHistorySaveManager.GetHistoryPath(profileId, null));
				if (Directory.Exists(text10))
				{
					IEnumerable<string> enumerable2 = Directory.EnumerateFiles(text10, "*", SearchOption.TopDirectoryOnly);
					Func<string, DateTime> func2;
					if ((func2 = GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime) == null)
					{
						func2 = (GetLogsConsoleCmd.<>O.<0>__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime));
					}
					string text11 = enumerable2.OrderByDescending<string, DateTime>(func2).FirstOrDefault<string>();
					if (text11 != null)
					{
						list3.Add(Path.GetRelativePath(text, text11));
					}
				}
				foreach (string text12 in GetLogsConsoleCmd.GetAllSaveFiles(text))
				{
					if (text12.EndsWith(".corrupt") && DateTimeOffset.Now - File.GetLastWriteTime(text12) < TimeSpan.FromDays(1))
					{
						list3.Add(Path.GetRelativePath(text, text12));
					}
				}
				foreach (string text13 in list3)
				{
					string text14 = Path.Combine(text, text13);
					if (File.Exists(text14))
					{
						string text15 = "saves/" + text13;
						if (Path.GetExtension(text14).Equals(".json", StringComparison.OrdinalIgnoreCase))
						{
							GetLogsConsoleCmd.ArchiveLogFile(text14, zipArchive, text15, 0L);
						}
						else
						{
							GetLogsConsoleCmd.ArchiveFile(text14, zipArchive, text15);
						}
					}
				}
				if (File.Exists(text4))
				{
					GetLogsConsoleCmd.ArchiveFile(text4, zipArchive, "release_info.json");
				}
			}
		}

		/// <summary>
		/// Best-effort collection of Linux core dumps via systemd-coredump.
		/// Silently does nothing if coredumpctl is unavailable or no dumps are found.
		/// </summary>
		// Token: 0x06008602 RID: 34306 RVA: 0x002CFD80 File Offset: 0x002CDF80
		private static void TryCollectLinuxCoreDump(ZipArchive archive)
		{
			if (OS.GetName() != "Linux")
			{
				return;
			}
			string fileName = Path.GetFileName(OS.GetExecutablePath());
			try
			{
				using (Process process = new Process())
				{
					process.StartInfo = new ProcessStartInfo
					{
						FileName = "coredumpctl",
						Arguments = "info -1 --no-pager " + fileName,
						RedirectStandardOutput = true,
						UseShellExecute = false
					};
					process.Start();
					Task<string> task = process.StandardOutput.ReadToEndAsync();
					if (!task.Wait(10000))
					{
						process.Kill();
						process.WaitForExit(5000);
						Log.Warn("coredumpctl info timed out after 10s", 2);
					}
					else
					{
						string result = task.Result;
						process.WaitForExit();
						if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(result))
						{
							ZipArchiveEntry zipArchiveEntry = archive.CreateEntry("crashes/coredump_info.txt");
							using (StreamWriter streamWriter = new StreamWriter(zipArchiveEntry.Open()))
							{
								streamWriter.Write(result);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Could not collect coredumpctl info: " + ex.Message, 2);
			}
			string tempPath = Path.GetTempPath();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("sts2_coredump_");
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(Guid.NewGuid());
			defaultInterpolatedStringHandler.AppendLiteral(".core");
			string text = Path.Combine(tempPath, defaultInterpolatedStringHandler.ToStringAndClear());
			try
			{
				using (Process process2 = new Process())
				{
					process2.StartInfo = new ProcessStartInfo
					{
						FileName = "coredumpctl",
						Arguments = "dump -1 --no-pager -o \"" + text + "\" " + fileName,
						UseShellExecute = false
					};
					process2.Start();
					bool flag = !process2.WaitForExit(10000);
					if (flag)
					{
						Log.Warn("coredumpctl dump timed out after 10s, killing process", 2);
						process2.Kill();
						process2.WaitForExit(5000);
					}
					else if (process2.ExitCode != 0)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
						defaultInterpolatedStringHandler.AppendLiteral("coredumpctl dump exited with code ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(process2.ExitCode);
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
					if (File.Exists(text))
					{
						long length = new FileInfo(text).Length;
						if (length > 209715200L)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Core dump is ");
							defaultInterpolatedStringHandler.AppendFormatted<long>(length / 1048576L);
							defaultInterpolatedStringHandler.AppendLiteral(" MB which exceeds the ");
							defaultInterpolatedStringHandler.AppendFormatted<long>(200L);
							defaultInterpolatedStringHandler.AppendLiteral(" MB limit, skipping");
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						}
						else if (length > 0L)
						{
							GetLogsConsoleCmd.ArchiveFile(text, archive, "crashes/coredump.core");
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Warn("Could not collect core dump: " + ex2.Message, 2);
			}
			finally
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Returns all save files in the account directory.
		/// Includes everything to ensure we capture info that might give clues for debugging,
		/// including old migrations or other unexpected files.
		/// </summary>
		// Token: 0x06008603 RID: 34307 RVA: 0x002D0104 File Offset: 0x002CE304
		private static IEnumerable<string> GetAllSaveFiles(string accountBasePath)
		{
			GetLogsConsoleCmd.<GetAllSaveFiles>d__20 <GetAllSaveFiles>d__ = new GetLogsConsoleCmd.<GetAllSaveFiles>d__20(-2);
			<GetAllSaveFiles>d__.<>3__accountBasePath = accountBasePath;
			return <GetAllSaveFiles>d__;
		}

		/// <summary>
		/// Reads text from a stream, truncating to the tail if it exceeds <paramref name="maxBytes" />.
		/// When truncated, skips past any partial UTF-8 character and partial line at the seek point,
		/// then prepends a truncation marker.
		/// </summary>
		/// <param name="stream">A readable, seekable stream containing UTF-8 text.</param>
		/// <param name="maxBytes">Maximum bytes to read from the tail. 0 means no limit.</param>
		/// <returns>The full or truncated text content.</returns>
		// Token: 0x06008604 RID: 34308 RVA: 0x002D0114 File Offset: 0x002CE314
		public static string ReadTailText(Stream stream, long maxBytes)
		{
			bool flag = false;
			if (maxBytes > 0L && stream.Length > maxBytes)
			{
				stream.Seek(stream.Length - maxBytes, SeekOrigin.Begin);
				while (stream.Position < stream.Length)
				{
					int num = stream.ReadByte();
					if (num == -1)
					{
						break;
					}
					if ((num & 192) != 128)
					{
						stream.Seek(-1L, SeekOrigin.Current);
						break;
					}
				}
				flag = true;
			}
			string text2;
			using (StreamReader streamReader = new StreamReader(stream, null, true, -1, true))
			{
				string text = streamReader.ReadToEnd();
				if (flag)
				{
					int num2 = text.IndexOf('\n');
					if (num2 >= 0)
					{
						text2 = text;
						int num3 = num2 + 1;
						text = text2.Substring(num3, text2.Length - num3);
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("[...truncated, showing last ~");
					defaultInterpolatedStringHandler.AppendFormatted<long>(maxBytes / 1048576L);
					defaultInterpolatedStringHandler.AppendLiteral(" MB...]\n");
					text = defaultInterpolatedStringHandler.ToStringAndClear() + text;
				}
				text2 = text;
			}
			return text2;
		}

		// Token: 0x06008605 RID: 34309 RVA: 0x002D021C File Offset: 0x002CE41C
		private static void ArchiveLogFile(string file, ZipArchive archive, string entryName, long maxBytes)
		{
			entryName = entryName.Replace("\\", "/");
			ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(entryName);
			try
			{
				using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					string text = GetLogsConsoleCmd.ReadTailText(fileStream, maxBytes);
					string text2 = LogSanitizer.Sanitize(text);
					using (StreamWriter streamWriter = new StreamWriter(zipArchiveEntry.Open()))
					{
						streamWriter.Write(text2);
					}
				}
			}
			catch (FileNotFoundException)
			{
				Log.Error("Could not find file for zipping: " + file, 2);
			}
		}

		// Token: 0x06008606 RID: 34310 RVA: 0x002D02CC File Offset: 0x002CE4CC
		private static void ArchiveFile(string file, ZipArchive archive, string entryName)
		{
			entryName = entryName.Replace("\\", "/");
			ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(entryName);
			try
			{
				using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					using (Stream stream = zipArchiveEntry.Open())
					{
						fileStream.CopyTo(stream);
					}
				}
			}
			catch (FileNotFoundException)
			{
				Log.Error("Could not find file for zipping: " + file, 2);
			}
		}

		// Token: 0x06008607 RID: 34311 RVA: 0x002D0364 File Offset: 0x002CE564
		private static void ArchiveBytes(byte[] bytes, ZipArchive archive, string entryName)
		{
			ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(entryName);
			using (Stream stream = zipArchiveEntry.Open())
			{
				stream.Write(bytes);
			}
		}

		// Token: 0x04002E65 RID: 11877
		private const string _dateTimeSpecifier = "yyyy_MM_dd_HH_mm_ss";

		// Token: 0x04002E66 RID: 11878
		private const long _maxLogFileBytes = 8388608L;

		// Token: 0x04002E67 RID: 11879
		private const long _maxFeedbackCrashDumpBytes = 52428800L;

		// Token: 0x04002E68 RID: 11880
		private const long _maxCoreDumpBytes = 209715200L;

		// Token: 0x020023F0 RID: 9200
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040097EA RID: 38890
			[Nullable(0)]
			public static Func<string, DateTime> <0>__GetLastWriteTime;
		}
	}
}
