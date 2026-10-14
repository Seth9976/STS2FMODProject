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
	// Token: 0x020009DC RID: 2524
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Genesis : CardModel
	{
		// Token: 0x06007241 RID: 29249 RVA: 0x0028CFD3 File Offset: 0x0028B1D3
		public Genesis()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001E3F RID: 7743
		// (get) Token: 0x06007242 RID: 29250 RVA: 0x0028CFE0 File Offset: 0x0028B1E0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("StarsPerTurn", 2m));
			}
		}

		// Token: 0x06007243 RID: 29251 RVA: 0x0028CFF8 File Offset: 0x0028B1F8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<GenesisPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StarsPerTurn"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007244 RID: 29252 RVA: 0x0028D043 File Offset: 0x0028B243
		protected override void OnUpgrade()
		{
			base.DynamicVars["StarsPerTurn"].UpgradeValueBy(1m);
		}

		// Token: 0x04002873 RID: 10355
		private const string _starsPerTurnKey = "StarsPerTurn";
	}
}
