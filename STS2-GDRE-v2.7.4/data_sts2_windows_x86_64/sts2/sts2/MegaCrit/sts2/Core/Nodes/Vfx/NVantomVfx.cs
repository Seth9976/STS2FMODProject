using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200020A RID: 522
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NVantomVfx.cs")]
	public class NVantomVfx : Node
	{
		// Token: 0x06001B01 RID: 6913 RVA: 0x000ADF9C File Offset: 0x000AC19C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._tailShaderMat = new MegaSlotNode(this._parent.GetNode("TailSlotNode")).GetNormalMaterial() as ShaderMaterial;
			this._sprayParticles = this._parent.GetNode<GpuParticles2D>("SprayBoneNode/SprayParticles");
			this._deathSprayParticles = this._parent.GetNode<GpuParticles2D>("DeathSpraySlotNode/DeathSprayParticles");
			this._deathSprayParticlesBack = this._parent.GetNode<GpuParticles2D>("DeathSprayBackSlotNode/DeathSprayParticlesBack");
			this._deathExplosionParticles = this._parent.GetNode<GpuParticles2D>("DeathExplosionSlotNode/DeathExplosionParticles");
			ShaderMaterial tailShaderMat = this._tailShaderMat;
			if (tailShaderMat != null)
			{
				tailShaderMat.SetShaderParameter(NVantomVfx._step, -0.1f);
			}
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("idle_loop", true, 0);
				animState.SetAnimation("_tracks/charged_0", true, 1);
			});
			this._sprayParticles.Emitting = false;
			this._deathSprayParticles.Emitting = false;
			this._deathSprayParticlesBack.Emitting = false;
			this._deathExplosionParticles.Emitting = false;
			this._deathExplosionParticles.OneShot = true;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000AE128 File Offset: 0x000AC328
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "dissolve_tail")
			{
				this.DissolveTail();
				return;
			}
			if (eventName == "spray_on")
			{
				this.StartSpray();
				return;
			}
			if (eventName == "spray_off")
			{
				this.EndSpray();
				return;
			}
			if (eventName == "death_spray_on")
			{
				this.StartDeathSpray();
				return;
			}
			if (eventName == "death_spray_off")
			{
				this.EndDeathSpray();
				return;
			}
			if (!(eventName == "death_explosion"))
			{
				return;
			}
			this.DeathExplode();
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000AE1C4 File Offset: 0x000AC3C4
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "die")
			{
				ShaderMaterial tailShaderMat = this._tailShaderMat;
				if (tailShaderMat == null)
				{
					return;
				}
				tailShaderMat.SetShaderParameter(NVantomVfx._step, 0f);
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x000AE204 File Offset: 0x000AC404
		private void DissolveTail()
		{
			if (this._tailShaderMat != null)
			{
				Tween tween = base.CreateTween();
				tween.SetEase(0L);
				tween.SetTrans(4L);
				tween.TweenProperty(this._tailShaderMat, "shader_parameter/step", 1f, 1.0);
				tween.TweenCallback(Callable.From(delegate
				{
					this._animController.GetAnimationState().SetAnimation("_tracks/charge_up_1", false, 1);
					this._animController.GetAnimationState().AddAnimation("_tracks/charged_1", 0f, true, 1);
				}));
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000AE274 File Offset: 0x000AC474
		private void StartSpray()
		{
			this._sprayParticles.Emitting = true;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000AE282 File Offset: 0x000AC482
		private void EndSpray()
		{
			this._sprayParticles.Emitting = false;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000AE290 File Offset: 0x000AC490
		private void StartDeathSpray()
		{
			this._deathSprayParticles.Emitting = true;
			this._deathSprayParticlesBack.Emitting = true;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000AE2AA File Offset: 0x000AC4AA
		private void EndDeathSpray()
		{
			this._deathSprayParticles.Emitting = false;
			this._deathSprayParticlesBack.Emitting = false;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000AE2C4 File Offset: 0x000AC4C4
		private void DeathExplode()
		{
			this._deathExplosionParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B0A RID: 6922 RVA: 0x000AE2D4 File Offset: 0x000AC4D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NVantomVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NVantomVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NVantomVfx.MethodName.DissolveTail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.StartSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.EndSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.StartDeathSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.EndDeathSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVantomVfx.MethodName.DeathExplode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B0B RID: 6923 RVA: 0x000AE5D0 File Offset: 0x000AC7D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVantomVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.DissolveTail && args.Count == 0)
			{
				this.DissolveTail();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.StartSpray && args.Count == 0)
			{
				this.StartSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.EndSpray && args.Count == 0)
			{
				this.EndSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.StartDeathSpray && args.Count == 0)
			{
				this.StartDeathSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.EndDeathSpray && args.Count == 0)
			{
				this.EndDeathSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVantomVfx.MethodName.DeathExplode && args.Count == 0)
			{
				this.DeathExplode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B0C RID: 6924 RVA: 0x000AE790 File Offset: 0x000AC990
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVantomVfx.MethodName._Ready || (ref method) == NVantomVfx.MethodName.OnAnimationEvent || (ref method) == NVantomVfx.MethodName.OnAnimationStart || (ref method) == NVantomVfx.MethodName.DissolveTail || (ref method) == NVantomVfx.MethodName.StartSpray || (ref method) == NVantomVfx.MethodName.EndSpray || (ref method) == NVantomVfx.MethodName.StartDeathSpray || (ref method) == NVantomVfx.MethodName.EndDeathSpray || (ref method) == NVantomVfx.MethodName.DeathExplode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B0D RID: 6925 RVA: 0x000AE82C File Offset: 0x000ACA2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVantomVfx.PropertyName._tailShaderMat)
			{
				this._tailShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._sprayParticles)
			{
				this._sprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathSprayParticles)
			{
				this._deathSprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathSprayParticlesBack)
			{
				this._deathSprayParticlesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathExplosionParticles)
			{
				this._deathExplosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B0E RID: 6926 RVA: 0x000AE8E4 File Offset: 0x000ACAE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVantomVfx.PropertyName._tailShaderMat)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._tailShaderMat);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._sprayParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sprayParticles);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathSprayParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathSprayParticles);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathSprayParticlesBack)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathSprayParticlesBack);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._deathExplosionParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathExplosionParticles);
				return true;
			}
			if ((ref name) == NVantomVfx.PropertyName._parent)
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
		// Token: 0x06001B0F RID: 6927 RVA: 0x000AE9BC File Offset: 0x000ACBBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NVantomVfx.PropertyName._tailShaderMat, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVantomVfx.PropertyName._sprayParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVantomVfx.PropertyName._deathSprayParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVantomVfx.PropertyName._deathSprayParticlesBack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVantomVfx.PropertyName._deathExplosionParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVantomVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B10 RID: 6928 RVA: 0x000AEA98 File Offset: 0x000ACC98
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NVantomVfx.PropertyName._tailShaderMat, Variant.From<ShaderMaterial>(ref this._tailShaderMat));
			info.AddProperty(NVantomVfx.PropertyName._sprayParticles, Variant.From<GpuParticles2D>(ref this._sprayParticles));
			info.AddProperty(NVantomVfx.PropertyName._deathSprayParticles, Variant.From<GpuParticles2D>(ref this._deathSprayParticles));
			info.AddProperty(NVantomVfx.PropertyName._deathSprayParticlesBack, Variant.From<GpuParticles2D>(ref this._deathSprayParticlesBack));
			info.AddProperty(NVantomVfx.PropertyName._deathExplosionParticles, Variant.From<GpuParticles2D>(ref this._deathExplosionParticles));
			info.AddProperty(NVantomVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001B11 RID: 6929 RVA: 0x000AEB30 File Offset: 0x000ACD30
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVantomVfx.PropertyName._tailShaderMat, ref variant))
			{
				this._tailShaderMat = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NVantomVfx.PropertyName._sprayParticles, ref variant2))
			{
				this._sprayParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NVantomVfx.PropertyName._deathSprayParticles, ref variant3))
			{
				this._deathSprayParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NVantomVfx.PropertyName._deathSprayParticlesBack, ref variant4))
			{
				this._deathSprayParticlesBack = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NVantomVfx.PropertyName._deathExplosionParticles, ref variant5))
			{
				this._deathExplosionParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NVantomVfx.PropertyName._parent, ref variant6))
			{
				this._parent = variant6.As<Node2D>();
			}
		}

		// Token: 0x04000A0C RID: 2572
		private static readonly StringName _step = new StringName("step");

		// Token: 0x04000A0D RID: 2573
		[Nullable(2)]
		private ShaderMaterial _tailShaderMat;

		// Token: 0x04000A0E RID: 2574
		private GpuParticles2D _sprayParticles;

		// Token: 0x04000A0F RID: 2575
		private GpuParticles2D _deathSprayParticles;

		// Token: 0x04000A10 RID: 2576
		private GpuParticles2D _deathSprayParticlesBack;

		// Token: 0x04000A11 RID: 2577
		private GpuParticles2D _deathExplosionParticles;

		// Token: 0x04000A12 RID: 2578
		private Node2D _parent;

		// Token: 0x04000A13 RID: 2579
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001194 RID: 4500
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004481 RID: 17537
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004482 RID: 17538
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004483 RID: 17539
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'DissolveTail' method.
			/// </summary>
			// Token: 0x04004484 RID: 17540
			public static readonly StringName DissolveTail = "DissolveTail";

			/// <summary>
			/// Cached name for the 'StartSpray' method.
			/// </summary>
			// Token: 0x04004485 RID: 17541
			public static readonly StringName StartSpray = "StartSpray";

			/// <summary>
			/// Cached name for the 'EndSpray' method.
			/// </summary>
			// Token: 0x04004486 RID: 17542
			public static readonly StringName EndSpray = "EndSpray";

			/// <summary>
			/// Cached name for the 'StartDeathSpray' method.
			/// </summary>
			// Token: 0x04004487 RID: 17543
			public static readonly StringName StartDeathSpray = "StartDeathSpray";

			/// <summary>
			/// Cached name for the 'EndDeathSpray' method.
			/// </summary>
			// Token: 0x04004488 RID: 17544
			public static readonly StringName EndDeathSpray = "EndDeathSpray";

			/// <summary>
			/// Cached name for the 'DeathExplode' method.
			/// </summary>
			// Token: 0x04004489 RID: 17545
			public static readonly StringName DeathExplode = "DeathExplode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001195 RID: 4501
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tailShaderMat' field.
			/// </summary>
			// Token: 0x0400448A RID: 17546
			public static readonly StringName _tailShaderMat = "_tailShaderMat";

			/// <summary>
			/// Cached name for the '_sprayParticles' field.
			/// </summary>
			// Token: 0x0400448B RID: 17547
			public static readonly StringName _sprayParticles = "_sprayParticles";

			/// <summary>
			/// Cached name for the '_deathSprayParticles' field.
			/// </summary>
			// Token: 0x0400448C RID: 17548
			public static readonly StringName _deathSprayParticles = "_deathSprayParticles";

			/// <summary>
			/// Cached name for the '_deathSprayParticlesBack' field.
			/// </summary>
			// Token: 0x0400448D RID: 17549
			public static readonly StringName _deathSprayParticlesBack = "_deathSprayParticlesBack";

			/// <summary>
			/// Cached name for the '_deathExplosionParticles' field.
			/// </summary>
			// Token: 0x0400448E RID: 17550
			public static readonly StringName _deathExplosionParticles = "_deathExplosionParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x0400448F RID: 17551
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001196 RID: 4502
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
