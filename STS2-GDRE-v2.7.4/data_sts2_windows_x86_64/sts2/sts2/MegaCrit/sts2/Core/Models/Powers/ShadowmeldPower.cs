using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020006E9 RID: 1769
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShadowmeldPower : PowerModel
	{
		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06005AEA RID: 23274 RVA: 0x002534A4 File Offset: 0x002516A4
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x002534A7 File Offset: 0x002516A7
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06005AEC RID: 23276 RVA: 0x002534AA File Offset: 0x002516AA
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002534BC File Offset: 0x002516BC
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != target)
			{
				return 1m;
			}
			return (decimal)Math.Pow(2.0, (double)base.Amount);
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x002534E8 File Offset: 0x002516E8
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
