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
	// Token: 0x02000954 RID: 2388
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloakOfStars : CardModel
	{
		// Token: 0x06006F5D RID: 28509 RVA: 0x0028742E File Offset: 0x0028562E
		public CloakOfStars()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001CF3 RID: 7411
		// (get) Token: 0x06006F5E RID: 28510 RVA: 0x0028743B File Offset: 0x0028563B
		public override int CanonicalStarCost
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001CF4 RID: 7412
		// (get) Token: 0x06006F5F RID: 28511 RVA: 0x0028743E File Offset: 0x0028563E
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CF5 RID: 7413
		// (get) Token: 0x06006F60 RID: 28512 RVA: 0x00287441 File Offset: 0x00285641
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x00287454 File Offset: 0x00285654
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x0028749F File Offset: 0x0028569F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
