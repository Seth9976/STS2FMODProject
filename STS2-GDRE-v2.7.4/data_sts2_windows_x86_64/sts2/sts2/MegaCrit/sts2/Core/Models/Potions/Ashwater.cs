using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000743 RID: 1859
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ashwater : PotionModel
	{
		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06005D31 RID: 23857 RVA: 0x002576CB File Offset: 0x002558CB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06005D32 RID: 23858 RVA: 0x002576CE File Offset: 0x002558CE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x06005D33 RID: 23859 RVA: 0x002576D1 File Offset: 0x002558D1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06005D34 RID: 23860 RVA: 0x002576D4 File Offset: 0x002558D4
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x002576E4 File Offset: 0x002558E4
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, this._tint);
			}
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 999999999);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, target.Player, cardSelectorPrefs, null, this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x04002530 RID: 9520
		private readonly Color _tint = new Color("83ebdf");
	}
}
