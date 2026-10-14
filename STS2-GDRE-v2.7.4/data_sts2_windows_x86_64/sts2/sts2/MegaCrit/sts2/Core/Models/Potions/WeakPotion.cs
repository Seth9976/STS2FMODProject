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
	// Token: 0x02000782 RID: 1922
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeakPotion : PotionModel
	{
		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x0025988B File Offset: 0x00257A8B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06005EC4 RID: 24260 RVA: 0x0025988E File Offset: 0x00257A8E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x00259891 File Offset: 0x00257A91
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06005EC6 RID: 24262 RVA: 0x00259894 File Offset: 0x00257A94
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<WeakPower>(3m));
			}
		}

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06005EC7 RID: 24263 RVA: 0x002598A8 File Offset: 0x00257AA8
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x002598C8 File Offset: 0x00257AC8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("94f882"));
			}
			await PowerCmd.Apply<WeakPower>(choiceContext, target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
