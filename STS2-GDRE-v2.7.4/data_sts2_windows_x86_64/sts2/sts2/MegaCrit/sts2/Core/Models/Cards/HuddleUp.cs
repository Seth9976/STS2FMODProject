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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A07 RID: 2567
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HuddleUp : CardModel
	{
		// Token: 0x06007327 RID: 29479 RVA: 0x0028ED07 File Offset: 0x0028CF07
		public HuddleUp()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001EA0 RID: 7840
		// (get) Token: 0x06007328 RID: 29480 RVA: 0x0028ED14 File Offset: 0x0028CF14
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001EA1 RID: 7841
		// (get) Token: 0x06007329 RID: 29481 RVA: 0x0028ED17 File Offset: 0x0028CF17
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x17001EA2 RID: 7842
		// (get) Token: 0x0600732A RID: 29482 RVA: 0x0028ED24 File Offset: 0x0028CF24
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x0028ED2C File Offset: 0x0028CF2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer
				select c;
			foreach (Creature creature in enumerable)
			{
				await CardPileCmd.DrawWithoutBlockingOnOtherPlayers(choiceContext, base.DynamicVars.Cards.BaseValue, creature.Player, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x0028ED77 File Offset: 0x0028CF77
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
