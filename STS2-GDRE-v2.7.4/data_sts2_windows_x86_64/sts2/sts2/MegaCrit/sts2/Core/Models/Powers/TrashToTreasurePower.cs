using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000723 RID: 1827
	public sealed class TrashToTreasurePower : PowerModel
	{
		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06005C5E RID: 23646 RVA: 0x0025625F File Offset: 0x0025445F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06005C5F RID: 23647 RVA: 0x00256262 File Offset: 0x00254462
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C60 RID: 23648 RVA: 0x00256268 File Offset: 0x00254468
		[NullableContext(1)]
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (card.Type == CardType.Status)
			{
				if (creator != null && creator.Creature == base.Owner)
				{
					base.Flash();
					for (int i = 0; i < base.Amount; i++)
					{
						OrbModel orbModel = OrbModel.GetRandomOrb(base.Owner.Player.RunState.Rng.CombatOrbGeneration).ToMutable(0);
						await OrbCmd.Channel(new ThrowingPlayerChoiceContext(), orbModel, base.Owner.Player);
					}
				}
			}
		}
	}
}
