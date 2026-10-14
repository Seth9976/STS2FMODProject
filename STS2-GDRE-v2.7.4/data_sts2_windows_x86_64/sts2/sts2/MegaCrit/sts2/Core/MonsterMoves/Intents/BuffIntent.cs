using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C5 RID: 1221
	[NullableContext(1)]
	[Nullable(0)]
	public class BuffIntent : AbstractIntent
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06004929 RID: 18729 RVA: 0x00225D6D File Offset: 0x00223F6D
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Buff;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600492A RID: 18730 RVA: 0x00225D70 File Offset: 0x00223F70
		protected override string IntentPrefix
		{
			get
			{
				return "BUFF";
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x00225D77 File Offset: 0x00223F77
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_buff.tres";
			}
		}
	}
}
