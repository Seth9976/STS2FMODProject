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
	// Token: 0x0200074A RID: 1866
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BottledPotential : PotionModel
	{
		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x06005D5B RID: 23899 RVA: 0x00257A6F File Offset: 0x00255C6F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06005D5C RID: 23900 RVA: 0x00257A72 File Offset: 0x00255C72
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06005D5D RID: 23901 RVA: 0x00257A75 File Offset: 0x00255C75
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x06005D5E RID: 23902 RVA: 0x00257A78 File Offset: 0x00255C78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(5));
			}
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x00257A88 File Offset: 0x00255C88
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("e645db"));
			}
			await CardPileCmd.Add(PileType.Hand.GetPile(target.Player).Cards, PileType.Draw, CardPilePosition.Bottom, null, false);
			await CardPileCmd.Shuffle(choiceContext, target.Player);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target.Player, false);
		}
	}
}
