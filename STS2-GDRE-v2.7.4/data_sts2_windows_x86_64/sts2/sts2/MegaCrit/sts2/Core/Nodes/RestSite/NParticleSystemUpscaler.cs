using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.RestSite
{
	/// <summary>
	/// A utility for particle systems whose textures can get downscaled
	/// Scales up the particle system so that it matches the original texture's size.
	/// Used mainly for the ground lighting in rest sites.
	/// </summary>
	// Token: 0x02000373 RID: 883
	[ScriptPath("res://src/Core/Nodes/RestSite/NParticleSystemUpscaler.cs")]
	public class NParticleSystemUpscaler : CpuParticles2D
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x00183C00 File Offset: 0x00181E00
		public override void _Ready()
		{
			Texture2D texture = base.Texture;
			Vector2 size = texture.GetSize();
			float num = this._originalResolution.X / size.X;
			base.ScaleAmountMin *= num;
			base.ScaleAmountMax = base.ScaleAmountMin;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600340C RID: 13324 RVA: 0x00183C48 File Offset: 0x00181E48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NParticleSystemUpscaler.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600340D RID: 13325 RVA: 0x00183C8C File Offset: 0x00181E8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NParticleSystemUpscaler.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600340E RID: 13326 RVA: 0x00183CBC File Offset: 0x00181EBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NParticleSystemUpscaler.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600340F RID: 13327 RVA: 0x00183CD4 File Offset: 0x00181ED4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NParticleSystemUpscaler.PropertyName._originalResolution)
			{
				this._originalResolution = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003410 RID: 13328 RVA: 0x00183CF9 File Offset: 0x00181EF9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NParticleSystemUpscaler.PropertyName._originalResolution)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalResolution);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003411 RID: 13329 RVA: 0x00183D24 File Offset: 0x00181F24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NParticleSystemUpscaler.PropertyName._originalResolution, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003412 RID: 13330 RVA: 0x00183D58 File Offset: 0x00181F58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NParticleSystemUpscaler.PropertyName._originalResolution, Variant.From<Vector2>(ref this._originalResolution));
		}

		/// <inheritdoc />
		// Token: 0x06003413 RID: 13331 RVA: 0x00183D78 File Offset: 0x00181F78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NParticleSystemUpscaler.PropertyName._originalResolution, ref variant))
			{
				this._originalResolution = variant.As<Vector2>();
			}
		}

		// Token: 0x0400142D RID: 5165
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _originalResolution;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B5 RID: 5813
		public class MethodName : CpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005880 RID: 22656
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B6 RID: 5814
		public class PropertyName : CpuParticles2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_originalResolution' field.
			/// </summary>
			// Token: 0x04005881 RID: 22657
			public static readonly StringName _originalResolution = "_originalResolution";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B7 RID: 5815
		public class SignalName : CpuParticles2D.SignalName
		{
		}
	}
}
