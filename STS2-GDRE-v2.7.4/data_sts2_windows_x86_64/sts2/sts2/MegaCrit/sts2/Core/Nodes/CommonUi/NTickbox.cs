using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Base class for tickboxes. Override me!
	/// Note that the hitbox for this Tickbox is its PARENT node, not this node.
	/// </summary>
	// Token: 0x020003EE RID: 1006
	[ScriptPath("res://src/Core/Nodes/CommonUi/NTickbox.cs")]
	public class NTickbox : NButton
	{
		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x001D6399 File Offset: 0x001D4599
		// (set) Token: 0x06003D6A RID: 15722 RVA: 0x001D63A1 File Offset: 0x001D45A1
		public bool IsTicked
		{
			get
			{
				return this._isTicked;
			}
			set
			{
				this._isTicked = value;
				this._tickedImage.Visible = this._isTicked;
				this._notTickedImage.Visible = !this._isTicked;
			}
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x001D63D0 File Offset: 0x001D45D0
		public override void _Ready()
		{
			if (base.GetType() != typeof(NTickbox))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x001D6428 File Offset: 0x001D4628
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._imageContainer = base.GetNode<Control>("%TickboxVisuals");
			this._baseScale = this._imageContainer.Scale;
			this._hsv = (ShaderMaterial)this._imageContainer.Material;
			this._tickedImage = base.GetNode<Control>("%TickboxVisuals/Ticked");
			this._notTickedImage = base.GetNode<Control>("%TickboxVisuals/NotTicked");
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x001D64A4 File Offset: 0x001D46A4
		protected override void OnRelease()
		{
			base.OnRelease();
			this.ForceToggleTick();
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x001D64B4 File Offset: 0x001D46B4
		public void ForceToggleTick()
		{
			this.IsTicked = !this.IsTicked;
			if (this.IsTicked)
			{
				SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_on", 1f);
				this.OnTick();
			}
			else
			{
				SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_off", 1f);
				this.OnUntick();
			}
			base.EmitSignal(NTickbox.SignalName.Toggled, new Variant[] { this });
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._imageContainer, "scale", this._baseScale * this._hoverScale, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), this._hoverV, 0.05);
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x001D65C2 File Offset: 0x001D47C2
		protected virtual void OnUntick()
		{
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x001D65C4 File Offset: 0x001D47C4
		protected virtual void OnTick()
		{
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x001D65C8 File Offset: 0x001D47C8
		protected override void OnFocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._imageContainer, "scale", this._baseScale * this._hoverScale, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), this._hoverV, 0.05);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x001D6674 File Offset: 0x001D4874
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._imageContainer, "scale", this._baseScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x001D6730 File Offset: 0x001D4930
		protected override void OnPress()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._imageContainer, "scale", this._baseScale * this._pressDownScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x001D67F7 File Offset: 0x001D49F7
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Modulate = StsColors.gray;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x001D680A File Offset: 0x001D4A0A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Modulate = Colors.White;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x001D681D File Offset: 0x001D4A1D
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NTickbox._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D77 RID: 15735 RVA: 0x001D6838 File Offset: 0x001D4A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.ForceToggleTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTickbox.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D78 RID: 15736 RVA: 0x001D6AA8 File Offset: 0x001D4CA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.ForceToggleTick && args.Count == 0)
			{
				this.ForceToggleTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTickbox.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003D79 RID: 15737 RVA: 0x001D6C88 File Offset: 0x001D4E88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTickbox.MethodName._Ready || (ref method) == NTickbox.MethodName.ConnectSignals || (ref method) == NTickbox.MethodName.OnRelease || (ref method) == NTickbox.MethodName.ForceToggleTick || (ref method) == NTickbox.MethodName.OnUntick || (ref method) == NTickbox.MethodName.OnTick || (ref method) == NTickbox.MethodName.OnFocus || (ref method) == NTickbox.MethodName.OnUnfocus || (ref method) == NTickbox.MethodName.OnPress || (ref method) == NTickbox.MethodName.OnDisable || (ref method) == NTickbox.MethodName.OnEnable || (ref method) == NTickbox.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003D7A RID: 15738 RVA: 0x001D6D50 File Offset: 0x001D4F50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTickbox.PropertyName.IsTicked)
			{
				this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._isTicked)
			{
				this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._imageContainer)
			{
				this._imageContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._tickedImage)
			{
				this._tickedImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._notTickedImage)
			{
				this._notTickedImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hoverScale)
			{
				this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._pressDownScale)
			{
				this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hoverV)
			{
				this._hoverV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003D7B RID: 15739 RVA: 0x001D6E90 File Offset: 0x001D5090
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTickbox.PropertyName.IsTicked)
			{
				bool isTicked = this.IsTicked;
				value = VariantUtils.CreateFrom<bool>(ref isTicked);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._isTicked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._imageContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._imageContainer);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._tickedImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._tickedImage);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._notTickedImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._notTickedImage);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hoverScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._pressDownScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pressDownScale);
				return true;
			}
			if ((ref name) == NTickbox.PropertyName._hoverV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hoverV);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D7C RID: 15740 RVA: 0x001D7008 File Offset: 0x001D5208
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NTickbox.PropertyName._isTicked, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTickbox.PropertyName.IsTicked, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTickbox.PropertyName._imageContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTickbox.PropertyName._tickedImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTickbox.PropertyName._notTickedImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTickbox.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTickbox.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NTickbox.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTickbox.PropertyName._hoverScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTickbox.PropertyName._pressDownScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTickbox.PropertyName._hoverV, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D7D RID: 15741 RVA: 0x001D7184 File Offset: 0x001D5384
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isTicked = NTickbox.PropertyName.IsTicked;
			bool isTicked2 = this.IsTicked;
			info.AddProperty(isTicked, Variant.From<bool>(ref isTicked2));
			info.AddProperty(NTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
			info.AddProperty(NTickbox.PropertyName._imageContainer, Variant.From<Control>(ref this._imageContainer));
			info.AddProperty(NTickbox.PropertyName._tickedImage, Variant.From<Control>(ref this._tickedImage));
			info.AddProperty(NTickbox.PropertyName._notTickedImage, Variant.From<Control>(ref this._notTickedImage));
			info.AddProperty(NTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
			info.AddProperty(NTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
			info.AddProperty(NTickbox.PropertyName._hoverV, Variant.From<float>(ref this._hoverV));
			info.AddSignalEventDelegate(NTickbox.SignalName.Toggled, this.backing_Toggled);
		}

		/// <inheritdoc />
		// Token: 0x06003D7E RID: 15742 RVA: 0x001D72A0 File Offset: 0x001D54A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTickbox.PropertyName.IsTicked, ref variant))
			{
				this.IsTicked = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTickbox.PropertyName._isTicked, ref variant2))
			{
				this._isTicked = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTickbox.PropertyName._imageContainer, ref variant3))
			{
				this._imageContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTickbox.PropertyName._tickedImage, ref variant4))
			{
				this._tickedImage = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTickbox.PropertyName._notTickedImage, ref variant5))
			{
				this._notTickedImage = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTickbox.PropertyName._hsv, ref variant6))
			{
				this._hsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTickbox.PropertyName._tween, ref variant7))
			{
				this._tween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTickbox.PropertyName._baseScale, ref variant8))
			{
				this._baseScale = variant8.As<Vector2>();
			}
			Variant variant9;
			if (info.TryGetProperty(NTickbox.PropertyName._hoverScale, ref variant9))
			{
				this._hoverScale = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NTickbox.PropertyName._pressDownScale, ref variant10))
			{
				this._pressDownScale = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NTickbox.PropertyName._hoverV, ref variant11))
			{
				this._hoverV = variant11.As<float>();
			}
			NTickbox.ToggledEventHandler toggledEventHandler;
			if (info.TryGetSignalEventDelegate<NTickbox.ToggledEventHandler>(NTickbox.SignalName.Toggled, ref toggledEventHandler))
			{
				this.backing_Toggled = toggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D7F RID: 15743 RVA: 0x001D7400 File Offset: 0x001D5600
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NTickbox.SignalName.Toggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NTickbox.ToggledEventHandler" />
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06003D80 RID: 15744 RVA: 0x001D7474 File Offset: 0x001D5674
		// (remove) Token: 0x06003D81 RID: 15745 RVA: 0x001D748D File Offset: 0x001D568D
		public event NTickbox.ToggledEventHandler Toggled
		{
			add
			{
				this.backing_Toggled = (NTickbox.ToggledEventHandler)Delegate.Combine(this.backing_Toggled, value);
			}
			remove
			{
				this.backing_Toggled = (NTickbox.ToggledEventHandler)Delegate.Remove(this.backing_Toggled, value);
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x001D74A6 File Offset: 0x001D56A6
		protected void EmitSignalToggled(NTickbox tickbox)
		{
			base.EmitSignal(NTickbox.SignalName.Toggled, new Variant[] { tickbox });
		}

		/// <inheritdoc />
		// Token: 0x06003D83 RID: 15747 RVA: 0x001D74C7 File Offset: 0x001D56C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NTickbox.SignalName.Toggled) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NTickbox.ToggledEventHandler toggledEventHandler = this.backing_Toggled;
			if (toggledEventHandler == null)
			{
				return;
			}
			toggledEventHandler(VariantUtils.ConvertTo<NTickbox>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003D84 RID: 15748 RVA: 0x001D7506 File Offset: 0x001D5706
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NTickbox.SignalName.Toggled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x040017EB RID: 6123
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040017EC RID: 6124
		private bool _isTicked = true;

		// Token: 0x040017ED RID: 6125
		[Nullable(1)]
		private Control _imageContainer;

		// Token: 0x040017EE RID: 6126
		[Nullable(1)]
		private Control _tickedImage;

		// Token: 0x040017EF RID: 6127
		[Nullable(1)]
		private Control _notTickedImage;

		// Token: 0x040017F0 RID: 6128
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x040017F1 RID: 6129
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040017F2 RID: 6130
		private Vector2 _baseScale;

		// Token: 0x040017F3 RID: 6131
		private float _hoverScale = 1.05f;

		// Token: 0x040017F4 RID: 6132
		private float _pressDownScale = 0.95f;

		// Token: 0x040017F5 RID: 6133
		private float _hoverV = 1.2f;

		// Token: 0x040017F6 RID: 6134
		private NTickbox.ToggledEventHandler backing_Toggled;

		// Token: 0x0200188D RID: 6285
		// (Invoke) Token: 0x0600A8F1 RID: 43249
		[Signal]
		public delegate void ToggledEventHandler(NTickbox tickbox);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200188E RID: 6286
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FE9 RID: 24553
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04005FEA RID: 24554
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005FEB RID: 24555
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'ForceToggleTick' method.
			/// </summary>
			// Token: 0x04005FEC RID: 24556
			public static readonly StringName ForceToggleTick = "ForceToggleTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04005FED RID: 24557
			public static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04005FEE RID: 24558
			public static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005FEF RID: 24559
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005FF0 RID: 24560
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005FF1 RID: 24561
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005FF2 RID: 24562
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005FF3 RID: 24563
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005FF4 RID: 24564
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200188F RID: 6287
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsTicked' property.
			/// </summary>
			// Token: 0x04005FF5 RID: 24565
			public static readonly StringName IsTicked = "IsTicked";

			/// <summary>
			/// Cached name for the '_isTicked' field.
			/// </summary>
			// Token: 0x04005FF6 RID: 24566
			public static readonly StringName _isTicked = "_isTicked";

			/// <summary>
			/// Cached name for the '_imageContainer' field.
			/// </summary>
			// Token: 0x04005FF7 RID: 24567
			public static readonly StringName _imageContainer = "_imageContainer";

			/// <summary>
			/// Cached name for the '_tickedImage' field.
			/// </summary>
			// Token: 0x04005FF8 RID: 24568
			public static readonly StringName _tickedImage = "_tickedImage";

			/// <summary>
			/// Cached name for the '_notTickedImage' field.
			/// </summary>
			// Token: 0x04005FF9 RID: 24569
			public static readonly StringName _notTickedImage = "_notTickedImage";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005FFA RID: 24570
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005FFB RID: 24571
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x04005FFC RID: 24572
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_hoverScale' field.
			/// </summary>
			// Token: 0x04005FFD RID: 24573
			public static readonly StringName _hoverScale = "_hoverScale";

			/// <summary>
			/// Cached name for the '_pressDownScale' field.
			/// </summary>
			// Token: 0x04005FFE RID: 24574
			public static readonly StringName _pressDownScale = "_pressDownScale";

			/// <summary>
			/// Cached name for the '_hoverV' field.
			/// </summary>
			// Token: 0x04005FFF RID: 24575
			public static readonly StringName _hoverV = "_hoverV";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001890 RID: 6288
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Toggled' signal.
			/// </summary>
			// Token: 0x04006000 RID: 24576
			public static readonly StringName Toggled = "Toggled";
		}
	}
}
