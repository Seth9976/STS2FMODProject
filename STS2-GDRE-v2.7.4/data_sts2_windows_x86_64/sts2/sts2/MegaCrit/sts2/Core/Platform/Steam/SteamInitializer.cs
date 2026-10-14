using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x0200015E RID: 350
	[NullableContext(2)]
	[Nullable(0)]
	public static class SteamInitializer
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00062594 File Offset: 0x00060794
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x0006259B File Offset: 0x0006079B
		public static bool Initialized { get; private set; }

		/// <summary>
		/// Initialization result. Failure code if Initialization was called and Initialized is false.
		/// </summary>
		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x000625A3 File Offset: 0x000607A3
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x000625AA File Offset: 0x000607AA
		public static ESteamAPIInitResult? InitResult { get; private set; }

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x000625B2 File Offset: 0x000607B2
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x000625B9 File Offset: 0x000607B9
		public static string InitErrorMessage { get; private set; }

		/// <summary>
		/// Cancellation token that is cancelled when Steam disconnects. Pass this to SteamCallResult
		/// to prevent async Steam API calls from hanging indefinitely when the callback pump stops.
		/// </summary>
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x000625C1 File Offset: 0x000607C1
		public static CancellationToken DisconnectToken
		{
			get
			{
				return SteamInitializer._disconnectCts.Token;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600109F RID: 4255 RVA: 0x000625D0 File Offset: 0x000607D0
		// (remove) Token: 0x060010A0 RID: 4256 RVA: 0x00062604 File Offset: 0x00060804
		public static event Action SteamNoLongerRunning;

		// Token: 0x060010A1 RID: 4257 RVA: 0x00062638 File Offset: 0x00060838
		[NullableContext(1)]
		private static IntPtr SteamDebugResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
		{
			if (libraryName != "steam_api" && libraryName != "steam_api64")
			{
				return IntPtr.Zero;
			}
			if (OS.HasFeature("editor"))
			{
				string name = OS.GetName();
				string text;
				if (name != null)
				{
					switch (name.Length)
					{
					case 3:
						if (!(name == "BSD"))
						{
							goto IL_011E;
						}
						break;
					case 4:
						goto IL_011E;
					case 5:
					{
						char c = name[0];
						if (c != 'L')
						{
							if (c != 'm')
							{
								goto IL_011E;
							}
							if (!(name == "macOS"))
							{
								goto IL_011E;
							}
							text = "steam/SteamApi/libsteam_api.dylib";
							goto IL_0120;
						}
						else if (!(name == "Linux"))
						{
							goto IL_011E;
						}
						break;
					}
					case 6:
						if (!(name == "NetBSD"))
						{
							goto IL_011E;
						}
						break;
					case 7:
					{
						char c = name[0];
						if (c != 'F')
						{
							if (c != 'O')
							{
								if (c != 'W')
								{
									goto IL_011E;
								}
								if (!(name == "Windows"))
								{
									goto IL_011E;
								}
								text = "steam/SteamApi/steam_api64.dll";
								goto IL_0120;
							}
							else if (!(name == "OpenBSD"))
							{
								goto IL_011E;
							}
						}
						else if (!(name == "FreeBSD"))
						{
							goto IL_011E;
						}
						break;
					}
					default:
						goto IL_011E;
					}
					text = "steam/SteamApi/libsteam_api.so";
					goto IL_0120;
				}
				IL_011E:
				text = null;
				IL_0120:
				string text2 = text;
				if (text2 != null)
				{
					return NativeLibrary.Load(text2);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00062778 File Offset: 0x00060978
		[NullableContext(1)]
		public static bool Initialize(Node node)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SteamAPI));
			DllImportResolver dllImportResolver;
			if ((dllImportResolver = SteamInitializer.<>O.<0>__SteamDebugResolver) == null)
			{
				dllImportResolver = (SteamInitializer.<>O.<0>__SteamDebugResolver = new DllImportResolver(SteamInitializer.SteamDebugResolver));
			}
			NativeLibrary.SetDllImportResolver(assembly, dllImportResolver);
			Log.Info("Steamworks: attempting initialization...", 2);
			for (int i = 0; i < 3; i++)
			{
				if (i > 0)
				{
					Thread.Sleep(100);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Steam initialization retry attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				SteamInitializer.Initialized = SteamInitializer.InitializeInternal();
				if (SteamInitializer.Initialized)
				{
					break;
				}
			}
			if (SteamInitializer.Initialized)
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				TaskHelper.RunSafely(SteamInitializer.RunCallbacksAsync(node));
			}
			return SteamInitializer.Initialized;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00062830 File Offset: 0x00060A30
		private static bool InitializeInternal()
		{
			try
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Steam is running: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(SteamAPI.IsSteamRunning());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				string text;
				SteamInitializer.InitResult = new ESteamAPIInitResult?(SteamAPI.InitEx(ref text));
				SteamInitializer.InitErrorMessage = text;
				ESteamAPIInitResult? initResult = SteamInitializer.InitResult;
				ESteamAPIInitResult esteamAPIInitResult = 0;
				if ((initResult.GetValueOrDefault() == esteamAPIInitResult) & (initResult != null))
				{
					Log.Info("Steamworks initialization succeeded!", 2);
					return true;
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Steamworks initialization failed! Result: ");
				defaultInterpolatedStringHandler.AppendFormatted<ESteamAPIInitResult?>(SteamInitializer.InitResult);
				defaultInterpolatedStringHandler.AppendLiteral(", message: ");
				defaultInterpolatedStringHandler.AppendFormatted(SteamInitializer.InitErrorMessage);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Steamworks initialization threw an exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SteamInitializer.InitResult = null;
				SteamInitializer.InitErrorMessage = ex.Message;
			}
			return false;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00062958 File Offset: 0x00060B58
		[NullableContext(1)]
		private static async Task RunCallbacksAsync(Node node)
		{
			while (SteamInitializer.Initialized)
			{
				await node.AwaitProcessFrame(default(CancellationToken));
				if (!SteamAPI.IsSteamRunning())
				{
					Log.Warn("Steam is no longer running. Stopping callbacks.", 2);
					SteamInitializer.Initialized = false;
					SteamInitializer._disconnectCts.Cancel();
					Action steamNoLongerRunning = SteamInitializer.SteamNoLongerRunning;
					if (steamNoLongerRunning == null)
					{
						break;
					}
					steamNoLongerRunning();
					break;
				}
				else
				{
					SteamAPI.RunCallbacks();
				}
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0006299C File Offset: 0x00060B9C
		public static void Uninitialize()
		{
			if (!SteamInitializer.Initialized)
			{
				return;
			}
			Log.Info("Steamworks: shutting down...", 2);
			try
			{
				SteamAPI.Shutdown();
				Log.Info("Steamworks shutdown succeeded!", 2);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Steamworks shutdown threw an exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			finally
			{
				SteamInitializer.Initialized = false;
			}
		}

		// Token: 0x040005EF RID: 1519
		public const ulong steamAppId = 2868840UL;

		// Token: 0x040005F3 RID: 1523
		[Nullable(1)]
		private static CancellationTokenSource _disconnectCts = new CancellationTokenSource();

		// Token: 0x02000F3B RID: 3899
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003C79 RID: 15481
			[Nullable(0)]
			public static DllImportResolver <0>__SteamDebugResolver;
		}
	}
}
