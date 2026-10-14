using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000606 RID: 1542
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TwistedFunnel : RelicModel
	{
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x060055CD RID: 21965 RVA: 0x0024946B File Offset: 0x0024766B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x060055CE RID: 21966 RVA: 0x0024946E File Offset: 0x0024766E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(4m));
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x060055CF RID: 21967 RVA: 0x00249480 File Offset: 0x00247680
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x002494A0 File Offset: 0x002476A0
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					foreach (Creature creature in base.Owner.Creature.CombatState.HittableEnemies)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.CombatVfxContainer.AddChildSafely(NSmokePuffVfx.Create(creature, NSmokePuffVfx.SmokePuffColor.Green));
						}
					}
					await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
					foreach (Creature creature2 in base.Owner.Creature.CombatState.HittableEnemies)
					{
						await PowerCmd.Apply<PoisonPower>(choiceContext, creature2, base.DynamicVars["PoisonPower"].IntValue, base.Owner.Creature, null, false);
					}
					IEnumerator<Creature> enumerator2 = null;
				}
			}
		}
	}
}
