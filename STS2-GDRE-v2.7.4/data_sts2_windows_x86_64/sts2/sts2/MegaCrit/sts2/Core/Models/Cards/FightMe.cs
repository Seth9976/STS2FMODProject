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
	// Token: 0x020009C1 RID: 2497
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FightMe : CardModel
	{
		// Token: 0x060071AC RID: 29100 RVA: 0x0028BBD3 File Offset: 0x00289DD3
		public FightMe()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E01 RID: 7681
		// (get) Token: 0x060071AD RID: 29101 RVA: 0x0028BBE0 File Offset: 0x00289DE0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001E02 RID: 7682
		// (get) Token: 0x060071AE RID: 29102 RVA: 0x0028BC00 File Offset: 0x00289E00
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(5m, ValueProp.Move),
					new RepeatVar(2),
					new PowerVar<StrengthPower>(3m),
					new DynamicVar("EnemyStrength", 1m)
				});
			}
		}

		// Token: 0x060071AF RID: 29103 RVA: 0x0028BC50 File Offset: 0x00289E50
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				.Execute(choiceContext);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, base.DynamicVars["EnemyStrength"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060071B0 RID: 29104 RVA: 0x0028BCA3 File Offset: 0x00289EA3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Strength.UpgradeValueBy(1m);
		}

		// Token: 0x0400286D RID: 10349
		private const string _enemyStrengthKey = "EnemyStrength";
	}
}
