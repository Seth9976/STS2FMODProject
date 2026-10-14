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
	// Token: 0x020009DB RID: 2523
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GatherLight : CardModel
	{
		// Token: 0x0600723C RID: 29244 RVA: 0x0028CF3B File Offset: 0x0028B13B
		public GatherLight()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E3D RID: 7741
		// (get) Token: 0x0600723D RID: 29245 RVA: 0x0028CF48 File Offset: 0x0028B148
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E3E RID: 7742
		// (get) Token: 0x0600723E RID: 29246 RVA: 0x0028CF4B File Offset: 0x0028B14B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(8m, ValueProp.Move),
					new StarsVar(1)
				});
			}
		}

		// Token: 0x0600723F RID: 29247 RVA: 0x0028CF70 File Offset: 0x0028B170
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
		}

		// Token: 0x06007240 RID: 29248 RVA: 0x0028CFBB File Offset: 0x0028B1BB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
