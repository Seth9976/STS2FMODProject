using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// Represents the animation which plays when players don't all vote for the same map point.
	/// The amount of state to keep track of was getting a little unwieldy to put directly into the map screen.
	/// </summary>
	// Token: 0x020002EA RID: 746
	[NullableContext(1)]
	[Nullable(0)]
	public class MapSplitVoteAnimation
	{
		// Token: 0x0600280B RID: 10251 RVA: 0x0011267D File Offset: 0x0011087D
		public MapSplitVoteAnimation(NMapScreen mapScreen, RunState runState, Dictionary<MapCoord, NMapPoint> mapPointDictionary)
		{
			this._mapScreen = mapScreen;
			this._runState = runState;
			this._mapPointDictionary = mapPointDictionary;
		}

		/// <summary>
		/// Plays the animation if necessary.
		/// The animation is not played if all players voted for the same coordinate.
		/// </summary>
		// Token: 0x0600280C RID: 10252 RVA: 0x0011269C File Offset: 0x0011089C
		public async Task TryPlay(MapCoord selectedCoord)
		{
			MapCoord? mapCoord = null;
			bool flag = true;
			List<Player> list = new List<Player>();
			foreach (KeyValuePair<Player, MapCoord?> keyValuePair in this._mapScreen.PlayerVoteDictionary)
			{
				if (mapCoord == null)
				{
					mapCoord = keyValuePair.Value;
				}
				else if (keyValuePair.Value != mapCoord)
				{
					flag = false;
				}
				if (keyValuePair.Value == selectedCoord)
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (!flag)
			{
				Rng rng = new Rng(this._runState.Rng.Seed + (ulong)((long)this._runState.ActFloor));
				this._ticks = rng.NextInt(12, 18);
				float num = rng.NextFloat(0.05f, 0.3f);
				this._winner = rng.NextItem<Player>(list);
				this._sortedPlayers = (from p in (from p in this._mapScreen.PlayerVoteDictionary.ToList<KeyValuePair<Player, MapCoord?>>()
						where p.Value != null
						select p).OrderBy<KeyValuePair<Player, MapCoord?>, MapCoord?>((KeyValuePair<Player, MapCoord?> p) => p.Value, Comparer<MapCoord?>.Create(new Comparison<MapCoord?>(this.MapCoordComparer))).ThenBy<KeyValuePair<Player, MapCoord?>, int>((KeyValuePair<Player, MapCoord?> p) => this._mapPointDictionary[p.Value.Value].VoteContainer.GetVoteIndex(p.Key))
					select p.Key).ToList<Player>();
				Tween tween = this._mapScreen.CreateTween();
				tween.TweenMethod(Callable.From<float>(new Action<float>(this.TickSplitVoteAnimation)), 0f, 1f, 1.2000000476837158).SetTrans(1L).SetEase(1L);
				tween.TweenInterval((double)num);
				TaskAwaiter<bool> taskAwaiter = tween.AwaitFinished(this._mapScreen).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this._mapPointDictionary[selectedCoord].VoteContainer.BouncePlayers();
				}
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x001126E8 File Offset: 0x001108E8
		private int MapCoordComparer(MapCoord? a, MapCoord? b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return 1;
			}
			if (b == null)
			{
				return -1;
			}
			if (a.Value.col != b.Value.col)
			{
				MapCoord mapCoord = a.Value;
				return mapCoord.col.CompareTo(b.Value.col);
			}
			if (a.Value.row != b.Value.row)
			{
				MapCoord mapCoord = a.Value;
				return mapCoord.row.CompareTo(b.Value.row);
			}
			return 0;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x001127BC File Offset: 0x001109BC
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

		// Token: 0x0600280F RID: 10255 RVA: 0x00112858 File Offset: 0x00110A58
		[NullableContext(2)]
		private void HighlightPlayer(Player player)
		{
			if (this._currentlyHighlightedPlayer == player)
			{
				return;
			}
			if (this._currentlyHighlightedPlayer != null)
			{
				MapCoord value = this._mapScreen.PlayerVoteDictionary[this._currentlyHighlightedPlayer].Value;
				this._mapPointDictionary[value].VoteContainer.SetPlayerHighlighted(this._currentlyHighlightedPlayer, false);
			}
			this._currentlyHighlightedPlayer = player;
			if (player != null)
			{
				MapCoord value2 = this._mapScreen.PlayerVoteDictionary[player].Value;
				this._mapPointDictionary[value2].VoteContainer.SetPlayerHighlighted(player, true);
			}
		}

		// Token: 0x04000EE1 RID: 3809
		private NMapScreen _mapScreen;

		// Token: 0x04000EE2 RID: 3810
		private RunState _runState;

		// Token: 0x04000EE3 RID: 3811
		private Dictionary<MapCoord, NMapPoint> _mapPointDictionary;

		// Token: 0x04000EE4 RID: 3812
		private int _ticks;

		// Token: 0x04000EE5 RID: 3813
		private Player _winner;

		// Token: 0x04000EE6 RID: 3814
		private List<Player> _sortedPlayers;

		// Token: 0x04000EE7 RID: 3815
		[Nullable(2)]
		private Player _currentlyHighlightedPlayer;
	}
}
