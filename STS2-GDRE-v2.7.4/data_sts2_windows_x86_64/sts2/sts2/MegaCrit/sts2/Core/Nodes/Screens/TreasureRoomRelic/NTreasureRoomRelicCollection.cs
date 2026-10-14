using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic
{
	/// <summary>
	/// Selection screen for relics at the treasure room screen.
	/// In singleplayer, this just shows one relic. In multiplayer, this shows multiple relics and handles animation that
	/// occurs after all players select a relic.
	/// </summary>
	// Token: 0x02000255 RID: 597
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NTreasureRoomRelicCollection.cs")]
	public class NTreasureRoomRelicCollection : Control, IScreenContext
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x000CDBC8 File Offset: 0x000CBDC8
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x000CDBD0 File Offset: 0x000CBDD0
		public NTreasureRoomRelicHolder SingleplayerRelicHolder { get; private set; }

		// Token: 0x06001F0C RID: 7948 RVA: 0x000CDBDC File Offset: 0x000CBDDC
		public override void _Ready()
		{
			this._fightBackstop = base.GetNode<Control>("%FightBackstop");
			this._hands = base.GetNode<NHandImageCollection>("%HandsContainer");
			this._relicContainer = base.GetNode<Control>("Container");
			this.SingleplayerRelicHolder = this._relicContainer.GetNode<NTreasureRoomRelicHolder>("%SingleplayerRelicHolder");
			foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in this._relicContainer.GetChildren(false).OfType<NTreasureRoomRelicHolder>())
			{
				if (ntreasureRoomRelicHolder != this.SingleplayerRelicHolder)
				{
					this._multiplayerHolders.Add(ntreasureRoomRelicHolder);
				}
			}
			CanvasItem fightBackstop = this._fightBackstop;
			Color modulate = this._fightBackstop.Modulate;
			modulate.A = 0f;
			fightBackstop.Modulate = modulate;
			this._fightBackstop.Visible = false;
			this._fightLabel = base.GetNode<MegaLabel>("%FightLabel");
			this._fightLabel.Text = new LocString("gameplay_ui", "TREASURE_FIGHT_TEXT").GetFormattedText();
			RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged += this.RefreshVotes;
			RunManager.Instance.TreasureRoomRelicSynchronizer.RelicsAwarded += this.OnRelicsAwarded;
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x000CDD3C File Offset: 0x000CBF3C
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x000CDD4C File Offset: 0x000CBF4C
		public override void _ExitTree()
		{
			this._cts.Cancel();
			RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged -= this.RefreshVotes;
			RunManager.Instance.TreasureRoomRelicSynchronizer.RelicsAwarded -= this.OnRelicsAwarded;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000CDD9A File Offset: 0x000CBF9A
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			this._hands.Initialize(runState);
		}

		/// <summary>
		/// Initialize the relic display.
		/// This can't get called in _Ready because RelicPickingSynchronizer.BeginRelicPicking has not been called by then.
		/// </summary>
		// Token: 0x06001F10 RID: 7952 RVA: 0x000CDDB0 File Offset: 0x000CBFB0
		public unsafe void InitializeRelics()
		{
			IReadOnlyList<RelicModel> currentRelics = RunManager.Instance.TreasureRoomRelicSynchronizer.CurrentRelics;
			if (currentRelics == null || currentRelics.Count == 0)
			{
				this._isEmptyChest = true;
				this.SingleplayerRelicHolder.Visible = false;
				foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in this._multiplayerHolders)
				{
					ntreasureRoomRelicHolder.Visible = false;
				}
				this.SpawnEmptyChestVfx();
				return;
			}
			if (currentRelics.Count == 1)
			{
				this.SingleplayerRelicHolder.Initialize(currentRelics[0], this._runState);
				this.SingleplayerRelicHolder.Visible = true;
				this.SingleplayerRelicHolder.Index = 0;
				this.SingleplayerRelicHolder.Connect(NClickableControl.SignalName.Released, Callable.From<NTreasureRoomRelicHolder>(delegate(NTreasureRoomRelicHolder _)
				{
					this.PickRelic(this.SingleplayerRelicHolder);
				}), 0U);
				int num = 1;
				List<NTreasureRoomRelicHolder> list = new List<NTreasureRoomRelicHolder>(num);
				CollectionsMarshal.SetCount<NTreasureRoomRelicHolder>(list, num);
				Span<NTreasureRoomRelicHolder> span = CollectionsMarshal.AsSpan<NTreasureRoomRelicHolder>(list);
				int num2 = 0;
				*span[num2] = this.SingleplayerRelicHolder;
				this._holdersInUse = list;
				using (List<NTreasureRoomRelicHolder>.Enumerator enumerator2 = this._multiplayerHolders.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						NTreasureRoomRelicHolder ntreasureRoomRelicHolder2 = enumerator2.Current;
						ntreasureRoomRelicHolder2.Visible = false;
					}
					return;
				}
			}
			this.SingleplayerRelicHolder.Visible = false;
			for (int i = 0; i < this._multiplayerHolders.Count; i++)
			{
				NTreasureRoomRelicHolder holder = this._multiplayerHolders[i];
				if (i < currentRelics.Count)
				{
					holder.Visible = true;
					holder.Relic.Model = currentRelics[i];
					holder.Initialize(currentRelics[i], this._runState);
				}
				else
				{
					holder.Visible = false;
				}
				holder.Index = i;
				holder.Connect(NClickableControl.SignalName.Released, Callable.From<NTreasureRoomRelicHolder>(delegate(NTreasureRoomRelicHolder _)
				{
					this.PickRelic(holder);
				}), 0U);
				this._holdersInUse.Add(holder);
				holder.VoteContainer.RefreshPlayerVotes(true);
			}
			for (int j = 0; j < this._holdersInUse.Count; j++)
			{
				this._holdersInUse[j].SetFocusMode(2L);
				this._holdersInUse[j].FocusNeighborTop = this._holdersInUse[j].GetPath();
				this._holdersInUse[j].FocusNeighborBottom = this._holdersInUse[j].GetPath();
				Control control = this._holdersInUse[j];
				NodePath nodePath;
				if (j <= 0)
				{
					List<NTreasureRoomRelicHolder> holdersInUse = this._holdersInUse;
					nodePath = holdersInUse[holdersInUse.Count - 1].GetPath();
				}
				else
				{
					nodePath = this._holdersInUse[j - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				this._holdersInUse[j].FocusNeighborRight = ((j < this._holdersInUse.Count - 1) ? this._holdersInUse[j + 1].GetPath() : this._holdersInUse[0].GetPath());
			}
			if (currentRelics.Count == 2)
			{
				this._multiplayerHolders[1].Position = this._multiplayerHolders[3].Position;
			}
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x000CE144 File Offset: 0x000CC344
		private void SpawnEmptyChestVfx()
		{
			MegaLabel node = base.GetNode<MegaLabel>("%EmptyLabel");
			node.Text = new LocString("gameplay_ui", "TREASURE_EMPTY").GetFormattedText();
			node.Visible = true;
			this._emptyVfxTween = base.CreateTween().SetParallel(true);
			this._emptyVfxTween.TweenProperty(node, "self_modulate:a", 1f, 0.25);
			this._emptyVfxTween.TweenProperty(node, "position:y", node.Position.Y, 1.0).From(node.Position.Y + 64f).SetEase(1L)
				.SetTrans(11L);
			this._emptyVfxTween.Chain().TweenInterval(1.0);
			this._emptyVfxTween.Chain();
			this._emptyVfxTween.TweenProperty(node, "self_modulate:a", 0f, 1.0);
			GpuParticles2D node2 = base.GetNode<GpuParticles2D>("%SmokePuffVfx");
			node2.Emitting = true;
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000CE284 File Offset: 0x000CC484
		public void SetSelectionEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				this.SingleplayerRelicHolder.Enable();
				using (List<NTreasureRoomRelicHolder>.Enumerator enumerator = this._multiplayerHolders.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NTreasureRoomRelicHolder ntreasureRoomRelicHolder = enumerator.Current;
						ntreasureRoomRelicHolder.Enable();
					}
					return;
				}
			}
			this.SingleplayerRelicHolder.Disable();
			foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder2 in this._multiplayerHolders)
			{
				ntreasureRoomRelicHolder2.Disable();
			}
		}

		/// <summary>
		/// Await this to know when relic picking begins.
		/// </summary>
		// Token: 0x06001F13 RID: 7955 RVA: 0x000CE330 File Offset: 0x000CC530
		public Task RelicPickingBegan()
		{
			return this._relicPickingBeganTaskCompletionSource.Task;
		}

		/// <summary>
		/// Await this to know when to hide the screen.
		/// </summary>
		// Token: 0x06001F14 RID: 7956 RVA: 0x000CE33D File Offset: 0x000CC53D
		public Task RelicPickingFinished()
		{
			return this._relicPickingCompleteTaskCompletionSource.Task;
		}

		/// <summary>
		/// Animates in the relic collection.
		/// </summary>
		/// <param name="chestVisual">Chest visual to fade out.</param>
		// Token: 0x06001F15 RID: 7957 RVA: 0x000CE34C File Offset: 0x000CC54C
		public void AnimIn(Node chestVisual)
		{
			base.Visible = true;
			base.Modulate = Colors.Transparent;
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenProperty(this, "modulate", Colors.White, 0.4);
			tween.TweenProperty(chestVisual, "modulate", StsColors.halfTransparentWhite, 0.4);
			if (this._isEmptyChest)
			{
				Player me = LocalContext.GetMe(this._runState);
				if (me != null)
				{
					SilverCrucible silverCrucible = me.Relics.OfType<SilverCrucible>().FirstOrDefault<SilverCrucible>();
					if (silverCrucible != null)
					{
						silverCrucible.Flash();
					}
				}
				tween.TweenCallback(Callable.From(delegate
				{
					RunManager.Instance.TreasureRoomRelicSynchronizer.CompleteWithNoRelics();
				})).SetDelay(1.0);
				return;
			}
			using (List<NTreasureRoomRelicHolder>.Enumerator enumerator = this._holdersInUse.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NTreasureRoomRelicHolder holder = enumerator.Current;
					holder.MouseFilter = 2L;
					float num = ((this._holdersInUse.Count == 1) ? 150f : 50f);
					float num2 = 0.2f + 0.2f * Rng.Chaotic.NextFloat(1f);
					holder.Modulate = Colors.Black;
					Control holder2 = holder;
					Vector2 position = holder.Position;
					position.Y = holder.Position.Y + num;
					holder2.Position = position;
					Tween tween2 = base.CreateTween().SetParallel(true);
					tween2.TweenProperty(holder, "modulate", Colors.White, 0.2).SetDelay((double)num2);
					tween2.TweenProperty(holder, "position:y", holder.Position.Y - num, 0.6).SetDelay((double)num2).SetEase(1L)
						.SetTrans(10L);
					tween2.TweenCallback(Callable.From<Control.MouseFilterEnum>(() => holder.MouseFilter = 0L)).SetDelay((double)num2 + 0.6);
				}
			}
			NRun.Instance.ScreenStateTracker.SetIsInSharedRelicPickingScreen(true);
			this._hands.AnimateHandsIn();
		}

		/// <summary>
		/// Animates out the relic collection, after relic picking is done.
		/// </summary>
		/// <param name="chestVisual">Chest visual to fade back in.</param>
		// Token: 0x06001F16 RID: 7958 RVA: 0x000CE5EC File Offset: 0x000CC7EC
		public void AnimOut(Node chestVisual)
		{
			base.Modulate = Colors.White;
			Tween tween = base.CreateTween().Parallel();
			tween.TweenProperty(this, "modulate", StsColors.transparentWhite, 0.3);
			tween.TweenProperty(chestVisual, "modulate", Colors.White, 0.3);
			tween.TweenCallback(Callable.From<bool>(() => base.Visible = false));
			NRun.Instance.ScreenStateTracker.SetIsInSharedRelicPickingScreen(false);
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000CE682 File Offset: 0x000CC882
		private void PickRelic(NTreasureRoomRelicHolder holder)
		{
			if (Time.GetTicksMsec() - this._openedTicks <= 200UL)
			{
				return;
			}
			RunManager.Instance.TreasureRoomRelicSynchronizer.PickRelicLocally(new int?(holder.Index));
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000CE6B3 File Offset: 0x000CC8B3
		private void OnRelicsAwarded(List<RelicPickingResult> results)
		{
			TaskHelper.RunSafely(this.AnimateRelicAwards(results));
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000CE6C4 File Offset: 0x000CC8C4
		private async Task AnimateRelicAwards(List<RelicPickingResult> results)
		{
			foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in this._holdersInUse)
			{
				ntreasureRoomRelicHolder.SetFocusMode(0L);
			}
			this._relicPickingBeganTaskCompletionSource.SetResult();
			foreach (Player player in this._runState.Players)
			{
				TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
				if (playerVote.voteReceived && playerVote.index == null)
				{
					NHandImage hand = this._hands.GetHand(player.NetId);
					if (hand != null)
					{
						hand.SetSkipped();
					}
				}
			}
			this._hands.BeforeRelicsAwarded();
			List<Task> tasksToWait = new List<Task>();
			RelicPickingResultType? relicPickingResultType = null;
			results.Sort((RelicPickingResult r1, RelicPickingResult r2) => r1.type.CompareTo(r2.type));
			using (List<RelicPickingResult>.Enumerator enumerator3 = results.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					NTreasureRoomRelicCollection.<>c__DisplayClass31_0 CS$<>8__locals1 = new NTreasureRoomRelicCollection.<>c__DisplayClass31_0();
					CS$<>8__locals1.result = enumerator3.Current;
					NTreasureRoomRelicHolder holder = this._holdersInUse.First<NTreasureRoomRelicHolder>((NTreasureRoomRelicHolder h) => h.Relic.Model == CS$<>8__locals1.result.relic);
					holder.AnimateAwayVotes();
					if (relicPickingResultType != null)
					{
						RelicPickingResultType type = CS$<>8__locals1.result.type;
						RelicPickingResultType? relicPickingResultType2 = relicPickingResultType;
						if (!((type == relicPickingResultType2.GetValueOrDefault()) & (relicPickingResultType2 != null)))
						{
							await Cmd.Wait(0.5f, this._cts.Token, false);
						}
					}
					if (CS$<>8__locals1.result.type == RelicPickingResultType.FoughtOver)
					{
						Node parent = this._fightBackstop.GetParent();
						holder.Reparent(parent, true);
						parent.MoveChildSafely(holder, this._fightBackstop.GetIndex(false) + 1);
						this._fightBackstop.Visible = true;
						Tween tween = base.CreateTween();
						tween.TweenProperty(holder, "global_position", (this._fightBackstop.Size - holder.Size) * 0.5f, 0.25).SetTrans(10L).SetEase(0L);
						tween.TweenProperty(this._fightBackstop, "modulate:a", 1f, 0.25);
						this._hands.BeforeFightStarted(CS$<>8__locals1.result.fight.playersInvolved);
						await tween.AwaitFinished(this._cts.Token);
						await Cmd.Wait(1f, this._cts.Token, false);
						await this._hands.DoFight(CS$<>8__locals1.result, holder);
						tween = base.CreateTween();
						tween.TweenProperty(this._fightBackstop, "modulate:a", 0f, 0.25);
						await tween.AwaitFinished(this._cts.Token);
						this._fightBackstop.Visible = false;
					}
					else if (CS$<>8__locals1.result.type != RelicPickingResultType.Skipped)
					{
						NHandImage hand2 = this._hands.GetHand(CS$<>8__locals1.result.player.NetId);
						if (hand2 != null)
						{
							tasksToWait.Add(TaskHelper.RunSafely(hand2.GrabRelic(holder)));
							await Cmd.Wait(0.25f, this._cts.Token, false);
						}
					}
					relicPickingResultType = new RelicPickingResultType?(CS$<>8__locals1.result.type);
					holder = null;
					CS$<>8__locals1 = null;
				}
			}
			List<RelicPickingResult>.Enumerator enumerator3 = default(List<RelicPickingResult>.Enumerator);
			await Task.WhenAll(tasksToWait);
			if (tasksToWait.Count == 0 && this._runState.Players.Count > 1)
			{
				await Cmd.Wait(0.7f, this._cts.Token, false);
			}
			using (List<RelicPickingResult>.Enumerator enumerator4 = results.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					RelicPickingResult result = enumerator4.Current;
					NTreasureRoomRelicHolder ntreasureRoomRelicHolder2 = this._holdersInUse.First<NTreasureRoomRelicHolder>((NTreasureRoomRelicHolder h) => h.Relic.Model == result.relic);
					RelicModel relicModel = result.relic.ToMutable();
					ntreasureRoomRelicHolder2.Disable();
					if (result.type != RelicPickingResultType.Skipped)
					{
						TaskHelper.RunSafely(RelicCmd.Obtain(relicModel, result.player, -1));
						if (LocalContext.IsMe(result.player))
						{
							NRun.Instance.GlobalUi.RelicInventory.AnimateRelic(relicModel, new Vector2?(ntreasureRoomRelicHolder2.GlobalPosition), new Vector2?(ntreasureRoomRelicHolder2.Scale));
						}
						if (this._runState.Players.Count == 1)
						{
							ntreasureRoomRelicHolder2.Visible = false;
						}
					}
					foreach (Player player2 in this._runState.Players)
					{
						if (player2 != result.player)
						{
							player2.RelicGrabBag.MoveToFallback(result.relic);
						}
					}
				}
			}
			this._relicPickingCompleteTaskCompletionSource.SetResult();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000CE710 File Offset: 0x000CC910
		private void RefreshVotes()
		{
			foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in this._holdersInUse)
			{
				ntreasureRoomRelicHolder.VoteContainer.RefreshPlayerVotes(true);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000CE768 File Offset: 0x000CC968
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._holdersInUse.Count <= 0)
				{
					return null;
				}
				return this._holdersInUse[this._runState.GetPlayerSlotIndex(LocalContext.GetMe(this._runState.Players))];
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F1C RID: 7964 RVA: 0x000CE7A0 File Offset: 0x000CC9A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NTreasureRoomRelicCollection.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.InitializeRelics, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isEnabled", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "chestVisual", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "chestVisual", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.PickRelic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTreasureRoomRelicCollection.MethodName.RefreshVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F1D RID: 7965 RVA: 0x000CEA44 File Offset: 0x000CCC44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTreasureRoomRelicCollection.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.InitializeRelics && args.Count == 0)
			{
				this.InitializeRelics();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx && args.Count == 0)
			{
				this.SpawnEmptyChestVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled && args.Count == 1)
			{
				this.SetSelectionEnabled(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.AnimIn && args.Count == 1)
			{
				this.AnimIn(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.AnimOut && args.Count == 1)
			{
				this.AnimOut(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.PickRelic && args.Count == 1)
			{
				this.PickRelic(VariantUtils.ConvertTo<NTreasureRoomRelicHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicCollection.MethodName.RefreshVotes && args.Count == 0)
			{
				this.RefreshVotes();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F1E RID: 7966 RVA: 0x000CEC04 File Offset: 0x000CCE04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTreasureRoomRelicCollection.MethodName._Ready || (ref method) == NTreasureRoomRelicCollection.MethodName._EnterTree || (ref method) == NTreasureRoomRelicCollection.MethodName._ExitTree || (ref method) == NTreasureRoomRelicCollection.MethodName.InitializeRelics || (ref method) == NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx || (ref method) == NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled || (ref method) == NTreasureRoomRelicCollection.MethodName.AnimIn || (ref method) == NTreasureRoomRelicCollection.MethodName.AnimOut || (ref method) == NTreasureRoomRelicCollection.MethodName.PickRelic || (ref method) == NTreasureRoomRelicCollection.MethodName.RefreshVotes || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001F1F RID: 7967 RVA: 0x000CECB0 File Offset: 0x000CCEB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder)
			{
				this.SingleplayerRelicHolder = VariantUtils.ConvertTo<NTreasureRoomRelicHolder>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._relicContainer)
			{
				this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._fightBackstop)
			{
				this._fightBackstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._fightLabel)
			{
				this._fightLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._emptyVfxTween)
			{
				this._emptyVfxTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._hands)
			{
				this._hands = VariantUtils.ConvertTo<NHandImageCollection>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._openedTicks)
			{
				this._openedTicks = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._isEmptyChest)
			{
				this._isEmptyChest = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001F20 RID: 7968 RVA: 0x000CEDA0 File Offset: 0x000CCFA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder)
			{
				NTreasureRoomRelicHolder singleplayerRelicHolder = this.SingleplayerRelicHolder;
				value = VariantUtils.CreateFrom<NTreasureRoomRelicHolder>(ref singleplayerRelicHolder);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._relicContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._fightBackstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._fightBackstop);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._fightLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._fightLabel);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._emptyVfxTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._emptyVfxTween);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._hands)
			{
				value = VariantUtils.CreateFrom<NHandImageCollection>(ref this._hands);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._openedTicks)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._openedTicks);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicCollection.PropertyName._isEmptyChest)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isEmptyChest);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F21 RID: 7969 RVA: 0x000CEEDC File Offset: 0x000CD0DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName._relicContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName._fightBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName._fightLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName._hands, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTreasureRoomRelicCollection.PropertyName._openedTicks, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTreasureRoomRelicCollection.PropertyName._isEmptyChest, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicCollection.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F22 RID: 7970 RVA: 0x000CF018 File Offset: 0x000CD218
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName singleplayerRelicHolder = NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder;
			NTreasureRoomRelicHolder singleplayerRelicHolder2 = this.SingleplayerRelicHolder;
			info.AddProperty(singleplayerRelicHolder, Variant.From<NTreasureRoomRelicHolder>(ref singleplayerRelicHolder2));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._fightBackstop, Variant.From<Control>(ref this._fightBackstop));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._fightLabel, Variant.From<MegaLabel>(ref this._fightLabel));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, Variant.From<Tween>(ref this._emptyVfxTween));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._hands, Variant.From<NHandImageCollection>(ref this._hands));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._openedTicks, Variant.From<ulong>(ref this._openedTicks));
			info.AddProperty(NTreasureRoomRelicCollection.PropertyName._isEmptyChest, Variant.From<bool>(ref this._isEmptyChest));
		}

		/// <inheritdoc />
		// Token: 0x06001F23 RID: 7971 RVA: 0x000CF0E0 File Offset: 0x000CD2E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder, ref variant))
			{
				this.SingleplayerRelicHolder = variant.As<NTreasureRoomRelicHolder>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._relicContainer, ref variant2))
			{
				this._relicContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._fightBackstop, ref variant3))
			{
				this._fightBackstop = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._fightLabel, ref variant4))
			{
				this._fightLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, ref variant5))
			{
				this._emptyVfxTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._hands, ref variant6))
			{
				this._hands = variant6.As<NHandImageCollection>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._openedTicks, ref variant7))
			{
				this._openedTicks = variant7.As<ulong>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._isEmptyChest, ref variant8))
			{
				this._isEmptyChest = variant8.As<bool>();
			}
		}

		// Token: 0x04000BF6 RID: 3062
		private const ulong _noSelectionTimeMsec = 200UL;

		// Token: 0x04000BF7 RID: 3063
		private Control _relicContainer;

		// Token: 0x04000BF8 RID: 3064
		private Control _fightBackstop;

		// Token: 0x04000BF9 RID: 3065
		private MegaLabel _fightLabel;

		// Token: 0x04000BFA RID: 3066
		[Nullable(2)]
		private Tween _emptyVfxTween;

		// Token: 0x04000BFB RID: 3067
		private NHandImageCollection _hands;

		// Token: 0x04000BFC RID: 3068
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000BFD RID: 3069
		private readonly List<NTreasureRoomRelicHolder> _multiplayerHolders = new List<NTreasureRoomRelicHolder>();

		// Token: 0x04000BFE RID: 3070
		private List<NTreasureRoomRelicHolder> _holdersInUse = new List<NTreasureRoomRelicHolder>();

		// Token: 0x04000BFF RID: 3071
		private readonly TaskCompletionSource _relicPickingBeganTaskCompletionSource = new TaskCompletionSource();

		// Token: 0x04000C00 RID: 3072
		private readonly TaskCompletionSource _relicPickingCompleteTaskCompletionSource = new TaskCompletionSource();

		// Token: 0x04000C01 RID: 3073
		private ulong _openedTicks;

		// Token: 0x04000C02 RID: 3074
		private IRunState _runState;

		// Token: 0x04000C03 RID: 3075
		private bool _isEmptyChest;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200128A RID: 4746
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040047FB RID: 18427
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040047FC RID: 18428
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040047FD RID: 18429
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'InitializeRelics' method.
			/// </summary>
			// Token: 0x040047FE RID: 18430
			public static readonly StringName InitializeRelics = "InitializeRelics";

			/// <summary>
			/// Cached name for the 'SpawnEmptyChestVfx' method.
			/// </summary>
			// Token: 0x040047FF RID: 18431
			public static readonly StringName SpawnEmptyChestVfx = "SpawnEmptyChestVfx";

			/// <summary>
			/// Cached name for the 'SetSelectionEnabled' method.
			/// </summary>
			// Token: 0x04004800 RID: 18432
			public static readonly StringName SetSelectionEnabled = "SetSelectionEnabled";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04004801 RID: 18433
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x04004802 RID: 18434
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'PickRelic' method.
			/// </summary>
			// Token: 0x04004803 RID: 18435
			public static readonly StringName PickRelic = "PickRelic";

			/// <summary>
			/// Cached name for the 'RefreshVotes' method.
			/// </summary>
			// Token: 0x04004804 RID: 18436
			public static readonly StringName RefreshVotes = "RefreshVotes";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200128B RID: 4747
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SingleplayerRelicHolder' property.
			/// </summary>
			// Token: 0x04004805 RID: 18437
			public static readonly StringName SingleplayerRelicHolder = "SingleplayerRelicHolder";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004806 RID: 18438
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_relicContainer' field.
			/// </summary>
			// Token: 0x04004807 RID: 18439
			public static readonly StringName _relicContainer = "_relicContainer";

			/// <summary>
			/// Cached name for the '_fightBackstop' field.
			/// </summary>
			// Token: 0x04004808 RID: 18440
			public static readonly StringName _fightBackstop = "_fightBackstop";

			/// <summary>
			/// Cached name for the '_fightLabel' field.
			/// </summary>
			// Token: 0x04004809 RID: 18441
			public static readonly StringName _fightLabel = "_fightLabel";

			/// <summary>
			/// Cached name for the '_emptyVfxTween' field.
			/// </summary>
			// Token: 0x0400480A RID: 18442
			public static readonly StringName _emptyVfxTween = "_emptyVfxTween";

			/// <summary>
			/// Cached name for the '_hands' field.
			/// </summary>
			// Token: 0x0400480B RID: 18443
			public static readonly StringName _hands = "_hands";

			/// <summary>
			/// Cached name for the '_openedTicks' field.
			/// </summary>
			// Token: 0x0400480C RID: 18444
			public static readonly StringName _openedTicks = "_openedTicks";

			/// <summary>
			/// Cached name for the '_isEmptyChest' field.
			/// </summary>
			// Token: 0x0400480D RID: 18445
			public static readonly StringName _isEmptyChest = "_isEmptyChest";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200128C RID: 4748
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
