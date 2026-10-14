using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// End Turn button. Differs from other buttons as there are several flags which interact with each other and a pulsing vfx:
	///    State.Shown: Indicates that this button is press-able. Additionally, flies on screen from the bottom with different behavior from isHidden.
	///    State.Disabled: Grayed out and cannot be pressed.
	///    State.Hidden: Grayed out and cannot be pressed. Additionally, flies off screen downward with different behavior from isHidden.
	///    isShiny: Used when you have no valid cards to play so the button is visually more noticeable.
	///    !isShiny: The opposite behavior of above.
	/// </summary>
	// Token: 0x020003FE RID: 1022
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NEndTurnButton.cs")]
	public class NEndTurnButton : NButton
	{
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003F81 RID: 16257 RVA: 0x001E6C97 File Offset: 0x001E4E97
		private static string EndTurnButtonPath
		{
			get
			{
				return "res://images/packed/combat_ui/end_turn_button.png";
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003F82 RID: 16258 RVA: 0x001E6C9E File Offset: 0x001E4E9E
		private static string EndTurnButtonGlowPath
		{
			get
			{
				return "res://images/packed/combat_ui/end_turn_button_glow.png";
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x001E6CA5 File Offset: 0x001E4EA5
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(NEndTurnButton.EndTurnButtonPath);
				list.Add(NEndTurnButton.EndTurnButtonGlowPath);
				list.AddRange(NMultiplayerVoteContainer.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x001E6CD2 File Offset: 0x001E4ED2
		private bool CanTurnBeEnded
		{
			get
			{
				return !NCombatRoom.Instance.Ui.Hand.InCardPlay && NCombatRoom.Instance.Ui.Hand.CurrentMode == NPlayerHand.Mode.Play;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003F85 RID: 16261 RVA: 0x001E6D04 File Offset: 0x001E4F04
		private Vector2 ShowPos
		{
			get
			{
				return NEndTurnButton._showPosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x001E6D30 File Offset: 0x001E4F30
		private Vector2 HidePos
		{
			get
			{
				return NEndTurnButton._hidePosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003F87 RID: 16263 RVA: 0x001E6D5A File Offset: 0x001E4F5A
		protected override string[] Hotkeys
		{
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x001E6D70 File Offset: 0x001E4F70
		public override void _Ready()
		{
			this.ConnectSignals();
			this._visuals = base.GetNode<Control>("Visuals");
			this._image = base.GetNode<TextureRect>("Visuals/Image");
			this._hsv = (ShaderMaterial)this._image.Material;
			this._glow = base.GetNode<TextureRect>("Visuals/Glow");
			this._glowVfx = base.GetNode<TextureRect>("Visuals/GlowVfx");
			this._label = base.GetNode<MegaLabel>("Visuals/Label");
			this._playerIconContainer = base.GetNode<NMultiplayerVoteContainer>("PlayerIconContainer");
			this._longPressBar = base.GetNode<NEndTurnLongPressBar>("%Bar");
			this._longPressBar.Init(this);
			base.Disable();
			this._combatUi = base.GetParent<NCombatUi>();
			this._viewport = base.GetViewport();
			this._glowTexture = PreloadManager.Cache.GetCompressedTexture2D(NEndTurnButton.EndTurnButtonGlowPath);
			this._normalTexture = PreloadManager.Cache.GetCompressedTexture2D(NEndTurnButton.EndTurnButtonPath);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x001E6E88 File Offset: 0x001E5088
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.TurnStarted += this.OnTurnStarted;
			CombatManager.Instance.AboutToSwitchToEnemyTurn += this.OnAboutToSwitchToEnemyTurn;
			CombatManager.Instance.PlayerEndedTurn += this.AfterPlayerEndedTurn;
			CombatManager.Instance.PlayerUnendedTurn += this.AfterPlayerUnendedTurn;
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x001E6F10 File Offset: 0x001E5110
		public override void _ExitTree()
		{
			base._ExitTree();
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			CombatManager.Instance.TurnStarted -= this.OnTurnStarted;
			CombatManager.Instance.AboutToSwitchToEnemyTurn -= this.OnAboutToSwitchToEnemyTurn;
			CombatManager.Instance.PlayerEndedTurn -= this.AfterPlayerEndedTurn;
			CombatManager.Instance.PlayerUnendedTurn -= this.AfterPlayerUnendedTurn;
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x001E6FB8 File Offset: 0x001E51B8
		public void Initialize(CombatState state)
		{
			this._combatState = state;
			this._playerHand = PileType.Hand.GetPile(LocalContext.GetMe(this._combatState));
			this._playerIconContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerIcon), this._combatState.Players);
		}

		/// <summary>
		/// Returns true when the player's icon should be displayed above the end turn button, indicating readiness.
		/// The icons are only refreshed when _playerIconContainer.RefreshPlayerVotes is called.
		/// </summary>
		// Token: 0x06003F8C RID: 16268 RVA: 0x001E7005 File Offset: 0x001E5205
		private bool ShouldDisplayPlayerIcon(Player player)
		{
			return CombatManager.Instance.IsPlayerReadyToEndTurn(player);
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x001E7012 File Offset: 0x001E5212
		private bool PlayerCanTakeAction(Player player)
		{
			return player.Creature.IsAlive && (CombatManager.Instance.PlayersTakingExtraTurn.Count == 0 || CombatManager.Instance.PlayersTakingExtraTurn.Contains(player));
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x001E7048 File Offset: 0x001E5248
		private void AfterPlayerEndedTurn(Player player, bool canBackOut)
		{
			this._playerIconContainer.RefreshPlayerVotes(true);
			if (LocalContext.IsMe(player))
			{
				this.StartOrStopPulseVfx();
				Player me = LocalContext.GetMe(player.Creature.CombatState);
				if (!CombatManager.Instance.AllPlayersReadyToEndTurn() && this.PlayerCanTakeAction(me) && canBackOut)
				{
					this.SetState(NEndTurnButton.State.Enabled);
					this._label.SetTextAutoSize(new LocString("gameplay_ui", "UNDO_END_TURN_BUTTON").GetFormattedText());
				}
				else
				{
					this.SetState(NEndTurnButton.State.Disabled);
				}
			}
			if (CombatManager.Instance.AllPlayersReadyToEndTurn())
			{
				this.SetState(NEndTurnButton.State.Disabled);
			}
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x001E70DC File Offset: 0x001E52DC
		private void AfterPlayerUnendedTurn(Player player)
		{
			this._playerIconContainer.RefreshPlayerVotes(true);
			if (!LocalContext.IsMe(player))
			{
				return;
			}
			if (!this.PlayerCanTakeAction(player))
			{
				return;
			}
			this.SetState(NEndTurnButton.State.Enabled);
			NEndTurnButton._endTurnLoc.Add("turnNumber", player.PlayerCombatState.TurnNumber);
			this._label.SetTextAutoSize(NEndTurnButton._endTurnLoc.GetFormattedText());
			this.StartOrStopPulseVfx();
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x001E7149 File Offset: 0x001E5349
		private void OnAboutToSwitchToEnemyTurn(CombatState _)
		{
			this.SetState(NEndTurnButton.State.Hidden);
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x001E7154 File Offset: 0x001E5354
		private void OnTurnStarted(CombatState state)
		{
			if (state.CurrentSide != CombatSide.Player)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			this._playerIconContainer.RefreshPlayerVotes(false);
			Player me = LocalContext.GetMe(state);
			NEndTurnButton._endTurnLoc.Add("turnNumber", me.PlayerCombatState.TurnNumber);
			this._label.SetTextAutoSize(NEndTurnButton._endTurnLoc.GetFormattedText());
			if (this.PlayerCanTakeAction(me))
			{
				this.SetState(NEndTurnButton.State.Enabled);
				return;
			}
			this.AnimIn();
			this.SetState(NEndTurnButton.State.Disabled);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x001E71DD File Offset: 0x001E53DD
		private void OnCombatStateChanged(CombatState combatState)
		{
			this.StartOrStopPulseVfx();
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x001E71E8 File Offset: 0x001E53E8
		private void StartOrStopPulseVfx()
		{
			bool flag = !this.HasPlayableCard() && !CombatManager.Instance.IsPlayerReadyToEndTurn(LocalContext.GetMe(this._combatState)) && this._state == NEndTurnButton.State.Enabled;
			if (this._isShiny)
			{
				if (!flag)
				{
					this._isShiny = false;
					Tween glowEnableTween = this._glowEnableTween;
					if (glowEnableTween != null)
					{
						glowEnableTween.Kill();
					}
					this._glowEnableTween = base.CreateTween().SetParallel(true);
					this._glowEnableTween.TweenProperty(this._glow, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(5L);
					Tween glowVfxTween = this._glowVfxTween;
					if (glowVfxTween != null)
					{
						glowVfxTween.Kill();
					}
					this._glowVfxTween = base.CreateTween();
					this._glowVfxTween.TweenProperty(this._glowVfx, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(5L);
					return;
				}
			}
			else if (flag)
			{
				this._isShiny = true;
				Tween glowVfxTween2 = this._glowVfxTween;
				if (glowVfxTween2 != null)
				{
					glowVfxTween2.Kill();
				}
				this.GlowPulse();
				Tween glowEnableTween2 = this._glowEnableTween;
				if (glowEnableTween2 != null)
				{
					glowEnableTween2.Kill();
				}
				this._glowEnableTween = base.CreateTween().SetParallel(true);
				this._glowEnableTween.TweenProperty(this._glow, "modulate:a", 0.75f, 0.8).SetEase(1L).SetTrans(10L);
				this._glowEnableTween.TweenProperty(this._glow, "scale", Vector2.One * 0.5f, 0.8).SetEase(1L).SetTrans(10L)
					.From(Vector2.One * 0.45f);
			}
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x001E73DC File Offset: 0x001E55DC
		private void GlowPulse()
		{
			this._glowVfxTween = base.CreateTween().SetParallel(true).SetLoops(0);
			this._glowVfxTween.TweenProperty(this._glowVfx, "scale", Vector2.One * 0.7f, 1.5).From(Vector2.One * 0.5f).SetEase(1L)
				.SetTrans(3L);
			this._glowVfxTween.TweenProperty(this._glowVfx, "modulate:a", 0f, 1.5).From(0.4f);
		}

		/// <summary>
		/// Things which occur when the EndTurn button is pressed
		/// </summary>
		// Token: 0x06003F95 RID: 16277 RVA: 0x001E749F File Offset: 0x001E569F
		protected override void OnRelease()
		{
			if (this.ShouldShowPlayableCardsFtue())
			{
				return;
			}
			if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
			{
				this._longPressBar.CancelPress();
				return;
			}
			this.CallReleaseLogic();
		}

		/// <summary>
		/// Helper because we can call the End Turn button in numerous ways (controller, mouse, long press mouse, etc)
		/// </summary>
		// Token: 0x06003F96 RID: 16278 RVA: 0x001E74D0 File Offset: 0x001E56D0
		public void CallReleaseLogic()
		{
			if (!this.CanTurnBeEnded)
			{
				return;
			}
			Tween glowEnableTween = this._glowEnableTween;
			if (glowEnableTween != null)
			{
				glowEnableTween.Kill();
			}
			this._glowEnableTween = base.CreateTween().SetParallel(true);
			this._glowEnableTween.TweenProperty(this._glow, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(5L);
			Player me = LocalContext.GetMe(this._combatState);
			int turnNumber = me.PlayerCombatState.TurnNumber;
			if (!CombatManager.Instance.IsPlayerReadyToEndTurn(me))
			{
				this.SetState(NEndTurnButton.State.Disabled);
				RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new EndPlayerTurnAction(me, turnNumber));
				return;
			}
			this.SetState(NEndTurnButton.State.Disabled);
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new UndoEndPlayerTurnAction(me, turnNumber));
		}

		/// <summary>
		/// Don't call me &gt;:(
		/// </summary>
		// Token: 0x06003F97 RID: 16279 RVA: 0x001E75A8 File Offset: 0x001E57A8
		public void SecretEndTurnLogicViaFtue()
		{
			Tween glowEnableTween = this._glowEnableTween;
			if (glowEnableTween != null)
			{
				glowEnableTween.Kill();
			}
			this._glowEnableTween = base.CreateTween().SetParallel(true);
			this._glowEnableTween.TweenProperty(this._glow, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(5L);
			Player me = LocalContext.GetMe(this._combatState);
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new EndPlayerTurnAction(me, me.PlayerCombatState.TurnNumber));
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x001E7644 File Offset: 0x001E5844
		private bool ShouldShowPlayableCardsFtue()
		{
			if (SaveManager.Instance.SeenFtue("can_play_cards_ftue"))
			{
				return false;
			}
			bool flag = LocalContext.GetMe(this._combatState).PlayerCombatState.HasCardsToPlay();
			if (flag)
			{
				NModalContainer.Instance.Add(NCanPlayCardsFtue.Create(), true);
				SaveManager.Instance.MarkFtueAsComplete("can_play_cards_ftue");
			}
			else
			{
				this._endTurnWithNoPlayableCardsCount++;
				if (this._endTurnWithNoPlayableCardsCount == 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Ended ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(3);
					defaultInterpolatedStringHandler.AppendLiteral(" turns without cards left to play. Good job! Disabling can_play_cards ftue.");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					SaveManager.Instance.MarkFtueAsComplete("can_play_cards_ftue");
				}
			}
			return flag;
		}

		/// <summary>
		/// Called at the start of player turn, and when the button re-enables itself if not everyone in multiplayer is ready.
		/// </summary>
		// Token: 0x06003F99 RID: 16281 RVA: 0x001E76FC File Offset: 0x001E58FC
		protected override void OnEnable()
		{
			base.OnEnable();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.FastForwardToCompletion();
			}
			this._image.Texture = this._normalTexture;
			this._image.Modulate = Colors.White;
			this._label.Modulate = StsColors.cream;
		}

		/// <summary>
		/// Called when the player presses End Turn or combat ends.
		/// </summary>
		// Token: 0x06003F9A RID: 16282 RVA: 0x001E7754 File Offset: 0x001E5954
		protected override void OnDisable()
		{
			base.OnDisable();
			NHoverTipSet.Remove(this);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.FastForwardToCompletion();
			}
			this._image.Modulate = StsColors.gray;
			this._label.Modulate = StsColors.gray;
			this.StartOrStopPulseVfx();
		}

		/// <summary>
		/// Animates the button out off the screen and disables the button if it is currently enabled.
		/// </summary>
		// Token: 0x06003F9B RID: 16283 RVA: 0x001E77A4 File Offset: 0x001E59A4
		private void AnimOut()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this.HidePos, 0.5).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Animates the button in from off the screen.
		/// </summary>
		// Token: 0x06003F9C RID: 16284 RVA: 0x001E7818 File Offset: 0x001E5A18
		private void AnimIn()
		{
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this.ShowPos, 0.5).SetEase(1L).SetTrans(10L);
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x001E787C File Offset: 0x001E5A7C
		public void OnCombatEnded()
		{
			this.SetState(NEndTurnButton.State.Hidden);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x001E7888 File Offset: 0x001E5A88
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hsv.SetShaderParameter(NEndTurnButton._v, 1.5);
			this._visuals.Position = new Vector2(0f, -2f);
			Player me = LocalContext.GetMe(this._combatState);
			if (CombatManager.Instance.IsPlayerReadyToEndTurn(me))
			{
				this._label.Modulate = StsColors.cream;
				return;
			}
			this._label.Modulate = (me.PlayerCombatState.HasCardsToPlay() ? StsColors.red : Colors.Cyan);
			this._combatUi.Hand.FlashPlayableHolders();
			LocString locString = new LocString("static_hover_tips", "END_TURN.title");
			locString.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.accept).ToString());
			HoverTip hoverTip = new HoverTip(locString, new LocString("static_hover_tips", "END_TURN.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet == null)
			{
				return;
			}
			nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NEndTurnButton._hoverTipOffset, false);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x001E79C0 File Offset: 0x001E5BC0
		private bool HasPlayableCard()
		{
			if (this._playerHand == null)
			{
				return false;
			}
			foreach (CardModel cardModel in this._playerHand.Cards)
			{
				if (cardModel.CanPlay())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x001E7A24 File Offset: 0x001E5C24
		protected override void OnUnfocus()
		{
			if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
			{
				this._longPressBar.CancelPress();
			}
			NHoverTipSet.Remove(this);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEndTurnButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._label, "modulate", base.IsEnabled ? StsColors.cream : StsColors.gray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x001E7B50 File Offset: 0x001E5D50
		protected override void OnPress()
		{
			if (!this.CanTurnBeEnded)
			{
				return;
			}
			if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
			{
				this._longPressBar.StartPress();
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEndTurnButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", new Vector2(0f, 8f), 0.5).SetEase(1L).SetTrans(7L);
			this._hoverTween.TweenProperty(this._label, "modulate", Colors.DarkGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x001E7C77 File Offset: 0x001E5E77
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NEndTurnButton._v, value);
		}

		/// <summary>
		/// Set the state of the button.
		/// Use this instead of calling Enable or Disable directly, as the enabled/disabled state of the button can also be
		/// controlled by the state of the UI above the button (see RefreshEnabled).
		/// </summary>
		// Token: 0x06003FA3 RID: 16291 RVA: 0x001E7C8F File Offset: 0x001E5E8F
		private void SetState(NEndTurnButton.State newState)
		{
			if (this._state != newState)
			{
				if (newState == NEndTurnButton.State.Hidden)
				{
					this.AnimOut();
				}
				if (newState == NEndTurnButton.State.Enabled && this._state == NEndTurnButton.State.Hidden)
				{
					this.AnimIn();
				}
				this._state = newState;
				this.RefreshEnabled();
			}
		}

		/// <summary>
		/// Refreshes whether the button is enabled or not. Should be called when a screen appears/disappears.
		/// Use this instead of calling Enable or Disable directly! The enabled/disabled state of the button should be
		/// controlled by the button itself.
		/// </summary>
		// Token: 0x06003FA4 RID: 16292 RVA: 0x001E7CC4 File Offset: 0x001E5EC4
		public void RefreshEnabled()
		{
			bool flag = NCombatRoom.Instance == null || NCombatRoom.Instance.Mode != CombatRoomMode.ActiveCombat || !ActiveScreenContext.Instance.IsCurrent(NCombatRoom.Instance) || NCombatRoom.Instance.Ui.Hand.IsInCardSelection;
			bool flag2 = this._state == NEndTurnButton.State.Enabled && !flag;
			if (flag2)
			{
				base.Enable();
				return;
			}
			base.Disable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FA5 RID: 16293 RVA: 0x001E7D30 File Offset: 0x001E5F30
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(21)
			{
				new MethodInfo(NEndTurnButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.StartOrStopPulseVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.GlowPulse, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.CallReleaseLogic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnCombatEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.HasPlayableCard, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEndTurnButton.MethodName.SetState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "newState", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEndTurnButton.MethodName.RefreshEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FA6 RID: 16294 RVA: 0x001E816C File Offset: 0x001E636C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEndTurnButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.StartOrStopPulseVfx && args.Count == 0)
			{
				this.StartOrStopPulseVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.GlowPulse && args.Count == 0)
			{
				this.GlowPulse();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.CallReleaseLogic && args.Count == 0)
			{
				this.CallReleaseLogic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue && args.Count == 0)
			{
				this.SecretEndTurnLogicViaFtue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue && args.Count == 0)
			{
				bool flag = this.ShouldShowPlayableCardsFtue();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnCombatEnded && args.Count == 0)
			{
				this.OnCombatEnded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.HasPlayableCard && args.Count == 0)
			{
				bool flag2 = this.HasPlayableCard();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.SetState && args.Count == 1)
			{
				this.SetState(VariantUtils.ConvertTo<NEndTurnButton.State>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnButton.MethodName.RefreshEnabled && args.Count == 0)
			{
				this.RefreshEnabled();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003FA7 RID: 16295 RVA: 0x001E84B8 File Offset: 0x001E66B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEndTurnButton.MethodName._Ready || (ref method) == NEndTurnButton.MethodName._EnterTree || (ref method) == NEndTurnButton.MethodName._ExitTree || (ref method) == NEndTurnButton.MethodName.StartOrStopPulseVfx || (ref method) == NEndTurnButton.MethodName.GlowPulse || (ref method) == NEndTurnButton.MethodName.OnRelease || (ref method) == NEndTurnButton.MethodName.CallReleaseLogic || (ref method) == NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue || (ref method) == NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue || (ref method) == NEndTurnButton.MethodName.OnEnable || (ref method) == NEndTurnButton.MethodName.OnDisable || (ref method) == NEndTurnButton.MethodName.AnimOut || (ref method) == NEndTurnButton.MethodName.AnimIn || (ref method) == NEndTurnButton.MethodName.OnCombatEnded || (ref method) == NEndTurnButton.MethodName.OnFocus || (ref method) == NEndTurnButton.MethodName.HasPlayableCard || (ref method) == NEndTurnButton.MethodName.OnUnfocus || (ref method) == NEndTurnButton.MethodName.OnPress || (ref method) == NEndTurnButton.MethodName.UpdateShaderV || (ref method) == NEndTurnButton.MethodName.SetState || (ref method) == NEndTurnButton.MethodName.RefreshEnabled || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003FA8 RID: 16296 RVA: 0x001E8608 File Offset: 0x001E6808
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEndTurnButton.PropertyName._state)
			{
				this._state = VariantUtils.ConvertTo<NEndTurnButton.State>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._isShiny)
			{
				this._isShiny = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowTexture)
			{
				this._glowTexture = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._normalTexture)
			{
				this._normalTexture = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glow)
			{
				this._glow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowVfx)
			{
				this._glowVfx = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._combatUi)
			{
				this._combatUi = VariantUtils.ConvertTo<NCombatUi>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._playerIconContainer)
			{
				this._playerIconContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._longPressBar)
			{
				this._longPressBar = VariantUtils.ConvertTo<NEndTurnLongPressBar>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._pulseTimer)
			{
				this._pulseTimer = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._positionTween)
			{
				this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowVfxTween)
			{
				this._glowVfxTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowEnableTween)
			{
				this._glowEnableTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount)
			{
				this._endTurnWithNoPlayableCardsCount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003FA9 RID: 16297 RVA: 0x001E883C File Offset: 0x001E6A3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEndTurnButton.PropertyName.CanTurnBeEnded)
			{
				bool canTurnBeEnded = this.CanTurnBeEnded;
				value = VariantUtils.CreateFrom<bool>(ref canTurnBeEnded);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName.ShowPos)
			{
				Vector2 vector = this.ShowPos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName.HidePos)
			{
				Vector2 vector = this.HidePos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._state)
			{
				value = VariantUtils.CreateFrom<NEndTurnButton.State>(ref this._state);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._isShiny)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isShiny);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowTexture)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._glowTexture);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._normalTexture)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._normalTexture);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._glow);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowVfx)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._glowVfx);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._combatUi)
			{
				value = VariantUtils.CreateFrom<NCombatUi>(ref this._combatUi);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._playerIconContainer)
			{
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._playerIconContainer);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._longPressBar)
			{
				value = VariantUtils.CreateFrom<NEndTurnLongPressBar>(ref this._longPressBar);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._pulseTimer)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pulseTimer);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._positionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowVfxTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowVfxTween);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._glowEnableTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowEnableTween);
				return true;
			}
			if ((ref name) == NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._endTurnWithNoPlayableCardsCount);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FAA RID: 16298 RVA: 0x001E8B60 File Offset: 0x001E6D60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NEndTurnButton.PropertyName.CanTurnBeEnded, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEndTurnButton.PropertyName._state, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEndTurnButton.PropertyName._isShiny, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._glowTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._normalTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._glow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._glowVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._combatUi, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._playerIconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._longPressBar, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEndTurnButton.PropertyName._pulseTimer, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEndTurnButton.PropertyName.ShowPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEndTurnButton.PropertyName.HidePos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._positionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._glowVfxTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnButton.PropertyName._glowEnableTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, 0L, "", 4096L, false),
				new PropertyInfo(34L, NEndTurnButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FAB RID: 16299 RVA: 0x001E8E88 File Offset: 0x001E7088
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEndTurnButton.PropertyName._state, Variant.From<NEndTurnButton.State>(ref this._state));
			info.AddProperty(NEndTurnButton.PropertyName._isShiny, Variant.From<bool>(ref this._isShiny));
			info.AddProperty(NEndTurnButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NEndTurnButton.PropertyName._glowTexture, Variant.From<Texture2D>(ref this._glowTexture));
			info.AddProperty(NEndTurnButton.PropertyName._normalTexture, Variant.From<Texture2D>(ref this._normalTexture));
			info.AddProperty(NEndTurnButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NEndTurnButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NEndTurnButton.PropertyName._glow, Variant.From<Control>(ref this._glow));
			info.AddProperty(NEndTurnButton.PropertyName._glowVfx, Variant.From<Control>(ref this._glowVfx));
			info.AddProperty(NEndTurnButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NEndTurnButton.PropertyName._combatUi, Variant.From<NCombatUi>(ref this._combatUi));
			info.AddProperty(NEndTurnButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NEndTurnButton.PropertyName._playerIconContainer, Variant.From<NMultiplayerVoteContainer>(ref this._playerIconContainer));
			info.AddProperty(NEndTurnButton.PropertyName._longPressBar, Variant.From<NEndTurnLongPressBar>(ref this._longPressBar));
			info.AddProperty(NEndTurnButton.PropertyName._pulseTimer, Variant.From<float>(ref this._pulseTimer));
			info.AddProperty(NEndTurnButton.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
			info.AddProperty(NEndTurnButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NEndTurnButton.PropertyName._glowVfxTween, Variant.From<Tween>(ref this._glowVfxTween));
			info.AddProperty(NEndTurnButton.PropertyName._glowEnableTween, Variant.From<Tween>(ref this._glowEnableTween));
			info.AddProperty(NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, Variant.From<int>(ref this._endTurnWithNoPlayableCardsCount));
		}

		/// <inheritdoc />
		// Token: 0x06003FAC RID: 16300 RVA: 0x001E9054 File Offset: 0x001E7254
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._state, ref variant))
			{
				this._state = variant.As<NEndTurnButton.State>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._isShiny, ref variant2))
			{
				this._isShiny = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._visuals, ref variant3))
			{
				this._visuals = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._glowTexture, ref variant4))
			{
				this._glowTexture = variant4.As<Texture2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._normalTexture, ref variant5))
			{
				this._normalTexture = variant5.As<Texture2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._image, ref variant6))
			{
				this._image = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._glow, ref variant8))
			{
				this._glow = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._glowVfx, ref variant9))
			{
				this._glowVfx = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._label, ref variant10))
			{
				this._label = variant10.As<MegaLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._combatUi, ref variant11))
			{
				this._combatUi = variant11.As<NCombatUi>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._viewport, ref variant12))
			{
				this._viewport = variant12.As<Viewport>();
			}
			Variant variant13;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._playerIconContainer, ref variant13))
			{
				this._playerIconContainer = variant13.As<NMultiplayerVoteContainer>();
			}
			Variant variant14;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._longPressBar, ref variant14))
			{
				this._longPressBar = variant14.As<NEndTurnLongPressBar>();
			}
			Variant variant15;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._pulseTimer, ref variant15))
			{
				this._pulseTimer = variant15.As<float>();
			}
			Variant variant16;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._positionTween, ref variant16))
			{
				this._positionTween = variant16.As<Tween>();
			}
			Variant variant17;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._hoverTween, ref variant17))
			{
				this._hoverTween = variant17.As<Tween>();
			}
			Variant variant18;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._glowVfxTween, ref variant18))
			{
				this._glowVfxTween = variant18.As<Tween>();
			}
			Variant variant19;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._glowEnableTween, ref variant19))
			{
				this._glowEnableTween = variant19.As<Tween>();
			}
			Variant variant20;
			if (info.TryGetProperty(NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, ref variant20))
			{
				this._endTurnWithNoPlayableCardsCount = variant20.As<int>();
			}
		}

		// Token: 0x04001893 RID: 6291
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001894 RID: 6292
		private const float _flyInOutDuration = 0.5f;

		// Token: 0x04001895 RID: 6293
		private static readonly LocString _endTurnLoc = new LocString("gameplay_ui", "END_TURN_BUTTON");

		// Token: 0x04001896 RID: 6294
		[Nullable(2)]
		private CombatState _combatState;

		// Token: 0x04001897 RID: 6295
		[Nullable(2)]
		private CardPile _playerHand;

		// Token: 0x04001898 RID: 6296
		private NEndTurnButton.State _state = NEndTurnButton.State.Hidden;

		// Token: 0x04001899 RID: 6297
		private bool _isShiny;

		// Token: 0x0400189A RID: 6298
		private Control _visuals;

		// Token: 0x0400189B RID: 6299
		private Texture2D _glowTexture;

		// Token: 0x0400189C RID: 6300
		private Texture2D _normalTexture;

		// Token: 0x0400189D RID: 6301
		private TextureRect _image;

		// Token: 0x0400189E RID: 6302
		private ShaderMaterial _hsv;

		// Token: 0x0400189F RID: 6303
		private Control _glow;

		// Token: 0x040018A0 RID: 6304
		private Control _glowVfx;

		// Token: 0x040018A1 RID: 6305
		private MegaLabel _label;

		// Token: 0x040018A2 RID: 6306
		private NCombatUi _combatUi;

		// Token: 0x040018A3 RID: 6307
		private Viewport _viewport;

		// Token: 0x040018A4 RID: 6308
		private NMultiplayerVoteContainer _playerIconContainer;

		// Token: 0x040018A5 RID: 6309
		private NEndTurnLongPressBar _longPressBar;

		// Token: 0x040018A6 RID: 6310
		private float _pulseTimer = 1f;

		// Token: 0x040018A7 RID: 6311
		private static readonly Vector2 _hoverTipOffset = new Vector2(-76f, -302f);

		// Token: 0x040018A8 RID: 6312
		private static readonly Vector2 _showPosRatio = new Vector2(1604f, 846f) / NGame.devResolution;

		// Token: 0x040018A9 RID: 6313
		private static readonly Vector2 _hidePosRatio = NEndTurnButton._showPosRatio + new Vector2(0f, 250f) / NGame.devResolution;

		// Token: 0x040018AA RID: 6314
		[Nullable(2)]
		private Tween _positionTween;

		// Token: 0x040018AB RID: 6315
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040018AC RID: 6316
		[Nullable(2)]
		private Tween _glowVfxTween;

		// Token: 0x040018AD RID: 6317
		[Nullable(2)]
		private Tween _glowEnableTween;

		// Token: 0x040018AE RID: 6318
		private const int _ftueDisableEndTurnCount = 3;

		// Token: 0x040018AF RID: 6319
		private int _endTurnWithNoPlayableCardsCount;

		// Token: 0x020018DC RID: 6364
		[NullableContext(0)]
		private enum State
		{
			// Token: 0x04006159 RID: 24921
			Enabled,
			// Token: 0x0400615A RID: 24922
			Disabled,
			// Token: 0x0400615B RID: 24923
			Hidden
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018DD RID: 6365
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400615C RID: 24924
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400615D RID: 24925
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400615E RID: 24926
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'StartOrStopPulseVfx' method.
			/// </summary>
			// Token: 0x0400615F RID: 24927
			public static readonly StringName StartOrStopPulseVfx = "StartOrStopPulseVfx";

			/// <summary>
			/// Cached name for the 'GlowPulse' method.
			/// </summary>
			// Token: 0x04006160 RID: 24928
			public static readonly StringName GlowPulse = "GlowPulse";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04006161 RID: 24929
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'CallReleaseLogic' method.
			/// </summary>
			// Token: 0x04006162 RID: 24930
			public static readonly StringName CallReleaseLogic = "CallReleaseLogic";

			/// <summary>
			/// Cached name for the 'SecretEndTurnLogicViaFtue' method.
			/// </summary>
			// Token: 0x04006163 RID: 24931
			public static readonly StringName SecretEndTurnLogicViaFtue = "SecretEndTurnLogicViaFtue";

			/// <summary>
			/// Cached name for the 'ShouldShowPlayableCardsFtue' method.
			/// </summary>
			// Token: 0x04006164 RID: 24932
			public static readonly StringName ShouldShowPlayableCardsFtue = "ShouldShowPlayableCardsFtue";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04006165 RID: 24933
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04006166 RID: 24934
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x04006167 RID: 24935
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04006168 RID: 24936
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'OnCombatEnded' method.
			/// </summary>
			// Token: 0x04006169 RID: 24937
			public static readonly StringName OnCombatEnded = "OnCombatEnded";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400616A RID: 24938
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'HasPlayableCard' method.
			/// </summary>
			// Token: 0x0400616B RID: 24939
			public static readonly StringName HasPlayableCard = "HasPlayableCard";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400616C RID: 24940
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400616D RID: 24941
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400616E RID: 24942
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the 'SetState' method.
			/// </summary>
			// Token: 0x0400616F RID: 24943
			public static readonly StringName SetState = "SetState";

			/// <summary>
			/// Cached name for the 'RefreshEnabled' method.
			/// </summary>
			// Token: 0x04006170 RID: 24944
			public static readonly StringName RefreshEnabled = "RefreshEnabled";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018DE RID: 6366
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CanTurnBeEnded' property.
			/// </summary>
			// Token: 0x04006171 RID: 24945
			public static readonly StringName CanTurnBeEnded = "CanTurnBeEnded";

			/// <summary>
			/// Cached name for the 'ShowPos' property.
			/// </summary>
			// Token: 0x04006172 RID: 24946
			public static readonly StringName ShowPos = "ShowPos";

			/// <summary>
			/// Cached name for the 'HidePos' property.
			/// </summary>
			// Token: 0x04006173 RID: 24947
			public static readonly StringName HidePos = "HidePos";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04006174 RID: 24948
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_state' field.
			/// </summary>
			// Token: 0x04006175 RID: 24949
			public static readonly StringName _state = "_state";

			/// <summary>
			/// Cached name for the '_isShiny' field.
			/// </summary>
			// Token: 0x04006176 RID: 24950
			public static readonly StringName _isShiny = "_isShiny";

			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x04006177 RID: 24951
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_glowTexture' field.
			/// </summary>
			// Token: 0x04006178 RID: 24952
			public static readonly StringName _glowTexture = "_glowTexture";

			/// <summary>
			/// Cached name for the '_normalTexture' field.
			/// </summary>
			// Token: 0x04006179 RID: 24953
			public static readonly StringName _normalTexture = "_normalTexture";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x0400617A RID: 24954
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x0400617B RID: 24955
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_glow' field.
			/// </summary>
			// Token: 0x0400617C RID: 24956
			public static readonly StringName _glow = "_glow";

			/// <summary>
			/// Cached name for the '_glowVfx' field.
			/// </summary>
			// Token: 0x0400617D RID: 24957
			public static readonly StringName _glowVfx = "_glowVfx";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400617E RID: 24958
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_combatUi' field.
			/// </summary>
			// Token: 0x0400617F RID: 24959
			public static readonly StringName _combatUi = "_combatUi";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04006180 RID: 24960
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_playerIconContainer' field.
			/// </summary>
			// Token: 0x04006181 RID: 24961
			public static readonly StringName _playerIconContainer = "_playerIconContainer";

			/// <summary>
			/// Cached name for the '_longPressBar' field.
			/// </summary>
			// Token: 0x04006182 RID: 24962
			public static readonly StringName _longPressBar = "_longPressBar";

			/// <summary>
			/// Cached name for the '_pulseTimer' field.
			/// </summary>
			// Token: 0x04006183 RID: 24963
			public static readonly StringName _pulseTimer = "_pulseTimer";

			/// <summary>
			/// Cached name for the '_positionTween' field.
			/// </summary>
			// Token: 0x04006184 RID: 24964
			public static readonly StringName _positionTween = "_positionTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04006185 RID: 24965
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_glowVfxTween' field.
			/// </summary>
			// Token: 0x04006186 RID: 24966
			public static readonly StringName _glowVfxTween = "_glowVfxTween";

			/// <summary>
			/// Cached name for the '_glowEnableTween' field.
			/// </summary>
			// Token: 0x04006187 RID: 24967
			public static readonly StringName _glowEnableTween = "_glowEnableTween";

			/// <summary>
			/// Cached name for the '_endTurnWithNoPlayableCardsCount' field.
			/// </summary>
			// Token: 0x04006188 RID: 24968
			public static readonly StringName _endTurnWithNoPlayableCardsCount = "_endTurnWithNoPlayableCardsCount";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018DF RID: 6367
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
