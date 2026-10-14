using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x0200037F RID: 895
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Potions/NPotionContainer.cs")]
	public class NPotionContainer : Control
	{
		// Token: 0x06003513 RID: 13587 RVA: 0x0018B3B4 File Offset: 0x001895B4
		public override void _Ready()
		{
			Callable.From(new Action(this.UpdateNavigation)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x0018B3E0 File Offset: 0x001895E0
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
			this._potionHolders = base.GetNode<Control>("MarginContainer/PotionHolders");
			this._potionErrorBg = base.GetNode<Control>("PotionErrorBg");
			this._potionShortcutButton = base.GetNode<NButton>("PotionShortcutButton");
			this._potionErrorBg.Modulate = Colors.Transparent;
			this._potionShortcutButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnPotionShortcutPressed)), 0U);
			CombatManager.Instance.CombatSetUp += this.OnCombatSetUp;
			this.ConnectPlayerEvents();
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x0018B48C File Offset: 0x0018968C
		public override void _ExitTree()
		{
			this._cts.Cancel();
			this.DisconnectPlayerEvents();
			this._player = null;
			this._potionShortcutButton.Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnPotionShortcutPressed)));
			CombatManager.Instance.CombatSetUp -= this.OnCombatSetUp;
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x0018B4E8 File Offset: 0x001896E8
		public void Initialize(IRunState runState)
		{
			this.DisconnectPlayerEvents();
			this._player = LocalContext.GetMe(runState);
			this.ConnectPlayerEvents();
			this.GrowPotionHolders(this._player.MaxPotionCount);
			foreach (PotionModel potionModel in this._player.Potions)
			{
				this.Add(potionModel, true);
			}
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x0018B564 File Offset: 0x00189764
		private void ConnectPlayerEvents()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.AddPotionFailed += this.PlayAddFailedAnim;
			this._player.PotionProcured += this.OnPotionProcured;
			this._player.UsedPotionRemoved += this.OnUsedPotionRemoved;
			this._player.PotionDiscarded += this.Discard;
			this._player.MaxPotionCountChanged += this.GrowPotionHolders;
			this._player.RelicObtained += this.OnRelicsUpdated;
			this._player.RelicRemoved += this.OnRelicsUpdated;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x0018B61C File Offset: 0x0018981C
		private void DisconnectPlayerEvents()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.AddPotionFailed -= this.PlayAddFailedAnim;
			this._player.PotionProcured -= this.OnPotionProcured;
			this._player.UsedPotionRemoved -= this.OnUsedPotionRemoved;
			this._player.PotionDiscarded -= this.Discard;
			this._player.MaxPotionCountChanged -= this.GrowPotionHolders;
			this._player.RelicObtained -= this.OnRelicsUpdated;
			this._player.RelicRemoved -= this.OnRelicsUpdated;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0018B6D4 File Offset: 0x001898D4
		private void GrowPotionHolders(int newMaxPotionSlots)
		{
			for (int i = this._holders.Count; i < newMaxPotionSlots; i++)
			{
				NPotionHolder node = NPotionHolder.Create(true);
				this._holders.Add(node);
				this._potionHolders.AddChildSafely(node);
				node.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
				{
					this.OnPotionHolderFocused(node);
				}), 0U);
				node.Connect(Control.SignalName.FocusExited, Callable.From(delegate
				{
					this.OnPotionHolderUnfocused(node);
				}), 0U);
				node.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
				{
					this.OnPotionHolderFocused(node);
				}), 0U);
				node.Connect(Control.SignalName.MouseExited, Callable.From(delegate
				{
					this.OnPotionHolderUnfocused(node);
				}), 0U);
			}
			this.UpdateNavigation();
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0018B7CC File Offset: 0x001899CC
		private void OnRelicsUpdated(RelicModel _)
		{
			Callable.From(new Action(this.UpdateNavigation)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x0018B7F8 File Offset: 0x001899F8
		private void UpdateNavigation()
		{
			Control control = NRun.Instance.GlobalUi.RelicInventory.RelicNodes.FirstOrDefault<NRelicInventoryHolder>();
			if (control == null)
			{
				return;
			}
			for (int i = 0; i < this._holders.Count; i++)
			{
				this._holders[i].FocusNeighborLeft = ((i > 0) ? this._holders[i - 1].GetPath() : NRun.Instance.GlobalUi.TopBar.Gold.GetPath());
				this._holders[i].FocusNeighborRight = ((i < this._holders.Count - 1) ? this._holders[i + 1].GetPath() : NRun.Instance.GlobalUi.TopBar.RoomIcon.GetPath());
				this._holders[i].FocusNeighborBottom = control.GetPath();
				this._holders[i].FocusNeighborTop = this._holders[i].GetPath();
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x0018B90C File Offset: 0x00189B0C
		private void Add(PotionModel potion, bool isInitialization)
		{
			if (this._holders.All<NPotionHolder>((NPotionHolder h) => h.HasPotion))
			{
				return;
			}
			if (!isInitialization)
			{
				this.PotionFtueCheck();
			}
			NPotion npotion = NPotion.Create(potion);
			npotion.Position = new Vector2(-30f, -30f);
			NPotionHolder npotionHolder = this._holders[potion.Owner.PotionSlots.IndexOf(potion)];
			npotionHolder.AddPotion(npotion);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x0018B990 File Offset: 0x00189B90
		public void AnimatePotion(PotionModel potion, Vector2? startPosition = null)
		{
			if (!LocalContext.IsMine(potion))
			{
				return;
			}
			NPotionHolder npotionHolder = this._holders.First<NPotionHolder>((NPotionHolder n) => n.Potion != null && n.Potion.Model == potion);
			TaskHelper.RunSafely(npotionHolder.Potion.PlayNewlyAcquiredAnimation(startPosition));
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x0018B9E4 File Offset: 0x00189BE4
		public void OnPotionUseOrDiscardCanceled(PotionModel potion)
		{
			NPotionHolder npotionHolder = this._holders.FirstOrDefault<NPotionHolder>(delegate(NPotionHolder n)
			{
				NPotion potion2 = n.Potion;
				return ((potion2 != null) ? potion2.Model : null) == potion;
			});
			if (npotionHolder != null)
			{
				npotionHolder.CancelPotionUseOrDiscard();
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(95, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to cancel potion use for potion ");
			defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
			defaultInterpolatedStringHandler.AppendLiteral(" but a holder for it does not exist in the player's belt!");
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x0018BA5C File Offset: 0x00189C5C
		private void PotionFtueCheck()
		{
			if (!SaveManager.Instance.SeenFtue("obtain_potion_ftue"))
			{
				NModalContainer.Instance.Add(NObtainPotionFtue.Create(), true);
				SaveManager.Instance.MarkFtueAsComplete("obtain_potion_ftue");
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x0018BA90 File Offset: 0x00189C90
		private void PlayAddFailedAnim()
		{
			if (this._potionsFullTween != null && this._potionsFullTween.IsRunning())
			{
				Tween potionsFullTween = this._potionsFullTween;
				if (potionsFullTween != null)
				{
					potionsFullTween.Kill();
				}
				this._potionHolders.Position = this._potionHolderInitPos;
			}
			this._potionsFullTween = base.CreateTween().SetParallel(true);
			this._potionHolderInitPos = this._potionHolders.Position;
			this._potionsFullTween.TweenMethod(Callable.From<float>(delegate(float t)
			{
				this._potionHolders.Position = this._potionHolderInitPos + Vector2.Right * 3f * Mathf.Sin(t * 5f) * Mathf.Sin(t * 0.5f);
			}), 0f, 6.2831855f, 0.5);
			this._potionsFullTween.TweenProperty(this._potionErrorBg, "modulate", Colors.White, 0.15);
			this._potionsFullTween.TweenProperty(this._potionErrorBg, "modulate", Colors.Transparent, 0.5).SetDelay(0.35);
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x0018BBA0 File Offset: 0x00189DA0
		private void Discard(PotionModel potion)
		{
			NPotionHolder npotionHolder = this._holders.First<NPotionHolder>((NPotionHolder n) => n.Potion != null && n.Potion.Model == potion);
			this.OnPotionHolderUnfocused(npotionHolder);
			npotionHolder.DiscardPotion();
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x0018BBE0 File Offset: 0x00189DE0
		private void RemoveUsed(PotionModel potion)
		{
			NPotionHolder npotionHolder = this._holders.First<NPotionHolder>((NPotionHolder n) => n.Potion != null && n.Potion.Model == potion);
			this.OnPotionHolderUnfocused(npotionHolder);
			npotionHolder.RemoveUsedPotion();
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x0018BC1F File Offset: 0x00189E1F
		private void OnPotionProcured(PotionModel potion)
		{
			this.Add(potion, false);
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x0018BC29 File Offset: 0x00189E29
		private void OnUsedPotionRemoved(PotionModel potion)
		{
			this.RemoveUsed(potion);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x0018BC32 File Offset: 0x00189E32
		private void OnPotionHolderFocused(NPotionHolder holder)
		{
			if (this._focusedHolder == holder)
			{
				return;
			}
			if (holder.Potion == null)
			{
				return;
			}
			RunManager.Instance.HoveredModelTracker.OnLocalPotionHovered(holder.Potion.Model);
			this._focusedHolder = holder;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0018BC68 File Offset: 0x00189E68
		private void OnPotionHolderUnfocused(NPotionHolder holder)
		{
			if (this._focusedHolder != holder)
			{
				return;
			}
			RunManager.Instance.HoveredModelTracker.OnLocalPotionUnhovered();
			this._focusedHolder = null;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x0018BC8A File Offset: 0x00189E8A
		private void OnCombatSetUp(CombatState _)
		{
			TaskHelper.RunSafely(this.ShinePotions());
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x0018BC98 File Offset: 0x00189E98
		private async Task ShinePotions()
		{
			await Cmd.Wait(1f, this._cts.Token, false);
			foreach (NPotionHolder npotionHolder in this._holders)
			{
				await TaskHelper.RunSafely(npotionHolder.ShineOnStartOfCombat());
			}
			List<NPotionHolder>.Enumerator enumerator = default(List<NPotionHolder>.Enumerator);
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x0018BCDC File Offset: 0x00189EDC
		private void OnPotionShortcutPressed(NButton _)
		{
			Viewport viewport = base.GetViewport();
			if (viewport == null)
			{
				this._potionHolders.GetChild<Control>(0, false).TryGrabFocus();
				return;
			}
			if (viewport.GuiGetFocusOwner() != null && NRun.Instance.GlobalUi.TopBar.IsAncestorOf(viewport.GuiGetFocusOwner()))
			{
				ActiveScreenContext.Instance.FocusOnDefaultControl();
				return;
			}
			this._potionHolders.GetChild<Control>(0, false).TryGrabFocus();
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600352A RID: 13610 RVA: 0x0018BD47 File Offset: 0x00189F47
		[Nullable(2)]
		public Control FirstPotionControl
		{
			[NullableContext(2)]
			get
			{
				return this._holders.FirstOrDefault<NPotionHolder>();
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x0600352B RID: 13611 RVA: 0x0018BD54 File Offset: 0x00189F54
		[Nullable(2)]
		public Control LastPotionControl
		{
			[NullableContext(2)]
			get
			{
				return this._holders.LastOrDefault<NPotionHolder>();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600352C RID: 13612 RVA: 0x0018BD64 File Offset: 0x00189F64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NPotionContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.ConnectPlayerEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.DisconnectPlayerEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.GrowPotionHolders, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "newMaxPotionSlots", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPotionContainer.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.PotionFtueCheck, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.PlayAddFailedAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionContainer.MethodName.OnPotionHolderFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionContainer.MethodName.OnPotionHolderUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionContainer.MethodName.OnPotionShortcutPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600352D RID: 13613 RVA: 0x0018C064 File Offset: 0x0018A264
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.ConnectPlayerEvents && args.Count == 0)
			{
				this.ConnectPlayerEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.DisconnectPlayerEvents && args.Count == 0)
			{
				this.DisconnectPlayerEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.GrowPotionHolders && args.Count == 1)
			{
				this.GrowPotionHolders(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.PotionFtueCheck && args.Count == 0)
			{
				this.PotionFtueCheck();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.PlayAddFailedAnim && args.Count == 0)
			{
				this.PlayAddFailedAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.OnPotionHolderFocused && args.Count == 1)
			{
				this.OnPotionHolderFocused(VariantUtils.ConvertTo<NPotionHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.OnPotionHolderUnfocused && args.Count == 1)
			{
				this.OnPotionHolderUnfocused(VariantUtils.ConvertTo<NPotionHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionContainer.MethodName.OnPotionShortcutPressed && args.Count == 1)
			{
				this.OnPotionShortcutPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600352E RID: 13614 RVA: 0x0018C270 File Offset: 0x0018A470
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionContainer.MethodName._Ready || (ref method) == NPotionContainer.MethodName._EnterTree || (ref method) == NPotionContainer.MethodName._ExitTree || (ref method) == NPotionContainer.MethodName.ConnectPlayerEvents || (ref method) == NPotionContainer.MethodName.DisconnectPlayerEvents || (ref method) == NPotionContainer.MethodName.GrowPotionHolders || (ref method) == NPotionContainer.MethodName.UpdateNavigation || (ref method) == NPotionContainer.MethodName.PotionFtueCheck || (ref method) == NPotionContainer.MethodName.PlayAddFailedAnim || (ref method) == NPotionContainer.MethodName.OnPotionHolderFocused || (ref method) == NPotionContainer.MethodName.OnPotionHolderUnfocused || (ref method) == NPotionContainer.MethodName.OnPotionShortcutPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600352F RID: 13615 RVA: 0x0018C338 File Offset: 0x0018A538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionContainer.PropertyName._potionHolders)
			{
				this._potionHolders = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionErrorBg)
			{
				this._potionErrorBg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionShortcutButton)
			{
				this._potionShortcutButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionsFullTween)
			{
				this._potionsFullTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionHolderInitPos)
			{
				this._potionHolderInitPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._focusedHolder)
			{
				this._focusedHolder = VariantUtils.ConvertTo<NPotionHolder>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003530 RID: 13616 RVA: 0x0018C3F0 File Offset: 0x0018A5F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionContainer.PropertyName.FirstPotionControl)
			{
				Control control = this.FirstPotionControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName.LastPotionControl)
			{
				Control control = this.LastPotionControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionHolders)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionHolders);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionErrorBg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionErrorBg);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionShortcutButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._potionShortcutButton);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionsFullTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._potionsFullTween);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._potionHolderInitPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._potionHolderInitPos);
				return true;
			}
			if ((ref name) == NPotionContainer.PropertyName._focusedHolder)
			{
				value = VariantUtils.CreateFrom<NPotionHolder>(ref this._focusedHolder);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003531 RID: 13617 RVA: 0x0018C50C File Offset: 0x0018A70C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionContainer.PropertyName._potionHolders, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName._potionErrorBg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName._potionShortcutButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName._potionsFullTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPotionContainer.PropertyName._potionHolderInitPos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName._focusedHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName.FirstPotionControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionContainer.PropertyName.LastPotionControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003532 RID: 13618 RVA: 0x0018C628 File Offset: 0x0018A828
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPotionContainer.PropertyName._potionHolders, Variant.From<Control>(ref this._potionHolders));
			info.AddProperty(NPotionContainer.PropertyName._potionErrorBg, Variant.From<Control>(ref this._potionErrorBg));
			info.AddProperty(NPotionContainer.PropertyName._potionShortcutButton, Variant.From<NButton>(ref this._potionShortcutButton));
			info.AddProperty(NPotionContainer.PropertyName._potionsFullTween, Variant.From<Tween>(ref this._potionsFullTween));
			info.AddProperty(NPotionContainer.PropertyName._potionHolderInitPos, Variant.From<Vector2>(ref this._potionHolderInitPos));
			info.AddProperty(NPotionContainer.PropertyName._focusedHolder, Variant.From<NPotionHolder>(ref this._focusedHolder));
		}

		/// <inheritdoc />
		// Token: 0x06003533 RID: 13619 RVA: 0x0018C6C0 File Offset: 0x0018A8C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionContainer.PropertyName._potionHolders, ref variant))
			{
				this._potionHolders = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionContainer.PropertyName._potionErrorBg, ref variant2))
			{
				this._potionErrorBg = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotionContainer.PropertyName._potionShortcutButton, ref variant3))
			{
				this._potionShortcutButton = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPotionContainer.PropertyName._potionsFullTween, ref variant4))
			{
				this._potionsFullTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPotionContainer.PropertyName._potionHolderInitPos, ref variant5))
			{
				this._potionHolderInitPos = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPotionContainer.PropertyName._focusedHolder, ref variant6))
			{
				this._focusedHolder = variant6.As<NPotionHolder>();
			}
		}

		// Token: 0x0400149C RID: 5276
		[Nullable(2)]
		private Player _player;

		// Token: 0x0400149D RID: 5277
		private readonly List<NPotionHolder> _holders = new List<NPotionHolder>();

		// Token: 0x0400149E RID: 5278
		private Control _potionHolders;

		// Token: 0x0400149F RID: 5279
		private Control _potionErrorBg;

		// Token: 0x040014A0 RID: 5280
		private NButton _potionShortcutButton;

		// Token: 0x040014A1 RID: 5281
		[Nullable(2)]
		private Tween _potionsFullTween;

		// Token: 0x040014A2 RID: 5282
		private Vector2 _potionHolderInitPos;

		// Token: 0x040014A3 RID: 5283
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040014A4 RID: 5284
		[Nullable(2)]
		private NPotionHolder _focusedHolder;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E8 RID: 5864
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005943 RID: 22851
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005944 RID: 22852
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005945 RID: 22853
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectPlayerEvents' method.
			/// </summary>
			// Token: 0x04005946 RID: 22854
			public static readonly StringName ConnectPlayerEvents = "ConnectPlayerEvents";

			/// <summary>
			/// Cached name for the 'DisconnectPlayerEvents' method.
			/// </summary>
			// Token: 0x04005947 RID: 22855
			public static readonly StringName DisconnectPlayerEvents = "DisconnectPlayerEvents";

			/// <summary>
			/// Cached name for the 'GrowPotionHolders' method.
			/// </summary>
			// Token: 0x04005948 RID: 22856
			public static readonly StringName GrowPotionHolders = "GrowPotionHolders";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04005949 RID: 22857
			public static readonly StringName UpdateNavigation = "UpdateNavigation";

			/// <summary>
			/// Cached name for the 'PotionFtueCheck' method.
			/// </summary>
			// Token: 0x0400594A RID: 22858
			public static readonly StringName PotionFtueCheck = "PotionFtueCheck";

			/// <summary>
			/// Cached name for the 'PlayAddFailedAnim' method.
			/// </summary>
			// Token: 0x0400594B RID: 22859
			public static readonly StringName PlayAddFailedAnim = "PlayAddFailedAnim";

			/// <summary>
			/// Cached name for the 'OnPotionHolderFocused' method.
			/// </summary>
			// Token: 0x0400594C RID: 22860
			public static readonly StringName OnPotionHolderFocused = "OnPotionHolderFocused";

			/// <summary>
			/// Cached name for the 'OnPotionHolderUnfocused' method.
			/// </summary>
			// Token: 0x0400594D RID: 22861
			public static readonly StringName OnPotionHolderUnfocused = "OnPotionHolderUnfocused";

			/// <summary>
			/// Cached name for the 'OnPotionShortcutPressed' method.
			/// </summary>
			// Token: 0x0400594E RID: 22862
			public static readonly StringName OnPotionShortcutPressed = "OnPotionShortcutPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E9 RID: 5865
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'FirstPotionControl' property.
			/// </summary>
			// Token: 0x0400594F RID: 22863
			public static readonly StringName FirstPotionControl = "FirstPotionControl";

			/// <summary>
			/// Cached name for the 'LastPotionControl' property.
			/// </summary>
			// Token: 0x04005950 RID: 22864
			public static readonly StringName LastPotionControl = "LastPotionControl";

			/// <summary>
			/// Cached name for the '_potionHolders' field.
			/// </summary>
			// Token: 0x04005951 RID: 22865
			public static readonly StringName _potionHolders = "_potionHolders";

			/// <summary>
			/// Cached name for the '_potionErrorBg' field.
			/// </summary>
			// Token: 0x04005952 RID: 22866
			public static readonly StringName _potionErrorBg = "_potionErrorBg";

			/// <summary>
			/// Cached name for the '_potionShortcutButton' field.
			/// </summary>
			// Token: 0x04005953 RID: 22867
			public static readonly StringName _potionShortcutButton = "_potionShortcutButton";

			/// <summary>
			/// Cached name for the '_potionsFullTween' field.
			/// </summary>
			// Token: 0x04005954 RID: 22868
			public static readonly StringName _potionsFullTween = "_potionsFullTween";

			/// <summary>
			/// Cached name for the '_potionHolderInitPos' field.
			/// </summary>
			// Token: 0x04005955 RID: 22869
			public static readonly StringName _potionHolderInitPos = "_potionHolderInitPos";

			/// <summary>
			/// Cached name for the '_focusedHolder' field.
			/// </summary>
			// Token: 0x04005956 RID: 22870
			public static readonly StringName _focusedHolder = "_focusedHolder";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016EA RID: 5866
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
