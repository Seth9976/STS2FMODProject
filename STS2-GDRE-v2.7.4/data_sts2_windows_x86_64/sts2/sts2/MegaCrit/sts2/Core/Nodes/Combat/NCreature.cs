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
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x020003F9 RID: 1017
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCreature.cs")]
	public class NCreature : Control
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x001E070D File Offset: 0x001DE90D
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCreature._scenePath);
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x001E0719 File Offset: 0x001DE919
		public static Vector2 PowerAppliedVfxPositionOffset
		{
			get
			{
				return new Vector2(0f, -200f);
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x001E072A File Offset: 0x001DE92A
		// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x001E0732 File Offset: 0x001DE932
		[Nullable(2)]
		public Task DeathAnimationTask
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x001E073B File Offset: 0x001DE93B
		public CancellationTokenSource DeathAnimCancelToken { get; } = new CancellationTokenSource();

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003EC4 RID: 16068 RVA: 0x001E0743 File Offset: 0x001DE943
		// (set) Token: 0x06003EC5 RID: 16069 RVA: 0x001E074B File Offset: 0x001DE94B
		public Control Hitbox { get; private set; }

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x001E0754 File Offset: 0x001DE954
		// (set) Token: 0x06003EC7 RID: 16071 RVA: 0x001E075C File Offset: 0x001DE95C
		[Nullable(2)]
		public NOrbManager OrbManager
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x001E0765 File Offset: 0x001DE965
		// (set) Token: 0x06003EC9 RID: 16073 RVA: 0x001E076D File Offset: 0x001DE96D
		public bool IsInteractable { get; private set; } = true;

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003ECA RID: 16074 RVA: 0x001E0776 File Offset: 0x001DE976
		// (set) Token: 0x06003ECB RID: 16075 RVA: 0x001E077E File Offset: 0x001DE97E
		public Creature Entity { get; private set; }

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003ECC RID: 16076 RVA: 0x001E0787 File Offset: 0x001DE987
		public Vector2 VfxSpawnPosition
		{
			get
			{
				return this.Visuals.VfxSpawnPosition.GlobalPosition;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003ECD RID: 16077 RVA: 0x001E0799 File Offset: 0x001DE999
		public Vector2 PowerAppliedVfxSpawnPosition
		{
			get
			{
				return this.VfxSpawnPosition + NCreature.PowerAppliedVfxPositionOffset;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003ECE RID: 16078 RVA: 0x001E07AB File Offset: 0x001DE9AB
		// (set) Token: 0x06003ECF RID: 16079 RVA: 0x001E07B3 File Offset: 0x001DE9B3
		public NCreatureVisuals Visuals { get; private set; }

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x001E07BC File Offset: 0x001DE9BC
		public Node2D Body
		{
			get
			{
				return this.Visuals.GetCurrentBody();
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x001E07C9 File Offset: 0x001DE9C9
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x001E07D1 File Offset: 0x001DE9D1
		public Control IntentContainer { get; private set; }

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x001E07DA File Offset: 0x001DE9DA
		public bool IsPlayingDeathAnimation
		{
			get
			{
				return this.DeathAnimationTask != null;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x001E07E5 File Offset: 0x001DE9E5
		public bool HasSpineAnimation
		{
			get
			{
				return this.Visuals.HasSpineAnimation;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x001E07F2 File Offset: 0x001DE9F2
		public SpineAnimationAccess SpineAnimation
		{
			get
			{
				return this.Visuals.SpineAnimation;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x001E07FF File Offset: 0x001DE9FF
		// (set) Token: 0x06003ED7 RID: 16087 RVA: 0x001E0807 File Offset: 0x001DEA07
		public bool IsFocused { get; private set; }

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x001E0810 File Offset: 0x001DEA10
		// (set) Token: 0x06003ED9 RID: 16089 RVA: 0x001E0818 File Offset: 0x001DEA18
		[Nullable(2)]
		public NMultiplayerPlayerIntentHandler PlayerIntentHandler
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x001E0821 File Offset: 0x001DEA21
		[return: Nullable(2)]
		public T GetSpecialNode<[Nullable(0)] T>(string name) where T : Node
		{
			return this.Visuals.GetNodeOrNull<T>(name);
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x001E0834 File Offset: 0x001DEA34
		[return: Nullable(2)]
		public static NCreature Create(Creature entity)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature ncreature = PreloadManager.Cache.GetScene(NCreature._scenePath).Instantiate<NCreature>(0L);
			ncreature.Entity = entity;
			ncreature.Visuals = entity.CreateVisuals();
			return ncreature;
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x001E0878 File Offset: 0x001DEA78
		public override void _Ready()
		{
			this._stateDisplay = base.GetNode<NCreatureStateDisplay>("%HealthBar");
			this.IntentContainer = base.GetNode<Control>("%Intents");
			this.Hitbox = base.GetNode<Control>("%Hitbox");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this.Hitbox.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			if (this.Entity.IsPlayer)
			{
				this.OrbManager = NOrbManager.Create(this, LocalContext.IsMe(this.Entity));
				this.AddChildSafely(this.OrbManager);
				this.UpdateNavigation();
			}
			bool flag;
			if (this.Entity.IsPlayer)
			{
				ICombatState combatState = this.Entity.CombatState;
				flag = combatState != null && combatState.RunState.Players.Count > 1;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.PlayerIntentHandler = NMultiplayerPlayerIntentHandler.Create(this.Entity.Player);
				if (this.PlayerIntentHandler != null)
				{
					this.IntentContainer.AddChildSafely(this.PlayerIntentHandler);
					this.IntentContainer.Modulate = Colors.White;
				}
			}
			this.AddChildSafely(this.Visuals);
			this.MoveChildSafely(this.Visuals, 0);
			this.Visuals.Position = Vector2.Zero;
			this._stateDisplay.SetCreature(this.Entity);
			bool flag3 = this.Entity.PetOwner != null && !LocalContext.IsMe(this.Entity.PetOwner);
			bool flag4 = this.Entity.IsPlayer && !LocalContext.IsMe(this.Entity);
			this._isRemotePlayerOrPet = flag4 || flag3;
			if (this._isRemotePlayerOrPet)
			{
				this._stateDisplay.HideImmediately();
			}
			else
			{
				bool flag5 = NCombatRoom.Instance != null && Time.GetTicksMsec() - NCombatRoom.Instance.CreatedMsec < 1000UL;
				this._stateDisplay.AnimateIn(flag5 ? HealthBarAnimMode.SpawnedAtCombatStart : HealthBarAnimMode.SpawnedDuringCombat);
			}
			if (this.HasSpineAnimation)
			{
				if (this.Entity.Player != null)
				{
					this._spineAnimator = this.Entity.Player.Character.GenerateAnimator(this.Visuals.SpineBody);
				}
				else
				{
					this._spineAnimator = this.Entity.Monster.GenerateAnimator(this.Visuals.SpineBody);
					this.Visuals.SetUpSkin(this.Entity.Monster);
				}
				this.ConnectSpineAnimatorSignals();
				if (this.Entity.IsDead)
				{
					this.SetAnimationTrigger("Dead");
					using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack(0))
					{
						if (currentTrack != null)
						{
							currentTrack.SetTrackTime(currentTrack.GetAnimationEnd());
						}
					}
				}
			}
			this.SetOrbManagerPosition();
			if (this.Entity.Monster != null)
			{
				this.ToggleIsInteractable(this.Entity.Monster.IsHealthBarVisible);
			}
			this.UpdateBounds(this.Visuals);
			this.UpdatePhobiaMode();
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x001E0BFC File Offset: 0x001DEDFC
		public override void _EnterTree()
		{
			base._EnterTree();
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
			}
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
			this.Entity.PowerApplied += this.OnPowerApplied;
			this.Entity.PowerRemoved += this.OnPowerRemoved;
			this.Entity.PowerIncreased += this.OnPowerIncreased;
			foreach (PowerModel powerModel in this.Entity.Powers)
			{
				this.SubscribeToPower(powerModel);
			}
			this.ConnectSpineAnimatorSignals();
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x001E0CE0 File Offset: 0x001DEEE0
		public override void _ExitTree()
		{
			base._ExitTree();
			this.StopAllSfxLoops();
			this.DeathAnimCancelToken.Cancel();
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
			}
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
			this.Entity.PowerApplied -= this.OnPowerApplied;
			this.Entity.PowerRemoved -= this.OnPowerRemoved;
			this.Entity.PowerIncreased -= this.OnPowerIncreased;
			foreach (PowerModel powerModel in this.Entity.Powers)
			{
				this.UnsubscribeFromPower(powerModel);
			}
			if (this._spineAnimator != null)
			{
				this._spineAnimator.BoundsUpdated -= this.UpdateBounds;
			}
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.ShowCreatureHoverTips;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x001E0E04 File Offset: 0x001DF004
		private void ConnectSpineAnimatorSignals()
		{
			if (this._spineAnimator != null)
			{
				this._spineAnimator.BoundsUpdated -= this.UpdateBounds;
				this._spineAnimator.BoundsUpdated += this.UpdateBounds;
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x001E0E3C File Offset: 0x001DF03C
		private void UpdateBounds(string boundsNodeName)
		{
			this.UpdateBounds(this.Visuals.GetNode<Control>(boundsNodeName));
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x001E0E55 File Offset: 0x001DF055
		private void UpdatePhobiaMode()
		{
			this.Visuals.UpdatePhobiaMode(this.Entity.Monster);
		}

		/// <summary>
		/// Called when a creatures hitbox is updated. Called on initialization and things like Osty getting bigger, thieving hopper flying, etc.
		/// </summary>
		/// <param name="boundsContainer"></param> The parent node which has the children: Bounds, CenterPos, and IntentPos nodes.
		// Token: 0x06003EE2 RID: 16098 RVA: 0x001E0E70 File Offset: 0x001DF070
		private void UpdateBounds(Node boundsContainer)
		{
			Control node = boundsContainer.GetNode<Control>("%Bounds");
			Vector2 vector = node.Size * this.Visuals.Scale / this._tempScale;
			Vector2 vector2 = (node.GlobalPosition - base.GlobalPosition) / this._tempScale;
			this.Hitbox.Size = vector;
			this.Hitbox.GlobalPosition = base.GlobalPosition + vector2;
			this._selectionReticle.Size = vector;
			this._selectionReticle.GlobalPosition = base.GlobalPosition + vector2;
			this._selectionReticle.PivotOffset = this._selectionReticle.Size * 0.5f;
			this.IntentContainer.Position = boundsContainer.GetNode<Marker2D>("IntentPos").Position - this.IntentContainer.Size * 0.5f;
			this.IntentContainer.Position = new Vector2(this.IntentContainer.Position.X, this.IntentContainer.Position.Y * this.Visuals.Scale.X);
			this._stateDisplay.SetCreatureBounds(this.Hitbox);
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x001E0FBF File Offset: 0x001DF1BF
		public void UpdateNavigation()
		{
			if (this.OrbManager != null)
			{
				this.Hitbox.FocusNeighborTop = this.OrbManager.DefaultFocusOwner.GetPath();
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x001E0FE4 File Offset: 0x001DF1E4
		public Task UpdateIntent(IEnumerable<Creature> targets)
		{
			if (this.Entity.Monster == null)
			{
				throw new InvalidOperationException("Only valid on monsters.");
			}
			IReadOnlyList<AbstractIntent> intents = this.Entity.Monster.NextMove.Intents;
			int i = 0;
			while (i < intents.Count && i < this.IntentContainer.GetChildCount(false))
			{
				NIntent child = this.IntentContainer.GetChild<NIntent>(i, false);
				child.SetFrozen(false);
				child.UpdateIntent(intents[i], targets, this.Entity);
				i++;
			}
			float num = (float)this.GetHashCode() * 0.01f;
			while (i < intents.Count)
			{
				NIntent nintent = NIntent.Create(num + (float)i * 0.3f);
				this.IntentContainer.AddChildSafely(nintent);
				nintent.UpdateIntent(intents[i], targets, this.Entity);
				i++;
			}
			List<Node> list = this.IntentContainer.GetChildren(false).TakeLast<Node>(this.IntentContainer.GetChildCount(false) - i).ToList<Node>();
			foreach (Node node in list)
			{
				this.IntentContainer.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
			return Task.CompletedTask;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x001E1138 File Offset: 0x001DF338
		public async Task PerformIntent()
		{
			foreach (NIntent nintent in this.IntentContainer.GetChildren(false).OfType<NIntent>())
			{
				nintent.PlayPerform();
				nintent.SetFrozen(true);
			}
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				this.IntentContainer.Modulate = new Color(this.IntentContainer.Modulate.R, this.IntentContainer.Modulate.G, this.IntentContainer.Modulate.B, 0f);
			}
			else
			{
				this.AnimHideIntent(0.4);
				await Cmd.CustomScaledWait(0.25f, 0.4f, false, default(CancellationToken));
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x001E117C File Offset: 0x001DF37C
		public async Task RefreshIntents()
		{
			await this.UpdateIntent(this.Entity.CombatState.Players.Select<Player, Creature>((Player p) => p.Creature));
			await this.RevealIntents();
		}

		/// <summary>
		/// This makes the intents visible again!
		/// So let's fade them in.
		/// </summary>
		// Token: 0x06003EE7 RID: 16103 RVA: 0x001E11C0 File Offset: 0x001DF3C0
		private Task RevealIntents()
		{
			this.IntentContainer.Modulate = Colors.Transparent;
			Tween intentFadeTween = this._intentFadeTween;
			if (intentFadeTween != null)
			{
				intentFadeTween.Kill();
			}
			this._intentFadeTween = base.CreateTween().SetParallel(true);
			this._intentFadeTween.TweenProperty(this.IntentContainer, "modulate:a", 1f, 1.0).SetDelay((double)Rng.Chaotic.NextFloat(0f, 0.3f));
			return Task.CompletedTask;
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x001E1250 File Offset: 0x001DF450
		private void OnFocus()
		{
			if (this.IsFocused)
			{
				return;
			}
			if (this._isInBestiary)
			{
				return;
			}
			this.IsFocused = true;
			if (this._isRemotePlayerOrPet)
			{
				this._stateDisplay.AnimateIn(HealthBarAnimMode.FromHidden);
				this._stateDisplay.ZIndex = 1;
				Player me = LocalContext.GetMe(this.Entity.CombatState);
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					NCreature creatureNode = instance.GetCreatureNode((me != null) ? me.Creature : null);
					if (creatureNode != null)
					{
						creatureNode.SetRemotePlayerFocused(true);
					}
				}
			}
			else
			{
				this._stateDisplay.ShowNameplate();
			}
			NRun.Instance.GlobalUi.MultiplayerPlayerContainer.HighlightPlayer(this.Entity.Player);
			if (NTargetManager.Instance.IsInSelection)
			{
				NTargetManager.Instance.OnNodeHovered(this);
				return;
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this.ShowSingleSelectReticle();
			}
			this.ShowHoverTips(this.Entity.HoverTips);
			CombatManager.Instance.StateTracker.CombatStateChanged += this.ShowCreatureHoverTips;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x001E1354 File Offset: 0x001DF554
		private void OnUnfocus()
		{
			if (this._isInBestiary)
			{
				return;
			}
			this.IsFocused = false;
			this.HideSingleSelectReticle();
			if (this._isRemotePlayerOrPet)
			{
				this._stateDisplay.AnimateOut();
				Player me = LocalContext.GetMe(this.Entity.CombatState);
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					NCreature creatureNode = instance.GetCreatureNode((me != null) ? me.Creature : null);
					if (creatureNode != null)
					{
						creatureNode.SetRemotePlayerFocused(false);
					}
				}
			}
			else
			{
				this._stateDisplay.HideNameplate();
			}
			NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnhighlightPlayer(this.Entity.Player);
			NTargetManager.Instance.OnNodeUnhovered(this);
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.ShowCreatureHoverTips;
			this.HideHoverTips();
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x001E141B File Offset: 0x001DF61B
		public void OnTargetingStarted()
		{
			if (!this.IsFocused)
			{
				return;
			}
			NTargetManager.Instance.OnNodeHovered(this);
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.ShowCreatureHoverTips;
			this.HideHoverTips();
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x001E1452 File Offset: 0x001DF652
		private void ShowCreatureHoverTips(CombatState _)
		{
			if (this.Entity.CombatState == null)
			{
				return;
			}
			this.ShowHoverTips(this.Entity.HoverTips);
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x001E1473 File Offset: 0x001DF673
		public void ShowHoverTips(IEnumerable<IHoverTip> hoverTips)
		{
			if (NCombatRoom.Instance.Ui.Hand.InCardPlay)
			{
				return;
			}
			this.HideHoverTips();
			NHoverTipSet.CreateAndShow(this.Hitbox, hoverTips, HoverTip.GetHoverTipAlignment(this, 0.5f));
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x001E14AC File Offset: 0x001DF6AC
		public void SetRemotePlayerFocused(bool remotePlayerFocused)
		{
			if (!LocalContext.IsMe(this.Entity))
			{
				throw new InvalidOperationException("This should only be called on the local player's creature node!");
			}
			if (remotePlayerFocused)
			{
				this._stateDisplay.AnimateOut();
				return;
			}
			if (this.Entity.IsAlive)
			{
				this._stateDisplay.AnimateIn(HealthBarAnimMode.FromHidden);
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x001E14F9 File Offset: 0x001DF6F9
		public void HideHoverTips()
		{
			NHoverTipSet.Remove(this.Hitbox);
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x001E1506 File Offset: 0x001DF706
		private void SubscribeToPower(PowerModel power)
		{
			power.Flashed += this.OnPowerFlashed;
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x001E151A File Offset: 0x001DF71A
		private void UnsubscribeFromPower(PowerModel power)
		{
			power.Flashed -= this.OnPowerFlashed;
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x001E152E File Offset: 0x001DF72E
		private void OnPowerApplied(PowerModel power)
		{
			this.SubscribeToPower(power);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x001E1538 File Offset: 0x001DF738
		private void OnPowerIncreased(PowerModel power, int amount, bool silent)
		{
			if (silent)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			bool flag = power.GetTypeForAmount(power.Amount) == PowerType.Buff;
			NPowerAppliedVfx vfx = NPowerAppliedVfx.Create(power, amount, flag);
			if (vfx != null)
			{
				if (flag)
				{
					NPowerAppliedBuffVfx buffVfx = NPowerAppliedBuffVfx.Create(this.PowerAppliedVfxSpawnPosition);
					Callable.From(delegate
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance == null)
						{
							return;
						}
						instance.CombatVfxContainer.AddChildSafely(buffVfx);
					}).CallDeferred(Array.Empty<Variant>());
				}
				else
				{
					NPowerAppliedDebuffVfx debuffVfx = NPowerAppliedDebuffVfx.Create(this.PowerAppliedVfxSpawnPosition);
					Callable.From(delegate
					{
						NCombatRoom instance2 = NCombatRoom.Instance;
						if (instance2 == null)
						{
							return;
						}
						instance2.CombatVfxContainer.AddChildSafely(debuffVfx);
					}).CallDeferred(Array.Empty<Variant>());
				}
				Callable.From(delegate
				{
					NCombatRoom instance3 = NCombatRoom.Instance;
					if (instance3 == null)
					{
						return;
					}
					instance3.CombatVfxContainer.AddChildSafely(vfx);
				}).CallDeferred(Array.Empty<Variant>());
			}
			if (power.ShouldPlayVfx)
			{
				SfxCmd.Play(flag ? "event:/sfx/buff" : "event:/sfx/debuff", 1f);
			}
			if (!flag)
			{
				this.AnimShake();
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x001E1644 File Offset: 0x001DF844
		private void OnPowerRemoved(PowerModel power)
		{
			NPowerRemovedVfx vfx = NPowerRemovedVfx.Create(power);
			if (vfx != null)
			{
				Callable.From(delegate
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance == null)
					{
						return;
					}
					instance.CombatVfxContainer.AddChildSafely(vfx);
				}).CallDeferred(Array.Empty<Variant>());
			}
			this.UnsubscribeFromPower(power);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x001E1690 File Offset: 0x001DF890
		private void OnPowerFlashed(PowerModel power)
		{
			NPowerFlashVfx vfx = NPowerFlashVfx.Create(power);
			if (vfx != null)
			{
				Callable.From(delegate
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance == null)
					{
						return;
					}
					instance.CombatVfxContainer.AddChildSafely(vfx);
				}).CallDeferred(Array.Empty<Variant>());
			}
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x001E16D5 File Offset: 0x001DF8D5
		private void OnCombatEnded(CombatRoom _)
		{
			this.AnimHideIntent(0.0);
			NOrbManager orbManager = this.OrbManager;
			if (orbManager == null)
			{
				return;
			}
			orbManager.ClearOrbs();
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x001E16F6 File Offset: 0x001DF8F6
		public void SetAnimationTrigger(string trigger)
		{
			CreatureAnimator spineAnimator = this._spineAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetTrigger(trigger);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x001E170C File Offset: 0x001DF90C
		public float GetCurrentAnimationLength()
		{
			return this.SpineAnimation.GetCurrentAnimationDuration(0).GetValueOrDefault();
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001E1730 File Offset: 0x001DF930
		public float GetCurrentAnimationTimeRemaining()
		{
			float num;
			using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack(0))
			{
				if (currentTrack == null)
				{
					num = 0f;
				}
				else
				{
					num = currentTrack.GetTrackComplete() - currentTrack.GetTrackTime();
				}
			}
			return num;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x001E1784 File Offset: 0x001DF984
		public void ToggleIsInteractable(bool on)
		{
			this.IsInteractable = on;
			this._stateDisplay.Visible = !NCombatUi.IsDebugHidingHpBar && on;
			this.Hitbox.MouseFilter = (on ? 0L : 2L);
			this.Hitbox.FocusMode = (on ? 2L : 0L);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x001E17D5 File Offset: 0x001DF9D5
		public void DisableInteractionForDeath()
		{
			if (this.Hitbox.HasFocus())
			{
				ActiveScreenContext.Instance.FocusOnDefaultControl();
			}
			this.Hitbox.FocusMode = 0L;
			this.Hitbox.MouseFilter = 2L;
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x001E1808 File Offset: 0x001DFA08
		public Tween AnimDisableUi()
		{
			Tween tween = base.CreateTween();
			if (!base.IsNodeReady())
			{
				tween.TweenInterval(0.0);
				return tween;
			}
			tween.TweenProperty(this._stateDisplay, "modulate:a", 0f, 0.5).SetDelay(0.5).SetEase(1L)
				.SetTrans(5L);
			return tween;
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x001E1880 File Offset: 0x001DFA80
		public Tween AnimEnableUi()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._stateDisplay, "modulate:a", 1f, 0.5).SetDelay(0.5).SetEase(1L)
				.SetTrans(5L);
			return tween;
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x001E18DC File Offset: 0x001DFADC
		public float StartDeathAnim(bool shouldRemove)
		{
			this.DisableInteractionForDeath();
			foreach (NIntent nintent in this.IntentContainer.GetChildren(false).OfType<NIntent>())
			{
				nintent.SetFrozen(true);
			}
			Task deathAnimationTask = this.DeathAnimationTask;
			if (deathAnimationTask != null && !deathAnimationTask.IsCompleted)
			{
				return 0f;
			}
			float num = 0f;
			if (this._spineAnimator != null)
			{
				MonsterModel monster = this.Entity.Monster;
				if (monster != null && monster.HasDeathSfx)
				{
					SfxCmd.PlayDeath(this.Entity.Monster);
				}
				if (this.Entity.Player != null)
				{
					SfxCmd.PlayDeath(this.Entity.Player);
				}
				this.SetAnimationTrigger("Dead");
				num = this.GetCurrentAnimationLength();
			}
			this.DeathAnimationTask = this.AnimDie(shouldRemove, this.DeathAnimCancelToken.Token);
			TaskHelper.RunSafely(this.DeathAnimationTask);
			MonsterModel monster2 = this.Entity.Monster;
			if (monster2 != null && monster2.HasDeathAnimLengthOverride)
			{
				return this.Entity.Monster.DeathAnimLengthOverride;
			}
			return Mathf.Min(num, 30f);
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x001E1A14 File Offset: 0x001DFC14
		public void StartReviveAnim()
		{
			CreatureAnimator spineAnimator = this._spineAnimator;
			if (spineAnimator != null && spineAnimator.HasTrigger("Revive"))
			{
				this.SetAnimationTrigger("Revive");
			}
			else if (this.Entity.IsPlayer)
			{
				this.AnimTempRevive();
			}
			if (!this._isRemotePlayerOrPet)
			{
				this.AnimEnableUi();
			}
			this.Hitbox.MouseFilter = 0L;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x001E1A78 File Offset: 0x001DFC78
		private void AnimTempRevive()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this.Visuals, "modulate:a", 0f, 0.2);
			tween.TweenCallback(Callable.From(new Action(this.ImmediatelySetIdle)));
			tween.TweenProperty(this.Visuals, "modulate:a", 1f, 0.2);
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x001E1AF8 File Offset: 0x001DFCF8
		private void ImmediatelySetIdle()
		{
			CreatureAnimator spineAnimator = this._spineAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetTrigger("Idle");
			}
			using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack(0))
			{
				if (currentTrack != null)
				{
					currentTrack.SetMixDuration(0f);
					currentTrack.SetTrackTime(currentTrack.GetAnimationEnd());
				}
			}
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x001E1B64 File Offset: 0x001DFD64
		private async Task AnimDie(bool shouldRemove, CancellationToken cancelToken)
		{
			Tween disableUiTween = this.AnimDisableUi();
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				NOrbManager orbManager = this.OrbManager;
				if (orbManager != null)
				{
					orbManager.ClearOrbs();
				}
			}
			if (shouldRemove)
			{
				this.AnimHideIntent(0.0);
			}
			if (this._spineAnimator != null)
			{
				float num = Math.Min(this.GetCurrentAnimationTimeRemaining() + 0.5f, 20f);
				await Cmd.Wait(num, cancelToken, true);
			}
			else
			{
				MonsterModel monsterModel = this.Entity.Monster;
				if (monsterModel != null && monsterModel.HasDeathAnimLengthOverride)
				{
					await Cmd.Wait(this.Entity.Monster.DeathAnimLengthOverride, cancelToken, true);
				}
			}
			if (!cancelToken.IsCancellationRequested)
			{
				if (shouldRemove)
				{
					Task fadeVfx = null;
					MonsterModel monsterModel = this.Entity.Monster;
					if (monsterModel != null && monsterModel.ShouldFadeAfterDeath && this.Body.IsVisibleInTree())
					{
						NMonsterDeathVfx nmonsterDeathVfx = NMonsterDeathVfx.Create(this, cancelToken);
						Node parent = base.GetParent();
						parent.AddChildSafely(nmonsterDeathVfx);
						if (nmonsterDeathVfx != null)
						{
							parent.MoveChildSafely(nmonsterDeathVfx, base.GetIndex(false));
						}
						fadeVfx = ((nmonsterDeathVfx != null) ? nmonsterDeathVfx.PlayVfx() : null);
					}
					if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
					{
						if (disableUiTween.IsValid() && disableUiTween.IsRunning())
						{
							TaskAwaiter<bool> taskAwaiter = disableUiTween.AwaitFinished(this).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (!taskAwaiter.GetResult())
							{
								return;
							}
						}
						foreach (IDeathDelayer deathDelayer in this.GetChildrenRecursive<IDeathDelayer>())
						{
							await deathDelayer.GetDelayTask();
						}
						IEnumerator<IDeathDelayer> enumerator = null;
					}
					if (fadeVfx != null)
					{
						await fadeVfx;
					}
					this.QueueFreeSafely();
				}
				else if (this.Entity.Monster is Osty)
				{
					this.OstyScaleToSize(0f, 0.75);
				}
			}
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x001E1BB8 File Offset: 0x001DFDB8
		public void AnimHideIntent(double delay = 0.0)
		{
			Tween intentFadeTween = this._intentFadeTween;
			if (intentFadeTween != null)
			{
				intentFadeTween.Kill();
			}
			this._intentFadeTween = base.CreateTween().SetParallel(true);
			PropertyTweener propertyTweener = this._intentFadeTween.TweenProperty(this.IntentContainer, "modulate:a", 0f, 0.5);
			if (delay > 0.0)
			{
				propertyTweener.SetDelay(delay);
			}
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x001E1C2B File Offset: 0x001DFE2B
		public void SetScaleAndHue(float scale, float hue)
		{
			this.Visuals.SetScaleAndHue(scale, hue);
			this.UpdateBounds(this.Visuals);
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x001E1C48 File Offset: 0x001DFE48
		public void ScaleTo(float size, double duration)
		{
			if (this.Entity.IsMonster && !this.Entity.Monster.CanChangeScale)
			{
				return;
			}
			this._tempScale = size;
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenMethod(Callable.From<Vector2>(new Action<Vector2>(this.DoScaleTween)), this.Visuals.Scale, Vector2.One * this._tempScale * this.Visuals.DefaultScale, duration).SetEase(2L).SetTrans(1L);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x001E1CFA File Offset: 0x001DFEFA
		public void SetDefaultScaleTo(float size, float duration)
		{
			if (this.Entity.IsMonster && !this.Entity.Monster.CanChangeScale)
			{
				return;
			}
			this.Visuals.DefaultScale = size;
			this.ScaleTo(this._tempScale, (double)duration);
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x001E1D38 File Offset: 0x001DFF38
		public void OstyScaleToSize(float ostyHealth, double duration)
		{
			float num = Mathf.Lerp(Osty.ScaleRange.X, Osty.ScaleRange.Y, Mathf.Clamp(ostyHealth / 150f, 0f, 1f));
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this.Entity.PetOwner.Creature) : null);
			this._scaleTween = base.CreateTween();
			Vector2 vector = Vector2.One * num * this.Visuals.DefaultScale;
			if (NCombatRoom.Instance != null && !NCombatRoom.Instance.SceneContainer.IsAncestorOf(this.Visuals))
			{
				ICombatState combatState = this.Entity.CombatState;
				if (combatState != null && combatState.Encounter != null)
				{
					vector *= this.Entity.CombatState.Encounter.GetCameraScaling();
				}
			}
			this._scaleTween.TweenProperty(this.Visuals, "scale", vector, duration).SetEase(2L).SetTrans(1L);
			if (LocalContext.IsMe(this.Entity.PetOwner))
			{
				this._scaleTween.Parallel().TweenProperty(this, "position", ncreature.Position + NCreature.GetOstyOffsetFromPlayer(this.Entity), duration);
			}
			this._scaleTween.TweenCallback(Callable.From(delegate
			{
				this.UpdateBounds(this.Visuals);
			}));
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x001E1EA8 File Offset: 0x001E00A8
		public static Vector2 GetOstyOffsetFromPlayer(Creature osty)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(osty.PetOwner.Creature) : null);
			return Vector2.Right * ncreature.Hitbox.Size.X * 0.5f + Osty.MinOffset.Lerp(Osty.MaxOffset, Mathf.Clamp((float)osty.MaxHp / 150f, 0f, 1f));
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x001E1F2C File Offset: 0x001E012C
		public void AnimShake()
		{
			if (!base.IsInsideTree())
			{
				return;
			}
			if (this._shakeTween != null && this._shakeTween.IsRunning())
			{
				return;
			}
			if (this.Visuals.IsPlayingHurtAnimation())
			{
				return;
			}
			this.Visuals.Position = Vector2.Zero;
			this._shakeTween = base.CreateTween();
			this._shakeTween.TweenMethod(Callable.From<float>(delegate(float t)
			{
				this.Visuals.Position = Vector2.Right * 10f * Mathf.Sin(t * 4f) * Mathf.Sin(t * 0.5f);
			}), 0f, 6.2831855f, 1.0).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x001E1FCA File Offset: 0x001E01CA
		private void DoScaleTween(Vector2 scale)
		{
			this.Visuals.Scale = scale;
			this.SetOrbManagerPosition();
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x001E1FE0 File Offset: 0x001E01E0
		private void SetOrbManagerPosition()
		{
			if (this.OrbManager == null)
			{
				return;
			}
			this.OrbManager.Scale = ((this.Visuals.Scale.X > 1f) ? Vector2.One : this.Visuals.Scale.Lerp(Vector2.One, 0.5f));
			this.OrbManager.Position = this.Visuals.OrbPosition.Position * Mathf.Min(this.Visuals.Scale.X, 1.25f);
			if (!this.OrbManager.IsLocal)
			{
				this.OrbManager.Position += Vector2.Up * 50f;
			}
		}

		/// <summary>
		/// Helper function to get the top of this creature's hitbox position.
		/// Used for spawning vfx or aligning UI elements to a creature's hitbox.
		/// </summary>
		// Token: 0x06003F0B RID: 16139 RVA: 0x001E20A8 File Offset: 0x001E02A8
		public Vector2 GetTopOfHitbox()
		{
			return this.Hitbox.GlobalPosition + new Vector2(this.Hitbox.Size.X * 0.5f, 0f);
		}

		/// <summary>
		/// Helper function to get the top of this creature's hitbox position.
		/// Used for spawning vfx or aligning UI elements to a creature's hitbox.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06003F0C RID: 16140 RVA: 0x001E20DA File Offset: 0x001E02DA
		public Vector2 GetBottomOfHitbox()
		{
			return this.Hitbox.GlobalPosition + new Vector2(this.Hitbox.Size.X * 0.5f, this.Hitbox.Size.Y);
		}

		/// <summary>
		/// Track the block status of another creature.
		/// Used for pets who want to show extra UI when their owner has block.
		/// For example, Osty tracks Necrobinder's block.
		/// </summary>
		/// <param name="creature">Extra creature (different from _creature) whose block status we want to track in this UI.</param>
		// Token: 0x06003F0D RID: 16141 RVA: 0x001E2117 File Offset: 0x001E0317
		public void TrackBlockStatus(Creature creature)
		{
			this._stateDisplay.TrackBlockStatus(creature);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x001E2125 File Offset: 0x001E0325
		public void ShowMultiselectReticle()
		{
			this._isInMultiselect = true;
			this.ShowSingleSelectReticle();
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x001E2134 File Offset: 0x001E0334
		public void HideMultiselectReticle()
		{
			this._isInMultiselect = false;
			this.HideSingleSelectReticle();
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x001E2143 File Offset: 0x001E0343
		public void ShowSingleSelectReticle()
		{
			this._selectionReticle.OnSelect();
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x001E2150 File Offset: 0x001E0350
		public void HideSingleSelectReticle()
		{
			if (this._isInMultiselect)
			{
				return;
			}
			this._selectionReticle.OnDeselect();
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x001E2166 File Offset: 0x001E0366
		public void SetupForBestiary()
		{
			this._stateDisplay.Visible = false;
			this.IntentContainer.Visible = false;
			this._isInBestiary = true;
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x001E2187 File Offset: 0x001E0387
		public void StartSfxLoop(string sfxName)
		{
			this.StartSfxLoop(sfxName, "loop", 1f);
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x001E219A File Offset: 0x001E039A
		public void StartSfxLoop(string sfxName, string loopParam, float loopStopValue)
		{
			if (this._sfxLoops.ContainsKey(sfxName))
			{
				return;
			}
			this._sfxLoops.Add(sfxName, new ValueTuple<string, float>(loopParam, loopStopValue));
			SfxCmd.PlayLoop(sfxName, false);
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x001E21C8 File Offset: 0x001E03C8
		public void StopSfxLoop(string sfxName)
		{
			ValueTuple<string, float> valueTuple;
			if (!this._sfxLoops.TryGetValue(sfxName, out valueTuple))
			{
				return;
			}
			SfxCmd.SetParam(sfxName, valueTuple.Item1, valueTuple.Item2);
			SfxCmd.StopLoop(sfxName);
			this._sfxLoops.Remove(sfxName);
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x001E220C File Offset: 0x001E040C
		public void StopAllSfxLoops()
		{
			List<string> list = this._sfxLoops.Keys.ToList<string>();
			foreach (string text in list)
			{
				this.StopSfxLoop(text);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F17 RID: 16151 RVA: 0x001E226C File Offset: 0x001E046C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(42)
			{
				new MethodInfo(NCreature.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.ConnectSpineAnimatorSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.UpdateBounds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "boundsNodeName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.UpdatePhobiaMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.OnTargetingStarted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.SetRemotePlayerFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "remotePlayerFocused", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.HideHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.SetAnimationTrigger, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "trigger", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.GetCurrentAnimationLength, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.GetCurrentAnimationTimeRemaining, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.ToggleIsInteractable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "on", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.DisableInteractionForDeath, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.AnimDisableUi, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Tween"), false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.AnimEnableUi, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Tween"), false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.StartDeathAnim, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "shouldRemove", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.StartReviveAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.AnimTempRevive, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.ImmediatelySetIdle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.AnimHideIntent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delay", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.SetScaleAndHue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "scale", 0L, "", 6L, false),
					new PropertyInfo(3L, "hue", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.ScaleTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "size", 0L, "", 6L, false),
					new PropertyInfo(3L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.SetDefaultScaleTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "size", 0L, "", 6L, false),
					new PropertyInfo(3L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.OstyScaleToSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "ostyHealth", 0L, "", 6L, false),
					new PropertyInfo(3L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.AnimShake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.DoScaleTween, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "scale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.SetOrbManagerPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.GetTopOfHitbox, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.GetBottomOfHitbox, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.ShowMultiselectReticle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.HideMultiselectReticle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.ShowSingleSelectReticle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.HideSingleSelectReticle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.SetupForBestiary, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreature.MethodName.StartSfxLoop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "sfxName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.StartSfxLoop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "sfxName", 0L, "", 6L, false),
					new PropertyInfo(4L, "loopParam", 0L, "", 6L, false),
					new PropertyInfo(3L, "loopStopValue", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.StopSfxLoop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "sfxName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreature.MethodName.StopAllSfxLoops, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F18 RID: 16152 RVA: 0x001E2D1C File Offset: 0x001E0F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreature.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ConnectSpineAnimatorSignals && args.Count == 0)
			{
				this.ConnectSpineAnimatorSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.UpdateBounds && args.Count == 1)
			{
				this.UpdateBounds(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.UpdatePhobiaMode && args.Count == 0)
			{
				this.UpdatePhobiaMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.OnTargetingStarted && args.Count == 0)
			{
				this.OnTargetingStarted();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetRemotePlayerFocused && args.Count == 1)
			{
				this.SetRemotePlayerFocused(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.HideHoverTips && args.Count == 0)
			{
				this.HideHoverTips();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetAnimationTrigger && args.Count == 1)
			{
				this.SetAnimationTrigger(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.GetCurrentAnimationLength && args.Count == 0)
			{
				float currentAnimationLength = this.GetCurrentAnimationLength();
				ret = VariantUtils.CreateFrom<float>(ref currentAnimationLength);
				return true;
			}
			if ((ref method) == NCreature.MethodName.GetCurrentAnimationTimeRemaining && args.Count == 0)
			{
				float currentAnimationTimeRemaining = this.GetCurrentAnimationTimeRemaining();
				ret = VariantUtils.CreateFrom<float>(ref currentAnimationTimeRemaining);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ToggleIsInteractable && args.Count == 1)
			{
				this.ToggleIsInteractable(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.DisableInteractionForDeath && args.Count == 0)
			{
				this.DisableInteractionForDeath();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.AnimDisableUi && args.Count == 0)
			{
				Tween tween = this.AnimDisableUi();
				ret = VariantUtils.CreateFrom<Tween>(ref tween);
				return true;
			}
			if ((ref method) == NCreature.MethodName.AnimEnableUi && args.Count == 0)
			{
				Tween tween2 = this.AnimEnableUi();
				ret = VariantUtils.CreateFrom<Tween>(ref tween2);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StartDeathAnim && args.Count == 1)
			{
				float num = this.StartDeathAnim(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StartReviveAnim && args.Count == 0)
			{
				this.StartReviveAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.AnimTempRevive && args.Count == 0)
			{
				this.AnimTempRevive();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ImmediatelySetIdle && args.Count == 0)
			{
				this.ImmediatelySetIdle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.AnimHideIntent && args.Count == 1)
			{
				this.AnimHideIntent(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetScaleAndHue && args.Count == 2)
			{
				this.SetScaleAndHue(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ScaleTo && args.Count == 2)
			{
				this.ScaleTo(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<double>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetDefaultScaleTo && args.Count == 2)
			{
				this.SetDefaultScaleTo(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.OstyScaleToSize && args.Count == 2)
			{
				this.OstyScaleToSize(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<double>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.AnimShake && args.Count == 0)
			{
				this.AnimShake();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.DoScaleTween && args.Count == 1)
			{
				this.DoScaleTween(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetOrbManagerPosition && args.Count == 0)
			{
				this.SetOrbManagerPosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.GetTopOfHitbox && args.Count == 0)
			{
				Vector2 topOfHitbox = this.GetTopOfHitbox();
				ret = VariantUtils.CreateFrom<Vector2>(ref topOfHitbox);
				return true;
			}
			if ((ref method) == NCreature.MethodName.GetBottomOfHitbox && args.Count == 0)
			{
				Vector2 bottomOfHitbox = this.GetBottomOfHitbox();
				ret = VariantUtils.CreateFrom<Vector2>(ref bottomOfHitbox);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ShowMultiselectReticle && args.Count == 0)
			{
				this.ShowMultiselectReticle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.HideMultiselectReticle && args.Count == 0)
			{
				this.HideMultiselectReticle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.ShowSingleSelectReticle && args.Count == 0)
			{
				this.ShowSingleSelectReticle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.HideSingleSelectReticle && args.Count == 0)
			{
				this.HideSingleSelectReticle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.SetupForBestiary && args.Count == 0)
			{
				this.SetupForBestiary();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StartSfxLoop && args.Count == 1)
			{
				this.StartSfxLoop(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StartSfxLoop && args.Count == 3)
			{
				this.StartSfxLoop(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<float>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StopSfxLoop && args.Count == 1)
			{
				this.StopSfxLoop(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreature.MethodName.StopAllSfxLoops && args.Count == 0)
			{
				this.StopAllSfxLoops();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003F19 RID: 16153 RVA: 0x001E348C File Offset: 0x001E168C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCreature.MethodName._Ready || (ref method) == NCreature.MethodName._EnterTree || (ref method) == NCreature.MethodName._ExitTree || (ref method) == NCreature.MethodName.ConnectSpineAnimatorSignals || (ref method) == NCreature.MethodName.UpdateBounds || (ref method) == NCreature.MethodName.UpdatePhobiaMode || (ref method) == NCreature.MethodName.UpdateNavigation || (ref method) == NCreature.MethodName.OnFocus || (ref method) == NCreature.MethodName.OnUnfocus || (ref method) == NCreature.MethodName.OnTargetingStarted || (ref method) == NCreature.MethodName.SetRemotePlayerFocused || (ref method) == NCreature.MethodName.HideHoverTips || (ref method) == NCreature.MethodName.SetAnimationTrigger || (ref method) == NCreature.MethodName.GetCurrentAnimationLength || (ref method) == NCreature.MethodName.GetCurrentAnimationTimeRemaining || (ref method) == NCreature.MethodName.ToggleIsInteractable || (ref method) == NCreature.MethodName.DisableInteractionForDeath || (ref method) == NCreature.MethodName.AnimDisableUi || (ref method) == NCreature.MethodName.AnimEnableUi || (ref method) == NCreature.MethodName.StartDeathAnim || (ref method) == NCreature.MethodName.StartReviveAnim || (ref method) == NCreature.MethodName.AnimTempRevive || (ref method) == NCreature.MethodName.ImmediatelySetIdle || (ref method) == NCreature.MethodName.AnimHideIntent || (ref method) == NCreature.MethodName.SetScaleAndHue || (ref method) == NCreature.MethodName.ScaleTo || (ref method) == NCreature.MethodName.SetDefaultScaleTo || (ref method) == NCreature.MethodName.OstyScaleToSize || (ref method) == NCreature.MethodName.AnimShake || (ref method) == NCreature.MethodName.DoScaleTween || (ref method) == NCreature.MethodName.SetOrbManagerPosition || (ref method) == NCreature.MethodName.GetTopOfHitbox || (ref method) == NCreature.MethodName.GetBottomOfHitbox || (ref method) == NCreature.MethodName.ShowMultiselectReticle || (ref method) == NCreature.MethodName.HideMultiselectReticle || (ref method) == NCreature.MethodName.ShowSingleSelectReticle || (ref method) == NCreature.MethodName.HideSingleSelectReticle || (ref method) == NCreature.MethodName.SetupForBestiary || (ref method) == NCreature.MethodName.StartSfxLoop || (ref method) == NCreature.MethodName.StopSfxLoop || (ref method) == NCreature.MethodName.StopAllSfxLoops || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003F1A RID: 16154 RVA: 0x001E3708 File Offset: 0x001E1908
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCreature.PropertyName.Hitbox)
			{
				this.Hitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.OrbManager)
			{
				this.OrbManager = VariantUtils.ConvertTo<NOrbManager>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IsInteractable)
			{
				this.IsInteractable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.Visuals)
			{
				this.Visuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IntentContainer)
			{
				this.IntentContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IsFocused)
			{
				this.IsFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.PlayerIntentHandler)
			{
				this.PlayerIntentHandler = VariantUtils.ConvertTo<NMultiplayerPlayerIntentHandler>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._stateDisplay)
			{
				this._stateDisplay = VariantUtils.ConvertTo<NCreatureStateDisplay>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._intentFadeTween)
			{
				this._intentFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._shakeTween)
			{
				this._shakeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isRemotePlayerOrPet)
			{
				this._isRemotePlayerOrPet = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isInBestiary)
			{
				this._isInBestiary = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._tempScale)
			{
				this._tempScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isInMultiselect)
			{
				this._isInMultiselect = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003F1B RID: 16155 RVA: 0x001E38D0 File Offset: 0x001E1AD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCreature.PropertyName.Hitbox)
			{
				Control control = this.Hitbox;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.OrbManager)
			{
				NOrbManager orbManager = this.OrbManager;
				value = VariantUtils.CreateFrom<NOrbManager>(ref orbManager);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IsInteractable)
			{
				bool flag = this.IsInteractable;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.VfxSpawnPosition)
			{
				Vector2 vector = this.VfxSpawnPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.PowerAppliedVfxSpawnPosition)
			{
				Vector2 vector = this.PowerAppliedVfxSpawnPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.Visuals)
			{
				NCreatureVisuals visuals = this.Visuals;
				value = VariantUtils.CreateFrom<NCreatureVisuals>(ref visuals);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.Body)
			{
				Node2D body = this.Body;
				value = VariantUtils.CreateFrom<Node2D>(ref body);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IntentContainer)
			{
				Control control = this.IntentContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IsPlayingDeathAnimation)
			{
				bool flag = this.IsPlayingDeathAnimation;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.HasSpineAnimation)
			{
				bool flag = this.HasSpineAnimation;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.IsFocused)
			{
				bool flag = this.IsFocused;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreature.PropertyName.PlayerIntentHandler)
			{
				NMultiplayerPlayerIntentHandler playerIntentHandler = this.PlayerIntentHandler;
				value = VariantUtils.CreateFrom<NMultiplayerPlayerIntentHandler>(ref playerIntentHandler);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._stateDisplay)
			{
				value = VariantUtils.CreateFrom<NCreatureStateDisplay>(ref this._stateDisplay);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._intentFadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._intentFadeTween);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._shakeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._shakeTween);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isRemotePlayerOrPet)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isRemotePlayerOrPet);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isInBestiary)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInBestiary);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._tempScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._tempScale);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._isInMultiselect)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInMultiselect);
				return true;
			}
			if ((ref name) == NCreature.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F1C RID: 16156 RVA: 0x001E3BAC File Offset: 0x001E1DAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCreature.PropertyName._stateDisplay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName._intentFadeTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName._shakeTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName._isRemotePlayerOrPet, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName._isInBestiary, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCreature.PropertyName._tempScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.OrbManager, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName._isInMultiselect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName.IsInteractable, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCreature.PropertyName.VfxSpawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCreature.PropertyName.PowerAppliedVfxSpawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.Visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.Body, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.IntentContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName.IsPlayingDeathAnimation, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName.HasSpineAnimation, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreature.PropertyName.IsFocused, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreature.PropertyName.PlayerIntentHandler, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F1D RID: 16157 RVA: 0x001E3E6C File Offset: 0x001E206C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hitbox = NCreature.PropertyName.Hitbox;
			Control control = this.Hitbox;
			info.AddProperty(hitbox, Variant.From<Control>(ref control));
			StringName orbManager = NCreature.PropertyName.OrbManager;
			NOrbManager orbManager2 = this.OrbManager;
			info.AddProperty(orbManager, Variant.From<NOrbManager>(ref orbManager2));
			StringName isInteractable = NCreature.PropertyName.IsInteractable;
			bool flag = this.IsInteractable;
			info.AddProperty(isInteractable, Variant.From<bool>(ref flag));
			StringName visuals = NCreature.PropertyName.Visuals;
			NCreatureVisuals visuals2 = this.Visuals;
			info.AddProperty(visuals, Variant.From<NCreatureVisuals>(ref visuals2));
			StringName intentContainer = NCreature.PropertyName.IntentContainer;
			control = this.IntentContainer;
			info.AddProperty(intentContainer, Variant.From<Control>(ref control));
			StringName isFocused = NCreature.PropertyName.IsFocused;
			flag = this.IsFocused;
			info.AddProperty(isFocused, Variant.From<bool>(ref flag));
			StringName playerIntentHandler = NCreature.PropertyName.PlayerIntentHandler;
			NMultiplayerPlayerIntentHandler playerIntentHandler2 = this.PlayerIntentHandler;
			info.AddProperty(playerIntentHandler, Variant.From<NMultiplayerPlayerIntentHandler>(ref playerIntentHandler2));
			info.AddProperty(NCreature.PropertyName._stateDisplay, Variant.From<NCreatureStateDisplay>(ref this._stateDisplay));
			info.AddProperty(NCreature.PropertyName._intentFadeTween, Variant.From<Tween>(ref this._intentFadeTween));
			info.AddProperty(NCreature.PropertyName._shakeTween, Variant.From<Tween>(ref this._shakeTween));
			info.AddProperty(NCreature.PropertyName._isRemotePlayerOrPet, Variant.From<bool>(ref this._isRemotePlayerOrPet));
			info.AddProperty(NCreature.PropertyName._isInBestiary, Variant.From<bool>(ref this._isInBestiary));
			info.AddProperty(NCreature.PropertyName._tempScale, Variant.From<float>(ref this._tempScale));
			info.AddProperty(NCreature.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NCreature.PropertyName._isInMultiselect, Variant.From<bool>(ref this._isInMultiselect));
			info.AddProperty(NCreature.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x06003F1E RID: 16158 RVA: 0x001E3FF8 File Offset: 0x001E21F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCreature.PropertyName.Hitbox, ref variant))
			{
				this.Hitbox = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCreature.PropertyName.OrbManager, ref variant2))
			{
				this.OrbManager = variant2.As<NOrbManager>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCreature.PropertyName.IsInteractable, ref variant3))
			{
				this.IsInteractable = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCreature.PropertyName.Visuals, ref variant4))
			{
				this.Visuals = variant4.As<NCreatureVisuals>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCreature.PropertyName.IntentContainer, ref variant5))
			{
				this.IntentContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCreature.PropertyName.IsFocused, ref variant6))
			{
				this.IsFocused = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCreature.PropertyName.PlayerIntentHandler, ref variant7))
			{
				this.PlayerIntentHandler = variant7.As<NMultiplayerPlayerIntentHandler>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCreature.PropertyName._stateDisplay, ref variant8))
			{
				this._stateDisplay = variant8.As<NCreatureStateDisplay>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCreature.PropertyName._intentFadeTween, ref variant9))
			{
				this._intentFadeTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCreature.PropertyName._shakeTween, ref variant10))
			{
				this._shakeTween = variant10.As<Tween>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCreature.PropertyName._isRemotePlayerOrPet, ref variant11))
			{
				this._isRemotePlayerOrPet = variant11.As<bool>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCreature.PropertyName._isInBestiary, ref variant12))
			{
				this._isInBestiary = variant12.As<bool>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCreature.PropertyName._tempScale, ref variant13))
			{
				this._tempScale = variant13.As<float>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCreature.PropertyName._scaleTween, ref variant14))
			{
				this._scaleTween = variant14.As<Tween>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCreature.PropertyName._isInMultiselect, ref variant15))
			{
				this._isInMultiselect = variant15.As<bool>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCreature.PropertyName._selectionReticle, ref variant16))
			{
				this._selectionReticle = variant16.As<NSelectionReticle>();
			}
		}

		// Token: 0x0400185F RID: 6239
		private static readonly string _scenePath = SceneHelper.GetScenePath("combat/creature");

		// Token: 0x04001860 RID: 6240
		private NCreatureStateDisplay _stateDisplay;

		// Token: 0x04001861 RID: 6241
		[Nullable(2)]
		private Tween _intentFadeTween;

		// Token: 0x04001862 RID: 6242
		[Nullable(2)]
		private Tween _shakeTween;

		// Token: 0x04001863 RID: 6243
		[Nullable(2)]
		private CreatureAnimator _spineAnimator;

		// Token: 0x04001864 RID: 6244
		private bool _isRemotePlayerOrPet;

		// Token: 0x04001865 RID: 6245
		private bool _isInBestiary;

		// Token: 0x04001866 RID: 6246
		private float _tempScale = 1f;

		// Token: 0x04001867 RID: 6247
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x0400186C RID: 6252
		private bool _isInMultiselect;

		// Token: 0x0400186D RID: 6253
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Keeps track of the looping sfx that are owned by this creature (ie thieving hopper's fluttering).
		/// We use this as a way to easily cancel all looping sfx when a creature instance is killed or despawned.
		/// - The Key is the string field of the sfx we are tracking (ie event://flying_sfx)
		///
		/// - The Value is a Tuple, where the first item is the loop param we will use to end the sfx and the second item
		///   is the value that we need to set that param to end the sfx. (ie ("loop", 1)).
		///   We need to do this because not all sfx loops use the same loop param or loop value to finish the FMOD event
		///   (ie waterfall giant uses the same loop param to ramp up how intense it is AND to end the loop).
		/// </summary>
		// Token: 0x04001872 RID: 6258
		[Nullable(new byte[] { 1, 1, 0, 1 })]
		private readonly Dictionary<string, ValueTuple<string, float>> _sfxLoops = new Dictionary<string, ValueTuple<string, float>>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018C3 RID: 6339
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040060C5 RID: 24773
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040060C6 RID: 24774
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040060C7 RID: 24775
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectSpineAnimatorSignals' method.
			/// </summary>
			// Token: 0x040060C8 RID: 24776
			public static readonly StringName ConnectSpineAnimatorSignals = "ConnectSpineAnimatorSignals";

			/// <summary>
			/// Cached name for the 'UpdateBounds' method.
			/// </summary>
			// Token: 0x040060C9 RID: 24777
			public static readonly StringName UpdateBounds = "UpdateBounds";

			/// <summary>
			/// Cached name for the 'UpdatePhobiaMode' method.
			/// </summary>
			// Token: 0x040060CA RID: 24778
			public static readonly StringName UpdatePhobiaMode = "UpdatePhobiaMode";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x040060CB RID: 24779
			public static readonly StringName UpdateNavigation = "UpdateNavigation";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040060CC RID: 24780
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040060CD RID: 24781
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnTargetingStarted' method.
			/// </summary>
			// Token: 0x040060CE RID: 24782
			public static readonly StringName OnTargetingStarted = "OnTargetingStarted";

			/// <summary>
			/// Cached name for the 'SetRemotePlayerFocused' method.
			/// </summary>
			// Token: 0x040060CF RID: 24783
			public static readonly StringName SetRemotePlayerFocused = "SetRemotePlayerFocused";

			/// <summary>
			/// Cached name for the 'HideHoverTips' method.
			/// </summary>
			// Token: 0x040060D0 RID: 24784
			public static readonly StringName HideHoverTips = "HideHoverTips";

			/// <summary>
			/// Cached name for the 'SetAnimationTrigger' method.
			/// </summary>
			// Token: 0x040060D1 RID: 24785
			public static readonly StringName SetAnimationTrigger = "SetAnimationTrigger";

			/// <summary>
			/// Cached name for the 'GetCurrentAnimationLength' method.
			/// </summary>
			// Token: 0x040060D2 RID: 24786
			public static readonly StringName GetCurrentAnimationLength = "GetCurrentAnimationLength";

			/// <summary>
			/// Cached name for the 'GetCurrentAnimationTimeRemaining' method.
			/// </summary>
			// Token: 0x040060D3 RID: 24787
			public static readonly StringName GetCurrentAnimationTimeRemaining = "GetCurrentAnimationTimeRemaining";

			/// <summary>
			/// Cached name for the 'ToggleIsInteractable' method.
			/// </summary>
			// Token: 0x040060D4 RID: 24788
			public static readonly StringName ToggleIsInteractable = "ToggleIsInteractable";

			/// <summary>
			/// Cached name for the 'DisableInteractionForDeath' method.
			/// </summary>
			// Token: 0x040060D5 RID: 24789
			public static readonly StringName DisableInteractionForDeath = "DisableInteractionForDeath";

			/// <summary>
			/// Cached name for the 'AnimDisableUi' method.
			/// </summary>
			// Token: 0x040060D6 RID: 24790
			public static readonly StringName AnimDisableUi = "AnimDisableUi";

			/// <summary>
			/// Cached name for the 'AnimEnableUi' method.
			/// </summary>
			// Token: 0x040060D7 RID: 24791
			public static readonly StringName AnimEnableUi = "AnimEnableUi";

			/// <summary>
			/// Cached name for the 'StartDeathAnim' method.
			/// </summary>
			// Token: 0x040060D8 RID: 24792
			public static readonly StringName StartDeathAnim = "StartDeathAnim";

			/// <summary>
			/// Cached name for the 'StartReviveAnim' method.
			/// </summary>
			// Token: 0x040060D9 RID: 24793
			public static readonly StringName StartReviveAnim = "StartReviveAnim";

			/// <summary>
			/// Cached name for the 'AnimTempRevive' method.
			/// </summary>
			// Token: 0x040060DA RID: 24794
			public static readonly StringName AnimTempRevive = "AnimTempRevive";

			/// <summary>
			/// Cached name for the 'ImmediatelySetIdle' method.
			/// </summary>
			// Token: 0x040060DB RID: 24795
			public static readonly StringName ImmediatelySetIdle = "ImmediatelySetIdle";

			/// <summary>
			/// Cached name for the 'AnimHideIntent' method.
			/// </summary>
			// Token: 0x040060DC RID: 24796
			public static readonly StringName AnimHideIntent = "AnimHideIntent";

			/// <summary>
			/// Cached name for the 'SetScaleAndHue' method.
			/// </summary>
			// Token: 0x040060DD RID: 24797
			public static readonly StringName SetScaleAndHue = "SetScaleAndHue";

			/// <summary>
			/// Cached name for the 'ScaleTo' method.
			/// </summary>
			// Token: 0x040060DE RID: 24798
			public static readonly StringName ScaleTo = "ScaleTo";

			/// <summary>
			/// Cached name for the 'SetDefaultScaleTo' method.
			/// </summary>
			// Token: 0x040060DF RID: 24799
			public static readonly StringName SetDefaultScaleTo = "SetDefaultScaleTo";

			/// <summary>
			/// Cached name for the 'OstyScaleToSize' method.
			/// </summary>
			// Token: 0x040060E0 RID: 24800
			public static readonly StringName OstyScaleToSize = "OstyScaleToSize";

			/// <summary>
			/// Cached name for the 'AnimShake' method.
			/// </summary>
			// Token: 0x040060E1 RID: 24801
			public static readonly StringName AnimShake = "AnimShake";

			/// <summary>
			/// Cached name for the 'DoScaleTween' method.
			/// </summary>
			// Token: 0x040060E2 RID: 24802
			public static readonly StringName DoScaleTween = "DoScaleTween";

			/// <summary>
			/// Cached name for the 'SetOrbManagerPosition' method.
			/// </summary>
			// Token: 0x040060E3 RID: 24803
			public static readonly StringName SetOrbManagerPosition = "SetOrbManagerPosition";

			/// <summary>
			/// Cached name for the 'GetTopOfHitbox' method.
			/// </summary>
			// Token: 0x040060E4 RID: 24804
			public static readonly StringName GetTopOfHitbox = "GetTopOfHitbox";

			/// <summary>
			/// Cached name for the 'GetBottomOfHitbox' method.
			/// </summary>
			// Token: 0x040060E5 RID: 24805
			public static readonly StringName GetBottomOfHitbox = "GetBottomOfHitbox";

			/// <summary>
			/// Cached name for the 'ShowMultiselectReticle' method.
			/// </summary>
			// Token: 0x040060E6 RID: 24806
			public static readonly StringName ShowMultiselectReticle = "ShowMultiselectReticle";

			/// <summary>
			/// Cached name for the 'HideMultiselectReticle' method.
			/// </summary>
			// Token: 0x040060E7 RID: 24807
			public static readonly StringName HideMultiselectReticle = "HideMultiselectReticle";

			/// <summary>
			/// Cached name for the 'ShowSingleSelectReticle' method.
			/// </summary>
			// Token: 0x040060E8 RID: 24808
			public static readonly StringName ShowSingleSelectReticle = "ShowSingleSelectReticle";

			/// <summary>
			/// Cached name for the 'HideSingleSelectReticle' method.
			/// </summary>
			// Token: 0x040060E9 RID: 24809
			public static readonly StringName HideSingleSelectReticle = "HideSingleSelectReticle";

			/// <summary>
			/// Cached name for the 'SetupForBestiary' method.
			/// </summary>
			// Token: 0x040060EA RID: 24810
			public static readonly StringName SetupForBestiary = "SetupForBestiary";

			/// <summary>
			/// Cached name for the 'StartSfxLoop' method.
			/// </summary>
			// Token: 0x040060EB RID: 24811
			public static readonly StringName StartSfxLoop = "StartSfxLoop";

			/// <summary>
			/// Cached name for the 'StopSfxLoop' method.
			/// </summary>
			// Token: 0x040060EC RID: 24812
			public static readonly StringName StopSfxLoop = "StopSfxLoop";

			/// <summary>
			/// Cached name for the 'StopAllSfxLoops' method.
			/// </summary>
			// Token: 0x040060ED RID: 24813
			public static readonly StringName StopAllSfxLoops = "StopAllSfxLoops";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018C4 RID: 6340
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x040060EE RID: 24814
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the 'OrbManager' property.
			/// </summary>
			// Token: 0x040060EF RID: 24815
			public static readonly StringName OrbManager = "OrbManager";

			/// <summary>
			/// Cached name for the 'IsInteractable' property.
			/// </summary>
			// Token: 0x040060F0 RID: 24816
			public static readonly StringName IsInteractable = "IsInteractable";

			/// <summary>
			/// Cached name for the 'VfxSpawnPosition' property.
			/// </summary>
			// Token: 0x040060F1 RID: 24817
			public static readonly StringName VfxSpawnPosition = "VfxSpawnPosition";

			/// <summary>
			/// Cached name for the 'PowerAppliedVfxSpawnPosition' property.
			/// </summary>
			// Token: 0x040060F2 RID: 24818
			public static readonly StringName PowerAppliedVfxSpawnPosition = "PowerAppliedVfxSpawnPosition";

			/// <summary>
			/// Cached name for the 'Visuals' property.
			/// </summary>
			// Token: 0x040060F3 RID: 24819
			public static readonly StringName Visuals = "Visuals";

			/// <summary>
			/// Cached name for the 'Body' property.
			/// </summary>
			// Token: 0x040060F4 RID: 24820
			public static readonly StringName Body = "Body";

			/// <summary>
			/// Cached name for the 'IntentContainer' property.
			/// </summary>
			// Token: 0x040060F5 RID: 24821
			public static readonly StringName IntentContainer = "IntentContainer";

			/// <summary>
			/// Cached name for the 'IsPlayingDeathAnimation' property.
			/// </summary>
			// Token: 0x040060F6 RID: 24822
			public static readonly StringName IsPlayingDeathAnimation = "IsPlayingDeathAnimation";

			/// <summary>
			/// Cached name for the 'HasSpineAnimation' property.
			/// </summary>
			// Token: 0x040060F7 RID: 24823
			public static readonly StringName HasSpineAnimation = "HasSpineAnimation";

			/// <summary>
			/// Cached name for the 'IsFocused' property.
			/// </summary>
			// Token: 0x040060F8 RID: 24824
			public static readonly StringName IsFocused = "IsFocused";

			/// <summary>
			/// Cached name for the 'PlayerIntentHandler' property.
			/// </summary>
			// Token: 0x040060F9 RID: 24825
			public static readonly StringName PlayerIntentHandler = "PlayerIntentHandler";

			/// <summary>
			/// Cached name for the '_stateDisplay' field.
			/// </summary>
			// Token: 0x040060FA RID: 24826
			public static readonly StringName _stateDisplay = "_stateDisplay";

			/// <summary>
			/// Cached name for the '_intentFadeTween' field.
			/// </summary>
			// Token: 0x040060FB RID: 24827
			public static readonly StringName _intentFadeTween = "_intentFadeTween";

			/// <summary>
			/// Cached name for the '_shakeTween' field.
			/// </summary>
			// Token: 0x040060FC RID: 24828
			public static readonly StringName _shakeTween = "_shakeTween";

			/// <summary>
			/// Cached name for the '_isRemotePlayerOrPet' field.
			/// </summary>
			// Token: 0x040060FD RID: 24829
			public static readonly StringName _isRemotePlayerOrPet = "_isRemotePlayerOrPet";

			/// <summary>
			/// Cached name for the '_isInBestiary' field.
			/// </summary>
			// Token: 0x040060FE RID: 24830
			public static readonly StringName _isInBestiary = "_isInBestiary";

			/// <summary>
			/// Cached name for the '_tempScale' field.
			/// </summary>
			// Token: 0x040060FF RID: 24831
			public static readonly StringName _tempScale = "_tempScale";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x04006100 RID: 24832
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_isInMultiselect' field.
			/// </summary>
			// Token: 0x04006101 RID: 24833
			public static readonly StringName _isInMultiselect = "_isInMultiselect";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04006102 RID: 24834
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018C5 RID: 6341
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
