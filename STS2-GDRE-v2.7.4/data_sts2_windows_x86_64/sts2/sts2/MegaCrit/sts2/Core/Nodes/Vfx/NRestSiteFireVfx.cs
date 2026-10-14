using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E7 RID: 487
	[ScriptPath("res://src/Core/Nodes/Vfx/NRestSiteFireVfx.cs")]
	public class NRestSiteFireVfx : Node2D
	{
		// Token: 0x060018B5 RID: 6325 RVA: 0x0009AA7A File Offset: 0x00098C7A
		public override void _Ready()
		{
			if (!this._enabled)
			{
				return;
			}
			this._baseScale = base.Scale;
			this._baseSkew = base.Skew;
			this.Flicker();
			this.Sway();
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0009AAAC File Offset: 0x00098CAC
		private void Flicker()
		{
			Vector2 vector;
			vector..ctor(this._baseScale.X, Rng.Chaotic.NextFloat(this._baseScale.Y * this._minFlickerScale, this._baseScale.Y));
			Vector2 vector2;
			vector2..ctor(this._baseScale.X, Rng.Chaotic.NextFloat(this._baseScale.Y, this._baseScale.Y * this._maxFlickerScale));
			Tween tween = base.CreateTween();
			tween.TweenProperty(this, "scale", vector, (double)Rng.Chaotic.NextFloat(this._minFlickerTime, this._maxFlickerTime)).SetTrans(4L).SetEase(2L);
			tween.TweenProperty(this, "scale", vector2, (double)Rng.Chaotic.NextFloat(this._minFlickerTime, this._maxFlickerTime)).SetTrans(4L).SetEase(2L);
			tween.TweenCallback(Callable.From(new Action(this.Flicker)));
			this._scaleTweenRef = tween;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0009ABCC File Offset: 0x00098DCC
		private void Sway()
		{
			float num = Rng.Chaotic.NextFloat(this._baseSkew + this._minSkew, this._baseSkew);
			float num2 = Rng.Chaotic.NextFloat(this._baseSkew, this._baseSkew + this._maxSkew);
			Tween tween = base.CreateTween();
			tween.TweenProperty(this, "skew", num, (double)Rng.Chaotic.NextFloat(this._minSkewTime, this._maxSkewTime)).SetTrans(1L).SetEase(2L);
			tween.TweenProperty(this, "skew", num2, (double)Rng.Chaotic.NextFloat(this._minSkewTime, this._maxSkewTime)).SetTrans(1L).SetEase(2L);
			tween.TweenCallback(Callable.From(new Action(this.Sway)));
		}

		/// <summary>
		/// Extinguish the fire.
		/// NOTE: This is public because it'll be used by front-end rest site classes in the future.
		/// Remove this note once we actually use it and Rider stops warning us.
		/// </summary>
		// Token: 0x060018B8 RID: 6328 RVA: 0x0009ACB0 File Offset: 0x00098EB0
		public void Extinguish()
		{
			Tween tween = base.CreateTween().SetParallel(true);
			foreach (CpuParticles2D cpuParticles2D in this._cpuGlowParticles)
			{
				cpuParticles2D.Emitting = false;
				tween.TweenProperty(cpuParticles2D, "scale", Vector2.Zero, (double)this._extinguishTime).SetTrans(10L).SetEase(0L);
			}
			foreach (GpuParticles2D gpuParticles2D in this._gpuSparkParticles)
			{
				gpuParticles2D.Emitting = false;
			}
			Tween scaleTweenRef = this._scaleTweenRef;
			if (scaleTweenRef != null)
			{
				scaleTweenRef.Kill();
			}
			Tween tween2 = base.CreateTween();
			tween2.TweenProperty(this, "scale", Vector2.Zero, (double)this._extinguishTime).SetTrans(10L).SetEase(0L);
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0009ADCC File Offset: 0x00098FCC
		public override void _ExitTree()
		{
			Tween scaleTweenRef = this._scaleTweenRef;
			if (scaleTweenRef == null)
			{
				return;
			}
			scaleTweenRef.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018BA RID: 6330 RVA: 0x0009ADE0 File Offset: 0x00098FE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRestSiteFireVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteFireVfx.MethodName.Flicker, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteFireVfx.MethodName.Sway, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteFireVfx.MethodName.Extinguish, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteFireVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018BB RID: 6331 RVA: 0x0009AEE0 File Offset: 0x000990E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteFireVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteFireVfx.MethodName.Flicker && args.Count == 0)
			{
				this.Flicker();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteFireVfx.MethodName.Sway && args.Count == 0)
			{
				this.Sway();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteFireVfx.MethodName.Extinguish && args.Count == 0)
			{
				this.Extinguish();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteFireVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060018BC RID: 6332 RVA: 0x0009AFB0 File Offset: 0x000991B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSiteFireVfx.MethodName._Ready || (ref method) == NRestSiteFireVfx.MethodName.Flicker || (ref method) == NRestSiteFireVfx.MethodName.Sway || (ref method) == NRestSiteFireVfx.MethodName.Extinguish || (ref method) == NRestSiteFireVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060018BD RID: 6333 RVA: 0x0009B010 File Offset: 0x00099210
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSiteFireVfx.PropertyName._minFlickerScale)
			{
				this._minFlickerScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxFlickerScale)
			{
				this._maxFlickerScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minFlickerTime)
			{
				this._minFlickerTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxFlickerTime)
			{
				this._maxFlickerTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minSkew)
			{
				this._minSkew = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxSkew)
			{
				this._maxSkew = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minSkewTime)
			{
				this._minSkewTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxSkewTime)
			{
				this._maxSkewTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._extinguishTime)
			{
				this._extinguishTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._enabled)
			{
				this._enabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._cpuGlowParticles)
			{
				this._cpuGlowParticles = VariantUtils.ConvertToArray<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._gpuSparkParticles)
			{
				this._gpuSparkParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._baseSkew)
			{
				this._baseSkew = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._scaleTweenRef)
			{
				this._scaleTweenRef = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060018BE RID: 6334 RVA: 0x0009B1BC File Offset: 0x000993BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSiteFireVfx.PropertyName._minFlickerScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minFlickerScale);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxFlickerScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxFlickerScale);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minFlickerTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minFlickerTime);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxFlickerTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxFlickerTime);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minSkew)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minSkew);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxSkew)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxSkew);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._minSkewTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minSkewTime);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._maxSkewTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxSkewTime);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._extinguishTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._extinguishTime);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._enabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._enabled);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._cpuGlowParticles)
			{
				value = VariantUtils.CreateFromArray<CpuParticles2D>(this._cpuGlowParticles);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._gpuSparkParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._gpuSparkParticles);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._baseSkew)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseSkew);
				return true;
			}
			if ((ref name) == NRestSiteFireVfx.PropertyName._scaleTweenRef)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTweenRef);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018BF RID: 6335 RVA: 0x0009B3B4 File Offset: 0x000995B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._minFlickerScale, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._maxFlickerScale, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._minFlickerTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._maxFlickerTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._minSkew, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._maxSkew, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._minSkewTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._maxSkewTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._extinguishTime, 0L, "", 4102L, true),
				new PropertyInfo(1L, NRestSiteFireVfx.PropertyName._enabled, 0L, "", 4102L, true),
				new PropertyInfo(28L, NRestSiteFireVfx.PropertyName._cpuGlowParticles, 23L, "24/34:CPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NRestSiteFireVfx.PropertyName._gpuSparkParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(5L, NRestSiteFireVfx.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NRestSiteFireVfx.PropertyName._baseSkew, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteFireVfx.PropertyName._scaleTweenRef, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018C0 RID: 6336 RVA: 0x0009B5B0 File Offset: 0x000997B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRestSiteFireVfx.PropertyName._minFlickerScale, Variant.From<float>(ref this._minFlickerScale));
			info.AddProperty(NRestSiteFireVfx.PropertyName._maxFlickerScale, Variant.From<float>(ref this._maxFlickerScale));
			info.AddProperty(NRestSiteFireVfx.PropertyName._minFlickerTime, Variant.From<float>(ref this._minFlickerTime));
			info.AddProperty(NRestSiteFireVfx.PropertyName._maxFlickerTime, Variant.From<float>(ref this._maxFlickerTime));
			info.AddProperty(NRestSiteFireVfx.PropertyName._minSkew, Variant.From<float>(ref this._minSkew));
			info.AddProperty(NRestSiteFireVfx.PropertyName._maxSkew, Variant.From<float>(ref this._maxSkew));
			info.AddProperty(NRestSiteFireVfx.PropertyName._minSkewTime, Variant.From<float>(ref this._minSkewTime));
			info.AddProperty(NRestSiteFireVfx.PropertyName._maxSkewTime, Variant.From<float>(ref this._maxSkewTime));
			info.AddProperty(NRestSiteFireVfx.PropertyName._extinguishTime, Variant.From<float>(ref this._extinguishTime));
			info.AddProperty(NRestSiteFireVfx.PropertyName._enabled, Variant.From<bool>(ref this._enabled));
			info.AddProperty(NRestSiteFireVfx.PropertyName._cpuGlowParticles, Variant.CreateFrom<CpuParticles2D>(this._cpuGlowParticles));
			info.AddProperty(NRestSiteFireVfx.PropertyName._gpuSparkParticles, Variant.CreateFrom<GpuParticles2D>(this._gpuSparkParticles));
			info.AddProperty(NRestSiteFireVfx.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NRestSiteFireVfx.PropertyName._baseSkew, Variant.From<float>(ref this._baseSkew));
			info.AddProperty(NRestSiteFireVfx.PropertyName._scaleTweenRef, Variant.From<Tween>(ref this._scaleTweenRef));
		}

		/// <inheritdoc />
		// Token: 0x060018C1 RID: 6337 RVA: 0x0009B710 File Offset: 0x00099910
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minFlickerScale, ref variant))
			{
				this._minFlickerScale = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxFlickerScale, ref variant2))
			{
				this._maxFlickerScale = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minFlickerTime, ref variant3))
			{
				this._minFlickerTime = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxFlickerTime, ref variant4))
			{
				this._maxFlickerTime = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minSkew, ref variant5))
			{
				this._minSkew = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxSkew, ref variant6))
			{
				this._maxSkew = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minSkewTime, ref variant7))
			{
				this._minSkewTime = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxSkewTime, ref variant8))
			{
				this._maxSkewTime = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._extinguishTime, ref variant9))
			{
				this._extinguishTime = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._enabled, ref variant10))
			{
				this._enabled = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._cpuGlowParticles, ref variant11))
			{
				this._cpuGlowParticles = variant11.AsGodotArray<CpuParticles2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._gpuSparkParticles, ref variant12))
			{
				this._gpuSparkParticles = variant12.AsGodotArray<GpuParticles2D>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._baseScale, ref variant13))
			{
				this._baseScale = variant13.As<Vector2>();
			}
			Variant variant14;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._baseSkew, ref variant14))
			{
				this._baseSkew = variant14.As<float>();
			}
			Variant variant15;
			if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._scaleTweenRef, ref variant15))
			{
				this._scaleTweenRef = variant15.As<Tween>();
			}
		}

		// Token: 0x040008F0 RID: 2288
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _minFlickerScale = 0.85f;

		// Token: 0x040008F1 RID: 2289
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _maxFlickerScale = 1.05f;

		// Token: 0x040008F2 RID: 2290
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _minFlickerTime = 0.3f;

		// Token: 0x040008F3 RID: 2291
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _maxFlickerTime = 0.5f;

		// Token: 0x040008F4 RID: 2292
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _minSkew = -0.1f;

		// Token: 0x040008F5 RID: 2293
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _maxSkew = 0.1f;

		// Token: 0x040008F6 RID: 2294
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _minSkewTime = 0.8f;

		// Token: 0x040008F7 RID: 2295
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _maxSkewTime = 1.5f;

		// Token: 0x040008F8 RID: 2296
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _extinguishTime = 0.2f;

		// Token: 0x040008F9 RID: 2297
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _enabled = true;

		// Token: 0x040008FA RID: 2298
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<CpuParticles2D> _cpuGlowParticles = new Array<CpuParticles2D>();

		// Token: 0x040008FB RID: 2299
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _gpuSparkParticles = new Array<GpuParticles2D>();

		// Token: 0x040008FC RID: 2300
		private Vector2 _baseScale;

		// Token: 0x040008FD RID: 2301
		private float _baseSkew;

		// Token: 0x040008FE RID: 2302
		[Nullable(2)]
		private Tween _scaleTweenRef;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200110B RID: 4363
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004292 RID: 17042
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Flicker' method.
			/// </summary>
			// Token: 0x04004293 RID: 17043
			public static readonly StringName Flicker = "Flicker";

			/// <summary>
			/// Cached name for the 'Sway' method.
			/// </summary>
			// Token: 0x04004294 RID: 17044
			public static readonly StringName Sway = "Sway";

			/// <summary>
			/// Cached name for the 'Extinguish' method.
			/// </summary>
			// Token: 0x04004295 RID: 17045
			public static readonly StringName Extinguish = "Extinguish";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004296 RID: 17046
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200110C RID: 4364
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_minFlickerScale' field.
			/// </summary>
			// Token: 0x04004297 RID: 17047
			public static readonly StringName _minFlickerScale = "_minFlickerScale";

			/// <summary>
			/// Cached name for the '_maxFlickerScale' field.
			/// </summary>
			// Token: 0x04004298 RID: 17048
			public static readonly StringName _maxFlickerScale = "_maxFlickerScale";

			/// <summary>
			/// Cached name for the '_minFlickerTime' field.
			/// </summary>
			// Token: 0x04004299 RID: 17049
			public static readonly StringName _minFlickerTime = "_minFlickerTime";

			/// <summary>
			/// Cached name for the '_maxFlickerTime' field.
			/// </summary>
			// Token: 0x0400429A RID: 17050
			public static readonly StringName _maxFlickerTime = "_maxFlickerTime";

			/// <summary>
			/// Cached name for the '_minSkew' field.
			/// </summary>
			// Token: 0x0400429B RID: 17051
			public static readonly StringName _minSkew = "_minSkew";

			/// <summary>
			/// Cached name for the '_maxSkew' field.
			/// </summary>
			// Token: 0x0400429C RID: 17052
			public static readonly StringName _maxSkew = "_maxSkew";

			/// <summary>
			/// Cached name for the '_minSkewTime' field.
			/// </summary>
			// Token: 0x0400429D RID: 17053
			public static readonly StringName _minSkewTime = "_minSkewTime";

			/// <summary>
			/// Cached name for the '_maxSkewTime' field.
			/// </summary>
			// Token: 0x0400429E RID: 17054
			public static readonly StringName _maxSkewTime = "_maxSkewTime";

			/// <summary>
			/// Cached name for the '_extinguishTime' field.
			/// </summary>
			// Token: 0x0400429F RID: 17055
			public static readonly StringName _extinguishTime = "_extinguishTime";

			/// <summary>
			/// Cached name for the '_enabled' field.
			/// </summary>
			// Token: 0x040042A0 RID: 17056
			public static readonly StringName _enabled = "_enabled";

			/// <summary>
			/// Cached name for the '_cpuGlowParticles' field.
			/// </summary>
			// Token: 0x040042A1 RID: 17057
			public static readonly StringName _cpuGlowParticles = "_cpuGlowParticles";

			/// <summary>
			/// Cached name for the '_gpuSparkParticles' field.
			/// </summary>
			// Token: 0x040042A2 RID: 17058
			public static readonly StringName _gpuSparkParticles = "_gpuSparkParticles";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x040042A3 RID: 17059
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_baseSkew' field.
			/// </summary>
			// Token: 0x040042A4 RID: 17060
			public static readonly StringName _baseSkew = "_baseSkew";

			/// <summary>
			/// Cached name for the '_scaleTweenRef' field.
			/// </summary>
			// Token: 0x040042A5 RID: 17061
			public static readonly StringName _scaleTweenRef = "_scaleTweenRef";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200110D RID: 4365
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
