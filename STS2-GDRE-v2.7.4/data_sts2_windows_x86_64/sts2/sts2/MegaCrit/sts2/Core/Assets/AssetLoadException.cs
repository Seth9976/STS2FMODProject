using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Assets
{
	// Token: 0x02000D8E RID: 3470
	[NullableContext(1)]
	[Nullable(0)]
	public class AssetLoadException : Exception
	{
		// Token: 0x06008B8E RID: 35726 RVA: 0x002E92A2 File Offset: 0x002E74A2
		public AssetLoadException(string message)
			: base(message)
		{
		}

		// Token: 0x06008B8F RID: 35727 RVA: 0x002E92AB File Offset: 0x002E74AB
		public AssetLoadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
