using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A82 RID: 2690
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Prolong : CardModel
	{
		// Token: 0x060075BB RID: 30139 RVA: 0x0029434E File Offset: 0x0029254E
		public Prolong()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FAE RID: 8110
		// (get) Token: 0x060075BC RID: 30140 RVA: 0x0029435B File Offset: 0x0029255B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001FAF RID: 8111
		// (get) Token: 0x060075BD RID: 30141 RVA: 0x0029436D File Offset: 0x0029256D
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x00294378 File Offset: 0x00292578
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			Creature creature = base.Owner.Creature;
			await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, creature, creature.Block, creature, this, false);
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x002943C3 File Offset: 0x002925C3
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
