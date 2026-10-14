using System;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// This class serves as a schema for the Profile save file.
	/// It's used for storing the preferred profile that the player plays as.
	/// This file is stored at the account level, not at the profile level, and is synced across devices/platforms.
	/// </summary>
	// Token: 0x020000A4 RID: 164
	public class ProfileSave : ISaveSchema
	{
		/// <summary>
		/// The schema version of this save.
		/// </summary>
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0003C121 File Offset: 0x0003A321
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0003C129 File Offset: 0x0003A329
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0003C132 File Offset: 0x0003A332
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0003C13A File Offset: 0x0003A33A
		[JsonPropertyName("last_profile_id")]
		public int LastProfileId { get; set; } = 1;
	}
}
