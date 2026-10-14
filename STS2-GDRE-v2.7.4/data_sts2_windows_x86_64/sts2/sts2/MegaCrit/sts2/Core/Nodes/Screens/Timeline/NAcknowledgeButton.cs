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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000259 RID: 601
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NAcknowledgeButton.cs")]
	public class NAcknowledgeButton : NButton
	{
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x000CFF23 File Offset: 0x000CE123
		[Nullable(1)]
		protected override string ClickedSfx
		{
			[NullableContext(1)]
			get
			{
				return "event:/sfx/ui/timeline/ui_timeline_close_epoch";
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000CFF2A File Offset: 0x000CE12A
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000CFF3F File Offset: 0x000CE13F
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("%Label");
			this._label.SetTextAutoSize(new LocString("timeline", "ACKNOWLEDGE_BUTTON").GetRawText());
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000CFF7C File Offset: 0x000CE17C
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

		// Token: 0x06001F44 RID: 8004 RVA: 0x000CFFE8 File Offset: 0x000CE1E8
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

		// Token: 0x06001F45 RID: 8005 RVA: 0x000D0058 File Offset: 0x000CE258
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

		// Token: 0x06001F46 RID: 8006 RVA: 0x000D00D0 File Offset: 0x000CE2D0
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.05);
			base.MouseFilter = 2L;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F47 RID: 8007 RVA: 0x000D0138 File Offset: 0x000CE338
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAcknowledgeButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAcknowledgeButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAcknowledgeButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAcknowledgeButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAcknowledgeButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F48 RID: 8008 RVA: 0x000D0238 File Offset: 0x000CE438
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAcknowledgeButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcknowledgeButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcknowledgeButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcknowledgeButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcknowledgeButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F49 RID: 8009 RVA: 0x000D0308 File Offset: 0x000CE508
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAcknowledgeButton.MethodName._Ready || (ref method) == NAcknowledgeButton.MethodName.OnFocus || (ref method) == NAcknowledgeButton.MethodName.OnUnfocus || (ref method) == NAcknowledgeButton.MethodName.OnPress || (ref method) == NAcknowledgeButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001F4A RID: 8010 RVA: 0x000D0367 File Offset: 0x000CE567
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAcknowledgeButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAcknowledgeButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001F4B RID: 8011 RVA: 0x000D03A8 File Offset: 0x000CE5A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAcknowledgeButton.PropertyName.ClickedSfx)
			{
				string clickedSfx = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref clickedSfx);
				return true;
			}
			if ((ref name) == NAcknowledgeButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NAcknowledgeButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NAcknowledgeButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F4C RID: 8012 RVA: 0x000D0444 File Offset: 0x000CE644
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NAcknowledgeButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAcknowledgeButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAcknowledgeButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(34L, NAcknowledgeButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F4D RID: 8013 RVA: 0x000D04DB File Offset: 0x000CE6DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAcknowledgeButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NAcknowledgeButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06001F4E RID: 8014 RVA: 0x000D0510 File Offset: 0x000CE710
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAcknowledgeButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAcknowledgeButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x04000C12 RID: 3090
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C13 RID: 3091
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001296 RID: 4758
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400482D RID: 18477
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400482E RID: 18478
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400482F RID: 18479
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004830 RID: 18480
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004831 RID: 18481
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001297 RID: 4759
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04004832 RID: 18482
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04004833 RID: 18483
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004834 RID: 18484
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004835 RID: 18485
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001298 RID: 4760
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
