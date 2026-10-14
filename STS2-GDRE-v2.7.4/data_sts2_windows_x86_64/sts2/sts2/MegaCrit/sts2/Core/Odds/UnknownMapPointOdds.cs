using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Odds
{
	/// <summary>
	/// Keeps track of the odds for different room types to be rolled when the player visits an unknown map point type.
	///
	/// Note: If a room type has negative odds, that means it should NEVER be rolled, and its odds shouldn't increase as
	/// other types are rolled. This is relevant for effects like the Deadly Enemies modifier.
	/// </summary>
	// Token: 0x02000173 RID: 371
	[NullableContext(1)]
	[Nullable(0)]
	public class UnknownMapPointOdds : AbstractOdds
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00064E06 File Offset: 0x00063006
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x00064E14 File Offset: 0x00063014
		public float MonsterOdds
		{
			get
			{
				return this._nonEventOdds[RoomType.Monster];
			}
			set
			{
				this._nonEventOdds[RoomType.Monster] = value;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00064E23 File Offset: 0x00063023
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x00064E31 File Offset: 0x00063031
		public float EliteOdds
		{
			get
			{
				return this._nonEventOdds[RoomType.Elite];
			}
			set
			{
				this._nonEventOdds[RoomType.Elite] = value;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00064E40 File Offset: 0x00063040
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x00064E4E File Offset: 0x0006304E
		public float TreasureOdds
		{
			get
			{
				return this._nonEventOdds[RoomType.Treasure];
			}
			set
			{
				this._nonEventOdds[RoomType.Treasure] = value;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x00064E5D File Offset: 0x0006305D
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x00064E6B File Offset: 0x0006306B
		public float ShopOdds
		{
			get
			{
				return this._nonEventOdds[RoomType.Shop];
			}
			set
			{
				this._nonEventOdds[RoomType.Shop] = value;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00064E7C File Offset: 0x0006307C
		public float EventOdds
		{
			get
			{
				return Math.Max(0f, 1f - this._nonEventOdds.Values.Where<float>((float v) => v > 0f).Sum());
			}
		}

		/// <summary>
		/// For creating at the start of a run.
		/// </summary>
		/// <param name="rng">RNG to use for rolls.</param>
		// Token: 0x06001153 RID: 4435 RVA: 0x00064ED0 File Offset: 0x000630D0
		public UnknownMapPointOdds(Rng rng)
		{
			Dictionary<RoomType, float> dictionary = new Dictionary<RoomType, float>();
			dictionary[RoomType.Monster] = 0.1f;
			dictionary[RoomType.Elite] = -1f;
			dictionary[RoomType.Treasure] = 0.02f;
			dictionary[RoomType.Shop] = 0.03f;
			this._baseOdds = dictionary;
			Dictionary<RoomType, float> dictionary2 = new Dictionary<RoomType, float>();
			dictionary2[RoomType.Monster] = 0.1f;
			dictionary2[RoomType.Elite] = -1f;
			dictionary2[RoomType.Treasure] = 0.02f;
			dictionary2[RoomType.Shop] = 0.03f;
			this._nonEventOdds = dictionary2;
			base..ctor(0f, rng);
		}

		/// <summary>
		/// Sets the base odds for a room type.
		/// Note that this does not set the _current_ odds. It only sets what the odds get reset to when that room is rolled.
		/// </summary>
		// Token: 0x06001154 RID: 4436 RVA: 0x00064F5F File Offset: 0x0006315F
		public void SetBaseOdds(RoomType roomType, float baseOdds)
		{
			this._baseOdds[roomType] = baseOdds;
		}

		/// <summary>
		/// Roll for the next room type and update future odds based on what's rolled.
		/// </summary>
		/// <param name="blacklist">Room types that we shouldn't be able to roll.</param>
		/// <param name="runState">The state of the run that this room is being entered in.</param>
		/// <returns>A RoomType.</returns>
		// Token: 0x06001155 RID: 4437 RVA: 0x00064F70 File Offset: 0x00063170
		public RoomType Roll(IEnumerable<RoomType> blacklist, IRunState runState)
		{
			if (runState.UnlockState.NumberOfRuns == 0)
			{
				int num = runState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> l) => l).Count<MapPointHistoryEntry>((MapPointHistoryEntry p) => p.MapPointType == MapPointType.Unknown);
				if (num < 2)
				{
					return RoomType.Event;
				}
				if (num == 2)
				{
					return RoomType.Monster;
				}
			}
			IReadOnlySet<RoomType> readOnlySet = this._nonEventOdds.Keys.Append(RoomType.Event).Except<RoomType>(blacklist).ToHashSet<RoomType>();
			readOnlySet = Hook.ModifyUnknownMapPointRoomTypes(runState, readOnlySet);
			RoomType roomType = (readOnlySet.Contains(RoomType.Event) ? RoomType.Event : readOnlySet.Order<RoomType>().First<RoomType>());
			float num2 = this._rng.NextFloat(1f);
			float num3 = 0f;
			foreach (KeyValuePair<RoomType, float> keyValuePair in this._nonEventOdds)
			{
				RoomType roomType2;
				float num4;
				keyValuePair.Deconstruct(out roomType2, out num4);
				RoomType roomType3 = roomType2;
				float num5 = num4;
				if (readOnlySet.Contains(roomType3) && num5 >= 0f)
				{
					num3 += num5;
					if (num2 <= num3)
					{
						roomType = roomType3;
						break;
					}
				}
			}
			foreach (KeyValuePair<RoomType, float> keyValuePair in this._baseOdds)
			{
				RoomType roomType2;
				float num4;
				keyValuePair.Deconstruct(out roomType2, out num4);
				RoomType roomType4 = roomType2;
				float num6 = num4;
				if (roomType == roomType4)
				{
					this._nonEventOdds[roomType4] = num6;
				}
				else if (readOnlySet.Contains(roomType4))
				{
					float num7 = Hook.ModifyOddsIncreaseForUnrolledRoomType(runState, roomType4, num6);
					Dictionary<RoomType, float> nonEventOdds = this._nonEventOdds;
					roomType2 = roomType4;
					nonEventOdds[roomType2] += num7;
				}
			}
			return roomType;
		}

		/// <summary>
		/// Reset the odds for all room types to their base level.
		/// Called between acts.
		/// </summary>
		// Token: 0x06001156 RID: 4438 RVA: 0x00065150 File Offset: 0x00063350
		public void ResetToBase()
		{
			foreach (KeyValuePair<RoomType, float> keyValuePair in this._baseOdds)
			{
				RoomType roomType;
				float num;
				keyValuePair.Deconstruct(out roomType, out num);
				RoomType roomType2 = roomType;
				float num2 = num;
				this._nonEventOdds[roomType2] = num2;
			}
		}

		// Token: 0x04000627 RID: 1575
		public const float baseMonsterOdds = 0.1f;

		// Token: 0x04000628 RID: 1576
		public const float baseEliteOdds = -1f;

		// Token: 0x04000629 RID: 1577
		public const float baseTreasureOdds = 0.02f;

		// Token: 0x0400062A RID: 1578
		public const float baseShopOdds = 0.03f;

		// Token: 0x0400062B RID: 1579
		private readonly Dictionary<RoomType, float> _baseOdds;

		/// <summary>
		/// Contains the odds of all non-Event rooms being rolled.
		/// If we roll higher than the sum of all these odds, the result will be considered an Event room.
		/// </summary>
		// Token: 0x0400062C RID: 1580
		private readonly Dictionary<RoomType, float> _nonEventOdds;
	}
}
