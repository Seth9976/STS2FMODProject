using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	// Token: 0x02000416 RID: 1046
	[ScriptPath("res://src/Core/Nodes/Cards/NCardHighlight.cs")]
	public class NCardHighlight : TextureRect
	{
		// Token: 0x06004280 RID: 17024 RVA: 0x00205418 File Offset: 0x00203618
		public override void _Ready()
		{
			this._shaderMaterial = (ShaderMaterial)base.Material;
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0020542C File Offset: 0x0020362C
		public void AnimShow()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween();
			this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), this.GetShaderParameter(), 0.075f, 0.5).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x002054A0 File Offset: 0x002036A0
		public void AnimHide()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween();
			this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), this.GetShaderParameter(), 0.0, 0.5);
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00205509 File Offset: 0x00203709
		public void AnimHideInstantly()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this.SetShaderParameter(0f);
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00205528 File Offset: 0x00203728
		public void AnimFlash()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween();
			this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), this.GetShaderParameter(), 0.15f, 0.1);
			this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), 0.15f, 0.075f, 0.35).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x002055D8 File Offset: 0x002037D8
		private float GetShaderParameter()
		{
			return this._shaderMaterial.GetShaderParameter(NCardHighlight._shaderParameterWidth).AsSingle();
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x002055FD File Offset: 0x002037FD
		private void SetShaderParameter(float val)
		{
			this._shaderMaterial.SetShaderParameter(NCardHighlight._shaderParameterWidth, val);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004287 RID: 17031 RVA: 0x00205618 File Offset: 0x00203818
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NCardHighlight.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.AnimShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.AnimHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.AnimHideInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.AnimFlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.GetShaderParameter, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHighlight.MethodName.SetShaderParameter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "val", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004288 RID: 17032 RVA: 0x0020579C File Offset: 0x0020399C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardHighlight.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.AnimShow && args.Count == 0)
			{
				this.AnimShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.AnimHide && args.Count == 0)
			{
				this.AnimHide();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.AnimHideInstantly && args.Count == 0)
			{
				this.AnimHideInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.AnimFlash && args.Count == 0)
			{
				this.AnimFlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.GetShaderParameter && args.Count == 0)
			{
				float shaderParameter = this.GetShaderParameter();
				ret = VariantUtils.CreateFrom<float>(ref shaderParameter);
				return true;
			}
			if ((ref method) == NCardHighlight.MethodName.SetShaderParameter && args.Count == 1)
			{
				this.SetShaderParameter(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004289 RID: 17033 RVA: 0x002058CC File Offset: 0x00203ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardHighlight.MethodName._Ready || (ref method) == NCardHighlight.MethodName.AnimShow || (ref method) == NCardHighlight.MethodName.AnimHide || (ref method) == NCardHighlight.MethodName.AnimHideInstantly || (ref method) == NCardHighlight.MethodName.AnimFlash || (ref method) == NCardHighlight.MethodName.GetShaderParameter || (ref method) == NCardHighlight.MethodName.SetShaderParameter || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600428A RID: 17034 RVA: 0x00205949 File Offset: 0x00203B49
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardHighlight.PropertyName._curTween)
			{
				this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardHighlight.PropertyName._shaderMaterial)
			{
				this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600428B RID: 17035 RVA: 0x0020598C File Offset: 0x00203B8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardHighlight.PropertyName._curTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
				return true;
			}
			if ((ref name) == NCardHighlight.PropertyName._shaderMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600428C RID: 17036 RVA: 0x002059E4 File Offset: 0x00203BE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardHighlight.PropertyName._curTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHighlight.PropertyName._shaderMaterial, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600428D RID: 17037 RVA: 0x00205A3A File Offset: 0x00203C3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardHighlight.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
			info.AddProperty(NCardHighlight.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
		}

		/// <inheritdoc />
		// Token: 0x0600428E RID: 17038 RVA: 0x00205A70 File Offset: 0x00203C70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardHighlight.PropertyName._curTween, ref variant))
			{
				this._curTween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardHighlight.PropertyName._shaderMaterial, ref variant2))
			{
				this._shaderMaterial = variant2.As<ShaderMaterial>();
			}
		}

		// Token: 0x040019F8 RID: 6648
		[Nullable(1)]
		private static readonly StringName _shaderParameterWidth = new StringName("width");

		// Token: 0x040019F9 RID: 6649
		public static readonly Color playableColor = new Color(0f, 0.957f, 0.988f, 0.98f);

		// Token: 0x040019FA RID: 6650
		public static readonly Color gold = new Color(1f, 0.784f, 0f, 0.98f);

		// Token: 0x040019FB RID: 6651
		public static readonly Color red = new Color(0.83f, 0f, 0.33f, 0.98f);

		// Token: 0x040019FC RID: 6652
		[Nullable(2)]
		private Tween _curTween;

		// Token: 0x040019FD RID: 6653
		[Nullable(1)]
		private ShaderMaterial _shaderMaterial;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200194B RID: 6475
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040063F1 RID: 25585
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimShow' method.
			/// </summary>
			// Token: 0x040063F2 RID: 25586
			public static readonly StringName AnimShow = "AnimShow";

			/// <summary>
			/// Cached name for the 'AnimHide' method.
			/// </summary>
			// Token: 0x040063F3 RID: 25587
			public static readonly StringName AnimHide = "AnimHide";

			/// <summary>
			/// Cached name for the 'AnimHideInstantly' method.
			/// </summary>
			// Token: 0x040063F4 RID: 25588
			public static readonly StringName AnimHideInstantly = "AnimHideInstantly";

			/// <summary>
			/// Cached name for the 'AnimFlash' method.
			/// </summary>
			// Token: 0x040063F5 RID: 25589
			public static readonly StringName AnimFlash = "AnimFlash";

			/// <summary>
			/// Cached name for the 'GetShaderParameter' method.
			/// </summary>
			// Token: 0x040063F6 RID: 25590
			public static readonly StringName GetShaderParameter = "GetShaderParameter";

			/// <summary>
			/// Cached name for the 'SetShaderParameter' method.
			/// </summary>
			// Token: 0x040063F7 RID: 25591
			public static readonly StringName SetShaderParameter = "SetShaderParameter";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200194C RID: 6476
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_curTween' field.
			/// </summary>
			// Token: 0x040063F8 RID: 25592
			public static readonly StringName _curTween = "_curTween";

			/// <summary>
			/// Cached name for the '_shaderMaterial' field.
			/// </summary>
			// Token: 0x040063F9 RID: 25593
			public static readonly StringName _shaderMaterial = "_shaderMaterial";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200194D RID: 6477
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
