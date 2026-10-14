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
	// Token: 0x02000B01 RID: 2817
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwordSage : CardModel
	{
		// Token: 0x0600786F RID: 30831 RVA: 0x00299D36 File Offset: 0x00297F36
		public SwordSage()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020C9 RID: 8393
		// (get) Token: 0x06007870 RID: 30832 RVA: 0x00299D43 File Offset: 0x00297F43
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>(false));
				list.Add(HoverTipFactory.Static(StaticHoverTip.ReplayStatic, Array.Empty<DynamicVar>()));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x170020CA RID: 8394
		// (get) Token: 0x06007871 RID: 30833 RVA: 0x00299D6D File Offset: 0x00297F6D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SwordSagePower>(1m));
			}
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x00299D80 File Offset: 0x00297F80
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SwordSagePower>(choiceContext, base.Owner.Creature, base.DynamicVars["SwordSagePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x00299DCB File Offset: 0x00297FCB
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
