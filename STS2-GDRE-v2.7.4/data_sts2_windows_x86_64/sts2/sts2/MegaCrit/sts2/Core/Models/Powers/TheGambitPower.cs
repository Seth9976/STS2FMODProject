using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200071A RID: 1818
	public sealed class TheGambitPower : PowerModel
	{
		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x06005C32 RID: 23602 RVA: 0x00255E4F File Offset: 0x0025404F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06005C33 RID: 23603 RVA: 0x00255E52 File Offset: 0x00254052
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x00255E58 File Offset: 0x00254058
		[NullableContext(1)]
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature _, [Nullable(2)] CardModel __)
		{
			if (target == base.Owner)
			{
				if (props.IsPoweredAttack())
				{
					if (result.UnblockedDamage > 0)
					{
						await PowerCmd.Remove(this);
						base.Flash();
						await CreatureCmd.Kill(base.Owner, false);
					}
				}
			}
		}
	}
}
