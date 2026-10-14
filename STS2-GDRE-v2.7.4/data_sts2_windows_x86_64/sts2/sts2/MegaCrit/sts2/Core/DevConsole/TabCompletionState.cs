using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.DevConsole
{
	/// <summary>
	/// Encapsulates all state for interactive tab completion in the dev console.
	/// </summary>
	// Token: 0x02000CDD RID: 3293
	[NullableContext(2)]
	[Nullable(0)]
	public class TabCompletionState
	{
		// Token: 0x17002476 RID: 9334
		// (get) Token: 0x06008562 RID: 34146 RVA: 0x002CCE7D File Offset: 0x002CB07D
		// (set) Token: 0x06008563 RID: 34147 RVA: 0x002CCE85 File Offset: 0x002CB085
		public int SelectionIndex { get; set; } = -1;

		// Token: 0x17002477 RID: 9335
		// (get) Token: 0x06008564 RID: 34148 RVA: 0x002CCE8E File Offset: 0x002CB08E
		[Nullable(1)]
		public List<string> CompletionCandidates
		{
			[NullableContext(1)]
			get;
		} = new List<string>();

		// Token: 0x17002478 RID: 9336
		// (get) Token: 0x06008565 RID: 34149 RVA: 0x002CCE96 File Offset: 0x002CB096
		// (set) Token: 0x06008566 RID: 34150 RVA: 0x002CCE9E File Offset: 0x002CB09E
		public bool InSelectionMode { get; set; }

		// Token: 0x17002479 RID: 9337
		// (get) Token: 0x06008567 RID: 34151 RVA: 0x002CCEA7 File Offset: 0x002CB0A7
		// (set) Token: 0x06008568 RID: 34152 RVA: 0x002CCEAF File Offset: 0x002CB0AF
		public bool ProgrammaticTextChange { get; set; }

		// Token: 0x1700247A RID: 9338
		// (get) Token: 0x06008569 RID: 34153 RVA: 0x002CCEB8 File Offset: 0x002CB0B8
		// (set) Token: 0x0600856A RID: 34154 RVA: 0x002CCEC0 File Offset: 0x002CB0C0
		public CompletionResult LastCompletionResult { get; set; }

		// Token: 0x0600856B RID: 34155 RVA: 0x002CCEC9 File Offset: 0x002CB0C9
		public void Reset()
		{
			this.InSelectionMode = false;
			this.SelectionIndex = -1;
			this.CompletionCandidates.Clear();
			this.LastCompletionResult = null;
		}
	}
}
