using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B42 RID: 2882
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WroughtInWar : CardModel
	{
		// Token: 0x060079D0 RID: 31184 RVA: 0x0029C7C9 File Offset: 0x0029A9C9
		public WroughtInWar()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700215D RID: 8541
		// (get) Token: 0x060079D1 RID: 31185 RVA: 0x0029C7D6 File Offset: 0x0029A9D6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(7m, ValueProp.Move),
					new ForgeVar(7)
				});
			}
		}

		// Token: 0x1700215E RID: 8542
		// (get) Token: 0x060079D2 RID: 31186 RVA: 0x0029C7FB File Offset: 0x0029A9FB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x0029C804 File Offset: 0x0029AA04
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(choiceContext);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x0029C857 File Offset: 0x0029AA57
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.Forge.UpgradeValueBy(2m);
		}
	}
}
