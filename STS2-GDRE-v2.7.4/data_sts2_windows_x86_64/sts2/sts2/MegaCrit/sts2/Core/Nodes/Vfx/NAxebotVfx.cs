using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000182 RID: 386
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NAxebotVfx.cs")]
	public class NAxebotVfx : Node
	{
		// Token: 0x060012A2 RID: 4770 RVA: 0x0006F54C File Offset: 0x0006D74C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._hurtParticles1 = this._parent.GetNode<GpuParticles2D>("SparksBoneNode/HurtParticles1");
			this._hurtParticles2 = this._parent.GetNode<GpuParticles2D>("SparksBoneNode/HurtParticles2");
			this._smokeParticlesLeft = this._parent.GetNode<GpuParticles2D>("SmokeNodeLeft/SmokeParticles");
			this._smokeParticlesRight = this._parent.GetNode<GpuParticles2D>("SmokeNodeRight/SmokeParticles");
			this._hurtParticles1.OneShot = true;
			this._hurtParticles2.OneShot = true;
			this._hurtParticles1.Emitting = false;
			this._hurtParticles2.Emitting = false;
			this._smokeParticlesLeft.Emitting = false;
			this._smokeParticlesRight.Emitting = false;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0006F64C File Offset: 0x0006D84C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "start_hurt_sparks")
			{
				this.TurnOnHurt();
				return;
			}
			if (eventName == "start_death_sparks1")
			{
				this.TurnOnDeath1();
				return;
			}
			if (eventName == "start_death_sparks2")
			{
				this.TurnOnDeath2();
				return;
			}
			if (eventName == "landing_smoke_start")
			{
				this.TurnOnLandingSmoke();
				return;
			}
			if (!(eventName == "landing_smoke_end"))
			{
				return;
			}
			this.TurnOffLandingSmoke();
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0006F6D4 File Offset: 0x0006D8D4
		private void TurnOnDeath1()
		{
			this._hurtParticles1.Restart();
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0006F6E1 File Offset: 0x0006D8E1
		private void TurnOnDeath2()
		{
			this._hurtParticles2.Restart();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0006F6EE File Offset: 0x0006D8EE
		private void TurnOnHurt()
		{
			if (this._currentWeapon == 1)
			{
				this._hurtParticles1.Restart();
				this._currentWeapon = 2;
				return;
			}
			this._hurtParticles2.Restart();
			this._currentWeapon = 1;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0006F71E File Offset: 0x0006D91E
		private void TurnOnLandingSmoke()
		{
			this._smokeParticlesLeft.Restart();
			this._smokeParticlesRight.Restart();
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0006F736 File Offset: 0x0006D936
		private void TurnOffLandingSmoke()
		{
			this._smokeParticlesLeft.Emitting = false;
			this._smokeParticlesRight.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012A9 RID: 4777 RVA: 0x0006F750 File Offset: 0x0006D950
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NAxebotVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAxebotVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NAxebotVfx.MethodName.TurnOnDeath1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAxebotVfx.MethodName.TurnOnDeath2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAxebotVfx.MethodName.TurnOnHurt, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAxebotVfx.MethodName.TurnOnLandingSmoke, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAxebotVfx.MethodName.TurnOffLandingSmoke, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012AA RID: 4778 RVA: 0x0006F964 File Offset: 0x0006DB64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAxebotVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.TurnOnDeath1 && args.Count == 0)
			{
				this.TurnOnDeath1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.TurnOnDeath2 && args.Count == 0)
			{
				this.TurnOnDeath2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.TurnOnHurt && args.Count == 0)
			{
				this.TurnOnHurt();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.TurnOnLandingSmoke && args.Count == 0)
			{
				this.TurnOnLandingSmoke();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAxebotVfx.MethodName.TurnOffLandingSmoke && args.Count == 0)
			{
				this.TurnOffLandingSmoke();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060012AB RID: 4779 RVA: 0x0006FAB4 File Offset: 0x0006DCB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAxebotVfx.MethodName._Ready || (ref method) == NAxebotVfx.MethodName.OnAnimationEvent || (ref method) == NAxebotVfx.MethodName.TurnOnDeath1 || (ref method) == NAxebotVfx.MethodName.TurnOnDeath2 || (ref method) == NAxebotVfx.MethodName.TurnOnHurt || (ref method) == NAxebotVfx.MethodName.TurnOnLandingSmoke || (ref method) == NAxebotVfx.MethodName.TurnOffLandingSmoke || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012AC RID: 4780 RVA: 0x0006FB34 File Offset: 0x0006DD34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAxebotVfx.PropertyName._hurtParticles1)
			{
				this._hurtParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._hurtParticles2)
			{
				this._hurtParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._smokeParticlesLeft)
			{
				this._smokeParticlesLeft = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._smokeParticlesRight)
			{
				this._smokeParticlesRight = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._currentWeapon)
			{
				this._currentWeapon = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012AD RID: 4781 RVA: 0x0006FBEC File Offset: 0x0006DDEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAxebotVfx.PropertyName._hurtParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hurtParticles1);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._hurtParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hurtParticles2);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._smokeParticlesLeft)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticlesLeft);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._smokeParticlesRight)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticlesRight);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NAxebotVfx.PropertyName._currentWeapon)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentWeapon);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012AE RID: 4782 RVA: 0x0006FCC4 File Offset: 0x0006DEC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAxebotVfx.PropertyName._hurtParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAxebotVfx.PropertyName._hurtParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAxebotVfx.PropertyName._smokeParticlesLeft, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAxebotVfx.PropertyName._smokeParticlesRight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAxebotVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(2L, NAxebotVfx.PropertyName._currentWeapon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012AF RID: 4783 RVA: 0x0006FDA0 File Offset: 0x0006DFA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAxebotVfx.PropertyName._hurtParticles1, Variant.From<GpuParticles2D>(ref this._hurtParticles1));
			info.AddProperty(NAxebotVfx.PropertyName._hurtParticles2, Variant.From<GpuParticles2D>(ref this._hurtParticles2));
			info.AddProperty(NAxebotVfx.PropertyName._smokeParticlesLeft, Variant.From<GpuParticles2D>(ref this._smokeParticlesLeft));
			info.AddProperty(NAxebotVfx.PropertyName._smokeParticlesRight, Variant.From<GpuParticles2D>(ref this._smokeParticlesRight));
			info.AddProperty(NAxebotVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NAxebotVfx.PropertyName._currentWeapon, Variant.From<int>(ref this._currentWeapon));
		}

		/// <inheritdoc />
		// Token: 0x060012B0 RID: 4784 RVA: 0x0006FE38 File Offset: 0x0006E038
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._hurtParticles1, ref variant))
			{
				this._hurtParticles1 = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._hurtParticles2, ref variant2))
			{
				this._hurtParticles2 = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._smokeParticlesLeft, ref variant3))
			{
				this._smokeParticlesLeft = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._smokeParticlesRight, ref variant4))
			{
				this._smokeParticlesRight = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._parent, ref variant5))
			{
				this._parent = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAxebotVfx.PropertyName._currentWeapon, ref variant6))
			{
				this._currentWeapon = variant6.As<int>();
			}
		}

		// Token: 0x0400069E RID: 1694
		[Nullable(1)]
		private GpuParticles2D _hurtParticles1;

		// Token: 0x0400069F RID: 1695
		[Nullable(1)]
		private GpuParticles2D _hurtParticles2;

		// Token: 0x040006A0 RID: 1696
		[Nullable(1)]
		private GpuParticles2D _smokeParticlesLeft;

		// Token: 0x040006A1 RID: 1697
		[Nullable(1)]
		private GpuParticles2D _smokeParticlesRight;

		// Token: 0x040006A2 RID: 1698
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x040006A3 RID: 1699
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x040006A4 RID: 1700
		private int _currentWeapon = 1;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F96 RID: 3990
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E3D RID: 15933
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003E3E RID: 15934
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'TurnOnDeath1' method.
			/// </summary>
			// Token: 0x04003E3F RID: 15935
			public static readonly StringName TurnOnDeath1 = "TurnOnDeath1";

			/// <summary>
			/// Cached name for the 'TurnOnDeath2' method.
			/// </summary>
			// Token: 0x04003E40 RID: 15936
			public static readonly StringName TurnOnDeath2 = "TurnOnDeath2";

			/// <summary>
			/// Cached name for the 'TurnOnHurt' method.
			/// </summary>
			// Token: 0x04003E41 RID: 15937
			public static readonly StringName TurnOnHurt = "TurnOnHurt";

			/// <summary>
			/// Cached name for the 'TurnOnLandingSmoke' method.
			/// </summary>
			// Token: 0x04003E42 RID: 15938
			public static readonly StringName TurnOnLandingSmoke = "TurnOnLandingSmoke";

			/// <summary>
			/// Cached name for the 'TurnOffLandingSmoke' method.
			/// </summary>
			// Token: 0x04003E43 RID: 15939
			public static readonly StringName TurnOffLandingSmoke = "TurnOffLandingSmoke";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F97 RID: 3991
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_hurtParticles1' field.
			/// </summary>
			// Token: 0x04003E44 RID: 15940
			public static readonly StringName _hurtParticles1 = "_hurtParticles1";

			/// <summary>
			/// Cached name for the '_hurtParticles2' field.
			/// </summary>
			// Token: 0x04003E45 RID: 15941
			public static readonly StringName _hurtParticles2 = "_hurtParticles2";

			/// <summary>
			/// Cached name for the '_smokeParticlesLeft' field.
			/// </summary>
			// Token: 0x04003E46 RID: 15942
			public static readonly StringName _smokeParticlesLeft = "_smokeParticlesLeft";

			/// <summary>
			/// Cached name for the '_smokeParticlesRight' field.
			/// </summary>
			// Token: 0x04003E47 RID: 15943
			public static readonly StringName _smokeParticlesRight = "_smokeParticlesRight";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003E48 RID: 15944
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_currentWeapon' field.
			/// </summary>
			// Token: 0x04003E49 RID: 15945
			public static readonly StringName _currentWeapon = "_currentWeapon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F98 RID: 3992
		public class SignalName : Node.SignalName
		{
		}
	}
}
