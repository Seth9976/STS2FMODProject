using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000916 RID: 2326
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Beckon : CardModel
	{
		// Token: 0x06006E22 RID: 28194 RVA: 0x00284C7B File Offset: 0x00282E7B
		public Beckon()
			: base(1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001C72 RID: 7282
		// (get) Token: 0x06006E23 RID: 28195 RVA: 0x00284C88 File Offset: 0x00282E88
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001C73 RID: 7283
		// (get) Token: 0x06006E24 RID: 28196 RVA: 0x00284C8B File Offset: 0x00282E8B
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x06006E25 RID: 28197 RVA: 0x00284C8E File Offset: 0x00282E8E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HpLossVar(6m));
			}
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x00284CA0 File Offset: 0x00282EA0
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, null);
		}
	}
}
