using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE7 RID: 2791
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Stack : CardModel
	{
		// Token: 0x060077F2 RID: 30706 RVA: 0x00298BEC File Offset: 0x00296DEC
		public Stack()
			: base(1, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x1700209D RID: 8349
		// (get) Token: 0x060077F3 RID: 30707 RVA: 0x00298BF9 File Offset: 0x00296DF9
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<DefectCardPool>();
			}
		}

		// Token: 0x1700209E RID: 8350
		// (get) Token: 0x060077F4 RID: 30708 RVA: 0x00298C00 File Offset: 0x00296E00
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700209F RID: 8351
		// (get) Token: 0x060077F5 RID: 30709 RVA: 0x00298C04 File Offset: 0x00296E04
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(1m);
				array[2] = new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => PileType.Discard.GetPile(card.Owner).Cards.Count<CardModel>());
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x00298C64 File Offset: 0x00296E64
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay, false);
		}

		// Token: 0x060077F7 RID: 30711 RVA: 0x00298CAF File Offset: 0x00296EAF
		protected override void OnUpgrade()
		{
			base.DynamicVars.CalculationBase.UpgradeValueBy(3m);
		}
	}
}
