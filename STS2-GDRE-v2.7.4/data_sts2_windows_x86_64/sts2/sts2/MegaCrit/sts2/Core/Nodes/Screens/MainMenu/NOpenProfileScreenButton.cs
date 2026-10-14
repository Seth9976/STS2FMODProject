using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Button on the main menu which opens the profile screen, allowing the player to switch profiles.
	/// </summary>
	// Token: 0x0200030E RID: 782
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NOpenProfileScreenButton.cs")]
	public class NOpenProfileScreenButton : NButton
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x001307F9 File Offset: 0x0012E9F9
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.pauseAndBack };
			}
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x00130810 File Offset: 0x0012EA10
		public override void _Ready()
		{
			this.ConnectSignals();
			this._profileIcon = base.GetNode<NProfileIcon>("ProfileIcon");
			this._title = base.GetNode<MegaLabel>("Title");
			this._description = base.GetNode<MegaLabel>("Description");
			this.RefreshLabels();
			this._profileIcon.SetProfileId(SaveManager.Instance.CurrentProfileId);
			this.UpdateDescription();
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x00130888 File Offset: 0x0012EA88
		public override void _EnterTree()
		{
			base._EnterTree();
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateDescription)), 0U);
				NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateDescription)), 0U);
			}
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x001308E8 File Offset: 0x0012EAE8
		public override void _ExitTree()
		{
			base._ExitTree();
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateDescription)));
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateDescription)));
			}
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x00130942 File Offset: 0x0012EB42
		public override void _Notification(int what)
		{
			if ((long)what == 2010L && base.IsNodeReady())
			{
				this.RefreshLabels();
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0013095C File Offset: 0x0012EB5C
		private void RefreshLabels()
		{
			this._titleLoc.Add("Id", SaveManager.Instance.CurrentProfileId);
			this._title.SetTextAutoSize(this._titleLoc.GetFormattedText());
			this._description.SetTextAutoSize(NOpenProfileScreenButton._descriptionLoc.GetFormattedText());
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x001309B3 File Offset: 0x0012EBB3
		protected override void OnRelease()
		{
			NGame.Instance.MainMenu.OpenProfileScreen();
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x001309C4 File Offset: 0x0012EBC4
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Scale = Vector2.One * 1.02f;
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x001309F4 File Offset: 0x0012EBF4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "scale", Vector2.One * 1f, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x00130A66 File Offset: 0x0012EC66
		private void UpdateDescription()
		{
			if (NControllerManager.Instance != null)
			{
				this._description.SetVisible(!NControllerManager.Instance.IsUsingController);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B3A RID: 11066 RVA: 0x00130A88 File Offset: 0x0012EC88
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NOpenProfileScreenButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName.RefreshLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenProfileScreenButton.MethodName.UpdateDescription, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B3B RID: 11067 RVA: 0x00130C6C File Offset: 0x0012EE6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOpenProfileScreenButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName.RefreshLabels && args.Count == 0)
			{
				this.RefreshLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenProfileScreenButton.MethodName.UpdateDescription && args.Count == 0)
			{
				this.UpdateDescription();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002B3C RID: 11068 RVA: 0x00130DE0 File Offset: 0x0012EFE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOpenProfileScreenButton.MethodName._Ready || (ref method) == NOpenProfileScreenButton.MethodName._EnterTree || (ref method) == NOpenProfileScreenButton.MethodName._ExitTree || (ref method) == NOpenProfileScreenButton.MethodName._Notification || (ref method) == NOpenProfileScreenButton.MethodName.RefreshLabels || (ref method) == NOpenProfileScreenButton.MethodName.OnRelease || (ref method) == NOpenProfileScreenButton.MethodName.OnFocus || (ref method) == NOpenProfileScreenButton.MethodName.OnUnfocus || (ref method) == NOpenProfileScreenButton.MethodName.UpdateDescription || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B3D RID: 11069 RVA: 0x00130E7C File Offset: 0x0012F07C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOpenProfileScreenButton.PropertyName._profileIcon)
			{
				this._profileIcon = VariantUtils.ConvertTo<NProfileIcon>(ref value);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B3E RID: 11070 RVA: 0x00130F00 File Offset: 0x0012F100
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOpenProfileScreenButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._profileIcon)
			{
				value = VariantUtils.CreateFrom<NProfileIcon>(ref this._profileIcon);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NOpenProfileScreenButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B3F RID: 11071 RVA: 0x00130FB8 File Offset: 0x0012F1B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NOpenProfileScreenButton.PropertyName._profileIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOpenProfileScreenButton.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOpenProfileScreenButton.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOpenProfileScreenButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NOpenProfileScreenButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B40 RID: 11072 RVA: 0x00131074 File Offset: 0x0012F274
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NOpenProfileScreenButton.PropertyName._profileIcon, Variant.From<NProfileIcon>(ref this._profileIcon));
			info.AddProperty(NOpenProfileScreenButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
			info.AddProperty(NOpenProfileScreenButton.PropertyName._description, Variant.From<MegaLabel>(ref this._description));
			info.AddProperty(NOpenProfileScreenButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002B41 RID: 11073 RVA: 0x001310E0 File Offset: 0x0012F2E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._profileIcon, ref variant))
			{
				this._profileIcon = variant.As<NProfileIcon>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._title, ref variant2))
			{
				this._title = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x0400103C RID: 4156
		[Nullable(1)]
		private readonly LocString _titleLoc = new LocString("main_menu_ui", "OPEN_PROFILE_SCREEN.title");

		// Token: 0x0400103D RID: 4157
		[Nullable(1)]
		private static readonly LocString _descriptionLoc = new LocString("main_menu_ui", "OPEN_PROFILE_SCREEN.description");

		// Token: 0x0400103E RID: 4158
		[Nullable(1)]
		private NProfileIcon _profileIcon;

		// Token: 0x0400103F RID: 4159
		[Nullable(1)]
		private MegaLabel _title;

		// Token: 0x04001040 RID: 4160
		[Nullable(1)]
		private MegaLabel _description;

		// Token: 0x04001041 RID: 4161
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001523 RID: 5411
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040050BB RID: 20667
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040050BC RID: 20668
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040050BD RID: 20669
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x040050BE RID: 20670
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'RefreshLabels' method.
			/// </summary>
			// Token: 0x040050BF RID: 20671
			public static readonly StringName RefreshLabels = "RefreshLabels";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040050C0 RID: 20672
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040050C1 RID: 20673
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040050C2 RID: 20674
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateDescription' method.
			/// </summary>
			// Token: 0x040050C3 RID: 20675
			public static readonly StringName UpdateDescription = "UpdateDescription";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001524 RID: 5412
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040050C4 RID: 20676
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_profileIcon' field.
			/// </summary>
			// Token: 0x040050C5 RID: 20677
			public static readonly StringName _profileIcon = "_profileIcon";

			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x040050C6 RID: 20678
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x040050C7 RID: 20679
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040050C8 RID: 20680
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001525 RID: 5413
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
