using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Confirm Button used in various places to confirm choices, embark on a run, etc.
	/// Very useful but not as cool as Back Button but that's life.
	/// Use the ButtonReleased Action to handle events.
	/// </summary>
	// Token: 0x020003D6 RID: 982
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NConfirmButton.cs")]
	public class NConfirmButton : NButton
	{
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x001C664A File Offset: 0x001C484A
		protected override string[] Hotkeys
		{
			get
			{
				return this._hotkeys;
			}
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x001C6654 File Offset: 0x001C4854
		public override void _Ready()
		{
			this.ConnectSignals();
			this._isEnabled = false;
			this._outline = base.GetNode<Control>("Outline");
			this._buttonImage = base.GetNode<Control>("Image");
			this._viewport = base.GetViewport();
			this._posOffset = new Vector2(base.OffsetRight + 120f, -base.OffsetBottom + 110f);
			base.GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
			this.OnDisable();
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x001C66FE File Offset: 0x001C48FE
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

		// Token: 0x06003B71 RID: 15217 RVA: 0x001C6728 File Offset: 0x001C4928
		private void OnWindowChange()
		{
			this._showPos = NGame.Instance.Size - this._posOffset;
			this._hidePos = this._showPos + NConfirmButton._hideOffset;
			base.Position = (this._isEnabled ? this._showPos : this._hidePos);
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// </summary>
		// Token: 0x06003B72 RID: 15218 RVA: 0x001C6784 File Offset: 0x001C4984
		protected override void OnEnable()
		{
			base.OnEnable();
			this._isEnabled = true;
			this._outline.Modulate = Colors.Transparent;
			this._buttonImage.Modulate = Colors.White;
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "position", this._showPos, 0.3499999940395355).SetEase(1L).SetTrans(10L)
				.FromCurrent();
		}

		/// <summary>
		/// Call when we want this button to hide this button (Disables clickability/hotkeys)
		/// </summary>
		// Token: 0x06003B73 RID: 15219 RVA: 0x001C681C File Offset: 0x001C4A1C
		protected override void OnDisable()
		{
			base.OnDisable();
			this._isEnabled = false;
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "position", this._hidePos, 0.3499999940395355).SetEase(1L).SetTrans(5L)
				.FromCurrent();
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x001C6894 File Offset: 0x001C4A94
		protected override void OnFocus()
		{
			base.OnFocus();
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken != null)
			{
				unhoverAnimCancelToken.Cancel();
			}
			base.Scale = NConfirmButton._hoverScale;
			this._outline.Modulate = this._outlineColor;
			this._buttonImage.Modulate = Colors.White;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x001C68E4 File Offset: 0x001C4AE4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
			if (pressDownCancelToken != null)
			{
				pressDownCancelToken.Cancel();
			}
			this._unhoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x001C691C File Offset: 0x001C4B1C
		private async Task AnimUnhover(CancellationTokenSource cancelToken)
		{
			float num = 0f;
			Vector2 startScale = base.Scale;
			Color startButtonColor = this._buttonImage.Modulate;
			Color startColor = this._outline.Modulate;
			while (num < 0.5f)
			{
				if (cancelToken.IsCancellationRequested)
				{
					return;
				}
				base.Scale = startScale.Lerp(Vector2.One, Ease.ExpoOut(num / 0.5f));
				this._outline.Modulate = startColor.Lerp(this._outlineTransparentColor, Ease.ExpoOut(num / 0.5f));
				this._buttonImage.Modulate = startButtonColor.Lerp(Colors.White, Ease.ExpoOut(num / 0.5f));
				float num2 = num;
				float num3 = await this.AwaitProcessFrame(default(CancellationToken));
				num = num2 + num3;
			}
			base.Scale = Vector2.One;
			this._outline.Modulate = this._outlineTransparentColor;
			this._buttonImage.Modulate = Colors.White;
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x001C6967 File Offset: 0x001C4B67
		protected override void OnPress()
		{
			base.OnPress();
			this._pressDownCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimPressDown(this._pressDownCancelToken));
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x001C698C File Offset: 0x001C4B8C
		private async Task AnimPressDown(CancellationTokenSource cancelToken)
		{
			float num = 0f;
			this._buttonImage.Modulate = Colors.White;
			this._outline.Modulate = this._outlineColor;
			base.Scale = NConfirmButton._hoverScale;
			while (num < 0.25f)
			{
				if (cancelToken.IsCancellationRequested)
				{
					return;
				}
				base.Scale = NConfirmButton._hoverScale.Lerp(NConfirmButton._downScale, Ease.CubicOut(num / 0.25f));
				this._buttonImage.Modulate = Colors.White.Lerp(this._downColor, Ease.CubicOut(num / 0.25f));
				this._outline.Modulate = this._outlineColor.Lerp(this._outlineTransparentColor, Ease.CubicOut(num / 0.25f));
				float num2 = num;
				float num3 = await this.AwaitProcessFrame(default(CancellationToken));
				num = num2 + num3;
			}
			base.Scale = NConfirmButton._downScale;
			this._buttonImage.Modulate = this._downColor;
			this._outline.Modulate = this._outlineTransparentColor;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x001C69D7 File Offset: 0x001C4BD7
		public void OverrideHotkeys(string[] hotkeys)
		{
			this._hotkeys = hotkeys;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B7A RID: 15226 RVA: 0x001C69E0 File Offset: 0x001C4BE0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NConfirmButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NConfirmButton.MethodName.OverrideHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(34L, "hotkeys", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B7B RID: 15227 RVA: 0x001C6BC4 File Offset: 0x001C4DC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NConfirmButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NConfirmButton.MethodName.OverrideHotkeys && args.Count == 1)
			{
				this.OverrideHotkeys(VariantUtils.ConvertTo<string[]>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B7C RID: 15228 RVA: 0x001C6D38 File Offset: 0x001C4F38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NConfirmButton.MethodName._Ready || (ref method) == NConfirmButton.MethodName._ExitTree || (ref method) == NConfirmButton.MethodName.OnWindowChange || (ref method) == NConfirmButton.MethodName.OnEnable || (ref method) == NConfirmButton.MethodName.OnDisable || (ref method) == NConfirmButton.MethodName.OnFocus || (ref method) == NConfirmButton.MethodName.OnUnfocus || (ref method) == NConfirmButton.MethodName.OnPress || (ref method) == NConfirmButton.MethodName.OverrideHotkeys || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003B7D RID: 15229 RVA: 0x001C6DD4 File Offset: 0x001C4FD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NConfirmButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._buttonImage)
			{
				this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._defaultOutlineColor)
			{
				this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hoveredOutlineColor)
			{
				this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._downColor)
			{
				this._downColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._outlineColor)
			{
				this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._outlineTransparentColor)
			{
				this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hotkeys)
			{
				this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._posOffset)
			{
				this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._showPos)
			{
				this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hidePos)
			{
				this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._moveTween)
			{
				this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003B7E RID: 15230 RVA: 0x001C6F48 File Offset: 0x001C5148
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NConfirmButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._buttonImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._defaultOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hoveredOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._downColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._downColor);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._outlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._outlineTransparentColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._posOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._showPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._hidePos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
				return true;
			}
			if ((ref name) == NConfirmButton.PropertyName._moveTween)
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
		// Token: 0x06003B7F RID: 15231 RVA: 0x001C7120 File Offset: 0x001C5320
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NConfirmButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NConfirmButton.PropertyName._buttonImage, 0L, "", 4096L, false),
				new PropertyInfo(20L, NConfirmButton.PropertyName._defaultOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NConfirmButton.PropertyName._hoveredOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NConfirmButton.PropertyName._downColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NConfirmButton.PropertyName._outlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NConfirmButton.PropertyName._outlineTransparentColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NConfirmButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(34L, NConfirmButton.PropertyName._hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(34L, NConfirmButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(5L, NConfirmButton.PropertyName._posOffset, 0L, "", 4096L, false),
				new PropertyInfo(5L, NConfirmButton.PropertyName._showPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NConfirmButton.PropertyName._hidePos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NConfirmButton.PropertyName._moveTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B80 RID: 15232 RVA: 0x001C7300 File Offset: 0x001C5500
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NConfirmButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NConfirmButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
			info.AddProperty(NConfirmButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
			info.AddProperty(NConfirmButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
			info.AddProperty(NConfirmButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
			info.AddProperty(NConfirmButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
			info.AddProperty(NConfirmButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
			info.AddProperty(NConfirmButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NConfirmButton.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
			info.AddProperty(NConfirmButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
			info.AddProperty(NConfirmButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
			info.AddProperty(NConfirmButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
			info.AddProperty(NConfirmButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
		}

		/// <inheritdoc />
		// Token: 0x06003B81 RID: 15233 RVA: 0x001C7434 File Offset: 0x001C5634
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NConfirmButton.PropertyName._outline, ref variant))
			{
				this._outline = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NConfirmButton.PropertyName._buttonImage, ref variant2))
			{
				this._buttonImage = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NConfirmButton.PropertyName._defaultOutlineColor, ref variant3))
			{
				this._defaultOutlineColor = variant3.As<Color>();
			}
			Variant variant4;
			if (info.TryGetProperty(NConfirmButton.PropertyName._hoveredOutlineColor, ref variant4))
			{
				this._hoveredOutlineColor = variant4.As<Color>();
			}
			Variant variant5;
			if (info.TryGetProperty(NConfirmButton.PropertyName._downColor, ref variant5))
			{
				this._downColor = variant5.As<Color>();
			}
			Variant variant6;
			if (info.TryGetProperty(NConfirmButton.PropertyName._outlineColor, ref variant6))
			{
				this._outlineColor = variant6.As<Color>();
			}
			Variant variant7;
			if (info.TryGetProperty(NConfirmButton.PropertyName._outlineTransparentColor, ref variant7))
			{
				this._outlineTransparentColor = variant7.As<Color>();
			}
			Variant variant8;
			if (info.TryGetProperty(NConfirmButton.PropertyName._viewport, ref variant8))
			{
				this._viewport = variant8.As<Viewport>();
			}
			Variant variant9;
			if (info.TryGetProperty(NConfirmButton.PropertyName._hotkeys, ref variant9))
			{
				this._hotkeys = variant9.As<string[]>();
			}
			Variant variant10;
			if (info.TryGetProperty(NConfirmButton.PropertyName._posOffset, ref variant10))
			{
				this._posOffset = variant10.As<Vector2>();
			}
			Variant variant11;
			if (info.TryGetProperty(NConfirmButton.PropertyName._showPos, ref variant11))
			{
				this._showPos = variant11.As<Vector2>();
			}
			Variant variant12;
			if (info.TryGetProperty(NConfirmButton.PropertyName._hidePos, ref variant12))
			{
				this._hidePos = variant12.As<Vector2>();
			}
			Variant variant13;
			if (info.TryGetProperty(NConfirmButton.PropertyName._moveTween, ref variant13))
			{
				this._moveTween = variant13.As<Tween>();
			}
		}

		// Token: 0x04001732 RID: 5938
		private Control _outline;

		// Token: 0x04001733 RID: 5939
		private Control _buttonImage;

		// Token: 0x04001734 RID: 5940
		private Color _defaultOutlineColor = StsColors.cream;

		// Token: 0x04001735 RID: 5941
		private Color _hoveredOutlineColor = StsColors.gold;

		// Token: 0x04001736 RID: 5942
		private Color _downColor = Colors.Gray;

		// Token: 0x04001737 RID: 5943
		private Color _outlineColor = new Color("F0B400");

		// Token: 0x04001738 RID: 5944
		private Color _outlineTransparentColor = new Color("00FFFF00");

		// Token: 0x04001739 RID: 5945
		private Viewport _viewport;

		// Token: 0x0400173A RID: 5946
		private string[] _hotkeys = new string[] { MegaInput.accept };

		// Token: 0x0400173B RID: 5947
		private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);

		// Token: 0x0400173C RID: 5948
		private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);

		// Token: 0x0400173D RID: 5949
		private const float _pressDownDur = 0.25f;

		// Token: 0x0400173E RID: 5950
		private const float _unhoverAnimDur = 0.5f;

		// Token: 0x0400173F RID: 5951
		private const float _animInOutDur = 0.35f;

		// Token: 0x04001740 RID: 5952
		private Vector2 _posOffset;

		// Token: 0x04001741 RID: 5953
		private Vector2 _showPos;

		// Token: 0x04001742 RID: 5954
		private Vector2 _hidePos;

		// Token: 0x04001743 RID: 5955
		private static readonly Vector2 _hideOffset = new Vector2(180f, 0f);

		// Token: 0x04001744 RID: 5956
		[Nullable(2)]
		private Tween _moveTween;

		// Token: 0x04001745 RID: 5957
		[Nullable(2)]
		private CancellationTokenSource _pressDownCancelToken;

		// Token: 0x04001746 RID: 5958
		[Nullable(2)]
		private CancellationTokenSource _unhoverAnimCancelToken;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001828 RID: 6184
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E79 RID: 24185
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005E7A RID: 24186
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005E7B RID: 24187
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005E7C RID: 24188
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005E7D RID: 24189
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005E7E RID: 24190
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005E7F RID: 24191
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005E80 RID: 24192
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OverrideHotkeys' method.
			/// </summary>
			// Token: 0x04005E81 RID: 24193
			public static readonly StringName OverrideHotkeys = "OverrideHotkeys";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001829 RID: 6185
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005E82 RID: 24194
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005E83 RID: 24195
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_buttonImage' field.
			/// </summary>
			// Token: 0x04005E84 RID: 24196
			public static readonly StringName _buttonImage = "_buttonImage";

			/// <summary>
			/// Cached name for the '_defaultOutlineColor' field.
			/// </summary>
			// Token: 0x04005E85 RID: 24197
			public static readonly StringName _defaultOutlineColor = "_defaultOutlineColor";

			/// <summary>
			/// Cached name for the '_hoveredOutlineColor' field.
			/// </summary>
			// Token: 0x04005E86 RID: 24198
			public static readonly StringName _hoveredOutlineColor = "_hoveredOutlineColor";

			/// <summary>
			/// Cached name for the '_downColor' field.
			/// </summary>
			// Token: 0x04005E87 RID: 24199
			public static readonly StringName _downColor = "_downColor";

			/// <summary>
			/// Cached name for the '_outlineColor' field.
			/// </summary>
			// Token: 0x04005E88 RID: 24200
			public static readonly StringName _outlineColor = "_outlineColor";

			/// <summary>
			/// Cached name for the '_outlineTransparentColor' field.
			/// </summary>
			// Token: 0x04005E89 RID: 24201
			public static readonly StringName _outlineTransparentColor = "_outlineTransparentColor";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04005E8A RID: 24202
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_hotkeys' field.
			/// </summary>
			// Token: 0x04005E8B RID: 24203
			public static readonly StringName _hotkeys = "_hotkeys";

			/// <summary>
			/// Cached name for the '_posOffset' field.
			/// </summary>
			// Token: 0x04005E8C RID: 24204
			public static readonly StringName _posOffset = "_posOffset";

			/// <summary>
			/// Cached name for the '_showPos' field.
			/// </summary>
			// Token: 0x04005E8D RID: 24205
			public static readonly StringName _showPos = "_showPos";

			/// <summary>
			/// Cached name for the '_hidePos' field.
			/// </summary>
			// Token: 0x04005E8E RID: 24206
			public static readonly StringName _hidePos = "_hidePos";

			/// <summary>
			/// Cached name for the '_moveTween' field.
			/// </summary>
			// Token: 0x04005E8F RID: 24207
			public static readonly StringName _moveTween = "_moveTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200182A RID: 6186
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
