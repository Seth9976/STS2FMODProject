using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x020003FA RID: 1018
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCreatureStateDisplay.cs")]
	public class NCreatureStateDisplay : Control
	{
		// Token: 0x06003F23 RID: 16163 RVA: 0x001E425C File Offset: 0x001E245C
		public override void _Ready()
		{
			this._powerContainer = base.GetNode<NPowerContainer>("%PowerContainer");
			this._nameplateContainer = base.GetNode<Control>("%NameplateContainer");
			this._nameplateLabel = base.GetNode<MegaLabel>("%NameplateLabel");
			this._healthBar = base.GetNode<NHealthBar>("%HealthBar");
			this._hpBarHitbox = base.GetNode<Control>("%HpBarHitbox");
			this._nameplateContainer.Modulate = StsColors.transparentWhite;
			this._originalPosition = base.Position;
			this._hpBarHitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			this._hpBarHitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x001E433C File Offset: 0x001E253C
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
			this.SubscribeToCreatureEvents();
			if (NCombatRoom.Instance != null)
			{
				NCombatRoom.Instance.Ui.DebugToggleHpBar += this.DebugToggleVisibility;
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x001E4394 File Offset: 0x001E2594
		public override void _ExitTree()
		{
			base._ExitTree();
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			if (this._creature != null)
			{
				this._creature.BlockChanged -= this.AnimateInBlock;
				this._creature.Died -= this.OnCreatureDied;
				this._creature.Revived -= this.OnCreatureRevived;
			}
			if (this._blockTrackingCreature != null)
			{
				this._blockTrackingCreature.BlockChanged -= this.OnBlockTrackingCreatureBlockChanged;
			}
			if (NCombatRoom.Instance != null)
			{
				NCombatRoom.Instance.Ui.DebugToggleHpBar -= this.DebugToggleVisibility;
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x001E4450 File Offset: 0x001E2650
		public void SetCreature(Creature creature)
		{
			if (this._creature != null)
			{
				throw new InvalidOperationException("Creature was already set.");
			}
			this._creature = creature;
			this.SubscribeToCreatureEvents();
			this._nameplateLabel.SetTextAutoSize(creature.Name);
			this._powerContainer.SetCreature(this._creature);
			this._healthBar.SetCreature(this._creature);
			this.RefreshValues();
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x001E44B8 File Offset: 0x001E26B8
		private void SubscribeToCreatureEvents()
		{
			if (this._creature == null)
			{
				return;
			}
			this._creature.BlockChanged += this.AnimateInBlock;
			this._creature.Died += this.OnCreatureDied;
			this._creature.Revived += this.OnCreatureRevived;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x001E4513 File Offset: 0x001E2713
		private void DebugToggleVisibility()
		{
			base.Visible = !NCombatUi.IsDebugHidingHpBar;
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x001E4524 File Offset: 0x001E2724
		public void SetCreatureBounds(Control bounds)
		{
			this._healthBar.UpdateLayoutForCreatureBounds(bounds);
			this._nameplateContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X, this._nameplateContainer.GlobalPosition.Y);
			this._nameplateContainer.Size = new Vector2(bounds.Size.X * bounds.Scale.X, this._nameplateContainer.Size.Y);
			this._powerContainer.SetCreatureBounds(bounds);
			this.RefreshValues();
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x001E45B1 File Offset: 0x001E27B1
		private void RefreshValues()
		{
			if (this._creature == null)
			{
				return;
			}
			this._nameplateLabel.SetTextAutoSize(this._creature.Name);
			this._healthBar.RefreshValues();
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x001E45DD File Offset: 0x001E27DD
		private void OnCombatStateChanged(CombatState _)
		{
			this.RefreshValues();
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x001E45E8 File Offset: 0x001E27E8
		private void OnHovered()
		{
			this._healthBar.FadeOutHpLabel(0.5f, 0.1f);
			this.ShowNameplate();
			if (!NTargetManager.Instance.IsInSelection)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					return;
				}
				NCreature creatureNode = instance.GetCreatureNode(this._creature);
				if (creatureNode == null)
				{
					return;
				}
				creatureNode.ShowHoverTips(this._creature.HoverTips);
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x001E4646 File Offset: 0x001E2846
		private void OnUnhovered()
		{
			this._healthBar.FadeInHpLabel(0.5f);
			this.HideNameplate();
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(this._creature);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.HideHoverTips();
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x001E4678 File Offset: 0x001E2878
		public void ShowNameplate()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._powerContainer, "modulate:a", 0.5f, 0.15);
			this._hoverTween.TweenProperty(this._nameplateContainer, "modulate:a", 1f, 0.15);
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x001E4708 File Offset: 0x001E2908
		public void HideNameplate()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._powerContainer, "modulate:a", 1f, 0.2);
			this._hoverTween.TweenProperty(this._nameplateContainer, "modulate:a", 0f, 0.2);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x001E4798 File Offset: 0x001E2998
		public void HideImmediately()
		{
			Tween showHideTween = this._showHideTween;
			if (showHideTween != null)
			{
				showHideTween.Kill();
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x001E47E4 File Offset: 0x001E29E4
		public void AnimateIn(HealthBarAnimMode mode)
		{
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				Color modulate = base.Modulate;
				modulate.A = 1f;
				base.Modulate = modulate;
				base.Visible = true;
				return;
			}
			float num = 0f;
			base.Visible = true;
			base.Modulate = StsColors.transparentWhite;
			base.Position -= NCreatureStateDisplay._healthBarAnimOffset;
			if (mode == HealthBarAnimMode.SpawnedAtCombatStart)
			{
				num = Rng.Chaotic.NextFloat(1.3f, 1.7f);
			}
			Tween showHideTween = this._showHideTween;
			if (showHideTween != null)
			{
				showHideTween.Kill();
			}
			this._showHideTween = base.CreateTween().SetParallel(true);
			this._showHideTween.TweenProperty(this, "modulate:a", 1f, (mode == HealthBarAnimMode.FromHidden) ? 0.15 : 1.0).SetEase(1L).SetTrans(1L)
				.SetDelay((double)num);
			this._showHideTween.TweenProperty(this, "position", this._originalPosition, (mode == HealthBarAnimMode.FromHidden) ? 0.15 : 0.5).SetEase(1L).SetTrans(4L)
				.SetDelay((double)num);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x001E492C File Offset: 0x001E2B2C
		private void AnimateInBlock(int oldBlock, int blockGain)
		{
			if (oldBlock != 0 || blockGain == 0)
			{
				return;
			}
			this._healthBar.AnimateInBlock(oldBlock, blockGain);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x001E4944 File Offset: 0x001E2B44
		public void AnimateOut()
		{
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				Color modulate = base.Modulate;
				modulate.A = 0f;
				base.Modulate = modulate;
				base.Visible = false;
				return;
			}
			Tween showHideTween = this._showHideTween;
			if (showHideTween != null)
			{
				showHideTween.Kill();
			}
			this._showHideTween = base.CreateTween().SetParallel(true);
			this._showHideTween.TweenProperty(this, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(1L);
			this._showHideTween.TweenProperty(this, "position", NCreatureStateDisplay._healthBarAnimOffset, 0.25).SetEase(1L).SetTrans(4L);
			this._showHideTween.Chain().TweenCallback(Callable.From<bool>(() => base.Visible = false));
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x001E4A3A File Offset: 0x001E2C3A
		private void OnCreatureDied(Creature _)
		{
			this._hpBarHitbox.MouseFilter = 2L;
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x001E4A49 File Offset: 0x001E2C49
		private void OnCreatureRevived(Creature _)
		{
			this._hpBarHitbox.MouseFilter = 0L;
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x001E4A58 File Offset: 0x001E2C58
		public void TrackBlockStatus(Creature creature)
		{
			this._blockTrackingCreature = creature;
			this._blockTrackingCreature.BlockChanged += this.OnBlockTrackingCreatureBlockChanged;
			this._healthBar.TrackBlockStatus(creature);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Nodes.Combat.NCreature.TrackBlockStatus(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> for details.
		/// </summary>
		// Token: 0x06003F37 RID: 16183 RVA: 0x001E4A84 File Offset: 0x001E2C84
		private void OnBlockTrackingCreatureBlockChanged(int oldBlock, int blockGain)
		{
			this._healthBar.RefreshValues();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F38 RID: 16184 RVA: 0x001E4A94 File Offset: 0x001E2C94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(16)
			{
				new MethodInfo(NCreatureStateDisplay.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.DebugToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.SetCreatureBounds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "bounds", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.RefreshValues, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.ShowNameplate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.HideNameplate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.HideImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "mode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.AnimateInBlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldBlock", 0L, "", 6L, false),
					new PropertyInfo(2L, "blockGain", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.AnimateOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldBlock", 0L, "", 6L, false),
					new PropertyInfo(2L, "blockGain", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F39 RID: 16185 RVA: 0x001E4E7C File Offset: 0x001E307C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreatureStateDisplay.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents && args.Count == 0)
			{
				this.SubscribeToCreatureEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.DebugToggleVisibility && args.Count == 0)
			{
				this.DebugToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.SetCreatureBounds && args.Count == 1)
			{
				this.SetCreatureBounds(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.RefreshValues && args.Count == 0)
			{
				this.RefreshValues();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.ShowNameplate && args.Count == 0)
			{
				this.ShowNameplate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.HideNameplate && args.Count == 0)
			{
				this.HideNameplate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.HideImmediately && args.Count == 0)
			{
				this.HideImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.AnimateIn && args.Count == 1)
			{
				this.AnimateIn(VariantUtils.ConvertTo<HealthBarAnimMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.AnimateInBlock && args.Count == 2)
			{
				this.AnimateInBlock(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.AnimateOut && args.Count == 0)
			{
				this.AnimateOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged && args.Count == 2)
			{
				this.OnBlockTrackingCreatureBlockChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003F3A RID: 16186 RVA: 0x001E5134 File Offset: 0x001E3334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCreatureStateDisplay.MethodName._Ready || (ref method) == NCreatureStateDisplay.MethodName._EnterTree || (ref method) == NCreatureStateDisplay.MethodName._ExitTree || (ref method) == NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents || (ref method) == NCreatureStateDisplay.MethodName.DebugToggleVisibility || (ref method) == NCreatureStateDisplay.MethodName.SetCreatureBounds || (ref method) == NCreatureStateDisplay.MethodName.RefreshValues || (ref method) == NCreatureStateDisplay.MethodName.OnHovered || (ref method) == NCreatureStateDisplay.MethodName.OnUnhovered || (ref method) == NCreatureStateDisplay.MethodName.ShowNameplate || (ref method) == NCreatureStateDisplay.MethodName.HideNameplate || (ref method) == NCreatureStateDisplay.MethodName.HideImmediately || (ref method) == NCreatureStateDisplay.MethodName.AnimateIn || (ref method) == NCreatureStateDisplay.MethodName.AnimateInBlock || (ref method) == NCreatureStateDisplay.MethodName.AnimateOut || (ref method) == NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003F3B RID: 16187 RVA: 0x001E5238 File Offset: 0x001E3438
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCreatureStateDisplay.PropertyName._powerContainer)
			{
				this._powerContainer = VariantUtils.ConvertTo<NPowerContainer>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._nameplateContainer)
			{
				this._nameplateContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._nameplateLabel)
			{
				this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._healthBar)
			{
				this._healthBar = VariantUtils.ConvertTo<NHealthBar>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._hpBarHitbox)
			{
				this._hpBarHitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._creatureSize)
			{
				this._creatureSize = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._showHideTween)
			{
				this._showHideTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._originalPosition)
			{
				this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003F3C RID: 16188 RVA: 0x001E5340 File Offset: 0x001E3540
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCreatureStateDisplay.PropertyName._powerContainer)
			{
				value = VariantUtils.CreateFrom<NPowerContainer>(ref this._powerContainer);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._nameplateContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._nameplateContainer);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._nameplateLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._healthBar)
			{
				value = VariantUtils.CreateFrom<NHealthBar>(ref this._healthBar);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._hpBarHitbox)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hpBarHitbox);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._creatureSize)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._creatureSize);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._showHideTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._showHideTween);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NCreatureStateDisplay.PropertyName._originalPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F3D RID: 16189 RVA: 0x001E5478 File Offset: 0x001E3678
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._powerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._nameplateContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._nameplateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._healthBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._hpBarHitbox, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCreatureStateDisplay.PropertyName._creatureSize, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._showHideTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureStateDisplay.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCreatureStateDisplay.PropertyName._originalPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F3E RID: 16190 RVA: 0x001E55B4 File Offset: 0x001E37B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCreatureStateDisplay.PropertyName._powerContainer, Variant.From<NPowerContainer>(ref this._powerContainer));
			info.AddProperty(NCreatureStateDisplay.PropertyName._nameplateContainer, Variant.From<Control>(ref this._nameplateContainer));
			info.AddProperty(NCreatureStateDisplay.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
			info.AddProperty(NCreatureStateDisplay.PropertyName._healthBar, Variant.From<NHealthBar>(ref this._healthBar));
			info.AddProperty(NCreatureStateDisplay.PropertyName._hpBarHitbox, Variant.From<Control>(ref this._hpBarHitbox));
			info.AddProperty(NCreatureStateDisplay.PropertyName._creatureSize, Variant.From<Vector2>(ref this._creatureSize));
			info.AddProperty(NCreatureStateDisplay.PropertyName._showHideTween, Variant.From<Tween>(ref this._showHideTween));
			info.AddProperty(NCreatureStateDisplay.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NCreatureStateDisplay.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
		}

		/// <inheritdoc />
		// Token: 0x06003F3F RID: 16191 RVA: 0x001E5690 File Offset: 0x001E3890
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._powerContainer, ref variant))
			{
				this._powerContainer = variant.As<NPowerContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._nameplateContainer, ref variant2))
			{
				this._nameplateContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._nameplateLabel, ref variant3))
			{
				this._nameplateLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._healthBar, ref variant4))
			{
				this._healthBar = variant4.As<NHealthBar>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._hpBarHitbox, ref variant5))
			{
				this._hpBarHitbox = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._creatureSize, ref variant6))
			{
				this._creatureSize = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._showHideTween, ref variant7))
			{
				this._showHideTween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._hoverTween, ref variant8))
			{
				this._hoverTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._originalPosition, ref variant9))
			{
				this._originalPosition = variant9.As<Vector2>();
			}
		}

		// Token: 0x04001875 RID: 6261
		private NPowerContainer _powerContainer;

		// Token: 0x04001876 RID: 6262
		private Control _nameplateContainer;

		// Token: 0x04001877 RID: 6263
		private MegaLabel _nameplateLabel;

		// Token: 0x04001878 RID: 6264
		private NHealthBar _healthBar;

		// Token: 0x04001879 RID: 6265
		private Control _hpBarHitbox;

		// Token: 0x0400187A RID: 6266
		[Nullable(2)]
		private Creature _creature;

		// Token: 0x0400187B RID: 6267
		private Vector2 _creatureSize;

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Nodes.Combat.NCreature.TrackBlockStatus(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> for details.
		/// </summary>
		// Token: 0x0400187C RID: 6268
		[Nullable(2)]
		private Creature _blockTrackingCreature;

		// Token: 0x0400187D RID: 6269
		[Nullable(2)]
		private Tween _showHideTween;

		// Token: 0x0400187E RID: 6270
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x0400187F RID: 6271
		private static readonly Vector2 _healthBarAnimOffset = new Vector2(0f, 20f);

		// Token: 0x04001880 RID: 6272
		private Vector2 _originalPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018CF RID: 6351
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400611C RID: 24860
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400611D RID: 24861
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400611E RID: 24862
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SubscribeToCreatureEvents' method.
			/// </summary>
			// Token: 0x0400611F RID: 24863
			public static readonly StringName SubscribeToCreatureEvents = "SubscribeToCreatureEvents";

			/// <summary>
			/// Cached name for the 'DebugToggleVisibility' method.
			/// </summary>
			// Token: 0x04006120 RID: 24864
			public static readonly StringName DebugToggleVisibility = "DebugToggleVisibility";

			/// <summary>
			/// Cached name for the 'SetCreatureBounds' method.
			/// </summary>
			// Token: 0x04006121 RID: 24865
			public static readonly StringName SetCreatureBounds = "SetCreatureBounds";

			/// <summary>
			/// Cached name for the 'RefreshValues' method.
			/// </summary>
			// Token: 0x04006122 RID: 24866
			public static readonly StringName RefreshValues = "RefreshValues";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04006123 RID: 24867
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04006124 RID: 24868
			public static readonly StringName OnUnhovered = "OnUnhovered";

			/// <summary>
			/// Cached name for the 'ShowNameplate' method.
			/// </summary>
			// Token: 0x04006125 RID: 24869
			public static readonly StringName ShowNameplate = "ShowNameplate";

			/// <summary>
			/// Cached name for the 'HideNameplate' method.
			/// </summary>
			// Token: 0x04006126 RID: 24870
			public static readonly StringName HideNameplate = "HideNameplate";

			/// <summary>
			/// Cached name for the 'HideImmediately' method.
			/// </summary>
			// Token: 0x04006127 RID: 24871
			public static readonly StringName HideImmediately = "HideImmediately";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04006128 RID: 24872
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'AnimateInBlock' method.
			/// </summary>
			// Token: 0x04006129 RID: 24873
			public static readonly StringName AnimateInBlock = "AnimateInBlock";

			/// <summary>
			/// Cached name for the 'AnimateOut' method.
			/// </summary>
			// Token: 0x0400612A RID: 24874
			public static readonly StringName AnimateOut = "AnimateOut";

			/// <summary>
			/// Cached name for the 'OnBlockTrackingCreatureBlockChanged' method.
			/// </summary>
			// Token: 0x0400612B RID: 24875
			public static readonly StringName OnBlockTrackingCreatureBlockChanged = "OnBlockTrackingCreatureBlockChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D0 RID: 6352
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_powerContainer' field.
			/// </summary>
			// Token: 0x0400612C RID: 24876
			public static readonly StringName _powerContainer = "_powerContainer";

			/// <summary>
			/// Cached name for the '_nameplateContainer' field.
			/// </summary>
			// Token: 0x0400612D RID: 24877
			public static readonly StringName _nameplateContainer = "_nameplateContainer";

			/// <summary>
			/// Cached name for the '_nameplateLabel' field.
			/// </summary>
			// Token: 0x0400612E RID: 24878
			public static readonly StringName _nameplateLabel = "_nameplateLabel";

			/// <summary>
			/// Cached name for the '_healthBar' field.
			/// </summary>
			// Token: 0x0400612F RID: 24879
			public static readonly StringName _healthBar = "_healthBar";

			/// <summary>
			/// Cached name for the '_hpBarHitbox' field.
			/// </summary>
			// Token: 0x04006130 RID: 24880
			public static readonly StringName _hpBarHitbox = "_hpBarHitbox";

			/// <summary>
			/// Cached name for the '_creatureSize' field.
			/// </summary>
			// Token: 0x04006131 RID: 24881
			public static readonly StringName _creatureSize = "_creatureSize";

			/// <summary>
			/// Cached name for the '_showHideTween' field.
			/// </summary>
			// Token: 0x04006132 RID: 24882
			public static readonly StringName _showHideTween = "_showHideTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04006133 RID: 24883
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_originalPosition' field.
			/// </summary>
			// Token: 0x04006134 RID: 24884
			public static readonly StringName _originalPosition = "_originalPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D1 RID: 6353
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
