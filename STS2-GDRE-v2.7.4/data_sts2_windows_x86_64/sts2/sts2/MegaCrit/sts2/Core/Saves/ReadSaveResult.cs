using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Result of a save file read operation.
	/// </summary>
	/// <typeparam name="T">The type of save data</typeparam>
	// Token: 0x020000A7 RID: 167
	[NullableContext(2)]
	[Nullable(0)]
	public class ReadSaveResult<[Nullable(0)] T> where T : ISaveSchema
	{
		/// <summary>
		/// The save data, if the operation succeeded or was recreated.
		/// </summary>
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0003ECEB File Offset: 0x0003CEEB
		public T SaveData { get; }

		/// <summary>
		/// The status of the operation.
		/// </summary>
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0003ECF3 File Offset: 0x0003CEF3
		public ReadSaveStatus Status { get; }

		/// <summary>
		/// Whether the operation succeeded and resulted in usable save data.
		/// This includes successful loads, migrations, repairs, and partial recoveries.
		/// </summary>
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0003ECFB File Offset: 0x0003CEFB
		public bool Success
		{
			get
			{
				return this.Status == ReadSaveStatus.Success || this.Status == ReadSaveStatus.MigrationRequired || this.Status == ReadSaveStatus.JsonRepaired || this.Status == ReadSaveStatus.RecoveredWithDataLoss;
			}
		}

		/// <summary>
		/// Error message, if the operation failed.
		/// </summary>
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0003ED24 File Offset: 0x0003CF24
		public string ErrorMessage { get; }

		/// <summary>
		/// Creates a new successful result with save data.
		/// </summary>
		/// <param name="data">The save data</param>
		// Token: 0x0600078E RID: 1934 RVA: 0x0003ED2C File Offset: 0x0003CF2C
		[NullableContext(1)]
		public ReadSaveResult(T data)
		{
			this.SaveData = data;
			this.Status = ReadSaveStatus.Success;
		}

		/// <summary>
		/// Creates a new result with a status.
		/// </summary>
		/// <param name="status">The status</param>
		/// <param name="errorMessage">Optional error message</param>
		// Token: 0x0600078F RID: 1935 RVA: 0x0003ED42 File Offset: 0x0003CF42
		public ReadSaveResult(ReadSaveStatus status, string errorMessage = null)
		{
			this.Status = status;
			this.ErrorMessage = errorMessage;
		}

		/// <summary>
		/// Creates a new result with a status and save data.
		/// </summary>
		/// <param name="data">The save data</param>
		/// <param name="status">The status</param>
		/// <param name="errorMessage">Optional error message</param>
		// Token: 0x06000790 RID: 1936 RVA: 0x0003ED58 File Offset: 0x0003CF58
		[NullableContext(1)]
		public ReadSaveResult(T data, ReadSaveStatus status, [Nullable(2)] string errorMessage = null)
		{
			this.SaveData = data;
			this.Status = status;
			this.ErrorMessage = errorMessage;
		}
	}
}
