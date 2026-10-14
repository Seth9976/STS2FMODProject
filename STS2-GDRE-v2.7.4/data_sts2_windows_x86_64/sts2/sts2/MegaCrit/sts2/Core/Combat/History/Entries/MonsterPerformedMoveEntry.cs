using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D54 RID: 3412
	[NullableContext(1)]
	[Nullable(0)]
	public class MonsterPerformedMoveEntry : CombatHistoryEntry
	{
		// Token: 0x170025EB RID: 9707
		// (get) Token: 0x06008A17 RID: 35351 RVA: 0x002E3D7B File Offset: 0x002E1F7B
		public MonsterModel Monster { get; }

		// Token: 0x170025EC RID: 9708
		// (get) Token: 0x06008A18 RID: 35352 RVA: 0x002E3D83 File Offset: 0x002E1F83
		public MoveState Move { get; }

		// Token: 0x170025ED RID: 9709
		// (get) Token: 0x06008A19 RID: 35353 RVA: 0x002E3D8B File Offset: 0x002E1F8B
		[Nullable(new byte[] { 2, 1 })]
		public IEnumerable<Creature> Targets
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x170025EE RID: 9710
		// (get) Token: 0x06008A1A RID: 35354 RVA: 0x002E3D94 File Offset: 0x002E1F94
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(this.Monster.Id.Entry + " performed " + this.Move.Id);
				if (this.Targets != null)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral(" targeting ");
					string text = ",";
					IEnumerable<Creature> targets = this.Targets;
					Func<Creature, string> func;
					if ((func = MonsterPerformedMoveEntry.<>O.<0>__GetTargetName) == null)
					{
						func = (MonsterPerformedMoveEntry.<>O.<0>__GetTargetName = new Func<Creature, string>(MonsterPerformedMoveEntry.GetTargetName));
					}
					appendInterpolatedStringHandler.AppendFormatted(string.Join(text, targets.Select<Creature, string>(func)));
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06008A1B RID: 35355 RVA: 0x002E3E33 File Offset: 0x002E2033
		public MonsterPerformedMoveEntry(MonsterModel monster, MoveState move, [Nullable(new byte[] { 2, 1 })] IEnumerable<Creature> targets, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(monster.Creature, roundNumber, currentSide, history, players)
		{
			this.Monster = monster;
			this.Move = move;
			this.Targets = targets;
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x002E3E5E File Offset: 0x002E205E
		private static string GetTargetName(Creature creature)
		{
			if (!creature.IsPlayer)
			{
				return creature.Monster.Id.Entry;
			}
			return creature.Player.Character.Id.Entry;
		}

		// Token: 0x02002500 RID: 9472
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04009E15 RID: 40469
			[Nullable(0)]
			public static Func<Creature, string> <0>__GetTargetName;
		}
	}
}
