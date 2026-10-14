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
	// Token: 0x02000984 RID: 2436
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefendRegent : CardModel
	{
		// Token: 0x06007057 RID: 28759 RVA: 0x0028934B File Offset: 0x0028754B
		public DefendRegent()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001D63 RID: 7523
		// (get) Token: 0x06007058 RID: 28760 RVA: 0x00289358 File Offset: 0x00287558
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D64 RID: 7524
		// (get) Token: 0x06007059 RID: 28761 RVA: 0x0028935B File Offset: 0x0028755B
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x17001D65 RID: 7525
		// (get) Token: 0x0600705A RID: 28762 RVA: 0x0028936A File Offset: 0x0028756A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x0600705B RID: 28763 RVA: 0x00289380 File Offset: 0x00287580
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600705C RID: 28764 RVA: 0x002893CB File Offset: 0x002875CB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
