using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002F4 RID: 756
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapEraseButton.cs")]
	public class NMapEraseButton : NButton
	{
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0011891E File Offset: 0x00116B1E
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NMapEraseButton._imagePath,
					NMapEraseButton._glowImagePath
				});
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x00118948 File Offset: 0x00116B48
		public override void _Ready()
		{
			this.ConnectSignals();
			this._drawingToolHolder = (Control)base.GetParent();
			this._icon = base.GetNode<TextureRect>("Icon");
			NControllerManager instance = NControllerManager.Instance;
			instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerUpdated)), 0U);
			instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerUpdated)), 0U);
			this.OnControllerUpdated();
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x001189CC File Offset: 0x00116BCC
		public void SetIsErasing(bool isErasing)
		{
			this._isErasing = isErasing;
			this._icon.Texture = PreloadManager.Cache.GetTexture2D(isErasing ? NMapEraseButton._glowImagePath : NMapEraseButton._imagePath);
			this._icon.SelfModulate = (isErasing ? NMapEraseButton._activeColor : NMapEraseButton._inactiveColor);
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x00118A24 File Offset: 0x00116C24
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One * 1.2f, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", NMapEraseButton._activeColor, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._drawingToolHolder, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._drawingToolHolder.GlobalPosition + new Vector2(10f, -132f), false);
			}
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x00118B04 File Offset: 0x00116D04
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One * 1.1f, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", this._isErasing ? NMapEraseButton._activeColor : NMapEraseButton._inactiveColor, 0.05);
			NHoverTipSet.Remove(this._drawingToolHolder);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x00118BBC File Offset: 0x00116DBC
		private void OnControllerUpdated()
		{
			LocString locString = new LocString("map", "ERASING_BUTTON.description");
			LocString locString2;
			if (NControllerManager.Instance.IsUsingController)
			{
				locString2 = new LocString("map", "ERASING_BUTTON.title_controller");
			}
			else
			{
				locString2 = new LocString("map", "ERASING_BUTTON.title_mkb");
			}
			this._hoverTip = new HoverTip(locString2, locString, null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028D1 RID: 10449 RVA: 0x00118C18 File Offset: 0x00116E18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMapEraseButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapEraseButton.MethodName.SetIsErasing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isErasing", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapEraseButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapEraseButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapEraseButton.MethodName.OnControllerUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028D2 RID: 10450 RVA: 0x00118D40 File Offset: 0x00116F40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapEraseButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapEraseButton.MethodName.SetIsErasing && args.Count == 1)
			{
				this.SetIsErasing(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapEraseButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapEraseButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapEraseButton.MethodName.OnControllerUpdated && args.Count == 0)
			{
				this.OnControllerUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060028D3 RID: 10451 RVA: 0x00118E20 File Offset: 0x00117020
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapEraseButton.MethodName._Ready || (ref method) == NMapEraseButton.MethodName.SetIsErasing || (ref method) == NMapEraseButton.MethodName.OnFocus || (ref method) == NMapEraseButton.MethodName.OnUnfocus || (ref method) == NMapEraseButton.MethodName.OnControllerUpdated || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060028D4 RID: 10452 RVA: 0x00118E80 File Offset: 0x00117080
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapEraseButton.PropertyName._drawingToolHolder)
			{
				this._drawingToolHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._isErasing)
			{
				this._isErasing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060028D5 RID: 10453 RVA: 0x00118F04 File Offset: 0x00117104
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapEraseButton.PropertyName._drawingToolHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._drawingToolHolder);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMapEraseButton.PropertyName._isErasing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isErasing);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028D6 RID: 10454 RVA: 0x00118F9C File Offset: 0x0011719C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapEraseButton.PropertyName._drawingToolHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapEraseButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapEraseButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapEraseButton.PropertyName._isErasing, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028D7 RID: 10455 RVA: 0x00119034 File Offset: 0x00117234
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapEraseButton.PropertyName._drawingToolHolder, Variant.From<Control>(ref this._drawingToolHolder));
			info.AddProperty(NMapEraseButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NMapEraseButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMapEraseButton.PropertyName._isErasing, Variant.From<bool>(ref this._isErasing));
		}

		/// <inheritdoc />
		// Token: 0x060028D8 RID: 10456 RVA: 0x001190A0 File Offset: 0x001172A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapEraseButton.PropertyName._drawingToolHolder, ref variant))
			{
				this._drawingToolHolder = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapEraseButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapEraseButton.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapEraseButton.PropertyName._isErasing, ref variant4))
			{
				this._isErasing = variant4.As<bool>();
			}
		}

		// Token: 0x04000F38 RID: 3896
		[Nullable(1)]
		private static readonly StringName _imagePath = "res://images/packed/map/drawing_eraser.png";

		// Token: 0x04000F39 RID: 3897
		[Nullable(1)]
		private static readonly StringName _glowImagePath = "res://images/packed/map/drawing_eraser_glow.png";

		// Token: 0x04000F3A RID: 3898
		[Nullable(1)]
		private Control _drawingToolHolder;

		// Token: 0x04000F3B RID: 3899
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000F3C RID: 3900
		private HoverTip _hoverTip;

		// Token: 0x04000F3D RID: 3901
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F3E RID: 3902
		private bool _isErasing;

		// Token: 0x04000F3F RID: 3903
		private static readonly Color _activeColor = new Color("FF5757FF");

		// Token: 0x04000F40 RID: 3904
		private static readonly Color _inactiveColor = new Color("FFFFFF80");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B3 RID: 5299
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E8E RID: 20110
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetIsErasing' method.
			/// </summary>
			// Token: 0x04004E8F RID: 20111
			public static readonly StringName SetIsErasing = "SetIsErasing";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E90 RID: 20112
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E91 RID: 20113
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnControllerUpdated' method.
			/// </summary>
			// Token: 0x04004E92 RID: 20114
			public static readonly StringName OnControllerUpdated = "OnControllerUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B4 RID: 5300
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_drawingToolHolder' field.
			/// </summary>
			// Token: 0x04004E93 RID: 20115
			public static readonly StringName _drawingToolHolder = "_drawingToolHolder";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004E94 RID: 20116
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004E95 RID: 20117
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_isErasing' field.
			/// </summary>
			// Token: 0x04004E96 RID: 20118
			public static readonly StringName _isErasing = "_isErasing";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B5 RID: 5301
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
