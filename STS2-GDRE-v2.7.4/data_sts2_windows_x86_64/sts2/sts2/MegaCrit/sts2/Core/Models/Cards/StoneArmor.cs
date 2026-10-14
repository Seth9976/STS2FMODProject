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
	// Token: 0x02000AEC RID: 2796
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StoneArmor : CardModel
	{
		// Token: 0x0600780B RID: 30731 RVA: 0x00298F8B File Offset: 0x0029718B
		public StoneArmor()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020A4 RID: 8356
		// (get) Token: 0x0600780C RID: 30732 RVA: 0x00298F98 File Offset: 0x00297198
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PlatingPower>(4m));
			}
		}

		// Token: 0x170020A5 RID: 8357
		// (get) Token: 0x0600780D RID: 30733 RVA: 0x00298FAC File Offset: 0x002971AC
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

		// Token: 0x0600780E RID: 30734 RVA: 0x00298FE4 File Offset: 0x002971E4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x0029902F File Offset: 0x0029722F
		protected override void OnUpgrade()
		{
			base.DynamicVars["PlatingPower"].UpgradeValueBy(2m);
		}
	}
}
