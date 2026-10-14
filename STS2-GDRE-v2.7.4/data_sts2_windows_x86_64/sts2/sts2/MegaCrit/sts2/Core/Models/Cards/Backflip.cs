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
	// Token: 0x02000909 RID: 2313
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Backflip : CardModel
	{
		// Token: 0x06006DDF RID: 28127 RVA: 0x00284299 File Offset: 0x00282499
		public Backflip()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x06006DE0 RID: 28128 RVA: 0x002842A6 File Offset: 0x002824A6
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x06006DE1 RID: 28129 RVA: 0x002842A9 File Offset: 0x002824A9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(5m, ValueProp.Move),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x06006DE2 RID: 28130 RVA: 0x002842D0 File Offset: 0x002824D0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x00284323 File Offset: 0x00282523
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
