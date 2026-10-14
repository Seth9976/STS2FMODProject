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
	// Token: 0x02000960 RID: 2400
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConsumingShadow : CardModel
	{
		// Token: 0x06006F9E RID: 28574 RVA: 0x00287C2F File Offset: 0x00285E2F
		public ConsumingShadow()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D12 RID: 7442
		// (get) Token: 0x06006F9F RID: 28575 RVA: 0x00287C3C File Offset: 0x00285E3C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<DarkOrb>(),
					HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x17001D13 RID: 7443
		// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x00287C6D File Offset: 0x00285E6D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new RepeatVar(2),
					new PowerVar<ConsumingShadowPower>(1m)
				});
			}
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x00287C90 File Offset: 0x00285E90
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
			{
				await OrbCmd.Channel<DarkOrb>(choiceContext, base.Owner);
			}
			await PowerCmd.Apply<ConsumingShadowPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ConsumingShadowPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x00287CDB File Offset: 0x00285EDB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
