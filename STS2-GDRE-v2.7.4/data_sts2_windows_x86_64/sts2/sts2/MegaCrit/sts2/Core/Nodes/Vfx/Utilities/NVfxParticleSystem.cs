using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	/// <summary>
	/// Plays all one shot particles in a vfx scene.
	/// Destroys the Vfx after "_lifetime" seconds.
	/// </summary>
	// Token: 0x0200021D RID: 541
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxParticleSystem.cs")]
	public class NVfxParticleSystem : Node2D
	{
		// Token: 0x06001BB2 RID: 7090 RVA: 0x000B4178 File Offset: 0x000B2378
		public override void _Ready()
		{
			this.TryPlayParticles(this);
			SceneTreeTimer sceneTreeTimer = base.GetTree().CreateTimer((double)this._lifetime, true, false, false);
			sceneTreeTimer.Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(new Action(this.AfterExpired)), 0U);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000B41C0 File Offset: 0x000B23C0
		[NullableContext(1)]
		private void TryPlayParticles(Node node)
		{
			CpuParticles2D cpuParticles2D = node as CpuParticles2D;
			if (cpuParticles2D == null)
			{
				GpuParticles2D gpuParticles2D = node as GpuParticles2D;
				if (gpuParticles2D != null)
				{
					gpuParticles2D.Emitting = true;
				}
			}
			else
			{
				cpuParticles2D.Emitting = true;
			}
			foreach (Node node2 in node.GetChildren(false))
			{
				this.TryPlayParticles(node2);
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000B4234 File Offset: 0x000B2434
		private void AfterExpired()
		{
			if (GodotObject.IsInstanceValid(this))
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BB5 RID: 7093 RVA: 0x000B4244 File Offset: 0x000B2444
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NVfxParticleSystem.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVfxParticleSystem.MethodName.TryPlayParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NVfxParticleSystem.MethodName.AfterExpired, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BB6 RID: 7094 RVA: 0x000B4318 File Offset: 0x000B2518
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVfxParticleSystem.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVfxParticleSystem.MethodName.TryPlayParticles && args.Count == 1)
			{
				this.TryPlayParticles(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVfxParticleSystem.MethodName.AfterExpired && args.Count == 0)
			{
				this.AfterExpired();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001BB7 RID: 7095 RVA: 0x000B43AB File Offset: 0x000B25AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVfxParticleSystem.MethodName._Ready || (ref method) == NVfxParticleSystem.MethodName.TryPlayParticles || (ref method) == NVfxParticleSystem.MethodName.AfterExpired || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001BB8 RID: 7096 RVA: 0x000B43E1 File Offset: 0x000B25E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVfxParticleSystem.PropertyName._lifetime)
			{
				this._lifetime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BB9 RID: 7097 RVA: 0x000B4406 File Offset: 0x000B2606
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVfxParticleSystem.PropertyName._lifetime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._lifetime);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BBA RID: 7098 RVA: 0x000B4430 File Offset: 0x000B2630
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NVfxParticleSystem.PropertyName._lifetime, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BBB RID: 7099 RVA: 0x000B4464 File Offset: 0x000B2664
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NVfxParticleSystem.PropertyName._lifetime, Variant.From<float>(ref this._lifetime));
		}

		/// <inheritdoc />
		// Token: 0x06001BBC RID: 7100 RVA: 0x000B4484 File Offset: 0x000B2684
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVfxParticleSystem.PropertyName._lifetime, ref variant))
			{
				this._lifetime = variant.As<float>();
			}
		}

		// Token: 0x04000A77 RID: 2679
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _lifetime = 1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C0 RID: 4544
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400451A RID: 17690
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'TryPlayParticles' method.
			/// </summary>
			// Token: 0x0400451B RID: 17691
			public static readonly StringName TryPlayParticles = "TryPlayParticles";

			/// <summary>
			/// Cached name for the 'AfterExpired' method.
			/// </summary>
			// Token: 0x0400451C RID: 17692
			public static readonly StringName AfterExpired = "AfterExpired";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C1 RID: 4545
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_lifetime' field.
			/// </summary>
			// Token: 0x0400451D RID: 17693
			public static readonly StringName _lifetime = "_lifetime";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C2 RID: 4546
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
