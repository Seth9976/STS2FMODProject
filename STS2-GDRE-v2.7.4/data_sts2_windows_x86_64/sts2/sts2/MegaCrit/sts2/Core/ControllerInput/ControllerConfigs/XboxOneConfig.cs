using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs
{
	// Token: 0x02000D27 RID: 3367
	[NullableContext(1)]
	[Nullable(0)]
	public class XboxOneConfig : ControllerConfig
	{
		// Token: 0x17002575 RID: 9589
		// (get) Token: 0x060087B2 RID: 34738 RVA: 0x002DB09A File Offset: 0x002D929A
		protected override string FolderPath
		{
			get
			{
				return "atlases/controller_atlas.sprites/xbox_one";
			}
		}
	}
}
