using System;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C14 RID: 3092
	[NullableContext(1)]
	[Nullable(0)]
	public static class SeedHelper
	{
		// Token: 0x06007FE1 RID: 32737 RVA: 0x002B69CC File Offset: 0x002B4BCC
		public static string GetRandomSeed([Nullable(2)] Rng rng = null, int length = 12)
		{
			if (rng == null)
			{
				rng = Rng.Chaotic;
			}
			string text;
			do
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < length; i++)
				{
					stringBuilder.Append(rng.NextItem<char>("0123456789ABCDEFGHJKLMNPQRSTUVWXYZ"));
				}
				text = stringBuilder.ToString();
			}
			while (BadWordChecker.ContainsBadWord(text));
			return text;
		}

		// Token: 0x06007FE2 RID: 32738 RVA: 0x002B6A17 File Offset: 0x002B4C17
		public static string CanonicalizeSeed(string seed)
		{
			seed = seed.ToUpperInvariant();
			seed = seed.Replace('O', '0');
			seed = seed.Replace('I', '1');
			seed = seed.Trim();
			return seed;
		}

		/// <summary>
		/// Possible characters for the seed.
		/// O and I are not included. They are replaced by 0 and 1.
		/// </summary>
		// Token: 0x04002A98 RID: 10904
		private const string _characters = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";

		// Token: 0x04002A99 RID: 10905
		public const int seedDefaultLength = 12;
	}
}
