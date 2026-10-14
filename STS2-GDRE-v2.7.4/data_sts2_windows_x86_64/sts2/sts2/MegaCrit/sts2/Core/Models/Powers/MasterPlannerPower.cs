using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006AC RID: 1708
	public sealed class MasterPlannerPower : PowerModel
	{
		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06005979 RID: 22905 RVA: 0x00250931 File Offset: 0x0024EB31
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x0600597A RID: 22906 RVA: 0x00250934 File Offset: 0x0024EB34
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x00250938 File Offset: 0x0024EB38
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Skill)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			CardCmd.ApplyKeyword(cardPlay.Card, new CardKeyword[] { CardKeyword.Sly });
			return Task.CompletedTask;
		}
	}
}
