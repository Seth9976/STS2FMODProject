using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BB0 RID: 2992
	public static class IdAnonymizer
	{
		// Token: 0x06007CCD RID: 31949 RVA: 0x002A8A68 File Offset: 0x002A6C68
		public static ulong Anonymize(ulong id)
		{
			ulong num;
			if (!IdAnonymizer._idToAnonymized.TryGetValue(id, out num))
			{
				num = (ulong)Rng.Chaotic.NextUnsignedInt(uint.MaxValue);
				IdAnonymizer._idToAnonymized[id] = num;
			}
			return num;
		}

		// Token: 0x040029A0 RID: 10656
		[Nullable(1)]
		private static readonly Dictionary<ulong, ulong> _idToAnonymized = new Dictionary<ulong, ulong>();
	}
}
