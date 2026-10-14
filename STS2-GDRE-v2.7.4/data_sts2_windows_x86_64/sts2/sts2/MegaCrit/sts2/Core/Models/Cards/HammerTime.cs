using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F0 RID: 2544
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HammerTime : CardModel
	{
		// Token: 0x060072AF RID: 29359 RVA: 0x0028DD14 File Offset: 0x0028BF14
		public HammerTime()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001E6F RID: 7791
		// (get) Token: 0x060072B0 RID: 29360 RVA: 0x0028DD21 File Offset: 0x0028BF21
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001E70 RID: 7792
		// (get) Token: 0x060072B1 RID: 29361 RVA: 0x0028DD24 File Offset: 0x0028BF24
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060072B2 RID: 29362 RVA: 0x0028DD2C File Offset: 0x0028BF2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<HammerTimePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x0028DD77 File Offset: 0x0028BF77
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
