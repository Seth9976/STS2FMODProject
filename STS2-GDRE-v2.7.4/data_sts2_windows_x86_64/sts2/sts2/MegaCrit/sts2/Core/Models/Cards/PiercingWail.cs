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
	// Token: 0x02000A72 RID: 2674
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PiercingWail : CardModel
	{
		// Token: 0x0600756D RID: 30061 RVA: 0x0029391C File Offset: 0x00291B1C
		public PiercingWail()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001F91 RID: 8081
		// (get) Token: 0x0600756E RID: 30062 RVA: 0x00293929 File Offset: 0x00291B29
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("StrengthLoss", 6m));
			}
		}

		// Token: 0x17001F92 RID: 8082
		// (get) Token: 0x0600756F RID: 30063 RVA: 0x00293940 File Offset: 0x00291B40
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F93 RID: 8083
		// (get) Token: 0x06007570 RID: 30064 RVA: 0x00293948 File Offset: 0x00291B48
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x00293968 File Offset: 0x00291B68
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (Creature creature in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<PiercingWailPower>(choiceContext, creature, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x002939B3 File Offset: 0x00291BB3
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(2m);
		}

		// Token: 0x040028AF RID: 10415
		private const string _strengthLossKey = "StrengthLoss";
	}
}
