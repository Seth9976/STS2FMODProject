using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000630 RID: 1584
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AutomationPower : PowerModel
	{
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x060056C2 RID: 22210 RVA: 0x0024BBB7 File Offset: 0x00249DB7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060056C3 RID: 22211 RVA: 0x0024BBBA File Offset: 0x00249DBA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060056C4 RID: 22212 RVA: 0x0024BBBD File Offset: 0x00249DBD
		public override int DisplayAmount
		{
			get
			{
				return base.GetInternalData<AutomationPower.Data>().cardsLeft;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060056C5 RID: 22213 RVA: 0x0024BBCA File Offset: 0x00249DCA
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060056C6 RID: 22214 RVA: 0x0024BBCD File Offset: 0x00249DCD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BaseCards", 10m));
			}
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x0024BBE5 File Offset: 0x00249DE5
		protected override object InitInternalData()
		{
			return new AutomationPower.Data();
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x0024BBEC File Offset: 0x00249DEC
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner == base.Owner.Player)
			{
				AutomationPower.Data data = base.GetInternalData<AutomationPower.Data>();
				data.cardsLeft--;
				base.InvokeDisplayAmountChanged();
				if (data.cardsLeft <= 0)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.Amount, base.Owner.Player);
					data.cardsLeft = 10;
					base.InvokeDisplayAmountChanged();
				}
			}
		}

		// Token: 0x040024EF RID: 9455
		private const int _baseCardsLeft = 10;

		// Token: 0x040024F0 RID: 9456
		private const string _baseCardsKey = "BaseCards";

		// Token: 0x02001B7B RID: 7035
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006D36 RID: 27958
			public int cardsLeft = 10;
		}
	}
}
