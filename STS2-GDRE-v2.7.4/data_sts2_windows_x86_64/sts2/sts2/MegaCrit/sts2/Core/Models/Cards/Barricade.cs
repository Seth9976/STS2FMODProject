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
	// Token: 0x0200090F RID: 2319
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Barricade : CardModel
	{
		// Token: 0x06006E02 RID: 28162 RVA: 0x002847AB File Offset: 0x002829AB
		public Barricade()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x06006E03 RID: 28163 RVA: 0x002847B8 File Offset: 0x002829B8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x002847CC File Offset: 0x002829CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<BarricadePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x00284817 File Offset: 0x00282A17
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
