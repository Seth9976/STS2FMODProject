using System;
using System.Runtime.CompilerServices;
using SmartFormat.Core.Parsing;

namespace MegaCrit.Sts2.Core.Localization
{
	/// <summary>
	/// Validates SmartFormat syntax in localization strings.
	/// Used to catch format string errors early during override file loading.
	/// </summary>
	// Token: 0x02000BC2 RID: 3010
	public static class LocValidator
	{
		/// <summary>
		/// Validates a SmartFormat format string for syntax errors.
		/// </summary>
		/// <param name="text">The format string to validate</param>
		/// <param name="errorMessage">The error message if validation fails, or null if valid</param>
		/// <returns>True if the format string is syntactically valid, false otherwise</returns>
		// Token: 0x06007D93 RID: 32147 RVA: 0x002AC1D4 File Offset: 0x002AA3D4
		[NullableContext(1)]
		public static bool ValidateFormatString(string text, [Nullable(2)] out string errorMessage)
		{
			bool flag;
			try
			{
				Parser parser = new Parser(null);
				parser.ParseFormat(text);
				errorMessage = null;
				flag = true;
			}
			catch (ParsingErrors parsingErrors)
			{
				errorMessage = parsingErrors.Message;
				flag = false;
			}
			return flag;
		}
	}
}
