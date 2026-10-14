using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000746 RID: 1862
	public sealed class BlessingOfTheForge : PotionModel
	{
		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x06005D42 RID: 23874 RVA: 0x00257843 File Offset: 0x00255A43
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x06005D43 RID: 23875 RVA: 0x00257846 File Offset: 0x00255A46
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x06005D44 RID: 23876 RVA: 0x00257849 File Offset: 0x00255A49
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x0025784C File Offset: 0x00255A4C
		[NullableContext(1)]
		protected override Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("e06e58"));
			}
			foreach (CardModel cardModel in PileType.Hand.GetPile(target.Player).Cards)
			{
				if (cardModel.IsUpgradable)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			return Task.CompletedTask;
		}
	}
}
