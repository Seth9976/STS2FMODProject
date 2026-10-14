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
	// Token: 0x02000A6A RID: 2666
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Parry : CardModel
	{
		// Token: 0x06007543 RID: 30019 RVA: 0x00293358 File Offset: 0x00291558
		public Parry()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F80 RID: 8064
		// (get) Token: 0x06007544 RID: 30020 RVA: 0x00293365 File Offset: 0x00291565
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<SovereignBlade>(false),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x17001F81 RID: 8065
		// (get) Token: 0x06007545 RID: 30021 RVA: 0x00293389 File Offset: 0x00291589
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ParryPower>(10m));
			}
		}

		// Token: 0x06007546 RID: 30022 RVA: 0x0029339C File Offset: 0x0029159C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ParryPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ParryPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x002933E7 File Offset: 0x002915E7
		protected override void OnUpgrade()
		{
			base.DynamicVars["ParryPower"].UpgradeValueBy(4m);
		}
	}
}
