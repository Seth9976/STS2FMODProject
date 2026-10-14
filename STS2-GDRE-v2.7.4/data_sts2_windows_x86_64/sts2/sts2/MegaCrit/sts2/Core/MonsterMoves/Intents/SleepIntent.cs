using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004D0 RID: 1232
	[NullableContext(1)]
	[Nullable(0)]
	public class SleepIntent : AbstractIntent
	{
		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004958 RID: 18776 RVA: 0x00226025 File Offset: 0x00224225
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Sleep;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004959 RID: 18777 RVA: 0x00226029 File Offset: 0x00224229
		protected override string IntentPrefix
		{
			get
			{
				return "SLEEP";
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600495A RID: 18778 RVA: 0x00226030 File Offset: 0x00224230
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_sleep.tres";
			}
		}
	}
}
