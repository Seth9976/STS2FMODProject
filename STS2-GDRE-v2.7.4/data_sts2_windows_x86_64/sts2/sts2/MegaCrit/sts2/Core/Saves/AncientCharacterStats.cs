using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data of a specific character's statistics with an ancient.
	/// </summary>
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class AncientCharacterStats
	{
		/// <summary>
		/// The character this stats entry is for.
		/// </summary>
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00015EC5 File Offset: 0x000140C5
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x00015ECD File Offset: 0x000140CD
		[RequiredMember]
		[JsonPropertyName("character")]
		public ModelId Character { get; set; }

		/// <summary>
		/// How many times this character has won a run after encountering this ancient.
		/// </summary>
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00015ED6 File Offset: 0x000140D6
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x00015EDE File Offset: 0x000140DE
		[JsonPropertyName("wins")]
		public int Wins { get; set; }

		/// <summary>
		/// How many times this character has lost a run after encountering this ancient.
		/// </summary>
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00015EE7 File Offset: 0x000140E7
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x00015EEF File Offset: 0x000140EF
		[JsonPropertyName("losses")]
		public int Losses { get; set; }

		/// <summary>
		/// How many times this character has visited this ancient.
		/// </summary>
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00015EF8 File Offset: 0x000140F8
		[JsonIgnore]
		public int Visits
		{
			get
			{
				return this.Wins + this.Losses;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00015F07 File Offset: 0x00014107
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public AncientCharacterStats()
		{
		}
	}
}
