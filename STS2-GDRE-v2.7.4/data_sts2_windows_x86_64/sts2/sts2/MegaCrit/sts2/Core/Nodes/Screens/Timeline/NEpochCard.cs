using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// Visually represents an Epoch.
	/// Used when animating images of Epochs or when we have them float about in space when we unlock Epochs.
	/// </summary>
	// Token: 0x0200025C RID: 604
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochCard.cs")]
	public class NEpochCard : Control
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x000D138C File Offset: 0x000CF58C
		[NullableContext(1)]
		public void Init(EpochModel epochModel)
		{
			this._glow = base.GetNode<TextureRect>("%GlowPlaceholder");
			this._portrait = base.GetNode<TextureRect>("%Portrait");
			this._portrait.Texture = epochModel.RealPortrait;
			base.Scale = Vector2.One;
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000D13E1 File Offset: 0x000CF5E1
		public override void _Ready()
		{
			this._mask = base.GetNode<TextureRect>("%Mask");
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000D13FC File Offset: 0x000CF5FC
		public override void _Process(double delta)
		{
			if (this._isWigglyUnlockPreviewMode)
			{
				this._time += this._noiseSpeed * (float)delta;
				float num = 42f * this._noise.GetNoise2D(this._time, 0f);
				float num2 = 42f * this._noise.GetNoise2D(0f, this._time);
				base.Position = new Vector2(num, num2) - base.GetPivotOffset();
			}
		}

		/// <summary>
		/// Used by the Unlock Epoch Screen
		/// </summary>
		// Token: 0x06001F76 RID: 8054 RVA: 0x000D147C File Offset: 0x000CF67C
		public void SetToWigglyUnlockPreviewMode()
		{
			this._noise = new FastNoiseLite();
			this._noise.Frequency = 0.2f;
			this._noise.SetSeed(Rng.Chaotic.NextInt(int.MaxValue));
			this._isWigglyUnlockPreviewMode = true;
			base.MouseFilter = 2L;
			base.Scale = Vector2.One * 1.2f;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000D14E4 File Offset: 0x000CF6E4
		private void GlowFlash()
		{
			base.Modulate = new Color(1f, 1f, 1f, 0.75f);
			this._glow.Modulate = Colors.Gold;
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			this._glowTween = base.CreateTween().SetParallel(true);
			this._glowTween.TweenProperty(this._glow, "modulate:a", 0.5f, 1.0).SetEase(1L).SetTrans(5L);
			this._glowTween.TweenProperty(this._glow, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L)
				.From(Vector2.One * 1.5f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F78 RID: 8056 RVA: 0x000D15D8 File Offset: 0x000CF7D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NEpochCard.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochCard.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochCard.MethodName.SetToWigglyUnlockPreviewMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochCard.MethodName.GlowFlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F79 RID: 8057 RVA: 0x000D16D0 File Offset: 0x000CF8D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochCard.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochCard.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochCard.MethodName.SetToWigglyUnlockPreviewMode && args.Count == 0)
			{
				this.SetToWigglyUnlockPreviewMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochCard.MethodName.GlowFlash && args.Count == 0)
			{
				this.GlowFlash();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F7A RID: 8058 RVA: 0x000D1788 File Offset: 0x000CF988
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochCard.MethodName._Ready || (ref method) == NEpochCard.MethodName._Process || (ref method) == NEpochCard.MethodName.SetToWigglyUnlockPreviewMode || (ref method) == NEpochCard.MethodName.GlowFlash || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001F7B RID: 8059 RVA: 0x000D17D8 File Offset: 0x000CF9D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochCard.PropertyName._glow)
			{
				this._glow = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._mask)
			{
				this._mask = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHoverable)
			{
				this._isHoverable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHeld)
			{
				this._isHeld = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isWigglyUnlockPreviewMode)
			{
				this._isWigglyUnlockPreviewMode = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._glowTween)
			{
				this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._targetScale)
			{
				this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._noiseSpeed)
			{
				this._noiseSpeed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._noise)
			{
				this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._denyTween)
			{
				this._denyTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._transparencyTween)
			{
				this._transparencyTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._blueGlowColor)
			{
				this._blueGlowColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._goldGlowColor)
			{
				this._goldGlowColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001F7C RID: 8060 RVA: 0x000D19B8 File Offset: 0x000CFBB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochCard.PropertyName._glow)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._glow);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._mask)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._mask);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHoverable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHoverable);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isHeld)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHeld);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._isWigglyUnlockPreviewMode)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isWigglyUnlockPreviewMode);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._glowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._targetScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<float>(ref this._time);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._noiseSpeed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._noiseSpeed);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._noise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._denyTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._denyTween);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._transparencyTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._transparencyTween);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._blueGlowColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._blueGlowColor);
				return true;
			}
			if ((ref name) == NEpochCard.PropertyName._goldGlowColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._goldGlowColor);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F7D RID: 8061 RVA: 0x000D1BF0 File Offset: 0x000CFDF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEpochCard.PropertyName._glow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._mask, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochCard.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochCard.PropertyName._isHoverable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochCard.PropertyName._isHeld, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochCard.PropertyName._isWigglyUnlockPreviewMode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._glowTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEpochCard.PropertyName._targetScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochCard.PropertyName._time, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochCard.PropertyName._noiseSpeed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._noise, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._denyTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._transparencyTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochCard.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(20L, NEpochCard.PropertyName._blueGlowColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NEpochCard.PropertyName._goldGlowColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F7E RID: 8062 RVA: 0x000D1E30 File Offset: 0x000D0030
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochCard.PropertyName._glow, Variant.From<TextureRect>(ref this._glow));
			info.AddProperty(NEpochCard.PropertyName._mask, Variant.From<TextureRect>(ref this._mask));
			info.AddProperty(NEpochCard.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NEpochCard.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NEpochCard.PropertyName._isHoverable, Variant.From<bool>(ref this._isHoverable));
			info.AddProperty(NEpochCard.PropertyName._isHeld, Variant.From<bool>(ref this._isHeld));
			info.AddProperty(NEpochCard.PropertyName._isWigglyUnlockPreviewMode, Variant.From<bool>(ref this._isWigglyUnlockPreviewMode));
			info.AddProperty(NEpochCard.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
			info.AddProperty(NEpochCard.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
			info.AddProperty(NEpochCard.PropertyName._time, Variant.From<float>(ref this._time));
			info.AddProperty(NEpochCard.PropertyName._noiseSpeed, Variant.From<float>(ref this._noiseSpeed));
			info.AddProperty(NEpochCard.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
			info.AddProperty(NEpochCard.PropertyName._denyTween, Variant.From<Tween>(ref this._denyTween));
			info.AddProperty(NEpochCard.PropertyName._transparencyTween, Variant.From<Tween>(ref this._transparencyTween));
			info.AddProperty(NEpochCard.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NEpochCard.PropertyName._blueGlowColor, Variant.From<Color>(ref this._blueGlowColor));
			info.AddProperty(NEpochCard.PropertyName._goldGlowColor, Variant.From<Color>(ref this._goldGlowColor));
		}

		/// <inheritdoc />
		// Token: 0x06001F7F RID: 8063 RVA: 0x000D1FBC File Offset: 0x000D01BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochCard.PropertyName._glow, ref variant))
			{
				this._glow = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochCard.PropertyName._mask, ref variant2))
			{
				this._mask = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochCard.PropertyName._portrait, ref variant3))
			{
				this._portrait = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEpochCard.PropertyName._isHovered, ref variant4))
			{
				this._isHovered = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEpochCard.PropertyName._isHoverable, ref variant5))
			{
				this._isHoverable = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEpochCard.PropertyName._isHeld, ref variant6))
			{
				this._isHeld = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEpochCard.PropertyName._isWigglyUnlockPreviewMode, ref variant7))
			{
				this._isWigglyUnlockPreviewMode = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEpochCard.PropertyName._glowTween, ref variant8))
			{
				this._glowTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEpochCard.PropertyName._targetScale, ref variant9))
			{
				this._targetScale = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEpochCard.PropertyName._time, ref variant10))
			{
				this._time = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEpochCard.PropertyName._noiseSpeed, ref variant11))
			{
				this._noiseSpeed = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEpochCard.PropertyName._noise, ref variant12))
			{
				this._noise = variant12.As<FastNoiseLite>();
			}
			Variant variant13;
			if (info.TryGetProperty(NEpochCard.PropertyName._denyTween, ref variant13))
			{
				this._denyTween = variant13.As<Tween>();
			}
			Variant variant14;
			if (info.TryGetProperty(NEpochCard.PropertyName._transparencyTween, ref variant14))
			{
				this._transparencyTween = variant14.As<Tween>();
			}
			Variant variant15;
			if (info.TryGetProperty(NEpochCard.PropertyName._scaleTween, ref variant15))
			{
				this._scaleTween = variant15.As<Tween>();
			}
			Variant variant16;
			if (info.TryGetProperty(NEpochCard.PropertyName._blueGlowColor, ref variant16))
			{
				this._blueGlowColor = variant16.As<Color>();
			}
			Variant variant17;
			if (info.TryGetProperty(NEpochCard.PropertyName._goldGlowColor, ref variant17))
			{
				this._goldGlowColor = variant17.As<Color>();
			}
		}

		// Token: 0x04000C18 RID: 3096
		[Nullable(1)]
		private TextureRect _glow;

		// Token: 0x04000C19 RID: 3097
		[Nullable(1)]
		private TextureRect _mask;

		// Token: 0x04000C1A RID: 3098
		[Nullable(1)]
		private TextureRect _portrait;

		// Token: 0x04000C1B RID: 3099
		private bool _isHovered;

		// Token: 0x04000C1C RID: 3100
		private bool _isHoverable = true;

		// Token: 0x04000C1D RID: 3101
		private bool _isHeld;

		// Token: 0x04000C1E RID: 3102
		private bool _isWigglyUnlockPreviewMode;

		// Token: 0x04000C1F RID: 3103
		[Nullable(2)]
		private Tween _glowTween;

		// Token: 0x04000C20 RID: 3104
		private Vector2 _targetScale;

		// Token: 0x04000C21 RID: 3105
		private float _time;

		// Token: 0x04000C22 RID: 3106
		private float _noiseSpeed = 0.25f;

		// Token: 0x04000C23 RID: 3107
		[Nullable(1)]
		private FastNoiseLite _noise;

		// Token: 0x04000C24 RID: 3108
		[Nullable(2)]
		private Tween _denyTween;

		// Token: 0x04000C25 RID: 3109
		[Nullable(2)]
		private Tween _transparencyTween;

		// Token: 0x04000C26 RID: 3110
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x04000C27 RID: 3111
		private Color _blueGlowColor = new Color("2de5ff80");

		// Token: 0x04000C28 RID: 3112
		private Color _goldGlowColor = new Color("ffd92e80");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200129F RID: 4767
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400484B RID: 18507
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400484C RID: 18508
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'SetToWigglyUnlockPreviewMode' method.
			/// </summary>
			// Token: 0x0400484D RID: 18509
			public static readonly StringName SetToWigglyUnlockPreviewMode = "SetToWigglyUnlockPreviewMode";

			/// <summary>
			/// Cached name for the 'GlowFlash' method.
			/// </summary>
			// Token: 0x0400484E RID: 18510
			public static readonly StringName GlowFlash = "GlowFlash";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A0 RID: 4768
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_glow' field.
			/// </summary>
			// Token: 0x0400484F RID: 18511
			public static readonly StringName _glow = "_glow";

			/// <summary>
			/// Cached name for the '_mask' field.
			/// </summary>
			// Token: 0x04004850 RID: 18512
			public static readonly StringName _mask = "_mask";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x04004851 RID: 18513
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x04004852 RID: 18514
			public static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_isHoverable' field.
			/// </summary>
			// Token: 0x04004853 RID: 18515
			public static readonly StringName _isHoverable = "_isHoverable";

			/// <summary>
			/// Cached name for the '_isHeld' field.
			/// </summary>
			// Token: 0x04004854 RID: 18516
			public static readonly StringName _isHeld = "_isHeld";

			/// <summary>
			/// Cached name for the '_isWigglyUnlockPreviewMode' field.
			/// </summary>
			// Token: 0x04004855 RID: 18517
			public static readonly StringName _isWigglyUnlockPreviewMode = "_isWigglyUnlockPreviewMode";

			/// <summary>
			/// Cached name for the '_glowTween' field.
			/// </summary>
			// Token: 0x04004856 RID: 18518
			public static readonly StringName _glowTween = "_glowTween";

			/// <summary>
			/// Cached name for the '_targetScale' field.
			/// </summary>
			// Token: 0x04004857 RID: 18519
			public static readonly StringName _targetScale = "_targetScale";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x04004858 RID: 18520
			public static readonly StringName _time = "_time";

			/// <summary>
			/// Cached name for the '_noiseSpeed' field.
			/// </summary>
			// Token: 0x04004859 RID: 18521
			public static readonly StringName _noiseSpeed = "_noiseSpeed";

			/// <summary>
			/// Cached name for the '_noise' field.
			/// </summary>
			// Token: 0x0400485A RID: 18522
			public static readonly StringName _noise = "_noise";

			/// <summary>
			/// Cached name for the '_denyTween' field.
			/// </summary>
			// Token: 0x0400485B RID: 18523
			public static readonly StringName _denyTween = "_denyTween";

			/// <summary>
			/// Cached name for the '_transparencyTween' field.
			/// </summary>
			// Token: 0x0400485C RID: 18524
			public static readonly StringName _transparencyTween = "_transparencyTween";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x0400485D RID: 18525
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_blueGlowColor' field.
			/// </summary>
			// Token: 0x0400485E RID: 18526
			public static readonly StringName _blueGlowColor = "_blueGlowColor";

			/// <summary>
			/// Cached name for the '_goldGlowColor' field.
			/// </summary>
			// Token: 0x0400485F RID: 18527
			public static readonly StringName _goldGlowColor = "_goldGlowColor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A1 RID: 4769
		public class SignalName : Control.SignalName
		{
		}
	}
}
