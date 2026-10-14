using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004CF RID: 1231
	[NullableContext(1)]
	[Nullable(0)]
	public class SingleAttackIntent : AttackIntent
	{
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004952 RID: 18770 RVA: 0x00225F93 File Offset: 0x00224193
		public override int Repeats
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004953 RID: 18771 RVA: 0x00225F96 File Offset: 0x00224196
		protected override LocString IntentLabelFormat
		{
			get
			{
				return new LocString("intents", "FORMAT_DAMAGE_SINGLE");
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00225FA8 File Offset: 0x002241A8
		public SingleAttackIntent(int damage)
		{
			base.DamageCalc = () => damage;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00225FDA File Offset: 0x002241DA
		public SingleAttackIntent(Func<decimal> damageCalc)
		{
			base.DamageCalc = damageCalc;
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00225FE9 File Offset: 0x002241E9
		public override int GetTotalDamage(IEnumerable<Creature> targets, Creature owner)
		{
			return base.GetSingleDamage(targets, owner);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00225FF4 File Offset: 0x002241F4
		public override LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
		{
			LocString intentLabelFormat = this.IntentLabelFormat;
			float num = (float)this.GetTotalDamage(targets, owner);
			intentLabelFormat.Add("Damage", (int)num);
			return intentLabelFormat;
		}
	}
}
