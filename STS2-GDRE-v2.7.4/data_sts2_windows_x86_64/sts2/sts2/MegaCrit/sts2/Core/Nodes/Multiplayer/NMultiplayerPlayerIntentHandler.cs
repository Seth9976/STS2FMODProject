using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// Handles the following:
	///  - Displaying "intents" for remote players when they hover over potions, cards, relics, and powers
	///  - Pulling cards that are executing player choice from the play pile and displaying them above the player's head
	///    with some thinky dots, then returning those cards to the play queue
	///  - Displaying relics, potions, and powers that are executing player choice and displaying them above the player's head
	///    with thinky dots
	///
	/// Note that none of these are executed in singleplayer.
	/// For the local player in multiplayer, the first responsibility is excluded (we know what we're hovering). However,
	/// the last two points still apply.
	/// </summary>
	// Token: 0x0200038F RID: 911
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerIntentHandler.cs")]
	public class NMultiplayerPlayerIntentHandler : Control
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003631 RID: 13873 RVA: 0x0019488C File Offset: 0x00192A8C
		public NMultiplayerCardIntent CardIntent
		{
			get
			{
				return this._cardIntent;
			}
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x00194894 File Offset: 0x00192A94
		[return: Nullable(2)]
		public static NMultiplayerPlayerIntentHandler Create(Player player)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				return null;
			}
			NMultiplayerPlayerIntentHandler nmultiplayerPlayerIntentHandler = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("combat/multiplayer_player_intent")).Instantiate<NMultiplayerPlayerIntentHandler>(0L);
			nmultiplayerPlayerIntentHandler._player = player;
			return nmultiplayerPlayerIntentHandler;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x001948DC File Offset: 0x00192ADC
		public override void _Ready()
		{
			this._cardIntent = base.GetNode<NMultiplayerCardIntent>("%CardIntent");
			this._relicIntent = base.GetNode<NRelic>("%RelicIntent");
			this._potionIntent = base.GetNode<NPotion>("%PotionIntent");
			this._powerIntent = base.GetNode<NPower>("%PowerIntent");
			this._hitbox = base.GetNode<Control>("%Hitbox");
			this._targetingIndicator = base.GetNode<NRemoteTargetingIndicator>("%TargetingIndicator");
			this._cardThinkyDots = this._cardIntent.GetNode<MegaRichTextLabel>("ThinkyDots");
			this._relicThinkyDots = this._relicIntent.GetNode<MegaRichTextLabel>("ThinkyDots");
			this._potionThinkyDots = this._potionIntent.GetNode<MegaRichTextLabel>("ThinkyDots");
			this._powerThinkyDots = this._powerIntent.GetNode<MegaRichTextLabel>("ThinkyDots");
			this._targetingIndicator.Initialize(this._player);
			this._cardIntent.Visible = false;
			this._relicIntent.Visible = false;
			this._potionIntent.Visible = false;
			this._powerIntent.Visible = false;
			this.HideThinkyDots();
			RunManager.Instance.ActionQueueSet.ActionEnqueued += this.OnActionEnqueued;
			if (!LocalContext.IsMe(this._player))
			{
				this._hitbox.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnHitboxEntered)), 0U);
				this._hitbox.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnHitboxExited)), 0U);
				this._hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHitboxEntered)), 0U);
				this._hitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnHitboxExited)), 0U);
				RunManager.Instance.HoveredModelTracker.HoverChanged += this.OnHoverChanged;
				RunManager.Instance.InputSynchronizer.StateChanged += this.OnPeerInputStateChanged;
				RunManager.Instance.InputSynchronizer.StateRemoved += this.OnPeerInputStateRemoved;
			}
			base.ProcessMode = 4L;
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x00194B30 File Offset: 0x00192D30
		public override void _ExitTree()
		{
			RunManager.Instance.ActionQueueSet.ActionEnqueued -= this.OnActionEnqueued;
			if (!LocalContext.IsMe(this._player))
			{
				RunManager.Instance.HoveredModelTracker.HoverChanged -= this.OnHoverChanged;
				RunManager.Instance.InputSynchronizer.StateChanged -= this.OnPeerInputStateChanged;
				RunManager.Instance.InputSynchronizer.StateRemoved -= this.OnPeerInputStateRemoved;
			}
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x00194BB6 File Offset: 0x00192DB6
		private void OnHitboxEntered()
		{
			this._shouldShowHoverTip = true;
			this.RefreshHoverTips();
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x00194BC5 File Offset: 0x00192DC5
		private void OnHitboxExited()
		{
			this._shouldShowHoverTip = false;
			this.RefreshHoverTips();
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x00194BD4 File Offset: 0x00192DD4
		private void OnHoverChanged(ulong playerId)
		{
			if (this._player.NetId != playerId)
			{
				return;
			}
			this.RefreshHoverDisplay();
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x00194BEC File Offset: 0x00192DEC
		private void RefreshHoverDisplay()
		{
			if (this._isInPlayerChoice)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this.HideThinkyDots();
			AbstractModel abstractModel = RunManager.Instance.HoveredModelTracker.GetHoveredModel(this._player.NetId);
			if (NCombatUi.IsDebugHideMpIntents)
			{
				abstractModel = null;
			}
			if (this._displayedModel == abstractModel)
			{
				return;
			}
			base.Modulate = StsColors.halfTransparentWhite;
			this._cardIntent.Visible = false;
			this._relicIntent.Visible = false;
			this._potionIntent.Visible = false;
			this._powerIntent.Visible = false;
			this._hitbox.Visible = abstractModel != null;
			if (abstractModel != null)
			{
				CardModel cardModel = abstractModel as CardModel;
				if (cardModel == null)
				{
					PotionModel potionModel = abstractModel as PotionModel;
					if (potionModel == null)
					{
						RelicModel relicModel = abstractModel as RelicModel;
						if (relicModel == null)
						{
							PowerModel powerModel = abstractModel as PowerModel;
							if (powerModel == null)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Player ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
								defaultInterpolatedStringHandler.AppendLiteral(" hovering unsupported model ");
								defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(abstractModel);
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							this._powerIntent.Visible = true;
							this._powerIntent.Model = powerModel;
							this._hitbox.Position = this._powerIntent.Position;
							this._hitbox.Size = this._powerIntent.Size;
						}
						else
						{
							this._relicIntent.Visible = true;
							this._relicIntent.Model = relicModel;
							this._hitbox.Position = this._relicIntent.Position;
							this._hitbox.Size = this._relicIntent.Size;
						}
					}
					else
					{
						this._potionIntent.Visible = true;
						this._potionIntent.Model = potionModel;
						this._hitbox.Position = this._potionIntent.Position;
						this._hitbox.Size = this._potionIntent.Size;
					}
				}
				else
				{
					this._cardIntent.Visible = true;
					this._cardIntent.Card = cardModel;
					this._hitbox.Position = this._cardIntent.Position;
					this._hitbox.Size = this._cardIntent.Size;
				}
			}
			this.RefreshHoverTips();
			this._displayedModel = abstractModel;
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x00194E44 File Offset: 0x00193044
		private void OnPeerInputStateChanged(ulong playerId)
		{
			if (playerId != this._player.NetId)
			{
				return;
			}
			bool isTargeting = RunManager.Instance.InputSynchronizer.GetIsTargeting(this._player.NetId);
			if (isTargeting && !this._targetingIndicator.Visible)
			{
				this._targetingIndicator.StartDrawingFrom(Vector2.Zero);
				base.ProcessMode = 0L;
				return;
			}
			if (!isTargeting && this._targetingIndicator.Visible)
			{
				this._targetingIndicator.StopDrawing();
				base.ProcessMode = 4L;
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x00194EC7 File Offset: 0x001930C7
		private void OnPeerInputStateRemoved(ulong playerId)
		{
			if (playerId != this._player.NetId)
			{
				return;
			}
			if (this._targetingIndicator.Visible)
			{
				this._targetingIndicator.StopDrawing();
				base.ProcessMode = 4L;
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00194EF8 File Offset: 0x001930F8
		public override void _Process(double delta)
		{
			Vector2 cursorPosition = NGame.Instance.RemoteCursorContainer.GetCursorPosition(this._player.NetId);
			this._targetingIndicator.UpdateDrawingTo(cursorPosition - this._targetingIndicator.GlobalPosition);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00194F3C File Offset: 0x0019313C
		private void OnActionEnqueued(GameAction action)
		{
			if (action.OwnerId != this._player.NetId)
			{
				return;
			}
			action.BeforeExecuted += this.BeforeActionExecuted;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x00194F64 File Offset: 0x00193164
		private void BeforeActionExecuted(GameAction action)
		{
			action.BeforeExecuted -= this.BeforeActionExecuted;
			action.BeforePausedForPlayerChoice += this.BeforeActionPausedForPlayerChoice;
			action.BeforeReadyToResumeAfterPlayerChoice += this.BeforeActionReadyToResumeAfterPlayerChoice;
			action.AfterFinished += this.UnsubscribeFromAction;
			action.BeforeCancelled += this.UnsubscribeFromAction;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x00194FCC File Offset: 0x001931CC
		private void UnsubscribeFromAction(GameAction action)
		{
			action.BeforePausedForPlayerChoice -= this.BeforeActionPausedForPlayerChoice;
			action.BeforeReadyToResumeAfterPlayerChoice -= this.BeforeActionReadyToResumeAfterPlayerChoice;
			action.AfterFinished -= this.UnsubscribeFromAction;
			action.BeforeCancelled -= this.UnsubscribeFromAction;
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00195024 File Offset: 0x00193224
		private void BeforeActionPausedForPlayerChoice(GameAction action)
		{
			AbstractModel abstractModel = null;
			PlayCardAction playCardAction = action as PlayCardAction;
			if (playCardAction != null)
			{
				PlayerChoiceContext playerChoiceContext = playCardAction.PlayerChoiceContext;
				abstractModel = ((playerChoiceContext != null) ? playerChoiceContext.LastInvolvedModel : null);
			}
			else
			{
				UsePotionAction usePotionAction = action as UsePotionAction;
				if (usePotionAction != null)
				{
					PlayerChoiceContext playerChoiceContext2 = usePotionAction.PlayerChoiceContext;
					abstractModel = ((playerChoiceContext2 != null) ? playerChoiceContext2.LastInvolvedModel : null);
				}
				else
				{
					GenericHookGameAction genericHookGameAction = action as GenericHookGameAction;
					if (genericHookGameAction != null)
					{
						HookPlayerChoiceContext choiceContext = genericHookGameAction.ChoiceContext;
						abstractModel = ((choiceContext != null) ? choiceContext.LastInvolvedModel : null);
					}
				}
			}
			if (abstractModel == null)
			{
				return;
			}
			this._isInPlayerChoice = true;
			this._cardIntent.Visible = false;
			this._relicIntent.Visible = false;
			this._potionIntent.Visible = false;
			this._powerIntent.Visible = false;
			this._hitbox.Visible = false;
			this._cardInPlayAwaitingPlayerChoice = null;
			CardModel cardModel = abstractModel as CardModel;
			if (cardModel != null)
			{
				NCard ncard = NCard.FindOnTable(cardModel, null);
				this._cardThinkyDots.Visible = true;
				this._cardThinkyDots.ProcessMode = 3L;
				if (ncard != null)
				{
					Tween playPileTween = ncard.PlayPileTween;
					if (playPileTween != null)
					{
						playPileTween.FastForwardToCompletion();
					}
					Tween tween = ncard.CreateTween();
					tween.Parallel().TweenProperty(ncard, "position", this._cardIntent.GlobalPosition + this._cardIntent.Size / 2f, (double)((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.2f : 0.3f));
					tween.Parallel().TweenProperty(ncard, "scale", Vector2.One * 0.25f, (double)((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.2f : 0.3f));
					this._cardInPlayAwaitingPlayerChoice = ncard;
					this._cardThinkyDots.Reparent(ncard.GetParent(), true);
					this._hitbox.Reparent(ncard.GetParent(), true);
				}
				else
				{
					this._cardIntent.Card = cardModel;
					this._cardIntent.Visible = true;
				}
				this._hitbox.Visible = true;
				this._hitbox.GlobalPosition = this._cardIntent.GlobalPosition;
				this._hitbox.Size = this._cardIntent.Size;
			}
			else
			{
				RelicModel relicModel = abstractModel as RelicModel;
				if (relicModel != null)
				{
					this._relicIntent.Model = relicModel;
					this._relicIntent.Visible = true;
					this._relicThinkyDots.Visible = true;
					this._relicThinkyDots.ProcessMode = 3L;
					this._hitbox.Visible = true;
					this._hitbox.Position = this._relicIntent.Position;
					this._hitbox.Size = this._relicIntent.Size;
				}
				else
				{
					PotionModel potionModel = abstractModel as PotionModel;
					if (potionModel != null)
					{
						this._potionIntent.Model = potionModel;
						this._potionIntent.Visible = true;
						this._potionThinkyDots.Visible = true;
						this._potionThinkyDots.ProcessMode = 3L;
						this._hitbox.Visible = true;
						this._hitbox.Position = this._potionIntent.Position;
						this._hitbox.Size = this._potionIntent.Size;
					}
					else
					{
						PowerModel powerModel = abstractModel as PowerModel;
						if (powerModel != null)
						{
							this._powerIntent.Model = powerModel;
							this._powerIntent.Visible = true;
							this._powerThinkyDots.Visible = true;
							this._powerThinkyDots.ProcessMode = 3L;
							this._hitbox.Visible = true;
							this._hitbox.Position = this._powerIntent.Position;
							this._hitbox.Size = this._powerIntent.Size;
						}
					}
				}
			}
			this.RefreshHoverTips();
			base.Modulate = StsColors.transparentWhite;
			Tween tween2 = this._tween;
			if (tween2 != null)
			{
				tween2.Kill();
			}
			this._tween = base.GetTree().CreateTween();
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.25);
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00195434 File Offset: 0x00193634
		private void BeforeActionReadyToResumeAfterPlayerChoice(GameAction action)
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.GetTree().CreateTween();
			this._tween.TweenProperty(this, "modulate", StsColors.transparentWhite, 0.15000000596046448);
			this._tween.TweenCallback(Callable.From(new Action(this.HideThinkyDots)));
			this._isInPlayerChoice = false;
			if (this._cardInPlayAwaitingPlayerChoice != null)
			{
				this._cardThinkyDots.Reparent(this._cardIntent, true);
				this._hitbox.Reparent(this, true);
				NCardPlayQueue.Instance.ReAddCardAfterPlayerChoice(this._cardInPlayAwaitingPlayerChoice, action);
				this._cardInPlayAwaitingPlayerChoice = null;
				this.RefreshHoverTips();
			}
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x001954F8 File Offset: 0x001936F8
		private void HideThinkyDots()
		{
			this._cardThinkyDots.Visible = false;
			this._relicThinkyDots.Visible = false;
			this._potionThinkyDots.Visible = false;
			this._powerThinkyDots.Visible = false;
			this._cardThinkyDots.ProcessMode = 4L;
			this._relicThinkyDots.ProcessMode = 4L;
			this._potionThinkyDots.ProcessMode = 4L;
			this._powerThinkyDots.ProcessMode = 4L;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x0019556C File Offset: 0x0019376C
		private void RefreshHoverTips()
		{
			if (LocalContext.IsMe(this._player))
			{
				return;
			}
			if (NCombatUi.IsDebugHideTargetingUi)
			{
				this._shouldShowHoverTip = false;
			}
			else if (!this._hitbox.Visible)
			{
				this._shouldShowHoverTip = false;
			}
			NHoverTipSet.Remove(this);
			if (!this._shouldShowHoverTip)
			{
				return;
			}
			List<IHoverTip> list = new List<IHoverTip>();
			if (this._cardInPlayAwaitingPlayerChoice != null)
			{
				list.Add(HoverTipFactory.FromCard(this._cardInPlayAwaitingPlayerChoice.Model, false));
				list.AddRange(this._cardInPlayAwaitingPlayerChoice.Model.HoverTips);
			}
			else if (this._cardIntent.Visible)
			{
				list.Add(HoverTipFactory.FromCard(this._cardIntent.Card, false));
				list.AddRange(this._cardIntent.Card.HoverTips);
			}
			else if (this._relicIntent.Visible)
			{
				list.AddRange(this._relicIntent.Model.HoverTips);
			}
			else if (this._potionIntent.Visible)
			{
				list.Add(HoverTipFactory.FromPotion(this._potionIntent.Model));
				list.AddRange(this._potionIntent.Model.HoverTips);
			}
			if (list.Count > 0)
			{
				NHoverTipSet.CreateAndShow(this, list, HoverTipAlignment.Right);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003643 RID: 13891 RVA: 0x001956A8 File Offset: 0x001938A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003644 RID: 13892 RVA: 0x00195958 File Offset: 0x00193B58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered && args.Count == 0)
			{
				this.OnHitboxEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited && args.Count == 0)
			{
				this.OnHitboxExited();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged && args.Count == 1)
			{
				this.OnHoverChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay && args.Count == 0)
			{
				this.RefreshHoverDisplay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged && args.Count == 1)
			{
				this.OnPeerInputStateChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved && args.Count == 1)
			{
				this.OnPeerInputStateRemoved(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots && args.Count == 0)
			{
				this.HideThinkyDots();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips && args.Count == 0)
			{
				this.RefreshHoverTips();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003645 RID: 13893 RVA: 0x00195B40 File Offset: 0x00193D40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerPlayerIntentHandler.MethodName._Ready || (ref method) == NMultiplayerPlayerIntentHandler.MethodName._ExitTree || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved || (ref method) == NMultiplayerPlayerIntentHandler.MethodName._Process || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots || (ref method) == NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003646 RID: 13894 RVA: 0x00195BFC File Offset: 0x00193DFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardIntent)
			{
				this._cardIntent = VariantUtils.ConvertTo<NMultiplayerCardIntent>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._relicIntent)
			{
				this._relicIntent = VariantUtils.ConvertTo<NRelic>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._potionIntent)
			{
				this._potionIntent = VariantUtils.ConvertTo<NPotion>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._powerIntent)
			{
				this._powerIntent = VariantUtils.ConvertTo<NPower>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._hitbox)
			{
				this._hitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator)
			{
				this._targetingIndicator = VariantUtils.ConvertTo<NRemoteTargetingIndicator>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots)
			{
				this._cardThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots)
			{
				this._relicThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots)
			{
				this._potionThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots)
			{
				this._powerThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip)
			{
				this._shouldShowHoverTip = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._hoverTips)
			{
				this._hoverTips = VariantUtils.ConvertTo<NHoverTipSet>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice)
			{
				this._isInPlayerChoice = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice)
			{
				this._cardInPlayAwaitingPlayerChoice = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003647 RID: 13895 RVA: 0x00195DA8 File Offset: 0x00193FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName.CardIntent)
			{
				NMultiplayerCardIntent cardIntent = this.CardIntent;
				value = VariantUtils.CreateFrom<NMultiplayerCardIntent>(ref cardIntent);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardIntent)
			{
				value = VariantUtils.CreateFrom<NMultiplayerCardIntent>(ref this._cardIntent);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._relicIntent)
			{
				value = VariantUtils.CreateFrom<NRelic>(ref this._relicIntent);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._potionIntent)
			{
				value = VariantUtils.CreateFrom<NPotion>(ref this._potionIntent);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._powerIntent)
			{
				value = VariantUtils.CreateFrom<NPower>(ref this._powerIntent);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._hitbox)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hitbox);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator)
			{
				value = VariantUtils.CreateFrom<NRemoteTargetingIndicator>(ref this._targetingIndicator);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardThinkyDots);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicThinkyDots);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._potionThinkyDots);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._powerThinkyDots);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._shouldShowHoverTip);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._hoverTips)
			{
				value = VariantUtils.CreateFrom<NHoverTipSet>(ref this._hoverTips);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInPlayerChoice);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardInPlayAwaitingPlayerChoice);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerIntentHandler.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003648 RID: 13896 RVA: 0x00195FC0 File Offset: 0x001941C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerIntentHandler.PropertyName.CardIntent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003649 RID: 13897 RVA: 0x001961E4 File Offset: 0x001943E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, Variant.From<NMultiplayerCardIntent>(ref this._cardIntent));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, Variant.From<NRelic>(ref this._relicIntent));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, Variant.From<NPotion>(ref this._potionIntent));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, Variant.From<NPower>(ref this._powerIntent));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._hitbox, Variant.From<Control>(ref this._hitbox));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, Variant.From<NRemoteTargetingIndicator>(ref this._targetingIndicator));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, Variant.From<MegaRichTextLabel>(ref this._cardThinkyDots));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, Variant.From<MegaRichTextLabel>(ref this._relicThinkyDots));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, Variant.From<MegaRichTextLabel>(ref this._potionThinkyDots));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, Variant.From<MegaRichTextLabel>(ref this._powerThinkyDots));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, Variant.From<bool>(ref this._shouldShowHoverTip));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, Variant.From<NHoverTipSet>(ref this._hoverTips));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, Variant.From<bool>(ref this._isInPlayerChoice));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, Variant.From<NCard>(ref this._cardInPlayAwaitingPlayerChoice));
			info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600364A RID: 13898 RVA: 0x00196344 File Offset: 0x00194544
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, ref variant))
			{
				this._cardIntent = variant.As<NMultiplayerCardIntent>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, ref variant2))
			{
				this._relicIntent = variant2.As<NRelic>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, ref variant3))
			{
				this._potionIntent = variant3.As<NPotion>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, ref variant4))
			{
				this._powerIntent = variant4.As<NPower>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._hitbox, ref variant5))
			{
				this._hitbox = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, ref variant6))
			{
				this._targetingIndicator = variant6.As<NRemoteTargetingIndicator>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, ref variant7))
			{
				this._cardThinkyDots = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, ref variant8))
			{
				this._relicThinkyDots = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, ref variant9))
			{
				this._potionThinkyDots = variant9.As<MegaRichTextLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, ref variant10))
			{
				this._powerThinkyDots = variant10.As<MegaRichTextLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, ref variant11))
			{
				this._shouldShowHoverTip = variant11.As<bool>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, ref variant12))
			{
				this._hoverTips = variant12.As<NHoverTipSet>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, ref variant13))
			{
				this._isInPlayerChoice = variant13.As<bool>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, ref variant14))
			{
				this._cardInPlayAwaitingPlayerChoice = variant14.As<NCard>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._tween, ref variant15))
			{
				this._tween = variant15.As<Tween>();
			}
		}

		// Token: 0x040014F4 RID: 5364
		private const string _scenePath = "combat/multiplayer_player_intent";

		// Token: 0x040014F5 RID: 5365
		private NMultiplayerCardIntent _cardIntent;

		// Token: 0x040014F6 RID: 5366
		private NRelic _relicIntent;

		// Token: 0x040014F7 RID: 5367
		private NPotion _potionIntent;

		// Token: 0x040014F8 RID: 5368
		private NPower _powerIntent;

		// Token: 0x040014F9 RID: 5369
		private Control _hitbox;

		// Token: 0x040014FA RID: 5370
		private NRemoteTargetingIndicator _targetingIndicator;

		// Token: 0x040014FB RID: 5371
		private MegaRichTextLabel _cardThinkyDots;

		// Token: 0x040014FC RID: 5372
		private MegaRichTextLabel _relicThinkyDots;

		// Token: 0x040014FD RID: 5373
		private MegaRichTextLabel _potionThinkyDots;

		// Token: 0x040014FE RID: 5374
		private MegaRichTextLabel _powerThinkyDots;

		// Token: 0x040014FF RID: 5375
		private bool _shouldShowHoverTip;

		// Token: 0x04001500 RID: 5376
		private Player _player;

		// Token: 0x04001501 RID: 5377
		[Nullable(2)]
		private AbstractModel _displayedModel;

		// Token: 0x04001502 RID: 5378
		[Nullable(2)]
		private NHoverTipSet _hoverTips;

		// Token: 0x04001503 RID: 5379
		private bool _isInPlayerChoice;

		// Token: 0x04001504 RID: 5380
		[Nullable(2)]
		private NCard _cardInPlayAwaitingPlayerChoice;

		// Token: 0x04001505 RID: 5381
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001722 RID: 5922
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005A16 RID: 23062
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005A17 RID: 23063
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnHitboxEntered' method.
			/// </summary>
			// Token: 0x04005A18 RID: 23064
			public static readonly StringName OnHitboxEntered = "OnHitboxEntered";

			/// <summary>
			/// Cached name for the 'OnHitboxExited' method.
			/// </summary>
			// Token: 0x04005A19 RID: 23065
			public static readonly StringName OnHitboxExited = "OnHitboxExited";

			/// <summary>
			/// Cached name for the 'OnHoverChanged' method.
			/// </summary>
			// Token: 0x04005A1A RID: 23066
			public static readonly StringName OnHoverChanged = "OnHoverChanged";

			/// <summary>
			/// Cached name for the 'RefreshHoverDisplay' method.
			/// </summary>
			// Token: 0x04005A1B RID: 23067
			public static readonly StringName RefreshHoverDisplay = "RefreshHoverDisplay";

			/// <summary>
			/// Cached name for the 'OnPeerInputStateChanged' method.
			/// </summary>
			// Token: 0x04005A1C RID: 23068
			public static readonly StringName OnPeerInputStateChanged = "OnPeerInputStateChanged";

			/// <summary>
			/// Cached name for the 'OnPeerInputStateRemoved' method.
			/// </summary>
			// Token: 0x04005A1D RID: 23069
			public static readonly StringName OnPeerInputStateRemoved = "OnPeerInputStateRemoved";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005A1E RID: 23070
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'HideThinkyDots' method.
			/// </summary>
			// Token: 0x04005A1F RID: 23071
			public static readonly StringName HideThinkyDots = "HideThinkyDots";

			/// <summary>
			/// Cached name for the 'RefreshHoverTips' method.
			/// </summary>
			// Token: 0x04005A20 RID: 23072
			public static readonly StringName RefreshHoverTips = "RefreshHoverTips";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001723 RID: 5923
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CardIntent' property.
			/// </summary>
			// Token: 0x04005A21 RID: 23073
			public static readonly StringName CardIntent = "CardIntent";

			/// <summary>
			/// Cached name for the '_cardIntent' field.
			/// </summary>
			// Token: 0x04005A22 RID: 23074
			public static readonly StringName _cardIntent = "_cardIntent";

			/// <summary>
			/// Cached name for the '_relicIntent' field.
			/// </summary>
			// Token: 0x04005A23 RID: 23075
			public static readonly StringName _relicIntent = "_relicIntent";

			/// <summary>
			/// Cached name for the '_potionIntent' field.
			/// </summary>
			// Token: 0x04005A24 RID: 23076
			public static readonly StringName _potionIntent = "_potionIntent";

			/// <summary>
			/// Cached name for the '_powerIntent' field.
			/// </summary>
			// Token: 0x04005A25 RID: 23077
			public static readonly StringName _powerIntent = "_powerIntent";

			/// <summary>
			/// Cached name for the '_hitbox' field.
			/// </summary>
			// Token: 0x04005A26 RID: 23078
			public static readonly StringName _hitbox = "_hitbox";

			/// <summary>
			/// Cached name for the '_targetingIndicator' field.
			/// </summary>
			// Token: 0x04005A27 RID: 23079
			public static readonly StringName _targetingIndicator = "_targetingIndicator";

			/// <summary>
			/// Cached name for the '_cardThinkyDots' field.
			/// </summary>
			// Token: 0x04005A28 RID: 23080
			public static readonly StringName _cardThinkyDots = "_cardThinkyDots";

			/// <summary>
			/// Cached name for the '_relicThinkyDots' field.
			/// </summary>
			// Token: 0x04005A29 RID: 23081
			public static readonly StringName _relicThinkyDots = "_relicThinkyDots";

			/// <summary>
			/// Cached name for the '_potionThinkyDots' field.
			/// </summary>
			// Token: 0x04005A2A RID: 23082
			public static readonly StringName _potionThinkyDots = "_potionThinkyDots";

			/// <summary>
			/// Cached name for the '_powerThinkyDots' field.
			/// </summary>
			// Token: 0x04005A2B RID: 23083
			public static readonly StringName _powerThinkyDots = "_powerThinkyDots";

			/// <summary>
			/// Cached name for the '_shouldShowHoverTip' field.
			/// </summary>
			// Token: 0x04005A2C RID: 23084
			public static readonly StringName _shouldShowHoverTip = "_shouldShowHoverTip";

			/// <summary>
			/// Cached name for the '_hoverTips' field.
			/// </summary>
			// Token: 0x04005A2D RID: 23085
			public static readonly StringName _hoverTips = "_hoverTips";

			/// <summary>
			/// Cached name for the '_isInPlayerChoice' field.
			/// </summary>
			// Token: 0x04005A2E RID: 23086
			public static readonly StringName _isInPlayerChoice = "_isInPlayerChoice";

			/// <summary>
			/// Cached name for the '_cardInPlayAwaitingPlayerChoice' field.
			/// </summary>
			// Token: 0x04005A2F RID: 23087
			public static readonly StringName _cardInPlayAwaitingPlayerChoice = "_cardInPlayAwaitingPlayerChoice";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005A30 RID: 23088
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001724 RID: 5924
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
