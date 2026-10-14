using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000575 RID: 1397
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LavaLamp : RelicModel
	{
		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x002421CD File Offset: 0x002403CD
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005206 RID: 20998 RVA: 0x002421D0 File Offset: 0x002403D0
		// (set) Token: 0x06005207 RID: 20999 RVA: 0x002421D8 File Offset: 0x002403D8
		[SavedProperty]
		public bool TookDamageThisCombat
		{
			get
			{
				return this._tookDamageThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._tookDamageThisCombat = value;
			}
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x002421E7 File Offset: 0x002403E7
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			this.TookDamageThisCombat = false;
			return Task.CompletedTask;
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x002421F8 File Offset: 0x002403F8
		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (!(base.Owner.RunState.CurrentRoom is CombatRoom))
			{
				return Task.CompletedTask;
			}
			if (target != base.Owner.Creature)
			{
				return Task.CompletedTask;
			}
			if (result.UnblockedDamage <= 0)
			{
				return Task.CompletedTask;
			}
			if (props.HasFlag(ValueProp.Unblockable))
			{
				return Task.CompletedTask;
			}
			this.TookDamageThisCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x0024226C File Offset: 0x0024046C
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (!(base.Owner.RunState.CurrentRoom is CombatRoom))
			{
				return false;
			}
			if (player != base.Owner)
			{
				return false;
			}
			if (this.TookDamageThisCombat)
			{
				return false;
			}
			foreach (CardCreationResult cardCreationResult in cardRewards)
			{
				CardModel card = cardCreationResult.Card;
				if (card.IsUpgradable)
				{
					CardModel cardModel = base.Owner.RunState.CloneCard(card);
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
					cardCreationResult.ModifyCard(cardModel, this);
				}
			}
			return true;
		}

		// Token: 0x0400246D RID: 9325
		private bool _tookDamageThisCombat;
	}
}
