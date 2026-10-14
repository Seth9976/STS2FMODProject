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
	// Token: 0x0200092B RID: 2347
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BootSequence : CardModel
	{
		// Token: 0x06006E94 RID: 28308 RVA: 0x00285A6B File Offset: 0x00283C6B
		public BootSequence()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x06006E95 RID: 28309 RVA: 0x00285A78 File Offset: 0x00283C78
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x06006E96 RID: 28310 RVA: 0x00285A7B File Offset: 0x00283C7B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x06006E97 RID: 28311 RVA: 0x00285A8F File Offset: 0x00283C8F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Innate,
					CardKeyword.Exhaust
				});
			}
		}

		// Token: 0x06006E98 RID: 28312 RVA: 0x00285AA4 File Offset: 0x00283CA4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06006E99 RID: 28313 RVA: 0x00285AEF File Offset: 0x00283CEF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
