using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A9E RID: 2718
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Relax : CardModel
	{
		// Token: 0x0600764F RID: 30287 RVA: 0x002956B7 File Offset: 0x002938B7
		public Relax()
			: base(3, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001FEB RID: 8171
		// (get) Token: 0x06007650 RID: 30288 RVA: 0x002956C4 File Offset: 0x002938C4
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001FEC RID: 8172
		// (get) Token: 0x06007651 RID: 30289 RVA: 0x002956C7 File Offset: 0x002938C7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(15m, ValueProp.Move),
					new CardsVar(2),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x17001FED RID: 8173
		// (get) Token: 0x06007652 RID: 30290 RVA: 0x002956F6 File Offset: 0x002938F6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001FEE RID: 8174
		// (get) Token: 0x06007653 RID: 30291 RVA: 0x00295703 File Offset: 0x00293903
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007654 RID: 30292 RVA: 0x0029570C File Offset: 0x0029390C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007655 RID: 30293 RVA: 0x00295760 File Offset: 0x00293960
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
