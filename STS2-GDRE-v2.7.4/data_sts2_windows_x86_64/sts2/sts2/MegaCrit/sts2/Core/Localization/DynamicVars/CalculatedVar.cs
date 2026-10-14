using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	/// <summary>
	/// A special type of DynamicVar that is used for cards that include a calculation in their base behavior.
	/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" /> uses a subclass of this (<see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedDamageVar" />) for its
	/// 6 base damage + 2 extra damage for each Strike.
	/// </summary>
	// Token: 0x02000BDC RID: 3036
	[NullableContext(1)]
	[Nullable(0)]
	public class CalculatedVar : DynamicVar
	{
		// Token: 0x06007DF6 RID: 32246 RVA: 0x002AD4F1 File Offset: 0x002AB6F1
		public CalculatedVar(string name)
			: base(name, 0m)
		{
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x002AD4FF File Offset: 0x002AB6FF
		public override void SetOwner(AbstractModel owner)
		{
			base.SetOwner(owner);
			this.UpdateValues();
		}

		/// <summary>
		/// Set the function that will be used for the multiplier value of this var.
		/// This will be multiplied by the <see cref="M:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedVar.GetExtraVar" /> value.
		/// This is the "dynamic" part of the calculation, and will often be based on something in the CombatState.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" /> says:
		/// "Deal 6 damage, plus 2 additional damage for ALL your cards containing "Strike"."
		/// In this case, the extra multiplier is the number of cards containing "Strike".
		/// </example>
		/// <param name="multiplierCalc">
		/// Function that will be used to calculate the extra multiplier value.
		/// Argument 1 is the card instance that this calculation is occurring on.
		/// Argument 2 is the creature that is being targeted for this calculation (null when there is no target).
		/// </param>
		// Token: 0x06007DF8 RID: 32248 RVA: 0x002AD510 File Offset: 0x002AB710
		public CalculatedVar WithMultiplier([Nullable(new byte[] { 1, 1, 2 })] Func<CardModel, Creature, decimal> multiplierCalc)
		{
			if (this._multiplierCalc != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set extra multiplier calc on ");
				defaultInterpolatedStringHandler.AppendFormatted<CalculatedVar>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" twice!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (multiplierCalc.Target is AbstractModel)
			{
				throw new InvalidOperationException("Multiplier calc must be static!");
			}
			this._multiplierCalc = multiplierCalc;
			return this;
		}

		/// <summary>
		/// Calculate this var's value based on the specified target.
		/// Effects that target multiple creatures (or no creatures) should pass null for the target.
		/// </summary>
		// Token: 0x06007DF9 RID: 32249 RVA: 0x002AD57C File Offset: 0x002AB77C
		[NullableContext(2)]
		public decimal Calculate(Creature target)
		{
			if (this._multiplierCalc == null)
			{
				throw new InvalidOperationException("Extra multiplier calc must be specified!");
			}
			CardModel cardModel = (CardModel)this._owner;
			decimal num = ((CombatManager.Instance.IsInProgress && cardModel.CombatState != null) ? this._multiplierCalc(cardModel, target) : 0m);
			return this.GetBaseVar().BaseValue + this.GetExtraVar().BaseValue * num;
		}

		/// <summary>
		/// Re-run the calculations to determine if any of the dependent values have been upgraded or enchanted.
		/// </summary>
		// Token: 0x06007DFA RID: 32250 RVA: 0x002AD5F4 File Offset: 0x002AB7F4
		public void RecalculateForUpgradeOrEnchant()
		{
			decimal baseValue = this.GetBaseVar().BaseValue;
			if (baseValue != base.BaseValue)
			{
				base.WasJustUpgraded = true;
			}
			base.BaseValue = baseValue;
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x002AD629 File Offset: 0x002AB829
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			base.PreviewValue = this.Calculate(target);
		}

		/// <summary>
		/// Get the DynamicVar that should be used for this calculation's base value.
		/// </summary>
		// Token: 0x06007DFC RID: 32252 RVA: 0x002AD638 File Offset: 0x002AB838
		protected virtual DynamicVar GetBaseVar()
		{
			return ((CardModel)this._owner).DynamicVars.CalculationBase;
		}

		/// <summary>
		/// Get the DynamicVar that should be used for this calculation's extra value.
		/// </summary>
		// Token: 0x06007DFD RID: 32253 RVA: 0x002AD64F File Offset: 0x002AB84F
		protected virtual DynamicVar GetExtraVar()
		{
			return ((CardModel)this._owner).DynamicVars.CalculationExtra;
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x002AD666 File Offset: 0x002AB866
		protected override decimal GetBaseValueForIConvertible()
		{
			return this.Calculate(null);
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x002AD670 File Offset: 0x002AB870
		public override string ToString()
		{
			return this.Calculate(null).ToString();
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x002AD68C File Offset: 0x002AB88C
		private void UpdateValues()
		{
			if (this._owner == null)
			{
				return;
			}
			base.BaseValue = this.GetBaseVar().BaseValue;
		}

		// Token: 0x04002A25 RID: 10789
		[Nullable(new byte[] { 2, 1, 2 })]
		private Func<CardModel, Creature, decimal> _multiplierCalc;
	}
}
