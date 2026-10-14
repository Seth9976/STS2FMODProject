using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000970 RID: 2416
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DaggerSpray : CardModel
	{
		// Token: 0x06006FED RID: 28653 RVA: 0x0028863D File Offset: 0x0028683D
		public DaggerSpray()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001D33 RID: 7475
		// (get) Token: 0x06006FEE RID: 28654 RVA: 0x0028864A File Offset: 0x0028684A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(4m, ValueProp.Move));
			}
		}

		// Token: 0x06006FEF RID: 28655 RVA: 0x00288660 File Offset: 0x00286860
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			SfxCmd.Play("event:/sfx/characters/silent/silent_dagger_spray", 1f);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithAttackerFx(() => NDaggerSprayFlurryVfx.Create(base.Owner.Creature, new Color("#b1ccca"), true))
				.BeforeDamage(delegate
				{
					IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
					foreach (Creature creature in hittableEnemies)
					{
						NDaggerSprayImpactVfx ndaggerSprayImpactVfx = NDaggerSprayImpactVfx.Create(creature, new Color("#b1ccca"), true);
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.CombatVfxContainer.AddChildSafely(ndaggerSprayImpactVfx);
						}
					}
					return Task.CompletedTask;
				})
				.Execute(choiceContext);
		}

		// Token: 0x06006FF0 RID: 28656 RVA: 0x002886B3 File Offset: 0x002868B3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x0400285B RID: 10331
		private const string _daggerSpraySfx = "event:/sfx/characters/silent/silent_dagger_spray";
	}
}
