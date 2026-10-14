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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200076E RID: 1902
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionOfCapacity : PotionModel
	{
		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06005E40 RID: 24128 RVA: 0x00258DD7 File Offset: 0x00256FD7
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06005E41 RID: 24129 RVA: 0x00258DDA File Offset: 0x00256FDA
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x06005E42 RID: 24130 RVA: 0x00258DDD File Offset: 0x00256FDD
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06005E43 RID: 24131 RVA: 0x00258DE0 File Offset: 0x00256FE0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(2));
			}
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x00258DF0 File Offset: 0x00256FF0
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("91a19f"));
			}
			await OrbCmd.AddSlots(target.Player, base.DynamicVars.Repeat.IntValue);
		}
	}
}
