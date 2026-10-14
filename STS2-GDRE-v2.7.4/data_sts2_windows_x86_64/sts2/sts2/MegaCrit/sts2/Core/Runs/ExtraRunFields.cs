using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Extra fields that are used for specific pieces of content within a run.
	/// </summary>
	// Token: 0x02000102 RID: 258
	[NullableContext(1)]
	[Nullable(0)]
	public class ExtraRunFields
	{
		/// <summary>
		/// Whether or not the player started the run at the <see cref="T:MegaCrit.Sts2.Core.Models.Events.Neow" /> Ancient.
		/// This is used to determine whether or not to show the first node in Act 1 as Neow when loading from a save.
		/// If the player starts a singleplayer run, then unlocks the Neow epoch in multiplayer and places it, we don't want
		/// to generate the map upon load with Neow in it.
		/// </summary>
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0004C483 File Offset: 0x0004A683
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x0004C48B File Offset: 0x0004A68B
		public bool StartedWithNeow { get; set; }

		/// <summary>
		/// Number of times the Test Subject was killed in this run. We need to save this so that it gets saved to the
		/// SerializableProgress at the end of the run.
		/// </summary>
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0004C494 File Offset: 0x0004A694
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x0004C49C File Offset: 0x0004A69C
		public int TestSubjectKills { get; set; }

		/// <summary>
		/// Whether or not the player went to the <see cref="T:MegaCrit.Sts2.Core.Models.Events.WarHistorianRepy" /> event and freed Repy in this run.
		/// This is then used to determine wither to show Repy in the background of the <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.QueenBoss" /> encounter.
		/// </summary>
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0004C4A5 File Offset: 0x0004A6A5
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x0004C4AD File Offset: 0x0004A6AD
		public bool FreedRepy { get; set; }

		// Token: 0x06000B39 RID: 2873 RVA: 0x0004C4B6 File Offset: 0x0004A6B6
		public SerializableExtraRunFields ToSerializable()
		{
			return new SerializableExtraRunFields
			{
				StartedWithNeow = this.StartedWithNeow,
				TestSubjectKills = this.TestSubjectKills,
				FreedRepy = this.FreedRepy
			};
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0004C4E1 File Offset: 0x0004A6E1
		public static ExtraRunFields FromSerializable(SerializableExtraRunFields save)
		{
			return new ExtraRunFields
			{
				StartedWithNeow = save.StartedWithNeow,
				TestSubjectKills = save.TestSubjectKills,
				FreedRepy = save.FreedRepy
			};
		}
	}
}
