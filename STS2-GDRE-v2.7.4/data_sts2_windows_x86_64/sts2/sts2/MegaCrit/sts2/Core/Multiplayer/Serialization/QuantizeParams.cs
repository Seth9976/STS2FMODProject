using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000452 RID: 1106
	public struct QuantizeParams
	{
		// Token: 0x0600454F RID: 17743 RVA: 0x00215A54 File Offset: 0x00213C54
		public QuantizeParams(float min, float max, int bits)
		{
			this.min = min;
			this.max = max;
			this.bits = bits;
		}

		// Token: 0x04001ADF RID: 6879
		public float min;

		// Token: 0x04001AE0 RID: 6880
		public float max;

		// Token: 0x04001AE1 RID: 6881
		public int bits;
	}
}
