using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Confirm Button used in various places to confirm choices, embark on a run, etc.
	/// Very useful but not as cool as Back Button but that's life.
	/// Use the ButtonReleased signal to handle events.
	/// </summary>
	// Token: 0x020003E5 RID: 997
	[ScriptPath("res://src/Core/Nodes/CommonUi/NMiscConfirmButton.cs")]
	public class NMiscConfirmButton : NButton
	{
		// Token: 0x06003CB3 RID: 15539 RVA: 0x001D0838 File Offset: 0x001CEA38
		public override void _Ready()
		{
			this.ConnectSignals();
			this._isEnabled = false;
			this._buttonImage = base.GetNode<Control>("Image");
			base.GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x001D0896 File Offset: 0x001CEA96
		public override void _ExitTree()
		{
			base._ExitTree();
			CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
			if (pressDownCancelToken != null)
			{
				pressDownCancelToken.Cancel();
			}
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken == null)
			{
				return;
			}
			unhoverAnimCancelToken.Cancel();
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x001D08BF File Offset: 0x001CEABF
		private void OnWindowChange()
		{
			this._showPos = base.Position;
			this._hidePos = base.Position + new Vector2(0f, 64f);
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// </summary>
		// Token: 0x06003CB6 RID: 15542 RVA: 0x001D08F0 File Offset: 0x001CEAF0
		protected override void OnEnable()
		{
			this._buttonImage.Modulate = Colors.White;
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "position", this._showPos, 0.3499999940395355).SetEase(1L).SetTrans(10L)
				.From(this._hidePos);
		}

		/// <summary>
		/// Call when we want this button to hide this button (Disables clickability/hotkeys)
		/// </summary>
		// Token: 0x06003CB7 RID: 15543 RVA: 0x001D0974 File Offset: 0x001CEB74
		protected override void OnDisable()
		{
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "position", this._hidePos, 0.3499999940395355).SetEase(1L).SetTrans(5L)
				.From(this._showPos);
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x001D09E7 File Offset: 0x001CEBE7
		protected override void OnFocus()
		{
			base.OnFocus();
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken != null)
			{
				unhoverAnimCancelToken.Cancel();
			}
			base.Scale = NMiscConfirmButton._hoverScale;
			this._buttonImage.Modulate = Colors.White;
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x001D0A1B File Offset: 0x001CEC1B
		protected override void OnUnfocus()
		{
			CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
			if (pressDownCancelToken != null)
			{
				pressDownCancelToken.Cancel();
			}
			this._unhoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x001D0A4C File Offset: 0x001CEC4C
		[NullableContext(1)]
		private async Task AnimUnhover(CancellationTokenSource cancelToken)
		{
			float num = 0f;
			Vector2 startScale = base.Scale;
			Color startButtonColor = this._buttonImage.Modulate;
			while (num < 0.5f)
			{
				if (cancelToken.IsCancellationRequested)
				{
					return;
				}
				base.Scale = startScale.Lerp(Vector2.One, Ease.ExpoOut(num / 0.5f));
				this._buttonImage.Modulate = startButtonColor.Lerp(Colors.White, Ease.ExpoOut(num / 0.5f));
				float num2 = num;
				float num3 = await this.AwaitProcessFrame(default(CancellationToken));
				num = num2 + num3;
			}
			base.Scale = Vector2.One;
			this._buttonImage.Modulate = Colors.White;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x001D0A97 File Offset: 0x001CEC97
		protected override void OnPress()
		{
			base.OnPress();
			this._pressDownCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimPressDown(this._pressDownCancelToken));
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x001D0ABC File Offset: 0x001CECBC
		[NullableContext(1)]
		private async Task AnimPressDown(CancellationTokenSource cancelToken)
		{
			float num = 0f;
			this._buttonImage.Modulate = Colors.White;
			base.Scale = NMiscConfirmButton._hoverScale;
			while (num < 0.25f)
			{
				if (cancelToken.IsCancellationRequested)
				{
					return;
				}
				base.Scale = NMiscConfirmButton._hoverScale.Lerp(NMiscConfirmButton._downScale, Ease.CubicOut(num / 0.25f));
				this._buttonImage.Modulate = Colors.White.Lerp(this._downColor, Ease.CubicOut(num / 0.25f));
				float num2 = num;
				float num3 = await this.AwaitProcessFrame(default(CancellationToken));
				num = num2 + num3;
			}
			base.Scale = NMiscConfirmButton._downScale;
			this._buttonImage.Modulate = this._downColor;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CBD RID: 15549 RVA: 0x001D0B08 File Offset: 0x001CED08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NMiscConfirmButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMiscConfirmButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CBE RID: 15550 RVA: 0x001D0C98 File Offset: 0x001CEE98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMiscConfirmButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMiscConfirmButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CBF RID: 15551 RVA: 0x001D0DD8 File Offset: 0x001CEFD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMiscConfirmButton.MethodName._Ready || (ref method) == NMiscConfirmButton.MethodName._ExitTree || (ref method) == NMiscConfirmButton.MethodName.OnWindowChange || (ref method) == NMiscConfirmButton.MethodName.OnEnable || (ref method) == NMiscConfirmButton.MethodName.OnDisable || (ref method) == NMiscConfirmButton.MethodName.OnFocus || (ref method) == NMiscConfirmButton.MethodName.OnUnfocus || (ref method) == NMiscConfirmButton.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003CC0 RID: 15552 RVA: 0x001D0E64 File Offset: 0x001CF064
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMiscConfirmButton.PropertyName._buttonImage)
			{
				this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._downColor)
			{
				this._downColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._showPos)
			{
				this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._hidePos)
			{
				this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._moveTween)
			{
				this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003CC1 RID: 15553 RVA: 0x001D0F00 File Offset: 0x001CF100
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMiscConfirmButton.PropertyName._buttonImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._downColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._downColor);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._showPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._hidePos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
				return true;
			}
			if ((ref name) == NMiscConfirmButton.PropertyName._moveTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CC2 RID: 15554 RVA: 0x001D0FB8 File Offset: 0x001CF1B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMiscConfirmButton.PropertyName._buttonImage, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMiscConfirmButton.PropertyName._downColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMiscConfirmButton.PropertyName._showPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMiscConfirmButton.PropertyName._hidePos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMiscConfirmButton.PropertyName._moveTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CC3 RID: 15555 RVA: 0x001D1070 File Offset: 0x001CF270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMiscConfirmButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
			info.AddProperty(NMiscConfirmButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
			info.AddProperty(NMiscConfirmButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
			info.AddProperty(NMiscConfirmButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
			info.AddProperty(NMiscConfirmButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
		}

		/// <inheritdoc />
		// Token: 0x06003CC4 RID: 15556 RVA: 0x001D10F4 File Offset: 0x001CF2F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMiscConfirmButton.PropertyName._buttonImage, ref variant))
			{
				this._buttonImage = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMiscConfirmButton.PropertyName._downColor, ref variant2))
			{
				this._downColor = variant2.As<Color>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMiscConfirmButton.PropertyName._showPos, ref variant3))
			{
				this._showPos = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMiscConfirmButton.PropertyName._hidePos, ref variant4))
			{
				this._hidePos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMiscConfirmButton.PropertyName._moveTween, ref variant5))
			{
				this._moveTween = variant5.As<Tween>();
			}
		}

		// Token: 0x040017A8 RID: 6056
		[Nullable(1)]
		private Control _buttonImage;

		// Token: 0x040017A9 RID: 6057
		private Color _downColor = Colors.Gray;

		// Token: 0x040017AA RID: 6058
		private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);

		// Token: 0x040017AB RID: 6059
		private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);

		// Token: 0x040017AC RID: 6060
		private const float _pressDownDur = 0.25f;

		// Token: 0x040017AD RID: 6061
		private const float _unhoverAnimDur = 0.5f;

		// Token: 0x040017AE RID: 6062
		private const float _animInOutDur = 0.35f;

		// Token: 0x040017AF RID: 6063
		private Vector2 _showPos;

		// Token: 0x040017B0 RID: 6064
		private Vector2 _hidePos;

		// Token: 0x040017B1 RID: 6065
		[Nullable(2)]
		private Tween _moveTween;

		// Token: 0x040017B2 RID: 6066
		[Nullable(2)]
		private CancellationTokenSource _pressDownCancelToken;

		// Token: 0x040017B3 RID: 6067
		[Nullable(2)]
		private CancellationTokenSource _unhoverAnimCancelToken;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001865 RID: 6245
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F66 RID: 24422
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005F67 RID: 24423
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005F68 RID: 24424
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005F69 RID: 24425
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005F6A RID: 24426
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005F6B RID: 24427
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005F6C RID: 24428
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005F6D RID: 24429
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001866 RID: 6246
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_buttonImage' field.
			/// </summary>
			// Token: 0x04005F6E RID: 24430
			public static readonly StringName _buttonImage = "_buttonImage";

			/// <summary>
			/// Cached name for the '_downColor' field.
			/// </summary>
			// Token: 0x04005F6F RID: 24431
			public static readonly StringName _downColor = "_downColor";

			/// <summary>
			/// Cached name for the '_showPos' field.
			/// </summary>
			// Token: 0x04005F70 RID: 24432
			public static readonly StringName _showPos = "_showPos";

			/// <summary>
			/// Cached name for the '_hidePos' field.
			/// </summary>
			// Token: 0x04005F71 RID: 24433
			public static readonly StringName _hidePos = "_hidePos";

			/// <summary>
			/// Cached name for the '_moveTween' field.
			/// </summary>
			// Token: 0x04005F72 RID: 24434
			public static readonly StringName _moveTween = "_moveTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001867 RID: 6247
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
