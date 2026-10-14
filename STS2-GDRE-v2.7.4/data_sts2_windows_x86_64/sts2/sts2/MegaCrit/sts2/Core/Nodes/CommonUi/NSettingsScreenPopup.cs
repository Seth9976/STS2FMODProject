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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A popup ui/modal which is used for the Reset Tutorials confirmation
	/// </summary>
	// Token: 0x020003ED RID: 1005
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NSettingsScreenPopup.cs")]
	public class NSettingsScreenPopup : Control, IScreenContext
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003D59 RID: 15705 RVA: 0x001D5F7E File Offset: 0x001D417E
		// (set) Token: 0x06003D5A RID: 15706 RVA: 0x001D5F86 File Offset: 0x001D4186
		public NVerticalPopup VerticalPopup { get; private set; }

		// Token: 0x06003D5B RID: 15707 RVA: 0x001D5F90 File Offset: 0x001D4190
		[return: Nullable(2)]
		public static NSettingsScreenPopup Create(LocString header, LocString description)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSettingsScreenPopup nsettingsScreenPopup = PreloadManager.Cache.GetScene(NSettingsScreenPopup._scenePath).Instantiate<NSettingsScreenPopup>(0L);
			nsettingsScreenPopup._header = header;
			nsettingsScreenPopup._description = description;
			return nsettingsScreenPopup;
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x001D5FCC File Offset: 0x001D41CC
		public override void _Ready()
		{
			this.VerticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this.VerticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), delegate(NButton _)
			{
			});
			this.VerticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
			this.VerticalPopup.SetText(this._header, this._description);
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x001D6065 File Offset: 0x001D4265
		private void OnYesButtonPressed(NButton _)
		{
			Log.Info("FTUEs have been reset!", 2);
			SaveManager.Instance.ResetFtues();
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x001D607C File Offset: 0x001D427C
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
		// Token: 0x06003D5F RID: 15711 RVA: 0x001D6080 File Offset: 0x001D4280
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSettingsScreenPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreenPopup.MethodName.OnYesButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D60 RID: 15712 RVA: 0x001D6124 File Offset: 0x001D4324
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsScreenPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreenPopup.MethodName.OnYesButtonPressed && args.Count == 1)
			{
				this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003D61 RID: 15713 RVA: 0x001D6192 File Offset: 0x001D4392
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsScreenPopup.MethodName._Ready || (ref method) == NSettingsScreenPopup.MethodName.OnYesButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003D62 RID: 15714 RVA: 0x001D61B9 File Offset: 0x001D43B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsScreenPopup.PropertyName.VerticalPopup)
			{
				this.VerticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			if ((ref name) == NSettingsScreenPopup.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003D63 RID: 15715 RVA: 0x001D61FC File Offset: 0x001D43FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsScreenPopup.PropertyName.VerticalPopup)
			{
				NVerticalPopup verticalPopup = this.VerticalPopup;
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref verticalPopup);
				return true;
			}
			if ((ref name) == NSettingsScreenPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NSettingsScreenPopup.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D64 RID: 15716 RVA: 0x001D6278 File Offset: 0x001D4478
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsScreenPopup.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreenPopup.PropertyName.VerticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreenPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D65 RID: 15717 RVA: 0x001D62F0 File Offset: 0x001D44F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName verticalPopup = NSettingsScreenPopup.PropertyName.VerticalPopup;
			NVerticalPopup verticalPopup2 = this.VerticalPopup;
			info.AddProperty(verticalPopup, Variant.From<NVerticalPopup>(ref verticalPopup2));
			info.AddProperty(NSettingsScreenPopup.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
		}

		/// <inheritdoc />
		// Token: 0x06003D66 RID: 15718 RVA: 0x001D6334 File Offset: 0x001D4534
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsScreenPopup.PropertyName.VerticalPopup, ref variant))
			{
				this.VerticalPopup = variant.As<NVerticalPopup>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsScreenPopup.PropertyName._backstop, ref variant2))
			{
				this._backstop = variant2.As<ColorRect>();
			}
		}

		// Token: 0x040017E6 RID: 6118
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/settings_screen_popup");

		// Token: 0x040017E7 RID: 6119
		private ColorRect _backstop;

		// Token: 0x040017E8 RID: 6120
		private LocString _header;

		// Token: 0x040017E9 RID: 6121
		private LocString _description;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001889 RID: 6281
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FE2 RID: 24546
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnYesButtonPressed' method.
			/// </summary>
			// Token: 0x04005FE3 RID: 24547
			public static readonly StringName OnYesButtonPressed = "OnYesButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200188A RID: 6282
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'VerticalPopup' property.
			/// </summary>
			// Token: 0x04005FE4 RID: 24548
			public static readonly StringName VerticalPopup = "VerticalPopup";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005FE5 RID: 24549
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04005FE6 RID: 24550
			public static readonly StringName _backstop = "_backstop";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200188B RID: 6283
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
