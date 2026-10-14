using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D1 RID: 1489
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RippleBasin : RelicModel
	{
		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x00246945 File Offset: 0x00244B45
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06005473 RID: 21619 RVA: 0x00246948 File Offset: 0x00244B48
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(4m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06005474 RID: 21620 RVA: 0x0024695B File Offset: 0x00244B5B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x00246970 File Offset: 0x00244B70
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				bool flag = CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.Owner.Creature.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == base.Owner);
				if (!flag)
				{
					base.Flash();
					base.Status = RelicStatus.Normal;
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
				}
			}
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x002469BB File Offset: 0x00244BBB
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x002469F6 File Offset: 0x00244BF6
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Active;
			return Task.CompletedTask;
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x00246A1D File Offset: 0x00244C1D
		public override Task AfterCombatEnd(CombatRoom room)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}
	}
}
