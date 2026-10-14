using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;

namespace MegaCrit.Sts2.Core.Multiplayer.Connection
{
	// Token: 0x020004B9 RID: 1209
	public class ClientConnectionFailedException : Exception
	{
		// Token: 0x060048C7 RID: 18631 RVA: 0x00224FD6 File Offset: 0x002231D6
		[NullableContext(1)]
		public ClientConnectionFailedException(string message, NetErrorInfo info)
			: base(message)
		{
			this.info = info;
		}

		// Token: 0x04001C96 RID: 7318
		public NetErrorInfo info;
	}
}
