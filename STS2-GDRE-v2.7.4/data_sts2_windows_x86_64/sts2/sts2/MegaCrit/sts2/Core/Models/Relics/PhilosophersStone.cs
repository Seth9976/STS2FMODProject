using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B7 RID: 1463
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhilosophersStone : RelicModel
	{
		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x0024566F File Offset: 0x0024386F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x00245672 File Offset: 0x00243872
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x00245698 File Offset: 0x00243898
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.ForEnergy(this)
				});
			}
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x002456CA File Offset: 0x002438CA
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x002456F4 File Offset: 0x002438F4
		public override Task AfterCreatureAddedToCombat(Creature creature)
		{
			if (creature.Side == base.Owner.Creature.Side)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, base.DynamicVars["StrengthPower"].BaseValue, null, null, false);
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x00245748 File Offset: 0x00243948
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				IEnumerable<Creature> enumerable = from c in base.Owner.Creature.CombatState.GetOpponentsOf(base.Owner.Creature)
					where c.IsAlive
					select c;
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), enumerable, base.DynamicVars["StrengthPower"].BaseValue, null, null, false);
			}
		}
	}
}
