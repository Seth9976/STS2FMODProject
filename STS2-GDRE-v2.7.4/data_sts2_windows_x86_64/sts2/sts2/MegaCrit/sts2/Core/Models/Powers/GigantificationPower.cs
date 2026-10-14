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
	// Token: 0x02000686 RID: 1670
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GigantificationPower : PowerModel
	{
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06005898 RID: 22680 RVA: 0x0024EE63 File Offset: 0x0024D063
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06005899 RID: 22681 RVA: 0x0024EE66 File Offset: 0x0024D066
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x0024EE69 File Offset: 0x0024D069
		protected override object InitInternalData()
		{
			return new GigantificationPower.Data();
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x0024EE70 File Offset: 0x0024D070
		public override Task BeforeAttack(AttackCommand command)
		{
			CardModel cardModel = command.ModelSource as CardModel;
			if (cardModel == null)
			{
				return Task.CompletedTask;
			}
			if (cardModel.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (cardModel.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			if (!command.DamageProps.IsPoweredAttack())
			{
				return Task.CompletedTask;
			}
			GigantificationPower.Data internalData = base.GetInternalData<GigantificationPower.Data>();
			if (internalData.commandToModify != null)
			{
				return Task.CompletedTask;
			}
			internalData.commandToModify = command;
			return Task.CompletedTask;
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x0024EEF0 File Offset: 0x0024D0F0
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (cardSource == null)
			{
				return 1m;
			}
			if (cardSource.Owner.Creature != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			GigantificationPower.Data internalData = base.GetInternalData<GigantificationPower.Data>();
			if (internalData.commandToModify != null && cardSource != internalData.commandToModify.ModelSource)
			{
				return 1m;
			}
			return 3m;
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x0024EF5C File Offset: 0x0024D15C
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			GigantificationPower.Data internalData = base.GetInternalData<GigantificationPower.Data>();
			if (command == internalData.commandToModify)
			{
				internalData.commandToModify = null;
				await PowerCmd.Decrement(this);
			}
		}

		// Token: 0x02001BD6 RID: 7126
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006F01 RID: 28417
			[Nullable(2)]
			public AttackCommand commandToModify;
		}
	}
}
