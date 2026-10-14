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
	// Token: 0x02000B1E RID: 2846
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TrashToTreasure : CardModel
	{
		// Token: 0x06007913 RID: 30995 RVA: 0x0029B1F4 File Offset: 0x002993F4
		public TrashToTreasure()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x1700210B RID: 8459
		// (get) Token: 0x06007914 RID: 30996 RVA: 0x0029B201 File Offset: 0x00299401
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x0029B214 File Offset: 0x00299414
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<TrashToTreasurePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007916 RID: 30998 RVA: 0x0029B25F File Offset: 0x0029945F
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
