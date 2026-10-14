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
	// Token: 0x02000A99 RID: 2713
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RefineBlade : CardModel
	{
		// Token: 0x06007632 RID: 30258 RVA: 0x0029536B File Offset: 0x0029356B
		public RefineBlade()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001FDE RID: 8158
		// (get) Token: 0x06007633 RID: 30259 RVA: 0x00295378 File Offset: 0x00293578
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new ForgeVar(9),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001FDF RID: 8159
		// (get) Token: 0x06007634 RID: 30260 RVA: 0x00295398 File Offset: 0x00293598
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x06007635 RID: 30261 RVA: 0x002953A0 File Offset: 0x002935A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007636 RID: 30262 RVA: 0x002953EB File Offset: 0x002935EB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(4m);
		}
	}
}
