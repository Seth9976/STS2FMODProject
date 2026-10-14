using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006CA RID: 1738
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PillarOfCreationPower : PowerModel
	{
		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x06005A26 RID: 23078 RVA: 0x00251B4B File Offset: 0x0024FD4B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06005A27 RID: 23079 RVA: 0x00251B4E File Offset: 0x0024FD4E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06005A28 RID: 23080 RVA: 0x00251B51 File Offset: 0x0024FD51
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x00251B64 File Offset: 0x0024FD64
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (creator != null && creator.Creature == base.Owner)
			{
				CardModel cardModel = (from entry in CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Where<CardGeneratedEntry>(delegate(CardGeneratedEntry entry)
					{
						Player creator2 = entry.Creator;
						return ((creator2 != null) ? creator2.Creature : null) == base.Owner && entry.HappenedThisTurn(base.CombatState);
					})
					select entry.Card).FirstOrDefault<CardModel>();
				if (cardModel == card)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
