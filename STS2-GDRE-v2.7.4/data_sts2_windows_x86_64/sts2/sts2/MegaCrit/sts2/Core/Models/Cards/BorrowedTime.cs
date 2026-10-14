using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200092C RID: 2348
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BorrowedTime : CardModel
	{
		// Token: 0x06006E9A RID: 28314 RVA: 0x00285B07 File Offset: 0x00283D07
		public BorrowedTime()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x06006E9B RID: 28315 RVA: 0x00285B14 File Offset: 0x00283D14
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(4),
					new EnergyVar("ExtraCost", 1)
				});
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x06006E9C RID: 28316 RVA: 0x00285B38 File Offset: 0x00283D38
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x00285B48 File Offset: 0x00283D48
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
			await PowerCmd.Apply<BorrowedTimePower>(choiceContext, base.Owner.Creature, base.DynamicVars["ExtraCost"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E9E RID: 28318 RVA: 0x00285B93 File Offset: 0x00283D93
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(2m);
		}

		// Token: 0x0400284C RID: 10316
		private const string _extraCostKey = "ExtraCost";
	}
}
