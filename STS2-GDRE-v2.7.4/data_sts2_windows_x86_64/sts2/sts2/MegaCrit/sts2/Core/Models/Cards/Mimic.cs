using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A3F RID: 2623
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Mimic : CardModel
	{
		// Token: 0x06007461 RID: 29793 RVA: 0x002916E6 File Offset: 0x0028F8E6
		public Mimic()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001F1D RID: 7965
		// (get) Token: 0x06007462 RID: 29794 RVA: 0x002916F3 File Offset: 0x0028F8F3
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001F1E RID: 7966
		// (get) Token: 0x06007463 RID: 29795 RVA: 0x002916F6 File Offset: 0x0028F8F6
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F1F RID: 7967
		// (get) Token: 0x06007464 RID: 29796 RVA: 0x00291700 File Offset: 0x0028F900
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(1m);
				array[2] = new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature target) => (target != null) ? target.Block : 0);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001F20 RID: 7968
		// (get) Token: 0x06007465 RID: 29797 RVA: 0x0029175F File Offset: 0x0028F95F
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x00291764 File Offset: 0x0028F964
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay, false);
		}

		// Token: 0x06007467 RID: 29799 RVA: 0x002917AF File Offset: 0x0028F9AF
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
