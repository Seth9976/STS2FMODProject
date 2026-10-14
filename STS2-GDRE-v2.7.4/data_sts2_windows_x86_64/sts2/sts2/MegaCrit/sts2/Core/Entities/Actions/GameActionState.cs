using System;

namespace MegaCrit.Sts2.Core.Entities.Actions
{
	// Token: 0x02000CD5 RID: 3285
	public enum GameActionState
	{
		// Token: 0x04002E3F RID: 11839
		None,
		// Token: 0x04002E40 RID: 11840
		WaitingForExecution,
		// Token: 0x04002E41 RID: 11841
		Executing,
		// Token: 0x04002E42 RID: 11842
		GatheringPlayerChoice,
		// Token: 0x04002E43 RID: 11843
		ReadyToResumeExecuting,
		// Token: 0x04002E44 RID: 11844
		Finished,
		// Token: 0x04002E45 RID: 11845
		Canceled
	}
}
