using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006EB RID: 1771
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShriekPower : PowerModel
	{
		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06005AF4 RID: 23284 RVA: 0x00253597 File Offset: 0x00251797
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x0025359A File Offset: 0x0025179A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06005AF6 RID: 23286 RVA: 0x0025359D File Offset: 0x0025179D
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06005AF7 RID: 23287 RVA: 0x002535A0 File Offset: 0x002517A0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Stun, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06005AF8 RID: 23288 RVA: 0x002535B2 File Offset: 0x002517B2
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x002535B8 File Offset: 0x002517B8
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage > 0)
				{
					if (target.CurrentHp <= base.Amount)
					{
						base.Flash();
						await CreatureCmd.Stun(base.Owner, ((TerrorEel)base.Owner.Monster).TerrorState.StateId);
						await PowerCmd.Remove(this);
					}
				}
			}
		}
	}
}
