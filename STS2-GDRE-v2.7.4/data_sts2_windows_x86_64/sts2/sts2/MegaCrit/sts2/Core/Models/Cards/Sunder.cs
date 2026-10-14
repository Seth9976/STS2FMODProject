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
	// Token: 0x02000AF9 RID: 2809
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sunder : CardModel
	{
		// Token: 0x06007845 RID: 30789 RVA: 0x0029976F File Offset: 0x0029796F
		public Sunder()
			: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020B8 RID: 8376
		// (get) Token: 0x06007846 RID: 30790 RVA: 0x0029977C File Offset: 0x0029797C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(26m, ValueProp.Move),
					new EnergyVar(3)
				});
			}
		}

		// Token: 0x170020B9 RID: 8377
		// (get) Token: 0x06007847 RID: 30791 RVA: 0x002997A2 File Offset: 0x002979A2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007848 RID: 30792 RVA: 0x002997B0 File Offset: 0x002979B0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			if (attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Any<DamageResult>((DamageResult r) => r.WasTargetKilled))
			{
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			}
		}

		// Token: 0x06007849 RID: 30793 RVA: 0x00299803 File Offset: 0x00297A03
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(8m);
		}
	}
}
