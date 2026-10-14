using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;

// Token: 0x02000009 RID: 9
[ScriptPath("res://src/Core/Nodes/Orbs/NPlasmaOrbVfx.cs")]
public class NPlasmaOrbVfx : NOrbVfx
{
	// Token: 0x0600005A RID: 90 RVA: 0x0000476F File Offset: 0x0000296F
	public override void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		base.OnPassiveActivated(passiveVal, evokeVal);
		base.ShakeOrb(1f, 0.5f);
		TaskHelper.RunSafely(this.SpawnProjectile(1, base.GetPlayerVfxPosition()));
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000479C File Offset: 0x0000299C
	protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
	{
		base.OnEvokeInternal(targetVfxSpawnPosition);
		TaskHelper.RunSafely(this.SpawnProjectile(2, base.GetPlayerVfxPosition()));
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000047B8 File Offset: 0x000029B8
	[NullableContext(1)]
	private async Task SpawnProjectile(int count, Vector2 targetPosition)
	{
		for (int i = 0; i < count; i++)
		{
			string text = "vfx/orbs/plasma/vfx_plasma_orb_projectile_handler";
			string text2 = "vfx/orbs/plasma/vfx_plasma_orb_projectile";
			Vector2 vector = base.GlobalPosition + this.GetRandomOffset();
			Callable callable;
			if (i != count - 1)
			{
				callable = default(Callable);
			}
			else
			{
				callable = Callable.From(delegate
				{
				});
			}
			NVfxProjectileHandler nvfxProjectileHandler = NVfxProjectileHandler.Create(text, text2, vector, targetPosition, callable);
			base.VfxContainer.AddChildSafely(nvfxProjectileHandler);
			if (i != count - 1)
			{
				await Cmd.Wait(this._projectileSpawnInterval, false);
			}
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000480C File Offset: 0x00002A0C
	private Vector2 GetRandomOffset()
	{
		float num = Mathf.DegToRad(GD.Randf() * 360f);
		float num2 = Mathf.Lerp(this._projectileOffsetRange.X, this._projectileOffsetRange.Y, GD.Randf());
		return new Vector2(Mathf.Cos(num) * num2, Mathf.Sin(num) * num2);
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600005E RID: 94 RVA: 0x00004860 File Offset: 0x00002A60
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(NPlasmaOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "targetVfxSpawnPosition", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NPlasmaOrbVfx.MethodName.GetRandomOffset, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600005F RID: 95 RVA: 0x000048F8 File Offset: 0x00002AF8
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NPlasmaOrbVfx.MethodName.OnEvokeInternal && args.Count == 1)
		{
			this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NPlasmaOrbVfx.MethodName.GetRandomOffset && args.Count == 0)
		{
			Vector2 randomOffset = this.GetRandomOffset();
			ret = VariantUtils.CreateFrom<Vector2>(ref randomOffset);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	/// <inheritdoc />
	// Token: 0x06000060 RID: 96 RVA: 0x0000496D File Offset: 0x00002B6D
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NPlasmaOrbVfx.MethodName.OnEvokeInternal || (ref method) == NPlasmaOrbVfx.MethodName.GetRandomOffset || base.HasGodotClassMethod(in method);
	}

	/// <inheritdoc />
	// Token: 0x06000061 RID: 97 RVA: 0x00004994 File Offset: 0x00002B94
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NPlasmaOrbVfx.PropertyName._projectileOffsetRange)
		{
			this._projectileOffsetRange = VariantUtils.ConvertTo<Vector2>(ref value);
			return true;
		}
		if ((ref name) == NPlasmaOrbVfx.PropertyName._projectileSpawnInterval)
		{
			this._projectileSpawnInterval = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	/// <inheritdoc />
	// Token: 0x06000062 RID: 98 RVA: 0x000049D4 File Offset: 0x00002BD4
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NPlasmaOrbVfx.PropertyName._projectileOffsetRange)
		{
			value = VariantUtils.CreateFrom<Vector2>(ref this._projectileOffsetRange);
			return true;
		}
		if ((ref name) == NPlasmaOrbVfx.PropertyName._projectileSpawnInterval)
		{
			value = VariantUtils.CreateFrom<float>(ref this._projectileSpawnInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000063 RID: 99 RVA: 0x00004A2C File Offset: 0x00002C2C
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(5L, NPlasmaOrbVfx.PropertyName._projectileOffsetRange, 0L, "", 4102L, true),
			new PropertyInfo(3L, NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, 0L, "", 4102L, true)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000064 RID: 100 RVA: 0x00004A80 File Offset: 0x00002C80
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NPlasmaOrbVfx.PropertyName._projectileOffsetRange, Variant.From<Vector2>(ref this._projectileOffsetRange));
		info.AddProperty(NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, Variant.From<float>(ref this._projectileSpawnInterval));
	}

	/// <inheritdoc />
	// Token: 0x06000065 RID: 101 RVA: 0x00004AB8 File Offset: 0x00002CB8
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NPlasmaOrbVfx.PropertyName._projectileOffsetRange, ref variant))
		{
			this._projectileOffsetRange = variant.As<Vector2>();
		}
		Variant variant2;
		if (info.TryGetProperty(NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, ref variant2))
		{
			this._projectileSpawnInterval = variant2.As<float>();
		}
	}

	// Token: 0x04000023 RID: 35
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Vector2 _projectileOffsetRange;

	// Token: 0x04000024 RID: 36
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _projectileSpawnInterval = 0.05f;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBD RID: 3517
	public new class MethodName : NOrbVfx.MethodName
	{
		/// <summary>
		/// Cached name for the 'OnEvokeInternal' method.
		/// </summary>
		// Token: 0x04003177 RID: 12663
		public new static readonly StringName OnEvokeInternal = "OnEvokeInternal";

		/// <summary>
		/// Cached name for the 'GetRandomOffset' method.
		/// </summary>
		// Token: 0x04003178 RID: 12664
		public static readonly StringName GetRandomOffset = "GetRandomOffset";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBE RID: 3518
	public new class PropertyName : NOrbVfx.PropertyName
	{
		/// <summary>
		/// Cached name for the '_projectileOffsetRange' field.
		/// </summary>
		// Token: 0x04003179 RID: 12665
		public static readonly StringName _projectileOffsetRange = "_projectileOffsetRange";

		/// <summary>
		/// Cached name for the '_projectileSpawnInterval' field.
		/// </summary>
		// Token: 0x0400317A RID: 12666
		public static readonly StringName _projectileSpawnInterval = "_projectileSpawnInterval";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBF RID: 3519
	public new class SignalName : NOrbVfx.SignalName
	{
	}
}
