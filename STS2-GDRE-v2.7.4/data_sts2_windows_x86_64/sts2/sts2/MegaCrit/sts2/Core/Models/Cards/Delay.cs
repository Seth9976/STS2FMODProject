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
	// Token: 0x0200098A RID: 2442
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Delay : CardModel
	{
		// Token: 0x06007079 RID: 28793 RVA: 0x0028970B File Offset: 0x0028790B
		public Delay()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D73 RID: 7539
		// (get) Token: 0x0600707A RID: 28794 RVA: 0x00289718 File Offset: 0x00287918
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D74 RID: 7540
		// (get) Token: 0x0600707B RID: 28795 RVA: 0x0028971B File Offset: 0x0028791B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(11m, ValueProp.Move),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001D75 RID: 7541
		// (get) Token: 0x0600707C RID: 28796 RVA: 0x00289741 File Offset: 0x00287941
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x0600707D RID: 28797 RVA: 0x00289750 File Offset: 0x00287950
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x002897A3 File Offset: 0x002879A3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
