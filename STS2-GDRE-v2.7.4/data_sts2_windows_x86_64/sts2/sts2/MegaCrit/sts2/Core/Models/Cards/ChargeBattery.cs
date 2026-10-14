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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200094C RID: 2380
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChargeBattery : CardModel
	{
		// Token: 0x06006F2E RID: 28462 RVA: 0x00286E4F File Offset: 0x0028504F
		public ChargeBattery()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001CDF RID: 7391
		// (get) Token: 0x06006F2F RID: 28463 RVA: 0x00286E5C File Offset: 0x0028505C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CE0 RID: 7392
		// (get) Token: 0x06006F30 RID: 28464 RVA: 0x00286E5F File Offset: 0x0028505F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(7m, ValueProp.Move),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001CE1 RID: 7393
		// (get) Token: 0x06006F31 RID: 28465 RVA: 0x00286E84 File Offset: 0x00285084
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006F32 RID: 28466 RVA: 0x00286E94 File Offset: 0x00285094
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F33 RID: 28467 RVA: 0x00286EE7 File Offset: 0x002850E7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
