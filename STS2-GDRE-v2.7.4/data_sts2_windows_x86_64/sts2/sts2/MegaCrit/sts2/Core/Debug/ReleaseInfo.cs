using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Represents release information for the game.
	/// </summary>
	// Token: 0x02000D12 RID: 3346
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class ReleaseInfo
	{
		/// <summary>
		/// The commit hash.
		/// </summary>
		// Token: 0x1700252B RID: 9515
		// (get) Token: 0x06008701 RID: 34561 RVA: 0x002D6330 File Offset: 0x002D4530
		// (set) Token: 0x06008702 RID: 34562 RVA: 0x002D6338 File Offset: 0x002D4538
		[RequiredMember]
		[JsonPropertyName("commit")]
		public string Commit { get; set; }

		/// <summary>
		/// The release version number.
		/// </summary>
		// Token: 0x1700252C RID: 9516
		// (get) Token: 0x06008703 RID: 34563 RVA: 0x002D6341 File Offset: 0x002D4541
		// (set) Token: 0x06008704 RID: 34564 RVA: 0x002D6349 File Offset: 0x002D4549
		[RequiredMember]
		[JsonPropertyName("version")]
		public string Version { get; set; }

		/// <summary>
		/// The release date in git's iso8601 format
		/// </summary>
		// Token: 0x1700252D RID: 9517
		// (get) Token: 0x06008705 RID: 34565 RVA: 0x002D6352 File Offset: 0x002D4552
		// (set) Token: 0x06008706 RID: 34566 RVA: 0x002D635A File Offset: 0x002D455A
		[RequiredMember]
		[JsonPropertyName("date")]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime Date { get; set; }

		/// <summary>
		/// The branch name.
		/// </summary>
		// Token: 0x1700252E RID: 9518
		// (get) Token: 0x06008707 RID: 34567 RVA: 0x002D6363 File Offset: 0x002D4563
		// (set) Token: 0x06008708 RID: 34568 RVA: 0x002D636B File Offset: 0x002D456B
		[RequiredMember]
		[JsonPropertyName("branch")]
		public string Branch { get; set; }

		/// <summary>
		/// A hash of the contents of the main STS2 assembly, at build-time.
		/// </summary>
		// Token: 0x1700252F RID: 9519
		// (get) Token: 0x06008709 RID: 34569 RVA: 0x002D6374 File Offset: 0x002D4574
		// (set) Token: 0x0600870A RID: 34570 RVA: 0x002D637C File Offset: 0x002D457C
		[RequiredMember]
		[JsonPropertyName("main_assembly_hash")]
		public int MainAssemblyHash { get; set; }

		// Token: 0x0600870B RID: 34571 RVA: 0x002D6385 File Offset: 0x002D4585
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public ReleaseInfo()
		{
		}
	}
}
