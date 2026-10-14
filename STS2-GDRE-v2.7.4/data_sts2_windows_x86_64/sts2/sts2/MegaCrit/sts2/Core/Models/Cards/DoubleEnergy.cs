using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000998 RID: 2456
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DoubleEnergy : CardModel
	{
		// Token: 0x060070C6 RID: 28870 RVA: 0x0028A00F File Offset: 0x0028820F
		public DoubleEnergy()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D97 RID: 7575
		// (get) Token: 0x060070C7 RID: 28871 RVA: 0x0028A01C File Offset: 0x0028821C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001D98 RID: 7576
		// (get) Token: 0x060070C8 RID: 28872 RVA: 0x0028A024 File Offset: 0x00288224
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x060070C9 RID: 28873 RVA: 0x0028A034 File Offset: 0x00288234
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PlayerCmd.GainEnergy(base.Owner.PlayerCombatState.Energy, base.Owner);
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x0028A077 File Offset: 0x00288277
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
