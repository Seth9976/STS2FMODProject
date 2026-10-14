using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C16 RID: 3094
	[NullableContext(1)]
	[Nullable(0)]
	public static class StringHelper
	{
		/// <remarks>
		/// Pattern:<br />
		/// <code>([A-Za-z0-9]|\\G(?!^))([A-Z])</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ 1st capture group.<br />
		///     ○ Match with 2 alternative expressions.<br />
		///         ○ Match a character in the set [0-9A-Za-z].<br />
		///         ○ Match a sequence of expressions.<br />
		///             ○ Match if at the start position.<br />
		///             ○ Zero-width negative lookahead.<br />
		///                 ○ Match if at the beginning of the string.<br />
		/// ○ 2nd capture group.<br />
		///     ○ Match a character in the set [A-Z].<br />
		/// </code>
		/// </remarks>
		// Token: 0x06007FE5 RID: 32741 RVA: 0x002B6AA7 File Offset: 0x002B4CA7
		[GeneratedRegex("([A-Za-z0-9]|\\G(?!^))([A-Z])")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex CamelCaseRegex()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.Instance;
		}

		/// <remarks>
		/// Pattern:<br />
		/// <code>(.*?)_([a-zA-Z0-9])</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ 1st capture group.<br />
		///     ○ Match a character other than '\n' lazily any number of times.<br />
		/// ○ Match '_'.<br />
		/// ○ 2nd capture group.<br />
		///     ○ Match a character in the set [0-9A-Za-z].<br />
		/// </code>
		/// </remarks>
		// Token: 0x06007FE6 RID: 32742 RVA: 0x002B6AAE File Offset: 0x002B4CAE
		[GeneratedRegex("(.*?)_([a-zA-Z0-9])")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex SnakeCaseRegex()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.Instance;
		}

		/// <remarks>
		/// Pattern:<br />
		/// <code>\\s+</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match a whitespace character atomically at least once.<br />
		/// </code>
		/// </remarks>
		// Token: 0x06007FE7 RID: 32743 RVA: 0x002B6AB5 File Offset: 0x002B4CB5
		[GeneratedRegex("\\s+")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex WhitespaceRegex()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2.Instance;
		}

		/// <remarks>
		/// Pattern:<br />
		/// <code>[^A-Z0-9_]</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match a character in the set [^0-9A-Z_].<br />
		/// </code>
		/// </remarks>
		// Token: 0x06007FE8 RID: 32744 RVA: 0x002B6ABC File Offset: 0x002B4CBC
		[GeneratedRegex("[^A-Z0-9_]")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex SpecialCharRegex()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.Instance;
		}

		// Token: 0x06007FE9 RID: 32745 RVA: 0x002B6AC3 File Offset: 0x002B4CC3
		public static string SnakeCase(string txt)
		{
			return StringHelper.CamelCaseRegex().Replace(txt.Trim(), "$1_$2").ToLowerInvariant();
		}

		// Token: 0x06007FEA RID: 32746 RVA: 0x002B6AE0 File Offset: 0x002B4CE0
		public static string Slugify(string txt)
		{
			string text = StringHelper.CamelCaseRegex().Replace(txt.Trim(), "$1_$2");
			string text2 = StringHelper.WhitespaceRegex().Replace(text.ToUpperInvariant(), "_");
			return StringHelper.SpecialCharRegex().Replace(text2, "");
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x002B6B2C File Offset: 0x002B4D2C
		public static string Unslugify(string txt)
		{
			string text = StringHelper.SnakeCaseRegex().Replace(txt.Trim().ToLowerInvariant(), delegate(Match match)
			{
				string text3 = match.Groups[1].ToString();
				string text4 = match.Groups[2].ToString();
				return text3 + text4.ToUpperInvariant();
			});
			char c = char.ToUpperInvariant(text[0]);
			ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(ref c);
			string text2 = text;
			return readOnlySpan + text2.Substring(1, text2.Length - 1);
		}

		/// <summary>
		/// Trims whitespace, converts newlines to spaces, and squashes multiple whitespace characters in a row down to
		/// a single space.
		/// </summary>
		/// <remarks>
		/// Good for writing multi-line indented verbatim strings (https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim)
		/// and having them output in a natural way.
		/// </remarks>
		/// <param name="text">The string to compact.</param>
		/// <returns>A compacted copy of <paramref name="text" />.</returns>
		// Token: 0x06007FEC RID: 32748 RVA: 0x002B6B9D File Offset: 0x002B4D9D
		public static string CompactText(string text)
		{
			return text.Trim();
		}

		/// <summary>
		/// Get a deterministic hash representation of a string. Unlike GetHashCode(), it is guaranteed to be the same
		/// even across program executions.
		///
		/// This is NOT guaranteed to be unique or cryptographically safe or anything like that. Its only guarantee is
		/// to be deterministic.
		/// </summary>
		/// <param name="str">String to hash.</param>
		/// <returns>Hash value.</returns>
		// Token: 0x06007FED RID: 32749 RVA: 0x002B6BA8 File Offset: 0x002B4DA8
		public static ulong GetDeterministicHashCode(string str)
		{
			if (StringHelper._stringHashCache == null)
			{
				StringHelper._stringHashCache = new byte[1024];
			}
			int num = Encoding.UTF8.GetByteCount(str);
			if (num > StringHelper._stringHashCache.Length)
			{
				StringHelper._stringHashCache = new byte[(int)Math.Round((double)num * 1.5)];
			}
			num = Encoding.UTF8.GetBytes(str, StringHelper._stringHashCache);
			return XxHash64.HashToUInt64(StringHelper._stringHashCache.AsSpan<byte>().Slice(0, num), 0L);
		}

		/// <summary>
		/// This should not be used for any new code. It is for backwards compatibility only.
		/// </summary>
		// Token: 0x06007FEE RID: 32750 RVA: 0x002B6C38 File Offset: 0x002B4E38
		public static int GetDeterministicHashCodeOld(string str)
		{
			int num = 352654597;
			int num2 = num;
			for (int i = 0; i < str.Length; i += 2)
			{
				num = ((num << 5) + num) ^ (int)str[i];
				if (i == str.Length - 1)
				{
					break;
				}
				num2 = ((num2 << 5) + num2) ^ (int)str[i + 1];
			}
			return num + num2 * 1566083941;
		}

		/// <summary>
		/// Returns a number as a string with radix applied based on set language.
		/// i.e. one-thousand seven-hundred fifty-five returns 1,755. However, in some languages returns 1.755.
		/// Used when we show BIG numbers in the stats screen. Maybe leaderboards, I dunno.
		/// </summary>
		// Token: 0x06007FEF RID: 32751 RVA: 0x002B6C90 File Offset: 0x002B4E90
		public static string Radix(int value)
		{
			string language = SaveManager.Instance.SettingsSave.Language;
			if (language != null)
			{
				int length = language.Length;
				if (length == 3)
				{
					switch (language[0])
					{
					case 'b':
						if (!(language == "ben"))
						{
							goto IL_024A;
						}
						goto IL_0233;
					case 'c':
						if (!(language == "cze"))
						{
							goto IL_024A;
						}
						goto IL_021C;
					case 'd':
						if (!(language == "deu") && !(language == "dut"))
						{
							goto IL_024A;
						}
						break;
					case 'e':
					case 'j':
					case 'k':
					case 'l':
					case 'o':
					case 'q':
						goto IL_024A;
					case 'f':
						if (!(language == "fin") && !(language == "fra"))
						{
							goto IL_024A;
						}
						goto IL_021C;
					case 'g':
						if (!(language == "gre"))
						{
							goto IL_024A;
						}
						break;
					case 'h':
						if (!(language == "hin"))
						{
							goto IL_024A;
						}
						goto IL_0233;
					case 'i':
						if (!(language == "ind") && !(language == "ita"))
						{
							goto IL_024A;
						}
						break;
					case 'm':
						if (!(language == "mal"))
						{
							goto IL_024A;
						}
						break;
					case 'n':
						if (!(language == "nor"))
						{
							goto IL_024A;
						}
						break;
					case 'p':
						if (!(language == "por") && !(language == "ptb"))
						{
							if (!(language == "pol"))
							{
								goto IL_024A;
							}
							goto IL_021C;
						}
						break;
					case 'r':
						if (!(language == "rus"))
						{
							goto IL_024A;
						}
						goto IL_021C;
					case 's':
						if (!(language == "spa"))
						{
							if (!(language == "swe"))
							{
								goto IL_024A;
							}
							goto IL_021C;
						}
						break;
					case 't':
						if (!(language == "tur"))
						{
							goto IL_024A;
						}
						break;
					case 'u':
						if (!(language == "ukr"))
						{
							goto IL_024A;
						}
						goto IL_021C;
					case 'v':
						if (!(language == "vie"))
						{
							goto IL_024A;
						}
						break;
					default:
						goto IL_024A;
					}
					return value.ToString("N0", new CultureInfo("es-ES"));
					IL_021C:
					return value.ToString("N0", new CultureInfo("fr-FR"));
					IL_0233:
					return value.ToString("N0", new CultureInfo("hi-IN"));
				}
			}
			IL_024A:
			return value.ToString("N0", new CultureInfo("en-US"));
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x002B6EFD File Offset: 0x002B50FD
		public static LocString RatioFormat(int numerator, int denominator)
		{
			return StringHelper.RatioFormat(numerator.ToString(), denominator.ToString());
		}

		/// <summary>
		/// Helps render text like "119/670"
		/// In case there are different ratio formats in other languages.
		/// </summary>
		// Token: 0x06007FF1 RID: 32753 RVA: 0x002B6F14 File Offset: 0x002B5114
		public static LocString RatioFormat(string numerator, string denominator)
		{
			LocString locString = new LocString("stats_screen", "RATIO_FORMAT");
			locString.Add("Numerator", numerator);
			locString.Add("Denominator", denominator);
			return locString;
		}

		/// <summary>
		/// Simple helper to capitalize the first character of a given string.
		/// Does not work with multiple words. Improve and add exceptions if its use case expands.
		/// </summary>
		// Token: 0x06007FF2 RID: 32754 RVA: 0x002B6F4C File Offset: 0x002B514C
		public static string Capitalize(string input)
		{
			char c = char.ToUpperInvariant(input[0]);
			return new ReadOnlySpan<char>(ref c) + input.Substring(1, input.Length - 1);
		}

		/// <summary>
		/// Removes the bbcode tags from the given string
		/// </summary>
		// Token: 0x06007FF3 RID: 32755 RVA: 0x002B6F88 File Offset: 0x002B5188
		public static string StripBbCode(this string text)
		{
			return Regex.Replace(text, "\\[(.*?)\\]", "");
		}

		/// <summary>
		/// Replaces brackets with [lb] and [rb] so they are not interpreted as bbcode tags.
		/// </summary>
		// Token: 0x06007FF4 RID: 32756 RVA: 0x002B6F9A File Offset: 0x002B519A
		public static string EscapeBbcodeTags(this string text)
		{
			return text.Replace("[", "[lb]");
		}

		// Token: 0x04002A9B RID: 10907
		[Nullable(2)]
		[ThreadStatic]
		private static byte[] _stringHashCache;
	}
}
