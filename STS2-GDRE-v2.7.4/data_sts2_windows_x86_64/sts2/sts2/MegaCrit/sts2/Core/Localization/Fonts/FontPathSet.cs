using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts
{
	/// <summary>
	/// A set of paths that can be used for substitution for a given language.
	/// </summary>
	// Token: 0x02000BD0 RID: 3024
	public abstract class FontPathSet
	{
		/// <summary>
		/// Get the path for a given font type.
		/// </summary>
		/// <param name="type">What type of font to get the path for (Regular, Bold, etc.)</param>
		/// <returns>The path to the font, or null if not found.</returns>
		// Token: 0x06007DD6 RID: 32214
		[NullableContext(2)]
		public abstract string GetPath(FontType type);
	}
}
