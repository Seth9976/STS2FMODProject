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
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000755 RID: 1877
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EnergyPotion : PotionModel
	{
		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06005D9B RID: 23963 RVA: 0x00257F1F File Offset: 0x0025611F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06005D9C RID: 23964 RVA: 0x00257F22 File Offset: 0x00256122
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06005D9D RID: 23965 RVA: 0x00257F25 File Offset: 0x00256125
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06005D9E RID: 23966 RVA: 0x00257F28 File Offset: 0x00256128
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x06005D9F RID: 23967 RVA: 0x00257F35 File Offset: 0x00256135
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x00257F44 File Offset: 0x00256144
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("f2e35c"));
			}
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, target.Player);
		}
	}
}
