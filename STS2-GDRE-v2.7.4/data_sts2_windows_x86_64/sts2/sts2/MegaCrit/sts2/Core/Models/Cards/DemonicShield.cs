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
	// Token: 0x0200098D RID: 2445
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DemonicShield : CardModel
	{
		// Token: 0x0600708A RID: 28810 RVA: 0x00289917 File Offset: 0x00287B17
		public DemonicShield()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001D7B RID: 7547
		// (get) Token: 0x0600708B RID: 28811 RVA: 0x00289924 File Offset: 0x00287B24
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001D7C RID: 7548
		// (get) Token: 0x0600708C RID: 28812 RVA: 0x00289927 File Offset: 0x00287B27
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D7D RID: 7549
		// (get) Token: 0x0600708D RID: 28813 RVA: 0x0028992A File Offset: 0x00287B2A
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001D7E RID: 7550
		// (get) Token: 0x0600708E RID: 28814 RVA: 0x00289934 File Offset: 0x00287B34
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new HpLossVar(1m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.Creature.Block);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x002899A0 File Offset: 0x00287BA0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			await CreatureCmd.GainBlock(cardPlay.Target, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay, false);
		}

		// Token: 0x06007090 RID: 28816 RVA: 0x002899F3 File Offset: 0x00287BF3
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
