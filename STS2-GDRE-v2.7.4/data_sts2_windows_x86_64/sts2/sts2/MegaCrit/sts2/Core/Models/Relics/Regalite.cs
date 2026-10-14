using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CB RID: 1483
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Regalite : RelicModel
	{
		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06005456 RID: 21590 RVA: 0x00246623 File Offset: 0x00244823
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06005457 RID: 21591 RVA: 0x00246626 File Offset: 0x00244826
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(2m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x0024663C File Offset: 0x0024483C
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (creator != null && creator == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, true);
			}
		}
	}
}
