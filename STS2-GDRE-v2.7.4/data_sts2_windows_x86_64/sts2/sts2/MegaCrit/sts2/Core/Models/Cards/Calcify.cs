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
	// Token: 0x02000941 RID: 2369
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Calcify : CardModel
	{
		// Token: 0x06006EFE RID: 28414 RVA: 0x00286875 File Offset: 0x00284A75
		public Calcify()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x06006EFF RID: 28415 RVA: 0x00286882 File Offset: 0x00284A82
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<CalcifyPower>(4m));
			}
		}

		// Token: 0x06006F00 RID: 28416 RVA: 0x00286894 File Offset: 0x00284A94
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CalcifyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CalcifyPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F01 RID: 28417 RVA: 0x002868DF File Offset: 0x00284ADF
		protected override void OnUpgrade()
		{
			base.DynamicVars["CalcifyPower"].UpgradeValueBy(2m);
		}
	}
}
