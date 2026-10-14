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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// The Abandon Run confirmation popup to prevent the player from accidentally deleting a run in progress.
	/// Used both mid-run and in the main menu.
	/// Renders above the capstone screens (above top bar).
	/// </summary>
	// Token: 0x020003D1 RID: 977
	[ScriptPath("res://src/Core/Nodes/CommonUi/NAbandonRunConfirmPopup.cs")]
	public class NAbandonRunConfirmPopup : Control, IScreenContext
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x001C3357 File Offset: 0x001C1557
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NAbandonRunConfirmPopup._scenePath);
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x001C3364 File Offset: 0x001C1564
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.header"), new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.body"));
			this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
			this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001C33FC File Offset: 0x001C15FC
		public override void _EnterTree()
		{
			base.Modulate = StsColors.transparentBlack;
			float y = base.Position.Y;
			base.Position += new Vector2(0f, 100f);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.1);
			this._tween.TweenProperty(this, "position:y", y, 0.3).SetEase(1L).SetTrans(10L);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x001C34C2 File Offset: 0x001C16C2
		public override void _ExitTree()
		{
			this._verticalPopup.DisconnectSignals();
			this._verticalPopup.DisconnectHotkeys();
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x001C34DC File Offset: 0x001C16DC
		[NullableContext(2)]
		public static NAbandonRunConfirmPopup Create(NMainMenu mainMenu)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NAbandonRunConfirmPopup nabandonRunConfirmPopup = PreloadManager.Cache.GetScene(NAbandonRunConfirmPopup._scenePath).Instantiate<NAbandonRunConfirmPopup>(0L);
			nabandonRunConfirmPopup._mainMenuNode = mainMenu;
			return nabandonRunConfirmPopup;
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x001C3511 File Offset: 0x001C1711
		[NullableContext(1)]
		private void OnYesButtonPressed(NButton _)
		{
			if (this._mainMenuNode == null)
			{
				RunManager.Instance.Abandon();
			}
			else
			{
				this._mainMenuNode.AbandonRun();
			}
			this.QueueFreeSafely();
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x001C3538 File Offset: 0x001C1738
		[NullableContext(1)]
		private void OnNoButtonPressed(NButton _)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x001C3540 File Offset: 0x001C1740
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
		// Token: 0x06003B21 RID: 15137 RVA: 0x001C3544 File Offset: 0x001C1744
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NAbandonRunConfirmPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAbandonRunConfirmPopup.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAbandonRunConfirmPopup.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAbandonRunConfirmPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mainMenu", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B22 RID: 15138 RVA: 0x001C3710 File Offset: 0x001C1910
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAbandonRunConfirmPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAbandonRunConfirmPopup.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAbandonRunConfirmPopup.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAbandonRunConfirmPopup.MethodName.Create && args.Count == 1)
			{
				NAbandonRunConfirmPopup nabandonRunConfirmPopup = NAbandonRunConfirmPopup.Create(VariantUtils.ConvertTo<NMainMenu>(args[0]));
				ret = VariantUtils.CreateFrom<NAbandonRunConfirmPopup>(ref nabandonRunConfirmPopup);
				return true;
			}
			if ((ref method) == NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed && args.Count == 1)
			{
				this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed && args.Count == 1)
			{
				this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x001C3834 File Offset: 0x001C1A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAbandonRunConfirmPopup.MethodName.Create && args.Count == 1)
			{
				NAbandonRunConfirmPopup nabandonRunConfirmPopup = NAbandonRunConfirmPopup.Create(VariantUtils.ConvertTo<NMainMenu>(args[0]));
				ret = VariantUtils.CreateFrom<NAbandonRunConfirmPopup>(ref nabandonRunConfirmPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003B24 RID: 15140 RVA: 0x001C3884 File Offset: 0x001C1A84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAbandonRunConfirmPopup.MethodName._Ready || (ref method) == NAbandonRunConfirmPopup.MethodName._EnterTree || (ref method) == NAbandonRunConfirmPopup.MethodName._ExitTree || (ref method) == NAbandonRunConfirmPopup.MethodName.Create || (ref method) == NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed || (ref method) == NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003B25 RID: 15141 RVA: 0x001C38F4 File Offset: 0x001C1AF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._mainMenuNode)
			{
				this._mainMenuNode = VariantUtils.ConvertTo<NMainMenu>(ref value);
				return true;
			}
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003B26 RID: 15142 RVA: 0x001C395C File Offset: 0x001C1B5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._mainMenuNode)
			{
				value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenuNode);
				return true;
			}
			if ((ref name) == NAbandonRunConfirmPopup.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B27 RID: 15143 RVA: 0x001C39F4 File Offset: 0x001C1BF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAbandonRunConfirmPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAbandonRunConfirmPopup.PropertyName._mainMenuNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAbandonRunConfirmPopup.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAbandonRunConfirmPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B28 RID: 15144 RVA: 0x001C3A8C File Offset: 0x001C1C8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAbandonRunConfirmPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
			info.AddProperty(NAbandonRunConfirmPopup.PropertyName._mainMenuNode, Variant.From<NMainMenu>(ref this._mainMenuNode));
			info.AddProperty(NAbandonRunConfirmPopup.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003B29 RID: 15145 RVA: 0x001C3AE4 File Offset: 0x001C1CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._verticalPopup, ref variant))
			{
				this._verticalPopup = variant.As<NVerticalPopup>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._mainMenuNode, ref variant2))
			{
				this._mainMenuNode = variant2.As<NMainMenu>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
		}

		// Token: 0x0400170D RID: 5901
		[Nullable(1)]
		private NVerticalPopup _verticalPopup;

		// Token: 0x0400170E RID: 5902
		[Nullable(2)]
		private NMainMenu _mainMenuNode;

		// Token: 0x0400170F RID: 5903
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001710 RID: 5904
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/abandon_run_confirm_popup");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001819 RID: 6169
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E39 RID: 24121
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005E3A RID: 24122
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005E3B RID: 24123
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005E3C RID: 24124
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'OnYesButtonPressed' method.
			/// </summary>
			// Token: 0x04005E3D RID: 24125
			public static readonly StringName OnYesButtonPressed = "OnYesButtonPressed";

			/// <summary>
			/// Cached name for the 'OnNoButtonPressed' method.
			/// </summary>
			// Token: 0x04005E3E RID: 24126
			public static readonly StringName OnNoButtonPressed = "OnNoButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181A RID: 6170
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005E3F RID: 24127
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04005E40 RID: 24128
			public static readonly StringName _verticalPopup = "_verticalPopup";

			/// <summary>
			/// Cached name for the '_mainMenuNode' field.
			/// </summary>
			// Token: 0x04005E41 RID: 24129
			public static readonly StringName _mainMenuNode = "_mainMenuNode";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005E42 RID: 24130
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181B RID: 6171
		public class SignalName : Control.SignalName
		{
		}
	}
}
