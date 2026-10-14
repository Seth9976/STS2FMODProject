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
	// Token: 0x02000A94 RID: 2708
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reap : CardModel
	{
		// Token: 0x0600761A RID: 30234 RVA: 0x00295087 File Offset: 0x00293287
		public Reap()
			: base(3, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FD5 RID: 8149
		// (get) Token: 0x0600761B RID: 30235 RVA: 0x00295094 File Offset: 0x00293294
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(27m, ValueProp.Move));
			}
		}

		// Token: 0x17001FD6 RID: 8150
		// (get) Token: 0x0600761C RID: 30236 RVA: 0x002950A8 File Offset: 0x002932A8
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x002950B0 File Offset: 0x002932B0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x00295103 File Offset: 0x00293303
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
