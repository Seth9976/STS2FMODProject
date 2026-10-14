using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000933 RID: 2355
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Buffer : CardModel
	{
		// Token: 0x06006EBE RID: 28350 RVA: 0x0028606F File Offset: 0x0028426F
		public Buffer()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x06006EBF RID: 28351 RVA: 0x0028607C File Offset: 0x0028427C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<BufferPower>(1m));
			}
		}

		// Token: 0x06006EC0 RID: 28352 RVA: 0x00286090 File Offset: 0x00284290
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<BufferPower>(choiceContext, base.Owner.Creature, base.DynamicVars["BufferPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EC1 RID: 28353 RVA: 0x002860DB File Offset: 0x002842DB
		protected override void OnUpgrade()
		{
			base.DynamicVars["BufferPower"].UpgradeValueBy(1m);
		}
	}
}
