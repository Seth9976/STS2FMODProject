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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen
{
	/// <summary>
	/// Vertical popup used to get player confirmation before loading mods.
	/// Renders above the capstone screens (above top bar).
	/// </summary>
	// Token: 0x020002E4 RID: 740
	[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NConfirmModLoadingPopup.cs")]
	public class NConfirmModLoadingPopup : Control, IScreenContext
	{
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x0010F647 File Offset: 0x0010D847
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NConfirmModLoadingPopup._scenePath);
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x0010F653 File Offset: 0x0010D853
		[Nullable(1)]
		public Control DefaultFocusedControl
		{
			[NullableContext(1)]
			get
			{
				return this._verticalPopup.NoButton;
			}
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x0010F660 File Offset: 0x0010D860
		[NullableContext(2)]
		public static NConfirmModLoadingPopup Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NConfirmModLoadingPopup._scenePath).Instantiate<NConfirmModLoadingPopup>(0L);
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x0010F690 File Offset: 0x0010D890
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(new LocString("main_menu_ui", "MODDING_POPUP.title"), new LocString("main_menu_ui", "MODDING_POPUP.description"));
			this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "MODDING_POPUP.load_mods"), new Action<NButton>(this.OnYesButtonPressed));
			this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "MODDING_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x0010F728 File Offset: 0x0010D928
		[NullableContext(1)]
		private void OnYesButtonPressed(NButton _)
		{
			Log.Info("Player chose to load mods", 2);
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (settingsSave.ModSettings == null)
			{
				settingsSave.ModSettings = new ModSettings();
			}
			SaveManager.Instance.SettingsSave.ModSettings.PlayerAgreedToModLoading = true;
			SaveManager.Instance.SaveSettings();
			NGame.Instance.Quit();
			this.QueueFreeSafely();
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0010F790 File Offset: 0x0010D990
		[NullableContext(1)]
		private void OnNoButtonPressed(NButton _)
		{
			Log.Info("Player chose not to load mods", 2);
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (settingsSave.ModSettings == null)
			{
				settingsSave.ModSettings = new ModSettings();
			}
			SaveManager.Instance.SettingsSave.ModSettings.PlayerAgreedToModLoading = false;
			SaveManager.Instance.SaveSettings();
			NGame instance = NGame.Instance;
			object obj;
			if (instance == null)
			{
				obj = null;
			}
			else
			{
				NMainMenu mainMenu = instance.MainMenu;
				obj = ((mainMenu != null) ? mainMenu.SubmenuStack.Peek() : null);
			}
			if (obj is NModdingScreen)
			{
				NGame.Instance.MainMenu.SubmenuStack.Pop();
			}
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027B5 RID: 10165 RVA: 0x0010F82C File Offset: 0x0010DA2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NConfirmModLoadingPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NConfirmModLoadingPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmModLoadingPopup.MethodName.OnYesButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NConfirmModLoadingPopup.MethodName.OnNoButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027B6 RID: 10166 RVA: 0x0010F96C File Offset: 0x0010DB6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NConfirmModLoadingPopup.MethodName.Create && args.Count == 0)
			{
				NConfirmModLoadingPopup nconfirmModLoadingPopup = NConfirmModLoadingPopup.Create();
				ret = VariantUtils.CreateFrom<NConfirmModLoadingPopup>(ref nconfirmModLoadingPopup);
				return true;
			}
			if ((ref method) == NConfirmModLoadingPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmModLoadingPopup.MethodName.OnYesButtonPressed && args.Count == 1)
			{
				this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmModLoadingPopup.MethodName.OnNoButtonPressed && args.Count == 1)
			{
				this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x0010FA38 File Offset: 0x0010DC38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NConfirmModLoadingPopup.MethodName.Create && args.Count == 0)
			{
				NConfirmModLoadingPopup nconfirmModLoadingPopup = NConfirmModLoadingPopup.Create();
				ret = VariantUtils.CreateFrom<NConfirmModLoadingPopup>(ref nconfirmModLoadingPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060027B8 RID: 10168 RVA: 0x0010FA78 File Offset: 0x0010DC78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NConfirmModLoadingPopup.MethodName.Create || (ref method) == NConfirmModLoadingPopup.MethodName._Ready || (ref method) == NConfirmModLoadingPopup.MethodName.OnYesButtonPressed || (ref method) == NConfirmModLoadingPopup.MethodName.OnNoButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060027B9 RID: 10169 RVA: 0x0010FAC8 File Offset: 0x0010DCC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NConfirmModLoadingPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060027BA RID: 10170 RVA: 0x0010FAF0 File Offset: 0x0010DCF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NConfirmModLoadingPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NConfirmModLoadingPopup.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027BB RID: 10171 RVA: 0x0010FB48 File Offset: 0x0010DD48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NConfirmModLoadingPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NConfirmModLoadingPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027BC RID: 10172 RVA: 0x0010FB9E File Offset: 0x0010DD9E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NConfirmModLoadingPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
		}

		/// <inheritdoc />
		// Token: 0x060027BD RID: 10173 RVA: 0x0010FBC0 File Offset: 0x0010DDC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NConfirmModLoadingPopup.PropertyName._verticalPopup, ref variant))
			{
				this._verticalPopup = variant.As<NVerticalPopup>();
			}
		}

		// Token: 0x04000EC4 RID: 3780
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/confirm_mod_loading_popup");

		// Token: 0x04000EC5 RID: 3781
		[Nullable(1)]
		private NVerticalPopup _verticalPopup;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001481 RID: 5249
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004DD2 RID: 19922
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DD3 RID: 19923
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnYesButtonPressed' method.
			/// </summary>
			// Token: 0x04004DD4 RID: 19924
			public static readonly StringName OnYesButtonPressed = "OnYesButtonPressed";

			/// <summary>
			/// Cached name for the 'OnNoButtonPressed' method.
			/// </summary>
			// Token: 0x04004DD5 RID: 19925
			public static readonly StringName OnNoButtonPressed = "OnNoButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001482 RID: 5250
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004DD6 RID: 19926
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04004DD7 RID: 19927
			public static readonly StringName _verticalPopup = "_verticalPopup";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001483 RID: 5251
		public class SignalName : Control.SignalName
		{
		}
	}
}
