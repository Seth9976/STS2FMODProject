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
	// Token: 0x02000A85 RID: 2693
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Prowess : CardModel
	{
		// Token: 0x060075CA RID: 30154 RVA: 0x00294557 File Offset: 0x00292757
		public Prowess()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FB4 RID: 8116
		// (get) Token: 0x060075CB RID: 30155 RVA: 0x00294564 File Offset: 0x00292764
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x17001FB5 RID: 8117
		// (get) Token: 0x060075CC RID: 30156 RVA: 0x0029459E File Offset: 0x0029279E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x002945C8 File Offset: 0x002927C8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x00294613 File Offset: 0x00292813
		protected override void OnUpgrade()
		{
			base.DynamicVars.Dexterity.UpgradeValueBy(1m);
			base.DynamicVars.Strength.UpgradeValueBy(1m);
		}
	}
}
