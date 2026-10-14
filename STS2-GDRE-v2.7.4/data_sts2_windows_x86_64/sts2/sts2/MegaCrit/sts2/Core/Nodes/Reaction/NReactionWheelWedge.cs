using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Reaction
{
	// Token: 0x0200037D RID: 893
	[ScriptPath("res://src/Core/Nodes/Reaction/NReactionWheelWedge.cs")]
	public class NReactionWheelWedge : TextureRect
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x0018A5CD File Offset: 0x001887CD
		[Nullable(1)]
		public Texture2D Reaction
		{
			[NullableContext(1)]
			get
			{
				return this._textureRect.Texture;
			}
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0018A5DA File Offset: 0x001887DA
		public override void _Ready()
		{
			this._textureRect = base.GetNode<TextureRect>("TextureRect");
			this._defaultPosition = base.Position;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x0018A600 File Offset: 0x00188800
		public void OnSelected()
		{
			Vector2 vector = Vector2.Right.Rotated(base.Rotation);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.SetParallel(true);
			this._tween.TweenProperty(this, "position", this._defaultPosition + vector * 25f, 0.05).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this, "self_modulate", NReactionWheelWedge._selectedColor, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x0018A6CC File Offset: 0x001888CC
		public void OnDeselected()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.SetParallel(true);
			this._tween.TweenProperty(this, "position", this._defaultPosition, 0.2).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this, "self_modulate", NReactionWheelWedge._defaultColor, 0.2).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034F1 RID: 13553 RVA: 0x0018A774 File Offset: 0x00188974
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NReactionWheelWedge.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheelWedge.MethodName.OnSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheelWedge.MethodName.OnDeselected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034F2 RID: 13554 RVA: 0x0018A818 File Offset: 0x00188A18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReactionWheelWedge.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheelWedge.MethodName.OnSelected && args.Count == 0)
			{
				this.OnSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheelWedge.MethodName.OnDeselected && args.Count == 0)
			{
				this.OnDeselected();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060034F3 RID: 13555 RVA: 0x0018A89D File Offset: 0x00188A9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NReactionWheelWedge.MethodName._Ready || (ref method) == NReactionWheelWedge.MethodName.OnSelected || (ref method) == NReactionWheelWedge.MethodName.OnDeselected || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060034F4 RID: 13556 RVA: 0x0018A8D4 File Offset: 0x00188AD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NReactionWheelWedge.PropertyName._textureRect)
			{
				this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._normal)
			{
				this._normal = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._defaultPosition)
			{
				this._defaultPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060034F5 RID: 13557 RVA: 0x0018A958 File Offset: 0x00188B58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NReactionWheelWedge.PropertyName.Reaction)
			{
				Texture2D reaction = this.Reaction;
				value = VariantUtils.CreateFrom<Texture2D>(ref reaction);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._textureRect)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._normal)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._normal);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NReactionWheelWedge.PropertyName._defaultPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._defaultPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034F6 RID: 13558 RVA: 0x0018AA10 File Offset: 0x00188C10
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NReactionWheelWedge.PropertyName._textureRect, 0L, "", 4096L, false),
				new PropertyInfo(5L, NReactionWheelWedge.PropertyName._normal, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheelWedge.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NReactionWheelWedge.PropertyName._defaultPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheelWedge.PropertyName.Reaction, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034F7 RID: 13559 RVA: 0x0018AAC8 File Offset: 0x00188CC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NReactionWheelWedge.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
			info.AddProperty(NReactionWheelWedge.PropertyName._normal, Variant.From<Vector2>(ref this._normal));
			info.AddProperty(NReactionWheelWedge.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NReactionWheelWedge.PropertyName._defaultPosition, Variant.From<Vector2>(ref this._defaultPosition));
		}

		/// <inheritdoc />
		// Token: 0x060034F8 RID: 13560 RVA: 0x0018AB34 File Offset: 0x00188D34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NReactionWheelWedge.PropertyName._textureRect, ref variant))
			{
				this._textureRect = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NReactionWheelWedge.PropertyName._normal, ref variant2))
			{
				this._normal = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NReactionWheelWedge.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NReactionWheelWedge.PropertyName._defaultPosition, ref variant4))
			{
				this._defaultPosition = variant4.As<Vector2>();
			}
		}

		// Token: 0x0400148C RID: 5260
		private static readonly Color _defaultColor = new Color("e0f9ff40");

		// Token: 0x0400148D RID: 5261
		private static readonly Color _selectedColor = new Color("c2f3ffc0");

		// Token: 0x0400148E RID: 5262
		[Nullable(1)]
		private TextureRect _textureRect;

		// Token: 0x0400148F RID: 5263
		private Vector2 _normal;

		// Token: 0x04001490 RID: 5264
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001491 RID: 5265
		private Vector2 _defaultPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E1 RID: 5857
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400592A RID: 22826
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSelected' method.
			/// </summary>
			// Token: 0x0400592B RID: 22827
			public static readonly StringName OnSelected = "OnSelected";

			/// <summary>
			/// Cached name for the 'OnDeselected' method.
			/// </summary>
			// Token: 0x0400592C RID: 22828
			public static readonly StringName OnDeselected = "OnDeselected";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E2 RID: 5858
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Reaction' property.
			/// </summary>
			// Token: 0x0400592D RID: 22829
			public static readonly StringName Reaction = "Reaction";

			/// <summary>
			/// Cached name for the '_textureRect' field.
			/// </summary>
			// Token: 0x0400592E RID: 22830
			public static readonly StringName _textureRect = "_textureRect";

			/// <summary>
			/// Cached name for the '_normal' field.
			/// </summary>
			// Token: 0x0400592F RID: 22831
			public static readonly StringName _normal = "_normal";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005930 RID: 22832
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_defaultPosition' field.
			/// </summary>
			// Token: 0x04005931 RID: 22833
			public static readonly StringName _defaultPosition = "_defaultPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E3 RID: 5859
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
