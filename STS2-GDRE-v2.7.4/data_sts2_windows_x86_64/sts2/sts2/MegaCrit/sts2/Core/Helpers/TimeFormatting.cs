using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C19 RID: 3097
	public static class TimeFormatting
	{
		// Token: 0x06007FF9 RID: 32761 RVA: 0x002B73FC File Offset: 0x002B55FC
		[NullableContext(1)]
		public static string Format(float time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			int num = (int)timeSpan.TotalHours;
			LocString locString;
			if (num > 0)
			{
				locString = new LocString("main_menu_ui", "RUN_TIME_FORMAT_HOURS");
				locString.Add("Hours", num.ToString());
			}
			else
			{
				locString = new LocString("main_menu_ui", "RUN_TIME_FORMAT");
			}
			locString.Add("Seconds", timeSpan.Seconds.ToString("00"));
			locString.Add("Minutes", timeSpan.Minutes.ToString("00"));
			return locString.GetFormattedText();
		}
	}
}
