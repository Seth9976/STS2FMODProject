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
	// Token: 0x02000A26 RID: 2598
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Leap : CardModel
	{
		// Token: 0x060073C5 RID: 29637 RVA: 0x00290107 File Offset: 0x0028E307
		public Leap()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001EDF RID: 7903
		// (get) Token: 0x060073C6 RID: 29638 RVA: 0x00290114 File Offset: 0x0028E314
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EE0 RID: 7904
		// (get) Token: 0x060073C7 RID: 29639 RVA: 0x00290117 File Offset: 0x0028E317
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x0029012C File Offset: 0x0028E32C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x00290177 File Offset: 0x0028E377
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
