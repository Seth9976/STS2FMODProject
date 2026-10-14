using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000947 RID: 2375
	public sealed class Cascade : CardModel
	{
		// Token: 0x06006F1A RID: 28442 RVA: 0x00286BCF File Offset: 0x00284DCF
		public Cascade()
			: base(-1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x06006F1B RID: 28443 RVA: 0x00286BDC File Offset: 0x00284DDC
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x00286BE0 File Offset: 0x00284DE0
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num = base.ResolveEnergyXValue();
			if (base.IsUpgraded)
			{
				num++;
			}
			await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner, num, CardPilePosition.Top, false);
		}
	}
}
