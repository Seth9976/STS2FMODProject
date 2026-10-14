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
	// Token: 0x0200097F RID: 2431
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Decay : CardModel
	{
		// Token: 0x06007039 RID: 28729 RVA: 0x0028906B File Offset: 0x0028726B
		public Decay()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001D53 RID: 7507
		// (get) Token: 0x0600703A RID: 28730 RVA: 0x00289079 File Offset: 0x00287279
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D54 RID: 7508
		// (get) Token: 0x0600703B RID: 28731 RVA: 0x0028907C File Offset: 0x0028727C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(2m, ValueProp.Unpowered | ValueProp.Move));
			}
		}

		// Token: 0x17001D55 RID: 7509
		// (get) Token: 0x0600703C RID: 28732 RVA: 0x00289090 File Offset: 0x00287290
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001D56 RID: 7510
		// (get) Token: 0x0600703D RID: 28733 RVA: 0x00289098 File Offset: 0x00287298
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x0028909C File Offset: 0x0028729C
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, this, null);
		}
	}
}
