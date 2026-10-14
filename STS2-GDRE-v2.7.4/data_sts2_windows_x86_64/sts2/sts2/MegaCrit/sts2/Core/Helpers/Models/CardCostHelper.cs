using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Helpers.Models
{
	// Token: 0x02000C1A RID: 3098
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardCostHelper
	{
		/// <summary>
		/// Get the color that should be used for the text in a card's energy cost.
		/// Depends on a whole bunch of tricky rules, see comments and tests for details.
		/// WARNING: If you make a change to this method, you should probably make a similar change to
		/// <see cref="M:MegaCrit.Sts2.Core.Helpers.Models.CardCostHelper.GetStarCostColor(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Combat.ICombatState)" />, or write a comment explaining why the two methods are different.
		/// </summary>
		/// <param name="card">Card whose energy cost color we want.</param>
		/// <param name="state">Combat state that the color depends on. Null outside of combat (like in the Card Library).</param>
		/// <returns>Energy cost color for the specified card in the specified combat state.</returns>
		// Token: 0x06007FFA RID: 32762 RVA: 0x002B7498 File Offset: 0x002B5698
		public static CardCostColor GetEnergyCostColor(CardModel card, [Nullable(2)] ICombatState state)
		{
			if (state == null)
			{
				return CardCostColor.Unmodified;
			}
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (!card.CanPlay(out unplayableReason, out abstractModel) && unplayableReason.HasFlag(UnplayableReason.EnergyCostTooHigh))
			{
				return CardCostColor.InsufficientResources;
			}
			if (card.EnergyCost.CostsX)
			{
				return CardCostColor.Unmodified;
			}
			decimal num;
			if (CardCostHelper.TryModifyEnergyCostWithHooks(card, state, out num))
			{
				return CardCostHelper.GetColorForHookModifiedCost(num, card.EnergyCost.GetWithModifiers(CostModifiers.None));
			}
			if (card.EnergyCost.HasLocalModifiers)
			{
				return CardCostHelper.GetColorForLocalCost(card.EnergyCost.GetWithModifiers(CostModifiers.Local), card.EnergyCost.GetWithModifiers(CostModifiers.None));
			}
			return CardCostColor.Unmodified;
		}

		/// <summary>
		/// Get the color that should be used for the text in a card's star cost.
		/// Depends on a whole bunch of tricky rules, see comments and tests for details.
		/// WARNING: If you make a change to this method, you should probably make a similar change to
		/// <see cref="M:MegaCrit.Sts2.Core.Helpers.Models.CardCostHelper.GetEnergyCostColor(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Combat.ICombatState)" />, or write a comment explaining why the two methods are different.
		/// </summary>
		/// <param name="card">Card whose star cost color we want.</param>
		/// <param name="state">Combat state that the color depends on. Null outside of combat (like in the Card Library).</param>
		/// <returns>Star cost color for the specified card in the specified combat state.</returns>
		// Token: 0x06007FFB RID: 32763 RVA: 0x002B7528 File Offset: 0x002B5728
		public static CardCostColor GetStarCostColor(CardModel card, [Nullable(2)] ICombatState state)
		{
			if (state == null)
			{
				return CardCostColor.Unmodified;
			}
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (!card.CanPlay(out unplayableReason, out abstractModel) && unplayableReason.HasFlag(UnplayableReason.StarCostTooHigh))
			{
				return CardCostColor.InsufficientResources;
			}
			if (card.HasStarCostX)
			{
				return CardCostColor.Unmodified;
			}
			decimal num;
			if (CardCostHelper.TryModifyStarCostWithHooks(card, state, out num))
			{
				return CardCostHelper.GetColorForHookModifiedCost(num, card.BaseStarCost);
			}
			if (card.TemporaryStarCost != null)
			{
				return CardCostHelper.GetColorForLocalCost(card.TemporaryStarCost.Cost, card.BaseStarCost);
			}
			return CardCostColor.Unmodified;
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x002B759E File Offset: 0x002B579E
		private static CardCostColor GetColorForLocalCost(int localCost, int baseCost)
		{
			if (localCost > baseCost)
			{
				return CardCostColor.Increased;
			}
			if (localCost < baseCost)
			{
				return CardCostColor.Decreased;
			}
			return CardCostColor.Unmodified;
		}

		// Token: 0x06007FFD RID: 32765 RVA: 0x002B75AD File Offset: 0x002B57AD
		private static CardCostColor GetColorForHookModifiedCost(decimal hookModifiedCost, int baseCost)
		{
			if (hookModifiedCost > baseCost)
			{
				return CardCostColor.Increased;
			}
			if (hookModifiedCost < baseCost)
			{
				return CardCostColor.Decreased;
			}
			return CardCostColor.Unmodified;
		}

		// Token: 0x06007FFE RID: 32766 RVA: 0x002B75D0 File Offset: 0x002B57D0
		private static bool TryModifyEnergyCostWithHooks(CardModel card, ICombatState state, out decimal hookModifiedCost)
		{
			hookModifiedCost = card.EnergyCost.GetWithModifiers(CostModifiers.None);
			bool flag = false;
			foreach (AbstractModel abstractModel in state.IterateHookListeners())
			{
				flag |= abstractModel.TryModifyEnergyCostInCombat(card, hookModifiedCost, out hookModifiedCost);
			}
			foreach (AbstractModel abstractModel2 in state.IterateHookListeners())
			{
				flag |= abstractModel2.TryModifyEnergyCostInCombatLate(card, hookModifiedCost, out hookModifiedCost);
			}
			return flag;
		}

		// Token: 0x06007FFF RID: 32767 RVA: 0x002B768C File Offset: 0x002B588C
		private static bool TryModifyStarCostWithHooks(CardModel card, ICombatState state, out decimal hookModifiedCost)
		{
			hookModifiedCost = card.BaseStarCost;
			bool flag = false;
			foreach (AbstractModel abstractModel in state.IterateHookListeners())
			{
				flag |= abstractModel.TryModifyStarCost(card, hookModifiedCost, out hookModifiedCost);
			}
			return flag;
		}
	}
}
