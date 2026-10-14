using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004CA RID: 1226
	[NullableContext(1)]
	[Nullable(0)]
	public class EscapeIntent : AbstractIntent
	{
		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x0600493F RID: 18751 RVA: 0x00225E4F File Offset: 0x0022404F
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Escape;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x00225E52 File Offset: 0x00224052
		protected override string IntentPrefix
		{
			get
			{
				return "ESCAPE";
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06004941 RID: 18753 RVA: 0x00225E59 File Offset: 0x00224059
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_escape.tres";
			}
		}
	}
}
