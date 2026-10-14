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
	// Token: 0x02000981 RID: 2433
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefendDefect : CardModel
	{
		// Token: 0x06007045 RID: 28741 RVA: 0x00289183 File Offset: 0x00287383
		public DefendDefect()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001D5A RID: 7514
		// (get) Token: 0x06007046 RID: 28742 RVA: 0x00289190 File Offset: 0x00287390
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D5B RID: 7515
		// (get) Token: 0x06007047 RID: 28743 RVA: 0x00289193 File Offset: 0x00287393
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x17001D5C RID: 7516
		// (get) Token: 0x06007048 RID: 28744 RVA: 0x002891A2 File Offset: 0x002873A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x002891B8 File Offset: 0x002873B8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x00289203 File Offset: 0x00287403
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
