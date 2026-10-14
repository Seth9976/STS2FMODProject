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
	// Token: 0x02000A00 RID: 2560
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HiddenCache : CardModel
	{
		// Token: 0x060072FF RID: 29439 RVA: 0x0028E80E File Offset: 0x0028CA0E
		public HiddenCache()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E8D RID: 7821
		// (get) Token: 0x06007300 RID: 29440 RVA: 0x0028E81B File Offset: 0x0028CA1B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StarsVar(1),
					new PowerVar<StarNextTurnPower>(3m)
				});
			}
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x0028E840 File Offset: 0x0028CA40
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			await PowerCmd.Apply<StarNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StarNextTurnPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x0028E88B File Offset: 0x0028CA8B
		protected override void OnUpgrade()
		{
			base.DynamicVars["StarNextTurnPower"].UpgradeValueBy(1m);
		}
	}
}
