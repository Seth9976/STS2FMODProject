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
	// Token: 0x02000985 RID: 2437
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefendSilent : CardModel
	{
		// Token: 0x0600705D RID: 28765 RVA: 0x002893E3 File Offset: 0x002875E3
		public DefendSilent()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001D66 RID: 7526
		// (get) Token: 0x0600705E RID: 28766 RVA: 0x002893F0 File Offset: 0x002875F0
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D67 RID: 7527
		// (get) Token: 0x0600705F RID: 28767 RVA: 0x002893F3 File Offset: 0x002875F3
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x17001D68 RID: 7528
		// (get) Token: 0x06007060 RID: 28768 RVA: 0x00289402 File Offset: 0x00287602
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x00289418 File Offset: 0x00287618
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x00289463 File Offset: 0x00287663
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
