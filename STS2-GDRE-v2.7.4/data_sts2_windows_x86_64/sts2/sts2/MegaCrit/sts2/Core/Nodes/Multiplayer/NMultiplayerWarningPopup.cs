using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// A popup to suggest that the player go and play a singleplayer run first.
	/// </summary>
	// Token: 0x02000393 RID: 915
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerWarningPopup.cs")]
	public class NMultiplayerWarningPopup : NVerticalPopup, IScreenContext
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x0019AC44 File Offset: 0x00198E44
		[Nullable(1)]
		public new static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NMultiplayerWarningPopup._scenePath);
			}
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x0019AC50 File Offset: 0x00198E50
		[NullableContext(2)]
		public static NMultiplayerWarningPopup Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMultiplayerWarningPopup._scenePath).Instantiate<NMultiplayerWarningPopup>(0L);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0019AC80 File Offset: 0x00198E80
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.title"), new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.body"));
			this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.back"), new Action<NButton>(this.OnBackButtonPressed));
			this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.ignore"), new Action<NButton>(this.OnIgnoreButtonPressed));
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0019AD18 File Offset: 0x00198F18
		[NullableContext(1)]
		private void OnBackButtonPressed(NButton _)
		{
			this.QueueFreeSafely();
			NGame.Instance.MainMenu.SubmenuStack.Pop();
			NModalContainer.Instance.HideBackstop();
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x0019AD3E File Offset: 0x00198F3E
		[NullableContext(1)]
		private void OnIgnoreButtonPressed(NButton _)
		{
			SaveManager.Instance.MarkFtueAsComplete("multiplayer_warning");
			this.QueueFreeSafely();
			NModalContainer.Instance.HideBackstop();
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x0019AD5F File Offset: 0x00198F5F
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036C3 RID: 14019 RVA: 0x0019AD64 File Offset: 0x00198F64
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NMultiplayerWarningPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMultiplayerWarningPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerWarningPopup.MethodName.OnBackButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036C4 RID: 14020 RVA: 0x0019AEA4 File Offset: 0x001990A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerWarningPopup.MethodName.Create && args.Count == 0)
			{
				NMultiplayerWarningPopup nmultiplayerWarningPopup = NMultiplayerWarningPopup.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerWarningPopup>(ref nmultiplayerWarningPopup);
				return true;
			}
			if ((ref method) == NMultiplayerWarningPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerWarningPopup.MethodName.OnBackButtonPressed && args.Count == 1)
			{
				this.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed && args.Count == 1)
			{
				this.OnIgnoreButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x0019AF70 File Offset: 0x00199170
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerWarningPopup.MethodName.Create && args.Count == 0)
			{
				NMultiplayerWarningPopup nmultiplayerWarningPopup = NMultiplayerWarningPopup.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerWarningPopup>(ref nmultiplayerWarningPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060036C6 RID: 14022 RVA: 0x0019AFB0 File Offset: 0x001991B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerWarningPopup.MethodName.Create || (ref method) == NMultiplayerWarningPopup.MethodName._Ready || (ref method) == NMultiplayerWarningPopup.MethodName.OnBackButtonPressed || (ref method) == NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060036C7 RID: 14023 RVA: 0x0019B000 File Offset: 0x00199200
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerWarningPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060036C8 RID: 14024 RVA: 0x0019B028 File Offset: 0x00199228
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerWarningPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMultiplayerWarningPopup.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036C9 RID: 14025 RVA: 0x0019B080 File Offset: 0x00199280
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerWarningPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerWarningPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036CA RID: 14026 RVA: 0x0019B0D6 File Offset: 0x001992D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerWarningPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
		}

		/// <inheritdoc />
		// Token: 0x060036CB RID: 14027 RVA: 0x0019B0F8 File Offset: 0x001992F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerWarningPopup.PropertyName._verticalPopup, ref variant))
			{
				this._verticalPopup = variant.As<NVerticalPopup>();
			}
		}

		// Token: 0x04001532 RID: 5426
		[Nullable(1)]
		public const string ftueId = "multiplayer_warning";

		// Token: 0x04001533 RID: 5427
		[Nullable(1)]
		private NVerticalPopup _verticalPopup;

		// Token: 0x04001534 RID: 5428
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_warning_popup");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200173D RID: 5949
		public new class MethodName : NVerticalPopup.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005AB8 RID: 23224
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AB9 RID: 23225
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnBackButtonPressed' method.
			/// </summary>
			// Token: 0x04005ABA RID: 23226
			public static readonly StringName OnBackButtonPressed = "OnBackButtonPressed";

			/// <summary>
			/// Cached name for the 'OnIgnoreButtonPressed' method.
			/// </summary>
			// Token: 0x04005ABB RID: 23227
			public static readonly StringName OnIgnoreButtonPressed = "OnIgnoreButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200173E RID: 5950
		public new class PropertyName : NVerticalPopup.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005ABC RID: 23228
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04005ABD RID: 23229
			public static readonly StringName _verticalPopup = "_verticalPopup";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200173F RID: 5951
		public new class SignalName : NVerticalPopup.SignalName
		{
		}
	}
}
