using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200086F RID: 2159
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class BattlewornDummyEventEncounter : EncounterModel
	{
		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x060069F9 RID: 27129 RVA: 0x0027EB74 File Offset: 0x0027CD74
		// (set) Token: 0x060069FA RID: 27130 RVA: 0x0027EB7C File Offset: 0x0027CD7C
		public bool RanOutOfTime
		{
			get
			{
				return this._ranOutOfTime;
			}
			set
			{
				base.AssertMutable();
				this._ranOutOfTime = value;
			}
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x0027EB8C File Offset: 0x0027CD8C
		public override Dictionary<string, string> SaveCustomState()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["RanOutOfTime"] = this.RanOutOfTime.ToString();
			return dictionary;
		}

		// Token: 0x060069FC RID: 27132 RVA: 0x0027EBB7 File Offset: 0x0027CDB7
		public override void LoadCustomState(Dictionary<string, string> state)
		{
			this.RanOutOfTime = bool.Parse(state["RanOutOfTime"]);
		}

		// Token: 0x04002801 RID: 10241
		private const string _ranOutOfTimeKey = "RanOutOfTime";

		// Token: 0x04002802 RID: 10242
		private bool _ranOutOfTime;
	}
}
