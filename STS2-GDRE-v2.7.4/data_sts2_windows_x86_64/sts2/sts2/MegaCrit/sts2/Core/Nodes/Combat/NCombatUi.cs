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
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x020003F7 RID: 1015
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCombatUi.cs")]
	public class NCombatUi : Control
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003E5E RID: 15966 RVA: 0x001DDE2D File Offset: 0x001DC02D
		// (set) Token: 0x06003E5F RID: 15967 RVA: 0x001DDE35 File Offset: 0x001DC035
		public Control EnergyCounterContainer { get; private set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003E60 RID: 15968 RVA: 0x001DDE3E File Offset: 0x001DC03E
		// (set) Token: 0x06003E61 RID: 15969 RVA: 0x001DDE46 File Offset: 0x001DC046
		public NEndTurnButton EndTurnButton { get; private set; }

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x001DDE4F File Offset: 0x001DC04F
		// (set) Token: 0x06003E63 RID: 15971 RVA: 0x001DDE57 File Offset: 0x001DC057
		private NPingButton PingButton { get; set; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x001DDE60 File Offset: 0x001DC060
		public NDrawPileButton DrawPile
		{
			get
			{
				return this._combatPilesContainer.DrawPile;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x001DDE6D File Offset: 0x001DC06D
		public NDiscardPileButton DiscardPile
		{
			get
			{
				return this._combatPilesContainer.DiscardPile;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003E66 RID: 15974 RVA: 0x001DDE7A File Offset: 0x001DC07A
		public NExhaustPileButton ExhaustPile
		{
			get
			{
				return this._combatPilesContainer.ExhaustPile;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x001DDE87 File Offset: 0x001DC087
		// (set) Token: 0x06003E68 RID: 15976 RVA: 0x001DDE8F File Offset: 0x001DC08F
		public NPlayerHand Hand { get; private set; }

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x001DDE98 File Offset: 0x001DC098
		// (set) Token: 0x06003E6A RID: 15978 RVA: 0x001DDEA0 File Offset: 0x001DC0A0
		public Control PlayContainer { get; private set; }

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x001DDEA9 File Offset: 0x001DC0A9
		// (set) Token: 0x06003E6C RID: 15980 RVA: 0x001DDEB1 File Offset: 0x001DC0B1
		public NCardPlayQueue PlayQueue { get; private set; }

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x001DDEBA File Offset: 0x001DC0BA
		// (set) Token: 0x06003E6E RID: 15982 RVA: 0x001DDEC2 File Offset: 0x001DC0C2
		public Control CardPreviewContainer { get; private set; }

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003E6F RID: 15983 RVA: 0x001DDECB File Offset: 0x001DC0CB
		// (set) Token: 0x06003E70 RID: 15984 RVA: 0x001DDED3 File Offset: 0x001DC0D3
		public NMessyCardPreviewContainer MessyCardPreviewContainer { get; private set; }

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003E71 RID: 15985 RVA: 0x001DDEDC File Offset: 0x001DC0DC
		private IEnumerable<NCard> PlayContainerCards
		{
			get
			{
				return this.PlayContainer.GetChildren(false).OfType<NCard>();
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003E72 RID: 15986 RVA: 0x001DDEEF File Offset: 0x001DC0EF
		// (set) Token: 0x06003E73 RID: 15987 RVA: 0x001DDEF6 File Offset: 0x001DC0F6
		public static bool IsDebugHidingIntent { get; private set; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003E74 RID: 15988 RVA: 0x001DDEFE File Offset: 0x001DC0FE
		// (set) Token: 0x06003E75 RID: 15989 RVA: 0x001DDF05 File Offset: 0x001DC105
		public static bool IsDebugHidingPlayContainer { get; private set; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003E76 RID: 15990 RVA: 0x001DDF0D File Offset: 0x001DC10D
		// (set) Token: 0x06003E77 RID: 15991 RVA: 0x001DDF14 File Offset: 0x001DC114
		public static bool IsDebugHidingHpBar { get; private set; }

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003E78 RID: 15992 RVA: 0x001DDF1C File Offset: 0x001DC11C
		// (set) Token: 0x06003E79 RID: 15993 RVA: 0x001DDF23 File Offset: 0x001DC123
		public static bool IsDebugHideTextVfx { get; private set; }

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003E7A RID: 15994 RVA: 0x001DDF2B File Offset: 0x001DC12B
		// (set) Token: 0x06003E7B RID: 15995 RVA: 0x001DDF32 File Offset: 0x001DC132
		public static bool IsDebugHideTargetingUi { get; private set; }

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003E7C RID: 15996 RVA: 0x001DDF3A File Offset: 0x001DC13A
		// (set) Token: 0x06003E7D RID: 15997 RVA: 0x001DDF41 File Offset: 0x001DC141
		public static bool IsDebugHideMpTargetingUi { get; private set; }

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x001DDF49 File Offset: 0x001DC149
		// (set) Token: 0x06003E7F RID: 15999 RVA: 0x001DDF50 File Offset: 0x001DC150
		public static bool IsDebugHideMpIntents { get; private set; }

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06003E80 RID: 16000 RVA: 0x001DDF58 File Offset: 0x001DC158
		// (remove) Token: 0x06003E81 RID: 16001 RVA: 0x001DDF90 File Offset: 0x001DC190
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DebugToggleIntent;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06003E82 RID: 16002 RVA: 0x001DDFC8 File Offset: 0x001DC1C8
		// (remove) Token: 0x06003E83 RID: 16003 RVA: 0x001DE000 File Offset: 0x001DC200
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DebugToggleHpBar;

		// Token: 0x06003E84 RID: 16004 RVA: 0x001DE038 File Offset: 0x001DC238
		public override void _Ready()
		{
			this.EnergyCounterContainer = base.GetNode<Control>("%EnergyCounterContainer");
			this._starCounter = base.GetNode<NStarCounter>("%StarCounter");
			this.EndTurnButton = base.GetNode<NEndTurnButton>("%EndTurnButton");
			this.PingButton = base.GetNode<NPingButton>("%PingButton");
			this._combatPilesContainer = base.GetNode<NCombatPilesContainer>("%CombatPileContainer");
			this.Hand = base.GetNode<NPlayerHand>("%Hand");
			this.PlayContainer = base.GetNode<Control>("%PlayContainer");
			this.PlayQueue = base.GetNode<NCardPlayQueue>("%PlayQueue");
			this.CardPreviewContainer = base.GetNode<Control>("%CardPreviewContainer");
			this.MessyCardPreviewContainer = base.GetNode<NMessyCardPreviewContainer>("%MessyCardPreviewContainer");
			if (NCombatUi._isDebugHidden)
			{
				foreach (Control control in base.GetChildren(false).OfType<Control>())
				{
					if (control != this.Hand)
					{
						control.Modulate = (NCombatUi._isDebugHidden ? Colors.Transparent : Colors.White);
					}
				}
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x001DE18C File Offset: 0x001DC38C
		public override void _ExitTree()
		{
			this._cts.Cancel();
			this.DisconnectSignals();
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x001DE1A0 File Offset: 0x001DC3A0
		public void Activate(CombatState state)
		{
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
			CombatManager.Instance.CombatWon += this.OnCombatWon;
			this._state = state;
			Player me = LocalContext.GetMe(this._state);
			this._combatPilesContainer.Initialize(me);
			this._starCounter.Initialize(me);
			this.EndTurnButton.Initialize(state);
			if (me.Character.ShouldAlwaysShowStarCounter)
			{
				this.EnergyCounterContainer.SetPosition(new Vector2(100f, 806f), true);
			}
			this._energyCounter = NEnergyCounter.Create(me);
			this.EnergyCounterContainer.AddChildSafely(this._energyCounter);
			this._starCounter.Reparent(this._energyCounter, true);
			base.Visible = true;
			this.AnimIn();
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x001DE274 File Offset: 0x001DC474
		public void Deactivate()
		{
			this.DisconnectSignals();
			base.Visible = false;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x001DE283 File Offset: 0x001DC483
		private void DisconnectSignals()
		{
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
			CombatManager.Instance.CombatWon -= this.OnCombatWon;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x001DE2B1 File Offset: 0x001DC4B1
		public void AddToPlayContainer(NCard card)
		{
			Node parent = card.GetParent();
			if (parent != null)
			{
				parent.RemoveChildSafely(card);
			}
			this.PlayContainer.AddChildSafely(card);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x001DE2D4 File Offset: 0x001DC4D4
		[return: Nullable(2)]
		public NCard GetCardFromPlayContainer(CardModel model)
		{
			return this.PlayContainerCards.FirstOrDefault<NCard>((NCard n) => n.Model == model);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x001DE305 File Offset: 0x001DC505
		private void OnCombatEnded(CombatRoom combatRoom)
		{
			this.AnimOut();
			this.PostCombatCleanUp();
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x001DE313 File Offset: 0x001DC513
		private void OnCombatWon(CombatRoom room)
		{
			if (room.Encounter.ShouldGiveRewards)
			{
				TaskHelper.RunSafely(this.ShowRewards(room));
				return;
			}
			TaskHelper.RunSafely(this.ProceedWithoutRewards());
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x001DE33C File Offset: 0x001DC53C
		public async Task ProceedWithoutRewards()
		{
			await Cmd.Wait(1f, this._cts.Token, false);
			await RunManager.Instance.ProceedFromTerminalRewardsScreen();
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x001DE380 File Offset: 0x001DC580
		private async Task ShowRewards(CombatRoom room)
		{
			float num = 0f;
			foreach (NCreature ncreature in NCombatRoom.Instance.RemovingCreatureNodes)
			{
				if (ncreature != null && ncreature.HasSpineAnimation && ncreature.IsPlayingDeathAnimation)
				{
					num = Math.Max(num, ncreature.GetCurrentAnimationTimeRemaining());
				}
				else
				{
					MonsterModel monster = ncreature.Entity.Monster;
					if (monster != null && monster.HasDeathAnimLengthOverride)
					{
						num = Math.Max(num, ncreature.Entity.Monster.DeathAnimLengthOverride);
					}
				}
			}
			if (NCombatUi._isDebugSlowRewards)
			{
				await Cmd.Wait(num + 3f, this._cts.Token, false);
			}
			else if (room.RoomType != RoomType.Boss)
			{
				await Cmd.CustomScaledWait(0.5f, num + 1f, false, this._cts.Token);
			}
			else
			{
				await Cmd.CustomScaledWait(num * 0.5f, num + 1f, false, this._cts.Token);
			}
			await room.OfferRoomEndRewards();
		}

		/// <summary>
		/// Animates combat UI elements into the scene visually.
		/// </summary>
		// Token: 0x06003E8F RID: 16015 RVA: 0x001DE3CB File Offset: 0x001DC5CB
		private void AnimIn()
		{
			this.Hand.AnimIn();
			this._energyCounter.AnimIn();
			this._combatPilesContainer.AnimIn();
		}

		/// <summary>
		/// Animates the combat UI elements out of the scene visually.
		/// </summary>
		// Token: 0x06003E90 RID: 16016 RVA: 0x001DE3F0 File Offset: 0x001DC5F0
		public void AnimOut()
		{
			this.Hand.AnimOut();
			this.PlayQueue.AnimOut();
			this.EndTurnButton.OnCombatEnded();
			this.PingButton.OnCombatEnded();
			this._energyCounter.AnimOut();
			this._combatPilesContainer.AnimOut();
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x001DE440 File Offset: 0x001DC640
		private void PostCombatCleanUp()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this.PlayContainer, "modulate", Colors.Transparent, 0.25);
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x001DE47E File Offset: 0x001DC67E
		public void OnHandSelectModeEntered()
		{
			this._originalHandChildIndex = this.Hand.GetIndex(false);
			this.Hand.MoveToFrontSafely();
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x001DE4A7 File Offset: 0x001DC6A7
		public void OnHandSelectModeExited()
		{
			this.MoveChildSafely(this.Hand, this._originalHandChildIndex);
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x001DE4C5 File Offset: 0x001DC6C5
		public void OnPeekButtonReady(NPeekButton peekButton)
		{
			peekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(new Action<NPeekButton>(this.OnPeekButtonToggled)), 0U);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x001DE4E8 File Offset: 0x001DC6E8
		private void OnPeekButtonToggled(NPeekButton peekButton)
		{
			if (this._playContainerPeekModeTween != null)
			{
				this._playContainerPeekModeTween.Pause();
				this._playContainerPeekModeTween.CustomStep(0.25);
				this._playContainerPeekModeTween.Kill();
				this._playContainerPeekModeTween = null;
			}
			Vector2 size = base.GetViewportRect().Size;
			if (peekButton.IsPeeking)
			{
				this.PlayQueue.Hide();
				using (IEnumerator<NCard> enumerator = this.PlayContainerCards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NCard ncard = enumerator.Current;
						this._originalPlayContainerCardPositions[ncard] = ncard.Position;
						this._originalPlayContainerCardScales[ncard] = ncard.Scale;
						Vector2 globalPosition = peekButton.CurrentCardMarker.GlobalPosition;
						Vector2 vector = ncard.Scale * 0.5f;
						if (this._playContainerPeekModeTween == null)
						{
							this._playContainerPeekModeTween = base.CreateTween();
						}
						this._playContainerPeekModeTween.TweenProperty(ncard, "global_position", globalPosition, 0.25).SetEase(1L).SetTrans(7L);
						this._playContainerPeekModeTween.Parallel().TweenProperty(ncard, "scale", vector, 0.25).SetEase(1L)
							.SetTrans(7L);
					}
					goto IL_0270;
				}
			}
			this.PlayQueue.Show();
			foreach (NCard ncard2 in this.PlayContainerCards)
			{
				Vector2 vector3;
				Vector2 vector2 = (this._originalPlayContainerCardPositions.TryGetValue(ncard2, out vector3) ? vector3 : (size * 0.5f));
				Vector2 vector5;
				Vector2 vector4 = (this._originalPlayContainerCardScales.TryGetValue(ncard2, out vector5) ? vector5 : Vector2.One);
				if (this._playContainerPeekModeTween == null)
				{
					this._playContainerPeekModeTween = base.CreateTween();
				}
				this._playContainerPeekModeTween.TweenProperty(ncard2, "position", vector2, 0.25).SetEase(1L).SetTrans(7L);
				this._playContainerPeekModeTween.Parallel().TweenProperty(ncard2, "scale", vector4, 0.25).SetEase(1L)
					.SetTrans(7L);
			}
			this._originalPlayContainerCardPositions.Clear();
			this._originalPlayContainerCardScales.Clear();
			IL_0270:
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001DE78C File Offset: 0x001DC98C
		public void Enable()
		{
			NPlayerHand hand = this.Hand;
			if (hand != null && hand.IsInCardSelection)
			{
				NPeekButton peekButton = hand.PeekButton;
				if (peekButton != null && !peekButton.IsPeeking)
				{
					this._combatPilesContainer.Disable();
					goto IL_003C;
				}
			}
			this._combatPilesContainer.Enable();
			IL_003C:
			if (this._state.CurrentSide == CombatSide.Player)
			{
				this.EndTurnButton.RefreshEnabled();
			}
			if (this._state.PlayerCreatures.Count > 1)
			{
				this.PingButton.RefreshEnabled();
			}
			NPlayerHand.Mode currentMode = this.Hand.CurrentMode;
			bool flag = currentMode - NPlayerHand.Mode.SimpleSelect <= 1;
			if (flag)
			{
				this.Hand.PeekButton.Enable();
			}
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x001DE837 File Offset: 0x001DCA37
		public void Disable()
		{
			this._combatPilesContainer.Disable();
			this.EndTurnButton.RefreshEnabled();
			this.PingButton.RefreshEnabled();
			this.Hand.PeekButton.Disable();
			this.Hand.CancelAllCardPlay();
		}

		/// <summary>
		/// Toggles the visibility of the combat UI
		/// </summary>
		// Token: 0x06003E98 RID: 16024 RVA: 0x001DE878 File Offset: 0x001DCA78
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideIntents, false))
			{
				NCombatUi.IsDebugHidingIntent = !NCombatUi.IsDebugHidingIntent;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingIntent ? "Hide Intents" : "Show Intents"));
				Action debugToggleIntent = this.DebugToggleIntent;
				if (debugToggleIntent != null)
				{
					debugToggleIntent();
				}
			}
			if (inputEvent.IsActionReleased(DebugHotkey.hideCombatUi, false))
			{
				NCombatUi._isDebugHidden = !NCombatUi._isDebugHidden;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi._isDebugHidden ? "Hide Combat UI" : "Show Combat UI"));
				this.DebugHideCombatUi();
				return;
			}
			if (inputEvent.IsActionReleased(DebugHotkey.hidePlayContainer, false))
			{
				NCombatUi.IsDebugHidingPlayContainer = !NCombatUi.IsDebugHidingPlayContainer;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingPlayContainer ? "Hide Played Card" : "Show Played Card"));
				this.DebugHideCombatUi();
				return;
			}
			if (inputEvent.IsActionReleased(DebugHotkey.hideHand, false))
			{
				NCombatUi._isDebugHidingHand = !NCombatUi._isDebugHidingHand;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi._isDebugHidingHand ? "Hide Hand Cards" : "Show Hand Cards"));
				this.DebugHideCombatUi();
				return;
			}
			if (inputEvent.IsActionReleased(DebugHotkey.hideHpBars, false))
			{
				NCombatUi.IsDebugHidingHpBar = !NCombatUi.IsDebugHidingHpBar;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingHpBar ? "Hide HP Bars" : "Show HP Bars"));
				Action debugToggleHpBar = this.DebugToggleHpBar;
				if (debugToggleHpBar == null)
				{
					return;
				}
				debugToggleHpBar();
				return;
			}
			else
			{
				if (inputEvent.IsActionReleased(DebugHotkey.hideTextVfx, false))
				{
					NCombatUi.IsDebugHideTextVfx = !NCombatUi.IsDebugHideTextVfx;
					NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHideTextVfx ? "Hide Text Vfx" : "Show Text Vfx"));
					return;
				}
				if (inputEvent.IsActionReleased(DebugHotkey.hideTargetingUi, false))
				{
					NCombatUi.IsDebugHideTargetingUi = !NCombatUi.IsDebugHideTargetingUi;
					NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHideTargetingUi ? "Hide Targeting UI" : "Show Targeting UI"));
					return;
				}
				if (inputEvent.IsActionReleased(DebugHotkey.slowRewards, false))
				{
					NCombatUi._isDebugSlowRewards = !NCombatUi._isDebugSlowRewards;
					NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi._isDebugSlowRewards ? "Slow Rewards Screens" : "Normal Rewards Screen"));
					return;
				}
				if (inputEvent.IsActionReleased(DebugHotkey.hideMpTargeting, false))
				{
					NCombatUi.IsDebugHideMpTargetingUi = !NCombatUi.IsDebugHideMpTargetingUi;
					NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHideMpTargetingUi ? "Hide MP Targeting" : "Show MP Targeting"));
					return;
				}
				if (inputEvent.IsActionReleased(DebugHotkey.hideMpIntents, false))
				{
					NCombatUi.IsDebugHideMpIntents = !NCombatUi.IsDebugHideMpIntents;
					NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NCombatUi.IsDebugHideMpIntents ? "Hide MP Intents" : "Show MP Intents"));
				}
				return;
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x001DEB24 File Offset: 0x001DCD24
		private void DebugHideCombatUi()
		{
			foreach (Control control in base.GetChildren(false).OfType<Control>())
			{
				if (control == this.Hand)
				{
					control.Modulate = (NCombatUi._isDebugHidingHand ? Colors.Transparent : Colors.White);
				}
				else if (control.Name == NCombatUi.PropertyName.PlayContainer)
				{
					control.Modulate = (NCombatUi.IsDebugHidingPlayContainer ? Colors.Transparent : Colors.White);
				}
				else
				{
					control.Modulate = (NCombatUi._isDebugHidden ? Colors.Transparent : Colors.White);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E9A RID: 16026 RVA: 0x001DEBDC File Offset: 0x001DCDDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(16)
			{
				new MethodInfo(NCombatUi.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.Deactivate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.DisconnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.AddToPlayContainer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatUi.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.PostCombatCleanUp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.OnHandSelectModeEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.OnHandSelectModeExited, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.OnPeekButtonReady, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "peekButton", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatUi.MethodName.OnPeekButtonToggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "peekButton", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatUi.MethodName.Enable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatUi.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCombatUi.MethodName.DebugHideCombatUi, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E9B RID: 16027 RVA: 0x001DEFA4 File Offset: 0x001DD1A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatUi.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.Deactivate && args.Count == 0)
			{
				this.Deactivate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.DisconnectSignals && args.Count == 0)
			{
				this.DisconnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.AddToPlayContainer && args.Count == 1)
			{
				this.AddToPlayContainer(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.PostCombatCleanUp && args.Count == 0)
			{
				this.PostCombatCleanUp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.OnHandSelectModeEntered && args.Count == 0)
			{
				this.OnHandSelectModeEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.OnHandSelectModeExited && args.Count == 0)
			{
				this.OnHandSelectModeExited();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.OnPeekButtonReady && args.Count == 1)
			{
				this.OnPeekButtonReady(VariantUtils.ConvertTo<NPeekButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.OnPeekButtonToggled && args.Count == 1)
			{
				this.OnPeekButtonToggled(VariantUtils.ConvertTo<NPeekButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.Enable && args.Count == 0)
			{
				this.Enable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatUi.MethodName.DebugHideCombatUi && args.Count == 0)
			{
				this.DebugHideCombatUi();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003E9C RID: 16028 RVA: 0x001DF244 File Offset: 0x001DD444
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatUi.MethodName._Ready || (ref method) == NCombatUi.MethodName._ExitTree || (ref method) == NCombatUi.MethodName.Deactivate || (ref method) == NCombatUi.MethodName.DisconnectSignals || (ref method) == NCombatUi.MethodName.AddToPlayContainer || (ref method) == NCombatUi.MethodName.AnimIn || (ref method) == NCombatUi.MethodName.AnimOut || (ref method) == NCombatUi.MethodName.PostCombatCleanUp || (ref method) == NCombatUi.MethodName.OnHandSelectModeEntered || (ref method) == NCombatUi.MethodName.OnHandSelectModeExited || (ref method) == NCombatUi.MethodName.OnPeekButtonReady || (ref method) == NCombatUi.MethodName.OnPeekButtonToggled || (ref method) == NCombatUi.MethodName.Enable || (ref method) == NCombatUi.MethodName.Disable || (ref method) == NCombatUi.MethodName._Input || (ref method) == NCombatUi.MethodName.DebugHideCombatUi || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003E9D RID: 16029 RVA: 0x001DF348 File Offset: 0x001DD548
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatUi.PropertyName.EnergyCounterContainer)
			{
				this.EnergyCounterContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.EndTurnButton)
			{
				this.EndTurnButton = VariantUtils.ConvertTo<NEndTurnButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PingButton)
			{
				this.PingButton = VariantUtils.ConvertTo<NPingButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.Hand)
			{
				this.Hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PlayContainer)
			{
				this.PlayContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PlayQueue)
			{
				this.PlayQueue = VariantUtils.ConvertTo<NCardPlayQueue>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.CardPreviewContainer)
			{
				this.CardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.MessyCardPreviewContainer)
			{
				this.MessyCardPreviewContainer = VariantUtils.ConvertTo<NMessyCardPreviewContainer>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._starCounter)
			{
				this._starCounter = VariantUtils.ConvertTo<NStarCounter>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._energyCounter)
			{
				this._energyCounter = VariantUtils.ConvertTo<NEnergyCounter>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._combatPilesContainer)
			{
				this._combatPilesContainer = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._playContainerPeekModeTween)
			{
				this._playContainerPeekModeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._originalHandChildIndex)
			{
				this._originalHandChildIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003E9E RID: 16030 RVA: 0x001DF4BC File Offset: 0x001DD6BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatUi.PropertyName.EnergyCounterContainer)
			{
				Control control = this.EnergyCounterContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.EndTurnButton)
			{
				NEndTurnButton endTurnButton = this.EndTurnButton;
				value = VariantUtils.CreateFrom<NEndTurnButton>(ref endTurnButton);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PingButton)
			{
				NPingButton pingButton = this.PingButton;
				value = VariantUtils.CreateFrom<NPingButton>(ref pingButton);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.DrawPile)
			{
				NDrawPileButton drawPile = this.DrawPile;
				value = VariantUtils.CreateFrom<NDrawPileButton>(ref drawPile);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.DiscardPile)
			{
				NDiscardPileButton discardPile = this.DiscardPile;
				value = VariantUtils.CreateFrom<NDiscardPileButton>(ref discardPile);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.ExhaustPile)
			{
				NExhaustPileButton exhaustPile = this.ExhaustPile;
				value = VariantUtils.CreateFrom<NExhaustPileButton>(ref exhaustPile);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.Hand)
			{
				NPlayerHand hand = this.Hand;
				value = VariantUtils.CreateFrom<NPlayerHand>(ref hand);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PlayContainer)
			{
				Control control = this.PlayContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.PlayQueue)
			{
				NCardPlayQueue playQueue = this.PlayQueue;
				value = VariantUtils.CreateFrom<NCardPlayQueue>(ref playQueue);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.CardPreviewContainer)
			{
				Control control = this.CardPreviewContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName.MessyCardPreviewContainer)
			{
				NMessyCardPreviewContainer messyCardPreviewContainer = this.MessyCardPreviewContainer;
				value = VariantUtils.CreateFrom<NMessyCardPreviewContainer>(ref messyCardPreviewContainer);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._starCounter)
			{
				value = VariantUtils.CreateFrom<NStarCounter>(ref this._starCounter);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._energyCounter)
			{
				value = VariantUtils.CreateFrom<NEnergyCounter>(ref this._energyCounter);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._combatPilesContainer)
			{
				value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPilesContainer);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._playContainerPeekModeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._playContainerPeekModeTween);
				return true;
			}
			if ((ref name) == NCombatUi.PropertyName._originalHandChildIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._originalHandChildIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E9F RID: 16031 RVA: 0x001DF6F8 File Offset: 0x001DD8F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatUi.PropertyName.EnergyCounterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.EndTurnButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.PingButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName._starCounter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName._energyCounter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName._combatPilesContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.DrawPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.DiscardPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.ExhaustPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.Hand, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.PlayContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.PlayQueue, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.CardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName.MessyCardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatUi.PropertyName._playContainerPeekModeTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatUi.PropertyName._originalHandChildIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003EA0 RID: 16032 RVA: 0x001DF91C File Offset: 0x001DDB1C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName energyCounterContainer = NCombatUi.PropertyName.EnergyCounterContainer;
			Control control = this.EnergyCounterContainer;
			info.AddProperty(energyCounterContainer, Variant.From<Control>(ref control));
			StringName endTurnButton = NCombatUi.PropertyName.EndTurnButton;
			NEndTurnButton endTurnButton2 = this.EndTurnButton;
			info.AddProperty(endTurnButton, Variant.From<NEndTurnButton>(ref endTurnButton2));
			StringName pingButton = NCombatUi.PropertyName.PingButton;
			NPingButton pingButton2 = this.PingButton;
			info.AddProperty(pingButton, Variant.From<NPingButton>(ref pingButton2));
			StringName hand = NCombatUi.PropertyName.Hand;
			NPlayerHand hand2 = this.Hand;
			info.AddProperty(hand, Variant.From<NPlayerHand>(ref hand2));
			StringName playContainer = NCombatUi.PropertyName.PlayContainer;
			control = this.PlayContainer;
			info.AddProperty(playContainer, Variant.From<Control>(ref control));
			StringName playQueue = NCombatUi.PropertyName.PlayQueue;
			NCardPlayQueue playQueue2 = this.PlayQueue;
			info.AddProperty(playQueue, Variant.From<NCardPlayQueue>(ref playQueue2));
			StringName cardPreviewContainer = NCombatUi.PropertyName.CardPreviewContainer;
			control = this.CardPreviewContainer;
			info.AddProperty(cardPreviewContainer, Variant.From<Control>(ref control));
			StringName messyCardPreviewContainer = NCombatUi.PropertyName.MessyCardPreviewContainer;
			NMessyCardPreviewContainer messyCardPreviewContainer2 = this.MessyCardPreviewContainer;
			info.AddProperty(messyCardPreviewContainer, Variant.From<NMessyCardPreviewContainer>(ref messyCardPreviewContainer2));
			info.AddProperty(NCombatUi.PropertyName._starCounter, Variant.From<NStarCounter>(ref this._starCounter));
			info.AddProperty(NCombatUi.PropertyName._energyCounter, Variant.From<NEnergyCounter>(ref this._energyCounter));
			info.AddProperty(NCombatUi.PropertyName._combatPilesContainer, Variant.From<NCombatPilesContainer>(ref this._combatPilesContainer));
			info.AddProperty(NCombatUi.PropertyName._playContainerPeekModeTween, Variant.From<Tween>(ref this._playContainerPeekModeTween));
			info.AddProperty(NCombatUi.PropertyName._originalHandChildIndex, Variant.From<int>(ref this._originalHandChildIndex));
		}

		/// <inheritdoc />
		// Token: 0x06003EA1 RID: 16033 RVA: 0x001DFA68 File Offset: 0x001DDC68
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatUi.PropertyName.EnergyCounterContainer, ref variant))
			{
				this.EnergyCounterContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatUi.PropertyName.EndTurnButton, ref variant2))
			{
				this.EndTurnButton = variant2.As<NEndTurnButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatUi.PropertyName.PingButton, ref variant3))
			{
				this.PingButton = variant3.As<NPingButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatUi.PropertyName.Hand, ref variant4))
			{
				this.Hand = variant4.As<NPlayerHand>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatUi.PropertyName.PlayContainer, ref variant5))
			{
				this.PlayContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCombatUi.PropertyName.PlayQueue, ref variant6))
			{
				this.PlayQueue = variant6.As<NCardPlayQueue>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCombatUi.PropertyName.CardPreviewContainer, ref variant7))
			{
				this.CardPreviewContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCombatUi.PropertyName.MessyCardPreviewContainer, ref variant8))
			{
				this.MessyCardPreviewContainer = variant8.As<NMessyCardPreviewContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCombatUi.PropertyName._starCounter, ref variant9))
			{
				this._starCounter = variant9.As<NStarCounter>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCombatUi.PropertyName._energyCounter, ref variant10))
			{
				this._energyCounter = variant10.As<NEnergyCounter>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCombatUi.PropertyName._combatPilesContainer, ref variant11))
			{
				this._combatPilesContainer = variant11.As<NCombatPilesContainer>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCombatUi.PropertyName._playContainerPeekModeTween, ref variant12))
			{
				this._playContainerPeekModeTween = variant12.As<Tween>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCombatUi.PropertyName._originalHandChildIndex, ref variant13))
			{
				this._originalHandChildIndex = variant13.As<int>();
			}
		}

		// Token: 0x04001840 RID: 6208
		private NStarCounter _starCounter;

		// Token: 0x04001841 RID: 6209
		private NEnergyCounter _energyCounter;

		// Token: 0x04001842 RID: 6210
		private NCombatPilesContainer _combatPilesContainer;

		/// <summary>
		/// When switching into peek mode, we move the play container cards off to the side for better visibility.
		/// When switching back out, we use the positions in this dictionary to move them back to their original positions.
		/// </summary>
		// Token: 0x04001848 RID: 6216
		private readonly Dictionary<NCard, Vector2> _originalPlayContainerCardPositions = new Dictionary<NCard, Vector2>();

		/// <summary>
		/// When switching into peek mode, we shrink the play container cards for better visibility.
		/// When switching back out, we use the scales in this dictionary to move them back to their original sizes.
		/// </summary>
		// Token: 0x04001849 RID: 6217
		private readonly Dictionary<NCard, Vector2> _originalPlayContainerCardScales = new Dictionary<NCard, Vector2>();

		// Token: 0x0400184A RID: 6218
		[Nullable(2)]
		private Tween _playContainerPeekModeTween;

		// Token: 0x0400184B RID: 6219
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400184C RID: 6220
		private int _originalHandChildIndex;

		// Token: 0x0400184D RID: 6221
		private CombatState _state;

		// Token: 0x04001855 RID: 6229
		private static bool _isDebugSlowRewards;

		// Token: 0x04001856 RID: 6230
		private static bool _isDebugHidden;

		// Token: 0x04001857 RID: 6231
		private static bool _isDebugHidingHand;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B5 RID: 6325
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006083 RID: 24707
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006084 RID: 24708
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Deactivate' method.
			/// </summary>
			// Token: 0x04006085 RID: 24709
			public static readonly StringName Deactivate = "Deactivate";

			/// <summary>
			/// Cached name for the 'DisconnectSignals' method.
			/// </summary>
			// Token: 0x04006086 RID: 24710
			public static readonly StringName DisconnectSignals = "DisconnectSignals";

			/// <summary>
			/// Cached name for the 'AddToPlayContainer' method.
			/// </summary>
			// Token: 0x04006087 RID: 24711
			public static readonly StringName AddToPlayContainer = "AddToPlayContainer";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04006088 RID: 24712
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x04006089 RID: 24713
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'PostCombatCleanUp' method.
			/// </summary>
			// Token: 0x0400608A RID: 24714
			public static readonly StringName PostCombatCleanUp = "PostCombatCleanUp";

			/// <summary>
			/// Cached name for the 'OnHandSelectModeEntered' method.
			/// </summary>
			// Token: 0x0400608B RID: 24715
			public static readonly StringName OnHandSelectModeEntered = "OnHandSelectModeEntered";

			/// <summary>
			/// Cached name for the 'OnHandSelectModeExited' method.
			/// </summary>
			// Token: 0x0400608C RID: 24716
			public static readonly StringName OnHandSelectModeExited = "OnHandSelectModeExited";

			/// <summary>
			/// Cached name for the 'OnPeekButtonReady' method.
			/// </summary>
			// Token: 0x0400608D RID: 24717
			public static readonly StringName OnPeekButtonReady = "OnPeekButtonReady";

			/// <summary>
			/// Cached name for the 'OnPeekButtonToggled' method.
			/// </summary>
			// Token: 0x0400608E RID: 24718
			public static readonly StringName OnPeekButtonToggled = "OnPeekButtonToggled";

			/// <summary>
			/// Cached name for the 'Enable' method.
			/// </summary>
			// Token: 0x0400608F RID: 24719
			public static readonly StringName Enable = "Enable";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04006090 RID: 24720
			public static readonly StringName Disable = "Disable";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04006091 RID: 24721
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'DebugHideCombatUi' method.
			/// </summary>
			// Token: 0x04006092 RID: 24722
			public static readonly StringName DebugHideCombatUi = "DebugHideCombatUi";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B6 RID: 6326
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'EnergyCounterContainer' property.
			/// </summary>
			// Token: 0x04006093 RID: 24723
			public static readonly StringName EnergyCounterContainer = "EnergyCounterContainer";

			/// <summary>
			/// Cached name for the 'EndTurnButton' property.
			/// </summary>
			// Token: 0x04006094 RID: 24724
			public static readonly StringName EndTurnButton = "EndTurnButton";

			/// <summary>
			/// Cached name for the 'PingButton' property.
			/// </summary>
			// Token: 0x04006095 RID: 24725
			public static readonly StringName PingButton = "PingButton";

			/// <summary>
			/// Cached name for the 'DrawPile' property.
			/// </summary>
			// Token: 0x04006096 RID: 24726
			public static readonly StringName DrawPile = "DrawPile";

			/// <summary>
			/// Cached name for the 'DiscardPile' property.
			/// </summary>
			// Token: 0x04006097 RID: 24727
			public static readonly StringName DiscardPile = "DiscardPile";

			/// <summary>
			/// Cached name for the 'ExhaustPile' property.
			/// </summary>
			// Token: 0x04006098 RID: 24728
			public static readonly StringName ExhaustPile = "ExhaustPile";

			/// <summary>
			/// Cached name for the 'Hand' property.
			/// </summary>
			// Token: 0x04006099 RID: 24729
			public static readonly StringName Hand = "Hand";

			/// <summary>
			/// Cached name for the 'PlayContainer' property.
			/// </summary>
			// Token: 0x0400609A RID: 24730
			public static readonly StringName PlayContainer = "PlayContainer";

			/// <summary>
			/// Cached name for the 'PlayQueue' property.
			/// </summary>
			// Token: 0x0400609B RID: 24731
			public static readonly StringName PlayQueue = "PlayQueue";

			/// <summary>
			/// Cached name for the 'CardPreviewContainer' property.
			/// </summary>
			// Token: 0x0400609C RID: 24732
			public static readonly StringName CardPreviewContainer = "CardPreviewContainer";

			/// <summary>
			/// Cached name for the 'MessyCardPreviewContainer' property.
			/// </summary>
			// Token: 0x0400609D RID: 24733
			public static readonly StringName MessyCardPreviewContainer = "MessyCardPreviewContainer";

			/// <summary>
			/// Cached name for the '_starCounter' field.
			/// </summary>
			// Token: 0x0400609E RID: 24734
			public static readonly StringName _starCounter = "_starCounter";

			/// <summary>
			/// Cached name for the '_energyCounter' field.
			/// </summary>
			// Token: 0x0400609F RID: 24735
			public static readonly StringName _energyCounter = "_energyCounter";

			/// <summary>
			/// Cached name for the '_combatPilesContainer' field.
			/// </summary>
			// Token: 0x040060A0 RID: 24736
			public static readonly StringName _combatPilesContainer = "_combatPilesContainer";

			/// <summary>
			/// Cached name for the '_playContainerPeekModeTween' field.
			/// </summary>
			// Token: 0x040060A1 RID: 24737
			public static readonly StringName _playContainerPeekModeTween = "_playContainerPeekModeTween";

			/// <summary>
			/// Cached name for the '_originalHandChildIndex' field.
			/// </summary>
			// Token: 0x040060A2 RID: 24738
			public static readonly StringName _originalHandChildIndex = "_originalHandChildIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B7 RID: 6327
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
