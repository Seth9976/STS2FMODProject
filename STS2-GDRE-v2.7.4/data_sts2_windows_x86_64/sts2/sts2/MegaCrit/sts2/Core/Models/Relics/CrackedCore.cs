using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000524 RID: 1316
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrackedCore : RelicModel
	{
		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005004 RID: 20484 RVA: 0x0023E6E7 File Offset: 0x0023C8E7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x0023E6EA File Offset: 0x0023C8EA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Lightning", 1m));
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x0023E700 File Offset: 0x0023C900
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x0023E724 File Offset: 0x0023C924
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					int i = 0;
					while (i < base.DynamicVars["Lightning"].BaseValue)
					{
						await OrbCmd.Channel<LightningOrb>(new BlockingPlayerChoiceContext(), base.Owner);
						i++;
					}
				}
			}
		}

		// Token: 0x0400243A RID: 9274
		private const string _lightningKey = "Lightning";
	}
}
