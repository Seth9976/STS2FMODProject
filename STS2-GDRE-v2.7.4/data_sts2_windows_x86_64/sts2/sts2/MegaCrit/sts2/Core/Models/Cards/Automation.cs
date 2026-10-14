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
	// Token: 0x02000908 RID: 2312
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Automation : CardModel
	{
		// Token: 0x06006DDA RID: 28122 RVA: 0x00284217 File Offset: 0x00282417
		public Automation()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x06006DDB RID: 28123 RVA: 0x00284224 File Offset: 0x00282424
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x06006DDC RID: 28124 RVA: 0x00284231 File Offset: 0x00282431
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x00284240 File Offset: 0x00282440
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<AutomationPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x0028428B File Offset: 0x0028248B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
