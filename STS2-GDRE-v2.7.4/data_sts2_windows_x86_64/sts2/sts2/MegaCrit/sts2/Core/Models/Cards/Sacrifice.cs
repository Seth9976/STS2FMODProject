using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AAA RID: 2730
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sacrifice : CardModel
	{
		// Token: 0x06007694 RID: 30356 RVA: 0x00295FDA File Offset: 0x002941DA
		public Sacrifice()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002009 RID: 8201
		// (get) Token: 0x06007695 RID: 30357 RVA: 0x00295FE7 File Offset: 0x002941E7
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x1700200A RID: 8202
		// (get) Token: 0x06007696 RID: 30358 RVA: 0x00295FF4 File Offset: 0x002941F4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x1700200B RID: 8203
		// (get) Token: 0x06007697 RID: 30359 RVA: 0x00295FFC File Offset: 0x002941FC
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700200C RID: 8204
		// (get) Token: 0x06007698 RID: 30360 RVA: 0x00296000 File Offset: 0x00294200
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(1m);
				array[2] = new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.IsOstyAlive ? (card.Owner.Osty.MaxHp * 2) : 0);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x00296060 File Offset: 0x00294260
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				decimal blockGain = base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target);
				await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
				await CreatureCmd.Kill(base.Owner.Osty, false);
				await CreatureCmd.GainBlock(base.Owner.Creature, blockGain, base.DynamicVars.CalculatedBlock.Props, cardPlay, false);
			}
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x002960AB File Offset: 0x002942AB
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
