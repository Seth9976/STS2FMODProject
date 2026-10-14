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
	// Token: 0x02000AED RID: 2797
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Storm : CardModel
	{
		// Token: 0x06007810 RID: 30736 RVA: 0x0029904C File Offset: 0x0029724C
		public Storm()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020A6 RID: 8358
		// (get) Token: 0x06007811 RID: 30737 RVA: 0x00299059 File Offset: 0x00297259
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StormPower>(1m));
			}
		}

		// Token: 0x170020A7 RID: 8359
		// (get) Token: 0x06007812 RID: 30738 RVA: 0x0029906A File Offset: 0x0029726A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x06007813 RID: 30739 RVA: 0x00299090 File Offset: 0x00297290
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<StormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StormPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x002990DB File Offset: 0x002972DB
		protected override void OnUpgrade()
		{
			base.DynamicVars["StormPower"].UpgradeValueBy(1m);
		}
	}
}
