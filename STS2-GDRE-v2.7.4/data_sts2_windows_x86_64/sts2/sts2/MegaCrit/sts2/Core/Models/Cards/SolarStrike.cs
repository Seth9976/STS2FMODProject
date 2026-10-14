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
	// Token: 0x02000AD4 RID: 2772
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SolarStrike : CardModel
	{
		// Token: 0x06007778 RID: 30584 RVA: 0x00297B37 File Offset: 0x00295D37
		public SolarStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700206D RID: 8301
		// (get) Token: 0x06007779 RID: 30585 RVA: 0x00297B44 File Offset: 0x00295D44
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x1700206E RID: 8302
		// (get) Token: 0x0600777A RID: 30586 RVA: 0x00297B53 File Offset: 0x00295D53
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(9m, ValueProp.Move),
					new StarsVar(1)
				});
			}
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x00297B7C File Offset: 0x00295D7C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x00297BCF File Offset: 0x00295DCF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Stars.UpgradeValueBy(1m);
		}
	}
}
