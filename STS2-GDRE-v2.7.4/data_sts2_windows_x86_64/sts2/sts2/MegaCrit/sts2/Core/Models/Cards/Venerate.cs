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
	// Token: 0x02000B31 RID: 2865
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Venerate : CardModel
	{
		// Token: 0x06007974 RID: 31092 RVA: 0x0029BD5B File Offset: 0x00299F5B
		public Venerate()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17002132 RID: 8498
		// (get) Token: 0x06007975 RID: 31093 RVA: 0x0029BD68 File Offset: 0x00299F68
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(2));
			}
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x0029BD78 File Offset: 0x00299F78
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x0029BDBB File Offset: 0x00299FBB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Stars.UpgradeValueBy(1m);
		}
	}
}
