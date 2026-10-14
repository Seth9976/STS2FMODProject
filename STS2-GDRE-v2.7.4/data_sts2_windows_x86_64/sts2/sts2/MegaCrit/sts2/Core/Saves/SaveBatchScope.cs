using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Disposable scope returned by <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.BeginSaveBatch" />.
	/// Calls <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.EndSaveBatch" /> on dispose, allowing batch usage with <c>using</c>.
	/// </summary>
	// Token: 0x020000AB RID: 171
	public readonly struct SaveBatchScope : IDisposable
	{
		/// <summary>
		/// Disposable scope returned by <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.BeginSaveBatch" />.
		/// Calls <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.EndSaveBatch" /> on dispose, allowing batch usage with <c>using</c>.
		/// </summary>
		// Token: 0x060007F6 RID: 2038 RVA: 0x000400B6 File Offset: 0x0003E2B6
		[NullableContext(1)]
		public SaveBatchScope(SaveManager saveManager)
		{
			this.<saveManager>P = saveManager;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000400BF File Offset: 0x0003E2BF
		public void Dispose()
		{
			this.<saveManager>P.EndSaveBatch();
		}

		// Token: 0x040002AE RID: 686
		[Nullable(1)]
		[CompilerGenerated]
		private readonly SaveManager <saveManager>P;
	}
}
