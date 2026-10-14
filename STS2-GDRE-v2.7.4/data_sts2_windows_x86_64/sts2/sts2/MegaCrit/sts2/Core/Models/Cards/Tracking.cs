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
	// Token: 0x02000B1C RID: 2844
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tracking : CardModel
	{
		// Token: 0x06007908 RID: 30984 RVA: 0x0029B0CF File Offset: 0x002992CF
		public Tracking()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002106 RID: 8454
		// (get) Token: 0x06007909 RID: 30985 RVA: 0x0029B0DC File Offset: 0x002992DC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x0029B0FC File Offset: 0x002992FC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<TrackingPower>(choiceContext, base.Owner.Creature, 50m, base.Owner.Creature, this, false);
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x0029B147 File Offset: 0x00299347
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
