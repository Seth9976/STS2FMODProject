using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000761 RID: 1889
	public sealed class GamblersBrew : PotionModel
	{
		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06005DEC RID: 24044 RVA: 0x002586C3 File Offset: 0x002568C3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06005DED RID: 24045 RVA: 0x002586C6 File Offset: 0x002568C6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06005DEE RID: 24046 RVA: 0x002586C9 File Offset: 0x002568C9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x002586CC File Offset: 0x002568CC
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("a19f91"));
			}
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, target.Player, new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 999999999), null, this);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardCmd.DiscardAndDraw(choiceContext, list, list.Count);
		}
	}
}
