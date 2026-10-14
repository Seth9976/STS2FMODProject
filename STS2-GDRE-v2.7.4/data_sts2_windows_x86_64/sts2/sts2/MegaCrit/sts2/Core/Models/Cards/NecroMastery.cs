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
	// Token: 0x02000A4E RID: 2638
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NecroMastery : CardModel
	{
		// Token: 0x060074B0 RID: 29872 RVA: 0x0029206D File Offset: 0x0029026D
		public NecroMastery()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F41 RID: 8001
		// (get) Token: 0x060074B1 RID: 29873 RVA: 0x0029207A File Offset: 0x0029027A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(5m));
			}
		}

		// Token: 0x17001F42 RID: 8002
		// (get) Token: 0x060074B2 RID: 29874 RVA: 0x0029208C File Offset: 0x0029028C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x002920B0 File Offset: 0x002902B0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			await PowerCmd.Apply<NecroMasteryPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x002920FB File Offset: 0x002902FB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(3m);
		}
	}
}
