using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B0E RID: 2830
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheGambit : CardModel
	{
		// Token: 0x060078B4 RID: 30900 RVA: 0x0029A719 File Offset: 0x00298919
		public TheGambit()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020E4 RID: 8420
		// (get) Token: 0x060078B5 RID: 30901 RVA: 0x0029A726 File Offset: 0x00298926
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020E5 RID: 8421
		// (get) Token: 0x060078B6 RID: 30902 RVA: 0x0029A729 File Offset: 0x00298929
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(50m, ValueProp.Move));
			}
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x0029A740 File Offset: 0x00298940
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<TheGambitPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x0029A793 File Offset: 0x00298993
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(25m);
		}
	}
}
