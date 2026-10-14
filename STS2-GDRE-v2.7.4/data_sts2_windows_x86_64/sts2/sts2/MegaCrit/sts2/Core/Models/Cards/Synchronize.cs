using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x02000B02 RID: 2818
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Synchronize : CardModel
	{
		// Token: 0x06007874 RID: 30836 RVA: 0x00299DD9 File Offset: 0x00297FD9
		public Synchronize()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020CB RID: 8395
		// (get) Token: 0x06007875 RID: 30837 RVA: 0x00299DE8 File Offset: 0x00297FE8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x170020CC RID: 8396
		// (get) Token: 0x06007876 RID: 30838 RVA: 0x00299E08 File Offset: 0x00298008
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(2m);
				array[2] = new CalculatedVar("CalculatedFocus").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => (from orb in card.Owner.PlayerCombatState.OrbQueue.Orbs
					group orb by orb.Id).Count<IGrouping<ModelId, OrbModel>>());
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x170020CD RID: 8397
		// (get) Token: 0x06007877 RID: 30839 RVA: 0x00299E6C File Offset: 0x0029806C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x00299E74 File Offset: 0x00298074
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<SynchronizePower>(choiceContext, base.Owner.Creature, ((CalculatedVar)base.DynamicVars["CalculatedFocus"]).Calculate(cardPlay.Target), base.Owner.Creature, this, false);
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x00299EC7 File Offset: 0x002980C7
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}

		// Token: 0x040028C9 RID: 10441
		private const string _calculatedFocusKey = "CalculatedFocus";
	}
}
