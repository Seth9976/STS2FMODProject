using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen
{
	// Token: 0x020002E5 RID: 741
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModdingScreen.cs")]
	public class NModdingScreen : NSubmenu
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x0010FC09 File Offset: 0x0010DE09
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._modRowContainer.GetChildren(false).OfType<NModMenuRow>().FirstOrDefault<NModMenuRow>();
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x0010FC21 File Offset: 0x0010DE21
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NModdingScreen._scenePath };
			}
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x0010FC31 File Offset: 0x0010DE31
		[NullableContext(2)]
		public static NModdingScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NModdingScreen._scenePath).Instantiate<NModdingScreen>(0L);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x0010FC54 File Offset: 0x0010DE54
		public override void _Ready()
		{
			this._modInfoContainer = base.GetNode<NModInfoContainer>("%ModInfoContainer");
			this._scrollableContainer = base.GetNode<NScrollableContainer>("%ModsScrollContainer");
			this._modRowContainer = this._scrollableContainer.GetNode<Control>("Mask/Content");
			this._pendingChangesWarning = base.GetNode<Control>("%PendingChangesLabel");
			NButton node = base.GetNode<NButton>("%GetModsButton");
			NButton node2 = base.GetNode<NButton>("%MakeModsButton");
			foreach (Node node3 in this._modRowContainer.GetChildren(false))
			{
				node3.QueueFreeSafely();
			}
			foreach (Mod mod in ModManager.Mods)
			{
				this.OnNewModDetected(mod);
			}
			node.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnGetModsPressed)), 0U);
			node2.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMakeModsPressed)), 0U);
			node.GetNode<MegaLabel>("Visuals/Label").SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.GET_MODS_BUTTON").GetFormattedText());
			node2.GetNode<MegaLabel>("Visuals/Label").SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.MAKE_MODS_BUTTON").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%InstalledModsTitle").SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.INSTALLED_MODS_TITLE").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%PendingChangesLabel").SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.PENDING_CHANGES_WARNING").GetFormattedText());
			this._pendingChangesWarning.Visible = false;
			ModManager.OnModDetected += this.OnNewModDetected;
			this.ConnectSignals();
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x0010FE6C File Offset: 0x0010E06C
		public override void OnSubmenuOpened()
		{
			if (!ModManager.PlayerAgreedToModLoading && ModManager.Mods.Count > 0)
			{
				NModalContainer.Instance.Add(NConfirmModLoadingPopup.Create(), true);
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x0010FE94 File Offset: 0x0010E094
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			foreach (NModMenuRow nmodMenuRow in this._modRowContainer.GetChildren(false).OfType<NModMenuRow>())
			{
				nmodMenuRow.SetSelected(false);
			}
			this._modInfoContainer.Clear();
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x0010FF00 File Offset: 0x0010E100
		private void OnGetModsPressed(NButton _)
		{
			PlatformUtil.OpenUrl("https://steamcommunity.com/app/2868840/workshop/");
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x0010FF0C File Offset: 0x0010E10C
		private void OnMakeModsPressed(NButton _)
		{
			PlatformUtil.OpenUrl("https://github.com/Alchyr/ModTemplate-StS2");
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x0010FF18 File Offset: 0x0010E118
		public void OnRowSelected(NModMenuRow row)
		{
			row.SetSelected(true);
			this._modInfoContainer.Fill(row.Mod);
			foreach (NModMenuRow nmodMenuRow in this._modRowContainer.GetChildren(false).OfType<NModMenuRow>())
			{
				if (nmodMenuRow != row)
				{
					nmodMenuRow.SetSelected(false);
				}
			}
		}

		/// <summary>
		/// Called both during initialization and when a new mod is installed during runtime.
		/// </summary>
		// Token: 0x060027C9 RID: 10185 RVA: 0x0010FF8C File Offset: 0x0010E18C
		private void OnNewModDetected(Mod mod)
		{
			NModMenuRow nmodMenuRow = NModMenuRow.Create(this, mod);
			this._modRowContainer.AddChildSafely(nmodMenuRow);
			this.OnModEnabledOrDisabled();
			this._scrollableContainer.DisableScrollingIfContentFits();
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x0010FFC0 File Offset: 0x0010E1C0
		public void OnModEnabledOrDisabled()
		{
			foreach (Mod mod in ModManager.Mods)
			{
				ModSettings modSettings = SaveManager.Instance.SettingsSave.ModSettings;
				bool flag = modSettings != null && modSettings.IsModDisabled(mod);
				if ((mod.state == ModLoadState.Disabled && !flag) || (mod.state == ModLoadState.Loaded && flag))
				{
					this._pendingChangesWarning.Visible = true;
					return;
				}
			}
			this._pendingChangesWarning.Visible = false;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00110058 File Offset: 0x0010E258
		public override void _ExitTree()
		{
			ModManager.OnModDetected -= this.OnNewModDetected;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027CC RID: 10188 RVA: 0x0011006C File Offset: 0x0010E26C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NModdingScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NModdingScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModdingScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModdingScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModdingScreen.MethodName.OnGetModsPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NModdingScreen.MethodName.OnMakeModsPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NModdingScreen.MethodName.OnRowSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "row", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NModdingScreen.MethodName.OnModEnabledOrDisabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModdingScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027CD RID: 10189 RVA: 0x001102C8 File Offset: 0x0010E4C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModdingScreen.MethodName.Create && args.Count == 0)
			{
				NModdingScreen nmoddingScreen = NModdingScreen.Create();
				ret = VariantUtils.CreateFrom<NModdingScreen>(ref nmoddingScreen);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnGetModsPressed && args.Count == 1)
			{
				this.OnGetModsPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnMakeModsPressed && args.Count == 1)
			{
				this.OnMakeModsPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnRowSelected && args.Count == 1)
			{
				this.OnRowSelected(VariantUtils.ConvertTo<NModMenuRow>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName.OnModEnabledOrDisabled && args.Count == 0)
			{
				this.OnModEnabledOrDisabled();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModdingScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x0011045C File Offset: 0x0010E65C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModdingScreen.MethodName.Create && args.Count == 0)
			{
				NModdingScreen nmoddingScreen = NModdingScreen.Create();
				ret = VariantUtils.CreateFrom<NModdingScreen>(ref nmoddingScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060027CF RID: 10191 RVA: 0x0011049C File Offset: 0x0010E69C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NModdingScreen.MethodName.Create || (ref method) == NModdingScreen.MethodName._Ready || (ref method) == NModdingScreen.MethodName.OnSubmenuOpened || (ref method) == NModdingScreen.MethodName.OnSubmenuClosed || (ref method) == NModdingScreen.MethodName.OnGetModsPressed || (ref method) == NModdingScreen.MethodName.OnMakeModsPressed || (ref method) == NModdingScreen.MethodName.OnRowSelected || (ref method) == NModdingScreen.MethodName.OnModEnabledOrDisabled || (ref method) == NModdingScreen.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060027D0 RID: 10192 RVA: 0x00110538 File Offset: 0x0010E738
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NModdingScreen.PropertyName._modInfoContainer)
			{
				this._modInfoContainer = VariantUtils.ConvertTo<NModInfoContainer>(ref value);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._scrollableContainer)
			{
				this._scrollableContainer = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._modRowContainer)
			{
				this._modRowContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._pendingChangesWarning)
			{
				this._pendingChangesWarning = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060027D1 RID: 10193 RVA: 0x001105BC File Offset: 0x0010E7BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NModdingScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._modInfoContainer)
			{
				value = VariantUtils.CreateFrom<NModInfoContainer>(ref this._modInfoContainer);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._scrollableContainer)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._scrollableContainer);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._modRowContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._modRowContainer);
				return true;
			}
			if ((ref name) == NModdingScreen.PropertyName._pendingChangesWarning)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._pendingChangesWarning);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027D2 RID: 10194 RVA: 0x00110674 File Offset: 0x0010E874
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NModdingScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModdingScreen.PropertyName._modInfoContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModdingScreen.PropertyName._scrollableContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModdingScreen.PropertyName._modRowContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModdingScreen.PropertyName._pendingChangesWarning, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027D3 RID: 10195 RVA: 0x00110730 File Offset: 0x0010E930
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NModdingScreen.PropertyName._modInfoContainer, Variant.From<NModInfoContainer>(ref this._modInfoContainer));
			info.AddProperty(NModdingScreen.PropertyName._scrollableContainer, Variant.From<NScrollableContainer>(ref this._scrollableContainer));
			info.AddProperty(NModdingScreen.PropertyName._modRowContainer, Variant.From<Control>(ref this._modRowContainer));
			info.AddProperty(NModdingScreen.PropertyName._pendingChangesWarning, Variant.From<Control>(ref this._pendingChangesWarning));
		}

		/// <inheritdoc />
		// Token: 0x060027D4 RID: 10196 RVA: 0x0011079C File Offset: 0x0010E99C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NModdingScreen.PropertyName._modInfoContainer, ref variant))
			{
				this._modInfoContainer = variant.As<NModInfoContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NModdingScreen.PropertyName._scrollableContainer, ref variant2))
			{
				this._scrollableContainer = variant2.As<NScrollableContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NModdingScreen.PropertyName._modRowContainer, ref variant3))
			{
				this._modRowContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NModdingScreen.PropertyName._pendingChangesWarning, ref variant4))
			{
				this._pendingChangesWarning = variant4.As<Control>();
			}
		}

		// Token: 0x04000EC6 RID: 3782
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/modding/modding_screen");

		// Token: 0x04000EC7 RID: 3783
		private NModInfoContainer _modInfoContainer;

		// Token: 0x04000EC8 RID: 3784
		private NScrollableContainer _scrollableContainer;

		// Token: 0x04000EC9 RID: 3785
		private Control _modRowContainer;

		// Token: 0x04000ECA RID: 3786
		private Control _pendingChangesWarning;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001484 RID: 5252
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004DD8 RID: 19928
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DD9 RID: 19929
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004DDA RID: 19930
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004DDB RID: 19931
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnGetModsPressed' method.
			/// </summary>
			// Token: 0x04004DDC RID: 19932
			public static readonly StringName OnGetModsPressed = "OnGetModsPressed";

			/// <summary>
			/// Cached name for the 'OnMakeModsPressed' method.
			/// </summary>
			// Token: 0x04004DDD RID: 19933
			public static readonly StringName OnMakeModsPressed = "OnMakeModsPressed";

			/// <summary>
			/// Cached name for the 'OnRowSelected' method.
			/// </summary>
			// Token: 0x04004DDE RID: 19934
			public static readonly StringName OnRowSelected = "OnRowSelected";

			/// <summary>
			/// Cached name for the 'OnModEnabledOrDisabled' method.
			/// </summary>
			// Token: 0x04004DDF RID: 19935
			public static readonly StringName OnModEnabledOrDisabled = "OnModEnabledOrDisabled";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004DE0 RID: 19936
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001485 RID: 5253
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004DE1 RID: 19937
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_modInfoContainer' field.
			/// </summary>
			// Token: 0x04004DE2 RID: 19938
			public static readonly StringName _modInfoContainer = "_modInfoContainer";

			/// <summary>
			/// Cached name for the '_scrollableContainer' field.
			/// </summary>
			// Token: 0x04004DE3 RID: 19939
			public static readonly StringName _scrollableContainer = "_scrollableContainer";

			/// <summary>
			/// Cached name for the '_modRowContainer' field.
			/// </summary>
			// Token: 0x04004DE4 RID: 19940
			public static readonly StringName _modRowContainer = "_modRowContainer";

			/// <summary>
			/// Cached name for the '_pendingChangesWarning' field.
			/// </summary>
			// Token: 0x04004DE5 RID: 19941
			public static readonly StringName _pendingChangesWarning = "_pendingChangesWarning";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001486 RID: 5254
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
