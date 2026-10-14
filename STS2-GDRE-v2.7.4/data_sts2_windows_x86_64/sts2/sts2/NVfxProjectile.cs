using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

// Token: 0x0200000C RID: 12
[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxProjectile.cs")]
public class NVfxProjectile : Node2D
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000087 RID: 135 RVA: 0x00005F78 File Offset: 0x00004178
	public bool AlignToVelocity
	{
		get
		{
			return this._alignToVelocity;
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00005F80 File Offset: 0x00004180
	public void SetEmitting(bool emitting)
	{
		if (this._projectileHead != null)
		{
			this._projectileHead.Visible = emitting;
		}
		for (int i = 0; i < this._particles.Length; i++)
		{
			this._particles[i].Emitting = emitting;
		}
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000089 RID: 137 RVA: 0x00005FC4 File Offset: 0x000041C4
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(NVfxProjectile.MethodName.SetEmitting, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(1L, "emitting", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600008A RID: 138 RVA: 0x0000602D File Offset: 0x0000422D
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NVfxProjectile.MethodName.SetEmitting && args.Count == 1)
		{
			this.SetEmitting(VariantUtils.ConvertTo<bool>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(ref method, args, ref ret);
	}

	/// <inheritdoc />
	// Token: 0x0600008B RID: 139 RVA: 0x0000606B File Offset: 0x0000426B
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NVfxProjectile.MethodName.SetEmitting || base.HasGodotClassMethod(ref method);
	}

	/// <inheritdoc />
	// Token: 0x0600008C RID: 140 RVA: 0x00006084 File Offset: 0x00004284
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NVfxProjectile.PropertyName._projectileHead)
		{
			this._projectileHead = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NVfxProjectile.PropertyName._particles)
		{
			this._particles = VariantUtils.ConvertToSystemArrayOfGodotObject<GpuParticles2D>(ref value);
			return true;
		}
		if ((ref name) == NVfxProjectile.PropertyName._alignToVelocity)
		{
			this._alignToVelocity = VariantUtils.ConvertTo<bool>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(ref name, ref value);
	}

	/// <inheritdoc />
	// Token: 0x0600008D RID: 141 RVA: 0x000060EC File Offset: 0x000042EC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NVfxProjectile.PropertyName.AlignToVelocity)
		{
			bool alignToVelocity = this.AlignToVelocity;
			value = VariantUtils.CreateFrom<bool>(ref alignToVelocity);
			return true;
		}
		if ((ref name) == NVfxProjectile.PropertyName._projectileHead)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._projectileHead);
			return true;
		}
		if ((ref name) == NVfxProjectile.PropertyName._particles)
		{
			GodotObject[] particles = this._particles;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(particles);
			return true;
		}
		if ((ref name) == NVfxProjectile.PropertyName._alignToVelocity)
		{
			value = VariantUtils.CreateFrom<bool>(ref this._alignToVelocity);
			return true;
		}
		return base.GetGodotClassPropertyValue(ref name, ref value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600008E RID: 142 RVA: 0x00006188 File Offset: 0x00004388
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NVfxProjectile.PropertyName._projectileHead, 34L, "Node2D", 4102L, true),
			new PropertyInfo(28L, NVfxProjectile.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
			new PropertyInfo(1L, NVfxProjectile.PropertyName._alignToVelocity, 0L, "", 4102L, true),
			new PropertyInfo(1L, NVfxProjectile.PropertyName.AlignToVelocity, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600008F RID: 143 RVA: 0x00006220 File Offset: 0x00004420
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NVfxProjectile.PropertyName._projectileHead, Variant.From<Node2D>(ref this._projectileHead));
		StringName particles = NVfxProjectile.PropertyName._particles;
		GodotObject[] particles2 = this._particles;
		info.AddProperty(particles, Variant.CreateFrom(particles2));
		info.AddProperty(NVfxProjectile.PropertyName._alignToVelocity, Variant.From<bool>(ref this._alignToVelocity));
	}

	/// <inheritdoc />
	// Token: 0x06000090 RID: 144 RVA: 0x00006278 File Offset: 0x00004478
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NVfxProjectile.PropertyName._projectileHead, ref variant))
		{
			this._projectileHead = variant.As<Node2D>();
		}
		Variant variant2;
		if (info.TryGetProperty(NVfxProjectile.PropertyName._particles, ref variant2))
		{
			this._particles = variant2.AsGodotObjectArray<GpuParticles2D>();
		}
		Variant variant3;
		if (info.TryGetProperty(NVfxProjectile.PropertyName._alignToVelocity, ref variant3))
		{
			this._alignToVelocity = variant3.As<bool>();
		}
	}

	// Token: 0x04000034 RID: 52
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Node2D _projectileHead;

	// Token: 0x04000035 RID: 53
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private GpuParticles2D[] _particles;

	// Token: 0x04000036 RID: 54
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private bool _alignToVelocity;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC8 RID: 3528
	public class MethodName : Node2D.MethodName
	{
		/// <summary>
		/// Cached name for the 'SetEmitting' method.
		/// </summary>
		// Token: 0x0400319F RID: 12703
		public static readonly StringName SetEmitting = "SetEmitting";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC9 RID: 3529
	public class PropertyName : Node2D.PropertyName
	{
		/// <summary>
		/// Cached name for the 'AlignToVelocity' property.
		/// </summary>
		// Token: 0x040031A0 RID: 12704
		public static readonly StringName AlignToVelocity = "AlignToVelocity";

		/// <summary>
		/// Cached name for the '_projectileHead' field.
		/// </summary>
		// Token: 0x040031A1 RID: 12705
		public static readonly StringName _projectileHead = "_projectileHead";

		/// <summary>
		/// Cached name for the '_particles' field.
		/// </summary>
		// Token: 0x040031A2 RID: 12706
		public static readonly StringName _particles = "_particles";

		/// <summary>
		/// Cached name for the '_alignToVelocity' field.
		/// </summary>
		// Token: 0x040031A3 RID: 12707
		public static readonly StringName _alignToVelocity = "_alignToVelocity";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DCA RID: 3530
	public class SignalName : Node2D.SignalName
	{
	}
}
