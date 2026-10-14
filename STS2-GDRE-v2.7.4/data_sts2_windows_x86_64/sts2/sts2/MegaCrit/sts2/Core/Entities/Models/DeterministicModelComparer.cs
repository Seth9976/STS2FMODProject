using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Models
{
	/// <summary>
	/// An IComparer which sorts a collection of models into the same order.
	/// The HookBus keeps a collection of models. In multiplayer, these models may be added or removed in different orders
	/// based on the peer. However, we need these models to execute their hooks in the exact same order across all peers
	/// to maintain determinism.
	/// </summary>
	// Token: 0x02000C92 RID: 3218
	[NullableContext(1)]
	[Nullable(0)]
	public class DeterministicModelComparer : IComparer<AbstractModel>
	{
		// Token: 0x170023EC RID: 9196
		// (get) Token: 0x06008390 RID: 33680 RVA: 0x002C710D File Offset: 0x002C530D
		public static DeterministicModelComparer Instance { get; } = new DeterministicModelComparer();

		// Token: 0x06008391 RID: 33681 RVA: 0x002C7114 File Offset: 0x002C5314
		private DeterministicModelComparer()
		{
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x002C711C File Offset: 0x002C531C
		[NullableContext(2)]
		public int Compare(AbstractModel model1, AbstractModel model2)
		{
			if (model1 == null && model2 == null)
			{
				return 0;
			}
			if (model1 == model2)
			{
				return 0;
			}
			if (model1 == null)
			{
				return -1;
			}
			if (model2 == null)
			{
				return 1;
			}
			if (model1.CategorySortingId != model2.CategorySortingId)
			{
				return model1.CategorySortingId.CompareTo(model2.CategorySortingId);
			}
			if (model1.EntrySortingId != model2.EntrySortingId)
			{
				return model1.EntrySortingId.CompareTo(model2.EntrySortingId);
			}
			Creature owner = DeterministicModelComparer.GetOwner(model1);
			Creature owner2 = DeterministicModelComparer.GetOwner(model2);
			if (owner == null && owner2 == null)
			{
				return 0;
			}
			if (owner == null)
			{
				return -1;
			}
			if (owner2 == null)
			{
				return 1;
			}
			if (owner == owner2)
			{
				return DeterministicModelComparer.CompareModelsWithSameOwnerAndId(model1, model2);
			}
			if (owner.IsPlayer && owner2.IsPlayer)
			{
				return owner.Player.NetId.CompareTo(owner2.Player.NetId);
			}
			if (owner.CombatId != null && owner2.CombatId != null)
			{
				return owner.CombatId.Value.CompareTo(owner2.CombatId.Value);
			}
			if (owner.CombatId != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x002C7244 File Offset: 0x002C5444
		[return: Nullable(2)]
		private static Creature GetOwner(AbstractModel model)
		{
			CardModel cardModel = model as CardModel;
			Creature creature;
			if (cardModel == null)
			{
				RelicModel relicModel = model as RelicModel;
				if (relicModel == null)
				{
					PotionModel potionModel = model as PotionModel;
					if (potionModel == null)
					{
						PowerModel powerModel = model as PowerModel;
						if (powerModel == null)
						{
							AfflictionModel afflictionModel = model as AfflictionModel;
							if (afflictionModel == null)
							{
								EnchantmentModel enchantmentModel = model as EnchantmentModel;
								if (enchantmentModel == null)
								{
									creature = null;
								}
								else
								{
									creature = enchantmentModel.Card.Owner.Creature;
								}
							}
							else
							{
								creature = afflictionModel.Card.Owner.Creature;
							}
						}
						else
						{
							creature = powerModel.Owner;
						}
					}
					else
					{
						creature = potionModel.Owner.Creature;
					}
				}
				else
				{
					creature = relicModel.Owner.Creature;
				}
			}
			else
			{
				creature = cardModel.Owner.Creature;
			}
			return creature;
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x002C72FC File Offset: 0x002C54FC
		private static int CompareModelsWithSameOwnerAndId(AbstractModel model1, AbstractModel model2)
		{
			CardModel cardModel = model1 as CardModel;
			if (cardModel != null)
			{
				CardModel cardModel2 = model2 as CardModel;
				if (cardModel2 != null)
				{
					return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(cardModel, cardModel2);
				}
			}
			AfflictionModel afflictionModel = model1 as AfflictionModel;
			if (afflictionModel != null)
			{
				AfflictionModel afflictionModel2 = model2 as AfflictionModel;
				if (afflictionModel2 != null)
				{
					return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(afflictionModel.Card, afflictionModel2.Card);
				}
			}
			EnchantmentModel enchantmentModel = model1 as EnchantmentModel;
			if (enchantmentModel != null)
			{
				EnchantmentModel enchantmentModel2 = model2 as EnchantmentModel;
				if (enchantmentModel2 != null)
				{
					return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(enchantmentModel.Card, enchantmentModel2.Card);
				}
			}
			RelicModel relicModel = model1 as RelicModel;
			if (relicModel != null)
			{
				RelicModel relicModel2 = model2 as RelicModel;
				if (relicModel2 != null)
				{
					return relicModel.Owner.Relics.IndexOf(relicModel).CompareTo(relicModel2.Owner.Relics.IndexOf(relicModel2));
				}
			}
			PotionModel potionModel = model1 as PotionModel;
			if (potionModel != null)
			{
				PotionModel potionModel2 = model2 as PotionModel;
				if (potionModel2 != null)
				{
					return potionModel.Owner.PotionSlots.IndexOf(potionModel).CompareTo(potionModel2.Owner.PotionSlots.IndexOf(potionModel2));
				}
			}
			PowerModel powerModel = model1 as PowerModel;
			if (powerModel != null)
			{
				PowerModel powerModel2 = model2 as PowerModel;
				if (powerModel2 != null)
				{
					return powerModel.Owner.Powers.IndexOf(powerModel).CompareTo(powerModel2.Owner.Powers.IndexOf(powerModel2));
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(87, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to compare equivalent models ");
			defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model1);
			defaultInterpolatedStringHandler.AppendLiteral(" and ");
			defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model2);
			defaultInterpolatedStringHandler.AppendLiteral(" but we can't map them to any know model types!");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06008395 RID: 33685 RVA: 0x002C749C File Offset: 0x002C569C
		private static int CompareCardModelsWithSameOwnerAndId(CardModel cardModel1, CardModel cardModel2)
		{
			if (cardModel1.Pile == null && cardModel2.Pile == null)
			{
				return 0;
			}
			if (cardModel1.Pile == null)
			{
				return 1;
			}
			if (cardModel2.Pile == null)
			{
				return -1;
			}
			if (cardModel1.Pile.Type != cardModel2.Pile.Type)
			{
				return cardModel1.Pile.Type.CompareTo(cardModel2.Pile.Type);
			}
			return cardModel1.Pile.Cards.IndexOf(cardModel1).CompareTo(cardModel2.Pile.Cards.IndexOf(cardModel2));
		}
	}
}
