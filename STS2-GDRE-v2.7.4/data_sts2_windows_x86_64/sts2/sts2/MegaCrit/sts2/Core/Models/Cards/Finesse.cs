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
	// Token: 0x020009C3 RID: 2499
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Finesse : CardModel
	{
		// Token: 0x060071B7 RID: 29111 RVA: 0x0028BD63 File Offset: 0x00289F63
		public Finesse()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E06 RID: 7686
		// (get) Token: 0x060071B8 RID: 29112 RVA: 0x0028BD70 File Offset: 0x00289F70
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E07 RID: 7687
		// (get) Token: 0x060071B9 RID: 29113 RVA: 0x0028BD73 File Offset: 0x00289F73
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(4m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x0028BD98 File Offset: 0x00289F98
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x0028BDB0 File Offset: 0x00289FB0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}
	}
}
