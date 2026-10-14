using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099F RID: 2463
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dualcast : CardModel
	{
		// Token: 0x060070EE RID: 28910 RVA: 0x0028A496 File Offset: 0x00288696
		public Dualcast()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001DAA RID: 7594
		// (get) Token: 0x060070EF RID: 28911 RVA: 0x0028A4A3 File Offset: 0x002886A3
		public override OrbEvokeType OrbEvokeType
		{
			get
			{
				return OrbEvokeType.Front;
			}
		}

		// Token: 0x17001DAB RID: 7595
		// (get) Token: 0x060070F0 RID: 28912 RVA: 0x0028A4A6 File Offset: 0x002886A6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060070F1 RID: 28913 RVA: 0x0028A4B8 File Offset: 0x002886B8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (base.Owner.PlayerCombatState.OrbQueue.Orbs.Count > 0)
			{
				await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
				await OrbCmd.EvokeNext(choiceContext, base.Owner, false);
				await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
				await OrbCmd.EvokeNext(choiceContext, base.Owner, true);
			}
		}

		// Token: 0x060070F2 RID: 28914 RVA: 0x0028A503 File Offset: 0x00288703
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
