using System;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Extension methods for ReadSaveStatus.
	/// </summary>
	// Token: 0x020000A9 RID: 169
	public static class ReadSaveStatusExtensions
	{
		/// <summary>
		/// Determines if a save read status is recoverable.
		/// </summary>
		/// <param name="status">The read status to check</param>
		/// <returns>True if the status indicates a recoverable error or success</returns>
		// Token: 0x06000791 RID: 1937 RVA: 0x0003ED78 File Offset: 0x0003CF78
		public static bool IsRecoverable(this ReadSaveStatus status)
		{
			bool flag;
			switch (status)
			{
			case ReadSaveStatus.JsonParseError:
			case ReadSaveStatus.FileEmpty:
			case ReadSaveStatus.MigrationFailed:
			case ReadSaveStatus.FutureVersion:
			case ReadSaveStatus.VersionTooOld:
			case ReadSaveStatus.FileAccessError:
			case ReadSaveStatus.Unrecoverable:
				flag = true;
				goto IL_0040;
			}
			flag = false;
			IL_0040:
			return !flag;
		}
	}
}
