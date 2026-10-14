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
	// Token: 0x0200020D RID: 525
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NWaterfallGiantVfx.cs")]
	public class NWaterfallGiantVfx : Node
	{
		// Token: 0x06001B28 RID: 6952 RVA: 0x000AF6D8 File Offset: 0x000AD8D8
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._steam1Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot1/steamParticles1");
			this._steam2Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot2/steamParticles2");
			this._steam3Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot3/steamParticles3");
			this._steam4Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot4/steamParticles4");
			this._steam5Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot5/steamParticles5");
			this._steam6Particles = this._parent.GetNode<GpuParticles2D>("SteamSlot6/steamParticles6");
			this._steamLeakParticles1 = this._parent.GetNode<GpuParticles2D>("SteamLeakSlot1/steamLeakParticles1");
			this._steamLeakParticles2 = this._parent.GetNode<GpuParticles2D>("SteamLeakSlot2/steamLeakParticles2");
			this._steamLeakParticles3 = this._parent.GetNode<GpuParticles2D>("SteamLeakSlot3/steamLeakParticles3");
			this._mistParticles = this._parent.GetNode<GpuParticles2D>("MistSlot/MistParticles");
			this._dropletParticles = this._parent.GetNode<GpuParticles2D>("MistSlot/Droplets");
			this._mouthParticles = this._parent.GetNode<GpuParticles2D>("MouthDropletsSlot/MouthDroplets");
			this._leakProcMat1 = (ParticleProcessMaterial)this._steamLeakParticles1.ProcessMaterial;
			this._leakProcMat2 = (ParticleProcessMaterial)this._steamLeakParticles2.ProcessMaterial;
			this._leakProcMat3 = (ParticleProcessMaterial)this._steamLeakParticles3.ProcessMaterial;
			this._steam1Particles.Emitting = false;
			this._steam2Particles.Emitting = false;
			this._steam3Particles.Emitting = false;
			this._steam4Particles.Emitting = false;
			this._steam5Particles.Emitting = false;
			this._steam6Particles.Emitting = false;
			this._steamLeakParticles1.Emitting = false;
			this._steamLeakParticles2.Emitting = false;
			this._steamLeakParticles3.Emitting = false;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000AF918 File Offset: 0x000ADB18
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 7:
					if (!(eventName == "explode"))
					{
						return;
					}
					this.Explode();
					return;
				case 8:
					switch (eventName[7])
					{
					case '1':
						if (!(eventName == "buildup1"))
						{
							return;
						}
						this.Buildup1();
						return;
					case '2':
						if (!(eventName == "buildup2"))
						{
							return;
						}
						this.Buildup2();
						return;
					case '3':
						if (!(eventName == "buildup3"))
						{
							return;
						}
						this.Buildup3();
						return;
					default:
						return;
					}
					break;
				case 9:
				case 10:
				case 12:
				case 14:
				case 16:
					break;
				case 11:
					switch (eventName[6])
					{
					case '1':
						if (!(eventName == "steam_1_end"))
						{
							return;
						}
						this.EndSteam1();
						return;
					case '2':
						if (!(eventName == "steam_2_end"))
						{
							return;
						}
						this.EndSteam2();
						return;
					case '3':
						if (!(eventName == "steam_3_end"))
						{
							return;
						}
						this.EndSteam3();
						return;
					case '4':
						break;
					case '5':
						if (!(eventName == "steam_5_end"))
						{
							return;
						}
						this.EndSteam5();
						return;
					default:
						return;
					}
					break;
				case 13:
				{
					char c = eventName[6];
					switch (c)
					{
					case '1':
						if (!(eventName == "steam_1_start"))
						{
							return;
						}
						this.StartSteam1();
						return;
					case '2':
						if (!(eventName == "steam_2_start"))
						{
							return;
						}
						this.StartSteam2();
						return;
					case '3':
						if (!(eventName == "steam_3_start"))
						{
							return;
						}
						this.StartSteam3();
						return;
					case '4':
						break;
					case '5':
						if (!(eventName == "steam_5_start"))
						{
							return;
						}
						this.StartSteam5();
						return;
					default:
						if (c != 'a')
						{
							return;
						}
						if (!(eventName == "waterfall_end"))
						{
							return;
						}
						this.EndWaterfall();
						return;
					}
					break;
				}
				case 15:
					if (!(eventName == "waterfall_start"))
					{
						return;
					}
					this.StartWaterfall();
					return;
				case 17:
					if (!(eventName == "clear_death_steam"))
					{
						return;
					}
					this.ClearDeathSteam();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000AFB45 File Offset: 0x000ADD45
		private void StartSteam1()
		{
			this.EmitGracefully(this._steam1Particles);
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x000AFB53 File Offset: 0x000ADD53
		private void EndSteam1()
		{
			this._steam1Particles.Emitting = false;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000AFB61 File Offset: 0x000ADD61
		private void StartSteam2()
		{
			this.EmitGracefully(this._steam2Particles);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000AFB6F File Offset: 0x000ADD6F
		private void EndSteam2()
		{
			this._steam2Particles.Emitting = false;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000AFB7D File Offset: 0x000ADD7D
		private void StartSteam3()
		{
			this.EmitGracefully(this._steam3Particles);
			this.EmitGracefully(this._steam4Particles);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000AFB97 File Offset: 0x000ADD97
		private void EndSteam3()
		{
			this._steam3Particles.Emitting = false;
			this._steam4Particles.Emitting = false;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000AFBB1 File Offset: 0x000ADDB1
		private void StartSteam5()
		{
			this.EmitGracefully(this._steam5Particles);
			this.EmitGracefully(this._steam6Particles);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000AFBCB File Offset: 0x000ADDCB
		private void EndSteam5()
		{
			this._steam5Particles.Emitting = false;
			this._steam6Particles.Emitting = false;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000AFBE5 File Offset: 0x000ADDE5
		private void StartWaterfall()
		{
			this._mouthParticles.Emitting = true;
			this._dropletParticles.Emitting = true;
			this._mistParticles.Emitting = true;
			this._isDead = false;
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000AFC12 File Offset: 0x000ADE12
		private void EndWaterfall()
		{
			this._mouthParticles.Emitting = false;
			this._dropletParticles.Emitting = false;
			this._mistParticles.Emitting = false;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000AFC38 File Offset: 0x000ADE38
		private void Explode()
		{
			this._steam1Particles.Visible = false;
			this._steam2Particles.Visible = false;
			this._steam3Particles.Visible = false;
			this._steam4Particles.Visible = false;
			this._steam5Particles.Visible = false;
			this._steam6Particles.Visible = false;
			this._steamLeakParticles1.Visible = false;
			this._steamLeakParticles2.Visible = false;
			this._steamLeakParticles3.Visible = false;
			this._isDead = true;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000AFCB8 File Offset: 0x000ADEB8
		private void Buildup1()
		{
			if (!this._isDead)
			{
				this.EmitGracefully(this._steamLeakParticles1);
				this.EmitGracefully(this._steamLeakParticles2);
				this.EmitGracefully(this._steamLeakParticles3);
				this._leakProcMat1.ScaleMin = (this._leakProcMat2.ScaleMin = (this._leakProcMat3.ScaleMin = 0.2f));
				this._leakProcMat1.ScaleMax = (this._leakProcMat2.ScaleMax = (this._leakProcMat3.ScaleMax = 0.4f));
				this._steamLeakParticles1.Amount = (this._steamLeakParticles2.Amount = (this._steamLeakParticles3.Amount = 10));
				this._steamLeakParticles1.Lifetime = (this._steamLeakParticles2.Lifetime = (this._steamLeakParticles3.Lifetime = 0.3700000047683716));
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000AFDAC File Offset: 0x000ADFAC
		private void Buildup2()
		{
			if (!this._isDead)
			{
				this.EmitGracefully(this._steamLeakParticles1);
				this.EmitGracefully(this._steamLeakParticles2);
				this.EmitGracefully(this._steamLeakParticles3);
				this._leakProcMat1.ScaleMin = (this._leakProcMat2.ScaleMin = (this._leakProcMat3.ScaleMin = 0.3f));
				this._leakProcMat1.ScaleMax = (this._leakProcMat2.ScaleMax = (this._leakProcMat3.ScaleMax = 0.6f));
				this._steamLeakParticles1.Amount = (this._steamLeakParticles2.Amount = (this._steamLeakParticles3.Amount = 20));
				this._steamLeakParticles1.Lifetime = (this._steamLeakParticles2.Lifetime = (this._steamLeakParticles3.Lifetime = 0.75));
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000AFEA0 File Offset: 0x000AE0A0
		private void Buildup3()
		{
			if (!this._isDead)
			{
				this.EmitGracefully(this._steamLeakParticles1);
				this.EmitGracefully(this._steamLeakParticles2);
				this.EmitGracefully(this._steamLeakParticles3);
				this._leakProcMat1.ScaleMin = (this._leakProcMat2.ScaleMin = (this._leakProcMat3.ScaleMin = 0.4f));
				this._leakProcMat1.ScaleMax = (this._leakProcMat2.ScaleMax = (this._leakProcMat3.ScaleMax = 0.8f));
				this._steamLeakParticles1.Amount = (this._steamLeakParticles2.Amount = (this._steamLeakParticles3.Amount = 28));
				this._steamLeakParticles1.Lifetime = (this._steamLeakParticles2.Lifetime = (this._steamLeakParticles3.Lifetime = 1.2));
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000AFF94 File Offset: 0x000AE194
		private void ClearDeathSteam()
		{
			this._steam3Particles.Emitting = false;
			this._steam4Particles.Emitting = false;
			this._steam5Particles.Emitting = false;
			this._steam6Particles.Emitting = false;
			this._steam3Particles.Visible = false;
			this._steam4Particles.Visible = false;
			this._steam5Particles.Visible = false;
			this._steam6Particles.Visible = false;
			this._isDead = false;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000B0008 File Offset: 0x000AE208
		private void EmitGracefully(GpuParticles2D emitter)
		{
			if (!emitter.Visible)
			{
				emitter.Visible = true;
				emitter.Restart();
				return;
			}
			emitter.Emitting = true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B3A RID: 6970 RVA: 0x000B0028 File Offset: 0x000AE228
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NWaterfallGiantVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam5, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam5, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.StartWaterfall, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EndWaterfall, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.Explode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.ClearDeathSteam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWaterfallGiantVfx.MethodName.EmitGracefully, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "emitter", 0L, "", 6L, new StringName("GPUParticles2D"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B3B RID: 6971 RVA: 0x000B0470 File Offset: 0x000AE670
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NWaterfallGiantVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.StartSteam1 && args.Count == 0)
			{
				this.StartSteam1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EndSteam1 && args.Count == 0)
			{
				this.EndSteam1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.StartSteam2 && args.Count == 0)
			{
				this.StartSteam2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EndSteam2 && args.Count == 0)
			{
				this.EndSteam2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.StartSteam3 && args.Count == 0)
			{
				this.StartSteam3();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EndSteam3 && args.Count == 0)
			{
				this.EndSteam3();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.StartSteam5 && args.Count == 0)
			{
				this.StartSteam5();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EndSteam5 && args.Count == 0)
			{
				this.EndSteam5();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.StartWaterfall && args.Count == 0)
			{
				this.StartWaterfall();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EndWaterfall && args.Count == 0)
			{
				this.EndWaterfall();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.Explode && args.Count == 0)
			{
				this.Explode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.Buildup1 && args.Count == 0)
			{
				this.Buildup1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.Buildup2 && args.Count == 0)
			{
				this.Buildup2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.Buildup3 && args.Count == 0)
			{
				this.Buildup3();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.ClearDeathSteam && args.Count == 0)
			{
				this.ClearDeathSteam();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWaterfallGiantVfx.MethodName.EmitGracefully && args.Count == 1)
			{
				this.EmitGracefully(VariantUtils.ConvertTo<GpuParticles2D>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B3C RID: 6972 RVA: 0x000B0764 File Offset: 0x000AE964
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NWaterfallGiantVfx.MethodName._Ready || (ref method) == NWaterfallGiantVfx.MethodName.OnAnimationEvent || (ref method) == NWaterfallGiantVfx.MethodName.StartSteam1 || (ref method) == NWaterfallGiantVfx.MethodName.EndSteam1 || (ref method) == NWaterfallGiantVfx.MethodName.StartSteam2 || (ref method) == NWaterfallGiantVfx.MethodName.EndSteam2 || (ref method) == NWaterfallGiantVfx.MethodName.StartSteam3 || (ref method) == NWaterfallGiantVfx.MethodName.EndSteam3 || (ref method) == NWaterfallGiantVfx.MethodName.StartSteam5 || (ref method) == NWaterfallGiantVfx.MethodName.EndSteam5 || (ref method) == NWaterfallGiantVfx.MethodName.StartWaterfall || (ref method) == NWaterfallGiantVfx.MethodName.EndWaterfall || (ref method) == NWaterfallGiantVfx.MethodName.Explode || (ref method) == NWaterfallGiantVfx.MethodName.Buildup1 || (ref method) == NWaterfallGiantVfx.MethodName.Buildup2 || (ref method) == NWaterfallGiantVfx.MethodName.Buildup3 || (ref method) == NWaterfallGiantVfx.MethodName.ClearDeathSteam || (ref method) == NWaterfallGiantVfx.MethodName.EmitGracefully || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B3D RID: 6973 RVA: 0x000B0888 File Offset: 0x000AEA88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam1Particles)
			{
				this._steam1Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam2Particles)
			{
				this._steam2Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam3Particles)
			{
				this._steam3Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam4Particles)
			{
				this._steam4Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam5Particles)
			{
				this._steam5Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam6Particles)
			{
				this._steam6Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles1)
			{
				this._steamLeakParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles2)
			{
				this._steamLeakParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles3)
			{
				this._steamLeakParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._mistParticles)
			{
				this._mistParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._mouthParticles)
			{
				this._mouthParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._dropletParticles)
			{
				this._dropletParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat1)
			{
				this._leakProcMat1 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat2)
			{
				this._leakProcMat2 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat3)
			{
				this._leakProcMat3 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._isDead)
			{
				this._isDead = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B3E RID: 6974 RVA: 0x000B0A68 File Offset: 0x000AEC68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam1Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam1Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam2Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam2Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam3Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam3Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam4Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam4Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam5Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam5Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steam6Particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam6Particles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles1);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles2);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._steamLeakParticles3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles3);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._mistParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._mistParticles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._mouthParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._mouthParticles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._dropletParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dropletParticles);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat1)
			{
				value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat1);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat2)
			{
				value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat2);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._leakProcMat3)
			{
				value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat3);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._isDead)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDead);
				return true;
			}
			if ((ref name) == NWaterfallGiantVfx.PropertyName._parent)
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
		// Token: 0x06001B3F RID: 6975 RVA: 0x000B0CA0 File Offset: 0x000AEEA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam1Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam2Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam3Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam4Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam5Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steam6Particles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._mistParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._mouthParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._dropletParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._leakProcMat1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._leakProcMat2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._leakProcMat3, 0L, "", 4096L, false),
				new PropertyInfo(1L, NWaterfallGiantVfx.PropertyName._isDead, 0L, "", 4096L, false),
				new PropertyInfo(24L, NWaterfallGiantVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B40 RID: 6976 RVA: 0x000B0EE4 File Offset: 0x000AF0E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam1Particles, Variant.From<GpuParticles2D>(ref this._steam1Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam2Particles, Variant.From<GpuParticles2D>(ref this._steam2Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam3Particles, Variant.From<GpuParticles2D>(ref this._steam3Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam4Particles, Variant.From<GpuParticles2D>(ref this._steam4Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam5Particles, Variant.From<GpuParticles2D>(ref this._steam5Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steam6Particles, Variant.From<GpuParticles2D>(ref this._steam6Particles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles1, Variant.From<GpuParticles2D>(ref this._steamLeakParticles1));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles2, Variant.From<GpuParticles2D>(ref this._steamLeakParticles2));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles3, Variant.From<GpuParticles2D>(ref this._steamLeakParticles3));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._mistParticles, Variant.From<GpuParticles2D>(ref this._mistParticles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._mouthParticles, Variant.From<GpuParticles2D>(ref this._mouthParticles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._dropletParticles, Variant.From<GpuParticles2D>(ref this._dropletParticles));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat1, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat1));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat2, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat2));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat3, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat3));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._isDead, Variant.From<bool>(ref this._isDead));
			info.AddProperty(NWaterfallGiantVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001B41 RID: 6977 RVA: 0x000B1070 File Offset: 0x000AF270
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam1Particles, ref variant))
			{
				this._steam1Particles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam2Particles, ref variant2))
			{
				this._steam2Particles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam3Particles, ref variant3))
			{
				this._steam3Particles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam4Particles, ref variant4))
			{
				this._steam4Particles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam5Particles, ref variant5))
			{
				this._steam5Particles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam6Particles, ref variant6))
			{
				this._steam6Particles = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles1, ref variant7))
			{
				this._steamLeakParticles1 = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles2, ref variant8))
			{
				this._steamLeakParticles2 = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles3, ref variant9))
			{
				this._steamLeakParticles3 = variant9.As<GpuParticles2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._mistParticles, ref variant10))
			{
				this._mistParticles = variant10.As<GpuParticles2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._mouthParticles, ref variant11))
			{
				this._mouthParticles = variant11.As<GpuParticles2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._dropletParticles, ref variant12))
			{
				this._dropletParticles = variant12.As<GpuParticles2D>();
			}
			Variant variant13;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat1, ref variant13))
			{
				this._leakProcMat1 = variant13.As<ParticleProcessMaterial>();
			}
			Variant variant14;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat2, ref variant14))
			{
				this._leakProcMat2 = variant14.As<ParticleProcessMaterial>();
			}
			Variant variant15;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat3, ref variant15))
			{
				this._leakProcMat3 = variant15.As<ParticleProcessMaterial>();
			}
			Variant variant16;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._isDead, ref variant16))
			{
				this._isDead = variant16.As<bool>();
			}
			Variant variant17;
			if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._parent, ref variant17))
			{
				this._parent = variant17.As<Node2D>();
			}
		}

		// Token: 0x04000A1C RID: 2588
		private GpuParticles2D _steam1Particles;

		// Token: 0x04000A1D RID: 2589
		private GpuParticles2D _steam2Particles;

		// Token: 0x04000A1E RID: 2590
		private GpuParticles2D _steam3Particles;

		// Token: 0x04000A1F RID: 2591
		private GpuParticles2D _steam4Particles;

		// Token: 0x04000A20 RID: 2592
		private GpuParticles2D _steam5Particles;

		// Token: 0x04000A21 RID: 2593
		private GpuParticles2D _steam6Particles;

		// Token: 0x04000A22 RID: 2594
		private GpuParticles2D _steamLeakParticles1;

		// Token: 0x04000A23 RID: 2595
		private GpuParticles2D _steamLeakParticles2;

		// Token: 0x04000A24 RID: 2596
		private GpuParticles2D _steamLeakParticles3;

		// Token: 0x04000A25 RID: 2597
		private GpuParticles2D _mistParticles;

		// Token: 0x04000A26 RID: 2598
		private GpuParticles2D _mouthParticles;

		// Token: 0x04000A27 RID: 2599
		private GpuParticles2D _dropletParticles;

		// Token: 0x04000A28 RID: 2600
		private ParticleProcessMaterial _leakProcMat1;

		// Token: 0x04000A29 RID: 2601
		private ParticleProcessMaterial _leakProcMat2;

		// Token: 0x04000A2A RID: 2602
		private ParticleProcessMaterial _leakProcMat3;

		// Token: 0x04000A2B RID: 2603
		private bool _isDead;

		// Token: 0x04000A2C RID: 2604
		private Node2D _parent;

		// Token: 0x04000A2D RID: 2605
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A0 RID: 4512
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040044A1 RID: 17569
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040044A2 RID: 17570
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartSteam1' method.
			/// </summary>
			// Token: 0x040044A3 RID: 17571
			public static readonly StringName StartSteam1 = "StartSteam1";

			/// <summary>
			/// Cached name for the 'EndSteam1' method.
			/// </summary>
			// Token: 0x040044A4 RID: 17572
			public static readonly StringName EndSteam1 = "EndSteam1";

			/// <summary>
			/// Cached name for the 'StartSteam2' method.
			/// </summary>
			// Token: 0x040044A5 RID: 17573
			public static readonly StringName StartSteam2 = "StartSteam2";

			/// <summary>
			/// Cached name for the 'EndSteam2' method.
			/// </summary>
			// Token: 0x040044A6 RID: 17574
			public static readonly StringName EndSteam2 = "EndSteam2";

			/// <summary>
			/// Cached name for the 'StartSteam3' method.
			/// </summary>
			// Token: 0x040044A7 RID: 17575
			public static readonly StringName StartSteam3 = "StartSteam3";

			/// <summary>
			/// Cached name for the 'EndSteam3' method.
			/// </summary>
			// Token: 0x040044A8 RID: 17576
			public static readonly StringName EndSteam3 = "EndSteam3";

			/// <summary>
			/// Cached name for the 'StartSteam5' method.
			/// </summary>
			// Token: 0x040044A9 RID: 17577
			public static readonly StringName StartSteam5 = "StartSteam5";

			/// <summary>
			/// Cached name for the 'EndSteam5' method.
			/// </summary>
			// Token: 0x040044AA RID: 17578
			public static readonly StringName EndSteam5 = "EndSteam5";

			/// <summary>
			/// Cached name for the 'StartWaterfall' method.
			/// </summary>
			// Token: 0x040044AB RID: 17579
			public static readonly StringName StartWaterfall = "StartWaterfall";

			/// <summary>
			/// Cached name for the 'EndWaterfall' method.
			/// </summary>
			// Token: 0x040044AC RID: 17580
			public static readonly StringName EndWaterfall = "EndWaterfall";

			/// <summary>
			/// Cached name for the 'Explode' method.
			/// </summary>
			// Token: 0x040044AD RID: 17581
			public static readonly StringName Explode = "Explode";

			/// <summary>
			/// Cached name for the 'Buildup1' method.
			/// </summary>
			// Token: 0x040044AE RID: 17582
			public static readonly StringName Buildup1 = "Buildup1";

			/// <summary>
			/// Cached name for the 'Buildup2' method.
			/// </summary>
			// Token: 0x040044AF RID: 17583
			public static readonly StringName Buildup2 = "Buildup2";

			/// <summary>
			/// Cached name for the 'Buildup3' method.
			/// </summary>
			// Token: 0x040044B0 RID: 17584
			public static readonly StringName Buildup3 = "Buildup3";

			/// <summary>
			/// Cached name for the 'ClearDeathSteam' method.
			/// </summary>
			// Token: 0x040044B1 RID: 17585
			public static readonly StringName ClearDeathSteam = "ClearDeathSteam";

			/// <summary>
			/// Cached name for the 'EmitGracefully' method.
			/// </summary>
			// Token: 0x040044B2 RID: 17586
			public static readonly StringName EmitGracefully = "EmitGracefully";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A1 RID: 4513
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_steam1Particles' field.
			/// </summary>
			// Token: 0x040044B3 RID: 17587
			public static readonly StringName _steam1Particles = "_steam1Particles";

			/// <summary>
			/// Cached name for the '_steam2Particles' field.
			/// </summary>
			// Token: 0x040044B4 RID: 17588
			public static readonly StringName _steam2Particles = "_steam2Particles";

			/// <summary>
			/// Cached name for the '_steam3Particles' field.
			/// </summary>
			// Token: 0x040044B5 RID: 17589
			public static readonly StringName _steam3Particles = "_steam3Particles";

			/// <summary>
			/// Cached name for the '_steam4Particles' field.
			/// </summary>
			// Token: 0x040044B6 RID: 17590
			public static readonly StringName _steam4Particles = "_steam4Particles";

			/// <summary>
			/// Cached name for the '_steam5Particles' field.
			/// </summary>
			// Token: 0x040044B7 RID: 17591
			public static readonly StringName _steam5Particles = "_steam5Particles";

			/// <summary>
			/// Cached name for the '_steam6Particles' field.
			/// </summary>
			// Token: 0x040044B8 RID: 17592
			public static readonly StringName _steam6Particles = "_steam6Particles";

			/// <summary>
			/// Cached name for the '_steamLeakParticles1' field.
			/// </summary>
			// Token: 0x040044B9 RID: 17593
			public static readonly StringName _steamLeakParticles1 = "_steamLeakParticles1";

			/// <summary>
			/// Cached name for the '_steamLeakParticles2' field.
			/// </summary>
			// Token: 0x040044BA RID: 17594
			public static readonly StringName _steamLeakParticles2 = "_steamLeakParticles2";

			/// <summary>
			/// Cached name for the '_steamLeakParticles3' field.
			/// </summary>
			// Token: 0x040044BB RID: 17595
			public static readonly StringName _steamLeakParticles3 = "_steamLeakParticles3";

			/// <summary>
			/// Cached name for the '_mistParticles' field.
			/// </summary>
			// Token: 0x040044BC RID: 17596
			public static readonly StringName _mistParticles = "_mistParticles";

			/// <summary>
			/// Cached name for the '_mouthParticles' field.
			/// </summary>
			// Token: 0x040044BD RID: 17597
			public static readonly StringName _mouthParticles = "_mouthParticles";

			/// <summary>
			/// Cached name for the '_dropletParticles' field.
			/// </summary>
			// Token: 0x040044BE RID: 17598
			public static readonly StringName _dropletParticles = "_dropletParticles";

			/// <summary>
			/// Cached name for the '_leakProcMat1' field.
			/// </summary>
			// Token: 0x040044BF RID: 17599
			public static readonly StringName _leakProcMat1 = "_leakProcMat1";

			/// <summary>
			/// Cached name for the '_leakProcMat2' field.
			/// </summary>
			// Token: 0x040044C0 RID: 17600
			public static readonly StringName _leakProcMat2 = "_leakProcMat2";

			/// <summary>
			/// Cached name for the '_leakProcMat3' field.
			/// </summary>
			// Token: 0x040044C1 RID: 17601
			public static readonly StringName _leakProcMat3 = "_leakProcMat3";

			/// <summary>
			/// Cached name for the '_isDead' field.
			/// </summary>
			// Token: 0x040044C2 RID: 17602
			public static readonly StringName _isDead = "_isDead";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040044C3 RID: 17603
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A2 RID: 4514
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
