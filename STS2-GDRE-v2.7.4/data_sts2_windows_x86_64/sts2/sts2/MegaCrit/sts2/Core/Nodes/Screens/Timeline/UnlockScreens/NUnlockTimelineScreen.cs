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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which isn't really a screen but just an animation.
	/// Extends NUnlockScreen so it supports queueing it up like the other unlocks on the TimelineScreen.
	/// </summary>
	// Token: 0x02000270 RID: 624
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockTimelineScreen.cs")]
	public class NUnlockTimelineScreen : NUnlockScreen
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x000DF248 File Offset: 0x000DD448
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockTimelineScreen._scenePath);
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000DF254 File Offset: 0x000DD454
		[NullableContext(1)]
		public static NUnlockTimelineScreen Create()
		{
			return PreloadManager.Cache.GetScene(NUnlockTimelineScreen._scenePath).Instantiate<NUnlockTimelineScreen>(0L);
		}

		/// <summary>
		/// Empty _Ready() function so we don't initialize the Confirm button like other unlock screens.
		/// Better ways to handle but w/e.
		/// </summary>
		// Token: 0x060020F0 RID: 8432 RVA: 0x000DF26C File Offset: 0x000DD46C
		public override void _Ready()
		{
		}

		/// <summary>
		/// Set which era slots are unlocked when this "screen" is triggered.
		/// </summary>
		/// <param name="eras"></param>
		// Token: 0x060020F1 RID: 8433 RVA: 0x000DF26E File Offset: 0x000DD46E
		[NullableContext(1)]
		public void SetUnlocks(List<EpochSlotData> eras)
		{
			this._erasToUnlock = eras.OrderBy<EpochSlotData, int>((EpochSlotData a) => a.EraPosition).ToList<EpochSlotData>();
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000DF2A0 File Offset: 0x000DD4A0
		public override void Open()
		{
			base.Open();
			TaskHelper.RunSafely(this.AnimateExpansion());
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000DF2B4 File Offset: 0x000DD4B4
		[NullableContext(1)]
		private async Task AnimateExpansion()
		{
			await NTimelineScreen.Instance.HideBackstopAndShowUi(false);
			await NTimelineScreen.Instance.AddEpochSlots(this._erasToUnlock, true);
			NTimelineScreen.Instance.ShowHeaderAndActionsUi();
			NTimelineScreen.Instance.SetScreenDraggability();
			await base.Close();
			NTimelineScreen.Instance.EnableInput();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020F4 RID: 8436 RVA: 0x000DF2F8 File Offset: 0x000DD4F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NUnlockTimelineScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NUnlockTimelineScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockTimelineScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020F5 RID: 8437 RVA: 0x000DF3A8 File Offset: 0x000DD5A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockTimelineScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockTimelineScreen>(ref nunlockTimelineScreen);
				return true;
			}
			if ((ref method) == NUnlockTimelineScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockTimelineScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000DF434 File Offset: 0x000DD634
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockTimelineScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockTimelineScreen>(ref nunlockTimelineScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060020F7 RID: 8439 RVA: 0x000DF474 File Offset: 0x000DD674
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockTimelineScreen.MethodName.Create || (ref method) == NUnlockTimelineScreen.MethodName._Ready || (ref method) == NUnlockTimelineScreen.MethodName.Open || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060020F8 RID: 8440 RVA: 0x000DF4AA File Offset: 0x000DD6AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060020F9 RID: 8441 RVA: 0x000DF4B3 File Offset: 0x000DD6B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000CD1 RID: 3281
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_timeline_screen");

		// Token: 0x04000CD2 RID: 3282
		[Nullable(1)]
		private List<EpochSlotData> _erasToUnlock;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012EF RID: 4847
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040049B7 RID: 18871
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049B8 RID: 18872
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x040049B9 RID: 18873
			public new static readonly StringName Open = "Open";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F0 RID: 4848
		public new class PropertyName : NUnlockScreen.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F1 RID: 4849
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
