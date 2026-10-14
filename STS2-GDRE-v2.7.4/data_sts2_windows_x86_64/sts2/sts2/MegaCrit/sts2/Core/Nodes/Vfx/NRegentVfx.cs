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
	// Token: 0x020001E5 RID: 485
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NRegentVfx.cs")]
	public class NRegentVfx : Node
	{
		// Token: 0x06001891 RID: 6289 RVA: 0x000995FC File Offset: 0x000977FC
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._deathParticlesArm = this._parent.GetNode<GpuParticles2D>("SpineArmBone/Particles");
			this._deathParticlesChest = this._parent.GetNode<GpuParticles2D>("SpineChestBone/Particles");
			this._deathParticlesBack = this._parent.GetNode<GpuParticles2D>("SpineChestBone/ParticlesBack");
			this._deathParticlesLeg = this._parent.GetNode<GpuParticles2D>("SpineLegBone/Particles");
			this._deathParticlesLegL = this._parent.GetNode<GpuParticles2D>("SpineLegBoneL/Particles");
			this._explosionParticles = this._parent.GetNode<GpuParticles2D>("Explosion");
			this._weapon = new MegaSprite(this._parent.GetNode("Weapons/WeaponAnim1"));
			this._weapon2 = new MegaSprite(this._parent.GetNode("Weapons/WeaponAnim2"));
			this.RunWhenSpineReady(this._weapon, delegate(MegaAnimationState animState)
			{
				this._weaponAnimState = animState;
			});
			this.RunWhenSpineReady(this._weapon2, delegate(MegaAnimationState animState)
			{
				this._weaponAnimState2 = animState;
			});
			this._deathParticlesArm.Emitting = false;
			this._deathParticlesChest.Emitting = false;
			this._deathParticlesBack.Emitting = false;
			this._deathParticlesLeg.Emitting = false;
			this._deathParticlesLegL.Emitting = false;
			this._explosionParticles.Emitting = false;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x000997CC File Offset: 0x000979CC
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "death_particles_start")
			{
				this.TurnOnDying();
				return;
			}
			if (eventName == "death_particles_start2")
			{
				this.TurnOnDying2();
				return;
			}
			if (eventName == "death_particles_end")
			{
				this.TurnOffDying();
				return;
			}
			if (eventName == "explode_dead")
			{
				this.Explode();
				return;
			}
			if (eventName == "explode_end")
			{
				this.DisableExplode();
				return;
			}
			if (!(eventName == "attack1"))
			{
				return;
			}
			this.Attack();
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00099868 File Offset: 0x00097A68
		private void TurnOnDying()
		{
			this._deathParticlesArm.Restart();
			this._deathParticlesLeg.Restart();
			this._deathParticlesLegL.Restart();
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0009988B File Offset: 0x00097A8B
		private void TurnOnDying2()
		{
			this._deathParticlesChest.Restart();
			this._deathParticlesBack.Restart();
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000998A3 File Offset: 0x00097AA3
		private void TurnOffDying()
		{
			this._deathParticlesArm.Emitting = false;
			this._deathParticlesChest.Emitting = false;
			this._deathParticlesBack.Emitting = false;
			this._deathParticlesLeg.Emitting = false;
			this._deathParticlesLegL.Emitting = false;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000998E1 File Offset: 0x00097AE1
		private void Explode()
		{
			this._explosionParticles.Restart();
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000998EE File Offset: 0x00097AEE
		private void DisableExplode()
		{
			this._explosionParticles.Emitting = false;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x000998FC File Offset: 0x00097AFC
		private void Attack()
		{
			if (this._curWeapon == 1)
			{
				MegaAnimationState weaponAnimState = this._weaponAnimState;
				if (weaponAnimState != null)
				{
					weaponAnimState.SetAnimation("attack", false, 0);
				}
				this._curWeapon = 2;
				return;
			}
			MegaAnimationState weaponAnimState2 = this._weaponAnimState2;
			if (weaponAnimState2 != null)
			{
				weaponAnimState2.SetAnimation("attack2", false, 0);
			}
			this._curWeapon = 1;
		}

		/// <summary>
		/// Check if we want to make sure we turn off any vfx between animations. We have to do this if the animation that
		/// is supposed to turn off the vfx is interrupted early.
		/// </summary>
		// Token: 0x06001899 RID: 6297 RVA: 0x00099951 File Offset: 0x00097B51
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "die")
			{
				this.DisableExplode();
				this.TurnOffDying();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600189A RID: 6298 RVA: 0x0009997C File Offset: 0x00097B7C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NRegentVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NRegentVfx.MethodName.TurnOnDying, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.TurnOnDying2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.TurnOffDying, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.Explode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.DisableExplode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.Attack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600189B RID: 6299 RVA: 0x00099C78 File Offset: 0x00097E78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRegentVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.TurnOnDying && args.Count == 0)
			{
				this.TurnOnDying();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.TurnOnDying2 && args.Count == 0)
			{
				this.TurnOnDying2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.TurnOffDying && args.Count == 0)
			{
				this.TurnOffDying();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.Explode && args.Count == 0)
			{
				this.Explode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.DisableExplode && args.Count == 0)
			{
				this.DisableExplode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.Attack && args.Count == 0)
			{
				this.Attack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600189C RID: 6300 RVA: 0x00099E38 File Offset: 0x00098038
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRegentVfx.MethodName._Ready || (ref method) == NRegentVfx.MethodName.OnAnimationEvent || (ref method) == NRegentVfx.MethodName.TurnOnDying || (ref method) == NRegentVfx.MethodName.TurnOnDying2 || (ref method) == NRegentVfx.MethodName.TurnOffDying || (ref method) == NRegentVfx.MethodName.Explode || (ref method) == NRegentVfx.MethodName.DisableExplode || (ref method) == NRegentVfx.MethodName.Attack || (ref method) == NRegentVfx.MethodName.OnAnimationStart || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600189D RID: 6301 RVA: 0x00099ED4 File Offset: 0x000980D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesArm)
			{
				this._deathParticlesArm = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesChest)
			{
				this._deathParticlesChest = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesBack)
			{
				this._deathParticlesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesLeg)
			{
				this._deathParticlesLeg = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesLegL)
			{
				this._deathParticlesLegL = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._explosionParticles)
			{
				this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesSmall)
			{
				this._attackParticlesSmall = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesSmall2)
			{
				this._attackParticlesSmall2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesLarge)
			{
				this._attackParticlesLarge = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._curWeapon)
			{
				this._curWeapon = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600189E RID: 6302 RVA: 0x0009A014 File Offset: 0x00098214
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesArm)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesArm);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesChest)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesChest);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesBack)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesBack);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesLeg)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesLeg);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._deathParticlesLegL)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesLegL);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._explosionParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesSmall)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesSmall);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesSmall2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesSmall2);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._attackParticlesLarge)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesLarge);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._curWeapon)
			{
				value = VariantUtils.CreateFrom<int>(ref this._curWeapon);
				return true;
			}
			if ((ref name) == NRegentVfx.PropertyName._parent)
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
		// Token: 0x0600189F RID: 6303 RVA: 0x0009A18C File Offset: 0x0009838C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRegentVfx.PropertyName._deathParticlesArm, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._deathParticlesChest, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._deathParticlesBack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._deathParticlesLeg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._deathParticlesLegL, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._explosionParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._attackParticlesSmall, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._attackParticlesSmall2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._attackParticlesLarge, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRegentVfx.PropertyName._curWeapon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018A0 RID: 6304 RVA: 0x0009A30C File Offset: 0x0009850C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRegentVfx.PropertyName._deathParticlesArm, Variant.From<GpuParticles2D>(ref this._deathParticlesArm));
			info.AddProperty(NRegentVfx.PropertyName._deathParticlesChest, Variant.From<GpuParticles2D>(ref this._deathParticlesChest));
			info.AddProperty(NRegentVfx.PropertyName._deathParticlesBack, Variant.From<GpuParticles2D>(ref this._deathParticlesBack));
			info.AddProperty(NRegentVfx.PropertyName._deathParticlesLeg, Variant.From<GpuParticles2D>(ref this._deathParticlesLeg));
			info.AddProperty(NRegentVfx.PropertyName._deathParticlesLegL, Variant.From<GpuParticles2D>(ref this._deathParticlesLegL));
			info.AddProperty(NRegentVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
			info.AddProperty(NRegentVfx.PropertyName._attackParticlesSmall, Variant.From<GpuParticles2D>(ref this._attackParticlesSmall));
			info.AddProperty(NRegentVfx.PropertyName._attackParticlesSmall2, Variant.From<GpuParticles2D>(ref this._attackParticlesSmall2));
			info.AddProperty(NRegentVfx.PropertyName._attackParticlesLarge, Variant.From<GpuParticles2D>(ref this._attackParticlesLarge));
			info.AddProperty(NRegentVfx.PropertyName._curWeapon, Variant.From<int>(ref this._curWeapon));
			info.AddProperty(NRegentVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060018A1 RID: 6305 RVA: 0x0009A414 File Offset: 0x00098614
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesArm, ref variant))
			{
				this._deathParticlesArm = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesChest, ref variant2))
			{
				this._deathParticlesChest = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesBack, ref variant3))
			{
				this._deathParticlesBack = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesLeg, ref variant4))
			{
				this._deathParticlesLeg = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesLegL, ref variant5))
			{
				this._deathParticlesLegL = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRegentVfx.PropertyName._explosionParticles, ref variant6))
			{
				this._explosionParticles = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesSmall, ref variant7))
			{
				this._attackParticlesSmall = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesSmall2, ref variant8))
			{
				this._attackParticlesSmall2 = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesLarge, ref variant9))
			{
				this._attackParticlesLarge = variant9.As<GpuParticles2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRegentVfx.PropertyName._curWeapon, ref variant10))
			{
				this._curWeapon = variant10.As<int>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRegentVfx.PropertyName._parent, ref variant11))
			{
				this._parent = variant11.As<Node2D>();
			}
		}

		// Token: 0x040008D7 RID: 2263
		private GpuParticles2D _deathParticlesArm;

		// Token: 0x040008D8 RID: 2264
		private GpuParticles2D _deathParticlesChest;

		// Token: 0x040008D9 RID: 2265
		private GpuParticles2D _deathParticlesBack;

		// Token: 0x040008DA RID: 2266
		private GpuParticles2D _deathParticlesLeg;

		// Token: 0x040008DB RID: 2267
		private GpuParticles2D _deathParticlesLegL;

		// Token: 0x040008DC RID: 2268
		private GpuParticles2D _explosionParticles;

		// Token: 0x040008DD RID: 2269
		private MegaSprite _weapon;

		// Token: 0x040008DE RID: 2270
		private MegaSprite _weapon2;

		// Token: 0x040008DF RID: 2271
		private GpuParticles2D _attackParticlesSmall;

		// Token: 0x040008E0 RID: 2272
		private GpuParticles2D _attackParticlesSmall2;

		// Token: 0x040008E1 RID: 2273
		private GpuParticles2D _attackParticlesLarge;

		// Token: 0x040008E2 RID: 2274
		[Nullable(2)]
		private MegaAnimationState _weaponAnimState;

		// Token: 0x040008E3 RID: 2275
		[Nullable(2)]
		private MegaAnimationState _weaponAnimState2;

		// Token: 0x040008E4 RID: 2276
		private int _curWeapon = 1;

		// Token: 0x040008E5 RID: 2277
		private Node2D _parent;

		// Token: 0x040008E6 RID: 2278
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001104 RID: 4356
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004274 RID: 17012
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004275 RID: 17013
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'TurnOnDying' method.
			/// </summary>
			// Token: 0x04004276 RID: 17014
			public static readonly StringName TurnOnDying = "TurnOnDying";

			/// <summary>
			/// Cached name for the 'TurnOnDying2' method.
			/// </summary>
			// Token: 0x04004277 RID: 17015
			public static readonly StringName TurnOnDying2 = "TurnOnDying2";

			/// <summary>
			/// Cached name for the 'TurnOffDying' method.
			/// </summary>
			// Token: 0x04004278 RID: 17016
			public static readonly StringName TurnOffDying = "TurnOffDying";

			/// <summary>
			/// Cached name for the 'Explode' method.
			/// </summary>
			// Token: 0x04004279 RID: 17017
			public static readonly StringName Explode = "Explode";

			/// <summary>
			/// Cached name for the 'DisableExplode' method.
			/// </summary>
			// Token: 0x0400427A RID: 17018
			public static readonly StringName DisableExplode = "DisableExplode";

			/// <summary>
			/// Cached name for the 'Attack' method.
			/// </summary>
			// Token: 0x0400427B RID: 17019
			public static readonly StringName Attack = "Attack";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x0400427C RID: 17020
			public static readonly StringName OnAnimationStart = "OnAnimationStart";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001105 RID: 4357
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_deathParticlesArm' field.
			/// </summary>
			// Token: 0x0400427D RID: 17021
			public static readonly StringName _deathParticlesArm = "_deathParticlesArm";

			/// <summary>
			/// Cached name for the '_deathParticlesChest' field.
			/// </summary>
			// Token: 0x0400427E RID: 17022
			public static readonly StringName _deathParticlesChest = "_deathParticlesChest";

			/// <summary>
			/// Cached name for the '_deathParticlesBack' field.
			/// </summary>
			// Token: 0x0400427F RID: 17023
			public static readonly StringName _deathParticlesBack = "_deathParticlesBack";

			/// <summary>
			/// Cached name for the '_deathParticlesLeg' field.
			/// </summary>
			// Token: 0x04004280 RID: 17024
			public static readonly StringName _deathParticlesLeg = "_deathParticlesLeg";

			/// <summary>
			/// Cached name for the '_deathParticlesLegL' field.
			/// </summary>
			// Token: 0x04004281 RID: 17025
			public static readonly StringName _deathParticlesLegL = "_deathParticlesLegL";

			/// <summary>
			/// Cached name for the '_explosionParticles' field.
			/// </summary>
			// Token: 0x04004282 RID: 17026
			public static readonly StringName _explosionParticles = "_explosionParticles";

			/// <summary>
			/// Cached name for the '_attackParticlesSmall' field.
			/// </summary>
			// Token: 0x04004283 RID: 17027
			public static readonly StringName _attackParticlesSmall = "_attackParticlesSmall";

			/// <summary>
			/// Cached name for the '_attackParticlesSmall2' field.
			/// </summary>
			// Token: 0x04004284 RID: 17028
			public static readonly StringName _attackParticlesSmall2 = "_attackParticlesSmall2";

			/// <summary>
			/// Cached name for the '_attackParticlesLarge' field.
			/// </summary>
			// Token: 0x04004285 RID: 17029
			public static readonly StringName _attackParticlesLarge = "_attackParticlesLarge";

			/// <summary>
			/// Cached name for the '_curWeapon' field.
			/// </summary>
			// Token: 0x04004286 RID: 17030
			public static readonly StringName _curWeapon = "_curWeapon";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004287 RID: 17031
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001106 RID: 4358
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
