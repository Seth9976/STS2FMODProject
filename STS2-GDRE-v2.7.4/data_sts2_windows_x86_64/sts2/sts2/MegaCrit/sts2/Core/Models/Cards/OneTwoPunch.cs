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
	// Token: 0x02000A5F RID: 2655
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OneTwoPunch : CardModel
	{
		// Token: 0x0600750C RID: 29964 RVA: 0x00292CBB File Offset: 0x00290EBB
		public OneTwoPunch()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F6A RID: 8042
		// (get) Token: 0x0600750D RID: 29965 RVA: 0x00292CC8 File Offset: 0x00290EC8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Attacks", 1m));
			}
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x00292CE0 File Offset: 0x00290EE0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<OneTwoPunchPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Attacks"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x00292D2B File Offset: 0x00290F2B
		protected override void OnUpgrade()
		{
			base.DynamicVars["Attacks"].UpgradeValueBy(1m);
		}

		// Token: 0x040028AB RID: 10411
		private const string _attacksKey = "Attacks";
	}
}
