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
	// Token: 0x02000180 RID: 384
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NAmalgamVfx.cs")]
	public class NAmalgamVfx : Node
	{
		// Token: 0x06001282 RID: 4738 RVA: 0x0006DC88 File Offset: 0x0006BE88
		public override void _Ready()
		{
			this._parent = base.GetParent();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._deathBodyParticles = this._parent.GetNode<CpuParticles2D>("CPUDeathParticles");
			this._deathBodyParticles.Emitting = false;
			this._deathBodyParticles.OneShot = true;
			this._laserBaseParticles = this._parent.GetNode<GpuParticles2D>("laserBaseBone/laserBaseParticles");
			this._laserBaseParticles.Emitting = false;
			this._torch1Node = this._parent.GetNode<Node2D>("torch1Slot/fire1_small_green");
			this._torch1Node.Visible = true;
			this._torch2Node = this._parent.GetNode<Node2D>("torch2Slot/fire2_small_green");
			this._torch2Node.Visible = true;
			this._torch3Node = this._parent.GetNode<Node2D>("torch3Slot/fire3_small_green");
			this._torch3Node.Visible = true;
			this._hitParticles1 = this._parent.GetNode<GpuParticles2D>("torch1UnscaledBone/hitParticles");
			this._hitParticles1.Emitting = false;
			this._hitParticles1.OneShot = true;
			this._hitParticles2 = this._parent.GetNode<GpuParticles2D>("torch2UnscaledBone/hitParticles");
			this._hitParticles2.Emitting = false;
			this._hitParticles2.OneShot = true;
			this._hitParticles3 = this._parent.GetNode<GpuParticles2D>("torch3UnscaledBone/hitParticles");
			this._hitParticles3.Emitting = false;
			this._hitParticles3.OneShot = true;
			this._constantSparks1 = this._parent.GetNode<GpuParticles2D>("torch1Slot/constantParticles");
			this._constantSparks2 = this._parent.GetNode<GpuParticles2D>("torch2Slot/constantParticles");
			this._constantSparks3 = this._parent.GetNode<GpuParticles2D>("torch3Slot/constantParticles");
			this._hitFxParticles.Visible = false;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0006DE9C File Offset: 0x0006C09C
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 4:
					switch (eventName[3])
					{
					case '1':
						if (!(eventName == "hit1"))
						{
							return;
						}
						this.PlayHit1();
						return;
					case '2':
						if (!(eventName == "hit2"))
						{
							return;
						}
						this.PlayHit2();
						return;
					case '3':
						if (!(eventName == "hit3"))
						{
							return;
						}
						this.PlayHit3();
						return;
					default:
						return;
					}
					break;
				case 5:
				case 6:
				case 8:
				case 9:
				case 12:
					break;
				case 7:
					if (!(eventName == "go_poof"))
					{
						return;
					}
					this.PoofToDeath();
					return;
				case 10:
					if (!(eventName == "torches_on"))
					{
						return;
					}
					this.RestartTorches();
					return;
				case 11:
					if (!(eventName == "torches_out"))
					{
						return;
					}
					this.KillTorches();
					return;
				case 13:
					if (!(eventName == "laser_hit_off"))
					{
						return;
					}
					this.PlayLaserHit(false);
					break;
				case 14:
				{
					char c = eventName[6];
					if (c != 'b')
					{
						if (c != 'h')
						{
							return;
						}
						if (!(eventName == "laser_hit_fire"))
						{
							return;
						}
						this.PlayLaserHit(true);
						return;
					}
					else
					{
						if (!(eventName == "laser_base_off"))
						{
							return;
						}
						this.PlayLaserBase(false);
						return;
					}
					break;
				}
				case 15:
					if (!(eventName == "laser_base_fire"))
					{
						return;
					}
					this.PlayLaserBase(true);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0006E00C File Offset: 0x0006C20C
		private void PoofToDeath()
		{
			this._deathBodyParticles.Restart();
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0006E01C File Offset: 0x0006C21C
		private void RestartTorches()
		{
			this._torch1Node.Visible = true;
			this._torch2Node.Visible = true;
			this._torch3Node.Visible = true;
			this._constantSparks1.Emitting = true;
			this._constantSparks2.Emitting = true;
			this._constantSparks3.Emitting = true;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0006E074 File Offset: 0x0006C274
		private void KillTorches()
		{
			this._torch1Node.Visible = false;
			this._torch2Node.Visible = false;
			this._torch3Node.Visible = false;
			this._constantSparks1.Emitting = false;
			this._constantSparks2.Emitting = false;
			this._constantSparks3.Emitting = false;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0006E0C9 File Offset: 0x0006C2C9
		private void PlayHit1()
		{
			this._hitParticles1.Restart();
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0006E0D6 File Offset: 0x0006C2D6
		private void PlayHit2()
		{
			this._hitParticles2.Restart();
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0006E0E3 File Offset: 0x0006C2E3
		private void PlayHit3()
		{
			this._hitParticles3.Restart();
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0006E0F0 File Offset: 0x0006C2F0
		private void PlayLaserBase(bool starting)
		{
			if (starting)
			{
				this._laserBaseParticles.Visible = true;
				this._laserBaseParticles.Restart();
				return;
			}
			this._laserBaseParticles.Emitting = false;
			this._laserBaseParticles.Visible = false;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0006E128 File Offset: 0x0006C328
		private void PlayLaserHit(bool starting)
		{
			if (starting)
			{
				this._hitFxParticles.GlobalPosition = this._hitBoneNode.GlobalPosition;
				this._hitFxParticles.Visible = true;
				this._hitFxParticles.Restart();
				return;
			}
			this._hitFxParticles.Emitting = false;
			this._hitFxParticles.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600128C RID: 4748 RVA: 0x0006E180 File Offset: 0x0006C380
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NAmalgamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NAmalgamVfx.MethodName.PoofToDeath, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.RestartTorches, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.KillTorches, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.PlayHit1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.PlayHit2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.PlayHit3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmalgamVfx.MethodName.PlayLaserBase, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "starting", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAmalgamVfx.MethodName.PlayLaserHit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "starting", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600128D RID: 4749 RVA: 0x0006E46C File Offset: 0x0006C66C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAmalgamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PoofToDeath && args.Count == 0)
			{
				this.PoofToDeath();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.RestartTorches && args.Count == 0)
			{
				this.RestartTorches();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.KillTorches && args.Count == 0)
			{
				this.KillTorches();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PlayHit1 && args.Count == 0)
			{
				this.PlayHit1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PlayHit2 && args.Count == 0)
			{
				this.PlayHit2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PlayHit3 && args.Count == 0)
			{
				this.PlayHit3();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PlayLaserBase && args.Count == 1)
			{
				this.PlayLaserBase(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmalgamVfx.MethodName.PlayLaserHit && args.Count == 1)
			{
				this.PlayLaserHit(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600128E RID: 4750 RVA: 0x0006E648 File Offset: 0x0006C848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAmalgamVfx.MethodName._Ready || (ref method) == NAmalgamVfx.MethodName.OnAnimationEvent || (ref method) == NAmalgamVfx.MethodName.PoofToDeath || (ref method) == NAmalgamVfx.MethodName.RestartTorches || (ref method) == NAmalgamVfx.MethodName.KillTorches || (ref method) == NAmalgamVfx.MethodName.PlayHit1 || (ref method) == NAmalgamVfx.MethodName.PlayHit2 || (ref method) == NAmalgamVfx.MethodName.PlayHit3 || (ref method) == NAmalgamVfx.MethodName.PlayLaserBase || (ref method) == NAmalgamVfx.MethodName.PlayLaserHit || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600128F RID: 4751 RVA: 0x0006E6F4 File Offset: 0x0006C8F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAmalgamVfx.PropertyName._hitFxParticles)
			{
				this._hitFxParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitBoneNode)
			{
				this._hitBoneNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._deathBodyParticles)
			{
				this._deathBodyParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._laserBaseParticles)
			{
				this._laserBaseParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles1)
			{
				this._hitParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles2)
			{
				this._hitParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles3)
			{
				this._hitParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks1)
			{
				this._constantSparks1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks2)
			{
				this._constantSparks2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks3)
			{
				this._constantSparks3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch1Node)
			{
				this._torch1Node = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch2Node)
			{
				this._torch2Node = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch3Node)
			{
				this._torch3Node = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001290 RID: 4752 RVA: 0x0006E884 File Offset: 0x0006CA84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAmalgamVfx.PropertyName._hitFxParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitFxParticles);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitBoneNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._hitBoneNode);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._deathBodyParticles)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._deathBodyParticles);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._laserBaseParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._laserBaseParticles);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles1);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles2);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._hitParticles3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles3);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks1);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks2);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._constantSparks3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks3);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch1Node)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._torch1Node);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch2Node)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._torch2Node);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._torch3Node)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._torch3Node);
				return true;
			}
			if ((ref name) == NAmalgamVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001291 RID: 4753 RVA: 0x0006EA5C File Offset: 0x0006CC5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._hitFxParticles, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._hitBoneNode, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._deathBodyParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._laserBaseParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._hitParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._hitParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._hitParticles3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._constantSparks1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._constantSparks2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._constantSparks3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._torch1Node, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._torch2Node, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._torch3Node, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAmalgamVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001292 RID: 4754 RVA: 0x0006EC40 File Offset: 0x0006CE40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAmalgamVfx.PropertyName._hitFxParticles, Variant.From<GpuParticles2D>(ref this._hitFxParticles));
			info.AddProperty(NAmalgamVfx.PropertyName._hitBoneNode, Variant.From<Node2D>(ref this._hitBoneNode));
			info.AddProperty(NAmalgamVfx.PropertyName._deathBodyParticles, Variant.From<CpuParticles2D>(ref this._deathBodyParticles));
			info.AddProperty(NAmalgamVfx.PropertyName._laserBaseParticles, Variant.From<GpuParticles2D>(ref this._laserBaseParticles));
			info.AddProperty(NAmalgamVfx.PropertyName._hitParticles1, Variant.From<GpuParticles2D>(ref this._hitParticles1));
			info.AddProperty(NAmalgamVfx.PropertyName._hitParticles2, Variant.From<GpuParticles2D>(ref this._hitParticles2));
			info.AddProperty(NAmalgamVfx.PropertyName._hitParticles3, Variant.From<GpuParticles2D>(ref this._hitParticles3));
			info.AddProperty(NAmalgamVfx.PropertyName._constantSparks1, Variant.From<GpuParticles2D>(ref this._constantSparks1));
			info.AddProperty(NAmalgamVfx.PropertyName._constantSparks2, Variant.From<GpuParticles2D>(ref this._constantSparks2));
			info.AddProperty(NAmalgamVfx.PropertyName._constantSparks3, Variant.From<GpuParticles2D>(ref this._constantSparks3));
			info.AddProperty(NAmalgamVfx.PropertyName._torch1Node, Variant.From<Node2D>(ref this._torch1Node));
			info.AddProperty(NAmalgamVfx.PropertyName._torch2Node, Variant.From<Node2D>(ref this._torch2Node));
			info.AddProperty(NAmalgamVfx.PropertyName._torch3Node, Variant.From<Node2D>(ref this._torch3Node));
			info.AddProperty(NAmalgamVfx.PropertyName._parent, Variant.From<Node>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001293 RID: 4755 RVA: 0x0006ED88 File Offset: 0x0006CF88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitFxParticles, ref variant))
			{
				this._hitFxParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitBoneNode, ref variant2))
			{
				this._hitBoneNode = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._deathBodyParticles, ref variant3))
			{
				this._deathBodyParticles = variant3.As<CpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._laserBaseParticles, ref variant4))
			{
				this._laserBaseParticles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles1, ref variant5))
			{
				this._hitParticles1 = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles2, ref variant6))
			{
				this._hitParticles2 = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles3, ref variant7))
			{
				this._hitParticles3 = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks1, ref variant8))
			{
				this._constantSparks1 = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks2, ref variant9))
			{
				this._constantSparks2 = variant9.As<GpuParticles2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks3, ref variant10))
			{
				this._constantSparks3 = variant10.As<GpuParticles2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch1Node, ref variant11))
			{
				this._torch1Node = variant11.As<Node2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch2Node, ref variant12))
			{
				this._torch2Node = variant12.As<Node2D>();
			}
			Variant variant13;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch3Node, ref variant13))
			{
				this._torch3Node = variant13.As<Node2D>();
			}
			Variant variant14;
			if (info.TryGetProperty(NAmalgamVfx.PropertyName._parent, ref variant14))
			{
				this._parent = variant14.As<Node>();
			}
		}

		// Token: 0x0400068B RID: 1675
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _hitFxParticles;

		// Token: 0x0400068C RID: 1676
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _hitBoneNode;

		// Token: 0x0400068D RID: 1677
		private CpuParticles2D _deathBodyParticles;

		// Token: 0x0400068E RID: 1678
		private GpuParticles2D _laserBaseParticles;

		// Token: 0x0400068F RID: 1679
		private GpuParticles2D _hitParticles1;

		// Token: 0x04000690 RID: 1680
		private GpuParticles2D _hitParticles2;

		// Token: 0x04000691 RID: 1681
		private GpuParticles2D _hitParticles3;

		// Token: 0x04000692 RID: 1682
		private GpuParticles2D _constantSparks1;

		// Token: 0x04000693 RID: 1683
		private GpuParticles2D _constantSparks2;

		// Token: 0x04000694 RID: 1684
		private GpuParticles2D _constantSparks3;

		// Token: 0x04000695 RID: 1685
		private Node2D _torch1Node;

		// Token: 0x04000696 RID: 1686
		private Node2D _torch2Node;

		// Token: 0x04000697 RID: 1687
		private Node2D _torch3Node;

		// Token: 0x04000698 RID: 1688
		private Node _parent;

		// Token: 0x04000699 RID: 1689
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F8F RID: 3983
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E1C RID: 15900
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003E1D RID: 15901
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'PoofToDeath' method.
			/// </summary>
			// Token: 0x04003E1E RID: 15902
			public static readonly StringName PoofToDeath = "PoofToDeath";

			/// <summary>
			/// Cached name for the 'RestartTorches' method.
			/// </summary>
			// Token: 0x04003E1F RID: 15903
			public static readonly StringName RestartTorches = "RestartTorches";

			/// <summary>
			/// Cached name for the 'KillTorches' method.
			/// </summary>
			// Token: 0x04003E20 RID: 15904
			public static readonly StringName KillTorches = "KillTorches";

			/// <summary>
			/// Cached name for the 'PlayHit1' method.
			/// </summary>
			// Token: 0x04003E21 RID: 15905
			public static readonly StringName PlayHit1 = "PlayHit1";

			/// <summary>
			/// Cached name for the 'PlayHit2' method.
			/// </summary>
			// Token: 0x04003E22 RID: 15906
			public static readonly StringName PlayHit2 = "PlayHit2";

			/// <summary>
			/// Cached name for the 'PlayHit3' method.
			/// </summary>
			// Token: 0x04003E23 RID: 15907
			public static readonly StringName PlayHit3 = "PlayHit3";

			/// <summary>
			/// Cached name for the 'PlayLaserBase' method.
			/// </summary>
			// Token: 0x04003E24 RID: 15908
			public static readonly StringName PlayLaserBase = "PlayLaserBase";

			/// <summary>
			/// Cached name for the 'PlayLaserHit' method.
			/// </summary>
			// Token: 0x04003E25 RID: 15909
			public static readonly StringName PlayLaserHit = "PlayLaserHit";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F90 RID: 3984
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_hitFxParticles' field.
			/// </summary>
			// Token: 0x04003E26 RID: 15910
			public static readonly StringName _hitFxParticles = "_hitFxParticles";

			/// <summary>
			/// Cached name for the '_hitBoneNode' field.
			/// </summary>
			// Token: 0x04003E27 RID: 15911
			public static readonly StringName _hitBoneNode = "_hitBoneNode";

			/// <summary>
			/// Cached name for the '_deathBodyParticles' field.
			/// </summary>
			// Token: 0x04003E28 RID: 15912
			public static readonly StringName _deathBodyParticles = "_deathBodyParticles";

			/// <summary>
			/// Cached name for the '_laserBaseParticles' field.
			/// </summary>
			// Token: 0x04003E29 RID: 15913
			public static readonly StringName _laserBaseParticles = "_laserBaseParticles";

			/// <summary>
			/// Cached name for the '_hitParticles1' field.
			/// </summary>
			// Token: 0x04003E2A RID: 15914
			public static readonly StringName _hitParticles1 = "_hitParticles1";

			/// <summary>
			/// Cached name for the '_hitParticles2' field.
			/// </summary>
			// Token: 0x04003E2B RID: 15915
			public static readonly StringName _hitParticles2 = "_hitParticles2";

			/// <summary>
			/// Cached name for the '_hitParticles3' field.
			/// </summary>
			// Token: 0x04003E2C RID: 15916
			public static readonly StringName _hitParticles3 = "_hitParticles3";

			/// <summary>
			/// Cached name for the '_constantSparks1' field.
			/// </summary>
			// Token: 0x04003E2D RID: 15917
			public static readonly StringName _constantSparks1 = "_constantSparks1";

			/// <summary>
			/// Cached name for the '_constantSparks2' field.
			/// </summary>
			// Token: 0x04003E2E RID: 15918
			public static readonly StringName _constantSparks2 = "_constantSparks2";

			/// <summary>
			/// Cached name for the '_constantSparks3' field.
			/// </summary>
			// Token: 0x04003E2F RID: 15919
			public static readonly StringName _constantSparks3 = "_constantSparks3";

			/// <summary>
			/// Cached name for the '_torch1Node' field.
			/// </summary>
			// Token: 0x04003E30 RID: 15920
			public static readonly StringName _torch1Node = "_torch1Node";

			/// <summary>
			/// Cached name for the '_torch2Node' field.
			/// </summary>
			// Token: 0x04003E31 RID: 15921
			public static readonly StringName _torch2Node = "_torch2Node";

			/// <summary>
			/// Cached name for the '_torch3Node' field.
			/// </summary>
			// Token: 0x04003E32 RID: 15922
			public static readonly StringName _torch3Node = "_torch3Node";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003E33 RID: 15923
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F91 RID: 3985
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
