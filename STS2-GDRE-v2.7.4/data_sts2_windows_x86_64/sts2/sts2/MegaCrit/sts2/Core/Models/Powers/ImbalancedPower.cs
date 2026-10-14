using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000699 RID: 1689
	public sealed class ImbalancedPower : PowerModel
	{
		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x0600590D RID: 22797 RVA: 0x0024FBF3 File Offset: 0x0024DDF3
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x0600590E RID: 22798 RVA: 0x0024FBF6 File Offset: 0x0024DDF6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x0600590F RID: 22799 RVA: 0x0024FBFC File Offset: 0x0024DDFC
		[NullableContext(1)]
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer == base.Owner)
			{
				if (result.WasFullyBlocked)
				{
					base.Flash();
					BowlbugRock bowlbugRock = base.Owner.Monster as BowlbugRock;
					if (bowlbugRock != null)
					{
						bowlbugRock.IsOffBalance = true;
					}
					else
					{
						await CreatureCmd.Stun(base.Owner, null);
					}
				}
			}
		}
	}
}
