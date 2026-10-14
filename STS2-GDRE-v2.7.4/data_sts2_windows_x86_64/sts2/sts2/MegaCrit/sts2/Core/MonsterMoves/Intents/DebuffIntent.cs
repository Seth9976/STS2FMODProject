using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C8 RID: 1224
	[NullableContext(1)]
	[Nullable(0)]
	public class DebuffIntent : AbstractIntent
	{
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06004937 RID: 18743 RVA: 0x00225E0C File Offset: 0x0022400C
		public override IntentType IntentType
		{
			get
			{
				if (!this._strong)
				{
					return IntentType.Debuff;
				}
				return IntentType.DebuffStrong;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x00225E19 File Offset: 0x00224019
		protected override string IntentPrefix
		{
			get
			{
				return "DEBUFF";
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06004939 RID: 18745 RVA: 0x00225E20 File Offset: 0x00224020
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_debuff.tres";
			}
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00225E27 File Offset: 0x00224027
		public DebuffIntent(bool strong = false)
		{
			this._strong = strong;
		}

		// Token: 0x04001CB4 RID: 7348
		private readonly bool _strong;
	}
}
