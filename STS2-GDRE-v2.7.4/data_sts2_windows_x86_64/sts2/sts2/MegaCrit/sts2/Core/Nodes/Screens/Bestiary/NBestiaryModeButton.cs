using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000365 RID: 869
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryModeButton.cs")]
	public class NBestiaryModeButton : NButton
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x00178BF1 File Offset: 0x00176DF1
		[Nullable(1)]
		protected override string ClickedSfx
		{
			[NullableContext(1)]
			get
			{
				return "event:/sfx/ui/timeline/ui_timeline_close_epoch";
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x00178BF8 File Offset: 0x00176DF8
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x00178C0D File Offset: 0x00176E0D
		[Nullable(1)]
		protected override string ControllerIconHotkey
		{
			[NullableContext(1)]
			get
			{
				return MegaInput.accept;
			}
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00178C19 File Offset: 0x00176E19
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00178C24 File Offset: 0x00176E24
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00178C90 File Offset: 0x00176E90
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00178D00 File Offset: 0x00176F00
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.25).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003297 RID: 12951 RVA: 0x00178D78 File Offset: 0x00176F78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NBestiaryModeButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryModeButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryModeButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryModeButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003298 RID: 12952 RVA: 0x00178E4C File Offset: 0x0017704C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryModeButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryModeButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryModeButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryModeButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003299 RID: 12953 RVA: 0x00178EF8 File Offset: 0x001770F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryModeButton.MethodName._Ready || (ref method) == NBestiaryModeButton.MethodName.OnFocus || (ref method) == NBestiaryModeButton.MethodName.OnUnfocus || (ref method) == NBestiaryModeButton.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600329A RID: 12954 RVA: 0x00178F48 File Offset: 0x00177148
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryModeButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600329B RID: 12955 RVA: 0x00178F70 File Offset: 0x00177170
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryModeButton.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryModeButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NBestiaryModeButton.PropertyName.ControllerIconHotkey)
			{
				string text = this.ControllerIconHotkey;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryModeButton.PropertyName._tween)
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
		// Token: 0x0600329C RID: 12956 RVA: 0x00179010 File Offset: 0x00177210
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NBestiaryModeButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryModeButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NBestiaryModeButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(4L, NBestiaryModeButton.PropertyName.ControllerIconHotkey, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600329D RID: 12957 RVA: 0x001790A6 File Offset: 0x001772A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryModeButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600329E RID: 12958 RVA: 0x001790C8 File Offset: 0x001772C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryModeButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x040013B1 RID: 5041
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001671 RID: 5745
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400577A RID: 22394
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400577B RID: 22395
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400577C RID: 22396
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400577D RID: 22397
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001672 RID: 5746
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x0400577E RID: 22398
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x0400577F RID: 22399
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'ControllerIconHotkey' property.
			/// </summary>
			// Token: 0x04005780 RID: 22400
			public new static readonly StringName ControllerIconHotkey = "ControllerIconHotkey";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005781 RID: 22401
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001673 RID: 5747
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
