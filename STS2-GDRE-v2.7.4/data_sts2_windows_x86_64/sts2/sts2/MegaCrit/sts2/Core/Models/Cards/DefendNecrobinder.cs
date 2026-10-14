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
	// Token: 0x02000983 RID: 2435
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefendNecrobinder : CardModel
	{
		// Token: 0x06007051 RID: 28753 RVA: 0x002892B3 File Offset: 0x002874B3
		public DefendNecrobinder()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001D60 RID: 7520
		// (get) Token: 0x06007052 RID: 28754 RVA: 0x002892C0 File Offset: 0x002874C0
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D61 RID: 7521
		// (get) Token: 0x06007053 RID: 28755 RVA: 0x002892C3 File Offset: 0x002874C3
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x17001D62 RID: 7522
		// (get) Token: 0x06007054 RID: 28756 RVA: 0x002892D2 File Offset: 0x002874D2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x06007055 RID: 28757 RVA: 0x002892E8 File Offset: 0x002874E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007056 RID: 28758 RVA: 0x00289333 File Offset: 0x00287533
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
