using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A33 RID: 2611
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Mangle : CardModel
	{
		// Token: 0x0600741E RID: 29726 RVA: 0x00290DBF File Offset: 0x0028EFBF
		public Mangle()
			: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F05 RID: 7941
		// (get) Token: 0x0600741F RID: 29727 RVA: 0x00290DCC File Offset: 0x0028EFCC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(15m, ValueProp.Move),
					new DynamicVar("StrengthLoss", 10m)
				});
			}
		}

		// Token: 0x17001F06 RID: 7942
		// (get) Token: 0x06007420 RID: 29728 RVA: 0x00290E00 File Offset: 0x0028F000
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06007421 RID: 29729 RVA: 0x00290E20 File Offset: 0x0028F020
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			await PowerCmd.Apply<ManglePower>(choiceContext, cardPlay.Target, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007422 RID: 29730 RVA: 0x00290E73 File Offset: 0x0028F073
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(5m);
		}

		// Token: 0x040028A0 RID: 10400
		private const string _strengthLossKey = "StrengthLoss";
	}
}
