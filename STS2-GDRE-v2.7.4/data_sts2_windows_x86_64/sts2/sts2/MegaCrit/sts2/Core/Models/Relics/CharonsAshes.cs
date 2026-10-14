using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200051D RID: 1309
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CharonsAshes : RelicModel
	{
		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06004FE2 RID: 20450 RVA: 0x0023E33F File Offset: 0x0023C53F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06004FE3 RID: 20451 RVA: 0x0023E342 File Offset: 0x0023C542
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x0023E358 File Offset: 0x0023C558
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
		{
			if (card.Owner == base.Owner)
			{
				base.Flash();
				DamageVar damage = base.DynamicVars.Damage;
				await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, damage.BaseValue, damage.Props, base.Owner.Creature);
			}
		}
	}
}
