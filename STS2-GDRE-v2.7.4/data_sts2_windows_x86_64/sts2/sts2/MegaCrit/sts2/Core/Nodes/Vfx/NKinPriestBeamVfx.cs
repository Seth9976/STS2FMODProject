using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001C5 RID: 453
	[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestBeamVfx.cs")]
	public class NKinPriestBeamVfx : Node2D
	{
		// Token: 0x060016B0 RID: 5808 RVA: 0x0008BB50 File Offset: 0x00089D50
		public override void _Ready()
		{
			this._beam = base.GetNode<Sprite2D>("BeamHolder/Beam");
			this._staticParticles = base.GetNode<GpuParticles2D>("BeamHolder/StaticParticles");
			this._beamHolder = base.GetNode<Node2D>("BeamHolder");
			this._baseBeamScale = this._beam.Scale;
			this._staticParticles.Emitting = false;
			this._staticParticles.Visible = false;
			this._beamHolder.Visible = false;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0008BBD4 File Offset: 0x00089DD4
		public override void _Process(double delta)
		{
			Vector2 vector;
			vector..ctor(Rng.Chaotic.NextFloat(-0.05f, 0.05f), Rng.Chaotic.NextFloat(-0.7f, 0.7f));
			this._beam.Scale = this._baseBeamScale + vector;
			Color modulate = base.Modulate;
			modulate.A = Rng.Chaotic.NextFloat(0.8f, 1f);
			base.Modulate = modulate;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0008BC50 File Offset: 0x00089E50
		public void Fire()
		{
			this._staticParticles.Restart();
			this._staticParticles.Visible = true;
			this._beamHolder.Visible = true;
			this._rotationTween = base.CreateTween();
			base.RotationDegrees = 1f;
			this._rotationTween.TweenProperty(this, "rotation_degrees", -1f, 0.800000011920929).SetEase(1L).SetTrans(7L);
			this._beamHolder.Scale = Vector2.One;
			this._lengthTween = base.CreateTween();
			this._lengthTween.TweenProperty(this._beamHolder, "scale:x", 4f, 0.3799999952316284).SetEase(1L).SetTrans(5L);
			this._lengthTween.Chain().TweenProperty(this._beamHolder, "scale:x", 0.5, 0.6000000238418579).SetEase(0L)
				.SetTrans(5L);
			this._lengthTween.TweenCallback(Callable.From(new Action(this.OnTweenComplete)));
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0008BD8B File Offset: 0x00089F8B
		private void OnTweenComplete()
		{
			this._rotationTween.Kill();
			this._lengthTween.Kill();
			this._staticParticles.Emitting = false;
			this._staticParticles.Visible = false;
			this._beamHolder.Visible = false;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0008BDC7 File Offset: 0x00089FC7
		public override void _ExitTree()
		{
			Tween lengthTween = this._lengthTween;
			if (lengthTween != null)
			{
				lengthTween.Kill();
			}
			Tween rotationTween = this._rotationTween;
			if (rotationTween == null)
			{
				return;
			}
			rotationTween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016B5 RID: 5813 RVA: 0x0008BDEC File Offset: 0x00089FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NKinPriestBeamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestBeamVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NKinPriestBeamVfx.MethodName.Fire, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestBeamVfx.MethodName.OnTweenComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestBeamVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016B6 RID: 5814 RVA: 0x0008BF14 File Offset: 0x0008A114
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKinPriestBeamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestBeamVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestBeamVfx.MethodName.Fire && args.Count == 0)
			{
				this.Fire();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestBeamVfx.MethodName.OnTweenComplete && args.Count == 0)
			{
				this.OnTweenComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestBeamVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060016B7 RID: 5815 RVA: 0x0008BFF4 File Offset: 0x0008A1F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKinPriestBeamVfx.MethodName._Ready || (ref method) == NKinPriestBeamVfx.MethodName._Process || (ref method) == NKinPriestBeamVfx.MethodName.Fire || (ref method) == NKinPriestBeamVfx.MethodName.OnTweenComplete || (ref method) == NKinPriestBeamVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016B8 RID: 5816 RVA: 0x0008C054 File Offset: 0x0008A254
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKinPriestBeamVfx.PropertyName._beam)
			{
				this._beam = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._beamHolder)
			{
				this._beamHolder = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._staticParticles)
			{
				this._staticParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._baseBeamScale)
			{
				this._baseBeamScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._lengthTween)
			{
				this._lengthTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._rotationTween)
			{
				this._rotationTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016B9 RID: 5817 RVA: 0x0008C10C File Offset: 0x0008A30C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKinPriestBeamVfx.PropertyName._beam)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._beam);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._beamHolder)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._beamHolder);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._staticParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._staticParticles);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._baseBeamScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseBeamScale);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._lengthTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._lengthTween);
				return true;
			}
			if ((ref name) == NKinPriestBeamVfx.PropertyName._rotationTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._rotationTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016BA RID: 5818 RVA: 0x0008C1E4 File Offset: 0x0008A3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKinPriestBeamVfx.PropertyName._beam, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestBeamVfx.PropertyName._beamHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestBeamVfx.PropertyName._staticParticles, 0L, "", 4096L, false),
				new PropertyInfo(5L, NKinPriestBeamVfx.PropertyName._baseBeamScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestBeamVfx.PropertyName._lengthTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestBeamVfx.PropertyName._rotationTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016BB RID: 5819 RVA: 0x0008C2C0 File Offset: 0x0008A4C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKinPriestBeamVfx.PropertyName._beam, Variant.From<Sprite2D>(ref this._beam));
			info.AddProperty(NKinPriestBeamVfx.PropertyName._beamHolder, Variant.From<Node2D>(ref this._beamHolder));
			info.AddProperty(NKinPriestBeamVfx.PropertyName._staticParticles, Variant.From<GpuParticles2D>(ref this._staticParticles));
			info.AddProperty(NKinPriestBeamVfx.PropertyName._baseBeamScale, Variant.From<Vector2>(ref this._baseBeamScale));
			info.AddProperty(NKinPriestBeamVfx.PropertyName._lengthTween, Variant.From<Tween>(ref this._lengthTween));
			info.AddProperty(NKinPriestBeamVfx.PropertyName._rotationTween, Variant.From<Tween>(ref this._rotationTween));
		}

		/// <inheritdoc />
		// Token: 0x060016BC RID: 5820 RVA: 0x0008C358 File Offset: 0x0008A558
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._beam, ref variant))
			{
				this._beam = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._beamHolder, ref variant2))
			{
				this._beamHolder = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._staticParticles, ref variant3))
			{
				this._staticParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._baseBeamScale, ref variant4))
			{
				this._baseBeamScale = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._lengthTween, ref variant5))
			{
				this._lengthTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._rotationTween, ref variant6))
			{
				this._rotationTween = variant6.As<Tween>();
			}
		}

		// Token: 0x04000815 RID: 2069
		private const float _beamMaxLengthScale = 4f;

		// Token: 0x04000816 RID: 2070
		private const float _startRotation = 1f;

		// Token: 0x04000817 RID: 2071
		private const float _endRotation = -1f;

		// Token: 0x04000818 RID: 2072
		[Nullable(1)]
		private Sprite2D _beam;

		// Token: 0x04000819 RID: 2073
		[Nullable(1)]
		private Node2D _beamHolder;

		// Token: 0x0400081A RID: 2074
		[Nullable(1)]
		private GpuParticles2D _staticParticles;

		// Token: 0x0400081B RID: 2075
		private Vector2 _baseBeamScale;

		// Token: 0x0400081C RID: 2076
		[Nullable(2)]
		private Tween _lengthTween;

		// Token: 0x0400081D RID: 2077
		[Nullable(2)]
		private Tween _rotationTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108D RID: 4237
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004117 RID: 16663
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004118 RID: 16664
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'Fire' method.
			/// </summary>
			// Token: 0x04004119 RID: 16665
			public static readonly StringName Fire = "Fire";

			/// <summary>
			/// Cached name for the 'OnTweenComplete' method.
			/// </summary>
			// Token: 0x0400411A RID: 16666
			public static readonly StringName OnTweenComplete = "OnTweenComplete";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400411B RID: 16667
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108E RID: 4238
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_beam' field.
			/// </summary>
			// Token: 0x0400411C RID: 16668
			public static readonly StringName _beam = "_beam";

			/// <summary>
			/// Cached name for the '_beamHolder' field.
			/// </summary>
			// Token: 0x0400411D RID: 16669
			public static readonly StringName _beamHolder = "_beamHolder";

			/// <summary>
			/// Cached name for the '_staticParticles' field.
			/// </summary>
			// Token: 0x0400411E RID: 16670
			public static readonly StringName _staticParticles = "_staticParticles";

			/// <summary>
			/// Cached name for the '_baseBeamScale' field.
			/// </summary>
			// Token: 0x0400411F RID: 16671
			public static readonly StringName _baseBeamScale = "_baseBeamScale";

			/// <summary>
			/// Cached name for the '_lengthTween' field.
			/// </summary>
			// Token: 0x04004120 RID: 16672
			public static readonly StringName _lengthTween = "_lengthTween";

			/// <summary>
			/// Cached name for the '_rotationTween' field.
			/// </summary>
			// Token: 0x04004121 RID: 16673
			public static readonly StringName _rotationTween = "_rotationTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108F RID: 4239
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
