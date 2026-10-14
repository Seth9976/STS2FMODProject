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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009BB RID: 2491
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Feed : CardModel
	{
		// Token: 0x06007188 RID: 29064 RVA: 0x0028B78F File Offset: 0x0028998F
		public Feed()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DF0 RID: 7664
		// (get) Token: 0x06007189 RID: 29065 RVA: 0x0028B79C File Offset: 0x0028999C
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DF1 RID: 7665
		// (get) Token: 0x0600718A RID: 29066 RVA: 0x0028B79F File Offset: 0x0028999F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new MaxHpVar(3m)
				});
			}
		}

		// Token: 0x17001DF2 RID: 7666
		// (get) Token: 0x0600718B RID: 29067 RVA: 0x0028B7CA File Offset: 0x002899CA
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001DF3 RID: 7667
		// (get) Token: 0x0600718C RID: 29068 RVA: 0x0028B7D2 File Offset: 0x002899D2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Fatal, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x0028B7E4 File Offset: 0x002899E4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_bite", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			if (shouldTriggerFatal)
			{
				if (attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Any<DamageResult>((DamageResult r) => r.WasTargetKilled))
				{
					await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.IntValue);
				}
			}
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x0028B837 File Offset: 0x00289A37
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.MaxHp.UpgradeValueBy(1m);
		}
	}
}
