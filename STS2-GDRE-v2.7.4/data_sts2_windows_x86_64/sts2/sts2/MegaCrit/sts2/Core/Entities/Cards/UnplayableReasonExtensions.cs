using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CCC RID: 3276
	internal static class UnplayableReasonExtensions
	{
		// Token: 0x06008506 RID: 34054 RVA: 0x002CB53E File Offset: 0x002C973E
		public static bool HasResourceCostReason(this UnplayableReason reason)
		{
			return reason.HasFlag(UnplayableReason.EnergyCostTooHigh) || reason.HasFlag(UnplayableReason.StarCostTooHigh);
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x002CB568 File Offset: 0x002C9768
		[NullableContext(2)]
		public static LocString GetPlayerDialogueLine(this UnplayableReason reason, AbstractModel preventer = null)
		{
			if (reason.HasFlag(UnplayableReason.NoLivingAllies))
			{
				return new LocString("combat_messages", "NO_LIVING_ALLIES");
			}
			if (reason.HasFlag(UnplayableReason.EnergyCostTooHigh))
			{
				return new LocString("combat_messages", "NOT_ENOUGH_ENERGY");
			}
			if (reason.HasFlag(UnplayableReason.StarCostTooHigh))
			{
				return new LocString("combat_messages", "NOT_ENOUGH_STARS");
			}
			if (reason.HasFlag(UnplayableReason.BlockedByHook))
			{
				if (preventer == null)
				{
					return new LocString("combat_messages", "UNPLAYABLE");
				}
				LocString locString = new LocString("combat_messages", "BLOCKED_BY_HOOK");
				CardModel cardModel = preventer as CardModel;
				string text;
				if (cardModel == null)
				{
					RelicModel relicModel = preventer as RelicModel;
					if (relicModel == null)
					{
						PowerModel powerModel = preventer as PowerModel;
						if (powerModel == null)
						{
							EnchantmentModel enchantmentModel = preventer as EnchantmentModel;
							if (enchantmentModel == null)
							{
								AfflictionModel afflictionModel = preventer as AfflictionModel;
								if (afflictionModel == null)
								{
									text = null;
								}
								else
								{
									text = afflictionModel.Title.GetFormattedText();
								}
							}
							else
							{
								text = enchantmentModel.Title.GetFormattedText();
							}
						}
						else
						{
							text = powerModel.Title.GetFormattedText();
						}
					}
					else
					{
						text = relicModel.Title.GetFormattedText();
					}
				}
				else
				{
					text = cardModel.Title;
				}
				string text2 = text;
				if (text2 == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Missing case for model ");
					defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(preventer);
					defaultInterpolatedStringHandler.AppendLiteral(" in UnplayableReason.GetPlayerDialogueLine switch!");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					text2 = "<UNKNOWN>";
				}
				locString.Add("BlockingHook", text2);
				return locString;
			}
			else
			{
				if (reason.HasFlag(UnplayableReason.HasUnplayableKeyword) || reason.HasFlag(UnplayableReason.BlockedByCardLogic))
				{
					return new LocString("combat_messages", "UNPLAYABLE");
				}
				return null;
			}
		}
	}
}
