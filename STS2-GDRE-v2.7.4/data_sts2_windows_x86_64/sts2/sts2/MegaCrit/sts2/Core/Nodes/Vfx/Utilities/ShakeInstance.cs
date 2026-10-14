using System;
using Godot;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000224 RID: 548
	public abstract class ShakeInstance
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000B4C0C File Offset: 0x000B2E0C
		protected static float WiggleSpeed
		{
			get
			{
				return 60f;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x000B4C13 File Offset: 0x000B2E13
		// (set) Token: 0x06001BD5 RID: 7125 RVA: 0x000B4C1B File Offset: 0x000B2E1B
		public bool IsDone { get; protected set; }

		// Token: 0x06001BD6 RID: 7126
		public abstract Vector2 Update(double delta);

		// Token: 0x04000A91 RID: 2705
		protected float _strength;

		// Token: 0x04000A92 RID: 2706
		protected double _startDuration;

		// Token: 0x04000A93 RID: 2707
		protected double _duration;

		// Token: 0x04000A94 RID: 2708
		protected float _ease;

		// Token: 0x04000A95 RID: 2709
		protected float _angle;
	}
}
