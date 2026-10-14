using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A08 RID: 2568
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hyperbeam : CardModel
	{
		// Token: 0x0600732D RID: 29485 RVA: 0x0028ED8E File Offset: 0x0028CF8E
		public Hyperbeam()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001EA3 RID: 7843
		// (get) Token: 0x0600732E RID: 29486 RVA: 0x0028ED9B File Offset: 0x0028CF9B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(30m, ValueProp.Move),
					new PowerVar<FocusPower>(3m)
				});
			}
		}

		// Token: 0x17001EA4 RID: 7844
		// (get) Token: 0x0600732F RID: 29487 RVA: 0x0028EDC8 File Offset: 0x0028CFC8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x06007330 RID: 29488 RVA: 0x0028EDE8 File Offset: 0x0028CFE8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithAttackerAnim("Cast", 0.5f, null)
				.BeforeDamage(async delegate
				{
					List<Creature> enemies = base.CombatState.Enemies.Where<Creature>((Creature e) => e.IsAlive).ToList<Creature>();
					NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(base.Owner.Creature, enemies.Last<Creature>());
					if (nhyperbeamVfx != null)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.CombatVfxContainer.AddChildSafely(nhyperbeamVfx);
						}
						await Cmd.Wait(0.5f, false);
					}
					foreach (Creature creature in enemies)
					{
						NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(base.Owner.Creature, creature);
						if (nhyperbeamImpactVfx != null)
						{
							NCombatRoom instance2 = NCombatRoom.Instance;
							if (instance2 != null)
							{
								instance2.CombatVfxContainer.AddChildSafely(nhyperbeamImpactVfx);
							}
						}
					}
				})
				.Execute(choiceContext);
			await PowerCmd.Apply<FocusPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007331 RID: 29489 RVA: 0x0028EE3B File Offset: 0x0028D03B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(8m);
		}
	}
}
