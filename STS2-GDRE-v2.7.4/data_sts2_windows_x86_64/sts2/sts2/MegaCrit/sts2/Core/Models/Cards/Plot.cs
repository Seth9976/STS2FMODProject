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
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A76 RID: 2678
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Plot : CardModel
	{
		// Token: 0x06007584 RID: 30084 RVA: 0x00293C53 File Offset: 0x00291E53
		public Plot()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001F98 RID: 8088
		// (get) Token: 0x06007585 RID: 30085 RVA: 0x00293C60 File Offset: 0x00291E60
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001F99 RID: 8089
		// (get) Token: 0x06007586 RID: 30086 RVA: 0x00293C63 File Offset: 0x00291E63
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x00293C70 File Offset: 0x00291E70
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer
				select c;
			foreach (Creature creature in enumerable)
			{
				await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x00293CBB File Offset: 0x00291EBB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
