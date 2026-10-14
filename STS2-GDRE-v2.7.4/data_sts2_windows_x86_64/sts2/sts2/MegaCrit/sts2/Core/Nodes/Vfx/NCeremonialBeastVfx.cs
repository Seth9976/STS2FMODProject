using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000195 RID: 405
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCeremonialBeastVfx.cs")]
	public class NCeremonialBeastVfx : Node, IDeathDelayer
	{
		// Token: 0x060013D4 RID: 5076 RVA: 0x0007774C File Offset: 0x0007594C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._deathParticles.OneShot = true;
			this._deathParticles.Emitting = false;
			this._energyParticlesBack.Emitting = true;
			this._energyParticlesFront.Emitting = true;
			this._globalPlowTarget = this._plowStartTarget.GlobalPosition;
			this._globalPlowEndTarget = this._plowEndTarget.GlobalPosition;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000777EC File Offset: 0x000759EC
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "turnOffEnergy")
			{
				this.TurnOffEnergyParticles();
				return;
			}
			if (eventName == "turnOnEnergy")
			{
				this.TurnOnEnergyParticles();
				return;
			}
			if (eventName == "deathParticles")
			{
				this.TurnOnDeathParticles();
				return;
			}
			if (eventName == "plowStart")
			{
				this.OnPlowStart();
				return;
			}
			if (!(eventName == "plowEnd"))
			{
				return;
			}
			this.OnPlowEnd();
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00077874 File Offset: 0x00075A74
		public Task GetDelayTask()
		{
			return this._deathTask.Task;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00077881 File Offset: 0x00075A81
		private void TurnOnDeathParticles()
		{
			this._deathParticles.Restart();
			TaskHelper.RunSafely(this.FinishTaskWhenDeathParticlesFinished());
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0007789C File Offset: 0x00075A9C
		private async Task FinishTaskWhenDeathParticlesFinished()
		{
			await this._deathParticles.AwaitSignal(CpuParticles2D.SignalName.Finished, this);
			this._deathTask.SetResult();
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000778DF File Offset: 0x00075ADF
		private void TurnOnEnergyParticles()
		{
			this._energyParticlesFront.Emitting = true;
			this._energyParticlesBack.Emitting = true;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000778F9 File Offset: 0x00075AF9
		private void TurnOffEnergyParticles()
		{
			this._energyParticlesFront.Emitting = false;
			this._energyParticlesBack.Emitting = false;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00077913 File Offset: 0x00075B13
		private void OnPlowStart()
		{
			this._plowStartTarget.GlobalPosition = this._globalPlowTarget;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00077926 File Offset: 0x00075B26
		private void OnPlowEnd()
		{
			this._plowEndTarget.GlobalPosition = this._globalPlowEndTarget;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013DD RID: 5085 RVA: 0x0007793C File Offset: 0x00075B3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NCeremonialBeastVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOnDeathParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.OnPlowStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastVfx.MethodName.OnPlowEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013DE RID: 5086 RVA: 0x00077B50 File Offset: 0x00075D50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCeremonialBeastVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.TurnOnDeathParticles && args.Count == 0)
			{
				this.TurnOnDeathParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles && args.Count == 0)
			{
				this.TurnOnEnergyParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles && args.Count == 0)
			{
				this.TurnOffEnergyParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.OnPlowStart && args.Count == 0)
			{
				this.OnPlowStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastVfx.MethodName.OnPlowEnd && args.Count == 0)
			{
				this.OnPlowEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060013DF RID: 5087 RVA: 0x00077CA0 File Offset: 0x00075EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCeremonialBeastVfx.MethodName._Ready || (ref method) == NCeremonialBeastVfx.MethodName.OnAnimationEvent || (ref method) == NCeremonialBeastVfx.MethodName.TurnOnDeathParticles || (ref method) == NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles || (ref method) == NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles || (ref method) == NCeremonialBeastVfx.MethodName.OnPlowStart || (ref method) == NCeremonialBeastVfx.MethodName.OnPlowEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013E0 RID: 5088 RVA: 0x00077D20 File Offset: 0x00075F20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCeremonialBeastVfx.PropertyName._deathParticles)
			{
				this._deathParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._energyParticlesFront)
			{
				this._energyParticlesFront = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._energyParticlesBack)
			{
				this._energyParticlesBack = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._plowStartTarget)
			{
				this._plowStartTarget = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._plowEndTarget)
			{
				this._plowEndTarget = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._globalPlowTarget)
			{
				this._globalPlowTarget = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._globalPlowEndTarget)
			{
				this._globalPlowEndTarget = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013E1 RID: 5089 RVA: 0x00077E10 File Offset: 0x00076010
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCeremonialBeastVfx.PropertyName._deathParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticles);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._energyParticlesFront)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._energyParticlesFront);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._energyParticlesBack)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._energyParticlesBack);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._plowStartTarget)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._plowStartTarget);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._plowEndTarget)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._plowEndTarget);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._globalPlowTarget)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._globalPlowTarget);
				return true;
			}
			if ((ref name) == NCeremonialBeastVfx.PropertyName._globalPlowEndTarget)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._globalPlowEndTarget);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013E2 RID: 5090 RVA: 0x00077F28 File Offset: 0x00076128
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._deathParticles, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._energyParticlesFront, 34L, "CPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._energyParticlesBack, 34L, "CPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._plowStartTarget, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._plowEndTarget, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCeremonialBeastVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCeremonialBeastVfx.PropertyName._globalPlowTarget, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013E3 RID: 5091 RVA: 0x00078048 File Offset: 0x00076248
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCeremonialBeastVfx.PropertyName._deathParticles, Variant.From<GpuParticles2D>(ref this._deathParticles));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._energyParticlesFront, Variant.From<CpuParticles2D>(ref this._energyParticlesFront));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._energyParticlesBack, Variant.From<CpuParticles2D>(ref this._energyParticlesBack));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._plowStartTarget, Variant.From<Node2D>(ref this._plowStartTarget));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._plowEndTarget, Variant.From<Node2D>(ref this._plowEndTarget));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._globalPlowTarget, Variant.From<Vector2>(ref this._globalPlowTarget));
			info.AddProperty(NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, Variant.From<Vector2>(ref this._globalPlowEndTarget));
		}

		/// <inheritdoc />
		// Token: 0x060013E4 RID: 5092 RVA: 0x0007810C File Offset: 0x0007630C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._deathParticles, ref variant))
			{
				this._deathParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._energyParticlesFront, ref variant2))
			{
				this._energyParticlesFront = variant2.As<CpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._energyParticlesBack, ref variant3))
			{
				this._energyParticlesBack = variant3.As<CpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._plowStartTarget, ref variant4))
			{
				this._plowStartTarget = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._plowEndTarget, ref variant5))
			{
				this._plowEndTarget = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._parent, ref variant6))
			{
				this._parent = variant6.As<Node2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._globalPlowTarget, ref variant7))
			{
				this._globalPlowTarget = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, ref variant8))
			{
				this._globalPlowEndTarget = variant8.As<Vector2>();
			}
		}

		// Token: 0x04000715 RID: 1813
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _deathParticles;

		// Token: 0x04000716 RID: 1814
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CpuParticles2D _energyParticlesFront;

		// Token: 0x04000717 RID: 1815
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CpuParticles2D _energyParticlesBack;

		// Token: 0x04000718 RID: 1816
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _plowStartTarget;

		// Token: 0x04000719 RID: 1817
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _plowEndTarget;

		// Token: 0x0400071A RID: 1818
		private Node2D _parent;

		// Token: 0x0400071B RID: 1819
		private MegaSprite _animController;

		// Token: 0x0400071C RID: 1820
		private Vector2 _globalPlowTarget;

		// Token: 0x0400071D RID: 1821
		private Vector2 _globalPlowEndTarget;

		// Token: 0x0400071E RID: 1822
		private readonly TaskCompletionSource _deathTask = new TaskCompletionSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FDF RID: 4063
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F22 RID: 16162
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003F23 RID: 16163
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'TurnOnDeathParticles' method.
			/// </summary>
			// Token: 0x04003F24 RID: 16164
			public static readonly StringName TurnOnDeathParticles = "TurnOnDeathParticles";

			/// <summary>
			/// Cached name for the 'TurnOnEnergyParticles' method.
			/// </summary>
			// Token: 0x04003F25 RID: 16165
			public static readonly StringName TurnOnEnergyParticles = "TurnOnEnergyParticles";

			/// <summary>
			/// Cached name for the 'TurnOffEnergyParticles' method.
			/// </summary>
			// Token: 0x04003F26 RID: 16166
			public static readonly StringName TurnOffEnergyParticles = "TurnOffEnergyParticles";

			/// <summary>
			/// Cached name for the 'OnPlowStart' method.
			/// </summary>
			// Token: 0x04003F27 RID: 16167
			public static readonly StringName OnPlowStart = "OnPlowStart";

			/// <summary>
			/// Cached name for the 'OnPlowEnd' method.
			/// </summary>
			// Token: 0x04003F28 RID: 16168
			public static readonly StringName OnPlowEnd = "OnPlowEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE0 RID: 4064
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_deathParticles' field.
			/// </summary>
			// Token: 0x04003F29 RID: 16169
			public static readonly StringName _deathParticles = "_deathParticles";

			/// <summary>
			/// Cached name for the '_energyParticlesFront' field.
			/// </summary>
			// Token: 0x04003F2A RID: 16170
			public static readonly StringName _energyParticlesFront = "_energyParticlesFront";

			/// <summary>
			/// Cached name for the '_energyParticlesBack' field.
			/// </summary>
			// Token: 0x04003F2B RID: 16171
			public static readonly StringName _energyParticlesBack = "_energyParticlesBack";

			/// <summary>
			/// Cached name for the '_plowStartTarget' field.
			/// </summary>
			// Token: 0x04003F2C RID: 16172
			public static readonly StringName _plowStartTarget = "_plowStartTarget";

			/// <summary>
			/// Cached name for the '_plowEndTarget' field.
			/// </summary>
			// Token: 0x04003F2D RID: 16173
			public static readonly StringName _plowEndTarget = "_plowEndTarget";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003F2E RID: 16174
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_globalPlowTarget' field.
			/// </summary>
			// Token: 0x04003F2F RID: 16175
			public static readonly StringName _globalPlowTarget = "_globalPlowTarget";

			/// <summary>
			/// Cached name for the '_globalPlowEndTarget' field.
			/// </summary>
			// Token: 0x04003F30 RID: 16176
			public static readonly StringName _globalPlowEndTarget = "_globalPlowEndTarget";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE1 RID: 4065
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
