using System;
using System.ComponentModel;
using Godot;
using Godot.Bridge;

// Token: 0x02000007 RID: 7
[ScriptPath("res://src/Core/Nodes/Orbs/NLightningOrbVfx.cs")]
public class NLightningOrbVfx : NOrbVfx
{
	// Token: 0x06000036 RID: 54 RVA: 0x00003630 File Offset: 0x00001830
	public override void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		base.OnPassiveActivated(passiveVal, evokeVal);
		base.ShakeOrb(1f, 0.4f);
	}

	/// <inheritdoc />
	// Token: 0x06000037 RID: 55 RVA: 0x0000364A File Offset: 0x0000184A
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	/// <inheritdoc />
	// Token: 0x06000038 RID: 56 RVA: 0x00003653 File Offset: 0x00001853
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB7 RID: 3511
	public new class MethodName : NOrbVfx.MethodName
	{
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB8 RID: 3512
	public new class PropertyName : NOrbVfx.PropertyName
	{
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB9 RID: 3513
	public new class SignalName : NOrbVfx.SignalName
	{
	}
}
