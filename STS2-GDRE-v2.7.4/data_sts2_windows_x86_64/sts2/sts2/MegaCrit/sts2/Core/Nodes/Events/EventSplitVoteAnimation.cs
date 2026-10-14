using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	/// <summary>
	/// Represents the animation which plays when players don't all vote for the same shared event option.
	/// </summary>
	// Token: 0x020003B6 RID: 950
	[NullableContext(1)]
	[Nullable(0)]
	public class EventSplitVoteAnimation
	{
		// Token: 0x06003927 RID: 14631 RVA: 0x001B0545 File Offset: 0x001AE745
		public EventSplitVoteAnimation(NEventLayout eventLayout, IRunState runState)
		{
			this._eventLayout = eventLayout;
			this._runState = runState;
		}

		/// <summary>
		/// Plays the animation if necessary.
		/// The animation is not played if all players voted for the same event option.
		/// Note that we are using the votes as indicated by the nodes and not by EventSynchronizer. This is on purpose -
		/// the votes contained in EventSynchronizer have already been cleared by this point in time.
		/// </summary>
		// Token: 0x06003928 RID: 14632 RVA: 0x001B0568 File Offset: 0x001AE768
		public async Task TryPlay(NEventOptionButton chosenButton)
		{
			if (this._eventLayout.OptionButtons.Count<NEventOptionButton>((NEventOptionButton b) => b.VoteContainer.Players.Any<Player>()) != 1)
			{
				Rng rng = new Rng(this._runState.Rng.Seed + (ulong)((long)this._runState.ActFloor));
				this._ticks = rng.NextInt(12, 18);
				float num = rng.NextFloat(0.05f, 0.3f);
				this._winner = rng.NextItem<Player>(chosenButton.VoteContainer.Players);
				foreach (NEventOptionButton neventOptionButton in this._eventLayout.OptionButtons)
				{
					this._sortedPlayers.AddRange(neventOptionButton.VoteContainer.Players);
				}
				Tween tween = this._eventLayout.CreateTween();
				tween.TweenMethod(Callable.From<float>(new Action<float>(this.TickSplitVoteAnimation)), 0f, 1f, 1.2000000476837158).SetTrans(1L).SetEase(1L);
				tween.TweenInterval((double)num);
				TaskAwaiter<bool> taskAwaiter = tween.AwaitFinished(this._eventLayout).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					chosenButton.VoteContainer.BouncePlayers();
				}
			}
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x001B05B4 File Offset: 0x001AE7B4
		private void TickSplitVoteAnimation(float value)
		{
			int num = Mathf.RoundToInt(value * (float)this._ticks);
			int num2 = this._sortedPlayers.IndexOf(this._winner) - (this._ticks - num);
			int num3 = (num2 % this._sortedPlayers.Count + this._sortedPlayers.Count) % this._sortedPlayers.Count;
			Player player = this._sortedPlayers[num3];
			if (this._currentlyHighlightedPlayer != player)
			{
				this.HighlightPlayer(this._sortedPlayers[num3]);
				NDebugAudioManager.Instance.Play("map_split_tick.mp3", 0.15f, PitchVariance.Small);
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x001B0650 File Offset: 0x001AE850
		[NullableContext(2)]
		private void HighlightPlayer(Player player)
		{
			if (this._currentlyHighlightedPlayer == player)
			{
				return;
			}
			if (this._currentlyHighlightedPlayer != null)
			{
				foreach (NEventOptionButton neventOptionButton in this._eventLayout.OptionButtons)
				{
					if (neventOptionButton.VoteContainer.Players.Contains(this._currentlyHighlightedPlayer))
					{
						neventOptionButton.VoteContainer.SetPlayerHighlighted(this._currentlyHighlightedPlayer, false);
					}
				}
			}
			this._currentlyHighlightedPlayer = player;
			if (player != null)
			{
				foreach (NEventOptionButton neventOptionButton2 in this._eventLayout.OptionButtons)
				{
					if (neventOptionButton2.VoteContainer.Players.Contains(player))
					{
						neventOptionButton2.VoteContainer.SetPlayerHighlighted(player, true);
					}
				}
			}
		}

		// Token: 0x04001635 RID: 5685
		private NEventLayout _eventLayout;

		// Token: 0x04001636 RID: 5686
		private IRunState _runState;

		// Token: 0x04001637 RID: 5687
		private int _ticks;

		// Token: 0x04001638 RID: 5688
		private Player _winner;

		// Token: 0x04001639 RID: 5689
		private readonly List<Player> _sortedPlayers = new List<Player>();

		// Token: 0x0400163A RID: 5690
		[Nullable(2)]
		private Player _currentlyHighlightedPlayer;
	}
}
