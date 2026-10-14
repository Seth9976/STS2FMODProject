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
	// Token: 0x02000752 RID: 1874
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DistilledChaos : PotionModel
	{
		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06005D8B RID: 23947 RVA: 0x00257DEB File Offset: 0x00255FEB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06005D8C RID: 23948 RVA: 0x00257DEE File Offset: 0x00255FEE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06005D8D RID: 23949 RVA: 0x00257DF1 File Offset: 0x00255FF1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06005D8E RID: 23950 RVA: 0x00257DF4 File Offset: 0x00255FF4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(3));
			}
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x00257E04 File Offset: 0x00256004
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("a296a3"));
			}
			await CardPileCmd.AutoPlayFromDrawPile(choiceContext, target.Player, base.DynamicVars.Repeat.IntValue, CardPilePosition.Top, false);
		}
	}
}
