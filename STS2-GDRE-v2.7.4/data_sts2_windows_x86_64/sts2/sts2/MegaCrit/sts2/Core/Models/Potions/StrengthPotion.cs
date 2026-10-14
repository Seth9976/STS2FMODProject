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
	// Token: 0x0200077E RID: 1918
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrengthPotion : PotionModel
	{
		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06005EAA RID: 24234 RVA: 0x00259683 File Offset: 0x00257883
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06005EAB RID: 24235 RVA: 0x00259686 File Offset: 0x00257886
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06005EAC RID: 24236 RVA: 0x00259689 File Offset: 0x00257889
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06005EAD RID: 24237 RVA: 0x0025968C File Offset: 0x0025788C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(2m));
			}
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06005EAE RID: 24238 RVA: 0x002596A0 File Offset: 0x002578A0
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x002596C0 File Offset: 0x002578C0
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("fd2155"));
			}
			await PowerCmd.Apply<StrengthPower>(choiceContext, target, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
