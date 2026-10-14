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
	// Token: 0x02000A61 RID: 2657
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Outbreak : CardModel
	{
		// Token: 0x06007515 RID: 29973 RVA: 0x00292DC9 File Offset: 0x00290FC9
		public Outbreak()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F6D RID: 8045
		// (get) Token: 0x06007516 RID: 29974 RVA: 0x00292DD8 File Offset: 0x00290FD8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x17001F6E RID: 8046
		// (get) Token: 0x06007517 RID: 29975 RVA: 0x00292DF8 File Offset: 0x00290FF8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<OutbreakPower>(4m));
			}
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x00292E0C File Offset: 0x0029100C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<OutbreakPower>(choiceContext, base.Owner.Creature, base.DynamicVars["OutbreakPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x00292E57 File Offset: 0x00291057
		protected override void OnUpgrade()
		{
			base.DynamicVars["OutbreakPower"].UpgradeValueBy(1m);
		}
	}
}
