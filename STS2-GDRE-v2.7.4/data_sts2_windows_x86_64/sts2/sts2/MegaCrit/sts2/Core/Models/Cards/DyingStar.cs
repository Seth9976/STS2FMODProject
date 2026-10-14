using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A1 RID: 2465
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DyingStar : CardModel
	{
		// Token: 0x060070F8 RID: 28920 RVA: 0x0028A596 File Offset: 0x00288796
		public DyingStar()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001DAE RID: 7598
		// (get) Token: 0x060070F9 RID: 28921 RVA: 0x0028A5A3 File Offset: 0x002887A3
		public override int CanonicalStarCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001DAF RID: 7599
		// (get) Token: 0x060070FA RID: 28922 RVA: 0x0028A5A6 File Offset: 0x002887A6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(9m, ValueProp.Move),
					new DynamicVar("StrengthLoss", 9m)
				});
			}
		}

		// Token: 0x17001DB0 RID: 7600
		// (get) Token: 0x060070FB RID: 28923 RVA: 0x0028A5D7 File Offset: 0x002887D7
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17001DB1 RID: 7601
		// (get) Token: 0x060070FC RID: 28924 RVA: 0x0028A5E0 File Offset: 0x002887E0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060070FD RID: 28925 RVA: 0x0028A600 File Offset: 0x00288800
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);
			IReadOnlyList<Creature> enemies = base.CombatState.HittableEnemies;
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.SpawningHitVfxOnEachCreature()
				.Execute(choiceContext);
			foreach (Creature enemy in enemies)
			{
				await PowerCmd.Apply<DyingStarPower>(choiceContext, enemy, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
				VfxCmd.PlayOnCreature(enemy, "vfx/vfx_attack_slash");
				enemy = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x0028A653 File Offset: 0x00288853
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(2m);
		}

		// Token: 0x04002863 RID: 10339
		private const string _strengthLossKey = "StrengthLoss";
	}
}
