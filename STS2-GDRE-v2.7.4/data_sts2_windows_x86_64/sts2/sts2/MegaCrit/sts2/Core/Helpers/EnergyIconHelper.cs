using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C06 RID: 3078
	[NullableContext(1)]
	[Nullable(0)]
	public static class EnergyIconHelper
	{
		/// <summary>
		/// Get the string that should be used as the prefix for a model's energy icon path.
		///
		/// The logic here is a little more complicated than you'd expect; it's easy if it's a card in the player's deck
		/// during a run (just use the owner's card pool), but it gets trickier before the card is in the deck (like in
		/// card rewards) or outside of a run entirely (like in the card library).
		/// </summary>
		/// <param name="model">Model whose energy icon we want.</param>
		// Token: 0x06007FA1 RID: 32673 RVA: 0x002B5137 File Offset: 0x002B3337
		public static string GetPrefix(AbstractModel model)
		{
			return EnergyIconHelper.GetPool(model).EnergyColorName;
		}

		/// <summary>
		/// Get the string that should be used as the specified model's energy icon path.
		///
		/// The logic here is a little more complicated than you'd expect; it's easy if it's a card in the player's deck
		/// during a run (just use the owner's card pool), but it gets trickier before the card is in the deck (like in
		/// card rewards) or outside of a run entirely (like in the card library).
		/// </summary>
		/// <param name="model">Model whose energy icon we want.</param>
		// Token: 0x06007FA2 RID: 32674 RVA: 0x002B5144 File Offset: 0x002B3344
		public static string GetPath(AbstractModel model)
		{
			return EnergyIconHelper.GetPath(EnergyIconHelper.GetPrefix(model));
		}

		/// <summary>
		/// Get the string that should be used as the energy icon path for the specified prefix.
		/// </summary>
		// Token: 0x06007FA3 RID: 32675 RVA: 0x002B5151 File Offset: 0x002B3351
		public static string GetPath(string prefix)
		{
			return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/energy_" + prefix.ToLowerInvariant() + ".tres");
		}

		// Token: 0x06007FA4 RID: 32676 RVA: 0x002B5170 File Offset: 0x002B3370
		private static IPoolModel GetPool(AbstractModel model)
		{
			IPoolModel poolModel = model as IPoolModel;
			if (poolModel != null)
			{
				return poolModel;
			}
			Player player = null;
			IPoolModel poolModel2 = null;
			CardModel cardModel = model as CardModel;
			if (cardModel == null)
			{
				EnchantmentModel enchantmentModel = model as EnchantmentModel;
				if (enchantmentModel == null)
				{
					PotionModel potionModel = model as PotionModel;
					if (potionModel == null)
					{
						RelicModel relicModel = model as RelicModel;
						if (relicModel == null)
						{
							PowerModel powerModel = model as PowerModel;
							if (powerModel != null)
							{
								if (powerModel != null && powerModel.IsMutable)
								{
									Creature owner = powerModel.Owner;
									if (owner != null && owner.IsPlayer)
									{
										player = powerModel.Owner.Player;
										goto IL_0108;
									}
								}
								poolModel2 = ModelDb.CardPool<ColorlessCardPool>();
							}
						}
						else
						{
							if (relicModel.IsMutable)
							{
								player = relicModel.Owner;
							}
							poolModel2 = relicModel.Pool;
						}
					}
					else
					{
						if (potionModel.IsMutable)
						{
							player = potionModel.Owner;
						}
						poolModel2 = potionModel.Pool;
					}
				}
				else if (enchantmentModel.HasCard)
				{
					player = enchantmentModel.Card.Owner;
					poolModel2 = enchantmentModel.Card.Pool;
				}
				else
				{
					poolModel2 = ModelDb.CardPool<ColorlessCardPool>();
				}
			}
			else
			{
				if (cardModel.IsMutable)
				{
					player = cardModel.Owner;
				}
				poolModel2 = cardModel.Pool;
			}
			IL_0108:
			if (player != null)
			{
				return player.Character.CardPool;
			}
			if (poolModel2 != null)
			{
				return poolModel2;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Model ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(model.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" is not in any pool! It was probably deprecated without being removed.");
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return ModelDb.CardPool<IroncladCardPool>();
		}
	}
}
