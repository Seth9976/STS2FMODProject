using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000945 RID: 2373
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Capacitor : CardModel
	{
		// Token: 0x06006F11 RID: 28433 RVA: 0x00286A98 File Offset: 0x00284C98
		public Capacitor()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x06006F12 RID: 28434 RVA: 0x00286AA5 File Offset: 0x00284CA5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(2));
			}
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x00286AB4 File Offset: 0x00284CB4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await OrbCmd.AddSlots(base.Owner, base.DynamicVars.Repeat.IntValue);
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x00286AF7 File Offset: 0x00284CF7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
