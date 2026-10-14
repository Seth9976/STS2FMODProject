using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	/// <summary>
	/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
	/// </summary>
	// Token: 0x02000A21 RID: 2593
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnockoutBlow : CardModel
	{
		/// <summary>
		/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
		/// </summary>
		// Token: 0x060073AD RID: 29613 RVA: 0x0028FE0D File Offset: 0x0028E00D
		public KnockoutBlow()
			: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ED5 RID: 7893
		// (get) Token: 0x060073AE RID: 29614 RVA: 0x0028FE1A File Offset: 0x0028E01A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(30m, ValueProp.Move),
					new StarsVar(5)
				});
			}
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x0028FE40 File Offset: 0x0028E040
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			if (attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Any<DamageResult>((DamageResult r) => r.WasTargetKilled))
			{
				await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			}
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x0028FE93 File Offset: 0x0028E093
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(8m);
		}
	}
}
