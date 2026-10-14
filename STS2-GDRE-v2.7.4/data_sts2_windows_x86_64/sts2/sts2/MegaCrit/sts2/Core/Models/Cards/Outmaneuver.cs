using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A62 RID: 2658
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Outmaneuver : CardModel
	{
		// Token: 0x0600751A RID: 29978 RVA: 0x00292E73 File Offset: 0x00291073
		public Outmaneuver()
			: base(1, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001F6F RID: 8047
		// (get) Token: 0x0600751B RID: 29979 RVA: 0x00292E80 File Offset: 0x00291080
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001F70 RID: 8048
		// (get) Token: 0x0600751C RID: 29980 RVA: 0x00292E8D File Offset: 0x0029108D
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<SilentCardPool>();
			}
		}

		// Token: 0x17001F71 RID: 8049
		// (get) Token: 0x0600751D RID: 29981 RVA: 0x00292E94 File Offset: 0x00291094
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x0600751E RID: 29982 RVA: 0x00292EA4 File Offset: 0x002910A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x00292EEF File Offset: 0x002910EF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
