using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004CC RID: 1228
	[NullableContext(1)]
	[Nullable(0)]
	public class HiddenIntent : AbstractIntent
	{
		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004947 RID: 18759 RVA: 0x00225E81 File Offset: 0x00224081
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Hidden;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004948 RID: 18760 RVA: 0x00225E84 File Offset: 0x00224084
		protected override string IntentPrefix
		{
			get
			{
				return "HIDDEN";
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004949 RID: 18761 RVA: 0x00225E8B File Offset: 0x0022408B
		[Nullable(2)]
		protected override string SpritePath
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x0600494A RID: 18762 RVA: 0x00225E8E File Offset: 0x0022408E
		public override bool HasIntentTip
		{
			get
			{
				return false;
			}
		}
	}
}
