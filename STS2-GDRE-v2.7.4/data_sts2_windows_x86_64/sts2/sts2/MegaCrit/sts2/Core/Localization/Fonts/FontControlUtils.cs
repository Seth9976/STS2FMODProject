using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Localization.Fonts
{
	// Token: 0x02000BCE RID: 3022
	public static class FontControlUtils
	{
		/// <summary>
		/// Applies locale-specific font substitution if the current language requires it.
		/// </summary>
		// Token: 0x06007DD0 RID: 32208 RVA: 0x002ACE20 File Offset: 0x002AB020
		[NullableContext(1)]
		public static void ApplyLocaleFontSubstitution(this Control control, FontType fontType, StringName themeFontName)
		{
			if (Engine.IsEditorHint())
			{
				return;
			}
			if (TestMode.IsOn)
			{
				return;
			}
			if (LocManager.Instance == null)
			{
				return;
			}
			if (!FontManager.NeedsFontSubstitution(LocManager.Instance.Language))
			{
				return;
			}
			Font substituteFont = FontManager.GetSubstituteFont(LocManager.Instance.Language, fontType);
			if (substituteFont == null)
			{
				return;
			}
			control.AddThemeFontOverride(themeFontName, substituteFont);
		}
	}
}
