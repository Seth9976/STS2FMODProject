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
	// Token: 0x02000B1B RID: 2843
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Toxic : CardModel
	{
		// Token: 0x06007902 RID: 30978 RVA: 0x0029B053 File Offset: 0x00299253
		public Toxic()
			: base(1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17002102 RID: 8450
		// (get) Token: 0x06007903 RID: 30979 RVA: 0x0029B060 File Offset: 0x00299260
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002103 RID: 8451
		// (get) Token: 0x06007904 RID: 30980 RVA: 0x0029B063 File Offset: 0x00299263
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002104 RID: 8452
		// (get) Token: 0x06007905 RID: 30981 RVA: 0x0029B06B File Offset: 0x0029926B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(5m, ValueProp.Unpowered | ValueProp.Move));
			}
		}

		// Token: 0x17002105 RID: 8453
		// (get) Token: 0x06007906 RID: 30982 RVA: 0x0029B07F File Offset: 0x0029927F
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x0029B084 File Offset: 0x00299284
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, this, null);
		}
	}
}
