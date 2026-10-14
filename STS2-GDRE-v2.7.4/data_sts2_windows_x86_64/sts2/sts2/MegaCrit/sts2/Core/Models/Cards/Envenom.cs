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
	// Token: 0x020009AC RID: 2476
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Envenom : CardModel
	{
		// Token: 0x06007132 RID: 28978 RVA: 0x0028ABEC File Offset: 0x00288DEC
		public Envenom()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DC9 RID: 7625
		// (get) Token: 0x06007133 RID: 28979 RVA: 0x0028ABF9 File Offset: 0x00288DF9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<EnvenomPower>(1m));
			}
		}

		// Token: 0x17001DCA RID: 7626
		// (get) Token: 0x06007134 RID: 28980 RVA: 0x0028AC0C File Offset: 0x00288E0C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x0028AC2C File Offset: 0x00288E2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<EnvenomPower>(choiceContext, base.Owner.Creature, base.DynamicVars["EnvenomPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x0028AC77 File Offset: 0x00288E77
		protected override void OnUpgrade()
		{
			base.DynamicVars["EnvenomPower"].UpgradeValueBy(1m);
		}
	}
}
