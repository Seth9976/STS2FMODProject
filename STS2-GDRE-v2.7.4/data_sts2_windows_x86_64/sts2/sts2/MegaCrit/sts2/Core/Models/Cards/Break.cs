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
	// Token: 0x0200092F RID: 2351
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Break : CardModel
	{
		// Token: 0x06006EA9 RID: 28329 RVA: 0x00285D42 File Offset: 0x00283F42
		public Break()
			: base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x06006EAA RID: 28330 RVA: 0x00285D50 File Offset: 0x00283F50
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x06006EAB RID: 28331 RVA: 0x00285D70 File Offset: 0x00283F70
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(20m, ValueProp.Move),
					new PowerVar<VulnerablePower>(5m)
				});
			}
		}

		// Token: 0x06006EAC RID: 28332 RVA: 0x00285D9C File Offset: 0x00283F9C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EAD RID: 28333 RVA: 0x00285DEF File Offset: 0x00283FEF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(10m);
			base.DynamicVars.Vulnerable.UpgradeValueBy(2m);
		}
	}
}
