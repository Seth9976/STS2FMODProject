using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E9 RID: 1513
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SparklingRouge : RelicModel
	{
		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06005510 RID: 21776 RVA: 0x00247DED File Offset: 0x00245FED
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06005511 RID: 21777 RVA: 0x00247DF0 File Offset: 0x00245FF0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06005512 RID: 21778 RVA: 0x00247E18 File Offset: 0x00246018
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x00247E54 File Offset: 0x00246054
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner.Creature)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 3)
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
					await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
