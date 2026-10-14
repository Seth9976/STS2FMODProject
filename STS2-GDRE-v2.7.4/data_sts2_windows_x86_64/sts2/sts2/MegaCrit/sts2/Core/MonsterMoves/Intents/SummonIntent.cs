using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004D3 RID: 1235
	[NullableContext(1)]
	[Nullable(0)]
	public class SummonIntent : AbstractIntent
	{
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004969 RID: 18793 RVA: 0x00226149 File Offset: 0x00224349
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Summon;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600496A RID: 18794 RVA: 0x0022614C File Offset: 0x0022434C
		protected override string IntentPrefix
		{
			get
			{
				return "SUMMON";
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x0600496B RID: 18795 RVA: 0x00226153 File Offset: 0x00224353
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_summon.tres";
			}
		}
	}
}
