using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000562 RID: 1378
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HelicalDart : RelicModel
	{
		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x00240F73 File Offset: 0x0023F173
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x00240F76 File Offset: 0x0023F176
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(1m));
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x00240F88 File Offset: 0x0023F188
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Shiv>(false).Concat<IHoverTip>(new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null)));
			}
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x00240FB4 File Offset: 0x0023F1B4
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Card.Tags.Contains(CardTag.Shiv))
				{
					base.Flash();
					await PowerCmd.Apply<HelicalDartPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.IntValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
