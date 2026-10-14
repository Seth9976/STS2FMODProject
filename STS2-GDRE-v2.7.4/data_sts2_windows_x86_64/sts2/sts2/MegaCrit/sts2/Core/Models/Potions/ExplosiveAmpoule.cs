using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000758 RID: 1880
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ExplosiveAmpoule : PotionModel
	{
		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x06005DAD RID: 23981 RVA: 0x0025806B File Offset: 0x0025626B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x06005DAE RID: 23982 RVA: 0x0025806E File Offset: 0x0025626E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x06005DAF RID: 23983 RVA: 0x00258071 File Offset: 0x00256271
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AllEnemies;
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x06005DB0 RID: 23984 RVA: 0x00258074 File Offset: 0x00256274
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x00258088 File Offset: 0x00256288
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			Creature player = base.Owner.Creature;
			DamageVar damage = base.DynamicVars.Damage;
			IReadOnlyList<Creature> targets = player.CombatState.HittableEnemies;
			foreach (Creature creature in targets)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(creature));
				}
			}
			await Cmd.CustomScaledWait(0.2f, 0.3f, false, default(CancellationToken));
			await CreatureCmd.Damage(choiceContext, targets, damage.BaseValue, damage.Props, player, null, null);
		}
	}
}
