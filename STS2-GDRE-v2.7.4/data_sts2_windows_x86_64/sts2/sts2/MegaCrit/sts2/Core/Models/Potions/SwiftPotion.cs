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
	// Token: 0x0200077F RID: 1919
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwiftPotion : PotionModel
	{
		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x0025971B File Offset: 0x0025791B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x0025971E File Offset: 0x0025791E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x00259721 File Offset: 0x00257921
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x00259724 File Offset: 0x00257924
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x00259734 File Offset: 0x00257934
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("45e6d0"));
			}
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target.Player, false);
		}
	}
}
