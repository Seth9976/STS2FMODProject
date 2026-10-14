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
	// Token: 0x020006B0 RID: 1712
	public sealed class MonarchsGazePower : PowerModel
	{
		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x0600598D RID: 22925 RVA: 0x00250A65 File Offset: 0x0024EC65
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x0600598E RID: 22926 RVA: 0x00250A68 File Offset: 0x0024EC68
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x00250A6C File Offset: 0x0024EC6C
		[NullableContext(1)]
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult _, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer == base.Owner)
			{
				if (props.IsPoweredAttack())
				{
					await PowerCmd.Apply<MonarchsGazeStrengthDownPower>(choiceContext, target, base.Amount, base.Owner, null, false);
				}
			}
		}
	}
}
