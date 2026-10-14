using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A65 RID: 2661
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PactsEnd : CardModel
	{
		// Token: 0x0600752A RID: 29994 RVA: 0x00293022 File Offset: 0x00291222
		public PactsEnd()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001F76 RID: 8054
		// (get) Token: 0x0600752B RID: 29995 RVA: 0x0029302F File Offset: 0x0029122F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(17m, ValueProp.Move),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x17001F77 RID: 8055
		// (get) Token: 0x0600752C RID: 29996 RVA: 0x00293055 File Offset: 0x00291255
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.CanDealDamage;
			}
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x00293060 File Offset: 0x00291260
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.CanDealDamage)
			{
				await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
					.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
					.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
					.WithHitVfxSpawnedAtBase()
					.Execute(choiceContext);
			}
		}

		// Token: 0x17001F78 RID: 8056
		// (get) Token: 0x0600752E RID: 29998 RVA: 0x002930B3 File Offset: 0x002912B3
		private bool CanDealDamage
		{
			get
			{
				return CardPile.GetCards(base.Owner, new PileType[] { PileType.Exhaust }).Count<CardModel>() >= base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x002930E4 File Offset: 0x002912E4
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
