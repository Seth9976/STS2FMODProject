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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AFE RID: 2814
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SweepingBeam : CardModel
	{
		// Token: 0x0600785F RID: 30815 RVA: 0x00299AF3 File Offset: 0x00297CF3
		public SweepingBeam()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x170020C3 RID: 8387
		// (get) Token: 0x06007860 RID: 30816 RVA: 0x00299B00 File Offset: 0x00297D00
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06007861 RID: 30817 RVA: 0x00299B28 File Offset: 0x00297D28
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithAttackerAnim("Cast", 0.5f, null)
				.BeforeDamage(async delegate
				{
					List<Creature> list = base.CombatState.HittableEnemies.ToList<Creature>();
					NSweepingBeamVfx nsweepingBeamVfx = NSweepingBeamVfx.Create(base.Owner.Creature, list);
					if (nsweepingBeamVfx != null)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.CombatVfxContainer.AddChildSafely(nsweepingBeamVfx);
						}
						await Cmd.Wait(0.5f, false);
					}
				})
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x00299B7B File Offset: 0x00297D7B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
