using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004CE RID: 1230
	[NullableContext(1)]
	[Nullable(0)]
	public class MultiAttackIntent : AttackIntent
	{
		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600494C RID: 18764 RVA: 0x00225E99 File Offset: 0x00224099
		protected override LocString IntentLabelFormat
		{
			get
			{
				return new LocString("intents", "FORMAT_DAMAGE_MULTI");
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600494D RID: 18765 RVA: 0x00225EAA File Offset: 0x002240AA
		public override int Repeats
		{
			get
			{
				Func<int> repeatCalc = this._repeatCalc;
				if (repeatCalc == null)
				{
					return this._repeat;
				}
				return repeatCalc();
			}
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x00225EC4 File Offset: 0x002240C4
		public MultiAttackIntent(int damage, int repeat)
		{
			base.DamageCalc = () => damage;
			this._repeat = repeat;
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00225F00 File Offset: 0x00224100
		public MultiAttackIntent(int damage, Func<int> repeatCalc)
		{
			base.DamageCalc = () => damage;
			this._repeatCalc = repeatCalc;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x00225F39 File Offset: 0x00224139
		public override int GetTotalDamage(IEnumerable<Creature> targets, Creature owner)
		{
			return base.GetSingleDamage(targets, owner) * this.Repeats;
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x00225F4C File Offset: 0x0022414C
		public override LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
		{
			LocString intentLabelFormat = this.IntentLabelFormat;
			float num = (float)base.GetSingleDamage(targets, owner);
			intentLabelFormat.Add("Damage", (int)num);
			intentLabelFormat.Add("Repeat", this.Repeats);
			return intentLabelFormat;
		}

		// Token: 0x04001CC5 RID: 7365
		private readonly int _repeat;

		// Token: 0x04001CC6 RID: 7366
		[Nullable(2)]
		private readonly Func<int> _repeatCalc;
	}
}
