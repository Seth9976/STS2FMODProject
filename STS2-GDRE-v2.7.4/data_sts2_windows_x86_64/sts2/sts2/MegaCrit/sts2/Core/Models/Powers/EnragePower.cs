using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200066C RID: 1644
	public sealed class EnragePower : PowerModel
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06005818 RID: 22552 RVA: 0x0024E101 File Offset: 0x0024C301
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06005819 RID: 22553 RVA: 0x0024E104 File Offset: 0x0024C304
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x0024E108 File Offset: 0x0024C308
		[NullableContext(1)]
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Type == CardType.Skill)
			{
				await Cmd.Wait(0.5f, false);
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
