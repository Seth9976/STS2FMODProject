using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Context
{
	// Token: 0x02000D28 RID: 3368
	[NullableContext(2)]
	[Nullable(0)]
	public static class LocalContext
	{
		/// <summary>
		/// The Net ID of the local player.
		/// </summary>
		// Token: 0x17002576 RID: 9590
		// (get) Token: 0x060087B4 RID: 34740 RVA: 0x002DB0A9 File Offset: 0x002D92A9
		// (set) Token: 0x060087B5 RID: 34741 RVA: 0x002DB0B0 File Offset: 0x002D92B0
		public static ulong? NetId { get; set; }

		/// <summary>
		/// Get the local player from the specified player collection.
		/// Note that the run state is a player collection.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the local player is not found.</exception>
		// Token: 0x060087B6 RID: 34742 RVA: 0x002DB0B8 File Offset: 0x002D92B8
		public static Player GetMe(IPlayerCollection playerCollection)
		{
			if (LocalContext.NetId == null || playerCollection == null)
			{
				return null;
			}
			Player player = playerCollection.GetPlayer(LocalContext.NetId.Value);
			if (player == null)
			{
				throw new InvalidOperationException("Local player not found in player collection.");
			}
			return player;
		}

		/// <summary>
		/// Get the local player from the specified serializable run.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the local player is not found.</exception>
		// Token: 0x060087B7 RID: 34743 RVA: 0x002DB0FC File Offset: 0x002D92FC
		public static SerializablePlayer GetMe(SerializableRun run)
		{
			if (LocalContext.NetId == null || run == null)
			{
				return null;
			}
			SerializablePlayer serializablePlayer = run.Players.FirstOrDefault<SerializablePlayer>((SerializablePlayer p) => p.NetId == LocalContext.NetId.Value);
			if (serializablePlayer == null)
			{
				throw new InvalidOperationException("Local player not found in serializable run.");
			}
			return serializablePlayer;
		}

		/// <summary>
		/// Get the local player from the specified combat state.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the local player is not found.</exception>
		// Token: 0x060087B8 RID: 34744 RVA: 0x002DB158 File Offset: 0x002D9358
		public static Player GetMe(ICombatState combatState)
		{
			if (LocalContext.NetId == null || combatState == null)
			{
				return null;
			}
			Player player = combatState.GetPlayer(LocalContext.NetId.Value);
			if (player == null)
			{
				throw new InvalidOperationException("Local player not found in combat.");
			}
			return player;
		}

		/// <summary>
		/// Get the local player from the specified list of players.
		/// </summary>
		// Token: 0x060087B9 RID: 34745 RVA: 0x002DB19C File Offset: 0x002D939C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static Player GetMe(IEnumerable<Player> players)
		{
			if (LocalContext.NetId == null)
			{
				return null;
			}
			return players.FirstOrDefault<Player>(delegate(Player player)
			{
				ulong netId = player.NetId;
				ulong? netId2 = LocalContext.NetId;
				return (netId == netId2.GetValueOrDefault()) & (netId2 != null);
			});
		}

		/// <summary>
		/// Get the local player's creature from the specified list of creatures.
		/// </summary>
		// Token: 0x060087BA RID: 34746 RVA: 0x002DB1E0 File Offset: 0x002D93E0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static Creature GetMe(IEnumerable<Creature> creatures)
		{
			if (LocalContext.NetId == null)
			{
				return null;
			}
			return creatures.FirstOrDefault<Creature>(delegate(Creature creature)
			{
				Player player = creature.Player;
				ulong? num = ((player != null) ? new ulong?(player.NetId) : null);
				ulong? netId = LocalContext.NetId;
				return (num.GetValueOrDefault() == netId.GetValueOrDefault()) & (num != null == (netId != null));
			});
		}

		/// <summary>
		/// Is the specified player the local player?
		/// </summary>
		// Token: 0x060087BB RID: 34747 RVA: 0x002DB224 File Offset: 0x002D9424
		public static bool IsMe(Player player)
		{
			if (player != null && LocalContext.NetId != null)
			{
				ulong netId = player.NetId;
				ulong? netId2 = LocalContext.NetId;
				return (netId == netId2.GetValueOrDefault()) & (netId2 != null);
			}
			return false;
		}

		/// <summary>
		/// Is the specified creature the local player's creature?
		/// </summary>
		// Token: 0x060087BC RID: 34748 RVA: 0x002DB262 File Offset: 0x002D9462
		public static bool IsMe(Creature creature)
		{
			return LocalContext.IsMe((creature != null) ? creature.Player : null);
		}

		/// <summary>
		/// Is the local player in the specified list of players?
		/// </summary>
		// Token: 0x060087BD RID: 34749 RVA: 0x002DB275 File Offset: 0x002D9475
		[NullableContext(1)]
		public static bool ContainsMe(IEnumerable<Player> players)
		{
			Func<Player, bool> func;
			if ((func = LocalContext.<>O.<0>__IsMe) == null)
			{
				func = (LocalContext.<>O.<0>__IsMe = new Func<Player, bool>(LocalContext.IsMe));
			}
			return players.Any<Player>(func);
		}

		/// <summary>
		/// Is the local player's creature in the specified list of creatures?
		/// </summary>
		// Token: 0x060087BE RID: 34750 RVA: 0x002DB298 File Offset: 0x002D9498
		[NullableContext(1)]
		public static bool ContainsMe(IEnumerable<Creature> creatures)
		{
			Func<Creature, bool> func;
			if ((func = LocalContext.<>O.<1>__IsMe) == null)
			{
				func = (LocalContext.<>O.<1>__IsMe = new Func<Creature, bool>(LocalContext.IsMe));
			}
			return creatures.Any<Creature>(func);
		}

		/// <summary>
		/// Does the specified card belong to the local player?
		/// </summary>
		// Token: 0x060087BF RID: 34751 RVA: 0x002DB2BB File Offset: 0x002D94BB
		public static bool IsMine(CardModel card)
		{
			return card != null && card.IsMutable && LocalContext.IsMe(card.Owner);
		}

		/// <summary>
		/// Does the specified potion belong to the local player?
		/// </summary>
		// Token: 0x060087C0 RID: 34752 RVA: 0x002DB2D5 File Offset: 0x002D94D5
		public static bool IsMine(PotionModel potion)
		{
			return potion != null && potion.IsMutable && LocalContext.IsMe(potion.Owner);
		}

		/// <summary>
		/// Does the specified relic belong to the local player?
		/// </summary>
		// Token: 0x060087C1 RID: 34753 RVA: 0x002DB2EF File Offset: 0x002D94EF
		public static bool IsMine(RelicModel relic)
		{
			return relic != null && relic.IsMutable && LocalContext.IsMe(relic.Owner);
		}

		/// <summary>
		/// Does the specified event belong to the local player?
		/// </summary>
		// Token: 0x060087C2 RID: 34754 RVA: 0x002DB309 File Offset: 0x002D9509
		public static bool IsMine(EventModel eventModel)
		{
			return eventModel != null && eventModel.IsMutable && LocalContext.IsMe(eventModel.Owner);
		}

		// Token: 0x02002439 RID: 9273
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040098FD RID: 39165
			[Nullable(0)]
			public static Func<Player, bool> <0>__IsMe;

			// Token: 0x040098FE RID: 39166
			[Nullable(0)]
			public static Func<Creature, bool> <1>__IsMe;
		}
	}
}
