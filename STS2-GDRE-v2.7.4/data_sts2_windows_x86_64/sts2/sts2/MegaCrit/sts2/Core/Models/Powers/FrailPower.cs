using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200067E RID: 1662
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FrailPower : PowerModel
	{
		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x0024E91B File Offset: 0x0024CB1B
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x0600586E RID: 22638 RVA: 0x0024E91E File Offset: 0x0024CB1E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x0600586F RID: 22639 RVA: 0x0024E921 File Offset: 0x0024CB21
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x0024E933 File Offset: 0x0024CB33
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != target)
			{
				return 1m;
			}
			if (!props.IsPoweredCardOrMonsterMoveBlock())
			{
				return 1m;
			}
			return 0.75m;
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x0024E960 File Offset: 0x0024CB60
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.TickDownDuration(this);
			}
		}
	}
}
