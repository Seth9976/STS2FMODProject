using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using SmartFormat.Extensions;

namespace MegaCrit.Sts2.Core.Helpers
{
	/// <summary>
	/// Contains methods that are called as part of initialization.
	/// The methods are called both in normal startup and in test mode, so only call things that need to be done in both.
	/// </summary>
	// Token: 0x02000C0F RID: 3087
	[NullableContext(1)]
	[Nullable(0)]
	public static class OneTimeInitialization
	{
		// Token: 0x17002308 RID: 8968
		// (get) Token: 0x06007FCD RID: 32717 RVA: 0x002B64A5 File Offset: 0x002B46A5
		// (set) Token: 0x06007FCE RID: 32718 RVA: 0x002B64AC File Offset: 0x002B46AC
		public static ReadSaveResult<SettingsSave> SettingsReadResult { get; private set; }

		/// <summary>
		/// Initializes stuff that has to happen very early.
		/// This gets called in both tests and in real games!! Only do stuff that needs to happen in both here.
		/// </summary>
		// Token: 0x06007FCF RID: 32719 RVA: 0x002B64B4 File Offset: 0x002B46B4
		public static async Task ExecuteVeryEarly()
		{
			if (OneTimeInitialization._state != OneTimeInitialization.State.None)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to call OneTimeInitialization.ExecuteVeryEarly in state ");
				defaultInterpolatedStringHandler.AppendFormatted<OneTimeInitialization.State>(OneTimeInitialization._state);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				OneTimeInitialization._state = OneTimeInitialization.State.VeryEarly;
				if (TestMode.IsOn)
				{
					OneTimeInitialization.SettingsReadResult = SaveManager.Instance.InitSettingsDataForTest();
				}
				else
				{
					OneTimeInitialization.SettingsReadResult = SaveManager.Instance.InitSettingsData();
				}
				await ModManager.Initialize(new ModManagerFileIo(), SaveManager.Instance.SettingsSave.ModSettings, ReleaseInfoManager.Instance.SemVer);
				UserDataPathProvider.IsRunningModded = ModManager.IsRunningModded();
				SentryService.DisableSentryIfModded();
			}
		}

		/// <summary>
		/// Essential initialization needed before main menu can display.
		/// Loads only ui_atlas and core systems (settings, mods, localization, model DB init).
		/// </summary>
		// Token: 0x06007FD0 RID: 32720 RVA: 0x002B64F0 File Offset: 0x002B46F0
		public static void ExecuteEssential()
		{
			if (OneTimeInitialization._state != OneTimeInitialization.State.VeryEarly)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to call OneTimeInitialization.ExecuteEssential in state ");
				defaultInterpolatedStringHandler.AppendFormatted<OneTimeInitialization.State>(OneTimeInitialization._state);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			OneTimeInitialization._state = OneTimeInitialization.State.Essential;
			OneTimeInitialization._atlasResourceLoader = new AtlasResourceLoader();
			ResourceLoader.AddResourceFormatLoader(OneTimeInitialization._atlasResourceLoader, true);
			AtlasManager.LoadEssentialAtlases();
			LocManager.Initialize();
			AssemblyInfo.Init();
			ModelDb.Init(null);
			ModelIdSerializationCache.Init();
			ModelDb.InitIds();
			MessageTypes.Initialize();
			ActionTypes.Initialize();
		}

		/// <summary>
		/// Deferred initialization that can run after main menu is displayed.
		/// Loads remaining atlases and runs expensive precomputation.
		/// </summary>
		// Token: 0x06007FD1 RID: 32721 RVA: 0x002B6588 File Offset: 0x002B4788
		public static void ExecuteDeferred()
		{
			if (OneTimeInitialization._state != OneTimeInitialization.State.Essential)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to call OneTimeInitialization.ExecuteDeferred in state ");
				defaultInterpolatedStringHandler.AppendFormatted<OneTimeInitialization.State>(OneTimeInitialization._state);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			OneTimeInitialization._state = OneTimeInitialization.State.Done;
			AtlasManager.LoadAllAtlases();
			if (!OS.HasFeature("editor"))
			{
				ModelDb.Preload();
				OneTimeInitialization.PrewarmJit();
				ConditionalFormatter conditionalFormatter = new ConditionalFormatter();
			}
		}

		// Token: 0x06007FD2 RID: 32722 RVA: 0x002B6600 File Offset: 0x002B4800
		private static void PrewarmJit()
		{
			Type typeFromHandle = typeof(PacketWriter);
			Type typeFromHandle2 = typeof(PacketReader);
			foreach (Type type in ReflectionHelper.GetSubtypes<IPacketSerializable>())
			{
				RuntimeHelpers.PrepareMethod(type.GetMethod("Serialize").MethodHandle);
				RuntimeHelpers.PrepareMethod(type.GetMethod("Deserialize").MethodHandle);
				RuntimeHelpers.PrepareMethod(typeFromHandle.GetMethod("WriteList").MethodHandle, new RuntimeTypeHandle[] { type.TypeHandle });
				RuntimeHelpers.PrepareMethod(typeFromHandle.GetMethod("Write").MethodHandle, new RuntimeTypeHandle[] { type.TypeHandle });
				RuntimeHelpers.PrepareMethod(typeFromHandle2.GetMethod("ReadList").MethodHandle, new RuntimeTypeHandle[] { type.TypeHandle });
				RuntimeHelpers.PrepareMethod(typeFromHandle2.GetMethod("Read").MethodHandle, new RuntimeTypeHandle[] { type.TypeHandle });
			}
			foreach (Type type2 in ReflectionHelper.GetSubtypes<INetMessage>())
			{
				RuntimeHelpers.PrepareMethod(type2.GetMethod("get_ShouldBuffer").MethodHandle);
				RuntimeHelpers.PrepareMethod(type2.GetMethod("get_LogLevel").MethodHandle);
				RuntimeHelpers.PrepareMethod(type2.GetMethod("get_Mode").MethodHandle);
				RuntimeHelpers.PrepareMethod(type2.GetMethod("get_ShouldBroadcast").MethodHandle);
			}
		}

		// Token: 0x04002A8C RID: 10892
		[Nullable(2)]
		private static AtlasResourceLoader _atlasResourceLoader;

		// Token: 0x04002A8D RID: 10893
		private static OneTimeInitialization.State _state;

		// Token: 0x02002352 RID: 9042
		[NullableContext(0)]
		private enum State
		{
			// Token: 0x040095BA RID: 38330
			None,
			// Token: 0x040095BB RID: 38331
			VeryEarly,
			// Token: 0x040095BC RID: 38332
			Essential,
			// Token: 0x040095BD RID: 38333
			Done
		}
	}
}
