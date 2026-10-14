using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ADC RID: 2780
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Speedster : CardModel
	{
		// Token: 0x060077B3 RID: 30643 RVA: 0x002982ED File Offset: 0x002964ED
		public Speedster()
			: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002084 RID: 8324
		// (get) Token: 0x060077B4 RID: 30644 RVA: 0x002982FA File Offset: 0x002964FA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SpeedsterPower>(2m));
			}
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x0029830C File Offset: 0x0029650C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<SpeedsterPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SpeedsterPower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x00298357 File Offset: 0x00296557
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
