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
	// Token: 0x02000986 RID: 2438
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Defile : CardModel
	{
		// Token: 0x06007063 RID: 28771 RVA: 0x0028947B File Offset: 0x0028767B
		public Defile()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D69 RID: 7529
		// (get) Token: 0x06007064 RID: 28772 RVA: 0x00289488 File Offset: 0x00287688
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(13m, ValueProp.Move));
			}
		}

		// Token: 0x17001D6A RID: 7530
		// (get) Token: 0x06007065 RID: 28773 RVA: 0x0028949C File Offset: 0x0028769C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x002894A4 File Offset: 0x002876A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x002894F7 File Offset: 0x002876F7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
