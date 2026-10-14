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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009CD RID: 2509
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FocusedStrike : CardModel
	{
		// Token: 0x060071ED RID: 29165 RVA: 0x0028C563 File Offset: 0x0028A763
		public FocusedStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E19 RID: 7705
		// (get) Token: 0x060071EE RID: 29166 RVA: 0x0028C570 File Offset: 0x0028A770
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001E1A RID: 7706
		// (get) Token: 0x060071EF RID: 29167 RVA: 0x0028C580 File Offset: 0x0028A780
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x17001E1B RID: 7707
		// (get) Token: 0x060071F0 RID: 29168 RVA: 0x0028C5A0 File Offset: 0x0028A7A0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(9m, ValueProp.Move),
					new PowerVar<FocusPower>(1m)
				});
			}
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x0028C5CC File Offset: 0x0028A7CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<FocusedStrikePower>(choiceContext, base.Owner.Creature, base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060071F2 RID: 29170 RVA: 0x0028C61F File Offset: 0x0028A81F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars["FocusPower"].UpgradeValueBy(1m);
		}
	}
}
