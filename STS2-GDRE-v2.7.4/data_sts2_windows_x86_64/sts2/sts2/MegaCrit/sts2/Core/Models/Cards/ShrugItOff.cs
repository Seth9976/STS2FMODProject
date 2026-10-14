using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC7 RID: 2759
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShrugItOff : CardModel
	{
		// Token: 0x06007734 RID: 30516 RVA: 0x0029739A File Offset: 0x0029559A
		public ShrugItOff()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x1700204E RID: 8270
		// (get) Token: 0x06007735 RID: 30517 RVA: 0x002973A7 File Offset: 0x002955A7
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700204F RID: 8271
		// (get) Token: 0x06007736 RID: 30518 RVA: 0x002973AA File Offset: 0x002955AA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(8m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x002973D0 File Offset: 0x002955D0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x00297423 File Offset: 0x00295623
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
