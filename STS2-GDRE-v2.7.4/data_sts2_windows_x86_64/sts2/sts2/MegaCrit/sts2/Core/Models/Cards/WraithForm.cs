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
	// Token: 0x02000B40 RID: 2880
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WraithForm : CardModel
	{
		// Token: 0x060079C8 RID: 31176 RVA: 0x0029C6CB File Offset: 0x0029A8CB
		public WraithForm()
			: base(3, CardType.Power, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17002159 RID: 8537
		// (get) Token: 0x060079C9 RID: 31177 RVA: 0x0029C6D8 File Offset: 0x0029A8D8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<IntangiblePower>(2m),
					new PowerVar<WraithFormPower>(1m)
				});
			}
		}

		// Token: 0x1700215A RID: 8538
		// (get) Token: 0x060079CA RID: 31178 RVA: 0x0029C700 File Offset: 0x0029A900
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<IntangiblePower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x060079CB RID: 31179 RVA: 0x0029C73C File Offset: 0x0029A93C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<IntangiblePower>(choiceContext, base.Owner.Creature, base.DynamicVars["IntangiblePower"].BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<WraithFormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["WraithFormPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060079CC RID: 31180 RVA: 0x0029C787 File Offset: 0x0029A987
		protected override void OnUpgrade()
		{
			base.DynamicVars["IntangiblePower"].UpgradeValueBy(1m);
		}
	}
}
