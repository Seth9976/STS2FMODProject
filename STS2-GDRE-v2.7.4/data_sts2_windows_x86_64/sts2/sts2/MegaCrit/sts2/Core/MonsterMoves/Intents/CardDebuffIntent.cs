using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C6 RID: 1222
	[NullableContext(1)]
	[Nullable(0)]
	public class CardDebuffIntent : AbstractIntent
	{
		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x00225D86 File Offset: 0x00223F86
		public override IntentType IntentType
		{
			get
			{
				return IntentType.CardDebuff;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x00225D8A File Offset: 0x00223F8A
		protected override string IntentPrefix
		{
			get
			{
				return "CARD_DEBUFF";
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x00225D91 File Offset: 0x00223F91
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_card_debuff.tres";
			}
		}
	}
}
