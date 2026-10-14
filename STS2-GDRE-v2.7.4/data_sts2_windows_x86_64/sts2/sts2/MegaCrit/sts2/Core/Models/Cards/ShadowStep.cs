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
	// Token: 0x02000ABF RID: 2751
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShadowStep : CardModel
	{
		// Token: 0x06007700 RID: 30464 RVA: 0x00296CFB File Offset: 0x00294EFB
		public ShadowStep()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002035 RID: 8245
		// (get) Token: 0x06007701 RID: 30465 RVA: 0x00296D08 File Offset: 0x00294F08
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x00296D18 File Offset: 0x00294F18
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardCmd.Discard(choiceContext, PileType.Hand.GetPile(base.Owner).Cards);
			await PowerCmd.Apply<ShadowStepPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x00296D63 File Offset: 0x00294F63
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
