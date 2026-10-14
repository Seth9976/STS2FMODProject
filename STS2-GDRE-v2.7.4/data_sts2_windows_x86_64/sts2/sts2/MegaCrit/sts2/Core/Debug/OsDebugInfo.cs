using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Logging;
using SharpGen.Runtime;
using Vortice.DXGI;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D11 RID: 3345
	[NullableContext(1)]
	[Nullable(0)]
	public static class OsDebugInfo
	{
		/// <summary>
		/// At some point in the future, logs system info.
		/// Getting system info takes a little while, about 1s on my Windows machine. This fetches the info asynchronously
		/// and then logs it once it's all done.
		/// Only logs on release builds to avoid cluttering dev logs.
		/// </summary>
		// Token: 0x060086FE RID: 34558 RVA: 0x002D5490 File Offset: 0x002D3690
		public static async Task LogSystemInfo()
		{
			if (ReleaseInfoManager.Instance.ReleaseInfo != null)
			{
				string systemInfo = "Fetching system info failed!";
				await Task.Run<string>(() => systemInfo = OsDebugInfo.GetSystemInfoString());
				Log.Info(systemInfo, 2);
			}
		}

		/// <summary>
		/// Gets a string containing diagnostic system info for logging.
		/// </summary>
		// Token: 0x060086FF RID: 34559 RVA: 0x002D54CC File Offset: 0x002D36CC
		public unsafe static string GetSystemInfoString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=== Godot OS Debug Information ===");
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Timestamp: ");
			appendInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now);
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("OS: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetName());
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("OS Version: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetVersion());
			stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder6 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Distribution Name: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetDistributionName());
			stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder7 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Device Model: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetModelName());
			stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder8 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is Debug Build: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(OS.IsDebugBuild());
			stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder9 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is Sandboxed: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(OS.IsSandboxed());
			stringBuilder9.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder10 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Executable Path: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetExecutablePath());
			stringBuilder10.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder11 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Data Directory: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetDataDir());
			stringBuilder11.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder12 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("User Data Directory: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetUserDataDir());
			stringBuilder12.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder13 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Command Line Args: ");
			appendInterpolatedStringHandler.AppendFormatted(string.Join(" ", OS.GetCmdlineArgs()));
			stringBuilder13.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder14 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("User Command Line Args: ");
			appendInterpolatedStringHandler.AppendFormatted(string.Join(" ", OS.GetCmdlineUserArgs()));
			stringBuilder14.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder15 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("OS Locale: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetLocale());
			stringBuilder15.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder16 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("OS Language: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetLocaleLanguage());
			stringBuilder16.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder17 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Game Locale: ");
			appendInterpolatedStringHandler.AppendFormatted(TranslationServer.GetLocale());
			stringBuilder17.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder18 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is UserFS Persistent: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(OS.IsUserfsPersistent());
			stringBuilder18.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder19 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is Stdout Verbose: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(OS.IsStdOutVerbose());
			stringBuilder19.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder20 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(29, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is Low Processor Usage Mode: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(OS.IsInLowProcessorUsageMode());
			stringBuilder20.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder21 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Architecture: ");
			appendInterpolatedStringHandler.AppendFormatted(Engine.GetArchitectureName());
			stringBuilder21.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder22 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Engine Version: ");
			appendInterpolatedStringHandler.AppendFormatted<Variant>(Engine.GetVersionInfo()["string"]);
			stringBuilder22.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder23 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Is Editor: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(Engine.IsEditorHint());
			stringBuilder23.AppendLine(ref appendInterpolatedStringHandler);
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			if (releaseInfo != null)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder24 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Release Version: ");
				appendInterpolatedStringHandler.AppendFormatted(releaseInfo.Version);
				stringBuilder24.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder25 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Release Commit: ");
				appendInterpolatedStringHandler.AppendFormatted(releaseInfo.Commit);
				stringBuilder25.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder26 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Release Date: ");
				appendInterpolatedStringHandler.AppendFormatted<DateTime>(releaseInfo.Date);
				stringBuilder26.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder27 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Processor Count: ");
			appendInterpolatedStringHandler.AppendFormatted<int>(OS.GetProcessorCount());
			stringBuilder27.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder28 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Processor Name: ");
			appendInterpolatedStringHandler.AppendFormatted(OS.GetProcessorName());
			stringBuilder28.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder29 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Main assembly hash: ");
			appendInterpolatedStringHandler.AppendFormatted<int>(AssemblyHasher.GetMainAssemblyHash());
			stringBuilder29.AppendLine(ref appendInterpolatedStringHandler);
			RenderingDevice renderingDevice = RenderingServer.GetRenderingDevice();
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder30 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Rendering device name: ");
			appendInterpolatedStringHandler.AppendFormatted(((renderingDevice != null) ? renderingDevice.GetDeviceName() : null) ?? "N/A (headless)");
			stringBuilder30.AppendLine(ref appendInterpolatedStringHandler);
			IDXGIFactory1 idxgifactory;
			if (DXGI.CreateDXGIFactory1<IDXGIFactory1>(out idxgifactory) == Result.Ok)
			{
				uint num = 0U;
				IDXGIAdapter1 idxgiadapter;
				while (idxgifactory.EnumAdapters1(num, out idxgiadapter) == Result.Ok)
				{
					long num2;
					idxgiadapter.CheckInterfaceSupport(typeof(IDXGIDevice), out num2);
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder31 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(21, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("  Graphics adapter ");
					appendInterpolatedStringHandler.AppendFormatted<uint>(num);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted(idxgiadapter.Description.Description);
					stringBuilder31.AppendLine(ref appendInterpolatedStringHandler);
					StringBuilder stringBuilder32 = stringBuilder;
					string text = "  version: {0}.{1}.{2}.{3}";
					<>y__InlineArray4<object> <>y__InlineArray = default(<>y__InlineArray4<object>);
					*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<object>, object>(ref <>y__InlineArray, 0) = num2 >> 48;
					*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<object>, object>(ref <>y__InlineArray, 1) = (num2 >> 32) & 65535L;
					*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<object>, object>(ref <>y__InlineArray, 2) = (num2 >> 16) & 65535L;
					*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<object>, object>(ref <>y__InlineArray, 3) = (num2 >> 32) & 65535L;
					stringBuilder32.AppendLine(string.Format(text, <PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray4<object>, object>(in <>y__InlineArray, 4)));
					num += 1U;
				}
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder33 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Screen info (primary ");
			appendInterpolatedStringHandler.AppendFormatted<int>(DisplayServer.GetPrimaryScreen());
			appendInterpolatedStringHandler.AppendLiteral("):");
			stringBuilder33.AppendLine(ref appendInterpolatedStringHandler);
			for (int i = 0; i < DisplayServer.GetScreenCount(); i++)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder34 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(59, 6, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Index ");
				appendInterpolatedStringHandler.AppendFormatted<int>(i);
				appendInterpolatedStringHandler.AppendLiteral(": Size: ");
				appendInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.ScreenGetSize(i));
				appendInterpolatedStringHandler.AppendLiteral(" Orientation: ");
				appendInterpolatedStringHandler.AppendFormatted<DisplayServer.ScreenOrientation>(DisplayServer.ScreenGetOrientation(i));
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendLiteral("Scale: ");
				appendInterpolatedStringHandler.AppendFormatted<float>(DisplayServer.ScreenGetScale(i));
				appendInterpolatedStringHandler.AppendLiteral(" DPI: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(DisplayServer.ScreenGetDpi(-1));
				appendInterpolatedStringHandler.AppendLiteral(" Refresh Rate: ");
				appendInterpolatedStringHandler.AppendFormatted<float>(DisplayServer.ScreenGetRefreshRate(-1));
				stringBuilder34.AppendLine(ref appendInterpolatedStringHandler);
			}
			ulong renderingInfo = RenderingServer.GetRenderingInfo(5L);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder35 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Video Memory Used: ");
			appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes(renderingInfo));
			stringBuilder35.AppendLine(ref appendInterpolatedStringHandler);
			Dictionary memoryInfo = OS.GetMemoryInfo();
			if (memoryInfo.Count > 0)
			{
				stringBuilder.AppendLine("Memory Info:");
				foreach (KeyValuePair<Variant, Variant> keyValuePair in memoryInfo)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder36 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("  ");
					appendInterpolatedStringHandler.AppendFormatted<Variant>(keyValuePair.Key);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes((ulong)keyValuePair.Value));
					stringBuilder36.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder37 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("  Static Memory Usage: ");
			appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes(OS.GetStaticMemoryUsage()));
			stringBuilder37.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder38 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(28, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("  Static Memory Peak Usage: ");
			appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes(OS.GetStaticMemoryPeakUsage()));
			stringBuilder38.AppendLine(ref appendInterpolatedStringHandler);
			try
			{
				stringBuilder.AppendLine("Disk Info:");
				DriveInfo[] drives = DriveInfo.GetDrives();
				for (int j = 0; j < drives.Length; j++)
				{
					DriveInfo driveInfo = drives[j];
					DriveType driveType = driveInfo.DriveType;
					bool flag = driveType - DriveType.Removable <= 1;
					if (flag)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder39 = stringBuilder2;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 3, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("  Index ");
						appendInterpolatedStringHandler.AppendFormatted<int>(j);
						appendInterpolatedStringHandler.AppendLiteral(": Name: ");
						appendInterpolatedStringHandler.AppendFormatted(driveInfo.Name);
						appendInterpolatedStringHandler.AppendLiteral(" Type: ");
						appendInterpolatedStringHandler.AppendFormatted<DriveType>(driveInfo.DriveType);
						stringBuilder39.Append(ref appendInterpolatedStringHandler);
						if (driveInfo.IsReady)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder40 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(47, 4, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral(" Format: ");
							appendInterpolatedStringHandler.AppendFormatted(driveInfo.DriveFormat);
							appendInterpolatedStringHandler.AppendLiteral(" ");
							appendInterpolatedStringHandler.AppendLiteral("Available: ");
							appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes((ulong)driveInfo.AvailableFreeSpace));
							appendInterpolatedStringHandler.AppendLiteral(" ");
							appendInterpolatedStringHandler.AppendLiteral("Total Free: ");
							appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes((ulong)driveInfo.TotalFreeSpace));
							appendInterpolatedStringHandler.AppendLiteral(" ");
							appendInterpolatedStringHandler.AppendLiteral("Total Size: ");
							appendInterpolatedStringHandler.AppendFormatted(OsDebugInfo.FormatBytes((ulong)driveInfo.TotalSize));
							stringBuilder40.AppendLine(ref appendInterpolatedStringHandler);
						}
						else
						{
							stringBuilder.AppendLine();
						}
					}
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Couldn't get disk info: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				stringBuilder.AppendLine("Couldn't get disk info");
			}
			string[] grantedPermissions = OS.GetGrantedPermissions();
			if (grantedPermissions.Length != 0)
			{
				stringBuilder.AppendLine("Granted Permissions:");
				foreach (string text2 in grantedPermissions)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder41 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("  ");
					appendInterpolatedStringHandler.AppendFormatted(text2);
					stringBuilder41.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			string[] array2 = new string[] { "PATH", "GODOT_ROOT_DIR", "HOME", "DYLD_LIBRARY_PATH", "LD_LIBRARY_PATH" };
			stringBuilder.AppendLine("Important Environment Variables:");
			foreach (string text3 in array2)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder42 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted(text3);
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted(LogSanitizer.Sanitize(OS.GetEnvironment(text3)));
				stringBuilder42.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (OS.GetName() == "Linux")
			{
				stringBuilder.AppendLine("Linux Environment:");
				bool flag2 = !string.IsNullOrEmpty(OS.GetEnvironment("STEAM_COMPAT_DATA_PATH"));
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder43 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Running under Proton: ");
				appendInterpolatedStringHandler.AppendFormatted<bool>(flag2);
				stringBuilder43.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder44 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  PROTON_VERSION: ");
				appendInterpolatedStringHandler.AppendFormatted(OS.GetEnvironment("PROTON_VERSION"));
				stringBuilder44.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder45 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  WINEPREFIX set: ");
				appendInterpolatedStringHandler.AppendFormatted<bool>(!string.IsNullOrEmpty(OS.GetEnvironment("WINEPREFIX")));
				stringBuilder45.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder46 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  XDG_SESSION_TYPE: ");
				appendInterpolatedStringHandler.AppendFormatted(OS.GetEnvironment("XDG_SESSION_TYPE"));
				stringBuilder46.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder47 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  XDG_CURRENT_DESKTOP: ");
				appendInterpolatedStringHandler.AppendFormatted(OS.GetEnvironment("XDG_CURRENT_DESKTOP"));
				stringBuilder47.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder48 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  WAYLAND_DISPLAY: ");
				appendInterpolatedStringHandler.AppendFormatted(OS.GetEnvironment("WAYLAND_DISPLAY"));
				stringBuilder48.AppendLine(ref appendInterpolatedStringHandler);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008700 RID: 34560 RVA: 0x002D6284 File Offset: 0x002D4484
		private static string FormatBytes(ulong bytes)
		{
			string[] array = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
			int num = 0;
			decimal num2 = bytes;
			while (Math.Round(num2 / 1024m) >= 1m)
			{
				num2 /= 1024m;
				num++;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(num2, "n1");
			defaultInterpolatedStringHandler.AppendFormatted(array[num]);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
