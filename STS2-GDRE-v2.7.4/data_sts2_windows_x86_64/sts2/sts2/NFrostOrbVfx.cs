using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;

// Token: 0x02000005 RID: 5
[ScriptPath("res://src/Core/Nodes/Orbs/NFrostOrbVfx.cs")]
public class NFrostOrbVfx : NOrbVfx
{
	// Token: 0x0600001E RID: 30 RVA: 0x00003099 File Offset: 0x00001299
	public override void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		base.OnPassiveActivated(passiveVal, evokeVal);
		base.ShakeOrb(1f, 0.5f);
		VfxCmd.PlayVfx(base.GetPlayerVfxPosition(), "vfx/orbs/frost/vfx_frost_orb_passive_shield", base.VfxContainer);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000030C9 File Offset: 0x000012C9
	protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
	{
		base.OnEvokeInternal(targetVfxSpawnPosition);
		VfxCmd.PlayVfx(base.GetPlayerVfxPosition(), "vfx/orbs/frost/vfx_frost_orb_evoke_shield", base.VfxContainer);
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000020 RID: 32 RVA: 0x000030E8 File Offset: 0x000012E8
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(NFrostOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "targetVfxSpawnPosition", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000021 RID: 33 RVA: 0x00003151 File Offset: 0x00001351
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NFrostOrbVfx.MethodName.OnEvokeInternal && args.Count == 1)
		{
			this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	/// <inheritdoc />
	// Token: 0x06000022 RID: 34 RVA: 0x0000318F File Offset: 0x0000138F
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NFrostOrbVfx.MethodName.OnEvokeInternal || base.HasGodotClassMethod(in method);
	}

	/// <inheritdoc />
	// Token: 0x06000023 RID: 35 RVA: 0x000031A7 File Offset: 0x000013A7
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	/// <inheritdoc />
	// Token: 0x06000024 RID: 36 RVA: 0x000031B0 File Offset: 0x000013B0
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB1 RID: 3505
	public new class MethodName : NOrbVfx.MethodName
	{
		/// <summary>
		/// Cached name for the 'OnEvokeInternal' method.
		/// </summary>
		// Token: 0x0400315A RID: 12634
		public new static readonly StringName OnEvokeInternal = "OnEvokeInternal";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB2 RID: 3506
	public new class PropertyName : NOrbVfx.PropertyName
	{
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB3 RID: 3507
	public new class SignalName : NOrbVfx.SignalName
	{
	}
}
