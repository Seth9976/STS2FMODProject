using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

// Token: 0x02000004 RID: 4
[ScriptPath("res://src/Core/Nodes/Orbs/NDarkOrbVfx.cs")]
public class NDarkOrbVfx : NOrbVfx
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002AC4 File Offset: 0x00000CC4
	public override void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		base.OnPassiveActivated(passiveVal, evokeVal);
		bool flag = (float)evokeVal >= this._superchargeThreshold;
		if (this._superchargedParticles != null)
		{
			this._superchargedParticles.SetEmitting(flag);
		}
		this.UpdateDarkBgSize(flag);
		base.ShakeOrb(this.HasFocusPower() ? 1f : 0.65f, 0.55f);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002B28 File Offset: 0x00000D28
	private void UpdateDarkBgSize(bool isSupercharged)
	{
		float num = (isSupercharged ? this._darkBgSuperchargedScale : this._darkBgNormalScale);
		if (Mathf.IsEqualApprox(this._darkBg.Scale.X, num))
		{
			return;
		}
		Tween tween = base.GetTree().CreateTween();
		tween.TweenProperty(this._darkBg, "scale", Vector2.One * num, 0.25);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002B9C File Offset: 0x00000D9C
	protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
	{
		base.OnEvokeInternal(targetVfxSpawnPosition);
		NVfxProjectileHandler nvfxProjectileHandler = NVfxProjectileHandler.Create("vfx/orbs/dark/vfx_dark_orb_evoke_projectile_handler", "vfx/orbs/dark/vfx_dark_orb_evoke_projectile", base.GlobalPosition, targetVfxSpawnPosition, default(Callable));
		base.VfxContainer.AddChildSafely(nvfxProjectileHandler);
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000015 RID: 21 RVA: 0x00002BDC File Offset: 0x00000DDC
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(NDarkOrbVfx.MethodName.UpdateDarkBgSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(1L, "isSupercharged", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NDarkOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "targetVfxSpawnPosition", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000016 RID: 22 RVA: 0x00002C9C File Offset: 0x00000E9C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NDarkOrbVfx.MethodName.UpdateDarkBgSize && args.Count == 1)
		{
			this.UpdateDarkBgSize(VariantUtils.ConvertTo<bool>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NDarkOrbVfx.MethodName.OnEvokeInternal && args.Count == 1)
		{
			this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	/// <inheritdoc />
	// Token: 0x06000017 RID: 23 RVA: 0x00002D18 File Offset: 0x00000F18
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NDarkOrbVfx.MethodName.UpdateDarkBgSize || (ref method) == NDarkOrbVfx.MethodName.OnEvokeInternal || base.HasGodotClassMethod(in method);
	}

	/// <inheritdoc />
	// Token: 0x06000018 RID: 24 RVA: 0x00002D40 File Offset: 0x00000F40
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NDarkOrbVfx.PropertyName._superchargedParticles)
		{
			this._superchargedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBg)
		{
			this._darkBg = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._superchargeThreshold)
		{
			this._superchargeThreshold = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBgNormalScale)
		{
			this._darkBgNormalScale = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBgSuperchargedScale)
		{
			this._darkBgSuperchargedScale = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	/// <inheritdoc />
	// Token: 0x06000019 RID: 25 RVA: 0x00002DDC File Offset: 0x00000FDC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NDarkOrbVfx.PropertyName._superchargedParticles)
		{
			value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._superchargedParticles);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBg)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._darkBg);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._superchargeThreshold)
		{
			value = VariantUtils.CreateFrom<float>(ref this._superchargeThreshold);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBgNormalScale)
		{
			value = VariantUtils.CreateFrom<float>(ref this._darkBgNormalScale);
			return true;
		}
		if ((ref name) == NDarkOrbVfx.PropertyName._darkBgSuperchargedScale)
		{
			value = VariantUtils.CreateFrom<float>(ref this._darkBgSuperchargedScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600001A RID: 26 RVA: 0x00002E94 File Offset: 0x00001094
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NDarkOrbVfx.PropertyName._superchargedParticles, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NDarkOrbVfx.PropertyName._darkBg, 34L, "Node2D", 4102L, true),
			new PropertyInfo(3L, NDarkOrbVfx.PropertyName._superchargeThreshold, 0L, "", 4102L, true),
			new PropertyInfo(3L, NDarkOrbVfx.PropertyName._darkBgNormalScale, 0L, "", 4096L, false),
			new PropertyInfo(3L, NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600001B RID: 27 RVA: 0x00002F4C File Offset: 0x0000114C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NDarkOrbVfx.PropertyName._superchargedParticles, Variant.From<NParticlesContainer>(ref this._superchargedParticles));
		info.AddProperty(NDarkOrbVfx.PropertyName._darkBg, Variant.From<Node2D>(ref this._darkBg));
		info.AddProperty(NDarkOrbVfx.PropertyName._superchargeThreshold, Variant.From<float>(ref this._superchargeThreshold));
		info.AddProperty(NDarkOrbVfx.PropertyName._darkBgNormalScale, Variant.From<float>(ref this._darkBgNormalScale));
		info.AddProperty(NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, Variant.From<float>(ref this._darkBgSuperchargedScale));
	}

	/// <inheritdoc />
	// Token: 0x0600001C RID: 28 RVA: 0x00002FD0 File Offset: 0x000011D0
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NDarkOrbVfx.PropertyName._superchargedParticles, ref variant))
		{
			this._superchargedParticles = variant.As<NParticlesContainer>();
		}
		Variant variant2;
		if (info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBg, ref variant2))
		{
			this._darkBg = variant2.As<Node2D>();
		}
		Variant variant3;
		if (info.TryGetProperty(NDarkOrbVfx.PropertyName._superchargeThreshold, ref variant3))
		{
			this._superchargeThreshold = variant3.As<float>();
		}
		Variant variant4;
		if (info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBgNormalScale, ref variant4))
		{
			this._darkBgNormalScale = variant4.As<float>();
		}
		Variant variant5;
		if (info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, ref variant5))
		{
			this._darkBgSuperchargedScale = variant5.As<float>();
		}
	}

	// Token: 0x0400000E RID: 14
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NParticlesContainer _superchargedParticles;

	// Token: 0x0400000F RID: 15
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Node2D _darkBg;

	/// <summary>
	/// Show the supercharged particles when the evoke value is greater than or equal to this value.
	/// </summary>
	// Token: 0x04000010 RID: 16
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _superchargeThreshold = 24f;

	// Token: 0x04000011 RID: 17
	private float _darkBgNormalScale = 1f;

	// Token: 0x04000012 RID: 18
	private float _darkBgSuperchargedScale = 1.2f;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DAE RID: 3502
	public new class MethodName : NOrbVfx.MethodName
	{
		/// <summary>
		/// Cached name for the 'UpdateDarkBgSize' method.
		/// </summary>
		// Token: 0x04003153 RID: 12627
		public static readonly StringName UpdateDarkBgSize = "UpdateDarkBgSize";

		/// <summary>
		/// Cached name for the 'OnEvokeInternal' method.
		/// </summary>
		// Token: 0x04003154 RID: 12628
		public new static readonly StringName OnEvokeInternal = "OnEvokeInternal";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DAF RID: 3503
	public new class PropertyName : NOrbVfx.PropertyName
	{
		/// <summary>
		/// Cached name for the '_superchargedParticles' field.
		/// </summary>
		// Token: 0x04003155 RID: 12629
		public static readonly StringName _superchargedParticles = "_superchargedParticles";

		/// <summary>
		/// Cached name for the '_darkBg' field.
		/// </summary>
		// Token: 0x04003156 RID: 12630
		public static readonly StringName _darkBg = "_darkBg";

		/// <summary>
		/// Cached name for the '_superchargeThreshold' field.
		/// </summary>
		// Token: 0x04003157 RID: 12631
		public static readonly StringName _superchargeThreshold = "_superchargeThreshold";

		/// <summary>
		/// Cached name for the '_darkBgNormalScale' field.
		/// </summary>
		// Token: 0x04003158 RID: 12632
		public static readonly StringName _darkBgNormalScale = "_darkBgNormalScale";

		/// <summary>
		/// Cached name for the '_darkBgSuperchargedScale' field.
		/// </summary>
		// Token: 0x04003159 RID: 12633
		public static readonly StringName _darkBgSuperchargedScale = "_darkBgSuperchargedScale";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DB0 RID: 3504
	public new class SignalName : NOrbVfx.SignalName
	{
	}
}
