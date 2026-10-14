using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002A7 RID: 679
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NPaginateArrow.cs")]
	public class NPaginateArrow : NButton
	{
		// Token: 0x060023F7 RID: 9207 RVA: 0x000F41F0 File Offset: 0x000F23F0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._paginator = base.GetParent<NPaginator>();
			this._image = base.GetNode<TextureRect>("Image");
			this._baseScale = this._image.Scale;
			this._hsv = (ShaderMaterial)this._image.Material;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000F424C File Offset: 0x000F244C
		protected override void OnRelease()
		{
			if (this._pageLeft)
			{
				NPaginator paginator = this._paginator;
				if (paginator != null)
				{
					paginator.PageLeft();
				}
			}
			else
			{
				NPaginator paginator2 = this._paginator;
				if (paginator2 != null)
				{
					paginator2.PageRight();
				}
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 1.1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), 1.5f, 0.05);
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000F4324 File Offset: 0x000F2524
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 1.1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), 1.5f, 0.05);
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000F43D4 File Offset: 0x000F25D4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), 0.9f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000F4498 File Offset: 0x000F2698
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000F4564 File Offset: 0x000F2764
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NPaginateArrow._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023FD RID: 9213 RVA: 0x000F457C File Offset: 0x000F277C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NPaginateArrow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginateArrow.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginateArrow.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginateArrow.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginateArrow.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginateArrow.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023FE RID: 9214 RVA: 0x000F46D0 File Offset: 0x000F28D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPaginateArrow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginateArrow.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginateArrow.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginateArrow.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginateArrow.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginateArrow.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060023FF RID: 9215 RVA: 0x000F47D4 File Offset: 0x000F29D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPaginateArrow.MethodName._Ready || (ref method) == NPaginateArrow.MethodName.OnRelease || (ref method) == NPaginateArrow.MethodName.OnFocus || (ref method) == NPaginateArrow.MethodName.OnUnfocus || (ref method) == NPaginateArrow.MethodName.OnPress || (ref method) == NPaginateArrow.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002400 RID: 9216 RVA: 0x000F4844 File Offset: 0x000F2A44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPaginateArrow.PropertyName._pageLeft)
			{
				this._pageLeft = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._paginator)
			{
				this._paginator = VariantUtils.ConvertTo<NPaginator>(ref value);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002401 RID: 9217 RVA: 0x000F48FC File Offset: 0x000F2AFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPaginateArrow.PropertyName._pageLeft)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._pageLeft);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._paginator)
			{
				value = VariantUtils.CreateFrom<NPaginator>(ref this._paginator);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NPaginateArrow.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002402 RID: 9218 RVA: 0x000F49D4 File Offset: 0x000F2BD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NPaginateArrow.PropertyName._pageLeft, 0L, "", 4102L, true),
				new PropertyInfo(24L, NPaginateArrow.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginateArrow.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginateArrow.PropertyName._paginator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginateArrow.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPaginateArrow.PropertyName._baseScale, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002403 RID: 9219 RVA: 0x000F4AAC File Offset: 0x000F2CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPaginateArrow.PropertyName._pageLeft, Variant.From<bool>(ref this._pageLeft));
			info.AddProperty(NPaginateArrow.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NPaginateArrow.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NPaginateArrow.PropertyName._paginator, Variant.From<NPaginator>(ref this._paginator));
			info.AddProperty(NPaginateArrow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NPaginateArrow.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
		}

		/// <inheritdoc />
		// Token: 0x06002404 RID: 9220 RVA: 0x000F4B44 File Offset: 0x000F2D44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._pageLeft, ref variant))
			{
				this._pageLeft = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._paginator, ref variant4))
			{
				this._paginator = variant4.As<NPaginator>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPaginateArrow.PropertyName._baseScale, ref variant6))
			{
				this._baseScale = variant6.As<Vector2>();
			}
		}

		// Token: 0x04000D9D RID: 3485
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000D9E RID: 3486
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _pageLeft;

		// Token: 0x04000D9F RID: 3487
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000DA0 RID: 3488
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04000DA1 RID: 3489
		[Nullable(2)]
		private NPaginator _paginator;

		// Token: 0x04000DA2 RID: 3490
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000DA3 RID: 3491
		private Vector2 _baseScale;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AB RID: 5035
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B81 RID: 19329
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004B82 RID: 19330
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004B83 RID: 19331
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004B84 RID: 19332
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004B85 RID: 19333
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004B86 RID: 19334
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AC RID: 5036
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_pageLeft' field.
			/// </summary>
			// Token: 0x04004B87 RID: 19335
			public static readonly StringName _pageLeft = "_pageLeft";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004B88 RID: 19336
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004B89 RID: 19337
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_paginator' field.
			/// </summary>
			// Token: 0x04004B8A RID: 19338
			public static readonly StringName _paginator = "_paginator";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004B8B RID: 19339
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x04004B8C RID: 19340
			public static readonly StringName _baseScale = "_baseScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AD RID: 5037
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
