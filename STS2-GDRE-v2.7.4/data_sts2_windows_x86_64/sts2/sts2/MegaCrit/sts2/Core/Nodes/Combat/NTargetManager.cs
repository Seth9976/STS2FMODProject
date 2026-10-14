using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000411 RID: 1041
	[ScriptPath("res://src/Core/Nodes/Combat/NTargetManager.cs")]
	public class NTargetManager : Node2D
	{
		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x001FAD3B File Offset: 0x001F8F3B
		[Nullable(1)]
		public static NTargetManager Instance
		{
			[NullableContext(1)]
			get
			{
				return NRun.Instance.GlobalUi.TargetManager;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x001FAD4C File Offset: 0x001F8F4C
		public bool IsInSelection
		{
			get
			{
				return this._targetMode > TargetMode.None;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x001FAD57 File Offset: 0x001F8F57
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x001FAD5F File Offset: 0x001F8F5F
		[Nullable(2)]
		private Node HoveredNode
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// This is the frame on which the last targeting was cancelled or finished as reported by SceneTree.GetFrame.
		/// </summary>
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x001FAD68 File Offset: 0x001F8F68
		// (set) Token: 0x0600418E RID: 16782 RVA: 0x001FAD70 File Offset: 0x001F8F70
		public long LastTargetingFinishedFrame { get; set; }

		// Token: 0x0600418F RID: 16783 RVA: 0x001FAD79 File Offset: 0x001F8F79
		public override void _Ready()
		{
			this._targetingArrow = base.GetNode<NTargetingArrow>("TargetingArrow");
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x001FAD94 File Offset: 0x001F8F94
		public override void _EnterTree()
		{
			base._EnterTree();
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.CancelTargeting)), 0U);
				NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.CancelTargeting)), 0U);
			}
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x001FAE08 File Offset: 0x001F9008
		public override void _ExitTree()
		{
			base._ExitTree();
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.CancelTargeting)));
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.CancelTargeting)));
			}
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x001FAE78 File Offset: 0x001F9078
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (!this.IsInSelection)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				if (buttonIndex != 1L)
				{
					if (buttonIndex == 2L)
					{
						flag = inputEventMouseButton.IsPressed();
						flag2 = true;
					}
				}
				else if (inputEventMouseButton.IsReleased())
				{
					TargetMode targetMode = this._targetMode;
					if (targetMode != TargetMode.ReleaseMouseToTarget)
					{
						if (targetMode == TargetMode.ClickMouseToTarget)
						{
							flag = true;
						}
					}
					else if (this.HoveredNode != null)
					{
						flag = true;
					}
					else
					{
						this._targetMode = TargetMode.ClickMouseToTarget;
					}
				}
			}
			else if (inputEvent.IsActionPressed(MegaInput.select, false, false) && this.HoveredNode != null)
			{
				flag = true;
				flag2 = false;
				base.GetViewport().SetInputAsHandled();
			}
			else if (inputEvent.IsActionPressed(MegaInput.cancel, false, false) || inputEvent.IsActionPressed(MegaInput.topPanel, false, false))
			{
				flag = true;
				flag2 = true;
				if (inputEvent.IsActionPressed(MegaInput.cancel, false, false))
				{
					base.GetViewport().SetInputAsHandled();
				}
			}
			if (this._exitEarlyCondition != null && this._exitEarlyCondition())
			{
				flag = true;
				flag2 = true;
			}
			if (flag)
			{
				this.FinishTargeting(flag2);
			}
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x001FAF80 File Offset: 0x001F9180
		public override void _Process(double delta)
		{
			if (this._exitEarlyCondition != null && this._exitEarlyCondition())
			{
				this.FinishTargeting(true);
			}
			NCreature ncreature = this.HoveredNode as NCreature;
			if (ncreature != null)
			{
				Creature entity = ncreature.Entity;
				if (entity != null && !entity.IsHittable)
				{
					this.FinishTargeting(true);
				}
			}
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x001FAFD1 File Offset: 0x001F91D1
		[NullableContext(1)]
		private void OnCombatEnded(CombatRoom _)
		{
			if (this._exitEarlyCondition != null)
			{
				this.FinishTargeting(true);
			}
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x001FAFE2 File Offset: 0x001F91E2
		public void CancelTargeting()
		{
			if (this._targetMode != TargetMode.None)
			{
				this.FinishTargeting(true);
			}
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x001FAFF4 File Offset: 0x001F91F4
		private void FinishTargeting(bool cancel)
		{
			NHoverTipSet.shouldBlockHoverTips = false;
			this._exitEarlyCondition = null;
			this._completionSource.SetResult(cancel ? null : this.HoveredNode);
			this.LastTargetingFinishedFrame = base.GetTree().GetFrame();
			base.EmitSignal(NTargetManager.SignalName.TargetingEnded, Array.Empty<Variant>());
			this._targetMode = TargetMode.None;
			this._targetingArrow.StopDrawing();
			NCreature ncreature = this.HoveredNode as NCreature;
			if (ncreature != null)
			{
				ncreature.HideMultiselectReticle();
			}
			else
			{
				NRestSiteCharacter nrestSiteCharacter = this.HoveredNode as NRestSiteCharacter;
				if (nrestSiteCharacter != null)
				{
					nrestSiteCharacter.Deselect();
				}
			}
			this.HoveredNode = null;
			RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(false);
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x001FB09C File Offset: 0x001F929C
		[return: Nullable(new byte[] { 1, 2 })]
		public async Task<Node> SelectionFinished()
		{
			return await this._completionSource.Task;
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x001FB0E0 File Offset: 0x001F92E0
		[NullableContext(2)]
		public void StartTargeting(TargetType validTargetsType, Vector2 startPosition, TargetMode startingMode, Func<bool> exitEarlyCondition, [Nullable(new byte[] { 2, 1 })] Func<Node, bool> nodeFilter)
		{
			if (!validTargetsType.IsSingleTarget())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to begin targeting with invalid ActionTarget ");
				defaultInterpolatedStringHandler.AppendFormatted<TargetType>(validTargetsType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._validTargetsType = validTargetsType;
			this._targetingArrow.StartDrawingFrom(startPosition, startingMode == TargetMode.Controller);
			this._completionSource = new TaskCompletionSource<Node>();
			this._exitEarlyCondition = exitEarlyCondition;
			this._nodeFilter = nodeFilter;
			this._targetMode = startingMode;
			NHoverTipSet.shouldBlockHoverTips = true;
			base.EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
			RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(true);
			NCombatRoom instance = NCombatRoom.Instance;
			foreach (NCreature ncreature in (((instance != null) ? instance.CreatureNodes : null) ?? Array.Empty<NCreature>()))
			{
				ncreature.OnTargetingStarted();
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x001FB1E0 File Offset: 0x001F93E0
		[NullableContext(1)]
		public void StartTargeting(TargetType validTargetsType, Control control, TargetMode startingMode, [Nullable(2)] Func<bool> exitEarlyCondition, [Nullable(new byte[] { 2, 1 })] Func<Node, bool> nodeFilter)
		{
			if (!validTargetsType.IsSingleTarget())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to begin targeting with invalid ActionTarget ");
				defaultInterpolatedStringHandler.AppendFormatted<TargetType>(validTargetsType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._validTargetsType = validTargetsType;
			this._targetingArrow.StartDrawingFrom(control, startingMode == TargetMode.Controller);
			this._completionSource = new TaskCompletionSource<Node>();
			this._exitEarlyCondition = exitEarlyCondition;
			this._nodeFilter = nodeFilter;
			this._targetMode = startingMode;
			NHoverTipSet.shouldBlockHoverTips = true;
			base.EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
			RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(true);
			NCombatRoom instance = NCombatRoom.Instance;
			foreach (NCreature ncreature in (((instance != null) ? instance.CreatureNodes : null) ?? Array.Empty<NCreature>()))
			{
				ncreature.OnTargetingStarted();
			}
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x001FB2E0 File Offset: 0x001F94E0
		[NullableContext(1)]
		public bool AllowedToTargetNode(Node node)
		{
			if (this._nodeFilter != null && !this._nodeFilter(node))
			{
				return false;
			}
			NCreature ncreature = node as NCreature;
			if (ncreature != null)
			{
				return this.AllowedToTargetCreature(ncreature.Entity);
			}
			NMultiplayerPlayerState nmultiplayerPlayerState = node as NMultiplayerPlayerState;
			return nmultiplayerPlayerState == null || this.AllowedToTargetCreature(nmultiplayerPlayerState.Player.Creature);
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x001FB33C File Offset: 0x001F953C
		[NullableContext(1)]
		private bool AllowedToTargetCreature(Creature creature)
		{
			switch (this._validTargetsType)
			{
			case TargetType.AnyEnemy:
				if (creature.Side != CombatSide.Enemy || creature.IsDead)
				{
					return false;
				}
				return true;
			case TargetType.AnyPlayer:
				if (!creature.IsPlayer || creature.IsDead)
				{
					return false;
				}
				return true;
			case TargetType.AnyAlly:
				if (!creature.IsPlayer || creature.IsDead)
				{
					return false;
				}
				if (LocalContext.IsMe(creature.Player))
				{
					return false;
				}
				return true;
			}
			throw new ArgumentOutOfRangeException("_validTargetsType", this._validTargetsType, null);
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x001FB3CC File Offset: 0x001F95CC
		[NullableContext(1)]
		public void OnNodeHovered(Node node)
		{
			if (!this.IsInSelection)
			{
				return;
			}
			if (!this.AllowedToTargetNode(node))
			{
				return;
			}
			NCreature ncreature = node as NCreature;
			if (ncreature != null)
			{
				this.OnCreatureHovered(ncreature);
				return;
			}
			this.HoveredNode = node;
			this._targetingArrow.SetHighlightingOn(false);
			if (this._targetMode == TargetMode.Controller)
			{
				NMultiplayerPlayerState nmultiplayerPlayerState = node as NMultiplayerPlayerState;
				if (nmultiplayerPlayerState == null)
				{
					Control control = node as Control;
					if (control == null)
					{
						Node2D node2D = node as Node2D;
						if (node2D != null)
						{
							this._targetingArrow.UpdateDrawingTo(node2D.GlobalPosition);
						}
					}
					else
					{
						this._targetingArrow.UpdateDrawingTo(control.GlobalPosition + control.PivotOffset);
					}
				}
				else
				{
					this._targetingArrow.UpdateDrawingTo(nmultiplayerPlayerState.GlobalPosition + Vector2.Right * nmultiplayerPlayerState.Hitbox.Size.X + Vector2.Down * nmultiplayerPlayerState.Hitbox.Size.Y / 2f);
				}
			}
			base.EmitSignal(NTargetManager.SignalName.NodeHovered, new Variant[] { node });
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x001FB4EC File Offset: 0x001F96EC
		[NullableContext(1)]
		public void OnNodeUnhovered(Node node)
		{
			if (!this.IsInSelection)
			{
				return;
			}
			if (!this.AllowedToTargetNode(node))
			{
				return;
			}
			NCreature ncreature = node as NCreature;
			if (ncreature != null)
			{
				this.OnCreatureUnhovered(ncreature);
				return;
			}
			this.HoveredNode = null;
			this._targetingArrow.SetHighlightingOff();
			base.EmitSignal(NTargetManager.SignalName.NodeUnhovered, new Variant[] { node });
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x001FB550 File Offset: 0x001F9750
		[NullableContext(1)]
		private void OnCreatureHovered(NCreature creature)
		{
			AbstractModel abstractModel;
			if (Hook.ShouldAllowTargeting(creature.Entity.CombatState, creature.Entity, out abstractModel))
			{
				this.HoveredNode = creature;
				this._targetingArrow.SetHighlightingOn(creature.Entity.IsEnemy);
				creature.ShowSingleSelectReticle();
				base.EmitSignal(NTargetManager.SignalName.CreatureHovered, new Variant[] { creature });
				if (this._targetMode == TargetMode.Controller)
				{
					this._targetingArrow.UpdateDrawingTo(creature.VfxSpawnPosition);
					return;
				}
			}
			else
			{
				TaskHelper.RunSafely(abstractModel.AfterTargetingBlockedVfx(creature.Entity));
			}
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x001FB5E8 File Offset: 0x001F97E8
		[NullableContext(1)]
		private void OnCreatureUnhovered(NCreature creature)
		{
			base.EmitSignal(NTargetManager.SignalName.CreatureUnhovered, new Variant[] { creature });
			if (this.HoveredNode == creature)
			{
				this.HoveredNode = null;
			}
			this._targetingArrow.SetHighlightingOff();
			if (this._targetMode != TargetMode.None)
			{
				creature.HideSingleSelectReticle();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060041A0 RID: 16800 RVA: 0x001FB640 File Offset: 0x001F9840
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NTargetManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetManager.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetManager.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetManager.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NTargetManager.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.CancelTargeting, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetManager.MethodName.FinishTargeting, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "cancel", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.AllowedToTargetNode, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.OnNodeHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.OnNodeUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.OnCreatureHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTargetManager.MethodName.OnCreatureUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060041A1 RID: 16801 RVA: 0x001FB9F4 File Offset: 0x001F9BF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTargetManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.CancelTargeting && args.Count == 0)
			{
				this.CancelTargeting();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.FinishTargeting && args.Count == 1)
			{
				this.FinishTargeting(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.AllowedToTargetNode && args.Count == 1)
			{
				bool flag = this.AllowedToTargetNode(VariantUtils.ConvertTo<Node>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.OnNodeHovered && args.Count == 1)
			{
				this.OnNodeHovered(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.OnNodeUnhovered && args.Count == 1)
			{
				this.OnNodeUnhovered(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.OnCreatureHovered && args.Count == 1)
			{
				this.OnCreatureHovered(VariantUtils.ConvertTo<NCreature>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetManager.MethodName.OnCreatureUnhovered && args.Count == 1)
			{
				this.OnCreatureUnhovered(VariantUtils.ConvertTo<NCreature>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060041A2 RID: 16802 RVA: 0x001FBC40 File Offset: 0x001F9E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTargetManager.MethodName._Ready || (ref method) == NTargetManager.MethodName._EnterTree || (ref method) == NTargetManager.MethodName._ExitTree || (ref method) == NTargetManager.MethodName._Input || (ref method) == NTargetManager.MethodName._Process || (ref method) == NTargetManager.MethodName.CancelTargeting || (ref method) == NTargetManager.MethodName.FinishTargeting || (ref method) == NTargetManager.MethodName.AllowedToTargetNode || (ref method) == NTargetManager.MethodName.OnNodeHovered || (ref method) == NTargetManager.MethodName.OnNodeUnhovered || (ref method) == NTargetManager.MethodName.OnCreatureHovered || (ref method) == NTargetManager.MethodName.OnCreatureUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060041A3 RID: 16803 RVA: 0x001FBD08 File Offset: 0x001F9F08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTargetManager.PropertyName.HoveredNode)
			{
				this.HoveredNode = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName.LastTargetingFinishedFrame)
			{
				this.LastTargetingFinishedFrame = VariantUtils.ConvertTo<long>(ref value);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._targetingArrow)
			{
				this._targetingArrow = VariantUtils.ConvertTo<NTargetingArrow>(ref value);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._targetMode)
			{
				this._targetMode = VariantUtils.ConvertTo<TargetMode>(ref value);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._validTargetsType)
			{
				this._validTargetsType = VariantUtils.ConvertTo<TargetType>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060041A4 RID: 16804 RVA: 0x001FBDA4 File Offset: 0x001F9FA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTargetManager.PropertyName.IsInSelection)
			{
				bool isInSelection = this.IsInSelection;
				value = VariantUtils.CreateFrom<bool>(ref isInSelection);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName.HoveredNode)
			{
				Node hoveredNode = this.HoveredNode;
				value = VariantUtils.CreateFrom<Node>(ref hoveredNode);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName.LastTargetingFinishedFrame)
			{
				long lastTargetingFinishedFrame = this.LastTargetingFinishedFrame;
				value = VariantUtils.CreateFrom<long>(ref lastTargetingFinishedFrame);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._targetingArrow)
			{
				value = VariantUtils.CreateFrom<NTargetingArrow>(ref this._targetingArrow);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._targetMode)
			{
				value = VariantUtils.CreateFrom<TargetMode>(ref this._targetMode);
				return true;
			}
			if ((ref name) == NTargetManager.PropertyName._validTargetsType)
			{
				value = VariantUtils.CreateFrom<TargetType>(ref this._validTargetsType);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060041A5 RID: 16805 RVA: 0x001FBE84 File Offset: 0x001FA084
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTargetManager.PropertyName._targetingArrow, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTargetManager.PropertyName.IsInSelection, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTargetManager.PropertyName._targetMode, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTargetManager.PropertyName._validTargetsType, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTargetManager.PropertyName.HoveredNode, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTargetManager.PropertyName.LastTargetingFinishedFrame, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060041A6 RID: 16806 RVA: 0x001FBF5C File Offset: 0x001FA15C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hoveredNode = NTargetManager.PropertyName.HoveredNode;
			Node hoveredNode2 = this.HoveredNode;
			info.AddProperty(hoveredNode, Variant.From<Node>(ref hoveredNode2));
			StringName lastTargetingFinishedFrame = NTargetManager.PropertyName.LastTargetingFinishedFrame;
			long lastTargetingFinishedFrame2 = this.LastTargetingFinishedFrame;
			info.AddProperty(lastTargetingFinishedFrame, Variant.From<long>(ref lastTargetingFinishedFrame2));
			info.AddProperty(NTargetManager.PropertyName._targetingArrow, Variant.From<NTargetingArrow>(ref this._targetingArrow));
			info.AddProperty(NTargetManager.PropertyName._targetMode, Variant.From<TargetMode>(ref this._targetMode));
			info.AddProperty(NTargetManager.PropertyName._validTargetsType, Variant.From<TargetType>(ref this._validTargetsType));
			info.AddSignalEventDelegate(NTargetManager.SignalName.CreatureHovered, this.backing_CreatureHovered);
			info.AddSignalEventDelegate(NTargetManager.SignalName.CreatureUnhovered, this.backing_CreatureUnhovered);
			info.AddSignalEventDelegate(NTargetManager.SignalName.NodeHovered, this.backing_NodeHovered);
			info.AddSignalEventDelegate(NTargetManager.SignalName.NodeUnhovered, this.backing_NodeUnhovered);
			info.AddSignalEventDelegate(NTargetManager.SignalName.TargetingBegan, this.backing_TargetingBegan);
			info.AddSignalEventDelegate(NTargetManager.SignalName.TargetingEnded, this.backing_TargetingEnded);
		}

		/// <inheritdoc />
		// Token: 0x060041A7 RID: 16807 RVA: 0x001FC04C File Offset: 0x001FA24C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTargetManager.PropertyName.HoveredNode, ref variant))
			{
				this.HoveredNode = variant.As<Node>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTargetManager.PropertyName.LastTargetingFinishedFrame, ref variant2))
			{
				this.LastTargetingFinishedFrame = variant2.As<long>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTargetManager.PropertyName._targetingArrow, ref variant3))
			{
				this._targetingArrow = variant3.As<NTargetingArrow>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTargetManager.PropertyName._targetMode, ref variant4))
			{
				this._targetMode = variant4.As<TargetMode>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTargetManager.PropertyName._validTargetsType, ref variant5))
			{
				this._validTargetsType = variant5.As<TargetType>();
			}
			NTargetManager.CreatureHoveredEventHandler creatureHoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.CreatureHoveredEventHandler>(NTargetManager.SignalName.CreatureHovered, ref creatureHoveredEventHandler))
			{
				this.backing_CreatureHovered = creatureHoveredEventHandler;
			}
			NTargetManager.CreatureUnhoveredEventHandler creatureUnhoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.CreatureUnhoveredEventHandler>(NTargetManager.SignalName.CreatureUnhovered, ref creatureUnhoveredEventHandler))
			{
				this.backing_CreatureUnhovered = creatureUnhoveredEventHandler;
			}
			NTargetManager.NodeHoveredEventHandler nodeHoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.NodeHoveredEventHandler>(NTargetManager.SignalName.NodeHovered, ref nodeHoveredEventHandler))
			{
				this.backing_NodeHovered = nodeHoveredEventHandler;
			}
			NTargetManager.NodeUnhoveredEventHandler nodeUnhoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.NodeUnhoveredEventHandler>(NTargetManager.SignalName.NodeUnhovered, ref nodeUnhoveredEventHandler))
			{
				this.backing_NodeUnhovered = nodeUnhoveredEventHandler;
			}
			NTargetManager.TargetingBeganEventHandler targetingBeganEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.TargetingBeganEventHandler>(NTargetManager.SignalName.TargetingBegan, ref targetingBeganEventHandler))
			{
				this.backing_TargetingBegan = targetingBeganEventHandler;
			}
			NTargetManager.TargetingEndedEventHandler targetingEndedEventHandler;
			if (info.TryGetSignalEventDelegate<NTargetManager.TargetingEndedEventHandler>(NTargetManager.SignalName.TargetingEnded, ref targetingEndedEventHandler))
			{
				this.backing_TargetingEnded = targetingEndedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060041A8 RID: 16808 RVA: 0x001FC178 File Offset: 0x001FA378
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTargetManager.SignalName.CreatureHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTargetManager.SignalName.CreatureUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTargetManager.SignalName.NodeHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTargetManager.SignalName.NodeUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTargetManager.SignalName.TargetingBegan, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetManager.SignalName.TargetingEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.CreatureHoveredEventHandler" />
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060041A9 RID: 16809 RVA: 0x001FC367 File Offset: 0x001FA567
		// (remove) Token: 0x060041AA RID: 16810 RVA: 0x001FC380 File Offset: 0x001FA580
		public event NTargetManager.CreatureHoveredEventHandler CreatureHovered
		{
			add
			{
				this.backing_CreatureHovered = (NTargetManager.CreatureHoveredEventHandler)Delegate.Combine(this.backing_CreatureHovered, value);
			}
			remove
			{
				this.backing_CreatureHovered = (NTargetManager.CreatureHoveredEventHandler)Delegate.Remove(this.backing_CreatureHovered, value);
			}
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x001FC399 File Offset: 0x001FA599
		protected void EmitSignalCreatureHovered(NCreature creature)
		{
			base.EmitSignal(NTargetManager.SignalName.CreatureHovered, new Variant[] { creature });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.CreatureUnhoveredEventHandler" />
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060041AC RID: 16812 RVA: 0x001FC3BA File Offset: 0x001FA5BA
		// (remove) Token: 0x060041AD RID: 16813 RVA: 0x001FC3D3 File Offset: 0x001FA5D3
		public event NTargetManager.CreatureUnhoveredEventHandler CreatureUnhovered
		{
			add
			{
				this.backing_CreatureUnhovered = (NTargetManager.CreatureUnhoveredEventHandler)Delegate.Combine(this.backing_CreatureUnhovered, value);
			}
			remove
			{
				this.backing_CreatureUnhovered = (NTargetManager.CreatureUnhoveredEventHandler)Delegate.Remove(this.backing_CreatureUnhovered, value);
			}
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x001FC3EC File Offset: 0x001FA5EC
		protected void EmitSignalCreatureUnhovered(NCreature creature)
		{
			base.EmitSignal(NTargetManager.SignalName.CreatureUnhovered, new Variant[] { creature });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.NodeHoveredEventHandler" />
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060041AF RID: 16815 RVA: 0x001FC40D File Offset: 0x001FA60D
		// (remove) Token: 0x060041B0 RID: 16816 RVA: 0x001FC426 File Offset: 0x001FA626
		public event NTargetManager.NodeHoveredEventHandler NodeHovered
		{
			add
			{
				this.backing_NodeHovered = (NTargetManager.NodeHoveredEventHandler)Delegate.Combine(this.backing_NodeHovered, value);
			}
			remove
			{
				this.backing_NodeHovered = (NTargetManager.NodeHoveredEventHandler)Delegate.Remove(this.backing_NodeHovered, value);
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x001FC43F File Offset: 0x001FA63F
		protected void EmitSignalNodeHovered(Node node)
		{
			base.EmitSignal(NTargetManager.SignalName.NodeHovered, new Variant[] { node });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.NodeUnhoveredEventHandler" />
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060041B2 RID: 16818 RVA: 0x001FC460 File Offset: 0x001FA660
		// (remove) Token: 0x060041B3 RID: 16819 RVA: 0x001FC479 File Offset: 0x001FA679
		public event NTargetManager.NodeUnhoveredEventHandler NodeUnhovered
		{
			add
			{
				this.backing_NodeUnhovered = (NTargetManager.NodeUnhoveredEventHandler)Delegate.Combine(this.backing_NodeUnhovered, value);
			}
			remove
			{
				this.backing_NodeUnhovered = (NTargetManager.NodeUnhoveredEventHandler)Delegate.Remove(this.backing_NodeUnhovered, value);
			}
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x001FC492 File Offset: 0x001FA692
		protected void EmitSignalNodeUnhovered(Node node)
		{
			base.EmitSignal(NTargetManager.SignalName.NodeUnhovered, new Variant[] { node });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.TargetingBeganEventHandler" />
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060041B5 RID: 16821 RVA: 0x001FC4B3 File Offset: 0x001FA6B3
		// (remove) Token: 0x060041B6 RID: 16822 RVA: 0x001FC4CC File Offset: 0x001FA6CC
		public event NTargetManager.TargetingBeganEventHandler TargetingBegan
		{
			add
			{
				this.backing_TargetingBegan = (NTargetManager.TargetingBeganEventHandler)Delegate.Combine(this.backing_TargetingBegan, value);
			}
			remove
			{
				this.backing_TargetingBegan = (NTargetManager.TargetingBeganEventHandler)Delegate.Remove(this.backing_TargetingBegan, value);
			}
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x001FC4E5 File Offset: 0x001FA6E5
		protected void EmitSignalTargetingBegan()
		{
			base.EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager.TargetingEndedEventHandler" />
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060041B8 RID: 16824 RVA: 0x001FC4F8 File Offset: 0x001FA6F8
		// (remove) Token: 0x060041B9 RID: 16825 RVA: 0x001FC511 File Offset: 0x001FA711
		public event NTargetManager.TargetingEndedEventHandler TargetingEnded
		{
			add
			{
				this.backing_TargetingEnded = (NTargetManager.TargetingEndedEventHandler)Delegate.Combine(this.backing_TargetingEnded, value);
			}
			remove
			{
				this.backing_TargetingEnded = (NTargetManager.TargetingEndedEventHandler)Delegate.Remove(this.backing_TargetingEnded, value);
			}
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x001FC52A File Offset: 0x001FA72A
		protected void EmitSignalTargetingEnded()
		{
			base.EmitSignal(NTargetManager.SignalName.TargetingEnded, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060041BB RID: 16827 RVA: 0x001FC540 File Offset: 0x001FA740
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NTargetManager.SignalName.CreatureHovered && args.Count == 1)
			{
				NTargetManager.CreatureHoveredEventHandler creatureHoveredEventHandler = this.backing_CreatureHovered;
				if (creatureHoveredEventHandler == null)
				{
					return;
				}
				creatureHoveredEventHandler(VariantUtils.ConvertTo<NCreature>(args[0]));
				return;
			}
			else if ((ref signal) == NTargetManager.SignalName.CreatureUnhovered && args.Count == 1)
			{
				NTargetManager.CreatureUnhoveredEventHandler creatureUnhoveredEventHandler = this.backing_CreatureUnhovered;
				if (creatureUnhoveredEventHandler == null)
				{
					return;
				}
				creatureUnhoveredEventHandler(VariantUtils.ConvertTo<NCreature>(args[0]));
				return;
			}
			else if ((ref signal) == NTargetManager.SignalName.NodeHovered && args.Count == 1)
			{
				NTargetManager.NodeHoveredEventHandler nodeHoveredEventHandler = this.backing_NodeHovered;
				if (nodeHoveredEventHandler == null)
				{
					return;
				}
				nodeHoveredEventHandler(VariantUtils.ConvertTo<Node>(args[0]));
				return;
			}
			else if ((ref signal) == NTargetManager.SignalName.NodeUnhovered && args.Count == 1)
			{
				NTargetManager.NodeUnhoveredEventHandler nodeUnhoveredEventHandler = this.backing_NodeUnhovered;
				if (nodeUnhoveredEventHandler == null)
				{
					return;
				}
				nodeUnhoveredEventHandler(VariantUtils.ConvertTo<Node>(args[0]));
				return;
			}
			else if ((ref signal) == NTargetManager.SignalName.TargetingBegan && args.Count == 0)
			{
				NTargetManager.TargetingBeganEventHandler targetingBeganEventHandler = this.backing_TargetingBegan;
				if (targetingBeganEventHandler == null)
				{
					return;
				}
				targetingBeganEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NTargetManager.SignalName.TargetingEnded) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NTargetManager.TargetingEndedEventHandler targetingEndedEventHandler = this.backing_TargetingEnded;
				if (targetingEndedEventHandler == null)
				{
					return;
				}
				targetingEndedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060041BC RID: 16828 RVA: 0x001FC678 File Offset: 0x001FA878
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NTargetManager.SignalName.CreatureHovered || (ref signal) == NTargetManager.SignalName.CreatureUnhovered || (ref signal) == NTargetManager.SignalName.NodeHovered || (ref signal) == NTargetManager.SignalName.NodeUnhovered || (ref signal) == NTargetManager.SignalName.TargetingBegan || (ref signal) == NTargetManager.SignalName.TargetingEnded || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x0400197C RID: 6524
		[Nullable(1)]
		private NTargetingArrow _targetingArrow;

		// Token: 0x0400197D RID: 6525
		[Nullable(2)]
		private TaskCompletionSource<Node> _completionSource;

		// Token: 0x0400197E RID: 6526
		[Nullable(2)]
		private Func<bool> _exitEarlyCondition;

		// Token: 0x0400197F RID: 6527
		[Nullable(new byte[] { 2, 1 })]
		private Func<Node, bool> _nodeFilter;

		// Token: 0x04001980 RID: 6528
		private TargetMode _targetMode;

		// Token: 0x04001981 RID: 6529
		private TargetType _validTargetsType;

		// Token: 0x04001984 RID: 6532
		private NTargetManager.CreatureHoveredEventHandler backing_CreatureHovered;

		// Token: 0x04001985 RID: 6533
		private NTargetManager.CreatureUnhoveredEventHandler backing_CreatureUnhovered;

		// Token: 0x04001986 RID: 6534
		private NTargetManager.NodeHoveredEventHandler backing_NodeHovered;

		// Token: 0x04001987 RID: 6535
		private NTargetManager.NodeUnhoveredEventHandler backing_NodeUnhovered;

		// Token: 0x04001988 RID: 6536
		private NTargetManager.TargetingBeganEventHandler backing_TargetingBegan;

		// Token: 0x04001989 RID: 6537
		private NTargetManager.TargetingEndedEventHandler backing_TargetingEnded;

		// Token: 0x0200192D RID: 6445
		// (Invoke) Token: 0x0600AA22 RID: 43554
		[Signal]
		public delegate void CreatureHoveredEventHandler(NCreature creature);

		// Token: 0x0200192E RID: 6446
		// (Invoke) Token: 0x0600AA26 RID: 43558
		[Signal]
		public delegate void CreatureUnhoveredEventHandler(NCreature creature);

		// Token: 0x0200192F RID: 6447
		// (Invoke) Token: 0x0600AA2A RID: 43562
		[Signal]
		public delegate void NodeHoveredEventHandler(Node node);

		// Token: 0x02001930 RID: 6448
		// (Invoke) Token: 0x0600AA2E RID: 43566
		[Signal]
		public delegate void NodeUnhoveredEventHandler(Node node);

		// Token: 0x02001931 RID: 6449
		// (Invoke) Token: 0x0600AA32 RID: 43570
		[Signal]
		public delegate void TargetingBeganEventHandler();

		// Token: 0x02001932 RID: 6450
		// (Invoke) Token: 0x0600AA36 RID: 43574
		[Signal]
		public delegate void TargetingEndedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001933 RID: 6451
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006317 RID: 25367
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006318 RID: 25368
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006319 RID: 25369
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x0400631A RID: 25370
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400631B RID: 25371
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CancelTargeting' method.
			/// </summary>
			// Token: 0x0400631C RID: 25372
			public static readonly StringName CancelTargeting = "CancelTargeting";

			/// <summary>
			/// Cached name for the 'FinishTargeting' method.
			/// </summary>
			// Token: 0x0400631D RID: 25373
			public static readonly StringName FinishTargeting = "FinishTargeting";

			/// <summary>
			/// Cached name for the 'AllowedToTargetNode' method.
			/// </summary>
			// Token: 0x0400631E RID: 25374
			public static readonly StringName AllowedToTargetNode = "AllowedToTargetNode";

			/// <summary>
			/// Cached name for the 'OnNodeHovered' method.
			/// </summary>
			// Token: 0x0400631F RID: 25375
			public static readonly StringName OnNodeHovered = "OnNodeHovered";

			/// <summary>
			/// Cached name for the 'OnNodeUnhovered' method.
			/// </summary>
			// Token: 0x04006320 RID: 25376
			public static readonly StringName OnNodeUnhovered = "OnNodeUnhovered";

			/// <summary>
			/// Cached name for the 'OnCreatureHovered' method.
			/// </summary>
			// Token: 0x04006321 RID: 25377
			public static readonly StringName OnCreatureHovered = "OnCreatureHovered";

			/// <summary>
			/// Cached name for the 'OnCreatureUnhovered' method.
			/// </summary>
			// Token: 0x04006322 RID: 25378
			public static readonly StringName OnCreatureUnhovered = "OnCreatureUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001934 RID: 6452
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsInSelection' property.
			/// </summary>
			// Token: 0x04006323 RID: 25379
			public static readonly StringName IsInSelection = "IsInSelection";

			/// <summary>
			/// Cached name for the 'HoveredNode' property.
			/// </summary>
			// Token: 0x04006324 RID: 25380
			public static readonly StringName HoveredNode = "HoveredNode";

			/// <summary>
			/// Cached name for the 'LastTargetingFinishedFrame' property.
			/// </summary>
			// Token: 0x04006325 RID: 25381
			public static readonly StringName LastTargetingFinishedFrame = "LastTargetingFinishedFrame";

			/// <summary>
			/// Cached name for the '_targetingArrow' field.
			/// </summary>
			// Token: 0x04006326 RID: 25382
			public static readonly StringName _targetingArrow = "_targetingArrow";

			/// <summary>
			/// Cached name for the '_targetMode' field.
			/// </summary>
			// Token: 0x04006327 RID: 25383
			public static readonly StringName _targetMode = "_targetMode";

			/// <summary>
			/// Cached name for the '_validTargetsType' field.
			/// </summary>
			// Token: 0x04006328 RID: 25384
			public static readonly StringName _validTargetsType = "_validTargetsType";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001935 RID: 6453
		public class SignalName : Node2D.SignalName
		{
			/// <summary>
			/// Cached name for the 'CreatureHovered' signal.
			/// </summary>
			// Token: 0x04006329 RID: 25385
			public static readonly StringName CreatureHovered = "CreatureHovered";

			/// <summary>
			/// Cached name for the 'CreatureUnhovered' signal.
			/// </summary>
			// Token: 0x0400632A RID: 25386
			public static readonly StringName CreatureUnhovered = "CreatureUnhovered";

			/// <summary>
			/// Cached name for the 'NodeHovered' signal.
			/// </summary>
			// Token: 0x0400632B RID: 25387
			public static readonly StringName NodeHovered = "NodeHovered";

			/// <summary>
			/// Cached name for the 'NodeUnhovered' signal.
			/// </summary>
			// Token: 0x0400632C RID: 25388
			public static readonly StringName NodeUnhovered = "NodeUnhovered";

			/// <summary>
			/// Cached name for the 'TargetingBegan' signal.
			/// </summary>
			// Token: 0x0400632D RID: 25389
			public static readonly StringName TargetingBegan = "TargetingBegan";

			/// <summary>
			/// Cached name for the 'TargetingEnded' signal.
			/// </summary>
			// Token: 0x0400632E RID: 25390
			public static readonly StringName TargetingEnded = "TargetingEnded";
		}
	}
}
