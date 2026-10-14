using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000510 RID: 1296
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bread : RelicModel
	{
		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x0023D846 File Offset: 0x0023BA46
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06004F88 RID: 20360 RVA: 0x0023D849 File Offset: 0x0023BA49
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar("GainEnergy", 1),
					new EnergyVar("LoseEnergy", 2)
				});
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x0023D872 File Offset: 0x0023BA72
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x0023D880 File Offset: 0x0023BA80
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			PlayerCombatState playerCombatState = base.Owner.PlayerCombatState;
			if (playerCombatState != null && playerCombatState.TurnNumber == 1)
			{
				return amount;
			}
			return amount + base.DynamicVars["GainEnergy"].BaseValue;
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x0023D8D4 File Offset: 0x0023BAD4
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					await PlayerCmd.LoseEnergy(base.DynamicVars["LoseEnergy"].BaseValue, base.Owner);
				}
			}
		}

		// Token: 0x0400242C RID: 9260
		private const string _gainEnergyKey = "GainEnergy";

		// Token: 0x0400242D RID: 9261
		private const string _loseEnergyKey = "LoseEnergy";
	}
}
