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
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x02000380 RID: 896
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Potions/NPotionHolder.cs")]
	public class NPotionHolder : NClickableControl
	{
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003536 RID: 13622 RVA: 0x0018C7F0 File Offset: 0x0018A9F0
		private static HoverTip EmptyHoverTip
		{
			get
			{
				return new HoverTip(new LocString("static_hover_tips", "POTION_SLOT.title"), new LocString("static_hover_tips", "POTION_SLOT.description"), null);
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003537 RID: 13623 RVA: 0x0018C816 File Offset: 0x0018AA16
		// (set) Token: 0x06003538 RID: 13624 RVA: 0x0018C81E File Offset: 0x0018AA1E
		[Nullable(2)]
		public NPotion Potion
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003539 RID: 13625 RVA: 0x0018C827 File Offset: 0x0018AA27
		public bool HasPotion
		{
			get
			{
				return this.Potion != null;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600353A RID: 13626 RVA: 0x0018C832 File Offset: 0x0018AA32
		public bool IsPotionUsable
		{
			get
			{
				return this._popup.IsUsable;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600353B RID: 13627 RVA: 0x0018C83F File Offset: 0x0018AA3F
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/potions/potion_holder");
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x0018C84B File Offset: 0x0018AA4B
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPotionHolder.ScenePath);
			}
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0018C858 File Offset: 0x0018AA58
		public static NPotionHolder Create(bool isUsable)
		{
			NPotionHolder npotionHolder = PreloadManager.Cache.GetScene(NPotionHolder.ScenePath).Instantiate<NPotionHolder>(0L);
			npotionHolder._isUsable = isUsable;
			return npotionHolder;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0018C884 File Offset: 0x0018AA84
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0018C891 File Offset: 0x0018AA91
		public override void _Ready()
		{
			this._emptyIcon = base.GetNode<TextureRect>("%EmptyIcon");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this.ConnectSignals();
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x0018C8C5 File Offset: 0x0018AAC5
		public override void _ExitTree()
		{
			CancellationTokenSource cancelGrayOutPotionSource = this._cancelGrayOutPotionSource;
			if (cancelGrayOutPotionSource != null)
			{
				cancelGrayOutPotionSource.Cancel();
			}
			this._cts.Cancel();
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0018C8E4 File Offset: 0x0018AAE4
		protected override void OnFocus()
		{
			if (this._isFocused)
			{
				return;
			}
			this._isFocused = true;
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			if (this.Potion != null)
			{
				this.Potion.DoBounce();
				this._hoverTween.TweenProperty(this.Potion, "scale", this._potionScale * 1.15f, 0.05);
				NDebugAudioManager instance = NDebugAudioManager.Instance;
				if (instance != null)
				{
					instance.Play(Rng.Chaotic.NextItem<string>(TmpSfx.PotionSlosh), 0.5f, PitchVariance.Large);
				}
				if (!GodotObject.IsInstanceValid(this._popup) || this._popup.IsMarkedForRemoval)
				{
					NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this.Potion.Model.HoverTips, HoverTipAlignment.Center);
					if (nhoverTipSet != null)
					{
						nhoverTipSet.SetGlobalPosition(base.GlobalPosition + Vector2.Down * base.Size.Y * Mathf.Max(1.5f, base.Scale.Y), false);
					}
				}
			}
			else
			{
				this._hoverTween.TweenProperty(this._emptyIcon, "scale", this._potionScale * 1.15f, 0.05);
				NHoverTipSet nhoverTipSet2 = NHoverTipSet.CreateAndShow(this, NPotionHolder.EmptyHoverTip, HoverTipAlignment.None);
				if (nhoverTipSet2 != null)
				{
					nhoverTipSet2.SetGlobalPosition(base.GlobalPosition + Vector2.Down * base.Size.Y * Mathf.Max(1.5f, base.Scale.Y), false);
				}
				if (nhoverTipSet2 != null)
				{
					nhoverTipSet2.SetAlignment(this, HoverTipAlignment.Center);
				}
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0018CAD0 File Offset: 0x0018ACD0
		protected override void OnUnfocus()
		{
			this._isFocused = false;
			NHoverTipSet.Remove(this);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			if (this.Potion != null)
			{
				if (!this._disabledUntilPotionRemoved)
				{
					this._hoverTween = base.CreateTween().SetParallel(true);
					this._hoverTween.TweenProperty(this.Potion, "scale", this._potionScale, 0.5).SetEase(1L).SetTrans(5L);
				}
			}
			else
			{
				this._hoverTween = base.CreateTween().SetParallel(true);
				this._hoverTween.TweenProperty(this._emptyIcon, "scale", this._potionScale, 0.5).SetEase(1L).SetTrans(5L);
			}
			this._selectionReticle.OnDeselect();
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x0018CBBB File Offset: 0x0018ADBB
		protected override void OnPress()
		{
			if (this.Potion != null && this._isUsable)
			{
				base.GetViewport().SetInputAsHandled();
			}
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x0018CBD8 File Offset: 0x0018ADD8
		protected override void OnRelease()
		{
			if (this._isUsable)
			{
				this.OpenPotionPopup();
			}
			if (this.Potion != null && this._isUsable)
			{
				base.GetViewport().SetInputAsHandled();
			}
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x0018CC04 File Offset: 0x0018AE04
		private void OpenPotionPopup()
		{
			if (this.HasPotion && !this.Potion.Model.Owner.RunState.IsGameOver && !this._disabledUntilPotionRemoved)
			{
				NHoverTipSet.Remove(this);
				this._popup = NPotionPopup.Create(this);
				this.AddChildSafely(this._popup);
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x0018CC5C File Offset: 0x0018AE5C
		public void AddPotion(NPotion potion)
		{
			if (this.Potion != null)
			{
				throw new InvalidOperationException("Slot already contains a potion");
			}
			this.Potion = potion;
			Tween emptyPotionTween = this._emptyPotionTween;
			if (emptyPotionTween != null)
			{
				emptyPotionTween.Kill();
			}
			this._emptyIcon.Modulate = Colors.Transparent;
			this.AddChildSafely(this.Potion);
			this.Potion.Scale = this._potionScale;
			this.Potion.PivotOffset = this.Potion.Size * 0.5f;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x0018CCE1 File Offset: 0x0018AEE1
		public void DisableUntilPotionRemoved()
		{
			if (this._popup != null && GodotObject.IsInstanceValid(this._popup))
			{
				this._popup.Remove();
			}
			this._disabledUntilPotionRemoved = true;
			TaskHelper.RunSafely(this.GrayPotionHolderUntilPlayedAfterDelay());
			this.TryGrabFocus();
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0018CD1C File Offset: 0x0018AF1C
		private async Task GrayPotionHolderUntilPlayedAfterDelay()
		{
			this._cancelGrayOutPotionSource = new CancellationTokenSource();
			await Task.Delay(100, this._cancelGrayOutPotionSource.Token);
			if (!this._cancelGrayOutPotionSource.IsCancellationRequested)
			{
				base.Modulate = StsColors.gray;
			}
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x0018CD5F File Offset: 0x0018AF5F
		public void CancelPotionUseOrDiscard()
		{
			CancellationTokenSource cancelGrayOutPotionSource = this._cancelGrayOutPotionSource;
			if (cancelGrayOutPotionSource != null)
			{
				cancelGrayOutPotionSource.Cancel();
			}
			this._disabledUntilPotionRemoved = false;
			base.Modulate = Colors.White;
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x0018CD84 File Offset: 0x0018AF84
		public void RemoveUsedPotion()
		{
			if (this.Potion == null)
			{
				throw new InvalidOperationException("This slot doesn't contain a potion");
			}
			if (this._popup != null && GodotObject.IsInstanceValid(this._popup))
			{
				this._popup.Remove();
			}
			NHoverTipSet.Remove(this);
			this._disabledUntilPotionRemoved = false;
			CancellationTokenSource cancelGrayOutPotionSource = this._cancelGrayOutPotionSource;
			if (cancelGrayOutPotionSource != null)
			{
				cancelGrayOutPotionSource.Cancel();
			}
			base.Modulate = Colors.White;
			NPotion potionToRemove = this.Potion;
			this.Potion = null;
			Tween tween = base.CreateTween();
			tween.TweenProperty(potionToRemove, "scale", Vector2.Zero, 0.20000000298023224).SetEase(0L).SetTrans(10L)
				.FromCurrent();
			tween.TweenCallback(Callable.From(delegate
			{
				this.RemoveChildSafely(potionToRemove);
				potionToRemove.QueueFreeSafely();
			}));
			if (base.IsFocused)
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, NPotionHolder.EmptyHoverTip, HoverTipAlignment.None);
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetGlobalPosition(base.GlobalPosition + Vector2.Down * base.Size.Y * 1.5f, false);
				}
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetAlignment(this, HoverTipAlignment.Center);
				}
			}
			Tween emptyPotionTween = this._emptyPotionTween;
			if (emptyPotionTween != null)
			{
				emptyPotionTween.Kill();
			}
			this._emptyPotionTween = base.CreateTween();
			this._emptyPotionTween.TweenProperty(this._emptyIcon, "modulate", Colors.White, 0.20000000298023224).SetDelay(0.20000000298023224);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0018CF20 File Offset: 0x0018B120
		public void DiscardPotion()
		{
			if (this.Potion == null)
			{
				throw new InvalidOperationException("This slot doesn't contain a potion");
			}
			if (this._popup != null && GodotObject.IsInstanceValid(this._popup))
			{
				this._popup.Remove();
			}
			this._disabledUntilPotionRemoved = false;
			CancellationTokenSource cancelGrayOutPotionSource = this._cancelGrayOutPotionSource;
			if (cancelGrayOutPotionSource != null)
			{
				cancelGrayOutPotionSource.Cancel();
			}
			base.Modulate = Colors.White;
			NPotion potionToRemove = this.Potion;
			this.Potion = null;
			Tween tween = base.CreateTween();
			tween.TweenProperty(potionToRemove, "position:y", -100f, 0.4000000059604645).SetEase(0L).SetTrans(10L);
			tween.TweenCallback(Callable.From(delegate
			{
				this.RemoveChildSafely(potionToRemove);
				potionToRemove.QueueFreeSafely();
			}));
			Tween emptyPotionTween = this._emptyPotionTween;
			if (emptyPotionTween != null)
			{
				emptyPotionTween.Kill();
			}
			this._emptyPotionTween = base.CreateTween();
			this._emptyPotionTween.TweenProperty(this._emptyIcon, "modulate", Colors.White, 0.20000000298023224).FromCurrent().SetDelay(0.20000000298023224);
		}

		/// <summary>
		/// Uses the potion.
		/// This may initiate targeting for single-targeted potions. If targeting is cancelled, the potion will not be used.
		/// </summary>
		// Token: 0x0600354C RID: 13644 RVA: 0x0018D05C File Offset: 0x0018B25C
		public async Task UsePotion()
		{
			if (this.Potion == null)
			{
				Log.Warn("Tried to use potion in holder, but potion node is null!", 2);
			}
			else
			{
				TargetType targetType = this.Potion.Model.TargetType;
				bool flag = targetType == TargetType.AnyEnemy || targetType == TargetType.TargetedNoCreature;
				if (flag || this.Potion.Model.CanThrowAtAlly())
				{
					RunManager.Instance.HoveredModelTracker.OnLocalPotionSelected(this.Potion.Model);
					await this.TargetNode(this.Potion.Model.TargetType);
					RunManager.Instance.HoveredModelTracker.OnLocalPotionDeselected();
				}
				else
				{
					Creature creature = ((this.Potion.Model.TargetType == TargetType.Self) ? this.Potion.Model.Owner.Creature : null);
					this.Potion.Model.EnqueueManualUse(creature);
					this.TryGrabFocus();
				}
			}
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0018D0A0 File Offset: 0x0018B2A0
		private async Task TargetNode(TargetType targetType)
		{
			Vector2 vector = base.GlobalPosition + Vector2.Right * base.Size.X * 0.5f + Vector2.Down * 50f;
			NTargetManager instance = NTargetManager.Instance;
			bool isUsingController = NControllerManager.Instance.IsUsingController;
			instance.StartTargeting(targetType, vector, isUsingController ? TargetMode.Controller : TargetMode.ClickMouseToTarget, new Func<bool>(this.ShouldCancelTargeting), null);
			Creature creature = this.Potion.Model.Owner.Creature;
			if (isUsingController && CombatManager.Instance.IsInProgress)
			{
				ICombatState combatState = creature.CombatState;
				IReadOnlyList<Creature> readOnlyList;
				if (targetType != TargetType.AnyEnemy)
				{
					if (targetType != TargetType.AnyPlayer)
					{
						throw new ArgumentOutOfRangeException("targetType", targetType, null);
					}
					readOnlyList = combatState.GetTeammatesOf(creature);
				}
				else
				{
					readOnlyList = combatState.GetOpponentsOf(creature);
				}
				IEnumerable<Creature> enumerable = readOnlyList;
				List<Creature> list = enumerable.Where<Creature>((Creature c) => c.IsAlive).ToList<Creature>();
				NCombatRoom.Instance.RestrictControllerNavigation(list.Select<Creature, Control>((Creature c) => NCombatRoom.Instance.GetCreatureNode(c).Hitbox));
				NCombatRoom.Instance.GetCreatureNode(list.First<Creature>()).Hitbox.TryGrabFocus();
			}
			else if (isUsingController && targetType == TargetType.AnyPlayer)
			{
				NMultiplayerPlayerStateContainer multiplayerPlayerContainer = NRun.Instance.GlobalUi.MultiplayerPlayerContainer;
				NMultiplayerPlayerState firstPlayerState = multiplayerPlayerContainer.FirstPlayerState;
				if (firstPlayerState != null)
				{
					firstPlayerState.Hitbox.TryGrabFocus();
				}
				multiplayerPlayerContainer.LockNavigation();
			}
			NMerchantButton merchantButton = null;
			Control.FocusBehaviorRecursiveEnum? savedFocusBehavior = null;
			Control merchantScreenContext = null;
			bool merchantButtonWasDisabled = false;
			if (isUsingController && this.Potion.Model is FoulPotion)
			{
				ValueTuple<NMerchantButton, Control> foulPotionMerchantTarget = FoulPotion.GetFoulPotionMerchantTarget(creature.Player.RunState.CurrentRoom);
				merchantButton = foulPotionMerchantTarget.Item1;
				merchantScreenContext = foulPotionMerchantTarget.Item2;
				if (merchantButton != null)
				{
					if (merchantScreenContext != null)
					{
						savedFocusBehavior = new Control.FocusBehaviorRecursiveEnum?(merchantScreenContext.FocusBehaviorRecursive);
						merchantScreenContext.FocusBehaviorRecursive = 2L;
					}
					if (!merchantButton.IsEnabled)
					{
						merchantButtonWasDisabled = true;
						merchantButton.Enable();
					}
				}
			}
			NMerchantButton nmerchantButton = merchantButton;
			if (nmerchantButton != null)
			{
				nmerchantButton.SetFocusMode(2L);
			}
			NMerchantButton nmerchantButton2 = merchantButton;
			if (nmerchantButton2 != null)
			{
				nmerchantButton2.TryGrabFocus();
			}
			try
			{
				Node node = await instance.SelectionFinished();
				Node node2 = node;
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 != null)
				{
					instance2.EnableControllerNavigation();
				}
				NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnlockNavigation();
				if (node2 != null)
				{
					NCreature ncreature = node2 as NCreature;
					Creature creature2;
					if (ncreature == null)
					{
						NMultiplayerPlayerState nmultiplayerPlayerState = node2 as NMultiplayerPlayerState;
						if (nmultiplayerPlayerState == null)
						{
							if (!(node2 is NMerchantButton))
							{
								throw new ArgumentOutOfRangeException("targetNode", node2, null);
							}
							creature2 = null;
						}
						else
						{
							creature2 = nmultiplayerPlayerState.Player.Creature;
						}
					}
					else
					{
						creature2 = ncreature.Entity;
					}
					Creature creature3 = creature2;
					this.Potion.Model.EnqueueManualUse(creature3);
				}
			}
			finally
			{
				NMerchantButton nmerchantButton3 = merchantButton;
				if (nmerchantButton3 != null)
				{
					nmerchantButton3.SetFocusMode(0L);
				}
				if (merchantButtonWasDisabled)
				{
					merchantButton.Disable();
				}
				if (merchantScreenContext != null && savedFocusBehavior != null)
				{
					merchantScreenContext.FocusBehaviorRecursive = savedFocusBehavior.Value;
				}
			}
			this.TryGrabFocus();
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x0018D0EB File Offset: 0x0018B2EB
		private bool ShouldCancelTargeting()
		{
			return this.Potion == null || (CombatManager.Instance.IsInProgress && (NOverlayStack.Instance.ScreenCount > 0 || NCapstoneContainer.Instance.InUse));
		}

		/// <summary>
		/// Makes all of your potions popup and make a slosh sound to remind players that they have potions.
		/// </summary>
		// Token: 0x0600354F RID: 13647 RVA: 0x0018D120 File Offset: 0x0018B320
		public async Task ShineOnStartOfCombat()
		{
			if (this.HasPotion && this.Potion.IsValid())
			{
				this.Potion.DoBounce();
				await Cmd.Wait(0.25f, this._cts.Token, false);
				NDebugAudioManager instance = NDebugAudioManager.Instance;
				if (instance != null)
				{
					instance.Play(Rng.Chaotic.NextItem<string>(TmpSfx.PotionSlosh), 0.3f, PitchVariance.Large);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003550 RID: 13648 RVA: 0x0018D164 File Offset: 0x0018B364
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NPotionHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isUsable", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPotionHolder.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.OpenPotionPopup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.AddPotion, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "potion", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionHolder.MethodName.DisableUntilPotionRemoved, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.CancelPotionUseOrDiscard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.RemoveUsedPotion, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.DiscardPotion, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionHolder.MethodName.ShouldCancelTargeting, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003551 RID: 13649 RVA: 0x0018D49C File Offset: 0x0018B69C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionHolder.MethodName.Create && args.Count == 1)
			{
				NPotionHolder npotionHolder = NPotionHolder.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionHolder>(ref npotionHolder);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.OpenPotionPopup && args.Count == 0)
			{
				this.OpenPotionPopup();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.AddPotion && args.Count == 1)
			{
				this.AddPotion(VariantUtils.ConvertTo<NPotion>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.DisableUntilPotionRemoved && args.Count == 0)
			{
				this.DisableUntilPotionRemoved();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.CancelPotionUseOrDiscard && args.Count == 0)
			{
				this.CancelPotionUseOrDiscard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.RemoveUsedPotion && args.Count == 0)
			{
				this.RemoveUsedPotion();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.DiscardPotion && args.Count == 0)
			{
				this.DiscardPotion();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionHolder.MethodName.ShouldCancelTargeting && args.Count == 0)
			{
				bool flag = this.ShouldCancelTargeting();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x0018D708 File Offset: 0x0018B908
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionHolder.MethodName.Create && args.Count == 1)
			{
				NPotionHolder npotionHolder = NPotionHolder.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionHolder>(ref npotionHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003553 RID: 13651 RVA: 0x0018D758 File Offset: 0x0018B958
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionHolder.MethodName.Create || (ref method) == NPotionHolder.MethodName._EnterTree || (ref method) == NPotionHolder.MethodName._Ready || (ref method) == NPotionHolder.MethodName._ExitTree || (ref method) == NPotionHolder.MethodName.OnFocus || (ref method) == NPotionHolder.MethodName.OnUnfocus || (ref method) == NPotionHolder.MethodName.OnPress || (ref method) == NPotionHolder.MethodName.OnRelease || (ref method) == NPotionHolder.MethodName.OpenPotionPopup || (ref method) == NPotionHolder.MethodName.AddPotion || (ref method) == NPotionHolder.MethodName.DisableUntilPotionRemoved || (ref method) == NPotionHolder.MethodName.CancelPotionUseOrDiscard || (ref method) == NPotionHolder.MethodName.RemoveUsedPotion || (ref method) == NPotionHolder.MethodName.DiscardPotion || (ref method) == NPotionHolder.MethodName.ShouldCancelTargeting || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003554 RID: 13652 RVA: 0x0018D850 File Offset: 0x0018BA50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionHolder.PropertyName.Potion)
			{
				this.Potion = VariantUtils.ConvertTo<NPotion>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._potionScale)
			{
				this._potionScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._emptyIcon)
			{
				this._emptyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._popup)
			{
				this._popup = VariantUtils.ConvertTo<NPotionPopup>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._potionTargeting)
			{
				this._potionTargeting = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._isUsable)
			{
				this._isUsable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._emptyPotionTween)
			{
				this._emptyPotionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._disabledUntilPotionRemoved)
			{
				this._disabledUntilPotionRemoved = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._isFocused)
			{
				this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003555 RID: 13653 RVA: 0x0018D990 File Offset: 0x0018BB90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionHolder.PropertyName.Potion)
			{
				NPotion potion = this.Potion;
				value = VariantUtils.CreateFrom<NPotion>(ref potion);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName.HasPotion)
			{
				bool flag = this.HasPotion;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName.IsPotionUsable)
			{
				bool flag = this.IsPotionUsable;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._potionScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._potionScale);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._emptyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._emptyIcon);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._popup)
			{
				value = VariantUtils.CreateFrom<NPotionPopup>(ref this._popup);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._potionTargeting)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._potionTargeting);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._isUsable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isUsable);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._emptyPotionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._emptyPotionTween);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._disabledUntilPotionRemoved)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._disabledUntilPotionRemoved);
				return true;
			}
			if ((ref name) == NPotionHolder.PropertyName._isFocused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003556 RID: 13654 RVA: 0x0018DB50 File Offset: 0x0018BD50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NPotionHolder.PropertyName._potionScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName.Potion, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName._emptyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName.HasPotion, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName._popup, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName._potionTargeting, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName._isUsable, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName._emptyPotionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionHolder.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName._disabledUntilPotionRemoved, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName.IsPotionUsable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionHolder.PropertyName._isFocused, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003557 RID: 13655 RVA: 0x0018DD0C File Offset: 0x0018BF0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName potion = NPotionHolder.PropertyName.Potion;
			NPotion potion2 = this.Potion;
			info.AddProperty(potion, Variant.From<NPotion>(ref potion2));
			info.AddProperty(NPotionHolder.PropertyName._potionScale, Variant.From<Vector2>(ref this._potionScale));
			info.AddProperty(NPotionHolder.PropertyName._emptyIcon, Variant.From<TextureRect>(ref this._emptyIcon));
			info.AddProperty(NPotionHolder.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NPotionHolder.PropertyName._popup, Variant.From<NPotionPopup>(ref this._popup));
			info.AddProperty(NPotionHolder.PropertyName._potionTargeting, Variant.From<bool>(ref this._potionTargeting));
			info.AddProperty(NPotionHolder.PropertyName._isUsable, Variant.From<bool>(ref this._isUsable));
			info.AddProperty(NPotionHolder.PropertyName._emptyPotionTween, Variant.From<Tween>(ref this._emptyPotionTween));
			info.AddProperty(NPotionHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NPotionHolder.PropertyName._disabledUntilPotionRemoved, Variant.From<bool>(ref this._disabledUntilPotionRemoved));
			info.AddProperty(NPotionHolder.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
		}

		/// <inheritdoc />
		// Token: 0x06003558 RID: 13656 RVA: 0x0018DE18 File Offset: 0x0018C018
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionHolder.PropertyName.Potion, ref variant))
			{
				this.Potion = variant.As<NPotion>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionHolder.PropertyName._potionScale, ref variant2))
			{
				this._potionScale = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotionHolder.PropertyName._emptyIcon, ref variant3))
			{
				this._emptyIcon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPotionHolder.PropertyName._selectionReticle, ref variant4))
			{
				this._selectionReticle = variant4.As<NSelectionReticle>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPotionHolder.PropertyName._popup, ref variant5))
			{
				this._popup = variant5.As<NPotionPopup>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPotionHolder.PropertyName._potionTargeting, ref variant6))
			{
				this._potionTargeting = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPotionHolder.PropertyName._isUsable, ref variant7))
			{
				this._isUsable = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPotionHolder.PropertyName._emptyPotionTween, ref variant8))
			{
				this._emptyPotionTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NPotionHolder.PropertyName._hoverTween, ref variant9))
			{
				this._hoverTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NPotionHolder.PropertyName._disabledUntilPotionRemoved, ref variant10))
			{
				this._disabledUntilPotionRemoved = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NPotionHolder.PropertyName._isFocused, ref variant11))
			{
				this._isFocused = variant11.As<bool>();
			}
		}

		// Token: 0x040014A5 RID: 5285
		private Vector2 _potionScale = 0.9f * Vector2.One;

		// Token: 0x040014A7 RID: 5287
		private TextureRect _emptyIcon;

		// Token: 0x040014A8 RID: 5288
		private NSelectionReticle _selectionReticle;

		// Token: 0x040014A9 RID: 5289
		[Nullable(2)]
		private NPotionPopup _popup;

		// Token: 0x040014AA RID: 5290
		private bool _potionTargeting;

		// Token: 0x040014AB RID: 5291
		private bool _isUsable;

		// Token: 0x040014AC RID: 5292
		[Nullable(2)]
		private Tween _emptyPotionTween;

		// Token: 0x040014AD RID: 5293
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040014AE RID: 5294
		private bool _disabledUntilPotionRemoved;

		// Token: 0x040014AF RID: 5295
		private bool _isFocused;

		// Token: 0x040014B0 RID: 5296
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040014B1 RID: 5297
		[Nullable(2)]
		private CancellationTokenSource _cancelGrayOutPotionSource;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016F2 RID: 5874
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005964 RID: 22884
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005965 RID: 22885
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005966 RID: 22886
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005967 RID: 22887
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005968 RID: 22888
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005969 RID: 22889
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400596A RID: 22890
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400596B RID: 22891
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OpenPotionPopup' method.
			/// </summary>
			// Token: 0x0400596C RID: 22892
			public static readonly StringName OpenPotionPopup = "OpenPotionPopup";

			/// <summary>
			/// Cached name for the 'AddPotion' method.
			/// </summary>
			// Token: 0x0400596D RID: 22893
			public static readonly StringName AddPotion = "AddPotion";

			/// <summary>
			/// Cached name for the 'DisableUntilPotionRemoved' method.
			/// </summary>
			// Token: 0x0400596E RID: 22894
			public static readonly StringName DisableUntilPotionRemoved = "DisableUntilPotionRemoved";

			/// <summary>
			/// Cached name for the 'CancelPotionUseOrDiscard' method.
			/// </summary>
			// Token: 0x0400596F RID: 22895
			public static readonly StringName CancelPotionUseOrDiscard = "CancelPotionUseOrDiscard";

			/// <summary>
			/// Cached name for the 'RemoveUsedPotion' method.
			/// </summary>
			// Token: 0x04005970 RID: 22896
			public static readonly StringName RemoveUsedPotion = "RemoveUsedPotion";

			/// <summary>
			/// Cached name for the 'DiscardPotion' method.
			/// </summary>
			// Token: 0x04005971 RID: 22897
			public static readonly StringName DiscardPotion = "DiscardPotion";

			/// <summary>
			/// Cached name for the 'ShouldCancelTargeting' method.
			/// </summary>
			// Token: 0x04005972 RID: 22898
			public static readonly StringName ShouldCancelTargeting = "ShouldCancelTargeting";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016F3 RID: 5875
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Potion' property.
			/// </summary>
			// Token: 0x04005973 RID: 22899
			public static readonly StringName Potion = "Potion";

			/// <summary>
			/// Cached name for the 'HasPotion' property.
			/// </summary>
			// Token: 0x04005974 RID: 22900
			public static readonly StringName HasPotion = "HasPotion";

			/// <summary>
			/// Cached name for the 'IsPotionUsable' property.
			/// </summary>
			// Token: 0x04005975 RID: 22901
			public static readonly StringName IsPotionUsable = "IsPotionUsable";

			/// <summary>
			/// Cached name for the '_potionScale' field.
			/// </summary>
			// Token: 0x04005976 RID: 22902
			public static readonly StringName _potionScale = "_potionScale";

			/// <summary>
			/// Cached name for the '_emptyIcon' field.
			/// </summary>
			// Token: 0x04005977 RID: 22903
			public static readonly StringName _emptyIcon = "_emptyIcon";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005978 RID: 22904
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_popup' field.
			/// </summary>
			// Token: 0x04005979 RID: 22905
			public static readonly StringName _popup = "_popup";

			/// <summary>
			/// Cached name for the '_potionTargeting' field.
			/// </summary>
			// Token: 0x0400597A RID: 22906
			public static readonly StringName _potionTargeting = "_potionTargeting";

			/// <summary>
			/// Cached name for the '_isUsable' field.
			/// </summary>
			// Token: 0x0400597B RID: 22907
			public static readonly StringName _isUsable = "_isUsable";

			/// <summary>
			/// Cached name for the '_emptyPotionTween' field.
			/// </summary>
			// Token: 0x0400597C RID: 22908
			public static readonly StringName _emptyPotionTween = "_emptyPotionTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x0400597D RID: 22909
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_disabledUntilPotionRemoved' field.
			/// </summary>
			// Token: 0x0400597E RID: 22910
			public static readonly StringName _disabledUntilPotionRemoved = "_disabledUntilPotionRemoved";

			/// <summary>
			/// Cached name for the '_isFocused' field.
			/// </summary>
			// Token: 0x0400597F RID: 22911
			public static readonly StringName _isFocused = "_isFocused";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016F4 RID: 5876
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
