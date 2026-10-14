using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000776 RID: 1910
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShacklingPotion : PotionModel
	{
		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06005E74 RID: 24180 RVA: 0x002591DF File Offset: 0x002573DF
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06005E75 RID: 24181 RVA: 0x002591E2 File Offset: 0x002573E2
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06005E76 RID: 24182 RVA: 0x002591E5 File Offset: 0x002573E5
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AllEnemies;
			}
		}

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06005E77 RID: 24183 RVA: 0x002591E8 File Offset: 0x002573E8
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06005E78 RID: 24184 RVA: 0x00259208 File Offset: 0x00257408
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(7m));
			}
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x0025921C File Offset: 0x0025741C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			Creature creature = base.Owner.Creature;
			foreach (Creature creature2 in creature.CombatState.HittableEnemies)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.PlaySplashVfx(creature2, new Color("91a19f"));
				}
			}
			await PowerCmd.Apply<ShacklingPotionPower>(choiceContext, creature.CombatState.HittableEnemies, base.DynamicVars.Strength.IntValue, base.Owner.Creature, null, false);
		}
	}
}
