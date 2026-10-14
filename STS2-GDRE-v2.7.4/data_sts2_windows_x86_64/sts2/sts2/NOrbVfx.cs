using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

// Token: 0x02000008 RID: 8
[NullableContext(1)]
[Nullable(0)]
[ScriptPath("res://src/Core/Nodes/Orbs/NOrbVfx.cs")]
public class NOrbVfx : Node2D
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00003664 File Offset: 0x00001864
	protected Control VfxContainer
	{
		get
		{
			Control control;
			if ((control = this._overrideCombatVfxContainer) == null)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					return null;
				}
				control = instance.CombatVfxContainer;
			}
			return control;
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003680 File Offset: 0x00001880
	public override void _Ready()
	{
		base._Ready();
		if (this._spineShaker != null)
		{
			this._spineShaker.Strength = 0f;
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000036A0 File Offset: 0x000018A0
	public void Initialize(OrbModel orbModel)
	{
		this._orbModel = orbModel;
		this._owner = this._orbModel.Owner;
		if (this._owner != null)
		{
			this._owner.Creature.PowerApplied += this.OnPowerApplied;
			this._owner.Creature.PowerIncreased += this.OnPowerIncreased;
			this._owner.Creature.PowerDecreased += this.OnPowerDecreased;
		}
		if (this._orbModel != null)
		{
			this._orbModel.PassiveActivated += this.OnPassiveActivated;
			this._orbModel.EvokeActivated += this.OnEvoke;
		}
		this.UpdateFocusPowerState();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003760 File Offset: 0x00001960
	public override void _ExitTree()
	{
		base._ExitTree();
		Tween shakeTween = this._shakeTween;
		if (shakeTween != null)
		{
			shakeTween.Kill();
		}
		if (this._owner != null)
		{
			this._owner.Creature.PowerApplied -= this.OnPowerApplied;
			this._owner.Creature.PowerIncreased -= this.OnPowerIncreased;
			this._owner.Creature.PowerDecreased -= this.OnPowerDecreased;
		}
		if (this._orbModel != null)
		{
			this._orbModel.PassiveActivated -= this.OnPassiveActivated;
			this._orbModel.EvokeActivated -= this.OnEvoke;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003816 File Offset: 0x00001A16
	protected void OnPowerApplied(PowerModel powerModel)
	{
		this.UpdateFocusPowerState();
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000381E File Offset: 0x00001A1E
	protected void OnPowerIncreased(PowerModel powerModel, int amount, bool silent)
	{
		this.UpdateFocusPowerState();
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003826 File Offset: 0x00001A26
	protected void OnPowerDecreased(PowerModel powerModel, bool silent)
	{
		this.UpdateFocusPowerState();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0000382E File Offset: 0x00001A2E
	protected virtual void UpdateFocusPowerState()
	{
		if (this._focusedParticles != null)
		{
			this._focusedParticles.SetEmitting(this.HasFocusPower());
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003849 File Offset: 0x00001A49
	public virtual void SetForcedFocusPower(bool forcedFocusPower)
	{
		this._forcedFocusPower = forcedFocusPower;
		this.UpdateFocusPowerState();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003858 File Offset: 0x00001A58
	protected virtual bool HasFocusPower()
	{
		bool flag = this._forcedFocusPower;
		if (this._owner != null)
		{
			flag |= this._owner.Creature.GetPowerAmount<FocusPower>() > 0;
		}
		return flag;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0000388B File Offset: 0x00001A8B
	public void OnPassiveActivated()
	{
		if (this._orbModel == null)
		{
			return;
		}
		this.OnPassiveActivated(this._orbModel.PassiveVal, this._orbModel.EvokeVal);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000038B2 File Offset: 0x00001AB2
	public virtual void OnPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
		if (this._passiveActivatedParticles != null)
		{
			this._passiveActivatedParticles.Restart();
		}
		if (this._passiveActivatedFocusedParticles != null && this.HasFocusPower())
		{
			this._passiveActivatedFocusedParticles.Restart();
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000038E2 File Offset: 0x00001AE2
	public virtual void AfterPassiveActivated(decimal passiveVal, decimal evokeVal)
	{
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000038E4 File Offset: 0x00001AE4
	private void OnEvoke(Creature[] targets)
	{
		List<NCreature> list = new List<NCreature>();
		foreach (Creature creature in targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				list.Add(ncreature);
			}
		}
		this.OnEvoke(list.ToArray());
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003938 File Offset: 0x00001B38
	public void OnEvoke(NCreature[] targets)
	{
		Vector2[] array = new Vector2[targets.Length];
		for (int i = 0; i < targets.Length; i++)
		{
			array[i] = targets[i].VfxSpawnPosition;
		}
		this.OnEvoke(array);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003974 File Offset: 0x00001B74
	public void OnEvoke(Vector2[] targetVfxSpawnPositions)
	{
		this.SpawnEvokeVfx();
		for (int i = 0; i < targetVfxSpawnPositions.Length; i++)
		{
			this.OnEvokeInternal(targetVfxSpawnPositions[i]);
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000039A2 File Offset: 0x00001BA2
	private void SpawnEvokeVfx()
	{
		if (string.IsNullOrEmpty(this._evokeVfxSceneName))
		{
			return;
		}
		VfxCmd.PlayVfx(base.GlobalPosition, NOrbVfx._evokeVfxScenePath + this._evokeVfxSceneName, this.VfxContainer);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000039D3 File Offset: 0x00001BD3
	protected virtual void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
	{
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000039D8 File Offset: 0x00001BD8
	protected Vector2 GetPlayerVfxPosition()
	{
		if (this._overridePlayerNode != null)
		{
			return this._overridePlayerNode.Position;
		}
		if (this._owner == null)
		{
			return base.Position;
		}
		NCombatRoom instance = NCombatRoom.Instance;
		NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._owner.Creature) : null);
		if (ncreature != null)
		{
			return ncreature.VfxSpawnPosition;
		}
		return base.Position;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003A35 File Offset: 0x00001C35
	public void SetOverrideCombatVfxContainer(Control overrideCombatVfxContainer)
	{
		this._overrideCombatVfxContainer = overrideCombatVfxContainer;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003A3E File Offset: 0x00001C3E
	public void SetOverridePlayerNode(Node2D overridePlayerNode)
	{
		this._overridePlayerNode = overridePlayerNode;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003A48 File Offset: 0x00001C48
	protected void ShakeOrb(float initialStrength, float duration)
	{
		if (this._spineShaker == null)
		{
			return;
		}
		if (this._shakeTween != null && this._shakeTween.IsValid())
		{
			this._shakeTween.Kill();
		}
		this._spineShaker.Strength = initialStrength;
		this._shakeTween = base.GetTree().CreateTween();
		this._shakeTween.TweenProperty(this._spineShaker, "_strength", 0f, (double)duration).SetEase(3L).SetTrans(1L);
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000050 RID: 80 RVA: 0x00003AD4 File Offset: 0x00001CD4
	[NullableContext(0)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(NOrbVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.UpdateFocusPowerState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.SetForcedFocusPower, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(1L, "forcedFocusPower", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NOrbVfx.MethodName.HasFocusPower, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.OnPassiveActivated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.OnEvoke, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(28L, "targets", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NOrbVfx.MethodName.SpawnEvokeVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "targetVfxSpawnPosition", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NOrbVfx.MethodName.GetPlayerVfxPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfx.MethodName.SetOverrideCombatVfxContainer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(24L, "overrideCombatVfxContainer", 0L, "", 6L, new StringName("Control"), false)
			}, null),
			new MethodInfo(NOrbVfx.MethodName.SetOverridePlayerNode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(24L, "overridePlayerNode", 0L, "", 6L, new StringName("Node2D"), false)
			}, null),
			new MethodInfo(NOrbVfx.MethodName.ShakeOrb, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(3L, "initialStrength", 0L, "", 6L, false),
				new PropertyInfo(3L, "duration", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000051 RID: 81 RVA: 0x00003E64 File Offset: 0x00002064
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NOrbVfx.MethodName._Ready && args.Count == 0)
		{
			this._Ready();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName._ExitTree && args.Count == 0)
		{
			this._ExitTree();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.UpdateFocusPowerState && args.Count == 0)
		{
			this.UpdateFocusPowerState();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.SetForcedFocusPower && args.Count == 1)
		{
			this.SetForcedFocusPower(VariantUtils.ConvertTo<bool>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.HasFocusPower && args.Count == 0)
		{
			bool flag = this.HasFocusPower();
			ret = VariantUtils.CreateFrom<bool>(ref flag);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.OnPassiveActivated && args.Count == 0)
		{
			this.OnPassiveActivated();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.OnEvoke && args.Count == 1)
		{
			this.OnEvoke(VariantUtils.ConvertToSystemArrayOfGodotObject<NCreature>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.SpawnEvokeVfx && args.Count == 0)
		{
			this.SpawnEvokeVfx();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.OnEvokeInternal && args.Count == 1)
		{
			this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.GetPlayerVfxPosition && args.Count == 0)
		{
			Vector2 playerVfxPosition = this.GetPlayerVfxPosition();
			ret = VariantUtils.CreateFrom<Vector2>(ref playerVfxPosition);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.SetOverrideCombatVfxContainer && args.Count == 1)
		{
			this.SetOverrideCombatVfxContainer(VariantUtils.ConvertTo<Control>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.SetOverridePlayerNode && args.Count == 1)
		{
			this.SetOverridePlayerNode(VariantUtils.ConvertTo<Node2D>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfx.MethodName.ShakeOrb && args.Count == 2)
		{
			this.ShakeOrb(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(ref method, args, ref ret);
	}

	/// <inheritdoc />
	// Token: 0x06000052 RID: 82 RVA: 0x000040CC File Offset: 0x000022CC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NOrbVfx.MethodName._Ready || (ref method) == NOrbVfx.MethodName._ExitTree || (ref method) == NOrbVfx.MethodName.UpdateFocusPowerState || (ref method) == NOrbVfx.MethodName.SetForcedFocusPower || (ref method) == NOrbVfx.MethodName.HasFocusPower || (ref method) == NOrbVfx.MethodName.OnPassiveActivated || (ref method) == NOrbVfx.MethodName.OnEvoke || (ref method) == NOrbVfx.MethodName.SpawnEvokeVfx || (ref method) == NOrbVfx.MethodName.OnEvokeInternal || (ref method) == NOrbVfx.MethodName.GetPlayerVfxPosition || (ref method) == NOrbVfx.MethodName.SetOverrideCombatVfxContainer || (ref method) == NOrbVfx.MethodName.SetOverridePlayerNode || (ref method) == NOrbVfx.MethodName.ShakeOrb || base.HasGodotClassMethod(ref method);
	}

	/// <inheritdoc />
	// Token: 0x06000053 RID: 83 RVA: 0x000041A4 File Offset: 0x000023A4
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NOrbVfx.PropertyName._focusedParticles)
		{
			this._focusedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._passiveActivatedParticles)
		{
			this._passiveActivatedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._passiveActivatedFocusedParticles)
		{
			this._passiveActivatedFocusedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._spineShaker)
		{
			this._spineShaker = VariantUtils.ConvertTo<NShaker>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._evokeVfxSceneName)
		{
			this._evokeVfxSceneName = VariantUtils.ConvertTo<string>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._forcedFocusPower)
		{
			this._forcedFocusPower = VariantUtils.ConvertTo<bool>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._overrideCombatVfxContainer)
		{
			this._overrideCombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._overridePlayerNode)
		{
			this._overridePlayerNode = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._shakeTween)
		{
			this._shakeTween = VariantUtils.ConvertTo<Tween>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(ref name, ref value);
	}

	/// <inheritdoc />
	// Token: 0x06000054 RID: 84 RVA: 0x000042AC File Offset: 0x000024AC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NOrbVfx.PropertyName.VfxContainer)
		{
			Control vfxContainer = this.VfxContainer;
			value = VariantUtils.CreateFrom<Control>(ref vfxContainer);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._focusedParticles)
		{
			value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._focusedParticles);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._passiveActivatedParticles)
		{
			value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._passiveActivatedParticles);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._passiveActivatedFocusedParticles)
		{
			value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._passiveActivatedFocusedParticles);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._spineShaker)
		{
			value = VariantUtils.CreateFrom<NShaker>(ref this._spineShaker);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._evokeVfxSceneName)
		{
			value = VariantUtils.CreateFrom<string>(ref this._evokeVfxSceneName);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._forcedFocusPower)
		{
			value = VariantUtils.CreateFrom<bool>(ref this._forcedFocusPower);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._overrideCombatVfxContainer)
		{
			value = VariantUtils.CreateFrom<Control>(ref this._overrideCombatVfxContainer);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._overridePlayerNode)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._overridePlayerNode);
			return true;
		}
		if ((ref name) == NOrbVfx.PropertyName._shakeTween)
		{
			value = VariantUtils.CreateFrom<Tween>(ref this._shakeTween);
			return true;
		}
		return base.GetGodotClassPropertyValue(ref name, ref value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000055 RID: 85 RVA: 0x00004404 File Offset: 0x00002604
	[NullableContext(0)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NOrbVfx.PropertyName._focusedParticles, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NOrbVfx.PropertyName._passiveActivatedParticles, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NOrbVfx.PropertyName._passiveActivatedFocusedParticles, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NOrbVfx.PropertyName._spineShaker, 34L, "Node", 4102L, true),
			new PropertyInfo(4L, NOrbVfx.PropertyName._evokeVfxSceneName, 0L, "", 4102L, true),
			new PropertyInfo(1L, NOrbVfx.PropertyName._forcedFocusPower, 0L, "", 4096L, false),
			new PropertyInfo(24L, NOrbVfx.PropertyName._overrideCombatVfxContainer, 0L, "", 4096L, false),
			new PropertyInfo(24L, NOrbVfx.PropertyName.VfxContainer, 0L, "", 4096L, false),
			new PropertyInfo(24L, NOrbVfx.PropertyName._overridePlayerNode, 0L, "", 4096L, false),
			new PropertyInfo(24L, NOrbVfx.PropertyName._shakeTween, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000056 RID: 86 RVA: 0x00004564 File Offset: 0x00002764
	[NullableContext(0)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NOrbVfx.PropertyName._focusedParticles, Variant.From<NParticlesContainer>(ref this._focusedParticles));
		info.AddProperty(NOrbVfx.PropertyName._passiveActivatedParticles, Variant.From<NParticlesContainer>(ref this._passiveActivatedParticles));
		info.AddProperty(NOrbVfx.PropertyName._passiveActivatedFocusedParticles, Variant.From<NParticlesContainer>(ref this._passiveActivatedFocusedParticles));
		info.AddProperty(NOrbVfx.PropertyName._spineShaker, Variant.From<NShaker>(ref this._spineShaker));
		info.AddProperty(NOrbVfx.PropertyName._evokeVfxSceneName, Variant.From<string>(ref this._evokeVfxSceneName));
		info.AddProperty(NOrbVfx.PropertyName._forcedFocusPower, Variant.From<bool>(ref this._forcedFocusPower));
		info.AddProperty(NOrbVfx.PropertyName._overrideCombatVfxContainer, Variant.From<Control>(ref this._overrideCombatVfxContainer));
		info.AddProperty(NOrbVfx.PropertyName._overridePlayerNode, Variant.From<Node2D>(ref this._overridePlayerNode));
		info.AddProperty(NOrbVfx.PropertyName._shakeTween, Variant.From<Tween>(ref this._shakeTween));
	}

	/// <inheritdoc />
	// Token: 0x06000057 RID: 87 RVA: 0x00004640 File Offset: 0x00002840
	[NullableContext(0)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NOrbVfx.PropertyName._focusedParticles, ref variant))
		{
			this._focusedParticles = variant.As<NParticlesContainer>();
		}
		Variant variant2;
		if (info.TryGetProperty(NOrbVfx.PropertyName._passiveActivatedParticles, ref variant2))
		{
			this._passiveActivatedParticles = variant2.As<NParticlesContainer>();
		}
		Variant variant3;
		if (info.TryGetProperty(NOrbVfx.PropertyName._passiveActivatedFocusedParticles, ref variant3))
		{
			this._passiveActivatedFocusedParticles = variant3.As<NParticlesContainer>();
		}
		Variant variant4;
		if (info.TryGetProperty(NOrbVfx.PropertyName._spineShaker, ref variant4))
		{
			this._spineShaker = variant4.As<NShaker>();
		}
		Variant variant5;
		if (info.TryGetProperty(NOrbVfx.PropertyName._evokeVfxSceneName, ref variant5))
		{
			this._evokeVfxSceneName = variant5.As<string>();
		}
		Variant variant6;
		if (info.TryGetProperty(NOrbVfx.PropertyName._forcedFocusPower, ref variant6))
		{
			this._forcedFocusPower = variant6.As<bool>();
		}
		Variant variant7;
		if (info.TryGetProperty(NOrbVfx.PropertyName._overrideCombatVfxContainer, ref variant7))
		{
			this._overrideCombatVfxContainer = variant7.As<Control>();
		}
		Variant variant8;
		if (info.TryGetProperty(NOrbVfx.PropertyName._overridePlayerNode, ref variant8))
		{
			this._overridePlayerNode = variant8.As<Node2D>();
		}
		Variant variant9;
		if (info.TryGetProperty(NOrbVfx.PropertyName._shakeTween, ref variant9))
		{
			this._shakeTween = variant9.As<Tween>();
		}
	}

	// Token: 0x04000017 RID: 23
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NParticlesContainer _focusedParticles;

	// Token: 0x04000018 RID: 24
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NParticlesContainer _passiveActivatedParticles;

	// Token: 0x04000019 RID: 25
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NParticlesContainer _passiveActivatedFocusedParticles;

	// Token: 0x0400001A RID: 26
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NShaker _spineShaker;

	// Token: 0x0400001B RID: 27
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private string _evokeVfxSceneName = "";

	// Token: 0x0400001C RID: 28
	private static string _evokeVfxScenePath = "vfx/orbs/";

	// Token: 0x0400001D RID: 29
	protected bool _forcedFocusPower;

	// Token: 0x0400001E RID: 30
	[Nullable(2)]
	private Control _overrideCombatVfxContainer;

	// Token: 0x0400001F RID: 31
	[Nullable(2)]
	protected OrbModel _orbModel;

	// Token: 0x04000020 RID: 32
	[Nullable(2)]
	protected Player _owner;

	// Token: 0x04000021 RID: 33
	[Nullable(2)]
	protected Node2D _overridePlayerNode;

	// Token: 0x04000022 RID: 34
	[Nullable(2)]
	private Tween _shakeTween;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBA RID: 3514
	[NullableContext(0)]
	public class MethodName : Node2D.MethodName
	{
		/// <summary>
		/// Cached name for the '_Ready' method.
		/// </summary>
		// Token: 0x04003160 RID: 12640
		public static readonly StringName _Ready = "_Ready";

		/// <summary>
		/// Cached name for the '_ExitTree' method.
		/// </summary>
		// Token: 0x04003161 RID: 12641
		public static readonly StringName _ExitTree = "_ExitTree";

		/// <summary>
		/// Cached name for the 'UpdateFocusPowerState' method.
		/// </summary>
		// Token: 0x04003162 RID: 12642
		public static readonly StringName UpdateFocusPowerState = "UpdateFocusPowerState";

		/// <summary>
		/// Cached name for the 'SetForcedFocusPower' method.
		/// </summary>
		// Token: 0x04003163 RID: 12643
		public static readonly StringName SetForcedFocusPower = "SetForcedFocusPower";

		/// <summary>
		/// Cached name for the 'HasFocusPower' method.
		/// </summary>
		// Token: 0x04003164 RID: 12644
		public static readonly StringName HasFocusPower = "HasFocusPower";

		/// <summary>
		/// Cached name for the 'OnPassiveActivated' method.
		/// </summary>
		// Token: 0x04003165 RID: 12645
		public static readonly StringName OnPassiveActivated = "OnPassiveActivated";

		/// <summary>
		/// Cached name for the 'OnEvoke' method.
		/// </summary>
		// Token: 0x04003166 RID: 12646
		public static readonly StringName OnEvoke = "OnEvoke";

		/// <summary>
		/// Cached name for the 'SpawnEvokeVfx' method.
		/// </summary>
		// Token: 0x04003167 RID: 12647
		public static readonly StringName SpawnEvokeVfx = "SpawnEvokeVfx";

		/// <summary>
		/// Cached name for the 'OnEvokeInternal' method.
		/// </summary>
		// Token: 0x04003168 RID: 12648
		public static readonly StringName OnEvokeInternal = "OnEvokeInternal";

		/// <summary>
		/// Cached name for the 'GetPlayerVfxPosition' method.
		/// </summary>
		// Token: 0x04003169 RID: 12649
		public static readonly StringName GetPlayerVfxPosition = "GetPlayerVfxPosition";

		/// <summary>
		/// Cached name for the 'SetOverrideCombatVfxContainer' method.
		/// </summary>
		// Token: 0x0400316A RID: 12650
		public static readonly StringName SetOverrideCombatVfxContainer = "SetOverrideCombatVfxContainer";

		/// <summary>
		/// Cached name for the 'SetOverridePlayerNode' method.
		/// </summary>
		// Token: 0x0400316B RID: 12651
		public static readonly StringName SetOverridePlayerNode = "SetOverridePlayerNode";

		/// <summary>
		/// Cached name for the 'ShakeOrb' method.
		/// </summary>
		// Token: 0x0400316C RID: 12652
		public static readonly StringName ShakeOrb = "ShakeOrb";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBB RID: 3515
	[NullableContext(0)]
	public class PropertyName : Node2D.PropertyName
	{
		/// <summary>
		/// Cached name for the 'VfxContainer' property.
		/// </summary>
		// Token: 0x0400316D RID: 12653
		public static readonly StringName VfxContainer = "VfxContainer";

		/// <summary>
		/// Cached name for the '_focusedParticles' field.
		/// </summary>
		// Token: 0x0400316E RID: 12654
		public static readonly StringName _focusedParticles = "_focusedParticles";

		/// <summary>
		/// Cached name for the '_passiveActivatedParticles' field.
		/// </summary>
		// Token: 0x0400316F RID: 12655
		public static readonly StringName _passiveActivatedParticles = "_passiveActivatedParticles";

		/// <summary>
		/// Cached name for the '_passiveActivatedFocusedParticles' field.
		/// </summary>
		// Token: 0x04003170 RID: 12656
		public static readonly StringName _passiveActivatedFocusedParticles = "_passiveActivatedFocusedParticles";

		/// <summary>
		/// Cached name for the '_spineShaker' field.
		/// </summary>
		// Token: 0x04003171 RID: 12657
		public static readonly StringName _spineShaker = "_spineShaker";

		/// <summary>
		/// Cached name for the '_evokeVfxSceneName' field.
		/// </summary>
		// Token: 0x04003172 RID: 12658
		public static readonly StringName _evokeVfxSceneName = "_evokeVfxSceneName";

		/// <summary>
		/// Cached name for the '_forcedFocusPower' field.
		/// </summary>
		// Token: 0x04003173 RID: 12659
		public static readonly StringName _forcedFocusPower = "_forcedFocusPower";

		/// <summary>
		/// Cached name for the '_overrideCombatVfxContainer' field.
		/// </summary>
		// Token: 0x04003174 RID: 12660
		public static readonly StringName _overrideCombatVfxContainer = "_overrideCombatVfxContainer";

		/// <summary>
		/// Cached name for the '_overridePlayerNode' field.
		/// </summary>
		// Token: 0x04003175 RID: 12661
		public static readonly StringName _overridePlayerNode = "_overridePlayerNode";

		/// <summary>
		/// Cached name for the '_shakeTween' field.
		/// </summary>
		// Token: 0x04003176 RID: 12662
		public static readonly StringName _shakeTween = "_shakeTween";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DBC RID: 3516
	[NullableContext(0)]
	public class SignalName : Node2D.SignalName
	{
	}
}
