using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004D4 RID: 1236
	[NullableContext(1)]
	[Nullable(0)]
	public class UnknownIntent : AbstractIntent
	{
		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600496D RID: 18797 RVA: 0x00226162 File Offset: 0x00224362
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Unknown;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600496E RID: 18798 RVA: 0x00226166 File Offset: 0x00224366
		protected override string IntentPrefix
		{
			get
			{
				return "UNKNOWN";
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x0022616D File Offset: 0x0022436D
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_unknown.tres";
			}
		}
	}
}
