using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001F7 RID: 503
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSovereignBladeVfx.cs")]
	public class NSovereignBladeVfx : Node2D
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x000A2A01 File Offset: 0x000A0C01
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NSovereignBladeVfx._scenePath);
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x000A2A0D File Offset: 0x000A0C0D
		// (set) Token: 0x060019BF RID: 6591 RVA: 0x000A2A15 File Offset: 0x000A0C15
		public CardModel Card { get; private set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x000A2A1E File Offset: 0x000A0C1E
		// (set) Token: 0x060019C1 RID: 6593 RVA: 0x000A2A26 File Offset: 0x000A0C26
		public double OrbitProgress { get; set; }

		// Token: 0x060019C2 RID: 6594 RVA: 0x000A2A30 File Offset: 0x000A0C30
		public override void _Ready()
		{
			this._spineNode = base.GetNode<Node2D>("SpineSword");
			this._animController = new MegaSprite(this._spineNode);
			this._bladeGlow = base.GetNode<Node2D>("SpineSword/SwordBone/ScaleContainer/BladeGlow");
			this._forgeSparks = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/ForgeSparks");
			this._spawnFlames = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/SpawnFlames");
			this._spawnFlamesBack = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/SpawnFlamesBack");
			this._slashParticles = base.GetNode<GpuParticles2D>("SpineSword/SlashParticles");
			this._chargeParticles = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/ChargeParticles");
			this._spikeParticles = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/Spikes");
			this._spikeParticles2 = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/Spikes2");
			this._spikeCircle = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/SpikeCircle");
			this._spikeCircle2 = base.GetNode<GpuParticles2D>("SpineSword/SwordBone/ScaleContainer/SpikeCircle2");
			this._hilt = base.GetNode<TextureRect>("SpineSword/SwordBone/ScaleContainer/Hilt");
			this._hilt2 = base.GetNode<TextureRect>("SpineSword/SwordBone/ScaleContainer/Hilt2");
			this._detail = base.GetNode<TextureRect>("SpineSword/SwordBone/ScaleContainer/Detail");
			this._trail = base.GetNode<Line2D>("Trail");
			this._orbitPath = base.GetNode<Path2D>("%Path");
			this._hitbox = base.GetNode<Control>("%Hitbox");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocused)), 0U);
			this._hitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocused)), 0U);
			this._hitbox.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocused)), 0U);
			this._hitbox.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocused)), 0U);
			this._forgeSparks.Emitting = false;
			this._forgeSparks.OneShot = true;
			this._spawnFlames.Emitting = false;
			this._spawnFlames.OneShot = true;
			this._spawnFlamesBack.Emitting = false;
			this._spawnFlamesBack.OneShot = true;
			this._slashParticles.Emitting = false;
			this._slashParticles.OneShot = true;
			this._chargeParticles.Emitting = false;
			this._spikeParticles2.Emitting = false;
			this._spikeCircle2.Emitting = false;
			this._bladeGlow.Modulate = Colors.Transparent;
			this._bladeGlow.Visible = false;
			this._trail.GlobalPosition = Vector2.Zero;
			this._trail.ClearPoints();
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("idle_loop", true, 0);
			});
			this._spineNode.Scale = Vector2.Zero;
			this._spineNode.Visible = true;
			NTargetManager.Instance.Connect(NTargetManager.SignalName.TargetingBegan, Callable.From(new Action(this.OnTargetingBegan)), 0U);
			NTargetManager.Instance.Connect(NTargetManager.SignalName.TargetingEnded, Callable.From(new Action(this.OnTargetingEnded)), 0U);
			this._owner = this.Card.Owner;
			this._owner.Creature.Died += this.OnOwnerDied;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000A2DE0 File Offset: 0x000A0FE0
		public override void _ExitTree()
		{
			Tween attackTween = this._attackTween;
			if (attackTween != null)
			{
				attackTween.Kill();
			}
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			Tween sparkDelay = this._sparkDelay;
			if (sparkDelay != null)
			{
				sparkDelay.Kill();
			}
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			Tween trailFadeTween = this._trailFadeTween;
			if (trailFadeTween != null)
			{
				trailFadeTween.Kill();
			}
			this._owner.Creature.Died -= this.OnOwnerDied;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x000A2E60 File Offset: 0x000A1060
		[return: Nullable(2)]
		public static NSovereignBladeVfx Create(CardModel card)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSovereignBladeVfx nsovereignBladeVfx = PreloadManager.Cache.GetScene(NSovereignBladeVfx._scenePath).Instantiate<NSovereignBladeVfx>(0L);
			nsovereignBladeVfx.Card = card;
			return nsovereignBladeVfx;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x000A2E98 File Offset: 0x000A1098
		public override void _Process(double delta)
		{
			float bakedLength = this._orbitPath.Curve.GetBakedLength();
			if (this._hoverTip == null)
			{
				this.OrbitProgress += 60.0 * delta / (double)bakedLength;
			}
			double num = this.OrbitProgress % 1.0;
			bool flag = num > 0.25 && num < 0.7799999713897705;
			if (flag != this._isBehindCharacter && this._bladeSize < 0.6f)
			{
				this._isBehindCharacter = !this._isBehindCharacter;
				base.GetParent().MoveChildSafely(this, flag ? 0 : (base.GetParent().GetChildCount(false) - 1));
			}
			Transform2D transform2D = this._orbitPath.Curve.SampleBakedWithRotation((float)(this.OrbitProgress % 1.0) * bakedLength, false);
			Vector2 vector = this._orbitPath.GlobalTransform * transform2D.Origin;
			vector.X = Mathf.Lerp(vector.X, base.GlobalPosition.X + 200f, Mathf.Clamp(this._bladeSize / 1.25f, 0f, 1f));
			this._targetOrbitPosition = vector + Vector2.Up * (this._spineNode.Scale.Y - 1f) * 100f;
			if (!this._isAttacking)
			{
				this._spineNode.GlobalPosition = this._spineNode.GlobalPosition.Lerp(this._targetOrbitPosition, (float)delta * 7f);
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x000A3034 File Offset: 0x000A1234
		public void Forge(float bladeDamage = 0f, bool showFlames = false)
		{
			if (this._isForging)
			{
				this.CleanupForge();
			}
			this._bladeSize = Mathf.Clamp(Mathf.Lerp(0f, 1f, bladeDamage / 200f), 0f, 1f);
			this._isForging = true;
			int num = (int)(this._bladeSize * 30f);
			if (num > 0)
			{
				this._chargeParticles.Amount = num;
				this._chargeParticles.Emitting = true;
			}
			else
			{
				this._chargeParticles.Emitting = false;
			}
			this._hilt.Visible = this._bladeSize < 0.3f;
			this._hilt2.Visible = !this._hilt.Visible;
			this._spikeParticles.Emitting = (this._spikeParticles.Visible = this._hilt.Visible);
			this._spikeParticles2.Emitting = (this._spikeParticles2.Visible = !this._hilt.Visible);
			this._spikeCircle.Emitting = (this._spikeCircle.Visible = this._hilt.Visible);
			this._spikeCircle2.Emitting = (this._spikeCircle2.Visible = !this._hilt.Visible);
			this._detail.Visible = bladeDamage >= 0.66f;
			this._bladeGlow.Visible = true;
			Color color = Color.FromHtml("#ff7300");
			Color color2 = color;
			color2.A = 0f;
			this._glowTween = base.CreateTween();
			if (showFlames)
			{
				this.FireFlames();
			}
			this._glowTween.TweenProperty(this._bladeGlow, "modulate", color, 0.05).SetEase(1L);
			this._glowTween.Chain().TweenProperty(this._bladeGlow, "modulate", color2, 0.5).SetEase(0L)
				.SetTrans(7L);
			this._glowTween.Chain().TweenCallback(Callable.From(new Action(this.CleanupForge)));
			Vector2 vector = Vector2.One * Mathf.Lerp(0.9f, 2f, this._bladeSize);
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenProperty(this._spineNode, "scale", vector * 1.2f, 0.05000000074505806).SetEase(1L).SetTrans(7L);
			this._scaleTween.Chain().TweenCallback(Callable.From(new Action(this.FireSparks)));
			this._scaleTween.Chain().TweenProperty(this._spineNode, "scale", vector, 0.30000001192092896).SetEase(2L)
				.SetTrans(7L);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000A3340 File Offset: 0x000A1540
		public void Attack(Vector2 targetPos)
		{
			if (this._isAttacking)
			{
				this.CleanupAttack();
			}
			this._isAttacking = true;
			this._animController.GetAnimationState().SetAnimation("attack", false, 0);
			this._attackTween = base.CreateTween();
			Vector2 vector;
			vector..ctor(this._spineNode.GlobalPosition.X - 50f, this._spineNode.GlobalPosition.Y);
			this._trail.Visible = true;
			this._trailStart = vector;
			this._attackTween.TweenProperty(this._spineNode, "rotation", this._spineNode.GetAngleTo(targetPos), 0.05000000074505806);
			this._attackTween.Parallel().TweenProperty(this._spineNode, "global_position", vector, 0.07999999821186066).SetEase(1L)
				.SetTrans(5L);
			this._attackTween.Chain().TweenProperty(this._spineNode, "rotation", this._spineNode.GetAngleTo(targetPos), 0.0);
			this._attackTween.Parallel().TweenProperty(this._spineNode, "global_position", targetPos, 0.05000000074505806).SetEase(0L)
				.SetTrans(5L);
			this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.EndSlash)));
			this._attackTween.TweenInterval(0.25);
			this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.FireSparks))).SetDelay(0.30000001192092896);
			this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.CleanupAttack)));
			this.UpdateHoverTip();
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000A3541 File Offset: 0x000A1741
		private void OnTargetingBegan()
		{
			this._hitbox.MouseFilter = 2L;
			this.UpdateHoverTip();
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000A3556 File Offset: 0x000A1756
		private void OnTargetingEnded()
		{
			this._hitbox.MouseFilter = 0L;
			this.UpdateHoverTip();
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000A356B File Offset: 0x000A176B
		private void OnFocused()
		{
			this._isFocused = true;
			if (!NCombatRoom.Instance.Ui.Hand.InCardPlay)
			{
				this.UpdateHoverTip();
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000A3590 File Offset: 0x000A1790
		private void OnUnfocused()
		{
			this._isFocused = false;
			this.UpdateHoverTip();
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000A35A0 File Offset: 0x000A17A0
		private void UpdateHoverTip()
		{
			bool flag = this._isFocused && !this._isAttacking && !NTargetManager.Instance.IsInSelection && this._hitbox.MouseFilter != 2L;
			if (flag)
			{
				this._selectionReticle.OnSelect();
				if (this._hoverTip == null)
				{
					this._hoverTip = NHoverTipSet.CreateAndShow(this._hitbox, HoverTipFactory.FromCard(this.Card, false), HoverTipAlignment.None);
					NHoverTipSet hoverTip = this._hoverTip;
					if (hoverTip == null)
					{
						return;
					}
					hoverTip.SetGlobalPosition(this._hitbox.GlobalPosition + Vector2.Right * this._hitbox.Size.X, false);
					return;
				}
			}
			else if (!flag)
			{
				this._selectionReticle.OnDeselect();
				if (this._hoverTip != null)
				{
					NHoverTipSet.Remove(this._hitbox);
					this._hoverTip = null;
				}
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000A367A File Offset: 0x000A187A
		private void FireSparks()
		{
			this._forgeSparks.Restart();
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x000A3687 File Offset: 0x000A1887
		private void FireFlames()
		{
			this._spawnFlames.Restart();
			this._spawnFlamesBack.Restart();
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000A36A0 File Offset: 0x000A18A0
		private void EndSlash()
		{
			this._chargeParticles.Emitting = false;
			this._chargeParticles.Restart();
			this._slashParticles.Rotation = this._spineNode.GetAngleTo(this._trailStart) - 1.5708f;
			this._slashParticles.Restart();
			this._trail.AddPoint(this._trailStart, -1);
			this._trail.AddPoint(base.GetNode<Node2D>("SpineSword/SwordBone/ScaleContainer/SpikeCircle").GlobalPosition, -1);
			this._trail.Modulate = Colors.White;
			this._trailFadeTween = base.CreateTween();
			this._trailFadeTween.TweenProperty(this._trail, "modulate:a", 0f, 0.20000000298023224);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x000A376F File Offset: 0x000A196F
		private void CleanupForge()
		{
			this._isForging = false;
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			Tween glowTween = this._glowTween;
			if (glowTween == null)
			{
				return;
			}
			glowTween.Kill();
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000A379C File Offset: 0x000A199C
		private void CleanupAttack()
		{
			this._isAttacking = false;
			Tween attackTween = this._attackTween;
			if (attackTween != null)
			{
				attackTween.Kill();
			}
			this._animController.GetAnimationState().SetAnimation("idle_loop", true, 0);
			this._spineNode.Rotation = 0f;
			this._trail.ClearPoints();
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000A37F4 File Offset: 0x000A19F4
		public void RemoveSovereignBlade()
		{
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenProperty(this._spineNode, "scale", Vector2.Zero, 0.20000000298023224).SetEase(1L).SetTrans(7L);
			this._scaleTween.Chain().TweenCallback(Callable.From(new Action(base.QueueFreeSafely)));
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000A387D File Offset: 0x000A1A7D
		private void OnOwnerDied(Creature creature)
		{
			this._hitbox.MouseFilter = 2L;
			this.UpdateHoverTip();
			this.RemoveSovereignBlade();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019D4 RID: 6612 RVA: 0x000A3898 File Offset: 0x000A1A98
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(16)
			{
				new MethodInfo(NSovereignBladeVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.Forge, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "bladeDamage", 0L, "", 6L, false),
					new PropertyInfo(1L, "showFlames", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.Attack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetPos", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.OnTargetingBegan, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.OnTargetingEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.OnFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.OnUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.UpdateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.FireSparks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.FireFlames, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.EndSlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.CleanupForge, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.CleanupAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSovereignBladeVfx.MethodName.RemoveSovereignBlade, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019D5 RID: 6613 RVA: 0x000A3C30 File Offset: 0x000A1E30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSovereignBladeVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.Forge && args.Count == 2)
			{
				this.Forge(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.Attack && args.Count == 1)
			{
				this.Attack(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.OnTargetingBegan && args.Count == 0)
			{
				this.OnTargetingBegan();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.OnTargetingEnded && args.Count == 0)
			{
				this.OnTargetingEnded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.OnFocused && args.Count == 0)
			{
				this.OnFocused();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.OnUnfocused && args.Count == 0)
			{
				this.OnUnfocused();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.UpdateHoverTip && args.Count == 0)
			{
				this.UpdateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.FireSparks && args.Count == 0)
			{
				this.FireSparks();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.FireFlames && args.Count == 0)
			{
				this.FireFlames();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.EndSlash && args.Count == 0)
			{
				this.EndSlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.CleanupForge && args.Count == 0)
			{
				this.CleanupForge();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.CleanupAttack && args.Count == 0)
			{
				this.CleanupAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSovereignBladeVfx.MethodName.RemoveSovereignBlade && args.Count == 0)
			{
				this.RemoveSovereignBlade();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060019D6 RID: 6614 RVA: 0x000A3ED0 File Offset: 0x000A20D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSovereignBladeVfx.MethodName._Ready || (ref method) == NSovereignBladeVfx.MethodName._ExitTree || (ref method) == NSovereignBladeVfx.MethodName._Process || (ref method) == NSovereignBladeVfx.MethodName.Forge || (ref method) == NSovereignBladeVfx.MethodName.Attack || (ref method) == NSovereignBladeVfx.MethodName.OnTargetingBegan || (ref method) == NSovereignBladeVfx.MethodName.OnTargetingEnded || (ref method) == NSovereignBladeVfx.MethodName.OnFocused || (ref method) == NSovereignBladeVfx.MethodName.OnUnfocused || (ref method) == NSovereignBladeVfx.MethodName.UpdateHoverTip || (ref method) == NSovereignBladeVfx.MethodName.FireSparks || (ref method) == NSovereignBladeVfx.MethodName.FireFlames || (ref method) == NSovereignBladeVfx.MethodName.EndSlash || (ref method) == NSovereignBladeVfx.MethodName.CleanupForge || (ref method) == NSovereignBladeVfx.MethodName.CleanupAttack || (ref method) == NSovereignBladeVfx.MethodName.RemoveSovereignBlade || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060019D7 RID: 6615 RVA: 0x000A3FD4 File Offset: 0x000A21D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSovereignBladeVfx.PropertyName.OrbitProgress)
			{
				this.OrbitProgress = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spineNode)
			{
				this._spineNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._bladeGlow)
			{
				this._bladeGlow = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._forgeSparks)
			{
				this._forgeSparks = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spawnFlames)
			{
				this._spawnFlames = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spawnFlamesBack)
			{
				this._spawnFlamesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._slashParticles)
			{
				this._slashParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._chargeParticles)
			{
				this._chargeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeParticles)
			{
				this._spikeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeParticles2)
			{
				this._spikeParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeCircle)
			{
				this._spikeCircle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeCircle2)
			{
				this._spikeCircle2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hilt)
			{
				this._hilt = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hilt2)
			{
				this._hilt2 = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._detail)
			{
				this._detail = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trail)
			{
				this._trail = VariantUtils.ConvertTo<Line2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._orbitPath)
			{
				this._orbitPath = VariantUtils.ConvertTo<Path2D>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hitbox)
			{
				this._hitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._attackTween)
			{
				this._attackTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._sparkDelay)
			{
				this._sparkDelay = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._glowTween)
			{
				this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trailFadeTween)
			{
				this._trailFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trailStart)
			{
				this._trailStart = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._bladeSize)
			{
				this._bladeSize = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._targetOrbitPosition)
			{
				this._targetOrbitPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isBehindCharacter)
			{
				this._isBehindCharacter = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isFocused)
			{
				this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hoverTip)
			{
				this._hoverTip = VariantUtils.ConvertTo<NHoverTipSet>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isForging)
			{
				this._isForging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isAttacking)
			{
				this._isAttacking = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isKeyPressed)
			{
				this._isKeyPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._testCharge)
			{
				this._testCharge = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060019D8 RID: 6616 RVA: 0x000A4380 File Offset: 0x000A2580
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSovereignBladeVfx.PropertyName.OrbitProgress)
			{
				double orbitProgress = this.OrbitProgress;
				value = VariantUtils.CreateFrom<double>(ref orbitProgress);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spineNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._spineNode);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._bladeGlow)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._bladeGlow);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._forgeSparks)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._forgeSparks);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spawnFlames)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spawnFlames);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spawnFlamesBack)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spawnFlamesBack);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._slashParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slashParticles);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._chargeParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._chargeParticles);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeParticles);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeParticles2);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeCircle)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeCircle);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._spikeCircle2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeCircle2);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hilt)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._hilt);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hilt2)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._hilt2);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._detail)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._detail);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trail)
			{
				value = VariantUtils.CreateFrom<Line2D>(ref this._trail);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._orbitPath)
			{
				value = VariantUtils.CreateFrom<Path2D>(ref this._orbitPath);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hitbox)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hitbox);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._attackTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._attackTween);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._sparkDelay)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._sparkDelay);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._glowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trailFadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._trailFadeTween);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._trailStart)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._trailStart);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._bladeSize)
			{
				value = VariantUtils.CreateFrom<float>(ref this._bladeSize);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._targetOrbitPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetOrbitPosition);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isBehindCharacter)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isBehindCharacter);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isFocused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._hoverTip)
			{
				value = VariantUtils.CreateFrom<NHoverTipSet>(ref this._hoverTip);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isForging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isForging);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isAttacking)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAttacking);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._isKeyPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isKeyPressed);
				return true;
			}
			if ((ref name) == NSovereignBladeVfx.PropertyName._testCharge)
			{
				value = VariantUtils.CreateFrom<float>(ref this._testCharge);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019D9 RID: 6617 RVA: 0x000A47D8 File Offset: 0x000A29D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spineNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._bladeGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._forgeSparks, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spawnFlames, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spawnFlamesBack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._slashParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._chargeParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spikeParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spikeParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spikeCircle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._spikeCircle2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._hilt, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._hilt2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._detail, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._trail, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._orbitPath, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._attackTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._sparkDelay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._glowTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._trailFadeTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSovereignBladeVfx.PropertyName._trailStart, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSovereignBladeVfx.PropertyName._bladeSize, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSovereignBladeVfx.PropertyName.OrbitProgress, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSovereignBladeVfx.PropertyName._targetOrbitPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSovereignBladeVfx.PropertyName._isBehindCharacter, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSovereignBladeVfx.PropertyName._isFocused, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSovereignBladeVfx.PropertyName._hoverTip, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSovereignBladeVfx.PropertyName._isForging, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSovereignBladeVfx.PropertyName._isAttacking, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSovereignBladeVfx.PropertyName._isKeyPressed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSovereignBladeVfx.PropertyName._testCharge, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019DA RID: 6618 RVA: 0x000A4C44 File Offset: 0x000A2E44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName orbitProgress = NSovereignBladeVfx.PropertyName.OrbitProgress;
			double orbitProgress2 = this.OrbitProgress;
			info.AddProperty(orbitProgress, Variant.From<double>(ref orbitProgress2));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spineNode, Variant.From<Node2D>(ref this._spineNode));
			info.AddProperty(NSovereignBladeVfx.PropertyName._bladeGlow, Variant.From<Node2D>(ref this._bladeGlow));
			info.AddProperty(NSovereignBladeVfx.PropertyName._forgeSparks, Variant.From<GpuParticles2D>(ref this._forgeSparks));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spawnFlames, Variant.From<GpuParticles2D>(ref this._spawnFlames));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spawnFlamesBack, Variant.From<GpuParticles2D>(ref this._spawnFlamesBack));
			info.AddProperty(NSovereignBladeVfx.PropertyName._slashParticles, Variant.From<GpuParticles2D>(ref this._slashParticles));
			info.AddProperty(NSovereignBladeVfx.PropertyName._chargeParticles, Variant.From<GpuParticles2D>(ref this._chargeParticles));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spikeParticles, Variant.From<GpuParticles2D>(ref this._spikeParticles));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spikeParticles2, Variant.From<GpuParticles2D>(ref this._spikeParticles2));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spikeCircle, Variant.From<GpuParticles2D>(ref this._spikeCircle));
			info.AddProperty(NSovereignBladeVfx.PropertyName._spikeCircle2, Variant.From<GpuParticles2D>(ref this._spikeCircle2));
			info.AddProperty(NSovereignBladeVfx.PropertyName._hilt, Variant.From<TextureRect>(ref this._hilt));
			info.AddProperty(NSovereignBladeVfx.PropertyName._hilt2, Variant.From<TextureRect>(ref this._hilt2));
			info.AddProperty(NSovereignBladeVfx.PropertyName._detail, Variant.From<TextureRect>(ref this._detail));
			info.AddProperty(NSovereignBladeVfx.PropertyName._trail, Variant.From<Line2D>(ref this._trail));
			info.AddProperty(NSovereignBladeVfx.PropertyName._orbitPath, Variant.From<Path2D>(ref this._orbitPath));
			info.AddProperty(NSovereignBladeVfx.PropertyName._hitbox, Variant.From<Control>(ref this._hitbox));
			info.AddProperty(NSovereignBladeVfx.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NSovereignBladeVfx.PropertyName._attackTween, Variant.From<Tween>(ref this._attackTween));
			info.AddProperty(NSovereignBladeVfx.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NSovereignBladeVfx.PropertyName._sparkDelay, Variant.From<Tween>(ref this._sparkDelay));
			info.AddProperty(NSovereignBladeVfx.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
			info.AddProperty(NSovereignBladeVfx.PropertyName._trailFadeTween, Variant.From<Tween>(ref this._trailFadeTween));
			info.AddProperty(NSovereignBladeVfx.PropertyName._trailStart, Variant.From<Vector2>(ref this._trailStart));
			info.AddProperty(NSovereignBladeVfx.PropertyName._bladeSize, Variant.From<float>(ref this._bladeSize));
			info.AddProperty(NSovereignBladeVfx.PropertyName._targetOrbitPosition, Variant.From<Vector2>(ref this._targetOrbitPosition));
			info.AddProperty(NSovereignBladeVfx.PropertyName._isBehindCharacter, Variant.From<bool>(ref this._isBehindCharacter));
			info.AddProperty(NSovereignBladeVfx.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
			info.AddProperty(NSovereignBladeVfx.PropertyName._hoverTip, Variant.From<NHoverTipSet>(ref this._hoverTip));
			info.AddProperty(NSovereignBladeVfx.PropertyName._isForging, Variant.From<bool>(ref this._isForging));
			info.AddProperty(NSovereignBladeVfx.PropertyName._isAttacking, Variant.From<bool>(ref this._isAttacking));
			info.AddProperty(NSovereignBladeVfx.PropertyName._isKeyPressed, Variant.From<bool>(ref this._isKeyPressed));
			info.AddProperty(NSovereignBladeVfx.PropertyName._testCharge, Variant.From<float>(ref this._testCharge));
		}

		/// <inheritdoc />
		// Token: 0x060019DB RID: 6619 RVA: 0x000A4F48 File Offset: 0x000A3148
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName.OrbitProgress, ref variant))
			{
				this.OrbitProgress = variant.As<double>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spineNode, ref variant2))
			{
				this._spineNode = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._bladeGlow, ref variant3))
			{
				this._bladeGlow = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._forgeSparks, ref variant4))
			{
				this._forgeSparks = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spawnFlames, ref variant5))
			{
				this._spawnFlames = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spawnFlamesBack, ref variant6))
			{
				this._spawnFlamesBack = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._slashParticles, ref variant7))
			{
				this._slashParticles = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._chargeParticles, ref variant8))
			{
				this._chargeParticles = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeParticles, ref variant9))
			{
				this._spikeParticles = variant9.As<GpuParticles2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeParticles2, ref variant10))
			{
				this._spikeParticles2 = variant10.As<GpuParticles2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeCircle, ref variant11))
			{
				this._spikeCircle = variant11.As<GpuParticles2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeCircle2, ref variant12))
			{
				this._spikeCircle2 = variant12.As<GpuParticles2D>();
			}
			Variant variant13;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hilt, ref variant13))
			{
				this._hilt = variant13.As<TextureRect>();
			}
			Variant variant14;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hilt2, ref variant14))
			{
				this._hilt2 = variant14.As<TextureRect>();
			}
			Variant variant15;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._detail, ref variant15))
			{
				this._detail = variant15.As<TextureRect>();
			}
			Variant variant16;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trail, ref variant16))
			{
				this._trail = variant16.As<Line2D>();
			}
			Variant variant17;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._orbitPath, ref variant17))
			{
				this._orbitPath = variant17.As<Path2D>();
			}
			Variant variant18;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hitbox, ref variant18))
			{
				this._hitbox = variant18.As<Control>();
			}
			Variant variant19;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._selectionReticle, ref variant19))
			{
				this._selectionReticle = variant19.As<NSelectionReticle>();
			}
			Variant variant20;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._attackTween, ref variant20))
			{
				this._attackTween = variant20.As<Tween>();
			}
			Variant variant21;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._scaleTween, ref variant21))
			{
				this._scaleTween = variant21.As<Tween>();
			}
			Variant variant22;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._sparkDelay, ref variant22))
			{
				this._sparkDelay = variant22.As<Tween>();
			}
			Variant variant23;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._glowTween, ref variant23))
			{
				this._glowTween = variant23.As<Tween>();
			}
			Variant variant24;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trailFadeTween, ref variant24))
			{
				this._trailFadeTween = variant24.As<Tween>();
			}
			Variant variant25;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trailStart, ref variant25))
			{
				this._trailStart = variant25.As<Vector2>();
			}
			Variant variant26;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._bladeSize, ref variant26))
			{
				this._bladeSize = variant26.As<float>();
			}
			Variant variant27;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._targetOrbitPosition, ref variant27))
			{
				this._targetOrbitPosition = variant27.As<Vector2>();
			}
			Variant variant28;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isBehindCharacter, ref variant28))
			{
				this._isBehindCharacter = variant28.As<bool>();
			}
			Variant variant29;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isFocused, ref variant29))
			{
				this._isFocused = variant29.As<bool>();
			}
			Variant variant30;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hoverTip, ref variant30))
			{
				this._hoverTip = variant30.As<NHoverTipSet>();
			}
			Variant variant31;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isForging, ref variant31))
			{
				this._isForging = variant31.As<bool>();
			}
			Variant variant32;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isAttacking, ref variant32))
			{
				this._isAttacking = variant32.As<bool>();
			}
			Variant variant33;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isKeyPressed, ref variant33))
			{
				this._isKeyPressed = variant33.As<bool>();
			}
			Variant variant34;
			if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._testCharge, ref variant34))
			{
				this._testCharge = variant34.As<float>();
			}
		}

		// Token: 0x04000967 RID: 2407
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/sovereign_blade");

		// Token: 0x04000969 RID: 2409
		private Player _owner;

		// Token: 0x0400096A RID: 2410
		private Node2D _spineNode;

		// Token: 0x0400096B RID: 2411
		private MegaSprite _animController;

		// Token: 0x0400096C RID: 2412
		private Node2D _bladeGlow;

		// Token: 0x0400096D RID: 2413
		private GpuParticles2D _forgeSparks;

		// Token: 0x0400096E RID: 2414
		private GpuParticles2D _spawnFlames;

		// Token: 0x0400096F RID: 2415
		private GpuParticles2D _spawnFlamesBack;

		// Token: 0x04000970 RID: 2416
		private GpuParticles2D _slashParticles;

		// Token: 0x04000971 RID: 2417
		private GpuParticles2D _chargeParticles;

		// Token: 0x04000972 RID: 2418
		private GpuParticles2D _spikeParticles;

		// Token: 0x04000973 RID: 2419
		private GpuParticles2D _spikeParticles2;

		// Token: 0x04000974 RID: 2420
		private GpuParticles2D _spikeCircle;

		// Token: 0x04000975 RID: 2421
		private GpuParticles2D _spikeCircle2;

		// Token: 0x04000976 RID: 2422
		private TextureRect _hilt;

		// Token: 0x04000977 RID: 2423
		private TextureRect _hilt2;

		// Token: 0x04000978 RID: 2424
		private TextureRect _detail;

		// Token: 0x04000979 RID: 2425
		private Line2D _trail;

		// Token: 0x0400097A RID: 2426
		private Path2D _orbitPath;

		// Token: 0x0400097B RID: 2427
		private Control _hitbox;

		// Token: 0x0400097C RID: 2428
		private NSelectionReticle _selectionReticle;

		// Token: 0x0400097D RID: 2429
		[Nullable(2)]
		private Tween _attackTween;

		// Token: 0x0400097E RID: 2430
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x0400097F RID: 2431
		[Nullable(2)]
		private Tween _sparkDelay;

		// Token: 0x04000980 RID: 2432
		[Nullable(2)]
		private Tween _glowTween;

		// Token: 0x04000981 RID: 2433
		[Nullable(2)]
		private Tween _trailFadeTween;

		// Token: 0x04000982 RID: 2434
		private Vector2 _trailStart;

		// Token: 0x04000983 RID: 2435
		private float _bladeSize;

		// Token: 0x04000985 RID: 2437
		private const float _orbitSpeed = 60f;

		// Token: 0x04000986 RID: 2438
		private Vector2 _targetOrbitPosition;

		// Token: 0x04000987 RID: 2439
		private bool _isBehindCharacter;

		// Token: 0x04000988 RID: 2440
		private const float _hiltThreshold = 0.3f;

		// Token: 0x04000989 RID: 2441
		private const float _detailThreshold = 0.66f;

		// Token: 0x0400098A RID: 2442
		private bool _isFocused;

		// Token: 0x0400098B RID: 2443
		[Nullable(2)]
		private NHoverTipSet _hoverTip;

		// Token: 0x0400098C RID: 2444
		private bool _isForging;

		// Token: 0x0400098D RID: 2445
		private bool _isAttacking;

		// Token: 0x0400098E RID: 2446
		private bool _isKeyPressed;

		// Token: 0x0400098F RID: 2447
		private float _testCharge;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001149 RID: 4425
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004365 RID: 17253
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004366 RID: 17254
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004367 RID: 17255
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'Forge' method.
			/// </summary>
			// Token: 0x04004368 RID: 17256
			public static readonly StringName Forge = "Forge";

			/// <summary>
			/// Cached name for the 'Attack' method.
			/// </summary>
			// Token: 0x04004369 RID: 17257
			public static readonly StringName Attack = "Attack";

			/// <summary>
			/// Cached name for the 'OnTargetingBegan' method.
			/// </summary>
			// Token: 0x0400436A RID: 17258
			public static readonly StringName OnTargetingBegan = "OnTargetingBegan";

			/// <summary>
			/// Cached name for the 'OnTargetingEnded' method.
			/// </summary>
			// Token: 0x0400436B RID: 17259
			public static readonly StringName OnTargetingEnded = "OnTargetingEnded";

			/// <summary>
			/// Cached name for the 'OnFocused' method.
			/// </summary>
			// Token: 0x0400436C RID: 17260
			public static readonly StringName OnFocused = "OnFocused";

			/// <summary>
			/// Cached name for the 'OnUnfocused' method.
			/// </summary>
			// Token: 0x0400436D RID: 17261
			public static readonly StringName OnUnfocused = "OnUnfocused";

			/// <summary>
			/// Cached name for the 'UpdateHoverTip' method.
			/// </summary>
			// Token: 0x0400436E RID: 17262
			public static readonly StringName UpdateHoverTip = "UpdateHoverTip";

			/// <summary>
			/// Cached name for the 'FireSparks' method.
			/// </summary>
			// Token: 0x0400436F RID: 17263
			public static readonly StringName FireSparks = "FireSparks";

			/// <summary>
			/// Cached name for the 'FireFlames' method.
			/// </summary>
			// Token: 0x04004370 RID: 17264
			public static readonly StringName FireFlames = "FireFlames";

			/// <summary>
			/// Cached name for the 'EndSlash' method.
			/// </summary>
			// Token: 0x04004371 RID: 17265
			public static readonly StringName EndSlash = "EndSlash";

			/// <summary>
			/// Cached name for the 'CleanupForge' method.
			/// </summary>
			// Token: 0x04004372 RID: 17266
			public static readonly StringName CleanupForge = "CleanupForge";

			/// <summary>
			/// Cached name for the 'CleanupAttack' method.
			/// </summary>
			// Token: 0x04004373 RID: 17267
			public static readonly StringName CleanupAttack = "CleanupAttack";

			/// <summary>
			/// Cached name for the 'RemoveSovereignBlade' method.
			/// </summary>
			// Token: 0x04004374 RID: 17268
			public static readonly StringName RemoveSovereignBlade = "RemoveSovereignBlade";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200114A RID: 4426
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'OrbitProgress' property.
			/// </summary>
			// Token: 0x04004375 RID: 17269
			public static readonly StringName OrbitProgress = "OrbitProgress";

			/// <summary>
			/// Cached name for the '_spineNode' field.
			/// </summary>
			// Token: 0x04004376 RID: 17270
			public static readonly StringName _spineNode = "_spineNode";

			/// <summary>
			/// Cached name for the '_bladeGlow' field.
			/// </summary>
			// Token: 0x04004377 RID: 17271
			public static readonly StringName _bladeGlow = "_bladeGlow";

			/// <summary>
			/// Cached name for the '_forgeSparks' field.
			/// </summary>
			// Token: 0x04004378 RID: 17272
			public static readonly StringName _forgeSparks = "_forgeSparks";

			/// <summary>
			/// Cached name for the '_spawnFlames' field.
			/// </summary>
			// Token: 0x04004379 RID: 17273
			public static readonly StringName _spawnFlames = "_spawnFlames";

			/// <summary>
			/// Cached name for the '_spawnFlamesBack' field.
			/// </summary>
			// Token: 0x0400437A RID: 17274
			public static readonly StringName _spawnFlamesBack = "_spawnFlamesBack";

			/// <summary>
			/// Cached name for the '_slashParticles' field.
			/// </summary>
			// Token: 0x0400437B RID: 17275
			public static readonly StringName _slashParticles = "_slashParticles";

			/// <summary>
			/// Cached name for the '_chargeParticles' field.
			/// </summary>
			// Token: 0x0400437C RID: 17276
			public static readonly StringName _chargeParticles = "_chargeParticles";

			/// <summary>
			/// Cached name for the '_spikeParticles' field.
			/// </summary>
			// Token: 0x0400437D RID: 17277
			public static readonly StringName _spikeParticles = "_spikeParticles";

			/// <summary>
			/// Cached name for the '_spikeParticles2' field.
			/// </summary>
			// Token: 0x0400437E RID: 17278
			public static readonly StringName _spikeParticles2 = "_spikeParticles2";

			/// <summary>
			/// Cached name for the '_spikeCircle' field.
			/// </summary>
			// Token: 0x0400437F RID: 17279
			public static readonly StringName _spikeCircle = "_spikeCircle";

			/// <summary>
			/// Cached name for the '_spikeCircle2' field.
			/// </summary>
			// Token: 0x04004380 RID: 17280
			public static readonly StringName _spikeCircle2 = "_spikeCircle2";

			/// <summary>
			/// Cached name for the '_hilt' field.
			/// </summary>
			// Token: 0x04004381 RID: 17281
			public static readonly StringName _hilt = "_hilt";

			/// <summary>
			/// Cached name for the '_hilt2' field.
			/// </summary>
			// Token: 0x04004382 RID: 17282
			public static readonly StringName _hilt2 = "_hilt2";

			/// <summary>
			/// Cached name for the '_detail' field.
			/// </summary>
			// Token: 0x04004383 RID: 17283
			public static readonly StringName _detail = "_detail";

			/// <summary>
			/// Cached name for the '_trail' field.
			/// </summary>
			// Token: 0x04004384 RID: 17284
			public static readonly StringName _trail = "_trail";

			/// <summary>
			/// Cached name for the '_orbitPath' field.
			/// </summary>
			// Token: 0x04004385 RID: 17285
			public static readonly StringName _orbitPath = "_orbitPath";

			/// <summary>
			/// Cached name for the '_hitbox' field.
			/// </summary>
			// Token: 0x04004386 RID: 17286
			public static readonly StringName _hitbox = "_hitbox";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004387 RID: 17287
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_attackTween' field.
			/// </summary>
			// Token: 0x04004388 RID: 17288
			public static readonly StringName _attackTween = "_attackTween";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x04004389 RID: 17289
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_sparkDelay' field.
			/// </summary>
			// Token: 0x0400438A RID: 17290
			public static readonly StringName _sparkDelay = "_sparkDelay";

			/// <summary>
			/// Cached name for the '_glowTween' field.
			/// </summary>
			// Token: 0x0400438B RID: 17291
			public static readonly StringName _glowTween = "_glowTween";

			/// <summary>
			/// Cached name for the '_trailFadeTween' field.
			/// </summary>
			// Token: 0x0400438C RID: 17292
			public static readonly StringName _trailFadeTween = "_trailFadeTween";

			/// <summary>
			/// Cached name for the '_trailStart' field.
			/// </summary>
			// Token: 0x0400438D RID: 17293
			public static readonly StringName _trailStart = "_trailStart";

			/// <summary>
			/// Cached name for the '_bladeSize' field.
			/// </summary>
			// Token: 0x0400438E RID: 17294
			public static readonly StringName _bladeSize = "_bladeSize";

			/// <summary>
			/// Cached name for the '_targetOrbitPosition' field.
			/// </summary>
			// Token: 0x0400438F RID: 17295
			public static readonly StringName _targetOrbitPosition = "_targetOrbitPosition";

			/// <summary>
			/// Cached name for the '_isBehindCharacter' field.
			/// </summary>
			// Token: 0x04004390 RID: 17296
			public static readonly StringName _isBehindCharacter = "_isBehindCharacter";

			/// <summary>
			/// Cached name for the '_isFocused' field.
			/// </summary>
			// Token: 0x04004391 RID: 17297
			public static readonly StringName _isFocused = "_isFocused";

			/// <summary>
			/// Cached name for the '_hoverTip' field.
			/// </summary>
			// Token: 0x04004392 RID: 17298
			public static readonly StringName _hoverTip = "_hoverTip";

			/// <summary>
			/// Cached name for the '_isForging' field.
			/// </summary>
			// Token: 0x04004393 RID: 17299
			public static readonly StringName _isForging = "_isForging";

			/// <summary>
			/// Cached name for the '_isAttacking' field.
			/// </summary>
			// Token: 0x04004394 RID: 17300
			public static readonly StringName _isAttacking = "_isAttacking";

			/// <summary>
			/// Cached name for the '_isKeyPressed' field.
			/// </summary>
			// Token: 0x04004395 RID: 17301
			public static readonly StringName _isKeyPressed = "_isKeyPressed";

			/// <summary>
			/// Cached name for the '_testCharge' field.
			/// </summary>
			// Token: 0x04004396 RID: 17302
			public static readonly StringName _testCharge = "_testCharge";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200114B RID: 4427
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
