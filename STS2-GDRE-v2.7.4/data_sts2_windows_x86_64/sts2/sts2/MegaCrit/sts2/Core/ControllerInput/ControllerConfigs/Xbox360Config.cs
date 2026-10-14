using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs
{
	// Token: 0x02000D26 RID: 3366
	[NullableContext(1)]
	[Nullable(0)]
	public class Xbox360Config : ControllerConfig
	{
		// Token: 0x17002574 RID: 9588
		// (get) Token: 0x060087B0 RID: 34736 RVA: 0x002DB08B File Offset: 0x002D928B
		protected override string FolderPath
		{
			get
			{
				return "atlases/controller_atlas.sprites/xbox_360";
			}
		}
	}
}
