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
	// Token: 0x020002F1 RID: 753
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawButton.cs")]
	public class NMapDrawButton : NButton
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002872 RID: 10354 RVA: 0x00115BF2 File Offset: 0x00113DF2
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				List<string> list = new List<string>();
				list.Add(NMapDrawButton._imagePath);
				list.Add(NMapDrawButton._glowImagePath);
				list.AddRange(NMapEraseButton.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00115C2C File Offset: 0x00113E2C
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

		// Token: 0x06002874 RID: 10356 RVA: 0x00115CB0 File Offset: 0x00113EB0
		public void SetIsDrawing(bool isDrawing)
		{
			this._isDrawing = isDrawing;
			this._icon.Texture = PreloadManager.Cache.GetTexture2D(isDrawing ? NMapDrawButton._glowImagePath : NMapDrawButton._imagePath);
			this._icon.SelfModulate = (isDrawing ? NMapDrawButton._activeColor : NMapDrawButton._inactiveColor);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x00115D08 File Offset: 0x00113F08
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
			this._tween.TweenProperty(this._icon, "self_modulate", NMapDrawButton._activeColor, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._drawingToolHolder, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._drawingToolHolder.GlobalPosition + new Vector2(10f, -132f), false);
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x00115DE8 File Offset: 0x00113FE8
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
			this._tween.TweenProperty(this._icon, "self_modulate", this._isDrawing ? NMapDrawButton._activeColor : NMapDrawButton._inactiveColor, 0.05);
			NHoverTipSet.Remove(this._drawingToolHolder);
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x00115EA0 File Offset: 0x001140A0
		private void OnControllerUpdated()
		{
			LocString locString = new LocString("map", "DRAWING_BUTTON.description");
			LocString locString2;
			if (NControllerManager.Instance.IsUsingController)
			{
				locString2 = new LocString("map", "DRAWING_BUTTON.title_controller");
			}
			else
			{
				locString2 = new LocString("map", "DRAWING_BUTTON.title_mkb");
			}
			this._hoverTip = new HoverTip(locString2, locString, null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002878 RID: 10360 RVA: 0x00115EFC File Offset: 0x001140FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMapDrawButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawButton.MethodName.SetIsDrawing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isDrawing", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawButton.MethodName.OnControllerUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002879 RID: 10361 RVA: 0x00116024 File Offset: 0x00114224
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapDrawButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawButton.MethodName.SetIsDrawing && args.Count == 1)
			{
				this.SetIsDrawing(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawButton.MethodName.OnControllerUpdated && args.Count == 0)
			{
				this.OnControllerUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600287A RID: 10362 RVA: 0x00116104 File Offset: 0x00114304
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapDrawButton.MethodName._Ready || (ref method) == NMapDrawButton.MethodName.SetIsDrawing || (ref method) == NMapDrawButton.MethodName.OnFocus || (ref method) == NMapDrawButton.MethodName.OnUnfocus || (ref method) == NMapDrawButton.MethodName.OnControllerUpdated || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600287B RID: 10363 RVA: 0x00116164 File Offset: 0x00114364
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapDrawButton.PropertyName._drawingToolHolder)
			{
				this._drawingToolHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._isDrawing)
			{
				this._isDrawing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600287C RID: 10364 RVA: 0x001161E8 File Offset: 0x001143E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapDrawButton.PropertyName._drawingToolHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._drawingToolHolder);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMapDrawButton.PropertyName._isDrawing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDrawing);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600287D RID: 10365 RVA: 0x00116280 File Offset: 0x00114480
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapDrawButton.PropertyName._drawingToolHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapDrawButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapDrawButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapDrawButton.PropertyName._isDrawing, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600287E RID: 10366 RVA: 0x00116318 File Offset: 0x00114518
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapDrawButton.PropertyName._drawingToolHolder, Variant.From<Control>(ref this._drawingToolHolder));
			info.AddProperty(NMapDrawButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NMapDrawButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMapDrawButton.PropertyName._isDrawing, Variant.From<bool>(ref this._isDrawing));
		}

		/// <inheritdoc />
		// Token: 0x0600287F RID: 10367 RVA: 0x00116384 File Offset: 0x00114584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapDrawButton.PropertyName._drawingToolHolder, ref variant))
			{
				this._drawingToolHolder = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapDrawButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapDrawButton.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapDrawButton.PropertyName._isDrawing, ref variant4))
			{
				this._isDrawing = variant4.As<bool>();
			}
		}

		// Token: 0x04000F15 RID: 3861
		[Nullable(1)]
		private static readonly StringName _imagePath = "res://images/packed/map/drawing_quill.png";

		// Token: 0x04000F16 RID: 3862
		[Nullable(1)]
		private static readonly StringName _glowImagePath = "res://images/packed/map/drawing_quill_glow.png";

		// Token: 0x04000F17 RID: 3863
		[Nullable(1)]
		private Control _drawingToolHolder;

		// Token: 0x04000F18 RID: 3864
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000F19 RID: 3865
		private HoverTip _hoverTip;

		// Token: 0x04000F1A RID: 3866
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F1B RID: 3867
		private bool _isDrawing;

		// Token: 0x04000F1C RID: 3868
		private static readonly Color _activeColor = new Color("57C4FFFF");

		// Token: 0x04000F1D RID: 3869
		private static readonly Color _inactiveColor = new Color("FFFFFF80");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A4 RID: 5284
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E4F RID: 20047
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetIsDrawing' method.
			/// </summary>
			// Token: 0x04004E50 RID: 20048
			public static readonly StringName SetIsDrawing = "SetIsDrawing";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E51 RID: 20049
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E52 RID: 20050
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnControllerUpdated' method.
			/// </summary>
			// Token: 0x04004E53 RID: 20051
			public static readonly StringName OnControllerUpdated = "OnControllerUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A5 RID: 5285
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_drawingToolHolder' field.
			/// </summary>
			// Token: 0x04004E54 RID: 20052
			public static readonly StringName _drawingToolHolder = "_drawingToolHolder";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004E55 RID: 20053
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004E56 RID: 20054
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_isDrawing' field.
			/// </summary>
			// Token: 0x04004E57 RID: 20055
			public static readonly StringName _isDrawing = "_isDrawing";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A6 RID: 5286
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
