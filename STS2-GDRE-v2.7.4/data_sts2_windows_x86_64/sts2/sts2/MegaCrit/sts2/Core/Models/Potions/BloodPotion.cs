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
	// Token: 0x02000748 RID: 1864
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BloodPotion : PotionModel
	{
		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x06005D4E RID: 23886 RVA: 0x0025795F File Offset: 0x00255B5F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x06005D4F RID: 23887 RVA: 0x00257962 File Offset: 0x00255B62
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.AnyTime;
			}
		}

		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x00257965 File Offset: 0x00255B65
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x06005D51 RID: 23889 RVA: 0x00257968 File Offset: 0x00255B68
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("HealPercent", 20m));
			}
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x00257980 File Offset: 0x00255B80
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, Colors.Red);
			}
			await CreatureCmd.Heal(target, target.MaxHp * base.DynamicVars["HealPercent"].BaseValue / 100m, true);
		}

		// Token: 0x04002532 RID: 9522
		private const string _healPercentKey = "HealPercent";
	}
}
