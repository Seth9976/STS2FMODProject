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
	// Token: 0x02000A45 RID: 2629
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Mirage : CardModel
	{
		// Token: 0x06007485 RID: 29829 RVA: 0x00291AFB File Offset: 0x0028FCFB
		public Mirage()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F30 RID: 7984
		// (get) Token: 0x06007486 RID: 29830 RVA: 0x00291B08 File Offset: 0x0028FD08
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17001F31 RID: 7985
		// (get) Token: 0x06007487 RID: 29831 RVA: 0x00291B15 File Offset: 0x0028FD15
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x00291B24 File Offset: 0x0028FD24
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x00291B6F File Offset: 0x0028FD6F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
