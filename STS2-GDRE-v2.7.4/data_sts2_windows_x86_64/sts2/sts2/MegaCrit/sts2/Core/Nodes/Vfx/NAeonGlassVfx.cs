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
	// Token: 0x0200017F RID: 383
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NAeonGlassVfx.cs")]
	public class NAeonGlassVfx : Node
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0006C93C File Offset: 0x0006AB3C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._liquidShaderMat = new MegaSlotNode(this._parent.GetNode("LiquidSlot")).GetNormalMaterial() as ShaderMaterial;
			if (this._liquidShaderMat != null)
			{
				this._baseScrollSpeed = (float)this._liquidShaderMat.GetShaderParameter(NAeonGlassVfx._scrollSpeedString);
			}
			this._witherParticles = this._parent.GetNode<GpuParticles2D>("WitherSlot/WitherParticles");
			this._leakParticles = this._parent.GetNode<GpuParticles2D>("LiquidSlot/LeakParticles");
			this._shardParticles = this._parent.GetNode<GpuParticles2D>("GlassCenterSlot/ShardParticles");
			this._dumpParticles = this._parent.GetNode<GpuParticles2D>("GlassCenterSlot/DumpParticles");
			this._topSparkParticles = this._parent.GetNode<GpuParticles2D>("TopSparksSlot/TopSparkParticles");
			this._bottomSparkParticles = this._parent.GetNode<GpuParticles2D>("BottomSparksSlot/BottomSparkParticles");
			this._groundDustParticles = this._parent.GetNode<GpuParticles2D>("GroundPlowSlot/DustParticles");
			this._groundChunkParticles = this._parent.GetNode<GpuParticles2D>("GroundPlowSlot/ChunkParticles");
			this._bottomSparkParticles.OneShot = true;
			this._topSparkParticles.OneShot = true;
			this._witherParticles.OneShot = true;
			this._shardParticles.OneShot = true;
			this._dumpParticles.OneShot = true;
			this.ResetVfx();
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("_track1/rings_normal", true, 1);
			});
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0006CB40 File Offset: 0x0006AD40
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				int length = eventName.Length;
				switch (length)
				{
				case 7:
					if (!(eventName == "end_die"))
					{
						return;
					}
					this.EndDie();
					return;
				case 8:
				case 11:
					break;
				case 9:
					if (!(eventName == "start_die"))
					{
						return;
					}
					this.StartDie();
					return;
				case 10:
					if (!(eventName == "end_wither"))
					{
						return;
					}
					this.EndWither();
					return;
				case 12:
				{
					char c = eventName[6];
					if (c != 's')
					{
						if (c == 'w')
						{
							if (!(eventName == "start_wither"))
							{
								return;
							}
							this.StartWither();
							return;
						}
					}
					else
					{
						if (!(eventName == "start_sparks"))
						{
							return;
						}
						this.StartSparks();
						return;
					}
					break;
				}
				default:
					if (length != 17)
					{
						if (length != 19)
						{
							return;
						}
						if (!(eventName == "start_ground_scrape"))
						{
							return;
						}
						this.StartScrape();
						return;
					}
					else
					{
						if (!(eventName == "end_ground_scrape"))
						{
							return;
						}
						this.EndScrape();
					}
					break;
				}
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0006CC44 File Offset: 0x0006AE44
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			this.ResetVfx();
			MegaAnimationState animationState2 = this._animController.GetAnimationState();
			string currentAnimationName = animationState2.GetCurrentAnimationName(0);
			if (currentAnimationName == this._curAnimName)
			{
				return;
			}
			this._curAnimName = currentAnimationName;
			if (currentAnimationName == "idle_loop" || currentAnimationName == "hurt" || currentAnimationName == "wither")
			{
				if (!this._ringsSpinningNormal)
				{
					animationState2.SetAnimation("_track1/rings_normal", true, 1);
				}
				this._ringsSpinningNormal = true;
			}
			if (currentAnimationName == "attack_heavy")
			{
				animationState2.SetAnimation("_track1/rings_attack_heavy", false, 1);
				animationState2.AddAnimation("_track1/rings_normal", 0f, true, 1);
				this._ringsSpinningNormal = false;
				return;
			}
			if (currentAnimationName == "attack_double")
			{
				animationState2.SetAnimation("_track1/rings_attack_double", false, 1);
				animationState2.AddAnimation("_track1/rings_normal", 0f, true, 1);
				this._ringsSpinningNormal = false;
				return;
			}
			if (currentAnimationName == "die")
			{
				animationState2.SetAnimation("_track1/rings_die", false, 1);
				this._ringsSpinningNormal = false;
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0006CD4D File Offset: 0x0006AF4D
		private void StartWither()
		{
			this._witherParticles.Restart();
			ShaderMaterial liquidShaderMat = this._liquidShaderMat;
			if (liquidShaderMat == null)
			{
				return;
			}
			liquidShaderMat.SetShaderParameter(NAeonGlassVfx._scrollSpeedString, -2f);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0006CD79 File Offset: 0x0006AF79
		private void EndWither()
		{
			this.ResetVfx();
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0006CD81 File Offset: 0x0006AF81
		private void StartDie()
		{
			this._dumpParticles.Restart();
			this._leakParticles.Restart();
			this._shardParticles.Restart();
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0006CDA4 File Offset: 0x0006AFA4
		private void EndDie()
		{
			this._leakParticles.Emitting = false;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0006CDB2 File Offset: 0x0006AFB2
		private void StartSparks()
		{
			this._topSparkParticles.Restart();
			this._bottomSparkParticles.Restart();
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0006CDCA File Offset: 0x0006AFCA
		private void StartScrape()
		{
			this._groundChunkParticles.Restart();
			this._groundDustParticles.Restart();
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0006CDE2 File Offset: 0x0006AFE2
		private void EndScrape()
		{
			this._groundChunkParticles.Emitting = false;
			this._groundDustParticles.Emitting = false;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0006CDFC File Offset: 0x0006AFFC
		private void ResetVfx()
		{
			ShaderMaterial liquidShaderMat = this._liquidShaderMat;
			if (liquidShaderMat != null)
			{
				liquidShaderMat.SetShaderParameter(NAeonGlassVfx._scrollSpeedString, this._baseScrollSpeed);
			}
			this._witherParticles.Restart();
			this._witherParticles.Emitting = false;
			this._leakParticles.Restart();
			this._leakParticles.Emitting = false;
			this._shardParticles.Emitting = false;
			this._dumpParticles.Restart();
			this._dumpParticles.Emitting = false;
			this._topSparkParticles.Emitting = false;
			this._bottomSparkParticles.Emitting = false;
			this._groundChunkParticles.Emitting = false;
			this._groundDustParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001278 RID: 4728 RVA: 0x0006CEAC File Offset: 0x0006B0AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NAeonGlassVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NAeonGlassVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NAeonGlassVfx.MethodName.StartWither, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.EndWither, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.StartDie, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.EndDie, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.StartSparks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.StartScrape, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.EndScrape, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAeonGlassVfx.MethodName.ResetVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001279 RID: 4729 RVA: 0x0006D204 File Offset: 0x0006B404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAeonGlassVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.StartWither && args.Count == 0)
			{
				this.StartWither();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.EndWither && args.Count == 0)
			{
				this.EndWither();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.StartDie && args.Count == 0)
			{
				this.StartDie();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.EndDie && args.Count == 0)
			{
				this.EndDie();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.StartSparks && args.Count == 0)
			{
				this.StartSparks();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.StartScrape && args.Count == 0)
			{
				this.StartScrape();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.EndScrape && args.Count == 0)
			{
				this.EndScrape();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAeonGlassVfx.MethodName.ResetVfx && args.Count == 0)
			{
				this.ResetVfx();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600127A RID: 4730 RVA: 0x0006D410 File Offset: 0x0006B610
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAeonGlassVfx.MethodName._Ready || (ref method) == NAeonGlassVfx.MethodName.OnAnimationEvent || (ref method) == NAeonGlassVfx.MethodName.OnAnimationStart || (ref method) == NAeonGlassVfx.MethodName.StartWither || (ref method) == NAeonGlassVfx.MethodName.EndWither || (ref method) == NAeonGlassVfx.MethodName.StartDie || (ref method) == NAeonGlassVfx.MethodName.EndDie || (ref method) == NAeonGlassVfx.MethodName.StartSparks || (ref method) == NAeonGlassVfx.MethodName.StartScrape || (ref method) == NAeonGlassVfx.MethodName.EndScrape || (ref method) == NAeonGlassVfx.MethodName.ResetVfx || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600127B RID: 4731 RVA: 0x0006D4CC File Offset: 0x0006B6CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAeonGlassVfx.PropertyName._ringsSpinningNormal)
			{
				this._ringsSpinningNormal = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._curAnimName)
			{
				this._curAnimName = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._liquidShaderMat)
			{
				this._liquidShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._baseScrollSpeed)
			{
				this._baseScrollSpeed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._witherParticles)
			{
				this._witherParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._leakParticles)
			{
				this._leakParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._shardParticles)
			{
				this._shardParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._dumpParticles)
			{
				this._dumpParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._topSparkParticles)
			{
				this._topSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._bottomSparkParticles)
			{
				this._bottomSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._groundDustParticles)
			{
				this._groundDustParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._groundChunkParticles)
			{
				this._groundChunkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600127C RID: 4732 RVA: 0x0006D640 File Offset: 0x0006B840
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAeonGlassVfx.PropertyName._ringsSpinningNormal)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._ringsSpinningNormal);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._curAnimName)
			{
				value = VariantUtils.CreateFrom<string>(ref this._curAnimName);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._liquidShaderMat)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._liquidShaderMat);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._baseScrollSpeed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseScrollSpeed);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._witherParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._witherParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._leakParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._leakParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._shardParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._shardParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._dumpParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dumpParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._topSparkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._topSparkParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._bottomSparkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bottomSparkParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._groundDustParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._groundDustParticles);
				return true;
			}
			if ((ref name) == NAeonGlassVfx.PropertyName._groundChunkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._groundChunkParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600127D RID: 4733 RVA: 0x0006D7F8 File Offset: 0x0006B9F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NAeonGlassVfx.PropertyName._ringsSpinningNormal, 0L, "", 4096L, false),
				new PropertyInfo(4L, NAeonGlassVfx.PropertyName._curAnimName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._liquidShaderMat, 0L, "", 4096L, false),
				new PropertyInfo(3L, NAeonGlassVfx.PropertyName._baseScrollSpeed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._witherParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._leakParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._shardParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._dumpParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._topSparkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._bottomSparkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._groundDustParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAeonGlassVfx.PropertyName._groundChunkParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600127E RID: 4734 RVA: 0x0006D9B8 File Offset: 0x0006BBB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAeonGlassVfx.PropertyName._ringsSpinningNormal, Variant.From<bool>(ref this._ringsSpinningNormal));
			info.AddProperty(NAeonGlassVfx.PropertyName._curAnimName, Variant.From<string>(ref this._curAnimName));
			info.AddProperty(NAeonGlassVfx.PropertyName._liquidShaderMat, Variant.From<ShaderMaterial>(ref this._liquidShaderMat));
			info.AddProperty(NAeonGlassVfx.PropertyName._baseScrollSpeed, Variant.From<float>(ref this._baseScrollSpeed));
			info.AddProperty(NAeonGlassVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NAeonGlassVfx.PropertyName._witherParticles, Variant.From<GpuParticles2D>(ref this._witherParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._leakParticles, Variant.From<GpuParticles2D>(ref this._leakParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._shardParticles, Variant.From<GpuParticles2D>(ref this._shardParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._dumpParticles, Variant.From<GpuParticles2D>(ref this._dumpParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._topSparkParticles, Variant.From<GpuParticles2D>(ref this._topSparkParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._bottomSparkParticles, Variant.From<GpuParticles2D>(ref this._bottomSparkParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._groundDustParticles, Variant.From<GpuParticles2D>(ref this._groundDustParticles));
			info.AddProperty(NAeonGlassVfx.PropertyName._groundChunkParticles, Variant.From<GpuParticles2D>(ref this._groundChunkParticles));
		}

		/// <inheritdoc />
		// Token: 0x0600127F RID: 4735 RVA: 0x0006DAEC File Offset: 0x0006BCEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._ringsSpinningNormal, ref variant))
			{
				this._ringsSpinningNormal = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._curAnimName, ref variant2))
			{
				this._curAnimName = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._liquidShaderMat, ref variant3))
			{
				this._liquidShaderMat = variant3.As<ShaderMaterial>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._baseScrollSpeed, ref variant4))
			{
				this._baseScrollSpeed = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._parent, ref variant5))
			{
				this._parent = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._witherParticles, ref variant6))
			{
				this._witherParticles = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._leakParticles, ref variant7))
			{
				this._leakParticles = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._shardParticles, ref variant8))
			{
				this._shardParticles = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._dumpParticles, ref variant9))
			{
				this._dumpParticles = variant9.As<GpuParticles2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._topSparkParticles, ref variant10))
			{
				this._topSparkParticles = variant10.As<GpuParticles2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._bottomSparkParticles, ref variant11))
			{
				this._bottomSparkParticles = variant11.As<GpuParticles2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._groundDustParticles, ref variant12))
			{
				this._groundDustParticles = variant12.As<GpuParticles2D>();
			}
			Variant variant13;
			if (info.TryGetProperty(NAeonGlassVfx.PropertyName._groundChunkParticles, ref variant13))
			{
				this._groundChunkParticles = variant13.As<GpuParticles2D>();
			}
		}

		// Token: 0x0400067C RID: 1660
		private bool _ringsSpinningNormal;

		// Token: 0x0400067D RID: 1661
		[Nullable(2)]
		private string _curAnimName;

		// Token: 0x0400067E RID: 1662
		private static readonly StringName _scrollSpeedString = new StringName("ScrollSpeed");

		// Token: 0x0400067F RID: 1663
		[Nullable(2)]
		private ShaderMaterial _liquidShaderMat;

		// Token: 0x04000680 RID: 1664
		private float _baseScrollSpeed;

		// Token: 0x04000681 RID: 1665
		private Node2D _parent;

		// Token: 0x04000682 RID: 1666
		private MegaSprite _animController;

		// Token: 0x04000683 RID: 1667
		private GpuParticles2D _witherParticles;

		// Token: 0x04000684 RID: 1668
		private GpuParticles2D _leakParticles;

		// Token: 0x04000685 RID: 1669
		private GpuParticles2D _shardParticles;

		// Token: 0x04000686 RID: 1670
		private GpuParticles2D _dumpParticles;

		// Token: 0x04000687 RID: 1671
		private GpuParticles2D _topSparkParticles;

		// Token: 0x04000688 RID: 1672
		private GpuParticles2D _bottomSparkParticles;

		// Token: 0x04000689 RID: 1673
		private GpuParticles2D _groundDustParticles;

		// Token: 0x0400068A RID: 1674
		private GpuParticles2D _groundChunkParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F8B RID: 3979
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E02 RID: 15874
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003E03 RID: 15875
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04003E04 RID: 15876
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'StartWither' method.
			/// </summary>
			// Token: 0x04003E05 RID: 15877
			public static readonly StringName StartWither = "StartWither";

			/// <summary>
			/// Cached name for the 'EndWither' method.
			/// </summary>
			// Token: 0x04003E06 RID: 15878
			public static readonly StringName EndWither = "EndWither";

			/// <summary>
			/// Cached name for the 'StartDie' method.
			/// </summary>
			// Token: 0x04003E07 RID: 15879
			public static readonly StringName StartDie = "StartDie";

			/// <summary>
			/// Cached name for the 'EndDie' method.
			/// </summary>
			// Token: 0x04003E08 RID: 15880
			public static readonly StringName EndDie = "EndDie";

			/// <summary>
			/// Cached name for the 'StartSparks' method.
			/// </summary>
			// Token: 0x04003E09 RID: 15881
			public static readonly StringName StartSparks = "StartSparks";

			/// <summary>
			/// Cached name for the 'StartScrape' method.
			/// </summary>
			// Token: 0x04003E0A RID: 15882
			public static readonly StringName StartScrape = "StartScrape";

			/// <summary>
			/// Cached name for the 'EndScrape' method.
			/// </summary>
			// Token: 0x04003E0B RID: 15883
			public static readonly StringName EndScrape = "EndScrape";

			/// <summary>
			/// Cached name for the 'ResetVfx' method.
			/// </summary>
			// Token: 0x04003E0C RID: 15884
			public static readonly StringName ResetVfx = "ResetVfx";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F8C RID: 3980
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_ringsSpinningNormal' field.
			/// </summary>
			// Token: 0x04003E0D RID: 15885
			public static readonly StringName _ringsSpinningNormal = "_ringsSpinningNormal";

			/// <summary>
			/// Cached name for the '_curAnimName' field.
			/// </summary>
			// Token: 0x04003E0E RID: 15886
			public static readonly StringName _curAnimName = "_curAnimName";

			/// <summary>
			/// Cached name for the '_liquidShaderMat' field.
			/// </summary>
			// Token: 0x04003E0F RID: 15887
			public static readonly StringName _liquidShaderMat = "_liquidShaderMat";

			/// <summary>
			/// Cached name for the '_baseScrollSpeed' field.
			/// </summary>
			// Token: 0x04003E10 RID: 15888
			public static readonly StringName _baseScrollSpeed = "_baseScrollSpeed";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003E11 RID: 15889
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_witherParticles' field.
			/// </summary>
			// Token: 0x04003E12 RID: 15890
			public static readonly StringName _witherParticles = "_witherParticles";

			/// <summary>
			/// Cached name for the '_leakParticles' field.
			/// </summary>
			// Token: 0x04003E13 RID: 15891
			public static readonly StringName _leakParticles = "_leakParticles";

			/// <summary>
			/// Cached name for the '_shardParticles' field.
			/// </summary>
			// Token: 0x04003E14 RID: 15892
			public static readonly StringName _shardParticles = "_shardParticles";

			/// <summary>
			/// Cached name for the '_dumpParticles' field.
			/// </summary>
			// Token: 0x04003E15 RID: 15893
			public static readonly StringName _dumpParticles = "_dumpParticles";

			/// <summary>
			/// Cached name for the '_topSparkParticles' field.
			/// </summary>
			// Token: 0x04003E16 RID: 15894
			public static readonly StringName _topSparkParticles = "_topSparkParticles";

			/// <summary>
			/// Cached name for the '_bottomSparkParticles' field.
			/// </summary>
			// Token: 0x04003E17 RID: 15895
			public static readonly StringName _bottomSparkParticles = "_bottomSparkParticles";

			/// <summary>
			/// Cached name for the '_groundDustParticles' field.
			/// </summary>
			// Token: 0x04003E18 RID: 15896
			public static readonly StringName _groundDustParticles = "_groundDustParticles";

			/// <summary>
			/// Cached name for the '_groundChunkParticles' field.
			/// </summary>
			// Token: 0x04003E19 RID: 15897
			public static readonly StringName _groundChunkParticles = "_groundChunkParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F8D RID: 3981
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
