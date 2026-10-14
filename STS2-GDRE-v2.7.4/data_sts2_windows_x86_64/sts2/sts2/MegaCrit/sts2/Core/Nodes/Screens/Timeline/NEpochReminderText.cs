using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x0200025F RID: 607
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochReminderText.cs")]
	public class NEpochReminderText : Control
	{
		// Token: 0x06001FA8 RID: 8104 RVA: 0x000D40AE File Offset: 0x000D22AE
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("%ReminderLabel");
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000D40C8 File Offset: 0x000D22C8
		public void AnimateIn()
		{
			int discoveredEpochCount = SaveManager.Instance.GetDiscoveredEpochCount();
			if (discoveredEpochCount == 0)
			{
				return;
			}
			base.Visible = true;
			this._loc = new LocString("timeline", "REMINDER_TEXT");
			this._loc.AddObj("RevealableEpochCount", discoveredEpochCount);
			this._label.Text = this._loc.GetFormattedText();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetLoops(0);
			this._tween.TweenProperty(this._label, "modulate:a", 1f, 0.8);
			this._tween.TweenProperty(this._label, "modulate:a", 0.25f, 0.8);
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000D41B0 File Offset: 0x000D23B0
		public void AnimateOut()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._label, "modulate:a", 0f, 0.25);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FAB RID: 8107 RVA: 0x000D420C File Offset: 0x000D240C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NEpochReminderText.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochReminderText.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochReminderText.MethodName.AnimateOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FAC RID: 8108 RVA: 0x000D42B0 File Offset: 0x000D24B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochReminderText.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochReminderText.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochReminderText.MethodName.AnimateOut && args.Count == 0)
			{
				this.AnimateOut();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001FAD RID: 8109 RVA: 0x000D4335 File Offset: 0x000D2535
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochReminderText.MethodName._Ready || (ref method) == NEpochReminderText.MethodName.AnimateIn || (ref method) == NEpochReminderText.MethodName.AnimateOut || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001FAE RID: 8110 RVA: 0x000D436C File Offset: 0x000D256C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochReminderText.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochReminderText.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochReminderText.PropertyName._vfxHolder)
			{
				this._vfxHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001FAF RID: 8111 RVA: 0x000D43D4 File Offset: 0x000D25D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochReminderText.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NEpochReminderText.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEpochReminderText.PropertyName._vfxHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._vfxHolder);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FB0 RID: 8112 RVA: 0x000D444C File Offset: 0x000D264C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEpochReminderText.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochReminderText.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochReminderText.PropertyName._vfxHolder, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FB1 RID: 8113 RVA: 0x000D44C4 File Offset: 0x000D26C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochReminderText.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NEpochReminderText.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEpochReminderText.PropertyName._vfxHolder, Variant.From<Control>(ref this._vfxHolder));
		}

		/// <inheritdoc />
		// Token: 0x06001FB2 RID: 8114 RVA: 0x000D451C File Offset: 0x000D271C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochReminderText.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochReminderText.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochReminderText.PropertyName._vfxHolder, ref variant3))
			{
				this._vfxHolder = variant3.As<Control>();
			}
		}

		// Token: 0x04000C4B RID: 3147
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x04000C4C RID: 3148
		[Nullable(1)]
		private LocString _loc;

		// Token: 0x04000C4D RID: 3149
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C4E RID: 3150
		[Nullable(1)]
		private Control _vfxHolder;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AA RID: 4778
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004895 RID: 18581
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04004896 RID: 18582
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'AnimateOut' method.
			/// </summary>
			// Token: 0x04004897 RID: 18583
			public static readonly StringName AnimateOut = "AnimateOut";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AB RID: 4779
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004898 RID: 18584
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004899 RID: 18585
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_vfxHolder' field.
			/// </summary>
			// Token: 0x0400489A RID: 18586
			public static readonly StringName _vfxHolder = "_vfxHolder";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AC RID: 4780
		public class SignalName : Control.SignalName
		{
		}
	}
}
