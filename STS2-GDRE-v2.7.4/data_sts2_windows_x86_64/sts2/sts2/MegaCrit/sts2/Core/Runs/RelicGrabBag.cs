using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x0200010B RID: 267
	[NullableContext(1)]
	[Nullable(0)]
	public class RelicGrabBag
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0004D5A8 File Offset: 0x0004B7A8
		public bool IsPopulated
		{
			get
			{
				return this._deques.Count > 0;
			}
		}

		/// <summary>
		/// Check if any relics are available to pull. Used by events to determine if they should appear.
		/// </summary>
		// Token: 0x06000BEF RID: 3055 RVA: 0x0004D5B8 File Offset: 0x0004B7B8
		public bool HasAvailableRelics(IRunState runState)
		{
			foreach (RelicRarity relicRarity in RelicGrabBag._rarities)
			{
				List<RelicModel> availableDeque = this.GetAvailableDeque(relicRarity, runState, (RelicModel _) => true);
				if (availableDeque != null)
				{
					return true;
				}
			}
			return this._mpFallbackDequeue.Any<RelicModel>((RelicModel r) => r.IsAllowed(runState));
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0004D660 File Offset: 0x0004B860
		public RelicGrabBag()
		{
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0004D67E File Offset: 0x0004B87E
		public RelicGrabBag(bool refreshAllowed)
		{
			this._refreshAllowed = refreshAllowed;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0004D6A4 File Offset: 0x0004B8A4
		public void Populate(Player player, Rng rng)
		{
			if (this.IsPopulated)
			{
				throw new InvalidOperationException("Grab bag was already populated.");
			}
			List<RelicModel> list = ModelDb.RelicPool<SharedRelicPool>().GetUnlockedRelics(player.UnlockState).ToList<RelicModel>();
			list.AddRange(player.Character.RelicPool.GetUnlockedRelics(player.UnlockState));
			list.RemoveAll((RelicModel r) => !RelicGrabBag._rarities.Contains(r.Rarity));
			this._originalRelics = list;
			foreach (RelicModel relicModel in list)
			{
				List<RelicModel> list2;
				if (!this._deques.TryGetValue(relicModel.Rarity, out list2))
				{
					list2 = new List<RelicModel>();
					this._deques[relicModel.Rarity] = list2;
				}
				list2.Add(relicModel);
			}
			foreach (List<RelicModel> list3 in this._deques.Values)
			{
				list3.UnstableShuffle<RelicModel>(rng);
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0004D7DC File Offset: 0x0004B9DC
		public void Populate(IEnumerable<RelicModel> relics, Rng rng)
		{
			if (this.IsPopulated)
			{
				throw new InvalidOperationException("Grab bag was already populated.");
			}
			this._originalRelics = relics.ToList<RelicModel>();
			foreach (RelicModel relicModel in this._originalRelics)
			{
				List<RelicModel> list;
				if (!this._deques.TryGetValue(relicModel.Rarity, out list))
				{
					list = new List<RelicModel>();
					this._deques[relicModel.Rarity] = list;
				}
				list.Add(relicModel);
			}
			foreach (List<RelicModel> list2 in this._deques.Values)
			{
				list2.UnstableShuffle<RelicModel>(rng);
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0004D8C8 File Offset: 0x0004BAC8
		[return: Nullable(2)]
		public RelicModel PullFromFront(RelicRarity rarity, IRunState runState)
		{
			return this.PullFromFront(rarity, (RelicModel _) => true, runState);
		}

		/// <summary>
		/// Get the front relic from the bucket of the specified rarity and remove it from the bucket.
		/// Used by most relic reward sources (elite combat rewards etc).
		/// </summary>
		/// <param name="rarity">Rarity of the relic we want.</param>
		/// <param name="runState">The current run state, used to check if relics are allowed.</param>
		/// <param name="filter">Filter for what type of relics we are allowed to return.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x06000BF5 RID: 3061 RVA: 0x0004D8F4 File Offset: 0x0004BAF4
		[return: Nullable(2)]
		public RelicModel PullFromFront(RelicRarity rarity, Func<RelicModel, bool> filter, IRunState runState)
		{
			List<RelicModel> availableDeque = this.GetAvailableDeque(rarity, runState, filter);
			if (availableDeque == null || availableDeque.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < availableDeque.Count; i++)
			{
				RelicModel relicModel = availableDeque[i];
				if (filter(relicModel))
				{
					availableDeque.RemoveAt(i);
					return relicModel;
				}
			}
			return null;
		}

		/// <summary>
		/// Get the back relic from the bucket of the specified rarity and remove it from the bucket.
		/// Used in shops.
		/// </summary>
		/// <param name="rarity">Rarity of the relic we want.</param>
		/// <param name="filter">Filter for what type of relics we are allowed to return.</param>
		/// <param name="runState">The current run state, used to check if relics are allowed.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x06000BF6 RID: 3062 RVA: 0x0004D944 File Offset: 0x0004BB44
		[return: Nullable(2)]
		public RelicModel PullFromBack(RelicRarity rarity, Func<RelicModel, bool> filter, IRunState runState)
		{
			List<RelicModel> availableDeque = this.GetAvailableDeque(rarity, runState, filter);
			if (availableDeque == null || availableDeque.Count == 0)
			{
				return null;
			}
			for (int i = availableDeque.Count - 1; i >= 0; i--)
			{
				RelicModel relicModel = availableDeque[i];
				if (filter(relicModel))
				{
					availableDeque.RemoveAt(i);
					return relicModel;
				}
			}
			return null;
		}

		/// <summary>
		/// Removes the specified relic from the bucket of the appropriate rarity so that it is never rolled.
		/// If it's already not there, then nothing happens.
		/// </summary>
		// Token: 0x06000BF7 RID: 3063 RVA: 0x0004D996 File Offset: 0x0004BB96
		[NullableContext(0)]
		public void Remove<T>() where T : RelicModel
		{
			this.Remove(ModelDb.Relic<T>());
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0004D9A8 File Offset: 0x0004BBA8
		public void Remove(RelicModel relic)
		{
			Predicate<RelicModel> <>9__0;
			foreach (KeyValuePair<RelicRarity, List<RelicModel>> keyValuePair in this._deques)
			{
				List<RelicModel> value = keyValuePair.Value;
				List<RelicModel> list = value;
				Predicate<RelicModel> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (RelicModel r) => r.Id == relic.Id);
				}
				list.RemoveAll(predicate);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0004DA34 File Offset: 0x0004BC34
		public void MoveToFallback(RelicModel toRemove)
		{
			RelicModel relicModel = null;
			foreach (KeyValuePair<RelicRarity, List<RelicModel>> keyValuePair in this._deques)
			{
				List<RelicModel> value = keyValuePair.Value;
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].Id == toRemove.Id)
					{
						if (relicModel == null)
						{
							relicModel = value[i];
						}
						value.RemoveAt(i);
						i--;
					}
				}
			}
			if (relicModel != null)
			{
				this._mpFallbackDequeue.Add(relicModel);
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0004DAE4 File Offset: 0x0004BCE4
		[return: Nullable(new byte[] { 2, 1 })]
		private List<RelicModel> GetAvailableDeque(RelicRarity rarity, IRunState runState, Func<RelicModel, bool> filter)
		{
			this.RemoveDisallowedRelicsFromDeques(runState);
			List<RelicModel> list = this.GetDeque(rarity);
			if (list.Count == 0 && this._refreshAllowed)
			{
				this.RefreshRarity(rarity);
				this.RemoveDisallowedRelicsFromDeques(runState);
			}
			while (list != null && !this.DequeHasAnyRelics(list, filter))
			{
				RelicRarity relicRarity;
				switch (rarity)
				{
				case RelicRarity.Common:
					relicRarity = RelicRarity.Uncommon;
					break;
				case RelicRarity.Uncommon:
					relicRarity = RelicRarity.Rare;
					break;
				case RelicRarity.Rare:
					goto IL_0055;
				case RelicRarity.Shop:
					relicRarity = RelicRarity.Common;
					break;
				default:
					goto IL_0055;
				}
				IL_0057:
				rarity = relicRarity;
				list = ((rarity == RelicRarity.None) ? null : this.GetDeque(rarity));
				continue;
				IL_0055:
				relicRarity = RelicRarity.None;
				goto IL_0057;
			}
			if (list == null && this.DequeHasAnyRelics(this._mpFallbackDequeue, filter))
			{
				list = this._mpFallbackDequeue;
			}
			return list;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0004DB80 File Offset: 0x0004BD80
		private bool DequeHasAnyRelics(List<RelicModel> deque, Func<RelicModel, bool> filter)
		{
			return deque.Any<RelicModel>(new Func<RelicModel, bool>(filter.Invoke));
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0004DB94 File Offset: 0x0004BD94
		private void RemoveDisallowedRelicsFromDeques(IRunState runState)
		{
			foreach (KeyValuePair<RelicRarity, List<RelicModel>> keyValuePair in this._deques)
			{
				List<RelicModel> value = keyValuePair.Value;
				for (int i = 0; i < value.Count; i++)
				{
					if (!value[i].IsAllowed(runState))
					{
						value.RemoveAt(i);
						i--;
					}
				}
			}
			for (int j = 0; j < this._mpFallbackDequeue.Count; j++)
			{
				if (!this._mpFallbackDequeue[j].IsAllowed(runState))
				{
					this._mpFallbackDequeue.RemoveAt(j);
					j--;
				}
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0004DC58 File Offset: 0x0004BE58
		public SerializableRelicGrabBag ToSerializable()
		{
			SerializableRelicGrabBag serializableRelicGrabBag = new SerializableRelicGrabBag();
			foreach (KeyValuePair<RelicRarity, List<RelicModel>> keyValuePair in this._deques)
			{
				RelicRarity relicRarity;
				List<RelicModel> list;
				keyValuePair.Deconstruct(out relicRarity, out list);
				RelicRarity relicRarity2 = relicRarity;
				List<RelicModel> list2 = list;
				List<ModelId> list3 = list2.Select<RelicModel, ModelId>((RelicModel r) => r.Id).ToList<ModelId>();
				serializableRelicGrabBag.RelicIdLists[relicRarity2] = list3;
			}
			return serializableRelicGrabBag;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0004DCF8 File Offset: 0x0004BEF8
		public static RelicGrabBag FromSerializable(SerializableRelicGrabBag save)
		{
			RelicGrabBag relicGrabBag = new RelicGrabBag();
			relicGrabBag.LoadFromSerializable(save);
			return relicGrabBag;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0004DD14 File Offset: 0x0004BF14
		public void LoadFromSerializable(SerializableRelicGrabBag save)
		{
			foreach (KeyValuePair<RelicRarity, List<ModelId>> keyValuePair in save.RelicIdLists)
			{
				RelicRarity relicRarity;
				List<ModelId> list;
				keyValuePair.Deconstruct(out relicRarity, out list);
				RelicRarity relicRarity2 = relicRarity;
				List<ModelId> list2 = list;
				List<RelicModel> list3;
				if (!this._deques.TryGetValue(relicRarity2, out list3))
				{
					list3 = new List<RelicModel>();
					this._deques[relicRarity2] = list3;
				}
				list3.Clear();
				foreach (ModelId modelId in list2)
				{
					RelicModel byIdOrNull = ModelDb.GetByIdOrNull<RelicModel>(modelId);
					if (byIdOrNull != null)
					{
						list3.Add(byIdOrNull);
					}
				}
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0004DDF4 File Offset: 0x0004BFF4
		private List<RelicModel> GetDeque(RelicRarity rarity)
		{
			List<RelicModel> list;
			if (!this._deques.TryGetValue(rarity, out list))
			{
				return new List<RelicModel>();
			}
			return list;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0004DE18 File Offset: 0x0004C018
		private void RefreshRarity(RelicRarity rarity)
		{
			if (this._originalRelics == null)
			{
				throw new InvalidOperationException("Tried to refresh relics but original list is null");
			}
			foreach (RelicModel relicModel in this._originalRelics)
			{
				if (relicModel.Rarity == rarity)
				{
					List<RelicModel> list;
					if (!this._deques.TryGetValue(relicModel.Rarity, out list))
					{
						list = new List<RelicModel>();
						this._deques[relicModel.Rarity] = list;
					}
					list.Add(relicModel);
				}
			}
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004DEB4 File Offset: 0x0004C0B4
		public bool Contains(RelicModel relic)
		{
			relic.AssertCanonical();
			foreach (KeyValuePair<RelicRarity, List<RelicModel>> keyValuePair in this._deques)
			{
				if (keyValuePair.Value.Contains(relic))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000422 RID: 1058
		private static readonly HashSet<RelicRarity> _rarities = new HashSet<RelicRarity>
		{
			RelicRarity.Common,
			RelicRarity.Uncommon,
			RelicRarity.Rare,
			RelicRarity.Shop
		};

		/// <summary>
		/// "Deque" is short for Double-Ended Queue.
		/// https://en.wikipedia.org/wiki/Double-ended_queue
		/// </summary>
		// Token: 0x04000423 RID: 1059
		private readonly Dictionary<RelicRarity, List<RelicModel>> _deques = new Dictionary<RelicRarity, List<RelicModel>>();

		/// <summary>
		/// This is used in multiplayer for relics that are seen at a shared relic picking treasure chest.
		/// Relics that are chosen by a player are removed from their player's grab bag, and the other relics seen at the
		/// chest are moved to this dequeue.
		/// It is not used at all for the shared relic grab bag.
		/// </summary>
		// Token: 0x04000424 RID: 1060
		private readonly List<RelicModel> _mpFallbackDequeue = new List<RelicModel>();

		// Token: 0x04000425 RID: 1061
		private readonly bool _refreshAllowed;

		// Token: 0x04000426 RID: 1062
		[Nullable(new byte[] { 2, 1 })]
		private List<RelicModel> _originalRelics;
	}
}
