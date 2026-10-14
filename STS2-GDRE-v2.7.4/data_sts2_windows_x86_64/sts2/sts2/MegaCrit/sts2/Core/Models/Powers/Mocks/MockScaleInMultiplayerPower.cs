using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x0200073F RID: 1855
	public class MockScaleInMultiplayerPower : PowerModel
	{
		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x06005D1B RID: 23835 RVA: 0x002575CF File Offset: 0x002557CF
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06005D1C RID: 23836 RVA: 0x002575D2 File Offset: 0x002557D2
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x002575D5 File Offset: 0x002557D5
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x06005D1E RID: 23838 RVA: 0x002575D8 File Offset: 0x002557D8
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x06005D1F RID: 23839 RVA: 0x002575DB File Offset: 0x002557DB
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return this.shouldScaleInMultiplayer;
			}
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x002575E3 File Offset: 0x002557E3
		[NullableContext(1)]
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (MockScaleInMultiplayerPower.getScaledAmountForMultiplayer != null)
			{
				return MockScaleInMultiplayerPower.getScaledAmountForMultiplayer(combatState, applier, amount, target, cardSource);
			}
			return base.GetScaledAmountForMultiplayer(combatState, applier, amount, target, cardSource);
		}

		// Token: 0x0400252D RID: 9517
		public bool shouldScaleInMultiplayer = true;

		// Token: 0x0400252E RID: 9518
		[Nullable(2)]
		public static MockScaleInMultiplayerPower.GetScaledAmountForMultiplayerDelegate getScaledAmountForMultiplayer;

		// Token: 0x02001CC5 RID: 7365
		// (Invoke) Token: 0x0600B209 RID: 45577
		public delegate decimal GetScaledAmountForMultiplayerDelegate(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource);
	}
}
