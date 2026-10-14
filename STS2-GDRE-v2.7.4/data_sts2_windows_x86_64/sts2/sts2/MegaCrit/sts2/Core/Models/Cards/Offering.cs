using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A5C RID: 2652
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Offering : CardModel
	{
		// Token: 0x060074FD RID: 29949 RVA: 0x00292AE6 File Offset: 0x00290CE6
		public Offering()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F64 RID: 8036
		// (get) Token: 0x060074FE RID: 29950 RVA: 0x00292AF3 File Offset: 0x00290CF3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(6m),
					new EnergyVar(2),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x17001F65 RID: 8037
		// (get) Token: 0x060074FF RID: 29951 RVA: 0x00292B20 File Offset: 0x00290D20
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F66 RID: 8038
		// (get) Token: 0x06007500 RID: 29952 RVA: 0x00292B28 File Offset: 0x00290D28
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x00292B38 File Offset: 0x00290D38
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x00292B8B File Offset: 0x00290D8B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(2m);
		}
	}
}
