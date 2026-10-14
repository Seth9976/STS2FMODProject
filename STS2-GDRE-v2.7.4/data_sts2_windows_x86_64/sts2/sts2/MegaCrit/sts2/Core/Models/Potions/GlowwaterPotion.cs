using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000764 RID: 1892
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GlowwaterPotion : PotionModel
	{
		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06005DFE RID: 24062 RVA: 0x00258837 File Offset: 0x00256A37
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Event;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06005DFF RID: 24063 RVA: 0x0025883A File Offset: 0x00256A3A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06005E00 RID: 24064 RVA: 0x0025883D File Offset: 0x00256A3D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06005E01 RID: 24065 RVA: 0x00258840 File Offset: 0x00256A40
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06005E02 RID: 24066 RVA: 0x0025884D File Offset: 0x00256A4D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(10));
			}
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x0025885C File Offset: 0x00256A5C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player targetPlayer = target.Player;
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("83ebdf"));
			}
			List<CardModel> list = PileType.Hand.GetPile(targetPlayer).Cards.ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, targetPlayer, false);
		}
	}
}
