using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D8 RID: 1752
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReboundPower : PowerModel
	{
		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x0025289B File Offset: 0x00250A9B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x0025289E File Offset: 0x00250A9E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x002528A1 File Offset: 0x00250AA1
		public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation location)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return location;
			}
			if (location.pileType != PileType.Discard)
			{
				return location;
			}
			location.pileType = PileType.Draw;
			location.position = CardPilePosition.Top;
			return location;
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x002528D8 File Offset: 0x00250AD8
		public override async Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation location)
		{
			if (card.Owner.Creature == base.Owner)
			{
				base.Flash();
				await PowerCmd.Decrement(this);
			}
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x00252924 File Offset: 0x00250B24
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
