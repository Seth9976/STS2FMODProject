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
	// Token: 0x02000781 RID: 1921
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VulnerablePotion : PotionModel
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06005EBC RID: 24252 RVA: 0x002597F3 File Offset: 0x002579F3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06005EBD RID: 24253 RVA: 0x002597F6 File Offset: 0x002579F6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06005EBE RID: 24254 RVA: 0x002597F9 File Offset: 0x002579F9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06005EBF RID: 24255 RVA: 0x002597FC File Offset: 0x002579FC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VulnerablePower>(3m));
			}
		}

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06005EC0 RID: 24256 RVA: 0x00259810 File Offset: 0x00257A10
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00259830 File Offset: 0x00257A30
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("fd2155"));
			}
			await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
