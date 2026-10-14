using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200076D RID: 1901
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionOfBinding : PotionModel
	{
		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x06005E39 RID: 24121 RVA: 0x00258D17 File Offset: 0x00256F17
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06005E3A RID: 24122 RVA: 0x00258D1A File Offset: 0x00256F1A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x06005E3B RID: 24123 RVA: 0x00258D1D File Offset: 0x00256F1D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AllEnemies;
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x06005E3C RID: 24124 RVA: 0x00258D20 File Offset: 0x00256F20
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<VulnerablePower>(1m),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06005E3D RID: 24125 RVA: 0x00258D48 File Offset: 0x00256F48
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x00258D84 File Offset: 0x00256F84
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			IReadOnlyList<Creature> targets = base.Owner.Creature.CombatState.HittableEnemies;
			foreach (Creature creature in targets)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NSmokePuffVfx.Create(creature, NSmokePuffVfx.SmokePuffColor.Green));
				}
			}
			await PowerCmd.Apply<WeakPower>(choiceContext, targets, base.DynamicVars["VulnerablePower"].IntValue, base.Owner.Creature, null, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, targets, base.DynamicVars["WeakPower"].IntValue, base.Owner.Creature, null, false);
		}
	}
}
