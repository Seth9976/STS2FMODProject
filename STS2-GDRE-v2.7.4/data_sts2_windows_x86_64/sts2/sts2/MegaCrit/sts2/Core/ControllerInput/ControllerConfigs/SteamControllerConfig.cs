using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs
{
	// Token: 0x02000D24 RID: 3364
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamControllerConfig : ControllerConfig
	{
		// Token: 0x1700256D RID: 9581
		// (get) Token: 0x060087A7 RID: 34727 RVA: 0x002DB00E File Offset: 0x002D920E
		protected override string FolderPath
		{
			get
			{
				return "atlases/controller_atlas.sprites/steam";
			}
		}
	}
}
