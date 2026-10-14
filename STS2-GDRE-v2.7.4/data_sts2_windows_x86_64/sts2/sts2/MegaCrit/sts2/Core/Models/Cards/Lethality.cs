using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A29 RID: 2601
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Lethality : CardModel
	{
		// Token: 0x060073D6 RID: 29654 RVA: 0x00290314 File Offset: 0x0028E514
		public Lethality()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EE7 RID: 7911
		// (get) Token: 0x060073D7 RID: 29655 RVA: 0x00290321 File Offset: 0x0028E521
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17001EE8 RID: 7912
		// (get) Token: 0x060073D8 RID: 29656 RVA: 0x00290329 File Offset: 0x0028E529
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<LethalityPower>(50m));
			}
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x0029033C File Offset: 0x0028E53C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<LethalityPower>(choiceContext, base.Owner.Creature, base.DynamicVars["LethalityPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073DA RID: 29658 RVA: 0x00290387 File Offset: 0x0028E587
		protected override void OnUpgrade()
		{
			base.DynamicVars["LethalityPower"].UpgradeValueBy(25m);
		}
	}
}
