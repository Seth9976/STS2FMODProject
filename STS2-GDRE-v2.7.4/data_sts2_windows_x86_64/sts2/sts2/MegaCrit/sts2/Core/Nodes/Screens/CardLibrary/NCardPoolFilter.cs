using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	/// <summary>
	/// The current pool the player is viewing in the Card Library.
	/// Unlike the other tickboxes, this one is a radio-button style.
	/// Clicking on a pool filter will deselect the others. If this filter
	/// is already active, then you can't click on it.
	/// </summary>
	// Token: 0x02000355 RID: 853
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardPoolFilter.cs")]
	public class NCardPoolFilter : NButton
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x0016DE88 File Offset: 0x0016C088
		// (set) Token: 0x06003141 RID: 12609 RVA: 0x0016DE90 File Offset: 0x0016C090
		[Nullable(2)]
		public LocString Loc
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x0016DE99 File Offset: 0x0016C099
		// (set) Token: 0x06003143 RID: 12611 RVA: 0x0016DEA1 File Offset: 0x0016C0A1
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				this.OnToggle();
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x0016DEB0 File Offset: 0x0016C0B0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<Control>("Image");
			this._controllerSelectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._hsv = (ShaderMaterial)this._image.GetMaterial();
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x0016DF08 File Offset: 0x0016C108
		private void OnToggle()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._hsv.SetShaderParameter(NCardPoolFilter._s, this._isSelected ? 1f : 0.3f);
			this._hsv.SetShaderParameter(NCardPoolFilter._v, this._isSelected ? 1f : 0.55f);
			if (!this._isSelected)
			{
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this._image, "scale", NCardPoolFilter._disabledScale, 0.3).SetEase(1L).SetTrans(5L);
				return;
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", NCardPoolFilter._enabledScale, 0.2).SetEase(1L).SetTrans(10L);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0016E020 File Offset: 0x0016C220
		protected override void OnRelease()
		{
			base.OnRelease();
			this.IsSelected = !this.IsSelected;
			base.EmitSignal(NCardPoolFilter.SignalName.Toggled, new Variant[] { this });
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x0016E058 File Offset: 0x0016C258
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", (this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale) * 1.2f, 0.05);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._controllerSelectionReticle.OnSelect();
			}
			if (this.Loc != null)
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(this.Loc, null), HoverTipAlignment.None);
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetGlobalPosition(new Vector2(310f, base.GlobalPosition.Y), false);
				}
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x0016E130 File Offset: 0x0016C330
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale, 0.3);
			this._controllerSelectionReticle.OnDeselect();
			if (this.Loc != null)
			{
				NHoverTipSet.Remove(this);
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0016E1C0 File Offset: 0x0016C3C0
		protected override void OnPress()
		{
			if (this._isSelected)
			{
				return;
			}
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", (this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale) * 0.8f, 0.3).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600314A RID: 12618 RVA: 0x0016E258 File Offset: 0x0016C458
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NCardPoolFilter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPoolFilter.MethodName.OnToggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPoolFilter.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPoolFilter.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPoolFilter.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPoolFilter.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600314B RID: 12619 RVA: 0x0016E388 File Offset: 0x0016C588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardPoolFilter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPoolFilter.MethodName.OnToggle && args.Count == 0)
			{
				this.OnToggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPoolFilter.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPoolFilter.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPoolFilter.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPoolFilter.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600314C RID: 12620 RVA: 0x0016E47C File Offset: 0x0016C67C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardPoolFilter.MethodName._Ready || (ref method) == NCardPoolFilter.MethodName.OnToggle || (ref method) == NCardPoolFilter.MethodName.OnRelease || (ref method) == NCardPoolFilter.MethodName.OnFocus || (ref method) == NCardPoolFilter.MethodName.OnUnfocus || (ref method) == NCardPoolFilter.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600314D RID: 12621 RVA: 0x0016E4EC File Offset: 0x0016C6EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardPoolFilter.PropertyName.IsSelected)
			{
				this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._controllerSelectionReticle)
			{
				this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600314E RID: 12622 RVA: 0x0016E5A4 File Offset: 0x0016C7A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardPoolFilter.PropertyName.IsSelected)
			{
				bool isSelected = this.IsSelected;
				value = VariantUtils.CreateFrom<bool>(ref isSelected);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._controllerSelectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
				return true;
			}
			if ((ref name) == NCardPoolFilter.PropertyName._tween)
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
		// Token: 0x0600314F RID: 12623 RVA: 0x0016E67C File Offset: 0x0016C87C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCardPoolFilter.PropertyName._isSelected, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPoolFilter.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPoolFilter.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPoolFilter.PropertyName._controllerSelectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPoolFilter.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardPoolFilter.PropertyName.IsSelected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003150 RID: 12624 RVA: 0x0016E754 File Offset: 0x0016C954
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isSelected = NCardPoolFilter.PropertyName.IsSelected;
			bool isSelected2 = this.IsSelected;
			info.AddProperty(isSelected, Variant.From<bool>(ref isSelected2));
			info.AddProperty(NCardPoolFilter.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
			info.AddProperty(NCardPoolFilter.PropertyName._image, Variant.From<Control>(ref this._image));
			info.AddProperty(NCardPoolFilter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCardPoolFilter.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
			info.AddProperty(NCardPoolFilter.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddSignalEventDelegate(NCardPoolFilter.SignalName.Toggled, this.backing_Toggled);
		}

		/// <inheritdoc />
		// Token: 0x06003151 RID: 12625 RVA: 0x0016E800 File Offset: 0x0016CA00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName.IsSelected, ref variant))
			{
				this.IsSelected = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName._isSelected, ref variant2))
			{
				this._isSelected = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName._image, ref variant3))
			{
				this._image = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName._controllerSelectionReticle, ref variant5))
			{
				this._controllerSelectionReticle = variant5.As<NSelectionReticle>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardPoolFilter.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
			NCardPoolFilter.ToggledEventHandler toggledEventHandler;
			if (info.TryGetSignalEventDelegate<NCardPoolFilter.ToggledEventHandler>(NCardPoolFilter.SignalName.Toggled, ref toggledEventHandler))
			{
				this.backing_Toggled = toggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003152 RID: 12626 RVA: 0x0016E8D4 File Offset: 0x0016CAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardPoolFilter.SignalName.Toggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "filter", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardPoolFilter.ToggledEventHandler" />
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06003153 RID: 12627 RVA: 0x0016E948 File Offset: 0x0016CB48
		// (remove) Token: 0x06003154 RID: 12628 RVA: 0x0016E961 File Offset: 0x0016CB61
		public event NCardPoolFilter.ToggledEventHandler Toggled
		{
			add
			{
				this.backing_Toggled = (NCardPoolFilter.ToggledEventHandler)Delegate.Combine(this.backing_Toggled, value);
			}
			remove
			{
				this.backing_Toggled = (NCardPoolFilter.ToggledEventHandler)Delegate.Remove(this.backing_Toggled, value);
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0016E97A File Offset: 0x0016CB7A
		protected void EmitSignalToggled(NCardPoolFilter filter)
		{
			base.EmitSignal(NCardPoolFilter.SignalName.Toggled, new Variant[] { filter });
		}

		/// <inheritdoc />
		// Token: 0x06003156 RID: 12630 RVA: 0x0016E99B File Offset: 0x0016CB9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NCardPoolFilter.SignalName.Toggled) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NCardPoolFilter.ToggledEventHandler toggledEventHandler = this.backing_Toggled;
			if (toggledEventHandler == null)
			{
				return;
			}
			toggledEventHandler(VariantUtils.ConvertTo<NCardPoolFilter>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003157 RID: 12631 RVA: 0x0016E9DA File Offset: 0x0016CBDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardPoolFilter.SignalName.Toggled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x04001333 RID: 4915
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001334 RID: 4916
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001336 RID: 4918
		private bool _isSelected;

		// Token: 0x04001337 RID: 4919
		[Nullable(1)]
		private Control _image;

		// Token: 0x04001338 RID: 4920
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001339 RID: 4921
		[Nullable(1)]
		private NSelectionReticle _controllerSelectionReticle;

		// Token: 0x0400133A RID: 4922
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400133B RID: 4923
		private const float _focusedMultiplier = 1.2f;

		// Token: 0x0400133C RID: 4924
		private const float _pressDownMultiplier = 0.8f;

		// Token: 0x0400133D RID: 4925
		private static readonly Vector2 _enabledScale = Vector2.One * 1.1f;

		// Token: 0x0400133E RID: 4926
		private static readonly Vector2 _disabledScale = Vector2.One * 0.95f;

		// Token: 0x0400133F RID: 4927
		private NCardPoolFilter.ToggledEventHandler backing_Toggled;

		// Token: 0x0200163B RID: 5691
		// (Invoke) Token: 0x0600A456 RID: 42070
		[Signal]
		public delegate void ToggledEventHandler(NCardPoolFilter filter);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200163C RID: 5692
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400569C RID: 22172
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnToggle' method.
			/// </summary>
			// Token: 0x0400569D RID: 22173
			public static readonly StringName OnToggle = "OnToggle";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400569E RID: 22174
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400569F RID: 22175
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040056A0 RID: 22176
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040056A1 RID: 22177
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200163D RID: 5693
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsSelected' property.
			/// </summary>
			// Token: 0x040056A2 RID: 22178
			public static readonly StringName IsSelected = "IsSelected";

			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x040056A3 RID: 22179
			public static readonly StringName _isSelected = "_isSelected";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x040056A4 RID: 22180
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040056A5 RID: 22181
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_controllerSelectionReticle' field.
			/// </summary>
			// Token: 0x040056A6 RID: 22182
			public static readonly StringName _controllerSelectionReticle = "_controllerSelectionReticle";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040056A7 RID: 22183
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200163E RID: 5694
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Toggled' signal.
			/// </summary>
			// Token: 0x040056A8 RID: 22184
			public static readonly StringName Toggled = "Toggled";
		}
	}
}
