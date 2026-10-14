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
	// Token: 0x02000996 RID: 2454
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DodgeAndRoll : CardModel
	{
		// Token: 0x060070BB RID: 28859 RVA: 0x00289E95 File Offset: 0x00288095
		public DodgeAndRoll()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001D92 RID: 7570
		// (get) Token: 0x060070BC RID: 28860 RVA: 0x00289EA2 File Offset: 0x002880A2
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D93 RID: 7571
		// (get) Token: 0x060070BD RID: 28861 RVA: 0x00289EA5 File Offset: 0x002880A5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(4m, ValueProp.Move));
			}
		}

		// Token: 0x060070BE RID: 28862 RVA: 0x00289EB8 File Offset: 0x002880B8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			decimal num = await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			decimal num2 = num;
			await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, base.Owner.Creature, num2, base.Owner.Creature, this, false);
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x00289F0B File Offset: 0x0028810B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
