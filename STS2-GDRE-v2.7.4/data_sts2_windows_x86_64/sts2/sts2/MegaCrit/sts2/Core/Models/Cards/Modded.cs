using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A47 RID: 2631
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Modded : CardModel
	{
		// Token: 0x0600748E RID: 29838 RVA: 0x00291C1A File Offset: 0x0028FE1A
		public Modded()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F33 RID: 7987
		// (get) Token: 0x0600748F RID: 29839 RVA: 0x00291C27 File Offset: 0x0028FE27
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new RepeatVar(1),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x00291C48 File Offset: 0x0028FE48
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await OrbCmd.AddSlots(base.Owner, base.DynamicVars.Repeat.IntValue);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			base.EnergyCost.AddThisCombat(1, false);
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00291C93 File Offset: 0x0028FE93
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
