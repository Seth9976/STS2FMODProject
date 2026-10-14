using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x02000390 RID: 912
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerState.cs")]
	public class NMultiplayerPlayerState : Control
	{
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x00196504 File Offset: 0x00194704
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NMultiplayerPlayerState._scenePath,
					NMultiplayerPlayerState._cardScenePath
				});
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600364D RID: 13901 RVA: 0x00196521 File Offset: 0x00194721
		// (set) Token: 0x0600364E RID: 13902 RVA: 0x00196529 File Offset: 0x00194729
		public NButton Hitbox { get; private set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600364F RID: 13903 RVA: 0x00196532 File Offset: 0x00194732
		// (set) Token: 0x06003650 RID: 13904 RVA: 0x0019653A File Offset: 0x0019473A
		public Player Player { get; private set; }

		// Token: 0x06003651 RID: 13905 RVA: 0x00196544 File Offset: 0x00194744
		public static NMultiplayerPlayerState Create(Player player)
		{
			NMultiplayerPlayerState nmultiplayerPlayerState = PreloadManager.Cache.GetScene(NMultiplayerPlayerState._scenePath).Instantiate<NMultiplayerPlayerState>(0L);
			nmultiplayerPlayerState.Player = player;
			return nmultiplayerPlayerState;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x00196570 File Offset: 0x00194770
		public override void _Ready()
		{
			this._nameplateLabel = base.GetNode<MegaLabel>("%NameplateLabel");
			this._healthBar = base.GetNode<NHealthBar>("%HealthBar");
			this._characterIcon = base.GetNode<TextureRect>("%CharacterIcon");
			this._turnEndIndicator = base.GetNode<TextureRect>("%TurnEndIndicator");
			this._disconnectedIndicator = base.GetNode<TextureRect>("%DisconnectedIndicator");
			this._networkProblemIndicator = base.GetNode<NMultiplayerNetworkProblemIndicator>("%NetworkProblemIndicator");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this.Hitbox = base.GetNode<NButton>("%Hitbox");
			this._locationIcon = base.GetNode<TextureRect>("%LocationIcon");
			this._locationContainer = base.GetNode<Control>("%LocationContainer");
			this._topContainer = base.GetNode<HBoxContainer>("TopInfoContainer");
			this._energyContainer = base.GetNode<Control>("%EnergyCountContainer");
			this._energyImage = this._energyContainer.GetNode<TextureRect>("Image");
			this._energyCount = this._energyContainer.GetNode<MegaLabel>("EnergyCount");
			this._starContainer = base.GetNode<Control>("%StarCountContainer");
			this._starCount = this._starContainer.GetNode<MegaLabel>("StarCount");
			this._cardContainer = base.GetNode<Control>("%CardCountContainer");
			this._cardImage = this._cardContainer.GetNode<NTinyCard>("TinyCard");
			this._cardCount = this._cardContainer.GetNode<MegaLabel>("CardCount");
			this._selectionReticle.Visible = true;
			this._characterIcon.Texture = this.Player.Character.IconTexture;
			this._nameplateLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(RunManager.Instance.NetService.Platform, this.Player.NetId));
			this._healthBar.SetCreature(this.Player.Creature);
			this._networkProblemIndicator.Initialize(this.Player.NetId);
			this._locationContainer.Visible = false;
			this._energyContainer.Visible = false;
			this._energyImage.Texture = ResourceLoader.Load<Texture2D>(this.Player.Character.CardPool.EnergyIconPath, null, 1L);
			this._starContainer.Visible = false;
			this._cardContainer.Visible = false;
			this._cardImage.Set(this.Player.Character.CardPool, CardType.Attack, CardRarity.Common);
			this._turnEndIndicator.Visible = false;
			this._healthBar.FadeOutHpLabel(0f, 0f);
			this.Player.Creature.BlockChanged += this.BlockChanged;
			this.Player.Creature.CurrentHpChanged += this.OnCreatureValueChanged;
			this.Player.Creature.MaxHpChanged += this.OnCreatureValueChanged;
			this.Player.Creature.PowerApplied += this.OnPowerAppliedOrRemoved;
			this.Player.Creature.PowerIncreased += this.OnPowerIncreased;
			this.Player.Creature.PowerDecreased += this.OnPowerDecreased;
			this.Player.Creature.PowerRemoved += this.OnPowerAppliedOrRemoved;
			this.Player.Creature.Died += this.OnCreatureChanged;
			this.Player.RelicObtained += this.OnRelicObtained;
			this.Player.RelicRemoved += this.OnRelicRemoved;
			this.Player.PotionProcured += this.OnPotionProcured;
			this.Player.PotionDiscarded += this.OnPotionDiscarded;
			this.Player.Deck.CardAdded += this.OnCardObtained;
			this.Player.Deck.CardRemoved += this.OnCardRemovedFromDeck;
			CombatManager.Instance.PlayerEndedTurn += this.RefreshPlayerReadyIndicator;
			CombatManager.Instance.PlayerUnendedTurn += this.RefreshPlayerReadyIndicator;
			CombatManager.Instance.TurnStarted += this.OnTurnStarted;
			CombatManager.Instance.CombatSetUp += this.OnCombatSetUp;
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
			RunManager.Instance.FlavorSynchronizer.OnEndTurnPingReceived += this.OnPlayerEndTurnPing;
			RunManager.Instance.InputSynchronizer.ScreenChanged += this.OnPlayerScreenChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged += this.OnPlayerVoteChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled += this.RefreshPlayerReadyIndicator;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVotesCleared += this.OnPlayerVotesCleared;
			if (RunManager.Instance.RunLobby != null)
			{
				RunManager.Instance.RunLobby.RemotePlayerDisconnected += this.RefreshConnectedState;
				RunManager.Instance.RunLobby.LocalPlayerDisconnected += this.RefreshConnectedState;
				RunManager.Instance.RunLobby.PlayerRejoined += this.RefreshConnectedState;
			}
			this.Hitbox.Connect(NClickableControl.SignalName.Focused, Callable.From<NButton>(new Action<NButton>(this.OnFocus)), 0U);
			this.Hitbox.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NButton>(new Action<NButton>(this.OnUnfocus)), 0U);
			this.Hitbox.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRelease)), 0U);
			this.RefreshValues();
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00196B80 File Offset: 0x00194D80
		public override void _ExitTree()
		{
			base._ExitTree();
			this.Player.Creature.BlockChanged -= this.BlockChanged;
			this.Player.Creature.CurrentHpChanged -= this.OnCreatureValueChanged;
			this.Player.Creature.MaxHpChanged -= this.OnCreatureValueChanged;
			this.Player.Creature.PowerApplied -= this.OnPowerAppliedOrRemoved;
			this.Player.Creature.PowerIncreased -= this.OnPowerIncreased;
			this.Player.Creature.PowerDecreased -= this.OnPowerDecreased;
			this.Player.Creature.PowerRemoved -= this.OnPowerAppliedOrRemoved;
			this.Player.Creature.Died -= this.OnCreatureChanged;
			this.Player.RelicObtained -= this.OnRelicObtained;
			this.Player.RelicRemoved -= this.OnRelicRemoved;
			this.Player.PotionProcured -= this.OnPotionProcured;
			this.Player.PotionDiscarded -= this.OnPotionDiscarded;
			this.Player.Deck.CardAdded -= this.OnCardObtained;
			this.Player.Deck.CardRemoved -= this.OnCardRemovedFromDeck;
			CombatManager.Instance.PlayerEndedTurn -= this.RefreshPlayerReadyIndicator;
			CombatManager.Instance.PlayerUnendedTurn -= this.RefreshPlayerReadyIndicator;
			CombatManager.Instance.TurnStarted -= this.OnTurnStarted;
			CombatManager.Instance.CombatSetUp -= this.OnCombatSetUp;
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
			RunManager.Instance.FlavorSynchronizer.OnEndTurnPingReceived -= this.OnPlayerEndTurnPing;
			RunManager.Instance.InputSynchronizer.ScreenChanged -= this.OnPlayerScreenChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged -= this.OnPlayerVoteChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled -= this.RefreshPlayerReadyIndicator;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVotesCleared -= this.OnPlayerVotesCleared;
			if (RunManager.Instance.RunLobby != null)
			{
				RunManager.Instance.RunLobby.RemotePlayerDisconnected -= this.RefreshConnectedState;
				RunManager.Instance.RunLobby.LocalPlayerDisconnected -= this.RefreshConnectedState;
				RunManager.Instance.RunLobby.PlayerRejoined -= this.RefreshConnectedState;
			}
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x00196E5C File Offset: 0x0019505C
		private void OnCombatSetUp(CombatState _)
		{
			if (!LocalContext.IsMe(this.Player))
			{
				this._energyContainer.Visible = true;
				CanvasItem starContainer = this._starContainer;
				bool flag;
				if (!(this.Player.Character is Regent))
				{
					PlayerCombatState playerCombatState = this.Player.PlayerCombatState;
					flag = playerCombatState != null && playerCombatState.Stars > 0;
				}
				else
				{
					flag = true;
				}
				starContainer.Visible = flag;
				this._cardContainer.Visible = true;
				this.Player.PlayerCombatState.EnergyChanged += this.OnEnergyChanged;
				this.Player.PlayerCombatState.StarsChanged += this.OnStarsChanged;
				this.Player.PlayerCombatState.Hand.CardAdded += this.OnCardAdded;
				this.Player.PlayerCombatState.Hand.CardRemoved += this.OnCardRemoved;
			}
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00196F48 File Offset: 0x00195148
		private void OnCombatEnded(CombatRoom _)
		{
			this._turnEndIndicator.Visible = false;
			if (!LocalContext.IsMe(this.Player))
			{
				this._energyContainer.Visible = false;
				this._starContainer.Visible = false;
				this._cardContainer.Visible = false;
				if (this.Player.PlayerCombatState != null)
				{
					this.Player.PlayerCombatState.EnergyChanged -= this.OnEnergyChanged;
					this.Player.PlayerCombatState.StarsChanged -= this.OnStarsChanged;
					this.Player.PlayerCombatState.Hand.CardAdded -= this.OnCardAdded;
					this.Player.PlayerCombatState.Hand.CardRemoved -= this.OnCardRemoved;
				}
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0019701C File Offset: 0x0019521C
		private void OnCreatureValueChanged(int _, int __)
		{
			this.RefreshValues();
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x00197024 File Offset: 0x00195224
		private void OnCreatureChanged(Creature _)
		{
			this.RefreshValues();
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x0019702C File Offset: 0x0019522C
		private void OnPowerAppliedOrRemoved(PowerModel _)
		{
			this.RefreshValues();
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x00197034 File Offset: 0x00195234
		private void OnPowerDecreased(PowerModel _, bool __)
		{
			this.RefreshValues();
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x0019703C File Offset: 0x0019523C
		private void OnPowerIncreased(PowerModel _, int __, bool ___)
		{
			this.RefreshValues();
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x00197044 File Offset: 0x00195244
		private void RefreshValues()
		{
			this.UpdateHealthBarWidth();
			this._healthBar.RefreshValues();
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x00197057 File Offset: 0x00195257
		private void UpdateHealthBarWidth()
		{
			this._healthBar.UpdateWidthRelativeToReferenceValue(80f, 175f);
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00197070 File Offset: 0x00195270
		private void UpdateSelectionReticleWidth()
		{
			Control control = null;
			foreach (Control control2 in this._topContainer.GetChildren(false).OfType<Control>())
			{
				if (control2.Visible && control2.Size.X > 0f)
				{
					control = control2;
				}
			}
			float num = control.GlobalPosition.X - base.GlobalPosition.X + control.Size.X + 6f;
			float num2 = this._healthBar.HpBarContainer.GlobalPosition.X - base.GlobalPosition.X + this._healthBar.HpBarContainer.Size.X + 6f;
			float num3 = Mathf.Max(num, num2);
			GodotObject selectionReticle = this._selectionReticle;
			StringName size = Control.PropertyName.Size;
			Vector2 size2 = this._selectionReticle.Size;
			size2.X = num3;
			selectionReticle.SetDeferred(size, size2);
			this.Hitbox.SetDeferred(Control.PropertyName.Size, new Vector2(num3, base.Size.Y));
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x001971AC File Offset: 0x001953AC
		private void OnEnergyChanged(int _, int __)
		{
			this.RefreshCombatValues();
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x001971B4 File Offset: 0x001953B4
		private void OnStarsChanged(int _, int __)
		{
			this.RefreshCombatValues();
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x001971BC File Offset: 0x001953BC
		private void OnCardAdded(CardModel _)
		{
			this.RefreshCombatValues();
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x001971C4 File Offset: 0x001953C4
		private void OnCardRemoved(CardModel _)
		{
			this.RefreshCombatValues();
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x001971CC File Offset: 0x001953CC
		private void RefreshCombatValues()
		{
			CanvasItem starContainer = this._starContainer;
			bool flag;
			if (!(this.Player.Character is Regent))
			{
				PlayerCombatState playerCombatState = this.Player.PlayerCombatState;
				flag = playerCombatState != null && playerCombatState.Stars > 0;
			}
			else
			{
				flag = true;
			}
			starContainer.Visible = flag;
			this._energyCount.SetTextAutoSize(this.Player.PlayerCombatState.Energy.ToString());
			this._starCount.SetTextAutoSize(this.Player.PlayerCombatState.Stars.ToString());
			this._cardCount.SetTextAutoSize(this.Player.PlayerCombatState.Hand.Cards.Count.ToString());
			this._energyCount.AddThemeColorOverride(ThemeConstants.Label.FontColor, (this.Player.PlayerCombatState.Energy == 0) ? StsColors.red : StsColors.cream);
			this._energyCount.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, (this.Player.PlayerCombatState.Energy == 0) ? StsColors.unplayableEnergyCostOutline : this.Player.Character.EnergyLabelOutlineColor);
			Material material = ((this.Player.PlayerCombatState.Energy == 0) ? PreloadManager.Cache.GetMaterial("res://materials/ui/energy_orb_dark.tres") : null);
			this._energyImage.Material = material;
			this._energyImage.Modulate = ((this.Player.PlayerCombatState.Energy == 0) ? Colors.DarkGray : Colors.White);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00197347 File Offset: 0x00195547
		public void OnCreatureHovered()
		{
			this._isCreatureHovered = true;
			this.UpdateHighlightedState();
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00197356 File Offset: 0x00195556
		public void OnCreatureUnhovered()
		{
			this._isCreatureHovered = false;
			this.UpdateHighlightedState();
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00197365 File Offset: 0x00195565
		public void FlashPlayerReady()
		{
			this.FlashEndTurn();
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00197370 File Offset: 0x00195570
		private void UpdateHighlightedState()
		{
			bool flag = this._isMouseOver || this._isCreatureHovered;
			if (NTargetManager.Instance.IsInSelection && !NTargetManager.Instance.AllowedToTargetNode(this))
			{
				flag = false;
			}
			if (!NTargetManager.Instance.IsInSelection)
			{
				NPlayerHand instance = NPlayerHand.Instance;
				if (instance != null && instance.InCardPlay)
				{
					flag = false;
				}
			}
			if (this._isHighlighted != flag)
			{
				this._isHighlighted = flag;
				if (this._isHighlighted)
				{
					this._healthBar.FadeInHpLabel(0.1f);
					this.UpdateSelectionReticleWidth();
					this._selectionReticle.OnSelect();
					if (this._networkProblemIndicator.IsShown)
					{
						LocString locString;
						LocString locString2;
						if (RunManager.Instance.NetService.Type == NetGameType.Client)
						{
							locString = new LocString("static_hover_tips", "NETWORK_PROBLEM_CLIENT.description");
							locString2 = new LocString("static_hover_tips", "NETWORK_PROBLEM_CLIENT.title");
						}
						else
						{
							locString = new LocString("static_hover_tips", "NETWORK_PROBLEM_HOST.description");
							locString2 = new LocString("static_hover_tips", "NETWORK_PROBLEM_HOST.title");
						}
						locString.Add("Player", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Player.NetId));
						NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(locString2, locString, null), HoverTipAlignment.None);
						if (nhoverTipSet != null)
						{
							nhoverTipSet.SetGlobalPosition(base.GlobalPosition + Vector2.Down * 80f, false);
							return;
						}
					}
				}
				else
				{
					this._healthBar.FadeOutHpLabel(0.5f, 0f);
					this._selectionReticle.OnDeselect();
					NHoverTipSet.Remove(this);
				}
			}
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x001974F8 File Offset: 0x001956F8
		private void BlockChanged(int oldBlock, int blockGain)
		{
			if (oldBlock == 0 && blockGain > 0)
			{
				this._healthBar.AnimateInBlock(oldBlock, blockGain);
			}
			this._healthBar.RefreshValues();
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00197519 File Offset: 0x00195719
		private void RefreshConnectedState(ulong _)
		{
			this.RefreshConnectedState();
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00197524 File Offset: 0x00195724
		private void RefreshConnectedState()
		{
			bool flag = RunManager.Instance.RunLobby.ConnectedPlayerIds.Contains(this.Player.NetId);
			this._disconnectedIndicator.Visible = !flag;
			this._characterIcon.SelfModulate = (flag ? Colors.White : StsColors.gray);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x0019757A File Offset: 0x0019577A
		private void OnRelicObtained(RelicModel relic)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimateRelicObtained(relic));
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00197598 File Offset: 0x00195798
		private async Task AnimateRelicObtained(RelicModel relic)
		{
			await this.WaitUntilNextTweenTime();
			NRelic relicImage = NRelic.Create(relic, NRelic.IconSize.Small);
			relicImage.Model = relic;
			await this.ObtainedAnimation(relicImage);
			relicImage.QueueFreeSafely();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x001975E3 File Offset: 0x001957E3
		private void OnRelicRemoved(RelicModel relic)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimateRelicRemoved(relic));
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00197600 File Offset: 0x00195800
		private async Task AnimateRelicRemoved(RelicModel relic)
		{
			await this.WaitUntilNextTweenTime();
			NRelic relicImage = NRelic.Create(relic, NRelic.IconSize.Small);
			relicImage.Model = relic;
			await this.RemovedAnimation(relicImage);
			relicImage.QueueFreeSafely();
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x0019764B File Offset: 0x0019584B
		private void OnCardObtained(CardModel card)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimateCardObtained(card));
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00197668 File Offset: 0x00195868
		private async Task AnimateCardObtained(CardModel card)
		{
			await this.WaitUntilNextTweenTime();
			NDeckHistoryEntry cardNode = NDeckHistoryEntry.Create(card, 1);
			await this.ObtainedAnimation(cardNode);
			cardNode.QueueFreeSafely();
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x001976B3 File Offset: 0x001958B3
		private void OnCardRemovedFromDeck(CardModel card)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimateCardRemovedFromDeck(card));
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x001976D0 File Offset: 0x001958D0
		private async Task AnimateCardRemovedFromDeck(CardModel card)
		{
			await this.WaitUntilNextTweenTime();
			NDeckHistoryEntry cardNode = NDeckHistoryEntry.Create(card, 1);
			await this.RemovedAnimation(cardNode);
			cardNode.QueueFreeSafely();
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x0019771B File Offset: 0x0019591B
		private void OnPotionProcured(PotionModel potion)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimatePotionObtained(potion));
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x00197738 File Offset: 0x00195938
		private async Task AnimatePotionObtained(PotionModel potion)
		{
			await this.WaitUntilNextTweenTime();
			NPotion node = NPotion.Create(potion);
			await this.ObtainedAnimation(node);
			node.QueueFreeSafely();
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x00197783 File Offset: 0x00195983
		private void OnPotionDiscarded(PotionModel potion)
		{
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			TaskHelper.RunSafely(this.AnimatePotionDiscarded(potion));
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x001977A0 File Offset: 0x001959A0
		private async Task AnimatePotionDiscarded(PotionModel potion)
		{
			await this.WaitUntilNextTweenTime();
			NPotion node = NPotion.Create(potion);
			await this.RemovedAnimation(node);
			node.QueueFreeSafely();
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x001977EB File Offset: 0x001959EB
		private void OnPlayerVoteChanged(Player player, MapVote? _, MapVote? __)
		{
			this.RefreshPlayerReadyIndicator(player);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x001977F4 File Offset: 0x001959F4
		private void OnPlayerVotesCleared()
		{
			this.RefreshPlayerReadyIndicator(this.Player);
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00197802 File Offset: 0x00195A02
		private void RefreshPlayerReadyIndicator(Player player, bool _)
		{
			this.RefreshPlayerReadyIndicator(player);
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x0019780C File Offset: 0x00195A0C
		private void RefreshPlayerReadyIndicator(Player player)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				this._turnEndIndicator.Visible = CombatManager.Instance.IsPlayerReadyToEndTurn(this.Player);
			}
			else
			{
				this._turnEndIndicator.Visible = RunManager.Instance.MapSelectionSynchronizer.GetVote(this.Player) != null;
			}
			if (this._turnEndIndicator.Visible && player == this.Player)
			{
				this.FlashEndTurn();
			}
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00197886 File Offset: 0x00195A86
		private void OnPlayerEndTurnPing(ulong playerId)
		{
			if (this.Player.NetId == playerId)
			{
				this.FlashEndTurn();
			}
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x0019789C File Offset: 0x00195A9C
		private void FlashEndTurn()
		{
			NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(this._turnEndIndicator.Texture, this._turnEndIndicator.SelfModulate);
			this._turnEndIndicator.AddChildSafely(nuiFlashVfx);
			nuiFlashVfx.SetDeferred(Control.PropertyName.Size, this._turnEndIndicator.Size);
			nuiFlashVfx.Position = Vector2.Zero;
			TaskHelper.RunSafely(nuiFlashVfx.StartVfx());
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x00197903 File Offset: 0x00195B03
		private void OnTurnStarted(CombatState _)
		{
			this._turnEndIndicator.Visible = CombatManager.Instance.IsPlayerReadyToEndTurn(this.Player);
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00197920 File Offset: 0x00195B20
		private void SetNextTweenTime()
		{
			ulong ticksMsec = Time.GetTicksMsec();
			if (this._nextTweenTime > ticksMsec)
			{
				this._nextTweenTime += 500UL;
				return;
			}
			this._nextTweenTime = ticksMsec + 500UL;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x00197960 File Offset: 0x00195B60
		private async Task WaitUntilNextTweenTime()
		{
			ulong nextTweenTime = this._nextTweenTime;
			this.SetNextTweenTime();
			if (nextTweenTime >= Time.GetTicksMsec())
			{
				double num = (nextTweenTime - Time.GetTicksMsec()) / 1000.0;
				await base.GetTree().CreateTimer(num, true, false, false).AwaitSignal(SceneTreeTimer.SignalName.Timeout, this);
			}
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x001979A4 File Offset: 0x00195BA4
		private async Task ObtainedAnimation(Control node)
		{
			this.AddChildSafely(node);
			node.Position = new Vector2(base.Size.X + 40f, 0f);
			node.Scale = Vector2.One * 1.1f;
			Tween tween = node.CreateTween();
			tween.TweenProperty(node, "scale", Vector2.One, 0.30000001192092896).SetEase(0L).SetTrans(9L);
			tween.TweenProperty(node, "position:x", base.Size.X - node.Size.X, 0.5).SetEase(1L).SetTrans(5L)
				.SetDelay(0.30000001192092896);
			tween.TweenProperty(node, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(5L)
				.SetDelay(0.30000001192092896);
			bool flag = await tween.AwaitFinished(this);
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x001979F0 File Offset: 0x00195BF0
		private async Task RemovedAnimation(Control node)
		{
			this.AddChildSafely(node);
			node.Position = new Vector2(base.Size.X - node.Size.X, 0f);
			Color modulate = node.Modulate;
			modulate.A = 0f;
			node.Modulate = modulate;
			Tween tween = node.CreateTween();
			tween.TweenProperty(node, "position:x", base.Size.X + 40f, 0.5).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(node, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(node, "scale:y", 0f, 0.30000001192092896).SetEase(1L).SetTrans(5L)
				.SetDelay(0.5);
			bool flag = await tween.AwaitFinished(this);
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00197A3C File Offset: 0x00195C3C
		private void OnPlayerScreenChanged(ulong playerId, NetScreenType _)
		{
			if (this.Player.NetId != playerId)
			{
				return;
			}
			if (LocalContext.IsMe(this.Player))
			{
				return;
			}
			Texture2D locationIcon = RunManager.Instance.InputSynchronizer.GetScreenType(playerId).GetLocationIcon();
			if (this._currentLocationIcon != locationIcon)
			{
				this._currentLocationIcon = locationIcon;
				if (locationIcon == null)
				{
					this.TweenLocationIconAway();
					return;
				}
				this.TweenLocationIconIn(locationIcon);
			}
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00197AA0 File Offset: 0x00195CA0
		private void TweenLocationIconAway()
		{
			Tween locationIconTween = this._locationIconTween;
			if (locationIconTween != null)
			{
				locationIconTween.Kill();
			}
			this._locationIconTween = this._locationIcon.CreateTween();
			this._locationIconTween.TweenProperty(this._locationIcon, "scale", Vector2.Zero, 0.4).SetTrans(10L).SetEase(0L);
			this._locationIconTween.TweenCallback(Callable.From<bool>(() => this._locationContainer.Visible = false));
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00197B2C File Offset: 0x00195D2C
		[NullableContext(2)]
		private void TweenLocationIconIn(Texture2D texture)
		{
			Tween locationIconTween = this._locationIconTween;
			if (locationIconTween != null)
			{
				locationIconTween.Kill();
			}
			this._locationIconTween = this._locationIcon.CreateTween();
			if (!this._locationContainer.Visible)
			{
				this._locationContainer.Visible = true;
				this._locationIcon.Texture = texture;
				this._locationIconTween.TweenProperty(this._locationIcon, "scale", Vector2.One, 0.4).SetTrans(10L).SetEase(1L);
				return;
			}
			this._locationIconTween.TweenProperty(this._locationIcon, "scale", Vector2.One * 0.5f, 0.2).SetTrans(10L).SetEase(0L);
			this._locationIconTween.TweenCallback(Callable.From<Texture2D>(() => this._locationIcon.Texture = texture));
			this._locationIconTween.TweenProperty(this._locationIcon, "scale", Vector2.One, 0.3).SetTrans(10L).SetEase(1L);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00197C7B File Offset: 0x00195E7B
		protected void OnFocus(NButton _)
		{
			this._isMouseOver = true;
			this.UpdateHighlightedState();
			if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode(this))
			{
				NTargetManager.Instance.OnNodeHovered(this);
				this._focusedWhileTargeting = true;
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00197CB5 File Offset: 0x00195EB5
		protected void OnUnfocus(NButton _)
		{
			this._isMouseOver = false;
			this.UpdateHighlightedState();
			if (this._focusedWhileTargeting)
			{
				NTargetManager.Instance.OnNodeUnhovered(this);
			}
			this._focusedWhileTargeting = false;
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x00197CE0 File Offset: 0x00195EE0
		protected void OnRelease(NButton _)
		{
			if (!NTargetManager.Instance.IsInSelection && NTargetManager.Instance.LastTargetingFinishedFrame != base.GetTree().GetFrame())
			{
				NMultiplayerPlayerExpandedState nmultiplayerPlayerExpandedState = NMultiplayerPlayerExpandedState.Create(this.Player);
				NCapstoneContainer.Instance.Open(nmultiplayerPlayerExpandedState);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003687 RID: 13959 RVA: 0x00197D28 File Offset: 0x00195F28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(26)
			{
				new MethodInfo(NMultiplayerPlayerState.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false),
					new PropertyInfo(2L, "__", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshValues, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnEnergyChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false),
					new PropertyInfo(2L, "__", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnStarsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false),
					new PropertyInfo(2L, "__", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshCombatValues, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.FlashPlayerReady, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateHighlightedState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.BlockChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldBlock", 0L, "", 6L, false),
					new PropertyInfo(2L, "blockGain", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshConnectedState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshConnectedState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.FlashEndTurn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.SetNextTweenTime, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false),
					new PropertyInfo(2L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.TweenLocationIconAway, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.TweenLocationIconIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "texture", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerState.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003688 RID: 13960 RVA: 0x0019846C File Offset: 0x0019666C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerPlayerState.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnCreatureValueChanged && args.Count == 2)
			{
				this.OnCreatureValueChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.RefreshValues && args.Count == 0)
			{
				this.RefreshValues();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth && args.Count == 0)
			{
				this.UpdateHealthBarWidth();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth && args.Count == 0)
			{
				this.UpdateSelectionReticleWidth();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnEnergyChanged && args.Count == 2)
			{
				this.OnEnergyChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnStarsChanged && args.Count == 2)
			{
				this.OnStarsChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.RefreshCombatValues && args.Count == 0)
			{
				this.RefreshCombatValues();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnCreatureHovered && args.Count == 0)
			{
				this.OnCreatureHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnCreatureUnhovered && args.Count == 0)
			{
				this.OnCreatureUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.FlashPlayerReady && args.Count == 0)
			{
				this.FlashPlayerReady();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.UpdateHighlightedState && args.Count == 0)
			{
				this.UpdateHighlightedState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.BlockChanged && args.Count == 2)
			{
				this.BlockChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.RefreshConnectedState && args.Count == 1)
			{
				this.RefreshConnectedState(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.RefreshConnectedState && args.Count == 0)
			{
				this.RefreshConnectedState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared && args.Count == 0)
			{
				this.OnPlayerVotesCleared();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing && args.Count == 1)
			{
				this.OnPlayerEndTurnPing(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.FlashEndTurn && args.Count == 0)
			{
				this.FlashEndTurn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.SetNextTweenTime && args.Count == 0)
			{
				this.SetNextTweenTime();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged && args.Count == 2)
			{
				this.OnPlayerScreenChanged(VariantUtils.ConvertTo<ulong>(args[0]), VariantUtils.ConvertTo<NetScreenType>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.TweenLocationIconAway && args.Count == 0)
			{
				this.TweenLocationIconAway();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.TweenLocationIconIn && args.Count == 1)
			{
				this.TweenLocationIconIn(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnFocus && args.Count == 1)
			{
				this.OnFocus(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnUnfocus && args.Count == 1)
			{
				this.OnUnfocus(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerState.MethodName.OnRelease && args.Count == 1)
			{
				this.OnRelease(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003689 RID: 13961 RVA: 0x00198920 File Offset: 0x00196B20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerPlayerState.MethodName._Ready || (ref method) == NMultiplayerPlayerState.MethodName._ExitTree || (ref method) == NMultiplayerPlayerState.MethodName.OnCreatureValueChanged || (ref method) == NMultiplayerPlayerState.MethodName.RefreshValues || (ref method) == NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth || (ref method) == NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth || (ref method) == NMultiplayerPlayerState.MethodName.OnEnergyChanged || (ref method) == NMultiplayerPlayerState.MethodName.OnStarsChanged || (ref method) == NMultiplayerPlayerState.MethodName.RefreshCombatValues || (ref method) == NMultiplayerPlayerState.MethodName.OnCreatureHovered || (ref method) == NMultiplayerPlayerState.MethodName.OnCreatureUnhovered || (ref method) == NMultiplayerPlayerState.MethodName.FlashPlayerReady || (ref method) == NMultiplayerPlayerState.MethodName.UpdateHighlightedState || (ref method) == NMultiplayerPlayerState.MethodName.BlockChanged || (ref method) == NMultiplayerPlayerState.MethodName.RefreshConnectedState || (ref method) == NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared || (ref method) == NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing || (ref method) == NMultiplayerPlayerState.MethodName.FlashEndTurn || (ref method) == NMultiplayerPlayerState.MethodName.SetNextTweenTime || (ref method) == NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged || (ref method) == NMultiplayerPlayerState.MethodName.TweenLocationIconAway || (ref method) == NMultiplayerPlayerState.MethodName.TweenLocationIconIn || (ref method) == NMultiplayerPlayerState.MethodName.OnFocus || (ref method) == NMultiplayerPlayerState.MethodName.OnUnfocus || (ref method) == NMultiplayerPlayerState.MethodName.OnRelease || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600368A RID: 13962 RVA: 0x00198AAC File Offset: 0x00196CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerState.PropertyName.Hitbox)
			{
				this.Hitbox = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._healthBar)
			{
				this._healthBar = VariantUtils.ConvertTo<NHealthBar>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._characterIcon)
			{
				this._characterIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._nameplateLabel)
			{
				this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._topContainer)
			{
				this._topContainer = VariantUtils.ConvertTo<HBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._turnEndIndicator)
			{
				this._turnEndIndicator = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._disconnectedIndicator)
			{
				this._disconnectedIndicator = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._networkProblemIndicator)
			{
				this._networkProblemIndicator = VariantUtils.ConvertTo<NMultiplayerNetworkProblemIndicator>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationIcon)
			{
				this._locationIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationContainer)
			{
				this._locationContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyContainer)
			{
				this._energyContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyImage)
			{
				this._energyImage = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyCount)
			{
				this._energyCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._starContainer)
			{
				this._starContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._starCount)
			{
				this._starCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardContainer)
			{
				this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardImage)
			{
				this._cardImage = VariantUtils.ConvertTo<NTinyCard>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardCount)
			{
				this._cardCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationIconTween)
			{
				this._locationIconTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isMouseOver)
			{
				this._isMouseOver = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isCreatureHovered)
			{
				this._isCreatureHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isHighlighted)
			{
				this._isHighlighted = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._focusedWhileTargeting)
			{
				this._focusedWhileTargeting = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._nextTweenTime)
			{
				this._nextTweenTime = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._currentLocationIcon)
			{
				this._currentLocationIcon = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600368B RID: 13963 RVA: 0x00198D80 File Offset: 0x00196F80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerState.PropertyName.Hitbox)
			{
				NButton hitbox = this.Hitbox;
				value = VariantUtils.CreateFrom<NButton>(ref hitbox);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._healthBar)
			{
				value = VariantUtils.CreateFrom<NHealthBar>(ref this._healthBar);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._characterIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._characterIcon);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._nameplateLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._topContainer)
			{
				value = VariantUtils.CreateFrom<HBoxContainer>(ref this._topContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._turnEndIndicator)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._turnEndIndicator);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._disconnectedIndicator)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._disconnectedIndicator);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._networkProblemIndicator)
			{
				value = VariantUtils.CreateFrom<NMultiplayerNetworkProblemIndicator>(ref this._networkProblemIndicator);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._locationIcon);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._locationContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._energyContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyImage)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._energyImage);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._energyCount)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._energyCount);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._starContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._starContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._starCount)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._starCount);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardImage)
			{
				value = VariantUtils.CreateFrom<NTinyCard>(ref this._cardImage);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._cardCount)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._cardCount);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._locationIconTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._locationIconTween);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isMouseOver)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isMouseOver);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isCreatureHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isCreatureHovered);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._isHighlighted)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHighlighted);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._focusedWhileTargeting)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._focusedWhileTargeting);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._nextTweenTime)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._nextTweenTime);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerState.PropertyName._currentLocationIcon)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._currentLocationIcon);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600368C RID: 13964 RVA: 0x001990D8 File Offset: 0x001972D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._healthBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._characterIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._nameplateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._topContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._turnEndIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._disconnectedIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._networkProblemIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._locationIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._locationContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._energyContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._energyImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._energyCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._starContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._starCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._cardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._cardImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._cardCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._locationIconTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerState.PropertyName._isMouseOver, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerState.PropertyName._isCreatureHovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerState.PropertyName._isHighlighted, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMultiplayerPlayerState.PropertyName._nextTweenTime, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerState.PropertyName._currentLocationIcon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600368D RID: 13965 RVA: 0x00199444 File Offset: 0x00197644
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hitbox = NMultiplayerPlayerState.PropertyName.Hitbox;
			NButton hitbox2 = this.Hitbox;
			info.AddProperty(hitbox, Variant.From<NButton>(ref hitbox2));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._healthBar, Variant.From<NHealthBar>(ref this._healthBar));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._characterIcon, Variant.From<TextureRect>(ref this._characterIcon));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._topContainer, Variant.From<HBoxContainer>(ref this._topContainer));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._turnEndIndicator, Variant.From<TextureRect>(ref this._turnEndIndicator));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._disconnectedIndicator, Variant.From<TextureRect>(ref this._disconnectedIndicator));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._networkProblemIndicator, Variant.From<NMultiplayerNetworkProblemIndicator>(ref this._networkProblemIndicator));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._locationIcon, Variant.From<TextureRect>(ref this._locationIcon));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._locationContainer, Variant.From<Control>(ref this._locationContainer));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._energyContainer, Variant.From<Control>(ref this._energyContainer));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._energyImage, Variant.From<TextureRect>(ref this._energyImage));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._energyCount, Variant.From<MegaLabel>(ref this._energyCount));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._starContainer, Variant.From<Control>(ref this._starContainer));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._starCount, Variant.From<MegaLabel>(ref this._starCount));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._cardImage, Variant.From<NTinyCard>(ref this._cardImage));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._cardCount, Variant.From<MegaLabel>(ref this._cardCount));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._locationIconTween, Variant.From<Tween>(ref this._locationIconTween));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._isMouseOver, Variant.From<bool>(ref this._isMouseOver));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._isCreatureHovered, Variant.From<bool>(ref this._isCreatureHovered));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._isHighlighted, Variant.From<bool>(ref this._isHighlighted));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, Variant.From<bool>(ref this._focusedWhileTargeting));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._nextTweenTime, Variant.From<ulong>(ref this._nextTweenTime));
			info.AddProperty(NMultiplayerPlayerState.PropertyName._currentLocationIcon, Variant.From<Texture2D>(ref this._currentLocationIcon));
		}

		/// <inheritdoc />
		// Token: 0x0600368E RID: 13966 RVA: 0x00199698 File Offset: 0x00197898
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName.Hitbox, ref variant))
			{
				this.Hitbox = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._healthBar, ref variant2))
			{
				this._healthBar = variant2.As<NHealthBar>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._characterIcon, ref variant3))
			{
				this._characterIcon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._nameplateLabel, ref variant4))
			{
				this._nameplateLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._topContainer, ref variant5))
			{
				this._topContainer = variant5.As<HBoxContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._turnEndIndicator, ref variant6))
			{
				this._turnEndIndicator = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._disconnectedIndicator, ref variant7))
			{
				this._disconnectedIndicator = variant7.As<TextureRect>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._networkProblemIndicator, ref variant8))
			{
				this._networkProblemIndicator = variant8.As<NMultiplayerNetworkProblemIndicator>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._selectionReticle, ref variant9))
			{
				this._selectionReticle = variant9.As<NSelectionReticle>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationIcon, ref variant10))
			{
				this._locationIcon = variant10.As<TextureRect>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationContainer, ref variant11))
			{
				this._locationContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyContainer, ref variant12))
			{
				this._energyContainer = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyImage, ref variant13))
			{
				this._energyImage = variant13.As<TextureRect>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyCount, ref variant14))
			{
				this._energyCount = variant14.As<MegaLabel>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._starContainer, ref variant15))
			{
				this._starContainer = variant15.As<Control>();
			}
			Variant variant16;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._starCount, ref variant16))
			{
				this._starCount = variant16.As<MegaLabel>();
			}
			Variant variant17;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardContainer, ref variant17))
			{
				this._cardContainer = variant17.As<Control>();
			}
			Variant variant18;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardImage, ref variant18))
			{
				this._cardImage = variant18.As<NTinyCard>();
			}
			Variant variant19;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardCount, ref variant19))
			{
				this._cardCount = variant19.As<MegaLabel>();
			}
			Variant variant20;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationIconTween, ref variant20))
			{
				this._locationIconTween = variant20.As<Tween>();
			}
			Variant variant21;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isMouseOver, ref variant21))
			{
				this._isMouseOver = variant21.As<bool>();
			}
			Variant variant22;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isCreatureHovered, ref variant22))
			{
				this._isCreatureHovered = variant22.As<bool>();
			}
			Variant variant23;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isHighlighted, ref variant23))
			{
				this._isHighlighted = variant23.As<bool>();
			}
			Variant variant24;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, ref variant24))
			{
				this._focusedWhileTargeting = variant24.As<bool>();
			}
			Variant variant25;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._nextTweenTime, ref variant25))
			{
				this._nextTweenTime = variant25.As<ulong>();
			}
			Variant variant26;
			if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._currentLocationIcon, ref variant26))
			{
				this._currentLocationIcon = variant26.As<Texture2D>();
			}
		}

		// Token: 0x04001506 RID: 5382
		private const ulong _delayBetweenTweensMsec = 500UL;

		// Token: 0x04001507 RID: 5383
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_player_state");

		// Token: 0x04001508 RID: 5384
		private static readonly string _cardScenePath = SceneHelper.GetScenePath("screens/run_history_screen/deck_history_entry");

		// Token: 0x04001509 RID: 5385
		private const string _darkenedEnergyMatPath = "res://materials/ui/energy_orb_dark.tres";

		// Token: 0x0400150A RID: 5386
		private const float _refHpBarWidth = 175f;

		// Token: 0x0400150B RID: 5387
		private const float _refHpBarMaxHp = 80f;

		// Token: 0x0400150C RID: 5388
		private const float _selectionReticlePadding = 6f;

		// Token: 0x0400150D RID: 5389
		private NHealthBar _healthBar;

		// Token: 0x0400150E RID: 5390
		private TextureRect _characterIcon;

		// Token: 0x0400150F RID: 5391
		private MegaLabel _nameplateLabel;

		// Token: 0x04001510 RID: 5392
		private HBoxContainer _topContainer;

		// Token: 0x04001511 RID: 5393
		private TextureRect _turnEndIndicator;

		// Token: 0x04001512 RID: 5394
		private TextureRect _disconnectedIndicator;

		// Token: 0x04001513 RID: 5395
		private NMultiplayerNetworkProblemIndicator _networkProblemIndicator;

		// Token: 0x04001514 RID: 5396
		private NSelectionReticle _selectionReticle;

		// Token: 0x04001516 RID: 5398
		private TextureRect _locationIcon;

		// Token: 0x04001517 RID: 5399
		private Control _locationContainer;

		// Token: 0x04001518 RID: 5400
		private Control _energyContainer;

		// Token: 0x04001519 RID: 5401
		private TextureRect _energyImage;

		// Token: 0x0400151A RID: 5402
		private MegaLabel _energyCount;

		// Token: 0x0400151B RID: 5403
		private Control _starContainer;

		// Token: 0x0400151C RID: 5404
		private MegaLabel _starCount;

		// Token: 0x0400151D RID: 5405
		private Control _cardContainer;

		// Token: 0x0400151E RID: 5406
		private NTinyCard _cardImage;

		// Token: 0x0400151F RID: 5407
		private MegaLabel _cardCount;

		// Token: 0x04001520 RID: 5408
		[Nullable(2)]
		private Tween _locationIconTween;

		// Token: 0x04001522 RID: 5410
		private bool _isMouseOver;

		// Token: 0x04001523 RID: 5411
		private bool _isCreatureHovered;

		// Token: 0x04001524 RID: 5412
		private bool _isHighlighted;

		// Token: 0x04001525 RID: 5413
		private bool _focusedWhileTargeting;

		// Token: 0x04001526 RID: 5414
		private ulong _nextTweenTime;

		// Token: 0x04001527 RID: 5415
		[Nullable(2)]
		private Texture2D _currentLocationIcon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001725 RID: 5925
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005A31 RID: 23089
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005A32 RID: 23090
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnCreatureValueChanged' method.
			/// </summary>
			// Token: 0x04005A33 RID: 23091
			public static readonly StringName OnCreatureValueChanged = "OnCreatureValueChanged";

			/// <summary>
			/// Cached name for the 'RefreshValues' method.
			/// </summary>
			// Token: 0x04005A34 RID: 23092
			public static readonly StringName RefreshValues = "RefreshValues";

			/// <summary>
			/// Cached name for the 'UpdateHealthBarWidth' method.
			/// </summary>
			// Token: 0x04005A35 RID: 23093
			public static readonly StringName UpdateHealthBarWidth = "UpdateHealthBarWidth";

			/// <summary>
			/// Cached name for the 'UpdateSelectionReticleWidth' method.
			/// </summary>
			// Token: 0x04005A36 RID: 23094
			public static readonly StringName UpdateSelectionReticleWidth = "UpdateSelectionReticleWidth";

			/// <summary>
			/// Cached name for the 'OnEnergyChanged' method.
			/// </summary>
			// Token: 0x04005A37 RID: 23095
			public static readonly StringName OnEnergyChanged = "OnEnergyChanged";

			/// <summary>
			/// Cached name for the 'OnStarsChanged' method.
			/// </summary>
			// Token: 0x04005A38 RID: 23096
			public static readonly StringName OnStarsChanged = "OnStarsChanged";

			/// <summary>
			/// Cached name for the 'RefreshCombatValues' method.
			/// </summary>
			// Token: 0x04005A39 RID: 23097
			public static readonly StringName RefreshCombatValues = "RefreshCombatValues";

			/// <summary>
			/// Cached name for the 'OnCreatureHovered' method.
			/// </summary>
			// Token: 0x04005A3A RID: 23098
			public static readonly StringName OnCreatureHovered = "OnCreatureHovered";

			/// <summary>
			/// Cached name for the 'OnCreatureUnhovered' method.
			/// </summary>
			// Token: 0x04005A3B RID: 23099
			public static readonly StringName OnCreatureUnhovered = "OnCreatureUnhovered";

			/// <summary>
			/// Cached name for the 'FlashPlayerReady' method.
			/// </summary>
			// Token: 0x04005A3C RID: 23100
			public static readonly StringName FlashPlayerReady = "FlashPlayerReady";

			/// <summary>
			/// Cached name for the 'UpdateHighlightedState' method.
			/// </summary>
			// Token: 0x04005A3D RID: 23101
			public static readonly StringName UpdateHighlightedState = "UpdateHighlightedState";

			/// <summary>
			/// Cached name for the 'BlockChanged' method.
			/// </summary>
			// Token: 0x04005A3E RID: 23102
			public static readonly StringName BlockChanged = "BlockChanged";

			/// <summary>
			/// Cached name for the 'RefreshConnectedState' method.
			/// </summary>
			// Token: 0x04005A3F RID: 23103
			public static readonly StringName RefreshConnectedState = "RefreshConnectedState";

			/// <summary>
			/// Cached name for the 'OnPlayerVotesCleared' method.
			/// </summary>
			// Token: 0x04005A40 RID: 23104
			public static readonly StringName OnPlayerVotesCleared = "OnPlayerVotesCleared";

			/// <summary>
			/// Cached name for the 'OnPlayerEndTurnPing' method.
			/// </summary>
			// Token: 0x04005A41 RID: 23105
			public static readonly StringName OnPlayerEndTurnPing = "OnPlayerEndTurnPing";

			/// <summary>
			/// Cached name for the 'FlashEndTurn' method.
			/// </summary>
			// Token: 0x04005A42 RID: 23106
			public static readonly StringName FlashEndTurn = "FlashEndTurn";

			/// <summary>
			/// Cached name for the 'SetNextTweenTime' method.
			/// </summary>
			// Token: 0x04005A43 RID: 23107
			public static readonly StringName SetNextTweenTime = "SetNextTweenTime";

			/// <summary>
			/// Cached name for the 'OnPlayerScreenChanged' method.
			/// </summary>
			// Token: 0x04005A44 RID: 23108
			public static readonly StringName OnPlayerScreenChanged = "OnPlayerScreenChanged";

			/// <summary>
			/// Cached name for the 'TweenLocationIconAway' method.
			/// </summary>
			// Token: 0x04005A45 RID: 23109
			public static readonly StringName TweenLocationIconAway = "TweenLocationIconAway";

			/// <summary>
			/// Cached name for the 'TweenLocationIconIn' method.
			/// </summary>
			// Token: 0x04005A46 RID: 23110
			public static readonly StringName TweenLocationIconIn = "TweenLocationIconIn";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005A47 RID: 23111
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005A48 RID: 23112
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005A49 RID: 23113
			public static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001726 RID: 5926
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x04005A4A RID: 23114
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the '_healthBar' field.
			/// </summary>
			// Token: 0x04005A4B RID: 23115
			public static readonly StringName _healthBar = "_healthBar";

			/// <summary>
			/// Cached name for the '_characterIcon' field.
			/// </summary>
			// Token: 0x04005A4C RID: 23116
			public static readonly StringName _characterIcon = "_characterIcon";

			/// <summary>
			/// Cached name for the '_nameplateLabel' field.
			/// </summary>
			// Token: 0x04005A4D RID: 23117
			public static readonly StringName _nameplateLabel = "_nameplateLabel";

			/// <summary>
			/// Cached name for the '_topContainer' field.
			/// </summary>
			// Token: 0x04005A4E RID: 23118
			public static readonly StringName _topContainer = "_topContainer";

			/// <summary>
			/// Cached name for the '_turnEndIndicator' field.
			/// </summary>
			// Token: 0x04005A4F RID: 23119
			public static readonly StringName _turnEndIndicator = "_turnEndIndicator";

			/// <summary>
			/// Cached name for the '_disconnectedIndicator' field.
			/// </summary>
			// Token: 0x04005A50 RID: 23120
			public static readonly StringName _disconnectedIndicator = "_disconnectedIndicator";

			/// <summary>
			/// Cached name for the '_networkProblemIndicator' field.
			/// </summary>
			// Token: 0x04005A51 RID: 23121
			public static readonly StringName _networkProblemIndicator = "_networkProblemIndicator";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005A52 RID: 23122
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_locationIcon' field.
			/// </summary>
			// Token: 0x04005A53 RID: 23123
			public static readonly StringName _locationIcon = "_locationIcon";

			/// <summary>
			/// Cached name for the '_locationContainer' field.
			/// </summary>
			// Token: 0x04005A54 RID: 23124
			public static readonly StringName _locationContainer = "_locationContainer";

			/// <summary>
			/// Cached name for the '_energyContainer' field.
			/// </summary>
			// Token: 0x04005A55 RID: 23125
			public static readonly StringName _energyContainer = "_energyContainer";

			/// <summary>
			/// Cached name for the '_energyImage' field.
			/// </summary>
			// Token: 0x04005A56 RID: 23126
			public static readonly StringName _energyImage = "_energyImage";

			/// <summary>
			/// Cached name for the '_energyCount' field.
			/// </summary>
			// Token: 0x04005A57 RID: 23127
			public static readonly StringName _energyCount = "_energyCount";

			/// <summary>
			/// Cached name for the '_starContainer' field.
			/// </summary>
			// Token: 0x04005A58 RID: 23128
			public static readonly StringName _starContainer = "_starContainer";

			/// <summary>
			/// Cached name for the '_starCount' field.
			/// </summary>
			// Token: 0x04005A59 RID: 23129
			public static readonly StringName _starCount = "_starCount";

			/// <summary>
			/// Cached name for the '_cardContainer' field.
			/// </summary>
			// Token: 0x04005A5A RID: 23130
			public static readonly StringName _cardContainer = "_cardContainer";

			/// <summary>
			/// Cached name for the '_cardImage' field.
			/// </summary>
			// Token: 0x04005A5B RID: 23131
			public static readonly StringName _cardImage = "_cardImage";

			/// <summary>
			/// Cached name for the '_cardCount' field.
			/// </summary>
			// Token: 0x04005A5C RID: 23132
			public static readonly StringName _cardCount = "_cardCount";

			/// <summary>
			/// Cached name for the '_locationIconTween' field.
			/// </summary>
			// Token: 0x04005A5D RID: 23133
			public static readonly StringName _locationIconTween = "_locationIconTween";

			/// <summary>
			/// Cached name for the '_isMouseOver' field.
			/// </summary>
			// Token: 0x04005A5E RID: 23134
			public static readonly StringName _isMouseOver = "_isMouseOver";

			/// <summary>
			/// Cached name for the '_isCreatureHovered' field.
			/// </summary>
			// Token: 0x04005A5F RID: 23135
			public static readonly StringName _isCreatureHovered = "_isCreatureHovered";

			/// <summary>
			/// Cached name for the '_isHighlighted' field.
			/// </summary>
			// Token: 0x04005A60 RID: 23136
			public static readonly StringName _isHighlighted = "_isHighlighted";

			/// <summary>
			/// Cached name for the '_focusedWhileTargeting' field.
			/// </summary>
			// Token: 0x04005A61 RID: 23137
			public static readonly StringName _focusedWhileTargeting = "_focusedWhileTargeting";

			/// <summary>
			/// Cached name for the '_nextTweenTime' field.
			/// </summary>
			// Token: 0x04005A62 RID: 23138
			public static readonly StringName _nextTweenTime = "_nextTweenTime";

			/// <summary>
			/// Cached name for the '_currentLocationIcon' field.
			/// </summary>
			// Token: 0x04005A63 RID: 23139
			public static readonly StringName _currentLocationIcon = "_currentLocationIcon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001727 RID: 5927
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
