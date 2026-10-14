using System;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Status codes for save file reading operations.
	/// </summary>
	// Token: 0x020000A8 RID: 168
	public enum ReadSaveStatus
	{
		/// <summary>
		/// The operation succeeded.
		/// </summary>
		// Token: 0x04000291 RID: 657
		Success,
		/// <summary>
		/// The JSON could not be parsed.
		/// </summary>
		// Token: 0x04000292 RID: 658
		JsonParseError,
		/// <summary>
		/// The file was not found.
		/// </summary>
		// Token: 0x04000293 RID: 659
		FileNotFound,
		/// <summary>
		/// The file was empty.
		/// </summary>
		// Token: 0x04000294 RID: 660
		FileEmpty,
		/// <summary>
		/// The migration failed.
		/// </summary>
		// Token: 0x04000295 RID: 661
		MigrationFailed,
		/// <summary>
		/// The schema version is missing from the save file.
		/// </summary>
		// Token: 0x04000296 RID: 662
		MissingSchemaVersion,
		/// <summary>
		/// The save file version is newer than the current version.
		/// </summary>
		// Token: 0x04000297 RID: 663
		FutureVersion,
		/// <summary>
		/// The save file version is too old to migrate.
		/// </summary>
		// Token: 0x04000298 RID: 664
		VersionTooOld,
		/// <summary>
		/// The save required migration and was migrated in memory.
		/// </summary>
		// Token: 0x04000299 RID: 665
		MigrationRequired,
		/// <summary>
		/// The JSON had errors that were automatically repaired.
		/// </summary>
		// Token: 0x0400029A RID: 666
		JsonRepaired,
		/// <summary>
		/// Data was recovered but some information may be lost.
		/// </summary>
		// Token: 0x0400029B RID: 667
		RecoveredWithDataLoss,
		/// <summary>
		/// File access error (I/O error, permissions, etc).
		/// </summary>
		// Token: 0x0400029C RID: 668
		FileAccessError,
		/// <summary>
		/// The save is completely unrecoverable.
		/// </summary>
		// Token: 0x0400029D RID: 669
		Unrecoverable,
		/// <summary>
		/// The save file loaded successfully but failed content validation.
		/// </summary>
		// Token: 0x0400029E RID: 670
		ValidationFailed
	}
}
