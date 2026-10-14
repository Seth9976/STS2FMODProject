using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;

// Token: 0x02000006 RID: 6
[ScriptPath("res://src/Core/Nodes/Orbs/NGlassOrbVfx.cs")]
public class NGlassOrbVfx : NOrbVfx
{
	// Token: 0x06000026 RID: 38 RVA: 0x000031C4 File Offset: 0x000013C4
	public override void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		if (passiveVal <= 0m)
		{
			return;
		}
		base.OnPassiveActivated(passiveVal, evokeVal);
		float num = (float)(passiveVal / this._basePassiveVal);
		this._passiveChromaticAberration.Set(NGlassOrbVfx._aberrationStrengthString, Mathf.Lerp(0f, this._basePassiveChromaticAberrationStength, num));
		base.ShakeOrb(num, 0.5f);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000322C File Offset: 0x0000142C
	public override void AfterPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		base.AfterPassiveActivated(passiveVal, evokeVal);
		this.UpdateFocusPowerState();
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000323C File Offset: 0x0000143C
	protected override bool HasFocusPower()
	{
		return (this._orbModel == null || !(this._orbModel.PassiveVal <= 0m)) && base.HasFocusPower();
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003268 File Offset: 0x00001468
	[NullableContext(1)]
	public void ShowPassiveImpact(Vector2[] targetVfxSpawnPositions)
	{
		for (int i = 0; i < targetVfxSpawnPositions.Length; i++)
		{
			this.ShowPassiveImpact(targetVfxSpawnPositions[i]);
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003290 File Offset: 0x00001490
	private void ShowPassiveImpact(Vector2 targetVfxSpawnPosition)
	{
		VfxCmd.PlayVfx(targetVfxSpawnPosition, "vfx/orbs/glass/vfx_glass_orb_passive_impact", base.VfxContainer);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000032A3 File Offset: 0x000014A3
	protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
	{
		base.OnEvokeInternal(targetVfxSpawnPosition);
		VfxCmd.PlayVfx(targetVfxSpawnPosition, "vfx/orbs/glass/vfx_glass_orb_evoke_impact", base.VfxContainer);
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600002C RID: 44 RVA: 0x000032C0 File Offset: 0x000014C0
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(NGlassOrbVfx.MethodName.HasFocusPower, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NGlassOrbVfx.MethodName.ShowPassiveImpact, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(35L, "targetVfxSpawnPositions", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NGlassOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "targetVfxSpawnPosition", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600002D RID: 45 RVA: 0x000033B0 File Offset: 0x000015B0
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NGlassOrbVfx.MethodName.HasFocusPower && args.Count == 0)
		{
			bool flag = this.HasFocusPower();
			ret = VariantUtils.CreateFrom<bool>(ref flag);
			return true;
		}
		if ((ref method) == NGlassOrbVfx.MethodName.ShowPassiveImpact && args.Count == 1)
		{
			this.ShowPassiveImpact(VariantUtils.ConvertTo<Vector2[]>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NGlassOrbVfx.MethodName.OnEvokeInternal && args.Count == 1)
		{
			this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	/// <inheritdoc />
	// Token: 0x0600002E RID: 46 RVA: 0x00003458 File Offset: 0x00001658
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NGlassOrbVfx.MethodName.HasFocusPower || (ref method) == NGlassOrbVfx.MethodName.ShowPassiveImpact || (ref method) == NGlassOrbVfx.MethodName.OnEvokeInternal || base.HasGodotClassMethod(in method);
	}

	/// <inheritdoc />
	// Token: 0x0600002F RID: 47 RVA: 0x0000348E File Offset: 0x0000168E
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NGlassOrbVfx.PropertyName._passiveChromaticAberration)
		{
			this._passiveChromaticAberration = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
			return true;
		}
		if ((ref name) == NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength)
		{
			this._basePassiveChromaticAberrationStength = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	/// <inheritdoc />
	// Token: 0x06000030 RID: 48 RVA: 0x000034D0 File Offset: 0x000016D0
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NGlassOrbVfx.PropertyName._passiveChromaticAberration)
		{
			value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._passiveChromaticAberration);
			return true;
		}
		if ((ref name) == NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength)
		{
			value = VariantUtils.CreateFrom<float>(ref this._basePassiveChromaticAberrationStength);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000031 RID: 49 RVA: 0x00003528 File Offset: 0x00001728
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NGlassOrbVfx.PropertyName._passiveChromaticAberration, 34L, "GPUParticles2D", 4102L, true),
			new PropertyInfo(3L, NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, 0L, "", 4102L, true)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000032 RID: 50 RVA: 0x0000357E File Offset: 0x0000177E
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NGlassOrbVfx.PropertyName._passiveChromaticAberration, Variant.From<GpuParticles2D>(ref this._passiveChromaticAberration));
		info.AddProperty(NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, Variant.From<float>(ref this._basePassiveChromaticAberrationStength));
	}

	/// <inheritdoc />
	// Token: 0x06000033 RID: 51 RVA: 0x000035B4 File Offset: 0x000017B4
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NGlassOrbVfx.PropertyName._passiveChromaticAberration, ref variant))
		{
			this._passiveChromaticAberration = variant.As<GpuParticles2D>();
		}
		Variant variant2;
		if (info.TryGetProperty(NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, ref variant2))
		{
			this._basePassiveChromaticAberrationStength = variant2.As<float>();
		}
	}

	// Token: 0x04000013 RID: 19
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private GpuParticles2D _passiveChromaticAberration;

	// Token: 0x04000014 RID: 20
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _basePassiveChromaticAberrationStength = 0.01f;

	// Token: 0x04000015 RID: 21
	private decimal _basePassiveVal = 4m;

	// Token: 0x04000016 RID: 22
	[Nullable(1)]
	private static readonly StringName _aberrationStrengthString = new StringName("instance_shader_parameters/base_intensity");

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB4 RID: 3508
	public new class MethodName : NOrbVfx.MethodName
	{
		/// <summary>
		/// Cached name for the 'HasFocusPower' method.
		/// </summary>
		// Token: 0x0400315B RID: 12635
		public new static readonly StringName HasFocusPower = "HasFocusPower";

		/// <summary>
		/// Cached name for the 'ShowPassiveImpact' method.
		/// </summary>
		// Token: 0x0400315C RID: 12636
		public static readonly StringName ShowPassiveImpact = "ShowPassiveImpact";

		/// <summary>
		/// Cached name for the 'OnEvokeInternal' method.
		/// </summary>
		// Token: 0x0400315D RID: 12637
		public new static readonly StringName OnEvokeInternal = "OnEvokeInternal";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB5 RID: 3509
	public new class PropertyName : NOrbVfx.PropertyName
	{
		/// <summary>
		/// Cached name for the '_passiveChromaticAberration' field.
		/// </summary>
		// Token: 0x0400315E RID: 12638
		public static readonly StringName _passiveChromaticAberration = "_passiveChromaticAberration";

		/// <summary>
		/// Cached name for the '_basePassiveChromaticAberrationStength' field.
		/// </summary>
		// Token: 0x0400315F RID: 12639
		public static readonly StringName _basePassiveChromaticAberrationStength = "_basePassiveChromaticAberrationStength";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB6 RID: 3510
	public new class SignalName : NOrbVfx.SignalName
	{
	}
}
