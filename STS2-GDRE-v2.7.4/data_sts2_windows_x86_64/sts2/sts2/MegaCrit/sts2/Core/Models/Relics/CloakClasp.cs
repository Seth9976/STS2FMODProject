using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000523 RID: 1315
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloakClasp : RelicModel
	{
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06004FFF RID: 20479 RVA: 0x0023E66C File Offset: 0x0023C86C
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06005000 RID: 20480 RVA: 0x0023E66F File Offset: 0x0023C86F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(1m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005001 RID: 20481 RVA: 0x0023E681 File Offset: 0x0023C881
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x0023E694 File Offset: 0x0023C894
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards;
				if (cards.Count != 0)
				{
					int num = (int)(cards.Count * base.DynamicVars.Block.BaseValue);
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, num, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
