using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000974 RID: 2420
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Darkness : CardModel
	{
		// Token: 0x06007001 RID: 28673 RVA: 0x0028892D File Offset: 0x00286B2D
		public Darkness()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D38 RID: 7480
		// (get) Token: 0x06007002 RID: 28674 RVA: 0x0028893A File Offset: 0x00286B3A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<DarkOrb>()
				});
			}
		}

		// Token: 0x06007003 RID: 28675 RVA: 0x00288960 File Offset: 0x00286B60
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await OrbCmd.Channel<DarkOrb>(choiceContext, base.Owner);
			IEnumerable<OrbModel> enumerable = base.Owner.PlayerCombatState.OrbQueue.Orbs.Where<OrbModel>((OrbModel orb) => orb is DarkOrb);
			int triggerCount = (base.IsUpgraded ? 2 : 1);
			foreach (OrbModel darknessOrb in enumerable)
			{
				for (int i = 0; i < triggerCount; i++)
				{
					await OrbCmd.Passive(choiceContext, darknessOrb, null, false);
				}
				darknessOrb = null;
			}
			IEnumerator<OrbModel> enumerator = null;
		}
	}
}
