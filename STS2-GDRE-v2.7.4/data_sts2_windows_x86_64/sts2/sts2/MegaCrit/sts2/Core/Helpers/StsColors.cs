using System;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	/// <summary>
	/// Color reference class for this specific game, Slay the Spire 2.
	/// If a color is a one-off, it probably doesn't need to be live here.
	/// </summary>
	// Token: 0x02000C17 RID: 3095
	public static class StsColors
	{
		// Token: 0x04002A9C RID: 10908
		public static readonly Color aqua = new Color("2AEBBE");

		// Token: 0x04002A9D RID: 10909
		public static readonly Color cream = new Color("FFF6E2");

		// Token: 0x04002A9E RID: 10910
		public static readonly Color halfTransparentCream = new Color("FFF6E280");

		// Token: 0x04002A9F RID: 10911
		public static readonly Color gold = new Color("EFC851");

		// Token: 0x04002AA0 RID: 10912
		public static readonly Color red = new Color("FF5555");

		// Token: 0x04002AA1 RID: 10913
		public static readonly Color green = new Color("7FFF00");

		// Token: 0x04002AA2 RID: 10914
		public static readonly Color blue = new Color("87CEEB");

		// Token: 0x04002AA3 RID: 10915
		public static readonly Color darkBlue = new Color("67AEEB");

		// Token: 0x04002AA4 RID: 10916
		public static readonly Color orange = new Color("FFA518");

		// Token: 0x04002AA5 RID: 10917
		public static readonly Color pink = new Color("FF78A0");

		// Token: 0x04002AA6 RID: 10918
		public static readonly Color purple = new Color("EE82EE");

		// Token: 0x04002AA7 RID: 10919
		public static readonly Color blueGlow = new Color("78FFFEC0");

		// Token: 0x04002AA8 RID: 10920
		public static readonly Color redGlow = new Color("FF0000");

		// Token: 0x04002AA9 RID: 10921
		public static readonly Color disabledTextForPotionPopup = new Color("5E5E5E");

		// Token: 0x04002AAA RID: 10922
		public static readonly Color screenBackdrop = new Color(0f, 0f, 0f, 0.8f);

		// Token: 0x04002AAB RID: 10923
		public static readonly Color transparentBlack = new Color("00000000");

		// Token: 0x04002AAC RID: 10924
		public static readonly Color halfTransparentBlack = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04002AAD RID: 10925
		public static readonly Color ninetyPercentBlack = new Color(0f, 0f, 0f, 0.9f);

		// Token: 0x04002AAE RID: 10926
		public static readonly Color transparentWhite = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04002AAF RID: 10927
		public static readonly Color halfTransparentWhite = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04002AB0 RID: 10928
		public static readonly Color disabledTopBarButton = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x04002AB1 RID: 10929
		public static readonly Color quarterTransparentBlack = new Color(0f, 0f, 0f, 0.25f);

		// Token: 0x04002AB2 RID: 10930
		public static readonly Color quarterTransparentWhite = new Color(1f, 1f, 1f, 0.25f);

		// Token: 0x04002AB3 RID: 10931
		public static readonly Color exhaustGray = new Color(0.1f, 0.1f, 0.1f, 0f);

		// Token: 0x04002AB4 RID: 10932
		public static readonly Color lightGray = new Color(0.75f, 0.75f, 0.75f, 1f);

		// Token: 0x04002AB5 RID: 10933
		public static readonly Color gray = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04002AB6 RID: 10934
		public static readonly Color placeholderGrayTabButton = new Color(0.75f, 0.75f, 0.75f, 1f);

		// Token: 0x04002AB7 RID: 10935
		public static readonly Color settingTabsButtonOutline = new Color("B1F8FF");

		// Token: 0x04002AB8 RID: 10936
		public static readonly Color settingTabsButtonOutlineFiftyPercent = new Color("B1F8FF80");

		// Token: 0x04002AB9 RID: 10937
		public static readonly Color bossNodeUntraveled = new Color("7D6A55D8");

		// Token: 0x04002ABA RID: 10938
		public static readonly Color pathDotTraveled = new Color("241F1A");

		// Token: 0x04002ABB RID: 10939
		public static readonly Color legendText = new Color("2B3152");

		// Token: 0x04002ABC RID: 10940
		public static readonly Color defaultEnergyCostOutline = new Color("4D4A43DC");

		// Token: 0x04002ABD RID: 10941
		public static readonly Color unplayableEnergyCostOutline = new Color("501717");

		// Token: 0x04002ABE RID: 10942
		public static readonly Color defaultStarCostOutline = new Color("175561DC");

		// Token: 0x04002ABF RID: 10943
		public static readonly Color cardTitleOutlineCommon = new Color("4D4B40FF");

		// Token: 0x04002AC0 RID: 10944
		public static readonly Color cardTitleOutlineUncommon = new Color("005C75FF");

		// Token: 0x04002AC1 RID: 10945
		public static readonly Color cardTitleOutlineRare = new Color("6B4B00FF");

		// Token: 0x04002AC2 RID: 10946
		public static readonly Color cardTitleOutlineCurse = new Color("550B9EFF");

		// Token: 0x04002AC3 RID: 10947
		public static readonly Color cardTitleOutlineQuest = new Color("7E3E15FF");

		// Token: 0x04002AC4 RID: 10948
		public static readonly Color cardTitleOutlineStatus = new Color("4F522FFF");

		// Token: 0x04002AC5 RID: 10949
		public static readonly Color cardTitleOutlineSpecial = new Color("1B6131FF");

		// Token: 0x04002AC6 RID: 10950
		public static readonly Color energyBlue = new Color("40FFFF");

		// Token: 0x04002AC7 RID: 10951
		public static readonly Color energyBlueOutline = new Color("20595C");

		// Token: 0x04002AC8 RID: 10952
		public static readonly Color energyGreenOutline = new Color("1F5923");

		// Token: 0x04002AC9 RID: 10953
		public static readonly Color rewardLabelOutline = new Color("225157FF");

		// Token: 0x04002ACA RID: 10954
		public static readonly Color rewardLabelGoldOutline = new Color("4B4511FF");

		// Token: 0x04002ACB RID: 10955
		public static readonly Color targetingArrowEnemy = new Color("E61E1B");

		// Token: 0x04002ACC RID: 10956
		public static readonly Color targetingArrowAlly = new Color("36c78a");

		// Token: 0x04002ACD RID: 10957
		public static readonly Color hpBarBackground = new Color("00113480");

		// Token: 0x04002ACE RID: 10958
		public static readonly Color merchantBlue = new Color("516ACF");
	}
}
