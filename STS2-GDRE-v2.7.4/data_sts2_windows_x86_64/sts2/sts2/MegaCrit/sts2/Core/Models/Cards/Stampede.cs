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
	// Token: 0x02000AE8 RID: 2792
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Stampede : CardModel
	{
		// Token: 0x060077F8 RID: 30712 RVA: 0x00298CC7 File Offset: 0x00296EC7
		public Stampede()
			: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020A0 RID: 8352
		// (get) Token: 0x060077F9 RID: 30713 RVA: 0x00298CD4 File Offset: 0x00296ED4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 1m));
			}
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x00298CEC File Offset: 0x00296EEC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<StampedePower>(choiceContext, base.Owner.Creature, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x00298D37 File Offset: 0x00296F37
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x040028C8 RID: 10440
		private const string _powerKey = "Power";
	}
}
