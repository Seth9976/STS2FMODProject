using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000109 RID: 265
	[NullableContext(1)]
	[Nullable(0)]
	public class NullRunState : IRunState, ICardScope, IPlayerCollection
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0004C924 File Offset: 0x0004AB24
		public static NullRunState Instance { get; } = new NullRunState();

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0004C92B File Offset: 0x0004AB2B
		public IReadOnlyList<Player> Players
		{
			get
			{
				return Array.Empty<Player>();
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0004C932 File Offset: 0x0004AB32
		public IReadOnlyList<ActModel> Acts
		{
			get
			{
				return ActModel.GetDefaultList();
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0004C939 File Offset: 0x0004AB39
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0004C93C File Offset: 0x0004AB3C
		public int CurrentActIndex
		{
			get
			{
				return 0;
			}
			set
			{
				throw new InvalidOperationException("Cannot set act index in a null run.");
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0004C948 File Offset: 0x0004AB48
		public ActModel Act
		{
			get
			{
				return ModelDb.Act<Overgrowth>().ToMutable();
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0004C954 File Offset: 0x0004AB54
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0004C95B File Offset: 0x0004AB5B
		public ActMap Map
		{
			get
			{
				return NullActMap.Instance;
			}
			set
			{
				throw new InvalidOperationException("Cannot set map in a null run.");
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x0004C968 File Offset: 0x0004AB68
		public MapCoord? CurrentMapCoord
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0004C97E File Offset: 0x0004AB7E
		[Nullable(2)]
		public MapPoint CurrentMapPoint
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0004C984 File Offset: 0x0004AB84
		public MapLocation MapLocation
		{
			get
			{
				return new MapLocation(null, 0);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0004C9A0 File Offset: 0x0004ABA0
		public RunLocation RunLocation
		{
			get
			{
				return new RunLocation(this.MapLocation, null);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0004C9C1 File Offset: 0x0004ABC1
		public GameMode GameMode
		{
			get
			{
				return GameMode.Standard;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0004C9C4 File Offset: 0x0004ABC4
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0004C9C7 File Offset: 0x0004ABC7
		public int ActFloor
		{
			get
			{
				return 0;
			}
			set
			{
				throw new InvalidOperationException("Cannot set act floor in a null run.");
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0004C9D3 File Offset: 0x0004ABD3
		public int TotalFloor
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0004C9D6 File Offset: 0x0004ABD6
		public IReadOnlyList<ModifierModel> Modifiers
		{
			get
			{
				return Array.Empty<ModifierModel>();
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0004C9DD File Offset: 0x0004ABDD
		public IReadOnlyList<BadgeModel> BadgeModels
		{
			get
			{
				return Array.Empty<BadgeModel>();
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0004C9E4 File Offset: 0x0004ABE4
		[Nullable(2)]
		public MultiplayerScalingModel MultiplayerScalingModel
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0004C9E7 File Offset: 0x0004ABE7
		public IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory
		{
			get
			{
				return Array.Empty<IReadOnlyList<MapPointHistoryEntry>>();
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0004C9EE File Offset: 0x0004ABEE
		[Nullable(2)]
		public MapPointHistoryEntry CurrentMapPointHistoryEntry
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0004C9F1 File Offset: 0x0004ABF1
		public int CurrentRoomCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0004C9F4 File Offset: 0x0004ABF4
		[Nullable(2)]
		public AbstractRoom CurrentRoom
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0004C9F7 File Offset: 0x0004ABF7
		[Nullable(2)]
		public AbstractRoom BaseRoom
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0004C9FA File Offset: 0x0004ABFA
		public bool IsGameOver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0004C9FD File Offset: 0x0004ABFD
		public int AscensionLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x0004CA00 File Offset: 0x0004AC00
		public RunRngSet Rng
		{
			get
			{
				return new RunRngSet(string.Empty);
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0004CA0C File Offset: 0x0004AC0C
		public RunOddsSet Odds
		{
			get
			{
				return new RunOddsSet(new Rng(0UL));
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0004CA1A File Offset: 0x0004AC1A
		public RelicGrabBag SharedRelicGrabBag
		{
			get
			{
				return new RelicGrabBag(false);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0004CA22 File Offset: 0x0004AC22
		public UnlockState UnlockState
		{
			get
			{
				return UnlockState.all;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0004CA29 File Offset: 0x0004AC29
		public ExtraRunFields ExtraFields
		{
			get
			{
				return new ExtraRunFields();
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0004CA30 File Offset: 0x0004AC30
		private NullRunState()
		{
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0004CA38 File Offset: 0x0004AC38
		[NullableContext(2)]
		public Player GetPlayer(ulong netId)
		{
			return null;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0004CA3B File Offset: 0x0004AC3B
		public int GetPlayerSlotIndex(Player player)
		{
			return -1;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0004CA40 File Offset: 0x0004AC40
		public T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel
		{
			throw new InvalidOperationException("Cannot create cards in a null run.");
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0004CA57 File Offset: 0x0004AC57
		public CardModel CreateCard(CardModel canonicalCard, Player owner)
		{
			throw new InvalidOperationException("Cannot create cards in a null run.");
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0004CA63 File Offset: 0x0004AC63
		public CardModel CloneCard(CardModel mutableCard)
		{
			throw new InvalidOperationException("Cannot clone cards in a null run.");
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0004CA6F File Offset: 0x0004AC6F
		public bool ContainsCard(CardModel card)
		{
			return false;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0004CA72 File Offset: 0x0004AC72
		public void AddCard(CardModel mutableCard, Player owner)
		{
			throw new InvalidOperationException("Cannot add cards in a null run.");
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0004CA7E File Offset: 0x0004AC7E
		public void RemoveCard(CardModel card)
		{
			throw new InvalidOperationException("Cannot remove cards in a null run.");
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0004CA8A File Offset: 0x0004AC8A
		public CardModel LoadCard(SerializableCard serializableCard, Player owner)
		{
			throw new InvalidOperationException("Cannot load cards in a null run.");
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0004CA96 File Offset: 0x0004AC96
		[NullableContext(2)]
		public void AppendToMapPointHistory(MapPointType mapPointType, RoomType initialRoomType, ModelId modelId)
		{
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0004CA98 File Offset: 0x0004AC98
		[NullableContext(2)]
		public MapPointHistoryEntry GetHistoryEntryFor(MapLocation location)
		{
			return null;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0004CA9B File Offset: 0x0004AC9B
		public int GetAndIncrementNextRoomId()
		{
			return 0;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0004CA9E File Offset: 0x0004AC9E
		public IEnumerable<AbstractModel> IterateHookListeners([Nullable(2)] ICombatState childCombatState)
		{
			return ((childCombatState != null) ? childCombatState.IterateHookListeners() : null) ?? Array.Empty<AbstractModel>();
		}
	}
}
