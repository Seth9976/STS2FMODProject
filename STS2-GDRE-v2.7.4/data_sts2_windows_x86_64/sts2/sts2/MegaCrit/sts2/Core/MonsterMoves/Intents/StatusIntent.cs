using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004D1 RID: 1233
	[NullableContext(1)]
	[Nullable(0)]
	public class StatusIntent : AbstractIntent
	{
		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x0022603F File Offset: 0x0022423F
		public override IntentType IntentType
		{
			get
			{
				return IntentType.StatusCard;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600495D RID: 18781 RVA: 0x00226043 File Offset: 0x00224243
		protected override LocString IntentLabelFormat
		{
			get
			{
				return new LocString("intents", "FORMAT_STATUS_CARD_COUNT");
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x00226054 File Offset: 0x00224254
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_status_card.tres";
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x0022605B File Offset: 0x0022425B
		protected override string IntentPrefix
		{
			get
			{
				return "STATUS";
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004960 RID: 18784 RVA: 0x00226062 File Offset: 0x00224262
		public int CardCount { get; }

		// Token: 0x06004961 RID: 18785 RVA: 0x0022606A File Offset: 0x0022426A
		public StatusIntent(int count)
		{
			this.CardCount = count;
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0022607C File Offset: 0x0022427C
		public override LocString GetIntentLabel(IEnumerable<Creature> _, Creature __)
		{
			LocString intentLabelFormat = this.IntentLabelFormat;
			intentLabelFormat.Add("CardCount", this.CardCount);
			return intentLabelFormat;
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x002260A8 File Offset: 0x002242A8
		protected override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
		{
			LocString intentDescription = base.GetIntentDescription(targets, owner);
			intentDescription.Add("CardCount", this.CardCount);
			return intentDescription;
		}
	}
}
