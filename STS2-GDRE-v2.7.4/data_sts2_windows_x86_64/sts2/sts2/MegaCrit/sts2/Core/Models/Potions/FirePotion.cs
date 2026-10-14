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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200075A RID: 1882
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FirePotion : PotionModel
	{
		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x06005DBB RID: 23995 RVA: 0x00258193 File Offset: 0x00256393
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x06005DBC RID: 23996 RVA: 0x00258196 File Offset: 0x00256396
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x06005DBD RID: 23997 RVA: 0x00258199 File Offset: 0x00256399
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x06005DBE RID: 23998 RVA: 0x0025819C File Offset: 0x0025639C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(20m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005DBF RID: 23999 RVA: 0x002581B0 File Offset: 0x002563B0
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			DamageVar damage = base.DynamicVars.Damage;
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target, VfxColor.Red));
			}
			await CreatureCmd.Damage(choiceContext, target, damage.BaseValue, damage.Props, base.Owner.Creature, null, null);
		}
	}
}
