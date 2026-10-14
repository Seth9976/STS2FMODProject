using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200019C RID: 412
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NDecimillipedeSegmentVfx.cs")]
	public class NDecimillipedeSegmentVfx : Node
	{
		// Token: 0x06001445 RID: 5189 RVA: 0x0007A49C File Offset: 0x0007869C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			foreach (Node2D node2D in this._sprayNodes)
			{
				node2D.Visible = false;
				this._sprayNodeScales.Add(node2D.Scale);
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0007A51C File Offset: 0x0007871C
		public void Regenerate()
		{
			for (int i = 0; i < this._sprayNodes.Length; i++)
			{
				Node2D node2D = this._sprayNodes[i];
				node2D.Visible = true;
				ShaderMaterial shaderMaterial = (ShaderMaterial)node2D.Material;
				shaderMaterial.SetShaderParameter(NDecimillipedeSegmentVfx._direction, -1);
				Tween tween = base.CreateTween().SetParallel(true);
				float num = Rng.Chaotic.NextFloat(0.5f);
				shaderMaterial.SetShaderParameter(NDecimillipedeSegmentVfx._opacity, 0.5f);
				tween.TweenProperty(node2D, "scale", this._sprayNodeScales[i], (double)(num + 0.5f)).SetTrans(5L).SetEase(1L);
				tween.TweenProperty(node2D.Material, "shader_parameter/opacity", 0.9f, (double)num).SetTrans(5L).SetEase(1L);
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0007A610 File Offset: 0x00078810
		private void EndRegenerate()
		{
			foreach (Node2D node2D in this._sprayNodes)
			{
				Vector2 scale = node2D.Scale;
				Tween tween = base.CreateTween();
				float num = Rng.Chaotic.NextFloat(0.9f, 1.25f);
				tween.TweenProperty(node2D, "scale", Vector2.Zero, (double)num).SetTrans(4L).SetEase(0L);
				tween.TweenProperty(node2D, "visible", false, 0.0);
				tween.TweenProperty(node2D, "scale", scale, 0.0);
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0007A6D4 File Offset: 0x000788D4
		private void Wither()
		{
			foreach (CpuParticles2D cpuParticles2D in this._damageParticleNodes)
			{
				cpuParticles2D.Gravity = this._particleGravity;
				cpuParticles2D.SpeedScale = (double)this._particleSpeedScale;
				cpuParticles2D.InitialVelocityMin = this._particleVelocityMinMax.X;
				cpuParticles2D.InitialVelocityMax = this._particleVelocityMinMax.Y;
				cpuParticles2D.Restart();
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0007A73C File Offset: 0x0007893C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject animEvent)
		{
			string eventName = new MegaEvent(animEvent).GetData().GetEventName();
			if (eventName == "suck_complete")
			{
				this.EndRegenerate();
				return;
			}
			if (!(eventName == "explode"))
			{
				return;
			}
			this.Wither();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600144A RID: 5194 RVA: 0x0007A788 File Offset: 0x00078988
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NDecimillipedeSegmentVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeSegmentVfx.MethodName.Regenerate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeSegmentVfx.MethodName.EndRegenerate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeSegmentVfx.MethodName.Wither, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600144B RID: 5195 RVA: 0x0007A93C File Offset: 0x00078B3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDecimillipedeSegmentVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentVfx.MethodName.Regenerate && args.Count == 0)
			{
				this.Regenerate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentVfx.MethodName.EndRegenerate && args.Count == 0)
			{
				this.EndRegenerate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentVfx.MethodName.Wither && args.Count == 0)
			{
				this.Wither();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600144C RID: 5196 RVA: 0x0007AA40 File Offset: 0x00078C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDecimillipedeSegmentVfx.MethodName._Ready || (ref method) == NDecimillipedeSegmentVfx.MethodName.Regenerate || (ref method) == NDecimillipedeSegmentVfx.MethodName.EndRegenerate || (ref method) == NDecimillipedeSegmentVfx.MethodName.Wither || (ref method) == NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600144D RID: 5197 RVA: 0x0007AAA0 File Offset: 0x00078CA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes)
			{
				this._damageParticleNodes = VariantUtils.ConvertToSystemArrayOfGodotObject<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale)
			{
				this._particleSpeedScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax)
			{
				this._particleVelocityMinMax = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._sprayNodes)
			{
				this._sprayNodes = VariantUtils.ConvertToSystemArrayOfGodotObject<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600144E RID: 5198 RVA: 0x0007AB3C File Offset: 0x00078D3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes)
			{
				GodotObject[] array = this._damageParticleNodes;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._particleGravity)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._particleGravity);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._particleSpeedScale);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._particleVelocityMinMax);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._sprayNodes)
			{
				GodotObject[] array = this._sprayNodes;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600144F RID: 5199 RVA: 0x0007AC18 File Offset: 0x00078E18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes, 23L, "24/34:CPUParticles2D", 4102L, true),
				new PropertyInfo(5L, NDecimillipedeSegmentVfx.PropertyName._particleGravity, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, 0L, "", 4096L, false),
				new PropertyInfo(28L, NDecimillipedeSegmentVfx.PropertyName._sprayNodes, 23L, "24/34:Node2D", 4102L, true),
				new PropertyInfo(24L, NDecimillipedeSegmentVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001450 RID: 5200 RVA: 0x0007ACF4 File Offset: 0x00078EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName damageParticleNodes = NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes;
			GodotObject[] array = this._damageParticleNodes;
			info.AddProperty(damageParticleNodes, Variant.CreateFrom(array));
			info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, Variant.From<float>(ref this._particleSpeedScale));
			info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, Variant.From<Vector2>(ref this._particleVelocityMinMax));
			StringName sprayNodes = NDecimillipedeSegmentVfx.PropertyName._sprayNodes;
			array = this._sprayNodes;
			info.AddProperty(sprayNodes, Variant.CreateFrom(array));
			info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001451 RID: 5201 RVA: 0x0007AD7C File Offset: 0x00078F7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes, ref variant))
			{
				this._damageParticleNodes = variant.AsGodotObjectArray<CpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, ref variant2))
			{
				this._particleSpeedScale = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, ref variant3))
			{
				this._particleVelocityMinMax = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._sprayNodes, ref variant4))
			{
				this._sprayNodes = variant4.AsGodotObjectArray<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._parent, ref variant5))
			{
				this._parent = variant5.As<Node2D>();
			}
		}

		// Token: 0x04000739 RID: 1849
		[Nullable(1)]
		private static readonly StringName _opacity = new StringName("opacity");

		// Token: 0x0400073A RID: 1850
		[Nullable(1)]
		private static readonly StringName _direction = new StringName("direction");

		// Token: 0x0400073B RID: 1851
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CpuParticles2D[] _damageParticleNodes;

		// Token: 0x0400073C RID: 1852
		private readonly Vector2 _particleGravity = new Vector2(0f, 300f);

		// Token: 0x0400073D RID: 1853
		private float _particleSpeedScale = 2f;

		// Token: 0x0400073E RID: 1854
		private Vector2 _particleVelocityMinMax = new Vector2(400f, 600f);

		// Token: 0x0400073F RID: 1855
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D[] _sprayNodes;

		// Token: 0x04000740 RID: 1856
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000741 RID: 1857
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x04000742 RID: 1858
		[Nullable(1)]
		private readonly List<Vector2> _sprayNodeScales = new List<Vector2>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFA RID: 4090
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F70 RID: 16240
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Regenerate' method.
			/// </summary>
			// Token: 0x04003F71 RID: 16241
			public static readonly StringName Regenerate = "Regenerate";

			/// <summary>
			/// Cached name for the 'EndRegenerate' method.
			/// </summary>
			// Token: 0x04003F72 RID: 16242
			public static readonly StringName EndRegenerate = "EndRegenerate";

			/// <summary>
			/// Cached name for the 'Wither' method.
			/// </summary>
			// Token: 0x04003F73 RID: 16243
			public static readonly StringName Wither = "Wither";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003F74 RID: 16244
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFB RID: 4091
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_damageParticleNodes' field.
			/// </summary>
			// Token: 0x04003F75 RID: 16245
			public static readonly StringName _damageParticleNodes = "_damageParticleNodes";

			/// <summary>
			/// Cached name for the '_particleGravity' field.
			/// </summary>
			// Token: 0x04003F76 RID: 16246
			public static readonly StringName _particleGravity = "_particleGravity";

			/// <summary>
			/// Cached name for the '_particleSpeedScale' field.
			/// </summary>
			// Token: 0x04003F77 RID: 16247
			public static readonly StringName _particleSpeedScale = "_particleSpeedScale";

			/// <summary>
			/// Cached name for the '_particleVelocityMinMax' field.
			/// </summary>
			// Token: 0x04003F78 RID: 16248
			public static readonly StringName _particleVelocityMinMax = "_particleVelocityMinMax";

			/// <summary>
			/// Cached name for the '_sprayNodes' field.
			/// </summary>
			// Token: 0x04003F79 RID: 16249
			public static readonly StringName _sprayNodes = "_sprayNodes";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003F7A RID: 16250
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFC RID: 4092
		public class SignalName : Node.SignalName
		{
		}
	}
}
