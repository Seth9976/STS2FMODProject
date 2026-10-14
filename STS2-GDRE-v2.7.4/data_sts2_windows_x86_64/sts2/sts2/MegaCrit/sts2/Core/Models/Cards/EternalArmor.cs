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
	// Token: 0x020009B0 RID: 2480
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EternalArmor : CardModel
	{
		// Token: 0x06007148 RID: 29000 RVA: 0x0028AE73 File Offset: 0x00289073
		public EternalArmor()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DD3 RID: 7635
		// (get) Token: 0x06007149 RID: 29001 RVA: 0x0028AE80 File Offset: 0x00289080
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<PlatingPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x17001DD4 RID: 7636
		// (get) Token: 0x0600714A RID: 29002 RVA: 0x0028AEB7 File Offset: 0x002890B7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PlatingPower>(9m));
			}
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x0028AECC File Offset: 0x002890CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PlatingPower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x0028AF17 File Offset: 0x00289117
		protected override void OnUpgrade()
		{
			base.DynamicVars["PlatingPower"].UpgradeValueBy(3m);
		}
	}
}
