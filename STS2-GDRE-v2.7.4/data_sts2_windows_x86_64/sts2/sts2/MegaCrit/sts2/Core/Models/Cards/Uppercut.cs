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
	// Token: 0x02000B2E RID: 2862
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Uppercut : CardModel
	{
		// Token: 0x06007966 RID: 31078 RVA: 0x0029BB4A File Offset: 0x00299D4A
		public Uppercut()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700212D RID: 8493
		// (get) Token: 0x06007967 RID: 31079 RVA: 0x0029BB57 File Offset: 0x00299D57
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(13m, ValueProp.Move),
					new DynamicVar("Power", 1m)
				});
			}
		}

		// Token: 0x1700212E RID: 8494
		// (get) Token: 0x06007968 RID: 31080 RVA: 0x0029BB88 File Offset: 0x00299D88
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x06007969 RID: 31081 RVA: 0x0029BBC4 File Offset: 0x00299DC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			int amount = base.DynamicVars["Power"].IntValue;
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x0029BC17 File Offset: 0x00299E17
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(1m);
		}

		// Token: 0x040028D8 RID: 10456
		private const string _powerKey = "Power";
	}
}
