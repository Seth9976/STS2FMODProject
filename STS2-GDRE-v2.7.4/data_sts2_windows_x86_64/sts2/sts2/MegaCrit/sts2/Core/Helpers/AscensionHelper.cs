using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C01 RID: 3073
	[NullableContext(1)]
	[Nullable(0)]
	public static class AscensionHelper
	{
		/// <summary>
		/// Return a different value depending on whether the given ascension is higher or lower than the current
		/// ascension of the run.
		/// </summary>
		/// <param name="level">The value that we use for ascensions below the ascensionThreshold</param>
		/// <param name="ascensionValue">The value that we use for ascensions above the ascensionThreshold</param>
		/// <param name="fallbackValue">The value that we use for ascensions below the ascensionThreshold</param>
		/// <returns></returns>
		// Token: 0x06007F72 RID: 32626 RVA: 0x002B2D44 File Offset: 0x002B0F44
		public static int GetValueIfAscension(AscensionLevel level, int ascensionValue, int fallbackValue)
		{
			if (!RunManager.Instance.HasAscension(level))
			{
				return fallbackValue;
			}
			return ascensionValue;
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x002B2D56 File Offset: 0x002B0F56
		public static float GetValueIfAscension(AscensionLevel level, float ascensionValue, float fallbackValue)
		{
			if (!RunManager.Instance.HasAscension(level))
			{
				return fallbackValue;
			}
			return ascensionValue;
		}

		// Token: 0x06007F74 RID: 32628 RVA: 0x002B2D68 File Offset: 0x002B0F68
		public static decimal GetValueIfAscension(AscensionLevel level, decimal ascensionValue, decimal fallbackValue)
		{
			if (!RunManager.Instance.HasAscension(level))
			{
				return fallbackValue;
			}
			return ascensionValue;
		}

		// Token: 0x06007F75 RID: 32629 RVA: 0x002B2D7A File Offset: 0x002B0F7A
		public static bool HasAscension(AscensionLevel level)
		{
			return RunManager.Instance.HasAscension(level);
		}

		// Token: 0x06007F76 RID: 32630 RVA: 0x002B2D87 File Offset: 0x002B0F87
		public static LocString GetTitle(int level)
		{
			return new LocString("ascension", "LEVEL_" + AscensionHelper.GetKey(level) + ".title");
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x002B2DA8 File Offset: 0x002B0FA8
		public static LocString GetDescription(int level)
		{
			return new LocString("ascension", "LEVEL_" + AscensionHelper.GetKey(level) + ".description");
		}

		// Token: 0x06007F78 RID: 32632 RVA: 0x002B2DC9 File Offset: 0x002B0FC9
		private static string GetKey(int level)
		{
			return level.ToString("D2");
		}

		// Token: 0x06007F79 RID: 32633 RVA: 0x002B2DD8 File Offset: 0x002B0FD8
		public static HoverTip GetHoverTip(CharacterModel character, int level, bool achievementsLocked)
		{
			LocString locString;
			if (level > 0)
			{
				locString = new LocString("ascension", "PORTRAIT_TITLE");
				locString.Add("character", character.Title);
				locString.Add("ascension", level);
			}
			else
			{
				locString = new LocString("ascension", "PORTRAIT_TITLE_NO_ASCENSION");
				locString.Add("character", character.Title);
			}
			LocString locString2 = new LocString("ascension", "PORTRAIT_DESCRIPTION");
			List<string> list = new List<string>();
			for (int i = 1; i <= level; i++)
			{
				list.Add(AscensionHelper.GetTitle(i).GetFormattedText());
			}
			locString2.Add("ascensions", list);
			if (!achievementsLocked)
			{
				return new HoverTip(locString, locString2, null);
			}
			if (level == 0)
			{
				LocString locString3 = new LocString("gameplay_ui", "ACHIEVEMENTS_LOCKED");
				return new HoverTip(locString, locString2.GetFormattedText() + locString3.GetFormattedText(), null);
			}
			LocString locString4 = new LocString("gameplay_ui", "ACHIEVEMENTS_LOCKED");
			return new HoverTip(locString, locString2.GetFormattedText() + "\n" + locString4.GetFormattedText(), null);
		}

		// Token: 0x17002304 RID: 8964
		// (get) Token: 0x06007F7A RID: 32634 RVA: 0x002B2EE6 File Offset: 0x002B10E6
		public static double PovertyAscensionGoldMultiplier
		{
			get
			{
				return 0.75;
			}
		}
	}
}
