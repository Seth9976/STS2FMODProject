using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// The beige banner which appears on many many screens. Choose a Card!
	/// </summary>
	// Token: 0x020003D5 RID: 981
	[ScriptPath("res://src/Core/Nodes/CommonUi/NCommonBanner.cs")]
	public class NCommonBanner : Control
	{
		// Token: 0x06003B5F RID: 15199 RVA: 0x001C5D58 File Offset: 0x001C3F58
		public override void _Ready()
		{
			this.label = base.GetNode<MegaLabel>("%Label");
			base.Modulate = StsColors.transparentWhite;
			this._imgOffset = new Vector2(base.GetViewportRect().Size.X * 0.5f, base.GetViewportRect().Size.Y * 0.5f) - base.GlobalPosition;
			base.GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x001C5DFC File Offset: 0x001C3FFC
		private void OnWindowChange()
		{
			this._showPos = new Vector2(base.GetViewportRect().Size.X * 0.5f, base.GetViewportRect().Size.Y * 0.5f) - this._imgOffset;
			this._hidePos = this._showPos + NCommonBanner._hideOffset;
			base.Position = this._showPos;
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x001C5E74 File Offset: 0x001C4074
		public void AnimateIn()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.4).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this, "global_position", this._showPos, 0.4).SetEase(1L).SetTrans(10L)
				.From(this._hidePos);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001C5F28 File Offset: 0x001C4128
		public void AnimateOut()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.4).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Call this when you want to change the text on an existing banner without animating the banner again.
		/// </summary>
		// Token: 0x06003B63 RID: 15203 RVA: 0x001C5F90 File Offset: 0x001C4190
		[NullableContext(1)]
		public void ChangeText(string text)
		{
			this.label.SetTextAutoSize(text);
			this.label.Modulate = StsColors.transparentWhite;
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween();
			this._labelTween.TweenProperty(this.label, "modulate:a", 1f, 0.25);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B64 RID: 15204 RVA: 0x001C6008 File Offset: 0x001C4208
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCommonBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonBanner.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonBanner.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonBanner.MethodName.AnimateOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonBanner.MethodName.ChangeText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B65 RID: 15205 RVA: 0x001C6130 File Offset: 0x001C4330
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCommonBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonBanner.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonBanner.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonBanner.MethodName.AnimateOut && args.Count == 0)
			{
				this.AnimateOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonBanner.MethodName.ChangeText && args.Count == 1)
			{
				this.ChangeText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B66 RID: 15206 RVA: 0x001C6210 File Offset: 0x001C4410
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCommonBanner.MethodName._Ready || (ref method) == NCommonBanner.MethodName.OnWindowChange || (ref method) == NCommonBanner.MethodName.AnimateIn || (ref method) == NCommonBanner.MethodName.AnimateOut || (ref method) == NCommonBanner.MethodName.ChangeText || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003B67 RID: 15207 RVA: 0x001C6270 File Offset: 0x001C4470
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCommonBanner.PropertyName.label)
			{
				this.label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._labelTween)
			{
				this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._showPos)
			{
				this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._hidePos)
			{
				this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._imgOffset)
			{
				this._imgOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003B68 RID: 15208 RVA: 0x001C6328 File Offset: 0x001C4528
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCommonBanner.PropertyName.label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this.label);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._labelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._showPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._hidePos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
				return true;
			}
			if ((ref name) == NCommonBanner.PropertyName._imgOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._imgOffset);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B69 RID: 15209 RVA: 0x001C6400 File Offset: 0x001C4600
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCommonBanner.PropertyName.label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCommonBanner.PropertyName._labelTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCommonBanner.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCommonBanner.PropertyName._showPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCommonBanner.PropertyName._hidePos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCommonBanner.PropertyName._imgOffset, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B6A RID: 15210 RVA: 0x001C64D8 File Offset: 0x001C46D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCommonBanner.PropertyName.label, Variant.From<MegaLabel>(ref this.label));
			info.AddProperty(NCommonBanner.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
			info.AddProperty(NCommonBanner.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCommonBanner.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
			info.AddProperty(NCommonBanner.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
			info.AddProperty(NCommonBanner.PropertyName._imgOffset, Variant.From<Vector2>(ref this._imgOffset));
		}

		/// <inheritdoc />
		// Token: 0x06003B6B RID: 15211 RVA: 0x001C6570 File Offset: 0x001C4770
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCommonBanner.PropertyName.label, ref variant))
			{
				this.label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCommonBanner.PropertyName._labelTween, ref variant2))
			{
				this._labelTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCommonBanner.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCommonBanner.PropertyName._showPos, ref variant4))
			{
				this._showPos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCommonBanner.PropertyName._hidePos, ref variant5))
			{
				this._hidePos = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCommonBanner.PropertyName._imgOffset, ref variant6))
			{
				this._imgOffset = variant6.As<Vector2>();
			}
		}

		/// NOTE: This is set to public for modders as it's a common UI element.
		// Token: 0x0400172B RID: 5931
		[Nullable(1)]
		public MegaLabel label;

		// Token: 0x0400172C RID: 5932
		[Nullable(2)]
		private Tween _labelTween;

		// Token: 0x0400172D RID: 5933
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400172E RID: 5934
		private Vector2 _showPos;

		// Token: 0x0400172F RID: 5935
		private Vector2 _hidePos;

		// Token: 0x04001730 RID: 5936
		private static readonly Vector2 _hideOffset = new Vector2(0f, 50f);

		// Token: 0x04001731 RID: 5937
		private Vector2 _imgOffset;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001825 RID: 6181
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E6E RID: 24174
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005E6F RID: 24175
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04005E70 RID: 24176
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'AnimateOut' method.
			/// </summary>
			// Token: 0x04005E71 RID: 24177
			public static readonly StringName AnimateOut = "AnimateOut";

			/// <summary>
			/// Cached name for the 'ChangeText' method.
			/// </summary>
			// Token: 0x04005E72 RID: 24178
			public static readonly StringName ChangeText = "ChangeText";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001826 RID: 6182
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'label' field.
			/// </summary>
			// Token: 0x04005E73 RID: 24179
			public static readonly StringName label = "label";

			/// <summary>
			/// Cached name for the '_labelTween' field.
			/// </summary>
			// Token: 0x04005E74 RID: 24180
			public static readonly StringName _labelTween = "_labelTween";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005E75 RID: 24181
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_showPos' field.
			/// </summary>
			// Token: 0x04005E76 RID: 24182
			public static readonly StringName _showPos = "_showPos";

			/// <summary>
			/// Cached name for the '_hidePos' field.
			/// </summary>
			// Token: 0x04005E77 RID: 24183
			public static readonly StringName _hidePos = "_hidePos";

			/// <summary>
			/// Cached name for the '_imgOffset' field.
			/// </summary>
			// Token: 0x04005E78 RID: 24184
			public static readonly StringName _imgOffset = "_imgOffset";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001827 RID: 6183
		public class SignalName : Control.SignalName
		{
		}
	}
}
