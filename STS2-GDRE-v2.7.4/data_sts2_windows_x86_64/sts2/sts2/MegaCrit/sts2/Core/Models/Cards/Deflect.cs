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
	// Token: 0x02000987 RID: 2439
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deflect : CardModel
	{
		// Token: 0x06007068 RID: 28776 RVA: 0x0028950F File Offset: 0x0028770F
		public Deflect()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001D6B RID: 7531
		// (get) Token: 0x06007069 RID: 28777 RVA: 0x0028951C File Offset: 0x0028771C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D6C RID: 7532
		// (get) Token: 0x0600706A RID: 28778 RVA: 0x0028951F File Offset: 0x0028771F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(4m, ValueProp.Move));
			}
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x00289534 File Offset: 0x00287734
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x0028957F File Offset: 0x0028777F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
