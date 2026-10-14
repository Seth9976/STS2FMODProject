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
	// Token: 0x020009E4 RID: 2532
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glow : CardModel
	{
		// Token: 0x06007272 RID: 29298 RVA: 0x0028D572 File Offset: 0x0028B772
		public Glow()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E53 RID: 7763
		// (get) Token: 0x06007273 RID: 29299 RVA: 0x0028D57F File Offset: 0x0028B77F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StarsVar(1),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06007274 RID: 29300 RVA: 0x0028D5A0 File Offset: 0x0028B7A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007275 RID: 29301 RVA: 0x0028D5EB File Offset: 0x0028B7EB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Stars.UpgradeValueBy(1m);
		}
	}
}
