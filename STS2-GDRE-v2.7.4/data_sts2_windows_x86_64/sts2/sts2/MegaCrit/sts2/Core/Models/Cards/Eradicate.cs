using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009AE RID: 2478
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Eradicate : CardModel
	{
		// Token: 0x0600713D RID: 28989 RVA: 0x0028AD4B File Offset: 0x00288F4B
		public Eradicate()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DCE RID: 7630
		// (get) Token: 0x0600713E RID: 28990 RVA: 0x0028AD58 File Offset: 0x00288F58
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DCF RID: 7631
		// (get) Token: 0x0600713F RID: 28991 RVA: 0x0028AD5B File Offset: 0x00288F5B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x17001DD0 RID: 7632
		// (get) Token: 0x06007140 RID: 28992 RVA: 0x0028AD63 File Offset: 0x00288F63
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x0028AD78 File Offset: 0x00288F78
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.ResolveEnergyXValue()).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007142 RID: 28994 RVA: 0x0028ADCB File Offset: 0x00288FCB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
