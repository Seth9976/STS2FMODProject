using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000729 RID: 1833
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VigorPower : PowerModel
	{
		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x06005C7E RID: 23678 RVA: 0x00256680 File Offset: 0x00254880
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x06005C7F RID: 23679 RVA: 0x00256683 File Offset: 0x00254883
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x00256686 File Offset: 0x00254886
		protected override object InitInternalData()
		{
			return new VigorPower.Data();
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x00256690 File Offset: 0x00254890
		public override Task BeforeAttack(AttackCommand command)
		{
			if (command.Attacker != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!command.DamageProps.IsPoweredAttack())
			{
				return Task.CompletedTask;
			}
			VigorPower.Data internalData = base.GetInternalData<VigorPower.Data>();
			if (internalData.commandToModify != null)
			{
				return Task.CompletedTask;
			}
			if (command.ModelSource != null && !(command.ModelSource is CardModel))
			{
				return Task.CompletedTask;
			}
			if (!command.DamageProps.IsPoweredAttack())
			{
				return Task.CompletedTask;
			}
			internalData.commandToModify = command;
			internalData.amountWhenAttackStarted = base.Amount;
			return Task.CompletedTask;
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x00256720 File Offset: 0x00254920
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != dealer)
			{
				return 0m;
			}
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			VigorPower.Data internalData = base.GetInternalData<VigorPower.Data>();
			if (internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource)
			{
				return 0m;
			}
			if (internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer)
			{
				return 0m;
			}
			return base.Amount;
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x0025679C File Offset: 0x0025499C
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			VigorPower.Data internalData = base.GetInternalData<VigorPower.Data>();
			if (command == internalData.commandToModify)
			{
				await PowerCmd.ModifyAmount(choiceContext, this, -internalData.amountWhenAttackStarted, null, null, false);
			}
		}

		// Token: 0x02001CB2 RID: 7346
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04007391 RID: 29585
			[Nullable(2)]
			public AttackCommand commandToModify;

			// Token: 0x04007392 RID: 29586
			public int amountWhenAttackStarted;
		}
	}
}
