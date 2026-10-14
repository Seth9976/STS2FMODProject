using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009AF RID: 2479
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EscapePlan : CardModel
	{
		// Token: 0x06007143 RID: 28995 RVA: 0x0028ADE3 File Offset: 0x00288FE3
		public EscapePlan()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DD1 RID: 7633
		// (get) Token: 0x06007144 RID: 28996 RVA: 0x0028ADF0 File Offset: 0x00288FF0
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DD2 RID: 7634
		// (get) Token: 0x06007145 RID: 28997 RVA: 0x0028ADF3 File Offset: 0x00288FF3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(3m, ValueProp.Move));
			}
		}

		// Token: 0x06007146 RID: 28998 RVA: 0x0028AE08 File Offset: 0x00289008
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = await CardPileCmd.Draw(choiceContext, 1m, base.Owner, false);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null && cardModel.Type == CardType.Skill)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
		}

		// Token: 0x06007147 RID: 28999 RVA: 0x0028AE5B File Offset: 0x0028905B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
