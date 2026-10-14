using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	/// <summary>
	/// Helper class to quickly load screens of the game without having to go through the main menu.
	/// Can specify character, room type, and room details in the parameters you want to load into.
	/// And then start the scene scene_bootstrapper.tscn
	/// </summary>
	// Token: 0x020003CA RID: 970
	[ScriptPath("res://src/Core/Nodes/Debug/NSceneBootstrapper.cs")]
	public class NSceneBootstrapper : Node
	{
		// Token: 0x06003AC7 RID: 15047 RVA: 0x001C10D0 File Offset: 0x001BF2D0
		public override void _Ready()
		{
			NGame ngame = base.GetParent() as NGame;
			if (ngame != null)
			{
				this._game = ngame;
			}
			else
			{
				this._game = SceneHelper.Instantiate<NGame>("game");
				this._game.StartOnMainMenu = false;
				this.AddChildSafely(this._game);
			}
			TaskHelper.RunSafely(this.StartNewRun());
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x001C112C File Offset: 0x001BF32C
		[NullableContext(1)]
		private async Task StartNewRun()
		{
			await this._game.GameStartupComplete;
			Type type = BootstrapSettingsUtil.Get();
			if (type == null)
			{
				Log.Error("No type implementing IBootstrapSettings found in the project! To use the bootstrap scene, copy src/Core/Nodes/Debug/BootstrapSettings.cs.template and rename it to BootstrapSettings.cs", 2);
			}
			else
			{
				IBootstrapSettings settings = (IBootstrapSettings)Activator.CreateInstance(type);
				if (settings.Language != null)
				{
					LocManager.Instance.SetLanguage(settings.Language);
				}
				PreloadManager.Enabled = settings.DoPreloading;
				string text = settings.Seed ?? SeedHelper.GetRandomSeed(null, 12);
				List<ActModel> list = ActModel.GetDefaultList().ToList<ActModel>();
				list[0] = settings.Act;
				RunState runState = RunState.CreateForNewRun(new <>z__ReadOnlySingleElementList<Player>(Player.CreateForNewRun(settings.Character, SaveManager.Instance.GenerateUnlockStateFromProgress(), 1UL)), list.Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), settings.Modifiers, GameMode.Standard, settings.Ascension, text);
				RunManager.Instance.SetUpNewSingleplayer(runState, settings.SaveRunHistory, null);
				await PreloadManager.LoadRunAssets(new <>z__ReadOnlySingleElementList<CharacterModel>(settings.Character));
				RunManager.Instance.Launch();
				this._game.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
				await RunManager.Instance.SetActInternal(0);
				RunManager.Instance.RunLocationTargetedBuffer.OnLocationChanged(runState.RunLocation);
				RunManager.Instance.MapSelectionSynchronizer.OnLocationChanged(runState.MapLocation);
				await settings.Setup(runState.Players[0]);
				switch (settings.RoomType)
				{
				case RoomType.Unassigned:
					await RunManager.Instance.EnterAct(0, true);
					goto IL_05D3;
				case RoomType.Treasure:
				case RoomType.Shop:
				case RoomType.RestSite:
					await RunManager.Instance.EnterRoomDebug(settings.RoomType, MapPointType.Unassigned, null, true);
					RunManager.Instance.ActionExecutor.Unpause();
					goto IL_05D3;
				case RoomType.Event:
				{
					AbstractRoom abstractRoom = await RunManager.Instance.EnterRoomDebug(settings.RoomType, MapPointType.Unassigned, settings.Event, true);
					if (abstractRoom != null && abstractRoom.IsVictoryRoom)
					{
						runState.CurrentActIndex = runState.Acts.Count - 1;
						goto IL_05D3;
					}
					goto IL_05D3;
				}
				}
				await RunManager.Instance.EnterRoomDebug(settings.RoomType, MapPointType.Unassigned, settings.RoomType.IsCombatRoom() ? settings.Encounter.ToMutable() : null, true);
				IL_05D3:
				if (this._openConsole)
				{
					NDevConsole.Instance.ShowConsole();
					NDevConsole.Instance.MakeFullScreen();
					NDevConsole.Instance.SetBackgroundColor(Colors.White);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AC9 RID: 15049 RVA: 0x001C1170 File Offset: 0x001BF370
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSceneBootstrapper.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003ACA RID: 15050 RVA: 0x001C11B4 File Offset: 0x001BF3B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSceneBootstrapper.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003ACB RID: 15051 RVA: 0x001C11E4 File Offset: 0x001BF3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSceneBootstrapper.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003ACC RID: 15052 RVA: 0x001C11FC File Offset: 0x001BF3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSceneBootstrapper.PropertyName._openConsole)
			{
				this._openConsole = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSceneBootstrapper.PropertyName._game)
			{
				this._game = VariantUtils.ConvertTo<NGame>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003ACD RID: 15053 RVA: 0x001C123C File Offset: 0x001BF43C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSceneBootstrapper.PropertyName._openConsole)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._openConsole);
				return true;
			}
			if ((ref name) == NSceneBootstrapper.PropertyName._game)
			{
				value = VariantUtils.CreateFrom<NGame>(ref this._game);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003ACE RID: 15054 RVA: 0x001C1294 File Offset: 0x001BF494
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NSceneBootstrapper.PropertyName._openConsole, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSceneBootstrapper.PropertyName._game, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003ACF RID: 15055 RVA: 0x001C12E9 File Offset: 0x001BF4E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSceneBootstrapper.PropertyName._openConsole, Variant.From<bool>(ref this._openConsole));
			info.AddProperty(NSceneBootstrapper.PropertyName._game, Variant.From<NGame>(ref this._game));
		}

		/// <inheritdoc />
		// Token: 0x06003AD0 RID: 15056 RVA: 0x001C1320 File Offset: 0x001BF520
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSceneBootstrapper.PropertyName._openConsole, ref variant))
			{
				this._openConsole = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSceneBootstrapper.PropertyName._game, ref variant2))
			{
				this._game = variant2.As<NGame>();
			}
		}

		// Token: 0x040016F3 RID: 5875
		private bool _openConsole;

		// Token: 0x040016F4 RID: 5876
		[Nullable(1)]
		private NGame _game;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001806 RID: 6150
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005DDE RID: 24030
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001807 RID: 6151
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_openConsole' field.
			/// </summary>
			// Token: 0x04005DDF RID: 24031
			public static readonly StringName _openConsole = "_openConsole";

			/// <summary>
			/// Cached name for the '_game' field.
			/// </summary>
			// Token: 0x04005DE0 RID: 24032
			public static readonly StringName _game = "_game";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001808 RID: 6152
		public class SignalName : Node.SignalName
		{
		}
	}
}
