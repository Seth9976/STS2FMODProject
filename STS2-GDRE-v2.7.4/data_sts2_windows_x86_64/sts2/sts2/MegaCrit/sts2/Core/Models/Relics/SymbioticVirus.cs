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
	// Token: 0x020005F4 RID: 1524
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SymbioticVirus : RelicModel
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06005550 RID: 21840 RVA: 0x0024857B File Offset: 0x0024677B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06005551 RID: 21841 RVA: 0x0024857E File Offset: 0x0024677E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Dark", 1m));
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06005552 RID: 21842 RVA: 0x00248594 File Offset: 0x00246794
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<DarkOrb>()
				});
			}
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x002485B8 File Offset: 0x002467B8
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					int i = 0;
					while (i < base.DynamicVars["Dark"].BaseValue)
					{
						await OrbCmd.Channel<DarkOrb>(new BlockingPlayerChoiceContext(), base.Owner);
						i++;
					}
				}
			}
		}

		// Token: 0x040024C7 RID: 9415
		private const string _darknessKey = "Dark";
	}
}
