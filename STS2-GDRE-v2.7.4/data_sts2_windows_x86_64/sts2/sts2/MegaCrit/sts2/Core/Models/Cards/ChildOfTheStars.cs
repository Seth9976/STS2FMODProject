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
	// Token: 0x0200094D RID: 2381
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChildOfTheStars : CardModel
	{
		// Token: 0x06006F34 RID: 28468 RVA: 0x00286EFF File Offset: 0x002850FF
		public ChildOfTheStars()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x00286F0C File Offset: 0x0028510C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BlockForStars", 2m));
			}
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x00286F23 File Offset: 0x00285123
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06006F37 RID: 28471 RVA: 0x00286F38 File Offset: 0x00285138
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ChildOfTheStarsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["BlockForStars"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F38 RID: 28472 RVA: 0x00286F83 File Offset: 0x00285183
		protected override void OnUpgrade()
		{
			base.DynamicVars["BlockForStars"].UpgradeValueBy(1m);
		}

		// Token: 0x04002852 RID: 10322
		private const string _blockForStarsKey = "BlockForStars";
	}
}
