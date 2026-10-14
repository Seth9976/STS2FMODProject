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
	// Token: 0x02000AA0 RID: 2720
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Resonance : CardModel
	{
		// Token: 0x0600765B RID: 30299 RVA: 0x002958BA File Offset: 0x00293ABA
		public Resonance()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001FF0 RID: 8176
		// (get) Token: 0x0600765C RID: 30300 RVA: 0x002958C7 File Offset: 0x00293AC7
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001FF1 RID: 8177
		// (get) Token: 0x0600765D RID: 30301 RVA: 0x002958CA File Offset: 0x00293ACA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(1m));
			}
		}

		// Token: 0x17001FF2 RID: 8178
		// (get) Token: 0x0600765E RID: 30302 RVA: 0x002958DC File Offset: 0x00293ADC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x0600765F RID: 30303 RVA: 0x002958FC File Offset: 0x00293AFC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int intValue = base.DynamicVars["StrengthPower"].IntValue;
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, intValue, base.Owner.Creature, this, false);
			foreach (Creature creature in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<StrengthPower>(choiceContext, creature, -1m, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06007660 RID: 30304 RVA: 0x00295947 File Offset: 0x00293B47
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthPower"].UpgradeValueBy(1m);
		}
	}
}
