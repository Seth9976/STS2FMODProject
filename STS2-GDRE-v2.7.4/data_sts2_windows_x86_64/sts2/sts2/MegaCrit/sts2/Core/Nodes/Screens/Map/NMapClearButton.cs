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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002F0 RID: 752
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapClearButton.cs")]
	public class NMapClearButton : NButton
	{
		// Token: 0x06002865 RID: 10341 RVA: 0x00115608 File Offset: 0x00113808
		public override void _Ready()
		{
			this.ConnectSignals();
			this._drawingToolHolder = (Control)base.GetParent();
			this._icon = base.GetNode<TextureRect>("Icon");
			this._hoverTip = new HoverTip(new LocString("map", "CLEAR_DRAWING.title"), new LocString("map", "CLEAR_DRAWING.description"), null);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x0011566C File Offset: 0x0011386C
		protected override void OnFocus()
		{
			base.OnFocus();
			this._icon.Texture = PreloadManager.Cache.GetTexture2D(NMapClearButton._glowImagePath);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One * 1.2f, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", NMapClearButton._activeColor, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._drawingToolHolder, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._drawingToolHolder.GlobalPosition + new Vector2(10f, -132f), false);
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x0011576C File Offset: 0x0011396C
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._icon.Texture = PreloadManager.Cache.GetTexture2D(NMapClearButton._imagePath);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One * 1.1f, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", NMapClearButton._inactiveColor, 0.05);
			NHoverTipSet.Remove(this._drawingToolHolder);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002868 RID: 10344 RVA: 0x00115834 File Offset: 0x00113A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapClearButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapClearButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapClearButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002869 RID: 10345 RVA: 0x001158D8 File Offset: 0x00113AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapClearButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapClearButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapClearButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600286A RID: 10346 RVA: 0x0011595D File Offset: 0x00113B5D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapClearButton.MethodName._Ready || (ref method) == NMapClearButton.MethodName.OnFocus || (ref method) == NMapClearButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600286B RID: 10347 RVA: 0x00115994 File Offset: 0x00113B94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapClearButton.PropertyName._drawingToolHolder)
			{
				this._drawingToolHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapClearButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapClearButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600286C RID: 10348 RVA: 0x001159FC File Offset: 0x00113BFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapClearButton.PropertyName._drawingToolHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._drawingToolHolder);
				return true;
			}
			if ((ref name) == NMapClearButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NMapClearButton.PropertyName._tween)
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
		// Token: 0x0600286D RID: 10349 RVA: 0x00115A74 File Offset: 0x00113C74
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapClearButton.PropertyName._drawingToolHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapClearButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapClearButton.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600286E RID: 10350 RVA: 0x00115AEC File Offset: 0x00113CEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapClearButton.PropertyName._drawingToolHolder, Variant.From<Control>(ref this._drawingToolHolder));
			info.AddProperty(NMapClearButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NMapClearButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600286F RID: 10351 RVA: 0x00115B44 File Offset: 0x00113D44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapClearButton.PropertyName._drawingToolHolder, ref variant))
			{
				this._drawingToolHolder = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapClearButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapClearButton.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000F0D RID: 3853
		[Nullable(1)]
		private static readonly StringName _imagePath = "res://images/packed/map/drawing_clear.png";

		// Token: 0x04000F0E RID: 3854
		[Nullable(1)]
		private static readonly StringName _glowImagePath = "res://images/packed/map/drawing_clear_glow.png";

		// Token: 0x04000F0F RID: 3855
		[Nullable(1)]
		private Control _drawingToolHolder;

		// Token: 0x04000F10 RID: 3856
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000F11 RID: 3857
		private HoverTip _hoverTip;

		// Token: 0x04000F12 RID: 3858
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F13 RID: 3859
		private static readonly Color _activeColor = new Color("FFE57DFF");

		// Token: 0x04000F14 RID: 3860
		private static readonly Color _inactiveColor = new Color("FFFFFF80");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A1 RID: 5281
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E49 RID: 20041
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E4A RID: 20042
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E4B RID: 20043
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A2 RID: 5282
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_drawingToolHolder' field.
			/// </summary>
			// Token: 0x04004E4C RID: 20044
			public static readonly StringName _drawingToolHolder = "_drawingToolHolder";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004E4D RID: 20045
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004E4E RID: 20046
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A3 RID: 5283
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
