using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	/// <summary>
	/// Manages the UI flow for a treasure room.
	/// </summary>
	// Token: 0x02000370 RID: 880
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Rooms/NTreasureRoom.cs")]
	public class NTreasureRoom : Control, IScreenContext, IRoomWithProceedButton
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x001813A8 File Offset: 0x0017F5A8
		public NProceedButton ProceedButton
		{
			get
			{
				return this._proceedButton;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x001813B0 File Offset: 0x0017F5B0
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NTreasureRoom._scenePath);
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x001813BC File Offset: 0x0017F5BC
		[return: Nullable(2)]
		public static NTreasureRoom Create(TreasureRoom room, IRunState runState)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NTreasureRoom ntreasureRoom = PreloadManager.Cache.GetScene(NTreasureRoom._scenePath).Instantiate<NTreasureRoom>(0L);
			ntreasureRoom._room = room;
			ntreasureRoom._runState = runState;
			return ntreasureRoom;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x001813F8 File Offset: 0x0017F5F8
		public override void _Ready()
		{
			this._banner = base.GetNode<NCommonBanner>("%Banner");
			if (this._runState.Players.Count == 1)
			{
				this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "TREASURE_BANNER").GetRawText());
			}
			else
			{
				this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_SHARED_RELIC_HEADER").GetRawText());
			}
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this._chestNode = base.GetNode<Node2D>("%ChestVisual");
			this._chestAnimController = new MegaSprite(this._chestNode);
			this._goldParticles = base.GetNode<GpuParticles2D>("%GoldExplosion");
			this._relicCollection = base.GetNode<NTreasureRoomRelicCollection>("%RelicCollection");
			this._skipVoteContainer = base.GetNode<NMultiplayerVoteContainer>("%SkipMultiplayerVoteContainer");
			this._relicCollection.Initialize(this._runState);
			this._relicCollection.Visible = false;
			this._chestAnimController.SetSkeletonDataRes(this._runState.Act.ChestSpineResource);
			MegaSkeleton skeleton = this._chestAnimController.GetSkeleton();
			if (skeleton != null)
			{
				MegaSkeletonDataResource data = skeleton.GetData();
				this._regularChestSkin = data.FindSkin(this._runState.Act.ChestSpineSkinNameNormal);
				this._outlineChestSkin = data.FindSkin(this._runState.Act.ChestSpineSkinNameStroke);
				skeleton.SetSlotsToSetupPose();
				this._chestAnimController.GetAnimationState().Apply(skeleton);
				MegaAnimationState animationState = this._chestAnimController.GetAnimationState();
				animationState.SetAnimation("animation", false, 0);
				this._chestAnimController.GetAnimationState().AddAnimation("shine_fade", 0f, false, 0);
				animationState.SetTimeScale(0f);
				this.UpdateChestSkin(false);
			}
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			this._skipVoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.IsPlayerVotingForSkip), this._runState.Players);
			this._chestButton = base.GetNode<NButton>("%Chest");
			this._chestButton.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnMouseEntered)), 0U);
			this._chestButton.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnMouseExited)), 0U);
			this._chestButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnChestButtonReleased)), 0U);
			NGame.Instance.SetScreenShakeTarget(this);
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x001816C2 File Offset: 0x0017F8C2
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenChanged;
			RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged += this.RefreshVotes;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x00181700 File Offset: 0x0017F900
		public override void _ExitTree()
		{
			this._cts.Cancel();
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenChanged;
			RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged -= this.RefreshVotes;
			NGame.Instance.ClearScreenShakeTarget();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x00181754 File Offset: 0x0017F954
		private void OnProceedButtonPressed(NButton _)
		{
			if (this._proceedButton.IsSkip)
			{
				RunManager.Instance.TreasureRoomRelicSynchronizer.SkipRelicLocally();
				if (this._runState.Players.Count == 1)
				{
					NMapScreen.Instance.SetTravelEnabled(true);
					TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
					return;
				}
			}
			else
			{
				TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
			}
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x001817BC File Offset: 0x0017F9BC
		private void RefreshVotes()
		{
			this._skipVoteContainer.RefreshPlayerVotes(true);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x001817CA File Offset: 0x0017F9CA
		private void OnProceedButtonReleased(NButton _)
		{
			NMapScreen.Instance.Open(false);
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x001817D8 File Offset: 0x0017F9D8
		private void OnChestButtonReleased(NButton _)
		{
			TaskHelper.RunSafely(this.OpenChest());
			this._chestButton.Disable();
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x001817F1 File Offset: 0x0017F9F1
		private void OnMouseEntered()
		{
			this.UpdateChestSkin(true);
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x001817FA File Offset: 0x0017F9FA
		private void OnMouseExited()
		{
			this.UpdateChestSkin(false);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00181804 File Offset: 0x0017FA04
		private async Task OpenChest()
		{
			this._banner.AnimateIn();
			this._proceedButton.Disable();
			this.UpdateChestSkin(false);
			SfxCmd.Play(this._runState.Act.ChestOpenSfx, 1f);
			this._chestAnimController.GetAnimationState().SetTimeScale(1f);
			this._chestButton.MouseFilter = 2L;
			int num = await this._room.DoNormalRewards();
			int num2 = num;
			if (num2 > 0)
			{
				this._goldParticles.Amount = num2;
				this._goldParticles.Emitting = true;
			}
			await this._room.DoExtraRewardsIfNeeded();
			this._relicCollection.InitializeRelics();
			this._relicCollection.AnimIn(this._chestNode);
			this._isRelicCollectionOpen = true;
			Control defaultFocusedControl = this.DefaultFocusedControl;
			if (defaultFocusedControl != null)
			{
				defaultFocusedControl.TryGrabFocus();
			}
			TaskHelper.RunSafely(this.RelicFtueCheck());
			CancellationTokenSource cancelSource = new CancellationTokenSource();
			if (this._runState.Players.Count == 1)
			{
				this._proceedButton.UpdateText(NProceedButton.SkipLoc);
				TaskHelper.RunSafely(this.EnableSkipAfterDelay(2.5f, cancelSource.Token));
			}
			this._hasChestBeenOpened = true;
			await this._relicCollection.RelicPickingBegan();
			await cancelSource.CancelAsync();
			this._proceedButton.Disable();
			await this._relicCollection.RelicPickingFinished();
			this._isRelicCollectionOpen = false;
			this._skipVoteContainer.RefreshPlayerVotes(true);
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			this._proceedButton.Enable();
			this._banner.AnimateOut();
			NMapScreen.Instance.SetTravelEnabled(true);
			this._relicCollection.AnimOut(this._chestNode);
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x00181848 File Offset: 0x0017FA48
		private async Task EnableSkipAfterDelay(float delay, CancellationToken token)
		{
			await Cmd.Wait(delay, token, false);
			if (!token.IsCancellationRequested)
			{
				this._proceedButton.Enable();
			}
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0018189C File Offset: 0x0017FA9C
		private async Task RelicFtueCheck()
		{
			if (!SaveManager.Instance.SeenFtue("obtain_relic_ftue"))
			{
				this._relicCollection.SetSelectionEnabled(false);
				await Cmd.Wait(1f, this._cts.Token, false);
				Control control;
				if (this._relicCollection.SingleplayerRelicHolder.Visible)
				{
					control = this._relicCollection.SingleplayerRelicHolder;
				}
				else
				{
					control = this._relicCollection;
				}
				this._relicCollection.SetSelectionEnabled(true);
				NModalContainer.Instance.Add(NRelicRewardFtue.Create(control), true);
				SaveManager.Instance.MarkFtueAsComplete("obtain_relic_ftue");
			}
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x001818E0 File Offset: 0x0017FAE0
		private void UpdateChestSkin(bool showOutline)
		{
			MegaSkeleton skeleton = this._chestAnimController.GetSkeleton();
			if (skeleton == null)
			{
				return;
			}
			skeleton.SetSkin(showOutline ? this._outlineChestSkin : this._regularChestSkin);
			skeleton.SetSlotsToSetupPose();
			this._chestAnimController.GetAnimationState().Apply(skeleton);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0018192B File Offset: 0x0017FB2B
		private void OnActiveScreenChanged()
		{
			this.UpdateControllerNavEnabled<NTreasureRoom>();
			if (ActiveScreenContext.Instance.IsCurrent(this) && this._hasChestBeenOpened)
			{
				this._proceedButton.Enable();
				return;
			}
			this._proceedButton.Disable();
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x00181960 File Offset: 0x0017FB60
		private bool IsPlayerVotingForSkip(Player player)
		{
			if (RunManager.Instance.TreasureRoomRelicSynchronizer.CurrentRelics == null)
			{
				return false;
			}
			TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
			if (playerVote != null && playerVote.voteReceived)
			{
				int? index = playerVote.index;
				return index == null;
			}
			return false;
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x001819AF File Offset: 0x0017FBAF
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (!this._isRelicCollectionOpen)
				{
					return null;
				}
				return this._relicCollection.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033C7 RID: 13255 RVA: 0x001819C8 File Offset: 0x0017FBC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NTreasureRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName.OnProceedButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTreasureRoom.MethodName.RefreshVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName.OnProceedButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTreasureRoom.MethodName.OnChestButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTreasureRoom.MethodName.OnMouseEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName.OnMouseExited, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoom.MethodName.UpdateChestSkin, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "showOutline", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTreasureRoom.MethodName.OnActiveScreenChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033C8 RID: 13256 RVA: 0x00181C98 File Offset: 0x0017FE98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTreasureRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnProceedButtonPressed && args.Count == 1)
			{
				this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.RefreshVotes && args.Count == 0)
			{
				this.RefreshVotes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnProceedButtonReleased && args.Count == 1)
			{
				this.OnProceedButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnChestButtonReleased && args.Count == 1)
			{
				this.OnChestButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnMouseEntered && args.Count == 0)
			{
				this.OnMouseEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnMouseExited && args.Count == 0)
			{
				this.OnMouseExited();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.UpdateChestSkin && args.Count == 1)
			{
				this.UpdateChestSkin(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoom.MethodName.OnActiveScreenChanged && args.Count == 0)
			{
				this.OnActiveScreenChanged();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060033C9 RID: 13257 RVA: 0x00181E80 File Offset: 0x00180080
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTreasureRoom.MethodName._Ready || (ref method) == NTreasureRoom.MethodName._EnterTree || (ref method) == NTreasureRoom.MethodName._ExitTree || (ref method) == NTreasureRoom.MethodName.OnProceedButtonPressed || (ref method) == NTreasureRoom.MethodName.RefreshVotes || (ref method) == NTreasureRoom.MethodName.OnProceedButtonReleased || (ref method) == NTreasureRoom.MethodName.OnChestButtonReleased || (ref method) == NTreasureRoom.MethodName.OnMouseEntered || (ref method) == NTreasureRoom.MethodName.OnMouseExited || (ref method) == NTreasureRoom.MethodName.UpdateChestSkin || (ref method) == NTreasureRoom.MethodName.OnActiveScreenChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060033CA RID: 13258 RVA: 0x00181F3C File Offset: 0x0018013C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTreasureRoom.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._chestButton)
			{
				this._chestButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._chestNode)
			{
				this._chestNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._goldParticles)
			{
				this._goldParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._relicCollection)
			{
				this._relicCollection = VariantUtils.ConvertTo<NTreasureRoomRelicCollection>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._skipVoteContainer)
			{
				this._skipVoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._isRelicCollectionOpen)
			{
				this._isRelicCollectionOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._hasChestBeenOpened)
			{
				this._hasChestBeenOpened = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060033CB RID: 13259 RVA: 0x00182044 File Offset: 0x00180244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTreasureRoom.PropertyName.ProceedButton)
			{
				NProceedButton proceedButton = this.ProceedButton;
				value = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._chestButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._chestButton);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._chestNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._chestNode);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._goldParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._goldParticles);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._relicCollection)
			{
				value = VariantUtils.CreateFrom<NTreasureRoomRelicCollection>(ref this._relicCollection);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._skipVoteContainer)
			{
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._skipVoteContainer);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._isRelicCollectionOpen)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isRelicCollectionOpen);
				return true;
			}
			if ((ref name) == NTreasureRoom.PropertyName._hasChestBeenOpened)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hasChestBeenOpened);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033CC RID: 13260 RVA: 0x001821C0 File Offset: 0x001803C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTreasureRoom.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._chestButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._chestNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName.ProceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._goldParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._relicCollection, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName._skipVoteContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTreasureRoom.PropertyName._isRelicCollectionOpen, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTreasureRoom.PropertyName._hasChestBeenOpened, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033CD RID: 13261 RVA: 0x00182340 File Offset: 0x00180540
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTreasureRoom.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NTreasureRoom.PropertyName._chestButton, Variant.From<NButton>(ref this._chestButton));
			info.AddProperty(NTreasureRoom.PropertyName._chestNode, Variant.From<Node2D>(ref this._chestNode));
			info.AddProperty(NTreasureRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NTreasureRoom.PropertyName._goldParticles, Variant.From<GpuParticles2D>(ref this._goldParticles));
			info.AddProperty(NTreasureRoom.PropertyName._relicCollection, Variant.From<NTreasureRoomRelicCollection>(ref this._relicCollection));
			info.AddProperty(NTreasureRoom.PropertyName._skipVoteContainer, Variant.From<NMultiplayerVoteContainer>(ref this._skipVoteContainer));
			info.AddProperty(NTreasureRoom.PropertyName._isRelicCollectionOpen, Variant.From<bool>(ref this._isRelicCollectionOpen));
			info.AddProperty(NTreasureRoom.PropertyName._hasChestBeenOpened, Variant.From<bool>(ref this._hasChestBeenOpened));
		}

		/// <inheritdoc />
		// Token: 0x060033CE RID: 13262 RVA: 0x0018241C File Offset: 0x0018061C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._banner, ref variant))
			{
				this._banner = variant.As<NCommonBanner>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._chestButton, ref variant2))
			{
				this._chestButton = variant2.As<NButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._chestNode, ref variant3))
			{
				this._chestNode = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._proceedButton, ref variant4))
			{
				this._proceedButton = variant4.As<NProceedButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._goldParticles, ref variant5))
			{
				this._goldParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._relicCollection, ref variant6))
			{
				this._relicCollection = variant6.As<NTreasureRoomRelicCollection>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._skipVoteContainer, ref variant7))
			{
				this._skipVoteContainer = variant7.As<NMultiplayerVoteContainer>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._isRelicCollectionOpen, ref variant8))
			{
				this._isRelicCollectionOpen = variant8.As<bool>();
			}
			Variant variant9;
			if (info.TryGetProperty(NTreasureRoom.PropertyName._hasChestBeenOpened, ref variant9))
			{
				this._hasChestBeenOpened = variant9.As<bool>();
			}
		}

		// Token: 0x04001409 RID: 5129
		private TreasureRoom _room;

		// Token: 0x0400140A RID: 5130
		private IRunState _runState;

		// Token: 0x0400140B RID: 5131
		private NCommonBanner _banner;

		// Token: 0x0400140C RID: 5132
		private NButton _chestButton;

		// Token: 0x0400140D RID: 5133
		private Node2D _chestNode;

		// Token: 0x0400140E RID: 5134
		private MegaSprite _chestAnimController;

		// Token: 0x0400140F RID: 5135
		private NProceedButton _proceedButton;

		// Token: 0x04001410 RID: 5136
		[Nullable(2)]
		private MegaSkin _regularChestSkin;

		// Token: 0x04001411 RID: 5137
		[Nullable(2)]
		private MegaSkin _outlineChestSkin;

		// Token: 0x04001412 RID: 5138
		private GpuParticles2D _goldParticles;

		// Token: 0x04001413 RID: 5139
		private NTreasureRoomRelicCollection _relicCollection;

		// Token: 0x04001414 RID: 5140
		private NMultiplayerVoteContainer _skipVoteContainer;

		// Token: 0x04001415 RID: 5141
		private static readonly string _scenePath = SceneHelper.GetScenePath("rooms/treasure_room");

		// Token: 0x04001416 RID: 5142
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04001417 RID: 5143
		private bool _isRelicCollectionOpen;

		// Token: 0x04001418 RID: 5144
		private bool _hasChestBeenOpened;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016A5 RID: 5797
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400583D RID: 22589
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400583E RID: 22590
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400583F RID: 22591
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnProceedButtonPressed' method.
			/// </summary>
			// Token: 0x04005840 RID: 22592
			public static readonly StringName OnProceedButtonPressed = "OnProceedButtonPressed";

			/// <summary>
			/// Cached name for the 'RefreshVotes' method.
			/// </summary>
			// Token: 0x04005841 RID: 22593
			public static readonly StringName RefreshVotes = "RefreshVotes";

			/// <summary>
			/// Cached name for the 'OnProceedButtonReleased' method.
			/// </summary>
			// Token: 0x04005842 RID: 22594
			public static readonly StringName OnProceedButtonReleased = "OnProceedButtonReleased";

			/// <summary>
			/// Cached name for the 'OnChestButtonReleased' method.
			/// </summary>
			// Token: 0x04005843 RID: 22595
			public static readonly StringName OnChestButtonReleased = "OnChestButtonReleased";

			/// <summary>
			/// Cached name for the 'OnMouseEntered' method.
			/// </summary>
			// Token: 0x04005844 RID: 22596
			public static readonly StringName OnMouseEntered = "OnMouseEntered";

			/// <summary>
			/// Cached name for the 'OnMouseExited' method.
			/// </summary>
			// Token: 0x04005845 RID: 22597
			public static readonly StringName OnMouseExited = "OnMouseExited";

			/// <summary>
			/// Cached name for the 'UpdateChestSkin' method.
			/// </summary>
			// Token: 0x04005846 RID: 22598
			public static readonly StringName UpdateChestSkin = "UpdateChestSkin";

			/// <summary>
			/// Cached name for the 'OnActiveScreenChanged' method.
			/// </summary>
			// Token: 0x04005847 RID: 22599
			public static readonly StringName OnActiveScreenChanged = "OnActiveScreenChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016A6 RID: 5798
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ProceedButton' property.
			/// </summary>
			// Token: 0x04005848 RID: 22600
			public static readonly StringName ProceedButton = "ProceedButton";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005849 RID: 22601
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x0400584A RID: 22602
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_chestButton' field.
			/// </summary>
			// Token: 0x0400584B RID: 22603
			public static readonly StringName _chestButton = "_chestButton";

			/// <summary>
			/// Cached name for the '_chestNode' field.
			/// </summary>
			// Token: 0x0400584C RID: 22604
			public static readonly StringName _chestNode = "_chestNode";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x0400584D RID: 22605
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_goldParticles' field.
			/// </summary>
			// Token: 0x0400584E RID: 22606
			public static readonly StringName _goldParticles = "_goldParticles";

			/// <summary>
			/// Cached name for the '_relicCollection' field.
			/// </summary>
			// Token: 0x0400584F RID: 22607
			public static readonly StringName _relicCollection = "_relicCollection";

			/// <summary>
			/// Cached name for the '_skipVoteContainer' field.
			/// </summary>
			// Token: 0x04005850 RID: 22608
			public static readonly StringName _skipVoteContainer = "_skipVoteContainer";

			/// <summary>
			/// Cached name for the '_isRelicCollectionOpen' field.
			/// </summary>
			// Token: 0x04005851 RID: 22609
			public static readonly StringName _isRelicCollectionOpen = "_isRelicCollectionOpen";

			/// <summary>
			/// Cached name for the '_hasChestBeenOpened' field.
			/// </summary>
			// Token: 0x04005852 RID: 22610
			public static readonly StringName _hasChestBeenOpened = "_hasChestBeenOpened";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016A7 RID: 5799
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
