using System;
using System.Runtime.CompilerServices;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x02000164 RID: 356
	public class SteamRemoteSaveStoreException : Exception
	{
		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00063CB1 File Offset: 0x00061EB1
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x00063CB9 File Offset: 0x00061EB9
		public EResult Result { get; private set; }

		// Token: 0x060010E9 RID: 4329 RVA: 0x00063CC2 File Offset: 0x00061EC2
		[NullableContext(1)]
		public SteamRemoteSaveStoreException(string message, EResult result)
			: base(message)
		{
			this.Result = result;
		}
	}
}
