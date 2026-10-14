using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000736 RID: 1846
	public sealed class MockGainBlockOnAttackPower : PowerModel
	{
		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x0025715E File Offset: 0x0025535E
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x06005CD4 RID: 23764 RVA: 0x00257161 File Offset: 0x00255361
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x00257164 File Offset: 0x00255364
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		/// <summary>
		/// This gives the attacker 1 block after every attack.
		/// A multi-hit attack should still only gain 1 block.
		/// </summary>
		// Token: 0x06005CD6 RID: 23766 RVA: 0x00257168 File Offset: 0x00255368
		[NullableContext(1)]
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			if (command.Attacker == base.Owner)
			{
				if (command.DamageProps.HasFlag(ValueProp.Move))
				{
					await CreatureCmd.GainBlock(base.Owner, 1m, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
