using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062F RID: 1583
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AsleepPower : PowerModel
	{
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060056BC RID: 22204 RVA: 0x0024BABD File Offset: 0x00249CBD
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060056BD RID: 22205 RVA: 0x0024BAC0 File Offset: 0x00249CC0
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x0024BAC4 File Offset: 0x00249CC4
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage != 0)
				{
					if (base.Owner.HasPower<PlatingPower>())
					{
						await PowerCmd.Remove(base.Owner.GetPower<PlatingPower>());
					}
					LagavulinMatriarch monster = (LagavulinMatriarch)base.Owner.Monster;
					SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken", 1f);
					await CreatureCmd.TriggerAnim(base.Owner, "Wake", 0.6f);
					monster.IsAwake = true;
					await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(monster.WakeUpMove), "SLASH_MOVE");
					await PowerCmd.Remove(this);
				}
			}
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x0024BB18 File Offset: 0x00249D18
		public override async Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Amount <= 1 && base.Owner.HasPower<PlatingPower>())
				{
					await PowerCmd.Remove(base.Owner.GetPower<PlatingPower>());
				}
			}
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x0024BB64 File Offset: 0x00249D64
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
				if (base.Amount <= 0)
				{
					await ((LagavulinMatriarch)base.Owner.Monster).WakeUpMove(Array.Empty<Creature>());
				}
			}
		}
	}
}
