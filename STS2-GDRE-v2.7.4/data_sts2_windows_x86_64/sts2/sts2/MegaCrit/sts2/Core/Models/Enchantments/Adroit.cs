using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D0 RID: 2256
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Adroit : EnchantmentModel
	{
		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x06006C45 RID: 27717 RVA: 0x00281A7D File Offset: 0x0027FC7D
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x06006C46 RID: 27718 RVA: 0x00281A80 File Offset: 0x0027FC80
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x06006C47 RID: 27719 RVA: 0x00281A83 File Offset: 0x0027FC83
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(0m, ValueProp.Move));
			}
		}

		// Token: 0x06006C48 RID: 27720 RVA: 0x00281A98 File Offset: 0x0027FC98
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Card.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x00281AE3 File Offset: 0x0027FCE3
		public override void RecalculateValues()
		{
			base.DynamicVars.Block.BaseValue = base.Amount;
		}
	}
}
