using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets;

namespace MegaCrit.Sts2.Core.Localization.Fonts
{
	/// <summary>
	/// Manages locale-specific font substitution for languages that require different fonts
	/// (e.g., CJK languages that need fonts with appropriate glyph coverage).
	/// </summary>
	// Token: 0x02000BCF RID: 3023
	[NullableContext(1)]
	[Nullable(0)]
	public static class FontManager
	{
		/// <summary>
		/// Returns true if the given language requires font substitution.
		/// </summary>
		// Token: 0x06007DD1 RID: 32209 RVA: 0x002ACE74 File Offset: 0x002AB074
		public static bool NeedsFontSubstitution(string language)
		{
			return FontManager._languageFontPathSets.ContainsKey(language);
		}

		/// <summary>
		/// Gets the substitute font for the given type based on the current locale.
		/// Returns null if no substitution is needed.
		/// </summary>
		// Token: 0x06007DD2 RID: 32210 RVA: 0x002ACE81 File Offset: 0x002AB081
		[return: Nullable(2)]
		public static Font GetSubstituteFont(string language, FontType type)
		{
			if (!FontManager.NeedsFontSubstitution(language))
			{
				return null;
			}
			return FontManager.GetFontForLanguage(language, type);
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x002ACE94 File Offset: 0x002AB094
		public static void ClearCache()
		{
			FontManager._localeFonts.Clear();
		}

		/// <summary>
		/// Gets the font for the specified language and type, loading and caching if necessary.
		/// </summary>
		// Token: 0x06007DD4 RID: 32212 RVA: 0x002ACEA0 File Offset: 0x002AB0A0
		[return: Nullable(2)]
		private static Font GetFontForLanguage(string language, FontType type)
		{
			Dictionary<FontType, Font> dictionary;
			Font font;
			if (FontManager._localeFonts.TryGetValue(language, out dictionary) && dictionary.TryGetValue(type, out font))
			{
				return font;
			}
			string path = FontManager._languageFontPathSets[language].GetPath(type);
			if (path == null)
			{
				return null;
			}
			Font font2 = ResourceLoader.Load<Font>(path, null, 1L);
			Dictionary<FontType, Font> dictionary2;
			if (!FontManager._localeFonts.TryGetValue(language, out dictionary2))
			{
				dictionary2 = new Dictionary<FontType, Font>();
				FontManager._localeFonts[language] = dictionary2;
			}
			dictionary2[type] = font2;
			return font2;
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x002ACF18 File Offset: 0x002AB118
		// Note: this type is marked as 'beforefieldinit'.
		static FontManager()
		{
			Dictionary<string, FontPathSet> dictionary = new Dictionary<string, FontPathSet>();
			dictionary["jpn"] = new JpnFontPathSet();
			dictionary["kor"] = new KorFontPathSet();
			dictionary["pol"] = FontManager._russian;
			dictionary["rus"] = FontManager._russian;
			dictionary["tha"] = new ThaFontPathSet();
			dictionary["zhs"] = new ZhsFontPathSet();
			dictionary["zht"] = new ZhtFontPathSet();
			FontManager._languageFontPathSets = dictionary;
			FontManager._localeFonts = new Dictionary<string, Dictionary<FontType, Font>>();
		}

		// Token: 0x04002A09 RID: 10761
		private static readonly FontPathSet _russian = new RusFontPathSet();

		/// <summary>
		/// Languages that require font substitution.
		/// Key is language code, value is the set of font paths for that language.
		/// </summary>
		// Token: 0x04002A0A RID: 10762
		private static readonly IReadOnlyDictionary<string, FontPathSet> _languageFontPathSets;

		/// <summary>
		/// Cached font resources, keyed by language code then font type.
		/// </summary>
		// Token: 0x04002A0B RID: 10763
		private static readonly Dictionary<string, Dictionary<FontType, Font>> _localeFonts;
	}
}
