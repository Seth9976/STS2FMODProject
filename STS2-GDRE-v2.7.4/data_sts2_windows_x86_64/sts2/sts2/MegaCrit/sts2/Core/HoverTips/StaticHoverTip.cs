using System;

namespace MegaCrit.Sts2.Core.HoverTips
{
	/// <summary>
	/// HoverTip tags that don't correspond to a model (i.e. powers/relics)
	/// See <see cref="M:MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.Static(MegaCrit.Sts2.Core.HoverTips.StaticHoverTip,MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar[])" /> for example usage.
	/// </summary>
	// Token: 0x02000BFD RID: 3069
	public enum StaticHoverTip
	{
		// Token: 0x04002A63 RID: 10851
		None,
		// Token: 0x04002A64 RID: 10852
		Channeling = 2,
		// Token: 0x04002A65 RID: 10853
		Evoke,
		// Token: 0x04002A66 RID: 10854
		Transform,
		// Token: 0x04002A67 RID: 10855
		Block,
		// Token: 0x04002A68 RID: 10856
		Fatal,
		// Token: 0x04002A69 RID: 10857
		Energy,
		// Token: 0x04002A6A RID: 10858
		Stun,
		// Token: 0x04002A6B RID: 10859
		CardReward,
		// Token: 0x04002A6C RID: 10860
		Forge,
		// Token: 0x04002A6D RID: 10861
		SummonDynamic = 12,
		// Token: 0x04002A6E RID: 10862
		SummonStatic,
		// Token: 0x04002A6F RID: 10863
		ReplayDynamic,
		// Token: 0x04002A70 RID: 10864
		ReplayStatic,
		// Token: 0x04002A71 RID: 10865
		Cook
	}
}
