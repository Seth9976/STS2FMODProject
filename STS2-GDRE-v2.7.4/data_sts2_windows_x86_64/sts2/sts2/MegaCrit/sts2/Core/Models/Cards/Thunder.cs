using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B16 RID: 2838
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Thunder : CardModel
	{
		// Token: 0x060078E9 RID: 30953 RVA: 0x0029ACCB File Offset: 0x00298ECB
		public Thunder()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020F8 RID: 8440
		// (get) Token: 0x060078EA RID: 30954 RVA: 0x0029ACD8 File Offset: 0x00298ED8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x170020F9 RID: 8441
		// (get) Token: 0x060078EB RID: 30955 RVA: 0x0029ACFB File Offset: 0x00298EFB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ThunderPower>(6m));
			}
		}

		// Token: 0x060078EC RID: 30956 RVA: 0x0029AD10 File Offset: 0x00298F10
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ThunderPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ThunderPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060078ED RID: 30957 RVA: 0x0029AD5B File Offset: 0x00298F5B
		protected override void OnUpgrade()
		{
			base.DynamicVars["ThunderPower"].UpgradeValueBy(2m);
		}
	}
}
