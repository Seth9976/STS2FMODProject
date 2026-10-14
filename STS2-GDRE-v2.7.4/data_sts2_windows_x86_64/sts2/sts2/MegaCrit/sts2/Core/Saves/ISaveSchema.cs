using System;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Interface for all save schemas that can be serialized and migrated.
	/// </summary>
	// Token: 0x0200009D RID: 157
	public interface ISaveSchema
	{
		/// <summary>
		/// The version of the schema, used for migration purposes.
		/// </summary>
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000527 RID: 1319
		// (set) Token: 0x06000528 RID: 1320
		[JsonPropertyName("schema_version")]
		int SchemaVersion { get; set; }
	}
}
