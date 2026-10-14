using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000219 RID: 537
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NParticlesContainer.cs")]
	public class NParticlesContainer : Node2D
	{
		// Token: 0x06001B7A RID: 7034 RVA: 0x000B2AC8 File Offset: 0x000B0CC8
		public void SetEmitting(bool emitting)
		{
			for (int i = 0; i < this._particles.Count; i++)
			{
				this._particles[i].Emitting = emitting;
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000B2B00 File Offset: 0x000B0D00
		public void Restart()
		{
			for (int i = 0; i < this._particles.Count; i++)
			{
				this._particles[i].Restart();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B7C RID: 7036 RVA: 0x000B2B34 File Offset: 0x000B0D34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NParticlesContainer.MethodName.SetEmitting, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "emitting", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NParticlesContainer.MethodName.Restart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B7D RID: 7037 RVA: 0x000B2BCC File Offset: 0x000B0DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NParticlesContainer.MethodName.SetEmitting && args.Count == 1)
			{
				this.SetEmitting(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParticlesContainer.MethodName.Restart && args.Count == 0)
			{
				this.Restart();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B7E RID: 7038 RVA: 0x000B2C3A File Offset: 0x000B0E3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NParticlesContainer.MethodName.SetEmitting || (ref method) == NParticlesContainer.MethodName.Restart || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B7F RID: 7039 RVA: 0x000B2C61 File Offset: 0x000B0E61
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NParticlesContainer.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B80 RID: 7040 RVA: 0x000B2C86 File Offset: 0x000B0E86
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NParticlesContainer.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B81 RID: 7041 RVA: 0x000B2CB0 File Offset: 0x000B0EB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NParticlesContainer.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B82 RID: 7042 RVA: 0x000B2CE6 File Offset: 0x000B0EE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NParticlesContainer.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
		}

		/// <inheritdoc />
		// Token: 0x06001B83 RID: 7043 RVA: 0x000B2D08 File Offset: 0x000B0F08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NParticlesContainer.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000A64 RID: 2660
		[Nullable(new byte[] { 2, 1 })]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B3 RID: 4531
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetEmitting' method.
			/// </summary>
			// Token: 0x040044F8 RID: 17656
			public static readonly StringName SetEmitting = "SetEmitting";

			/// <summary>
			/// Cached name for the 'Restart' method.
			/// </summary>
			// Token: 0x040044F9 RID: 17657
			public static readonly StringName Restart = "Restart";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B4 RID: 4532
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040044FA RID: 17658
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B5 RID: 4533
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
