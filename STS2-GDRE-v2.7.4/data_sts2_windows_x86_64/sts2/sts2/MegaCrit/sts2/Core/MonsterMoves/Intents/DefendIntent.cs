using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C9 RID: 1225
	[NullableContext(1)]
	[Nullable(0)]
	public class DefendIntent : AbstractIntent
	{
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x00225E36 File Offset: 0x00224036
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Defend;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600493C RID: 18748 RVA: 0x00225E39 File Offset: 0x00224039
		protected override string IntentPrefix
		{
			get
			{
				return "DEFEND";
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600493D RID: 18749 RVA: 0x00225E40 File Offset: 0x00224040
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_defend.tres";
			}
		}
	}
}
