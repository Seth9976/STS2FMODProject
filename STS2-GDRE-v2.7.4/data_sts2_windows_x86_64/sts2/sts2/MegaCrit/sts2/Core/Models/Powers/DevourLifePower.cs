using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000660 RID: 1632
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DevourLifePower : PowerModel
	{
		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x060057D2 RID: 22482 RVA: 0x0024D867 File Offset: 0x0024BA67
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x060057D3 RID: 22483 RVA: 0x0024D86A File Offset: 0x0024BA6A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x060057D4 RID: 22484 RVA: 0x0024D86D File Offset: 0x0024BA6D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<Soul>(false),
					HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x0024D894 File Offset: 0x0024BA94
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card is Soul)
			{
				if (cardPlay.Card.Owner.Creature == base.Owner)
				{
					await OstyCmd.Summon(choiceContext, cardPlay.Card.Owner, base.Amount, this);
				}
			}
		}
	}
}
