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
	// Token: 0x020006B7 RID: 1719
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoBlockPower : PowerModel
	{
		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x060059B4 RID: 22964 RVA: 0x00250E86 File Offset: 0x0024F086
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x060059B5 RID: 22965 RVA: 0x00250E89 File Offset: 0x0024F089
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x060059B6 RID: 22966 RVA: 0x00250E8C File Offset: 0x0024F08C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x00250EA0 File Offset: 0x0024F0A0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Decrement(this);
			}
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x00250EEB File Offset: 0x0024F0EB
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (props.HasFlag(ValueProp.Unpowered))
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			return 0m;
		}
	}
}
