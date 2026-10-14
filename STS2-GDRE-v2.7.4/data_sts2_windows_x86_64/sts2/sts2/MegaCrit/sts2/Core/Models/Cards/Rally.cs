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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A90 RID: 2704
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rally : CardModel
	{
		// Token: 0x06007601 RID: 30209 RVA: 0x00294D54 File Offset: 0x00292F54
		public Rally()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001FCA RID: 8138
		// (get) Token: 0x06007602 RID: 30210 RVA: 0x00294D61 File Offset: 0x00292F61
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001FCB RID: 8139
		// (get) Token: 0x06007603 RID: 30211 RVA: 0x00294D64 File Offset: 0x00292F64
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001FCC RID: 8140
		// (get) Token: 0x06007604 RID: 30212 RVA: 0x00294D67 File Offset: 0x00292F67
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(12m, ValueProp.Move));
			}
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x00294D7C File Offset: 0x00292F7C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer
				select c;
			foreach (Creature creature in enumerable)
			{
				await CreatureCmd.GainBlock(creature, base.DynamicVars.Block, cardPlay, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x00294DC7 File Offset: 0x00292FC7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(5m);
		}
	}
}
