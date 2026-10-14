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
	// Token: 0x02000990 RID: 2448
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DevourLife : CardModel
	{
		// Token: 0x0600709A RID: 28826 RVA: 0x00289AF0 File Offset: 0x00287CF0
		public DevourLife()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D83 RID: 7555
		// (get) Token: 0x0600709B RID: 28827 RVA: 0x00289AFD File Offset: 0x00287CFD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<Soul>(false),
					HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x17001D84 RID: 7556
		// (get) Token: 0x0600709C RID: 28828 RVA: 0x00289B22 File Offset: 0x00287D22
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DevourLifePower>(1m));
			}
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x00289B34 File Offset: 0x00287D34
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DevourLifePower>(choiceContext, base.Owner.Creature, base.DynamicVars["DevourLifePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x00289B7F File Offset: 0x00287D7F
		protected override void OnUpgrade()
		{
			base.DynamicVars["DevourLifePower"].UpgradeValueBy(1m);
		}
	}
}
