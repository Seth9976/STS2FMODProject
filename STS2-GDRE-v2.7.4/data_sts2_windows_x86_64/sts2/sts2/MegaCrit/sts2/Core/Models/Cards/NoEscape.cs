using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A55 RID: 2645
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoEscape : CardModel
	{
		// Token: 0x060074D7 RID: 29911 RVA: 0x00292581 File Offset: 0x00290781
		public NoEscape()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F53 RID: 8019
		// (get) Token: 0x060074D8 RID: 29912 RVA: 0x00292590 File Offset: 0x00290790
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DynamicVar("DoomThreshold", 10m);
				array[1] = new CalculationBaseVar(10m);
				array[2] = new CalculationExtraVar(5m);
				array[3] = new CalculatedVar("CalculatedDoom").WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature target)
				{
					int num = ((target != null) ? target.GetPowerAmount<DoomPower>() : 0);
					decimal baseValue = card.DynamicVars["DoomThreshold"].BaseValue;
					return Math.Floor(num / baseValue);
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001F54 RID: 8020
		// (get) Token: 0x060074D9 RID: 29913 RVA: 0x0029260C File Offset: 0x0029080C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060074DA RID: 29914 RVA: 0x0029262C File Offset: 0x0029082C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, ((CalculatedVar)base.DynamicVars["CalculatedDoom"]).Calculate(cardPlay.Target), base.Owner.Creature, this, false);
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x0029267F File Offset: 0x0029087F
		protected override void OnUpgrade()
		{
			base.DynamicVars.CalculationBase.UpgradeValueBy(5m);
		}

		// Token: 0x040028A6 RID: 10406
		private const string _calculatedDoomKey = "CalculatedDoom";

		// Token: 0x040028A7 RID: 10407
		private const string _doomThresholdKey = "DoomThreshold";
	}
}
