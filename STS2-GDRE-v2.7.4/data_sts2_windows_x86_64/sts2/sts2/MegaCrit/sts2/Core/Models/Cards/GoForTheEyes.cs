using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020009E5 RID: 2533
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoForTheEyes : CardModel
	{
		// Token: 0x06007276 RID: 29302 RVA: 0x0028D602 File Offset: 0x0028B802
		public GoForTheEyes()
			: base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E54 RID: 7764
		// (get) Token: 0x06007277 RID: 29303 RVA: 0x0028D60F File Offset: 0x0028B80F
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				if (base.CombatState == null)
				{
					return false;
				}
				return base.CombatState.HittableEnemies.Any<Creature>(delegate(Creature e)
				{
					MonsterModel monster = e.Monster;
					return monster != null && monster.IntendsToAttack;
				});
			}
		}

		// Token: 0x17001E55 RID: 7765
		// (get) Token: 0x06007278 RID: 29304 RVA: 0x0028D64C File Offset: 0x0028B84C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x17001E56 RID: 7766
		// (get) Token: 0x06007279 RID: 29305 RVA: 0x0028D66C File Offset: 0x0028B86C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(3m, ValueProp.Move),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x0028D698 File Offset: 0x0028B898
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			if (cardPlay.Target.Monster.IntendsToAttack)
			{
				await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
			}
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x0028D6EB File Offset: 0x0028B8EB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Weak.UpgradeValueBy(1m);
		}
	}
}
