using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.TreasureRelicPicking
{
	// Token: 0x02000C5E RID: 3166
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class RelicPickingResult
	{
		/// <summary>
		/// Generates a relic fight and returns it as a relic picking result.
		/// The relic fight follows these rules:
		/// - It's a rock-paper-scissors game.
		/// - Each round, each player rolls a move using the generateMove function that is passed.
		/// - If there are not exactly two distinct moves rolled, then the round ends in a tie.
		/// - If there are two distinct moves rolled, everyone who rolled the losing move is eliminated.
		/// - The game plays rounds until there is only one player left, who is awarded the relic.
		/// </summary>
		/// <param name="players">The players involved in the fight.</param>
		/// <param name="relic">The relic that will be awarded to the winning player.</param>
		/// <param name="generateMove">Method that is called once per player every round to generate moves for each player.</param>
		/// <returns>The result of the fight. The `fight` member is guaranteed to be non-null and contain at least one round.</returns>
		// Token: 0x06008237 RID: 33335 RVA: 0x002C0AA4 File Offset: 0x002BECA4
		public static RelicPickingResult GenerateRelicFight(List<Player> players, RelicModel relic, Func<RelicPickingFightMove> generateMove)
		{
			RelicPickingFight relicPickingFight = new RelicPickingFight();
			relicPickingFight.playersInvolved.AddRange(players);
			HashSet<Player> hashSet = new HashSet<Player>();
			foreach (Player player in players)
			{
				hashSet.Add(player);
			}
			HashSet<Player> hashSet2 = hashSet;
			while (hashSet2.Count > 1)
			{
				RelicPickingFightRound relicPickingFightRound = new RelicPickingFightRound();
				foreach (Player player2 in players)
				{
					if (hashSet2.Contains(player2))
					{
						RelicPickingFightMove relicPickingFightMove = generateMove();
						relicPickingFightRound.moves.Add(new RelicPickingFightMove?(relicPickingFightMove));
					}
					else
					{
						relicPickingFightRound.moves.Add(null);
					}
				}
				relicPickingFight.rounds.Add(relicPickingFightRound);
				List<RelicPickingFightMove> list = relicPickingFightRound.moves.OfType<RelicPickingFightMove>().Distinct<RelicPickingFightMove>().ToList<RelicPickingFightMove>();
				if (list.Count == 2)
				{
					RelicPickingFightMove losingMove = RelicPickingResult.GetLosingMove(list[0], list[1]);
					for (int i = 0; i < players.Count; i++)
					{
						RelicPickingFightMove? relicPickingFightMove2 = relicPickingFightRound.moves[i];
						RelicPickingFightMove relicPickingFightMove3 = losingMove;
						if ((relicPickingFightMove2.GetValueOrDefault() == relicPickingFightMove3) & (relicPickingFightMove2 != null))
						{
							hashSet2.Remove(players[i]);
						}
					}
				}
			}
			return new RelicPickingResult
			{
				type = RelicPickingResultType.FoughtOver,
				player = hashSet2.First<Player>(),
				relic = relic,
				fight = relicPickingFight
			};
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x002C0C58 File Offset: 0x002BEE58
		private static RelicPickingFightMove GetLosingMove(RelicPickingFightMove move1, RelicPickingFightMove move2)
		{
			if ((move1 + 1) % (RelicPickingFightMove)3 == move2)
			{
				return move1;
			}
			return move2;
		}

		// Token: 0x06008239 RID: 33337 RVA: 0x002C0C65 File Offset: 0x002BEE65
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public RelicPickingResult()
		{
		}

		// Token: 0x04002BBD RID: 11197
		public RelicPickingResultType type;

		// Token: 0x04002BBE RID: 11198
		[RequiredMember]
		public RelicModel relic;

		/// <summary>
		/// This is only null if type is Skipped.
		/// </summary>
		// Token: 0x04002BBF RID: 11199
		[Nullable(2)]
		[RequiredMember]
		public Player player;

		// Token: 0x04002BC0 RID: 11200
		[Nullable(2)]
		public RelicPickingFight fight;
	}
}
