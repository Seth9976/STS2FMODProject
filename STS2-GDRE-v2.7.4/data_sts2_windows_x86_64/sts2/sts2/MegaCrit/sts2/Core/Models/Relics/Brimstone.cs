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
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000512 RID: 1298
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Brimstone : RelicModel
	{
		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x0023DB38 File Offset: 0x0023BD38
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x0023DB3B File Offset: 0x0023BD3B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>("SelfStrength", 2m),
					new PowerVar<StrengthPower>("EnemyStrength", 1m)
				});
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x0023DB70 File Offset: 0x0023BD70
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x0023DB90 File Offset: 0x0023BD90
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["SelfStrength"].BaseValue, base.Owner.Creature, null, false);
				IEnumerable<Creature> enumerable = from c in combatState.GetOpponentsOf(base.Owner.Creature)
					where c.IsAlive
					select c;
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), enumerable, base.DynamicVars["EnemyStrength"].BaseValue, null, null, false);
			}
		}

		// Token: 0x0400242F RID: 9263
		private const string _selfStrengthKey = "SelfStrength";

		// Token: 0x04002430 RID: 9264
		private const string _enemyStrengthKey = "EnemyStrength";
	}
}
