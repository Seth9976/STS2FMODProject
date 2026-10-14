using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	// Token: 0x0200041E RID: 1054
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NHandCardHolder.cs")]
	public class NHandCardHolder : NCardHolder
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06004326 RID: 17190 RVA: 0x00209A38 File Offset: 0x00207C38
		// (set) Token: 0x06004327 RID: 17191 RVA: 0x00209A40 File Offset: 0x00207C40
		public bool InSelectMode { get; set; }

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06004328 RID: 17192 RVA: 0x00209A49 File Offset: 0x00207C49
		public Vector2 TargetPosition
		{
			get
			{
				return this._targetPosition;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x00209A51 File Offset: 0x00207C51
		public float TargetAngle
		{
			get
			{
				return this._targetAngle;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x00209A59 File Offset: 0x00207C59
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("cards/holders/hand_card_holder");
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600432B RID: 17195 RVA: 0x00209A65 File Offset: 0x00207C65
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NHandCardHolder.ScenePath);
			}
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00209A74 File Offset: 0x00207C74
		[NullableContext(1)]
		public static NHandCardHolder Create(NCard card, NPlayerHand hand)
		{
			NHandCardHolder nhandCardHolder = PreloadManager.Cache.GetScene(NHandCardHolder.ScenePath).Instantiate<NHandCardHolder>(0L);
			Node node = nhandCardHolder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(nhandCardHolder.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Model.Id);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			nhandCardHolder.SetCard(card);
			nhandCardHolder._hand = hand;
			return nhandCardHolder;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00209AF4 File Offset: 0x00207CF4
		public override void _Ready()
		{
			base.ConnectSignals();
			this._flash = base.GetNode<Control>("Flash");
			this._flash.Modulate = new Color(this._flash.Modulate.R, this._flash.Modulate.G, this._flash.Modulate.B, 0f);
			this._handIndexLabel = base.GetNode<MegaLabel>("%HandIndex");
			this.UpdateCard();
			base.Hitbox.SetEnabled(false);
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00209B8A File Offset: 0x00207D8A
		public override void _ExitTree()
		{
			base._ExitTree();
			NCard cardNode = base.CardNode;
			this.UnsubscribeFromEvents((cardNode != null) ? cardNode.Model : null);
			this.StopAnimations();
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00209BB0 File Offset: 0x00207DB0
		public override void Clear()
		{
			NCard cardNode = base.CardNode;
			this.UnsubscribeFromEvents((cardNode != null) ? cardNode.Model : null);
			base.Clear();
			this.StopAnimations();
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x00209BD6 File Offset: 0x00207DD6
		protected override void OnFocus()
		{
			base.EmitSignal(NHandCardHolder.SignalName.HolderFocused, new Variant[] { this });
			base.OnFocus();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00209BFD File Offset: 0x00207DFD
		protected override void OnUnfocus()
		{
			base.EmitSignal(NHandCardHolder.SignalName.HolderUnfocused, new Variant[] { this });
			base.OnUnfocus();
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00209C24 File Offset: 0x00207E24
		[NullableContext(1)]
		protected override void OnMousePressed(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton == null)
			{
				return;
			}
			if (inputEventMouseButton.ButtonIndex == 1L && this._isClickable)
			{
				SfxCmd.Play("event:/sfx/ui/clicks/ui_click", 1f);
				base.EmitSignal(NHandCardHolder.SignalName.HolderMouseClicked, new Variant[] { this });
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00209C7C File Offset: 0x00207E7C
		[NullableContext(1)]
		protected override void OnMouseReleased(InputEvent inputEvent)
		{
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x00209C7E File Offset: 0x00207E7E
		protected override void DoCardHoverEffects(bool isHovered)
		{
			base.ZIndex = ((isHovered > false) ? 1 : 0);
			if (isHovered)
			{
				this.CreateHoverTips();
				return;
			}
			base.ClearHoverTips();
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x00209C9A File Offset: 0x00207E9A
		public void SetIndexLabel(int i)
		{
			this._handIndexLabel.SetTextAutoSize(i.ToString());
			this._handIndexLabel.Visible = i > 0 && SaveManager.Instance.PrefsSave.ShowCardIndices;
		}

		/// <summary>
		/// Sets the angle we want this card to be (in degrees).
		/// Used when fanning out cards in the hand. Lacks features that normal tweens have.
		/// </summary>
		/// <param name="angle"></param>
		// Token: 0x06004336 RID: 17206 RVA: 0x00209CCF File Offset: 0x00207ECF
		public void SetTargetAngle(float angle)
		{
			this._targetAngle = angle;
			CancellationTokenSource angleCancelToken = this._angleCancelToken;
			if (angleCancelToken != null)
			{
				angleCancelToken.Cancel();
			}
			this._angleCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimAngle(this._angleCancelToken));
		}

		/// <summary>
		/// Sets the target global position we want this card to be at.
		/// Used when we want this card to navigate from one position to another without a fixed time frame (non-tween).
		/// </summary>
		// Token: 0x06004337 RID: 17207 RVA: 0x00209D06 File Offset: 0x00207F06
		public void SetTargetPosition(Vector2 position)
		{
			this._targetPosition = position;
			CancellationTokenSource positionCancelToken = this._positionCancelToken;
			if (positionCancelToken != null)
			{
				positionCancelToken.Cancel();
			}
			this._positionCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimPosition(this._positionCancelToken));
		}

		/// <summary>
		/// Sets the target scale we want this card to be at, it will smoothly get there.
		/// Used when we want to lerp this card scale in various screens but lacks features that normal tweens have.
		/// </summary>
		/// <param name="scale"></param>
		// Token: 0x06004338 RID: 17208 RVA: 0x00209D3D File Offset: 0x00207F3D
		public void SetTargetScale(Vector2 scale)
		{
			this._targetScale = scale;
			CancellationTokenSource scaleCancelToken = this._scaleCancelToken;
			if (scaleCancelToken != null)
			{
				scaleCancelToken.Cancel();
			}
			this._scaleCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimScale(this._scaleCancelToken));
		}

		/// <summary>
		/// Cancels tweens and sets the Rotation of this CardHolder
		/// </summary>
		/// <param name="setAngle"></param>
		// Token: 0x06004339 RID: 17209 RVA: 0x00209D74 File Offset: 0x00207F74
		public void SetAngleInstantly(float setAngle)
		{
			CancellationTokenSource angleCancelToken = this._angleCancelToken;
			if (angleCancelToken != null)
			{
				angleCancelToken.Cancel();
			}
			base.RotationDegrees = setAngle;
		}

		/// <summary>
		/// Cancels tweens and sets the Scale of this CardHolder
		/// </summary>
		/// <param name="setScale"></param>
		// Token: 0x0600433A RID: 17210 RVA: 0x00209D8E File Offset: 0x00207F8E
		public void SetScaleInstantly(Vector2 setScale)
		{
			CancellationTokenSource scaleCancelToken = this._scaleCancelToken;
			if (scaleCancelToken != null)
			{
				scaleCancelToken.Cancel();
			}
			base.Scale = setScale;
		}

		/// <summary>
		/// Call if we want to stop animating this CardHolder for any reason (like using separate animators/tweens)
		/// </summary>
		// Token: 0x0600433B RID: 17211 RVA: 0x00209DA8 File Offset: 0x00207FA8
		private void StopAnimations()
		{
			CancellationTokenSource angleCancelToken = this._angleCancelToken;
			if (angleCancelToken != null)
			{
				angleCancelToken.Cancel();
			}
			CancellationTokenSource positionCancelToken = this._positionCancelToken;
			if (positionCancelToken != null)
			{
				positionCancelToken.Cancel();
			}
			CancellationTokenSource scaleCancelToken = this._scaleCancelToken;
			if (scaleCancelToken == null)
			{
				return;
			}
			scaleCancelToken.Cancel();
		}

		/// <summary>
		/// Animates our card to rotate to targetAngle.
		/// NOTE: This is NOT a timer-based tween.
		/// </summary>
		/// <param name="cancelToken"></param>
		// Token: 0x0600433C RID: 17212 RVA: 0x00209DDC File Offset: 0x00207FDC
		[NullableContext(1)]
		private async Task AnimAngle(CancellationTokenSource cancelToken)
		{
			while (!cancelToken.IsCancellationRequested)
			{
				base.RotationDegrees = Mathf.Lerp(base.RotationDegrees, this._targetAngle, (float)base.GetProcessDeltaTime() * 10f);
				if (Mathf.Abs(base.RotationDegrees - this._targetAngle) < 0.1f)
				{
					base.RotationDegrees = this._targetAngle;
					return;
				}
				await this.AwaitProcessFrameNonThrowing(cancelToken);
			}
		}

		/// <summary>
		/// Animates our card to scale to targetSize.
		/// NOTE: This is NOT a timer-based tween.
		/// </summary>
		/// <param name="cancelToken"></param>
		// Token: 0x0600433D RID: 17213 RVA: 0x00209E28 File Offset: 0x00208028
		[NullableContext(1)]
		private async Task AnimScale(CancellationTokenSource cancelToken)
		{
			while (!cancelToken.IsCancellationRequested)
			{
				base.Scale = base.Scale.Lerp(this._targetScale, (float)base.GetProcessDeltaTime() * 8f);
				if (Mathf.Abs(this._targetScale.X - base.Scale.X) < 0.002f)
				{
					base.Scale = this._targetScale;
					return;
				}
				await this.AwaitProcessFrameNonThrowing(cancelToken);
			}
		}

		/// <summary>
		/// Animates our card to move to a specified position.
		/// NOTE: This is NOT a timer-based tween.
		/// </summary>
		/// <param name="cancelToken"></param>
		// Token: 0x0600433E RID: 17214 RVA: 0x00209E74 File Offset: 0x00208074
		[NullableContext(1)]
		private async Task AnimPosition(CancellationTokenSource cancelToken)
		{
			while (!cancelToken.IsCancellationRequested)
			{
				base.Position = base.Position.Lerp(this._targetPosition, (float)base.GetProcessDeltaTime() * 7f);
				float num = Mathf.Abs(base.Position.X - this._targetPosition.X);
				if (!base.Hitbox.IsEnabled && num < 200f)
				{
					base.Hitbox.SetEnabled(true);
				}
				if (base.Position.DistanceSquaredTo(this._targetPosition) < 1f)
				{
					base.Position = this._targetPosition;
					return;
				}
				await this.AwaitProcessFrameNonThrowing(cancelToken);
			}
			if (this.IsValid() && !base.Hitbox.IsEnabled && base.Position.DistanceSquaredTo(this._targetPosition) < 200f)
			{
				base.Hitbox.SetEnabled(true);
			}
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00209EC0 File Offset: 0x002080C0
		[NullableContext(1)]
		protected override void SetCard(NCard node)
		{
			if (base.CardNode != null)
			{
				base.CardNode.ModelChanged -= this.OnModelChanged;
			}
			NCard cardNode = base.CardNode;
			this.UnsubscribeFromEvents((cardNode != null) ? cardNode.Model : null);
			base.SetCard(node);
			this.UpdateCard();
			NCard cardNode2 = base.CardNode;
			this.SubscribeToEvents((cardNode2 != null) ? cardNode2.Model : null);
			if (base.CardNode != null)
			{
				base.CardNode.ModelChanged += this.OnModelChanged;
			}
			if (node.Scale != Vector2.One)
			{
				node.CreateTween().TweenProperty(node, "scale", Vector2.One, 0.25);
			}
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00209F84 File Offset: 0x00208184
		public void UpdateCard()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (base.CardNode == null)
			{
				return;
			}
			base.CardNode.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
			if (CombatManager.Instance.IsInProgress)
			{
				if (base.CardNode.Model.CanPlay() || this.ShouldGlowRed || this.ShouldGlowGold)
				{
					base.CardNode.CardHighlight.AnimShow();
					base.CardNode.CardHighlight.Modulate = NCardHighlight.playableColor;
					if (this.ShouldGlowRed)
					{
						base.CardNode.CardHighlight.Modulate = NCardHighlight.red;
						return;
					}
					if (this.ShouldGlowGold)
					{
						base.CardNode.CardHighlight.Modulate = NCardHighlight.gold;
						return;
					}
				}
				else
				{
					base.CardNode.CardHighlight.AnimHide();
				}
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0020A052 File Offset: 0x00208252
		public void BeginDrag()
		{
			this.SetAngleInstantly(0f);
			this.SetScaleInstantly(this.HoverScale);
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x0020A06B File Offset: 0x0020826B
		public void CancelDrag()
		{
			base.ZIndex = 0;
			this.SetAngleInstantly(0f);
			this.SetScaleInstantly(Vector2.One);
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x0020A08C File Offset: 0x0020828C
		public void SetDefaultTargets()
		{
			base.ZIndex = 0;
			IReadOnlyList<NHandCardHolder> activeHolders = this._hand.ActiveHolders;
			int num = activeHolders.IndexOf(this);
			int count = activeHolders.Count;
			if (num < 0)
			{
				return;
			}
			this.SetTargetPosition(HandPosHelper.GetPosition(count, num));
			this.SetTargetAngle(HandPosHelper.GetAngle(count, num));
			this.SetTargetScale(HandPosHelper.GetScale(count));
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x0020A0E8 File Offset: 0x002082E8
		public void Flash()
		{
			if (!GodotObject.IsInstanceValid(this._flash))
			{
				return;
			}
			this._flash.Scale = Vector2.One;
			this._flash.Modulate = NCardHighlight.playableColor;
			if (this.ShouldGlowGold)
			{
				this._flash.Modulate = NCardHighlight.gold;
			}
			else if (this.ShouldGlowRed)
			{
				this._flash.Modulate = NCardHighlight.red;
			}
			Tween flashTween = this._flashTween;
			if (flashTween != null)
			{
				flashTween.Kill();
			}
			this._flashTween = base.CreateTween();
			this._flashTween.TweenProperty(this._flash, "modulate:a", 0.6, 0.15);
			this._flashTween.TweenProperty(this._flash, "modulate:a", 0, 0.3);
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06004345 RID: 17221 RVA: 0x0020A1D0 File Offset: 0x002083D0
		private bool ShouldGlowGold
		{
			get
			{
				NCard cardNode = base.CardNode;
				CardModel cardModel = ((cardNode != null) ? cardNode.Model : null);
				if (cardModel == null)
				{
					return false;
				}
				if (this._hand.SelectModeGoldGlowOverride != null)
				{
					return this._hand.SelectModeGoldGlowOverride(cardModel);
				}
				if (cardModel.CanPlay())
				{
					if (cardModel != null && cardModel.ShouldGlowGold)
					{
						Player owner = cardModel.Owner;
						if (owner != null)
						{
							PlayerCombatState playerCombatState = owner.PlayerCombatState;
							if (playerCombatState != null)
							{
								return playerCombatState.Phase == PlayerTurnPhase.Play;
							}
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06004346 RID: 17222 RVA: 0x0020A248 File Offset: 0x00208448
		private bool ShouldGlowRed
		{
			get
			{
				NCard cardNode = base.CardNode;
				CardModel cardModel = ((cardNode != null) ? cardNode.Model : null);
				if (cardModel != null && cardModel.ShouldGlowRed)
				{
					Player owner = cardModel.Owner;
					if (owner != null)
					{
						PlayerCombatState playerCombatState = owner.PlayerCombatState;
						if (playerCombatState != null)
						{
							return playerCombatState.Phase == PlayerTurnPhase.Play;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x0020A294 File Offset: 0x00208494
		[NullableContext(2)]
		private void SubscribeToEvents(CardModel card)
		{
			if (card == null || !base.IsInsideTree())
			{
				return;
			}
			card.Upgraded += this.Flash;
			card.KeywordsChanged += this.Flash;
			card.ReplayCountChanged += this.Flash;
			card.AfflictionChanged += this.Flash;
			card.EnergyCostChanged += this.Flash;
			card.StarCostChanged += this.Flash;
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x0020A31C File Offset: 0x0020851C
		[NullableContext(2)]
		private void UnsubscribeFromEvents(CardModel card)
		{
			if (card == null)
			{
				return;
			}
			card.Upgraded -= this.Flash;
			card.KeywordsChanged -= this.Flash;
			card.ReplayCountChanged -= this.Flash;
			card.AfflictionChanged -= this.Flash;
			card.EnergyCostChanged -= this.Flash;
			card.StarCostChanged -= this.Flash;
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x0020A399 File Offset: 0x00208599
		[NullableContext(2)]
		private void OnModelChanged(CardModel oldModel)
		{
			this.UnsubscribeFromEvents(oldModel);
			NCard cardNode = base.CardNode;
			this.SubscribeToEvents((cardNode != null) ? cardNode.Model : null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600434A RID: 17226 RVA: 0x0020A3BC File Offset: 0x002085BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(22)
			{
				new MethodInfo(NHandCardHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(24L, "hand", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.OnMousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.OnMouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.DoCardHoverEffects, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isHovered", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetIndexLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "i", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetTargetAngle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "angle", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetTargetPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetTargetScale, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "scale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetAngleInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "setAngle", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.SetScaleInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "setScale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.StopAnimations, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.SetCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHandCardHolder.MethodName.UpdateCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.BeginDrag, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.CancelDrag, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.SetDefaultTargets, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardHolder.MethodName.Flash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600434B RID: 17227 RVA: 0x0020A9D8 File Offset: 0x00208BD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandCardHolder.MethodName.Create && args.Count == 2)
			{
				NHandCardHolder nhandCardHolder = NHandCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<NPlayerHand>(args[1]));
				ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.Clear && args.Count == 0)
			{
				this.Clear();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.OnMousePressed && args.Count == 1)
			{
				this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.OnMouseReleased && args.Count == 1)
			{
				this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.DoCardHoverEffects && args.Count == 1)
			{
				this.DoCardHoverEffects(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetIndexLabel && args.Count == 1)
			{
				this.SetIndexLabel(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetTargetAngle && args.Count == 1)
			{
				this.SetTargetAngle(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetTargetPosition && args.Count == 1)
			{
				this.SetTargetPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetTargetScale && args.Count == 1)
			{
				this.SetTargetScale(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetAngleInstantly && args.Count == 1)
			{
				this.SetAngleInstantly(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetScaleInstantly && args.Count == 1)
			{
				this.SetScaleInstantly(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.StopAnimations && args.Count == 0)
			{
				this.StopAnimations();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetCard && args.Count == 1)
			{
				this.SetCard(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.UpdateCard && args.Count == 0)
			{
				this.UpdateCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.BeginDrag && args.Count == 0)
			{
				this.BeginDrag();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.CancelDrag && args.Count == 0)
			{
				this.CancelDrag();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.SetDefaultTargets && args.Count == 0)
			{
				this.SetDefaultTargets();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardHolder.MethodName.Flash && args.Count == 0)
			{
				this.Flash();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x0020ADCC File Offset: 0x00208FCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandCardHolder.MethodName.Create && args.Count == 2)
			{
				NHandCardHolder nhandCardHolder = NHandCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<NPlayerHand>(args[1]));
				ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600434D RID: 17229 RVA: 0x0020AE28 File Offset: 0x00209028
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHandCardHolder.MethodName.Create || (ref method) == NHandCardHolder.MethodName._Ready || (ref method) == NHandCardHolder.MethodName._ExitTree || (ref method) == NHandCardHolder.MethodName.Clear || (ref method) == NHandCardHolder.MethodName.OnFocus || (ref method) == NHandCardHolder.MethodName.OnUnfocus || (ref method) == NHandCardHolder.MethodName.OnMousePressed || (ref method) == NHandCardHolder.MethodName.OnMouseReleased || (ref method) == NHandCardHolder.MethodName.DoCardHoverEffects || (ref method) == NHandCardHolder.MethodName.SetIndexLabel || (ref method) == NHandCardHolder.MethodName.SetTargetAngle || (ref method) == NHandCardHolder.MethodName.SetTargetPosition || (ref method) == NHandCardHolder.MethodName.SetTargetScale || (ref method) == NHandCardHolder.MethodName.SetAngleInstantly || (ref method) == NHandCardHolder.MethodName.SetScaleInstantly || (ref method) == NHandCardHolder.MethodName.StopAnimations || (ref method) == NHandCardHolder.MethodName.SetCard || (ref method) == NHandCardHolder.MethodName.UpdateCard || (ref method) == NHandCardHolder.MethodName.BeginDrag || (ref method) == NHandCardHolder.MethodName.CancelDrag || (ref method) == NHandCardHolder.MethodName.SetDefaultTargets || (ref method) == NHandCardHolder.MethodName.Flash || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600434E RID: 17230 RVA: 0x0020AF88 File Offset: 0x00209188
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHandCardHolder.PropertyName.InSelectMode)
			{
				this.InSelectMode = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._flash)
			{
				this._flash = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._flashTween)
			{
				this._flashTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._handIndexLabel)
			{
				this._handIndexLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetPosition)
			{
				this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetAngle)
			{
				this._targetAngle = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetScale)
			{
				this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._hand)
			{
				this._hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600434F RID: 17231 RVA: 0x0020B078 File Offset: 0x00209278
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHandCardHolder.PropertyName.InSelectMode)
			{
				bool flag = this.InSelectMode;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName.TargetPosition)
			{
				Vector2 targetPosition = this.TargetPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref targetPosition);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName.TargetAngle)
			{
				float targetAngle = this.TargetAngle;
				value = VariantUtils.CreateFrom<float>(ref targetAngle);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName.ShouldGlowGold)
			{
				bool flag = this.ShouldGlowGold;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName.ShouldGlowRed)
			{
				bool flag = this.ShouldGlowRed;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._flash)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._flash);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._flashTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._flashTween);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._handIndexLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._handIndexLabel);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetAngle)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetAngle);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._targetScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
				return true;
			}
			if ((ref name) == NHandCardHolder.PropertyName._hand)
			{
				value = VariantUtils.CreateFrom<NPlayerHand>(ref this._hand);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004350 RID: 17232 RVA: 0x0020B21C File Offset: 0x0020941C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHandCardHolder.PropertyName._flash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHandCardHolder.PropertyName._flashTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHandCardHolder.PropertyName._handIndexLabel, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandCardHolder.PropertyName._targetPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHandCardHolder.PropertyName._targetAngle, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandCardHolder.PropertyName._targetScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHandCardHolder.PropertyName._hand, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandCardHolder.PropertyName.InSelectMode, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandCardHolder.PropertyName.TargetPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHandCardHolder.PropertyName.TargetAngle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandCardHolder.PropertyName.ShouldGlowGold, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandCardHolder.PropertyName.ShouldGlowRed, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004351 RID: 17233 RVA: 0x0020B3B4 File Offset: 0x002095B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inSelectMode = NHandCardHolder.PropertyName.InSelectMode;
			bool inSelectMode2 = this.InSelectMode;
			info.AddProperty(inSelectMode, Variant.From<bool>(ref inSelectMode2));
			info.AddProperty(NHandCardHolder.PropertyName._flash, Variant.From<Control>(ref this._flash));
			info.AddProperty(NHandCardHolder.PropertyName._flashTween, Variant.From<Tween>(ref this._flashTween));
			info.AddProperty(NHandCardHolder.PropertyName._handIndexLabel, Variant.From<MegaLabel>(ref this._handIndexLabel));
			info.AddProperty(NHandCardHolder.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
			info.AddProperty(NHandCardHolder.PropertyName._targetAngle, Variant.From<float>(ref this._targetAngle));
			info.AddProperty(NHandCardHolder.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
			info.AddProperty(NHandCardHolder.PropertyName._hand, Variant.From<NPlayerHand>(ref this._hand));
			info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderFocused, this.backing_HolderFocused);
			info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderUnfocused, this.backing_HolderUnfocused);
			info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderMouseClicked, this.backing_HolderMouseClicked);
		}

		/// <inheritdoc />
		// Token: 0x06004352 RID: 17234 RVA: 0x0020B4B0 File Offset: 0x002096B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHandCardHolder.PropertyName.InSelectMode, ref variant))
			{
				this.InSelectMode = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._flash, ref variant2))
			{
				this._flash = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._flashTween, ref variant3))
			{
				this._flashTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._handIndexLabel, ref variant4))
			{
				this._handIndexLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._targetPosition, ref variant5))
			{
				this._targetPosition = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._targetAngle, ref variant6))
			{
				this._targetAngle = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._targetScale, ref variant7))
			{
				this._targetScale = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NHandCardHolder.PropertyName._hand, ref variant8))
			{
				this._hand = variant8.As<NPlayerHand>();
			}
			NHandCardHolder.HolderFocusedEventHandler holderFocusedEventHandler;
			if (info.TryGetSignalEventDelegate<NHandCardHolder.HolderFocusedEventHandler>(NHandCardHolder.SignalName.HolderFocused, ref holderFocusedEventHandler))
			{
				this.backing_HolderFocused = holderFocusedEventHandler;
			}
			NHandCardHolder.HolderUnfocusedEventHandler holderUnfocusedEventHandler;
			if (info.TryGetSignalEventDelegate<NHandCardHolder.HolderUnfocusedEventHandler>(NHandCardHolder.SignalName.HolderUnfocused, ref holderUnfocusedEventHandler))
			{
				this.backing_HolderUnfocused = holderUnfocusedEventHandler;
			}
			NHandCardHolder.HolderMouseClickedEventHandler holderMouseClickedEventHandler;
			if (info.TryGetSignalEventDelegate<NHandCardHolder.HolderMouseClickedEventHandler>(NHandCardHolder.SignalName.HolderMouseClicked, ref holderMouseClickedEventHandler))
			{
				this.backing_HolderMouseClicked = holderMouseClickedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004353 RID: 17235 RVA: 0x0020B5EC File Offset: 0x002097EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NHandCardHolder.SignalName.HolderFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHandCardHolder.SignalName.HolderUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHandCardHolder.SignalName.HolderMouseClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NHandCardHolder.HolderFocusedEventHandler" />
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06004354 RID: 17236 RVA: 0x0020B71E File Offset: 0x0020991E
		// (remove) Token: 0x06004355 RID: 17237 RVA: 0x0020B737 File Offset: 0x00209937
		public event NHandCardHolder.HolderFocusedEventHandler HolderFocused
		{
			add
			{
				this.backing_HolderFocused = (NHandCardHolder.HolderFocusedEventHandler)Delegate.Combine(this.backing_HolderFocused, value);
			}
			remove
			{
				this.backing_HolderFocused = (NHandCardHolder.HolderFocusedEventHandler)Delegate.Remove(this.backing_HolderFocused, value);
			}
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0020B750 File Offset: 0x00209950
		protected void EmitSignalHolderFocused(NHandCardHolder cardHolder)
		{
			base.EmitSignal(NHandCardHolder.SignalName.HolderFocused, new Variant[] { cardHolder });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NHandCardHolder.HolderUnfocusedEventHandler" />
		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06004357 RID: 17239 RVA: 0x0020B771 File Offset: 0x00209971
		// (remove) Token: 0x06004358 RID: 17240 RVA: 0x0020B78A File Offset: 0x0020998A
		public event NHandCardHolder.HolderUnfocusedEventHandler HolderUnfocused
		{
			add
			{
				this.backing_HolderUnfocused = (NHandCardHolder.HolderUnfocusedEventHandler)Delegate.Combine(this.backing_HolderUnfocused, value);
			}
			remove
			{
				this.backing_HolderUnfocused = (NHandCardHolder.HolderUnfocusedEventHandler)Delegate.Remove(this.backing_HolderUnfocused, value);
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0020B7A3 File Offset: 0x002099A3
		protected void EmitSignalHolderUnfocused(NHandCardHolder cardHolder)
		{
			base.EmitSignal(NHandCardHolder.SignalName.HolderUnfocused, new Variant[] { cardHolder });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NHandCardHolder.HolderMouseClickedEventHandler" />
		// Token: 0x1400005C RID: 92
		// (add) Token: 0x0600435A RID: 17242 RVA: 0x0020B7C4 File Offset: 0x002099C4
		// (remove) Token: 0x0600435B RID: 17243 RVA: 0x0020B7DD File Offset: 0x002099DD
		public event NHandCardHolder.HolderMouseClickedEventHandler HolderMouseClicked
		{
			add
			{
				this.backing_HolderMouseClicked = (NHandCardHolder.HolderMouseClickedEventHandler)Delegate.Combine(this.backing_HolderMouseClicked, value);
			}
			remove
			{
				this.backing_HolderMouseClicked = (NHandCardHolder.HolderMouseClickedEventHandler)Delegate.Remove(this.backing_HolderMouseClicked, value);
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0020B7F6 File Offset: 0x002099F6
		protected void EmitSignalHolderMouseClicked(NCardHolder cardHolder)
		{
			base.EmitSignal(NHandCardHolder.SignalName.HolderMouseClicked, new Variant[] { cardHolder });
		}

		/// <inheritdoc />
		// Token: 0x0600435D RID: 17245 RVA: 0x0020B818 File Offset: 0x00209A18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NHandCardHolder.SignalName.HolderFocused && args.Count == 1)
			{
				NHandCardHolder.HolderFocusedEventHandler holderFocusedEventHandler = this.backing_HolderFocused;
				if (holderFocusedEventHandler == null)
				{
					return;
				}
				holderFocusedEventHandler(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				return;
			}
			else if ((ref signal) == NHandCardHolder.SignalName.HolderUnfocused && args.Count == 1)
			{
				NHandCardHolder.HolderUnfocusedEventHandler holderUnfocusedEventHandler = this.backing_HolderUnfocused;
				if (holderUnfocusedEventHandler == null)
				{
					return;
				}
				holderUnfocusedEventHandler(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NHandCardHolder.SignalName.HolderMouseClicked) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(in signal, args);
					return;
				}
				NHandCardHolder.HolderMouseClickedEventHandler holderMouseClickedEventHandler = this.backing_HolderMouseClicked;
				if (holderMouseClickedEventHandler == null)
				{
					return;
				}
				holderMouseClickedEventHandler(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x0600435E RID: 17246 RVA: 0x0020B8CC File Offset: 0x00209ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NHandCardHolder.SignalName.HolderFocused || (ref signal) == NHandCardHolder.SignalName.HolderUnfocused || (ref signal) == NHandCardHolder.SignalName.HolderMouseClicked || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x04001A1B RID: 6683
		[Nullable(1)]
		private Control _flash;

		// Token: 0x04001A1C RID: 6684
		[Nullable(2)]
		private Tween _flashTween;

		// Token: 0x04001A1D RID: 6685
		[Nullable(1)]
		private MegaLabel _handIndexLabel;

		// Token: 0x04001A1E RID: 6686
		private const float _rotateSpeed = 10f;

		// Token: 0x04001A1F RID: 6687
		private const float _angleSnapThreshold = 0.1f;

		// Token: 0x04001A20 RID: 6688
		private const float _scaleSpeed = 8f;

		// Token: 0x04001A21 RID: 6689
		private const float _scaleSnapThreshold = 0.002f;

		// Token: 0x04001A22 RID: 6690
		private const float _moveSpeed = 7f;

		// Token: 0x04001A23 RID: 6691
		private const float _positionSnapThreshold = 1f;

		// Token: 0x04001A24 RID: 6692
		private const float _reenableHitboxThreshold = 200f;

		// Token: 0x04001A25 RID: 6693
		private Vector2 _targetPosition;

		// Token: 0x04001A26 RID: 6694
		private float _targetAngle;

		// Token: 0x04001A27 RID: 6695
		private Vector2 _targetScale;

		// Token: 0x04001A28 RID: 6696
		[Nullable(2)]
		private CancellationTokenSource _angleCancelToken;

		// Token: 0x04001A29 RID: 6697
		[Nullable(2)]
		private CancellationTokenSource _positionCancelToken;

		// Token: 0x04001A2A RID: 6698
		[Nullable(2)]
		private CancellationTokenSource _scaleCancelToken;

		// Token: 0x04001A2B RID: 6699
		[Nullable(1)]
		private NPlayerHand _hand;

		// Token: 0x04001A2D RID: 6701
		private NHandCardHolder.HolderFocusedEventHandler backing_HolderFocused;

		// Token: 0x04001A2E RID: 6702
		private NHandCardHolder.HolderUnfocusedEventHandler backing_HolderUnfocused;

		// Token: 0x04001A2F RID: 6703
		private NHandCardHolder.HolderMouseClickedEventHandler backing_HolderMouseClicked;

		// Token: 0x02001966 RID: 6502
		// (Invoke) Token: 0x0600AAAF RID: 43695
		[Signal]
		public delegate void HolderFocusedEventHandler(NHandCardHolder cardHolder);

		// Token: 0x02001967 RID: 6503
		// (Invoke) Token: 0x0600AAB3 RID: 43699
		[Signal]
		public delegate void HolderUnfocusedEventHandler(NHandCardHolder cardHolder);

		// Token: 0x02001968 RID: 6504
		// (Invoke) Token: 0x0600AAB7 RID: 43703
		[Signal]
		public delegate void HolderMouseClickedEventHandler(NCardHolder cardHolder);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001969 RID: 6505
		public new class MethodName : NCardHolder.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04006454 RID: 25684
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006455 RID: 25685
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006456 RID: 25686
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04006457 RID: 25687
			public new static readonly StringName Clear = "Clear";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04006458 RID: 25688
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04006459 RID: 25689
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnMousePressed' method.
			/// </summary>
			// Token: 0x0400645A RID: 25690
			public new static readonly StringName OnMousePressed = "OnMousePressed";

			/// <summary>
			/// Cached name for the 'OnMouseReleased' method.
			/// </summary>
			// Token: 0x0400645B RID: 25691
			public new static readonly StringName OnMouseReleased = "OnMouseReleased";

			/// <summary>
			/// Cached name for the 'DoCardHoverEffects' method.
			/// </summary>
			// Token: 0x0400645C RID: 25692
			public new static readonly StringName DoCardHoverEffects = "DoCardHoverEffects";

			/// <summary>
			/// Cached name for the 'SetIndexLabel' method.
			/// </summary>
			// Token: 0x0400645D RID: 25693
			public static readonly StringName SetIndexLabel = "SetIndexLabel";

			/// <summary>
			/// Cached name for the 'SetTargetAngle' method.
			/// </summary>
			// Token: 0x0400645E RID: 25694
			public static readonly StringName SetTargetAngle = "SetTargetAngle";

			/// <summary>
			/// Cached name for the 'SetTargetPosition' method.
			/// </summary>
			// Token: 0x0400645F RID: 25695
			public static readonly StringName SetTargetPosition = "SetTargetPosition";

			/// <summary>
			/// Cached name for the 'SetTargetScale' method.
			/// </summary>
			// Token: 0x04006460 RID: 25696
			public static readonly StringName SetTargetScale = "SetTargetScale";

			/// <summary>
			/// Cached name for the 'SetAngleInstantly' method.
			/// </summary>
			// Token: 0x04006461 RID: 25697
			public static readonly StringName SetAngleInstantly = "SetAngleInstantly";

			/// <summary>
			/// Cached name for the 'SetScaleInstantly' method.
			/// </summary>
			// Token: 0x04006462 RID: 25698
			public static readonly StringName SetScaleInstantly = "SetScaleInstantly";

			/// <summary>
			/// Cached name for the 'StopAnimations' method.
			/// </summary>
			// Token: 0x04006463 RID: 25699
			public static readonly StringName StopAnimations = "StopAnimations";

			/// <summary>
			/// Cached name for the 'SetCard' method.
			/// </summary>
			// Token: 0x04006464 RID: 25700
			public new static readonly StringName SetCard = "SetCard";

			/// <summary>
			/// Cached name for the 'UpdateCard' method.
			/// </summary>
			// Token: 0x04006465 RID: 25701
			public static readonly StringName UpdateCard = "UpdateCard";

			/// <summary>
			/// Cached name for the 'BeginDrag' method.
			/// </summary>
			// Token: 0x04006466 RID: 25702
			public static readonly StringName BeginDrag = "BeginDrag";

			/// <summary>
			/// Cached name for the 'CancelDrag' method.
			/// </summary>
			// Token: 0x04006467 RID: 25703
			public static readonly StringName CancelDrag = "CancelDrag";

			/// <summary>
			/// Cached name for the 'SetDefaultTargets' method.
			/// </summary>
			// Token: 0x04006468 RID: 25704
			public static readonly StringName SetDefaultTargets = "SetDefaultTargets";

			/// <summary>
			/// Cached name for the 'Flash' method.
			/// </summary>
			// Token: 0x04006469 RID: 25705
			public static readonly StringName Flash = "Flash";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200196A RID: 6506
		public new class PropertyName : NCardHolder.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InSelectMode' property.
			/// </summary>
			// Token: 0x0400646A RID: 25706
			public static readonly StringName InSelectMode = "InSelectMode";

			/// <summary>
			/// Cached name for the 'TargetPosition' property.
			/// </summary>
			// Token: 0x0400646B RID: 25707
			public static readonly StringName TargetPosition = "TargetPosition";

			/// <summary>
			/// Cached name for the 'TargetAngle' property.
			/// </summary>
			// Token: 0x0400646C RID: 25708
			public static readonly StringName TargetAngle = "TargetAngle";

			/// <summary>
			/// Cached name for the 'ShouldGlowGold' property.
			/// </summary>
			// Token: 0x0400646D RID: 25709
			public static readonly StringName ShouldGlowGold = "ShouldGlowGold";

			/// <summary>
			/// Cached name for the 'ShouldGlowRed' property.
			/// </summary>
			// Token: 0x0400646E RID: 25710
			public static readonly StringName ShouldGlowRed = "ShouldGlowRed";

			/// <summary>
			/// Cached name for the '_flash' field.
			/// </summary>
			// Token: 0x0400646F RID: 25711
			public static readonly StringName _flash = "_flash";

			/// <summary>
			/// Cached name for the '_flashTween' field.
			/// </summary>
			// Token: 0x04006470 RID: 25712
			public static readonly StringName _flashTween = "_flashTween";

			/// <summary>
			/// Cached name for the '_handIndexLabel' field.
			/// </summary>
			// Token: 0x04006471 RID: 25713
			public static readonly StringName _handIndexLabel = "_handIndexLabel";

			/// <summary>
			/// Cached name for the '_targetPosition' field.
			/// </summary>
			// Token: 0x04006472 RID: 25714
			public static readonly StringName _targetPosition = "_targetPosition";

			/// <summary>
			/// Cached name for the '_targetAngle' field.
			/// </summary>
			// Token: 0x04006473 RID: 25715
			public static readonly StringName _targetAngle = "_targetAngle";

			/// <summary>
			/// Cached name for the '_targetScale' field.
			/// </summary>
			// Token: 0x04006474 RID: 25716
			public static readonly StringName _targetScale = "_targetScale";

			/// <summary>
			/// Cached name for the '_hand' field.
			/// </summary>
			// Token: 0x04006475 RID: 25717
			public static readonly StringName _hand = "_hand";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200196B RID: 6507
		public new class SignalName : NCardHolder.SignalName
		{
			/// <summary>
			/// Cached name for the 'HolderFocused' signal.
			/// </summary>
			// Token: 0x04006476 RID: 25718
			public static readonly StringName HolderFocused = "HolderFocused";

			/// <summary>
			/// Cached name for the 'HolderUnfocused' signal.
			/// </summary>
			// Token: 0x04006477 RID: 25719
			public static readonly StringName HolderUnfocused = "HolderUnfocused";

			/// <summary>
			/// Cached name for the 'HolderMouseClicked' signal.
			/// </summary>
			// Token: 0x04006478 RID: 25720
			public static readonly StringName HolderMouseClicked = "HolderMouseClicked";
		}
	}
}
