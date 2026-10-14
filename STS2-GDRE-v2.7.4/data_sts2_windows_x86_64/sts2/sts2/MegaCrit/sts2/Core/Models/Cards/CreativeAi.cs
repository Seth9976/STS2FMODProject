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
	// Token: 0x0200096A RID: 2410
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CreativeAi : CardModel
	{
		// Token: 0x06006FD1 RID: 28625 RVA: 0x0028828F File Offset: 0x0028648F
		public CreativeAi()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D27 RID: 7463
		// (get) Token: 0x06006FD2 RID: 28626 RVA: 0x0028829C File Offset: 0x0028649C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CreativeAi", 1m));
			}
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x002882B4 File Offset: 0x002864B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CreativeAiPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CreativeAi"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x002882FF File Offset: 0x002864FF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x04002859 RID: 10329
		private const string _creativeAiKey = "CreativeAi";
	}
}
