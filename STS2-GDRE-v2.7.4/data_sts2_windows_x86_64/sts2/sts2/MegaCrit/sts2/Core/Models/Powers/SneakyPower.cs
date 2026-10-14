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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F8 RID: 1784
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SneakyPower : PowerModel
	{
		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06005B51 RID: 23377 RVA: 0x00254103 File Offset: 0x00252303
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06005B52 RID: 23378 RVA: 0x00254106 File Offset: 0x00252306
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x00254109 File Offset: 0x00252309
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0025411C File Offset: 0x0025231C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature != base.Owner)
			{
				if (cardPlay.Card.Type == CardType.Attack)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, true);
				}
			}
		}
	}
}
