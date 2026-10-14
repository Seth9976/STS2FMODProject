using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// The disconnection confirmation popup to prevent the player from accidentally leaving a run in progress.
	/// Renders above the capstone screens (above top bar).
	/// </summary>
	// Token: 0x020003D9 RID: 985
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NDisconnectConfirmPopup.cs")]
	public class NDisconnectConfirmPopup : Control, IScreenContext
	{
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x001C943F File Offset: 0x001C763F
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDisconnectConfirmPopup._scenePath);
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001C944C File Offset: 0x001C764C
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(new LocString("settings_ui", "DISCONNECT_CONFIRMATION.header"), new LocString("settings_ui", "DISCONNECT_CONFIRMATION.body"));
			this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
			this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x001C94E4 File Offset: 0x001C76E4
		[NullableContext(2)]
		public static NDisconnectConfirmPopup Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDisconnectConfirmPopup._scenePath).Instantiate<NDisconnectConfirmPopup>(0L);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001C9512 File Offset: 0x001C7712
		private void OnYesButtonPressed(NButton _)
		{
			RunManager.Instance.NetService.Disconnect(NetError.Quit, false);
			this.QueueFreeSafely();
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x001C952B File Offset: 0x001C772B
		private void OnNoButtonPressed(NButton _)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x001C9533 File Offset: 0x001C7733
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
		// Token: 0x06003BC9 RID: 15305 RVA: 0x001C9538 File Offset: 0x001C7738
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDisconnectConfirmPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDisconnectConfirmPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NDisconnectConfirmPopup.MethodName.OnYesButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDisconnectConfirmPopup.MethodName.OnNoButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BCA RID: 15306 RVA: 0x001C9678 File Offset: 0x001C7878
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDisconnectConfirmPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDisconnectConfirmPopup.MethodName.Create && args.Count == 0)
			{
				NDisconnectConfirmPopup ndisconnectConfirmPopup = NDisconnectConfirmPopup.Create();
				ret = VariantUtils.CreateFrom<NDisconnectConfirmPopup>(ref ndisconnectConfirmPopup);
				return true;
			}
			if ((ref method) == NDisconnectConfirmPopup.MethodName.OnYesButtonPressed && args.Count == 1)
			{
				this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDisconnectConfirmPopup.MethodName.OnNoButtonPressed && args.Count == 1)
			{
				this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x001C9744 File Offset: 0x001C7944
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDisconnectConfirmPopup.MethodName.Create && args.Count == 0)
			{
				NDisconnectConfirmPopup ndisconnectConfirmPopup = NDisconnectConfirmPopup.Create();
				ret = VariantUtils.CreateFrom<NDisconnectConfirmPopup>(ref ndisconnectConfirmPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003BCC RID: 15308 RVA: 0x001C9784 File Offset: 0x001C7984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDisconnectConfirmPopup.MethodName._Ready || (ref method) == NDisconnectConfirmPopup.MethodName.Create || (ref method) == NDisconnectConfirmPopup.MethodName.OnYesButtonPressed || (ref method) == NDisconnectConfirmPopup.MethodName.OnNoButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003BCD RID: 15309 RVA: 0x001C97D4 File Offset: 0x001C79D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._noButton)
			{
				this._noButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._yesButton)
			{
				this._yesButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._mainMenuNode)
			{
				this._mainMenuNode = VariantUtils.ConvertTo<NMainMenu>(ref value);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003BCE RID: 15310 RVA: 0x001C988C File Offset: 0x001C7A8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDisconnectConfirmPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._noButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._noButton);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._yesButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._yesButton);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._mainMenuNode)
			{
				value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenuNode);
				return true;
			}
			if ((ref name) == NDisconnectConfirmPopup.PropertyName._verticalPopup)
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
		// Token: 0x06003BCF RID: 15311 RVA: 0x001C9984 File Offset: 0x001C7B84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._noButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._yesButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._mainMenuNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDisconnectConfirmPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BD0 RID: 15312 RVA: 0x001C9A80 File Offset: 0x001C7C80
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._noButton, Variant.From<NButton>(ref this._noButton));
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._yesButton, Variant.From<NButton>(ref this._yesButton));
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._mainMenuNode, Variant.From<NMainMenu>(ref this._mainMenuNode));
			info.AddProperty(NDisconnectConfirmPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
		}

		/// <inheritdoc />
		// Token: 0x06003BD1 RID: 15313 RVA: 0x001C9B18 File Offset: 0x001C7D18
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._header, ref variant))
			{
				this._header = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._description, ref variant2))
			{
				this._description = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._noButton, ref variant3))
			{
				this._noButton = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._yesButton, ref variant4))
			{
				this._yesButton = variant4.As<NButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._mainMenuNode, ref variant5))
			{
				this._mainMenuNode = variant5.As<NMainMenu>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._verticalPopup, ref variant6))
			{
				this._verticalPopup = variant6.As<NVerticalPopup>();
			}
		}

		// Token: 0x0400175E RID: 5982
		private MegaLabel _header;

		// Token: 0x0400175F RID: 5983
		private MegaRichTextLabel _description;

		// Token: 0x04001760 RID: 5984
		private NButton _noButton;

		// Token: 0x04001761 RID: 5985
		private NButton _yesButton;

		// Token: 0x04001762 RID: 5986
		[Nullable(2)]
		private NMainMenu _mainMenuNode;

		// Token: 0x04001763 RID: 5987
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/disconnect_confirm_popup");

		// Token: 0x04001764 RID: 5988
		private NVerticalPopup _verticalPopup;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001839 RID: 6201
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005ECF RID: 24271
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005ED0 RID: 24272
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'OnYesButtonPressed' method.
			/// </summary>
			// Token: 0x04005ED1 RID: 24273
			public static readonly StringName OnYesButtonPressed = "OnYesButtonPressed";

			/// <summary>
			/// Cached name for the 'OnNoButtonPressed' method.
			/// </summary>
			// Token: 0x04005ED2 RID: 24274
			public static readonly StringName OnNoButtonPressed = "OnNoButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200183A RID: 6202
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005ED3 RID: 24275
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005ED4 RID: 24276
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005ED5 RID: 24277
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_noButton' field.
			/// </summary>
			// Token: 0x04005ED6 RID: 24278
			public static readonly StringName _noButton = "_noButton";

			/// <summary>
			/// Cached name for the '_yesButton' field.
			/// </summary>
			// Token: 0x04005ED7 RID: 24279
			public static readonly StringName _yesButton = "_yesButton";

			/// <summary>
			/// Cached name for the '_mainMenuNode' field.
			/// </summary>
			// Token: 0x04005ED8 RID: 24280
			public static readonly StringName _mainMenuNode = "_mainMenuNode";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04005ED9 RID: 24281
			public static readonly StringName _verticalPopup = "_verticalPopup";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200183B RID: 6203
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
