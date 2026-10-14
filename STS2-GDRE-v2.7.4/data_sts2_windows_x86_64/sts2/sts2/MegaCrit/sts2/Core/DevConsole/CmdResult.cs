using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CD6 RID: 3286
	[NullableContext(1)]
	[Nullable(0)]
	public struct CmdResult
	{
		// Token: 0x06008535 RID: 34101 RVA: 0x002CC003 File Offset: 0x002CA203
		[NullableContext(2)]
		public CmdResult(bool success, string msg = null)
		{
			this.success = success;
			this.msg = msg ?? string.Empty;
			this.task = null;
		}

		// Token: 0x06008536 RID: 34102 RVA: 0x002CC023 File Offset: 0x002CA223
		public CmdResult(Task task, bool success, [Nullable(2)] string msg = null)
		{
			this.task = task;
			this.success = success;
			this.msg = msg ?? string.Empty;
		}

		// Token: 0x04002E46 RID: 11846
		public readonly bool success;

		// Token: 0x04002E47 RID: 11847
		public readonly string msg;

		// Token: 0x04002E48 RID: 11848
		[Nullable(2)]
		public readonly Task task;
	}
}
