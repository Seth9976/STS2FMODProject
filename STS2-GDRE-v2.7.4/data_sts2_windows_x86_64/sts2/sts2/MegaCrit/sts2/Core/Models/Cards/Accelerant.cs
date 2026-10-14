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
	// Token: 0x020008F2 RID: 2290
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Accelerant : CardModel
	{
		// Token: 0x06006D6D RID: 28013 RVA: 0x00283537 File Offset: 0x00281737
		public Accelerant()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x06006D6E RID: 28014 RVA: 0x00283544 File Offset: 0x00281744
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x06006D6F RID: 28015 RVA: 0x00283564 File Offset: 0x00281764
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Accelerant", 1m));
			}
		}

		// Token: 0x06006D70 RID: 28016 RVA: 0x0028357C File Offset: 0x0028177C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<AccelerantPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Accelerant"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x002835C7 File Offset: 0x002817C7
		protected override void OnUpgrade()
		{
			base.DynamicVars["Accelerant"].UpgradeValueBy(1m);
		}

		// Token: 0x04002846 RID: 10310
		private const string _powerVarName = "Accelerant";
	}
}
