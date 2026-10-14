using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001C3 RID: 451
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NKaiserCrabBossVfx.cs")]
	public class NKaiserCrabBossVfx : Node
	{
		// Token: 0x06001688 RID: 5768 RVA: 0x0008A0C4 File Offset: 0x000882C4
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._regenSplatParticles = this._parent.GetNode<GpuParticles2D>("RegenSplatSlot/RegenSplatParticles");
			this._plowChunkParticles = this._parent.GetNode<GpuParticles2D>("PlowChunkSlot/PlowChunkParticles");
			this._steamParticles1 = this._parent.GetNode<GpuParticles2D>("RocketSlot/SteamParticles1");
			this._steamParticles2 = this._parent.GetNode<GpuParticles2D>("RocketSlot/SteamParticles2");
			this._steamParticles3 = this._parent.GetNode<GpuParticles2D>("RocketSlot/SteamParticles3");
			this._sparkParticles = this._parent.GetNode<GpuParticles2D>("RocketSlot/SparkParticles");
			this._smokeParticles = this._parent.GetNode<GpuParticles2D>("RocketSlot/SmokeParticles");
			this._leftArmExplosionPosition = this._parent.GetNode<Node2D>("%LeftArmExplosionPosition");
			this._spittleParticles = this._parent.GetNode<GpuParticles2D>("SpittleSlot/SpittleParticles");
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._megaSprite.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._regenSplatParticles.Emitting = false;
			this._plowChunkParticles.Emitting = false;
			this._steamParticles1.Emitting = false;
			this._steamParticles2.Emitting = false;
			this._steamParticles3.Emitting = false;
			this._spittleParticles.Emitting = false;
			this._sparkParticles.Emitting = false;
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x0008A274 File Offset: 0x00088474
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 14:
				{
					char c = eventName[0];
					if (c != 'c')
					{
						if (c != 'd')
						{
							return;
						}
						if (!(eventName == "death_spit_end"))
						{
							return;
						}
						this.OnDeathSpitEnd();
						return;
					}
					else
					{
						if (!(eventName == "claw_explode_l"))
						{
							return;
						}
						this.OnClawLExplode();
					}
					break;
				}
				case 15:
					if (!(eventName == "plow_chunks_end"))
					{
						return;
					}
					this.OnPlowChunksEnd();
					return;
				case 16:
				{
					char c = eventName[0];
					if (c != 'c')
					{
						if (c != 'd')
						{
							if (c != 'r')
							{
								return;
							}
							if (!(eventName == "regen_splats_end"))
							{
								return;
							}
							this.OnRegenSplatsEnd();
							return;
						}
						else
						{
							if (!(eventName == "death_spit_start"))
							{
								return;
							}
							this.OnDeathSpitStart();
							return;
						}
					}
					else
					{
						if (!(eventName == "charge_steam_end"))
						{
							return;
						}
						this.OnChargeSteamEnd();
						return;
					}
					break;
				}
				case 17:
				{
					char c = eventName[0];
					if (c != 'p')
					{
						if (c != 'r')
						{
							return;
						}
						if (!(eventName == "rocket_thrust_end"))
						{
							return;
						}
						this.OnRocketThrustEnd();
						return;
					}
					else
					{
						if (!(eventName == "plow_chunks_start"))
						{
							return;
						}
						this.OnPlowChunksStart();
						return;
					}
					break;
				}
				case 18:
				{
					char c = eventName[0];
					if (c != 'c')
					{
						if (c != 'r')
						{
							return;
						}
						if (!(eventName == "regen_splats_start"))
						{
							return;
						}
						this.OnRegenSplatsStart();
						return;
					}
					else
					{
						if (!(eventName == "charge_steam_start"))
						{
							return;
						}
						this.OnChargeSteamStart();
						return;
					}
					break;
				}
				case 19:
					if (!(eventName == "rocket_thrust_start"))
					{
						return;
					}
					this.OnRocketThrustStart();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x0008A408 File Offset: 0x00088608
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			string currentAnimationName = new MegaAnimationState(animationState).GetCurrentAnimationName(2);
			if (currentAnimationName != "right/charged_loop" && currentAnimationName != "right/charge_up" && currentAnimationName != "right/hurt_charged")
			{
				this.OnChargeSteamEnd();
			}
			if (currentAnimationName != "right/attack_heavy")
			{
				this.OnRocketThrustEnd();
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0008A467 File Offset: 0x00088667
		private void OnChargeSteamStart()
		{
			this._steamParticles1.Restart();
			this._steamParticles2.Restart();
			this._steamParticles3.Restart();
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0008A48A File Offset: 0x0008868A
		private void OnChargeSteamEnd()
		{
			this._steamParticles1.Emitting = false;
			this._steamParticles2.Emitting = false;
			this._steamParticles3.Emitting = false;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0008A4B0 File Offset: 0x000886B0
		private void OnDeathSpitStart()
		{
			this._spittleParticles.Restart();
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0008A4BD File Offset: 0x000886BD
		private void OnDeathSpitEnd()
		{
			this._spittleParticles.Emitting = false;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0008A4CB File Offset: 0x000886CB
		private void OnLeftEmbersStart()
		{
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0008A4CD File Offset: 0x000886CD
		private void OnPlowChunksStart()
		{
			this._plowChunkParticles.Restart();
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0008A4DA File Offset: 0x000886DA
		private void OnPlowChunksEnd()
		{
			this._plowChunkParticles.Emitting = false;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0008A4E8 File Offset: 0x000886E8
		private void OnRegenSplatsStart()
		{
			this._regenSplatParticles.Restart();
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0008A4F5 File Offset: 0x000886F5
		private void OnRegenSplatsEnd()
		{
			this._regenSplatParticles.Emitting = false;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0008A503 File Offset: 0x00088703
		private void OnRocketThrustStart()
		{
			this._smokeParticles.Restart();
			this._sparkParticles.Restart();
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0008A51B File Offset: 0x0008871B
		private void OnRocketThrustEnd()
		{
			this._smokeParticles.Emitting = false;
			this._sparkParticles.Emitting = false;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0008A535 File Offset: 0x00088735
		private void OnClawLExplode()
		{
			Vector2 globalPosition = this._leftArmExplosionPosition.GlobalPosition;
			string text = "vfx/monsters/kaiser_crab_boss_explosion";
			NCombatRoom instance = NCombatRoom.Instance;
			VfxCmd.PlayVfx(globalPosition, text, (instance != null) ? instance.CombatVfxContainer : null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001697 RID: 5783 RVA: 0x0008A560 File Offset: 0x00088760
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NKaiserCrabBossVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnChargeSteamStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnDeathSpitStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnPlowChunksStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRocketThrustStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossVfx.MethodName.OnClawLExplode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001698 RID: 5784 RVA: 0x0008A974 File Offset: 0x00088B74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKaiserCrabBossVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnChargeSteamStart && args.Count == 0)
			{
				this.OnChargeSteamStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd && args.Count == 0)
			{
				this.OnChargeSteamEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnDeathSpitStart && args.Count == 0)
			{
				this.OnDeathSpitStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd && args.Count == 0)
			{
				this.OnDeathSpitEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart && args.Count == 0)
			{
				this.OnLeftEmbersStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnPlowChunksStart && args.Count == 0)
			{
				this.OnPlowChunksStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd && args.Count == 0)
			{
				this.OnPlowChunksEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart && args.Count == 0)
			{
				this.OnRegenSplatsStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd && args.Count == 0)
			{
				this.OnRegenSplatsEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnRocketThrustStart && args.Count == 0)
			{
				this.OnRocketThrustStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd && args.Count == 0)
			{
				this.OnRocketThrustEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossVfx.MethodName.OnClawLExplode && args.Count == 0)
			{
				this.OnClawLExplode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001699 RID: 5785 RVA: 0x0008AC14 File Offset: 0x00088E14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKaiserCrabBossVfx.MethodName._Ready || (ref method) == NKaiserCrabBossVfx.MethodName.OnAnimationEvent || (ref method) == NKaiserCrabBossVfx.MethodName.OnAnimationStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnChargeSteamStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd || (ref method) == NKaiserCrabBossVfx.MethodName.OnDeathSpitStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd || (ref method) == NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnPlowChunksStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd || (ref method) == NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd || (ref method) == NKaiserCrabBossVfx.MethodName.OnRocketThrustStart || (ref method) == NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd || (ref method) == NKaiserCrabBossVfx.MethodName.OnClawLExplode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600169A RID: 5786 RVA: 0x0008AD0C File Offset: 0x00088F0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._regenSplatParticles)
			{
				this._regenSplatParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._plowChunkParticles)
			{
				this._plowChunkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles1)
			{
				this._steamParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles2)
			{
				this._steamParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles3)
			{
				this._steamParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._smokeParticles)
			{
				this._smokeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._sparkParticles)
			{
				this._sparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._spittleParticles)
			{
				this._spittleParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition)
			{
				this._leftArmExplosionPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600169B RID: 5787 RVA: 0x0008AE30 File Offset: 0x00089030
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._regenSplatParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._regenSplatParticles);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._plowChunkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._plowChunkParticles);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles1);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles2);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._steamParticles3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles3);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._smokeParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticles);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._sparkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkParticles);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._spittleParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spittleParticles);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._leftArmExplosionPosition);
				return true;
			}
			if ((ref name) == NKaiserCrabBossVfx.PropertyName._parent)
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
		// Token: 0x0600169C RID: 5788 RVA: 0x0008AF88 File Offset: 0x00089188
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._regenSplatParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._plowChunkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._steamParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._steamParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._steamParticles3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._smokeParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._sparkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._spittleParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600169D RID: 5789 RVA: 0x0008B0E8 File Offset: 0x000892E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._regenSplatParticles, Variant.From<GpuParticles2D>(ref this._regenSplatParticles));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._plowChunkParticles, Variant.From<GpuParticles2D>(ref this._plowChunkParticles));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles1, Variant.From<GpuParticles2D>(ref this._steamParticles1));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles2, Variant.From<GpuParticles2D>(ref this._steamParticles2));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles3, Variant.From<GpuParticles2D>(ref this._steamParticles3));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._smokeParticles, Variant.From<GpuParticles2D>(ref this._smokeParticles));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._sparkParticles, Variant.From<GpuParticles2D>(ref this._sparkParticles));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._spittleParticles, Variant.From<GpuParticles2D>(ref this._spittleParticles));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, Variant.From<Node2D>(ref this._leftArmExplosionPosition));
			info.AddProperty(NKaiserCrabBossVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x0600169E RID: 5790 RVA: 0x0008B1D8 File Offset: 0x000893D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._regenSplatParticles, ref variant))
			{
				this._regenSplatParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._plowChunkParticles, ref variant2))
			{
				this._plowChunkParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles1, ref variant3))
			{
				this._steamParticles1 = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles2, ref variant4))
			{
				this._steamParticles2 = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles3, ref variant5))
			{
				this._steamParticles3 = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._smokeParticles, ref variant6))
			{
				this._smokeParticles = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._sparkParticles, ref variant7))
			{
				this._sparkParticles = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._spittleParticles, ref variant8))
			{
				this._spittleParticles = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, ref variant9))
			{
				this._leftArmExplosionPosition = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._parent, ref variant10))
			{
				this._parent = variant10.As<Node2D>();
			}
		}

		// Token: 0x04000805 RID: 2053
		private MegaSprite _megaSprite;

		// Token: 0x04000806 RID: 2054
		private GpuParticles2D _regenSplatParticles;

		// Token: 0x04000807 RID: 2055
		private GpuParticles2D _plowChunkParticles;

		// Token: 0x04000808 RID: 2056
		private GpuParticles2D _steamParticles1;

		// Token: 0x04000809 RID: 2057
		private GpuParticles2D _steamParticles2;

		// Token: 0x0400080A RID: 2058
		private GpuParticles2D _steamParticles3;

		// Token: 0x0400080B RID: 2059
		private GpuParticles2D _smokeParticles;

		// Token: 0x0400080C RID: 2060
		private GpuParticles2D _sparkParticles;

		// Token: 0x0400080D RID: 2061
		private GpuParticles2D _spittleParticles;

		// Token: 0x0400080E RID: 2062
		private Node2D _leftArmExplosionPosition;

		// Token: 0x0400080F RID: 2063
		private Node2D _parent;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001087 RID: 4231
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040F3 RID: 16627
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040040F4 RID: 16628
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x040040F5 RID: 16629
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'OnChargeSteamStart' method.
			/// </summary>
			// Token: 0x040040F6 RID: 16630
			public static readonly StringName OnChargeSteamStart = "OnChargeSteamStart";

			/// <summary>
			/// Cached name for the 'OnChargeSteamEnd' method.
			/// </summary>
			// Token: 0x040040F7 RID: 16631
			public static readonly StringName OnChargeSteamEnd = "OnChargeSteamEnd";

			/// <summary>
			/// Cached name for the 'OnDeathSpitStart' method.
			/// </summary>
			// Token: 0x040040F8 RID: 16632
			public static readonly StringName OnDeathSpitStart = "OnDeathSpitStart";

			/// <summary>
			/// Cached name for the 'OnDeathSpitEnd' method.
			/// </summary>
			// Token: 0x040040F9 RID: 16633
			public static readonly StringName OnDeathSpitEnd = "OnDeathSpitEnd";

			/// <summary>
			/// Cached name for the 'OnLeftEmbersStart' method.
			/// </summary>
			// Token: 0x040040FA RID: 16634
			public static readonly StringName OnLeftEmbersStart = "OnLeftEmbersStart";

			/// <summary>
			/// Cached name for the 'OnPlowChunksStart' method.
			/// </summary>
			// Token: 0x040040FB RID: 16635
			public static readonly StringName OnPlowChunksStart = "OnPlowChunksStart";

			/// <summary>
			/// Cached name for the 'OnPlowChunksEnd' method.
			/// </summary>
			// Token: 0x040040FC RID: 16636
			public static readonly StringName OnPlowChunksEnd = "OnPlowChunksEnd";

			/// <summary>
			/// Cached name for the 'OnRegenSplatsStart' method.
			/// </summary>
			// Token: 0x040040FD RID: 16637
			public static readonly StringName OnRegenSplatsStart = "OnRegenSplatsStart";

			/// <summary>
			/// Cached name for the 'OnRegenSplatsEnd' method.
			/// </summary>
			// Token: 0x040040FE RID: 16638
			public static readonly StringName OnRegenSplatsEnd = "OnRegenSplatsEnd";

			/// <summary>
			/// Cached name for the 'OnRocketThrustStart' method.
			/// </summary>
			// Token: 0x040040FF RID: 16639
			public static readonly StringName OnRocketThrustStart = "OnRocketThrustStart";

			/// <summary>
			/// Cached name for the 'OnRocketThrustEnd' method.
			/// </summary>
			// Token: 0x04004100 RID: 16640
			public static readonly StringName OnRocketThrustEnd = "OnRocketThrustEnd";

			/// <summary>
			/// Cached name for the 'OnClawLExplode' method.
			/// </summary>
			// Token: 0x04004101 RID: 16641
			public static readonly StringName OnClawLExplode = "OnClawLExplode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001088 RID: 4232
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_regenSplatParticles' field.
			/// </summary>
			// Token: 0x04004102 RID: 16642
			public static readonly StringName _regenSplatParticles = "_regenSplatParticles";

			/// <summary>
			/// Cached name for the '_plowChunkParticles' field.
			/// </summary>
			// Token: 0x04004103 RID: 16643
			public static readonly StringName _plowChunkParticles = "_plowChunkParticles";

			/// <summary>
			/// Cached name for the '_steamParticles1' field.
			/// </summary>
			// Token: 0x04004104 RID: 16644
			public static readonly StringName _steamParticles1 = "_steamParticles1";

			/// <summary>
			/// Cached name for the '_steamParticles2' field.
			/// </summary>
			// Token: 0x04004105 RID: 16645
			public static readonly StringName _steamParticles2 = "_steamParticles2";

			/// <summary>
			/// Cached name for the '_steamParticles3' field.
			/// </summary>
			// Token: 0x04004106 RID: 16646
			public static readonly StringName _steamParticles3 = "_steamParticles3";

			/// <summary>
			/// Cached name for the '_smokeParticles' field.
			/// </summary>
			// Token: 0x04004107 RID: 16647
			public static readonly StringName _smokeParticles = "_smokeParticles";

			/// <summary>
			/// Cached name for the '_sparkParticles' field.
			/// </summary>
			// Token: 0x04004108 RID: 16648
			public static readonly StringName _sparkParticles = "_sparkParticles";

			/// <summary>
			/// Cached name for the '_spittleParticles' field.
			/// </summary>
			// Token: 0x04004109 RID: 16649
			public static readonly StringName _spittleParticles = "_spittleParticles";

			/// <summary>
			/// Cached name for the '_leftArmExplosionPosition' field.
			/// </summary>
			// Token: 0x0400410A RID: 16650
			public static readonly StringName _leftArmExplosionPosition = "_leftArmExplosionPosition";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x0400410B RID: 16651
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001089 RID: 4233
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
