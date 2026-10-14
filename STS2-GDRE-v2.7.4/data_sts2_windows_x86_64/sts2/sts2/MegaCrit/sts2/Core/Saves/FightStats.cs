using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data of a specific Character's matchup against a specific Encounter or Enemy.
	/// </summary>
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class FightStats
	{
		/// <summary>
		/// The character this FightStat entry is for.
		/// </summary>
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000170C7 File Offset: 0x000152C7
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x000170CF File Offset: 0x000152CF
		[RequiredMember]
		[JsonPropertyName("character")]
		public ModelId Character { get; set; }

		/// <summary>
		/// How many times this character has won against this encounter or enemy.
		/// </summary>
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x000170D8 File Offset: 0x000152D8
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x000170E0 File Offset: 0x000152E0
		[JsonPropertyName("wins")]
		public int Wins { get; set; }

		/// <summary>
		/// How many times this character has lost to this encounter or enemy.
		/// </summary>
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000170E9 File Offset: 0x000152E9
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x000170F1 File Offset: 0x000152F1
		[JsonPropertyName("losses")]
		public int Losses { get; set; }

		// Token: 0x060004F5 RID: 1269 RVA: 0x000170FA File Offset: 0x000152FA
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public FightStats()
		{
		}
	}
}
