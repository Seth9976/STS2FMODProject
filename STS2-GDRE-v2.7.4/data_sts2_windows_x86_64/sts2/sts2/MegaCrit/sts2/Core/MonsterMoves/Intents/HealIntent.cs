using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004CB RID: 1227
	[NullableContext(1)]
	[Nullable(0)]
	public class HealIntent : AbstractIntent
	{
		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06004943 RID: 18755 RVA: 0x00225E68 File Offset: 0x00224068
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Heal;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06004944 RID: 18756 RVA: 0x00225E6B File Offset: 0x0022406B
		protected override string IntentPrefix
		{
			get
			{
				return "HEAL";
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06004945 RID: 18757 RVA: 0x00225E72 File Offset: 0x00224072
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_heal.tres";
			}
		}
	}
}
