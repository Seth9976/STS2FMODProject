using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DD RID: 1501
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SelfFormingClay : RelicModel
	{
		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060054B7 RID: 21687 RVA: 0x00247397 File Offset: 0x00245597
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060054B8 RID: 21688 RVA: 0x0024739A File Offset: 0x0024559A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BlockNextTurn", 3m));
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060054B9 RID: 21689 RVA: 0x002473B1 File Offset: 0x002455B1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x002473C4 File Offset: 0x002455C4
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				if (target == base.Owner.Creature)
				{
					if (result.UnblockedDamage > 0)
					{
						await PowerCmd.Apply<SelfFormingClayPower>(choiceContext, base.Owner.Creature, base.DynamicVars["BlockNextTurn"].BaseValue, base.Owner.Creature, null, false);
					}
				}
			}
		}

		// Token: 0x040024B8 RID: 9400
		private const string _blockNextTurnKey = "BlockNextTurn";
	}
}
