using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000742 RID: 1858
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ambergris : PotionModel
	{
		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x06005D2B RID: 23851 RVA: 0x0025764E File Offset: 0x0025584E
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Event;
			}
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x06005D2C RID: 23852 RVA: 0x00257651 File Offset: 0x00255851
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.AnyTime;
			}
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x06005D2D RID: 23853 RVA: 0x00257654 File Offset: 0x00255854
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x06005D2E RID: 23854 RVA: 0x00257657 File Offset: 0x00255857
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("HealPercent", 50m));
			}
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x00257670 File Offset: 0x00255870
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, Colors.Red);
			}
			await CreatureCmd.Heal(target, target.MaxHp * base.DynamicVars["HealPercent"].BaseValue / 100m, true);
			if (CombatManager.Instance.IsInProgress)
			{
				await PowerCmd.Apply<AmbergrisPower>(choiceContext, target, 1m, base.Owner.Creature, null, false);
			}
		}

		// Token: 0x0400252F RID: 9519
		private const string _healPercentKey = "HealPercent";
	}
}
