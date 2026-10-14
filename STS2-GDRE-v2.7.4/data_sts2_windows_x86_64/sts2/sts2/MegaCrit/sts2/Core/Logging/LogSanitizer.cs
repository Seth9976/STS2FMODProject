using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BB5 RID: 2997
	[NullableContext(1)]
	[Nullable(0)]
	public static class LogSanitizer
	{
		/// <remarks>
		/// Pattern:<br />
		/// <code>\\b76561\\d{12}\\b</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match if at a word boundary.<br />
		/// ○ Match the string "76561".<br />
		/// ○ Match a Unicode digit exactly 12 times.<br />
		/// ○ Match if at a word boundary.<br />
		/// </code>
		/// </remarks>
		// Token: 0x06007CEF RID: 31983 RVA: 0x002A8FC0 File Offset: 0x002A71C0
		[GeneratedRegex("\\b76561\\d{12}\\b")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex SteamIdRegex()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.Instance;
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x002A8FC8 File Offset: 0x002A71C8
		public static string Sanitize(string text)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			if (!string.IsNullOrEmpty(folderPath))
			{
				text = text.Replace(folderPath, LogSanitizer._homeReplacement);
				string text2 = folderPath.Replace('\\', '/');
				if (text2 != folderPath)
				{
					text = text.Replace(text2, LogSanitizer._homeReplacement);
				}
			}
			Regex regex = LogSanitizer.SteamIdRegex();
			string text3 = text;
			MatchEvaluator matchEvaluator;
			if ((matchEvaluator = LogSanitizer.<>O.<0>__ReplaceSteamId) == null)
			{
				matchEvaluator = (LogSanitizer.<>O.<0>__ReplaceSteamId = new MatchEvaluator(LogSanitizer.ReplaceSteamId));
			}
			text = regex.Replace(text3, matchEvaluator);
			return text;
		}

		// Token: 0x06007CF1 RID: 31985 RVA: 0x002A9040 File Offset: 0x002A7240
		public static string ReplaceSteamId(Match m)
		{
			ulong num = ulong.Parse(m.Value);
			string text = IdAnonymizer.Anonymize(num).ToString();
			return "A" + text;
		}

		// Token: 0x040029B2 RID: 10674
		private static readonly string _homeReplacement = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "%USERPROFILE%" : "~");

		// Token: 0x020022EC RID: 8940
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040092B9 RID: 37561
			[Nullable(0)]
			public static MatchEvaluator <0>__ReplaceSteamId;
		}
	}
}
