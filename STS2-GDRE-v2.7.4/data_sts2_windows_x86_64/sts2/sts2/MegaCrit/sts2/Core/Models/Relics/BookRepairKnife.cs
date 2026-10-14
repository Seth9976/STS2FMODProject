using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x0200050C RID: 1292
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BookRepairKnife : RelicModel
	{
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06004F6C RID: 20332 RVA: 0x0023D4FB File Offset: 0x0023B6FB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06004F6D RID: 20333 RVA: 0x0023D500 File Offset: 0x0023B700
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06004F6E RID: 20334 RVA: 0x0023D520 File Offset: 0x0023B720
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(3m));
			}
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x0023D534 File Offset: 0x0023B734
		public override Task AfterDiedToDoom(PlayerChoiceContext choiceContext, IReadOnlyList<Creature> creatures)
		{
			int num = creatures.Count<Creature>(delegate(Creature c)
			{
				if (c != base.Owner.Creature)
				{
					return c.Powers.All<PowerModel>((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
				}
				return false;
			});
			if (num == 0)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue * num, true);
		}
	}
}
