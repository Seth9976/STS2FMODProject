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
	// Token: 0x02000B2C RID: 2860
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Untouchable : CardModel
	{
		// Token: 0x06007959 RID: 31065 RVA: 0x0029BA1F File Offset: 0x00299C1F
		public Untouchable()
			: base(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17002127 RID: 8487
		// (get) Token: 0x0600795A RID: 31066 RVA: 0x0029BA2C File Offset: 0x00299C2C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002128 RID: 8488
		// (get) Token: 0x0600795B RID: 31067 RVA: 0x0029BA2F File Offset: 0x00299C2F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x17002129 RID: 8489
		// (get) Token: 0x0600795C RID: 31068 RVA: 0x0029BA42 File Offset: 0x00299C42
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x0029BA4C File Offset: 0x00299C4C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x0029BA97 File Offset: 0x00299C97
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
