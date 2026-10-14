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
	// Token: 0x02000962 RID: 2402
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Coolant : CardModel
	{
		// Token: 0x06006FA8 RID: 28584 RVA: 0x00287DA2 File Offset: 0x00285FA2
		public Coolant()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D16 RID: 7446
		// (get) Token: 0x06006FA9 RID: 28585 RVA: 0x00287DAF File Offset: 0x00285FAF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001D17 RID: 7447
		// (get) Token: 0x06006FAA RID: 28586 RVA: 0x00287DC1 File Offset: 0x00285FC1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<CoolantPower>(2m));
			}
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x00287DD4 File Offset: 0x00285FD4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CoolantPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CoolantPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x00287E1F File Offset: 0x0028601F
		protected override void OnUpgrade()
		{
			base.DynamicVars["CoolantPower"].UpgradeValueBy(1m);
		}
	}
}
