using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Entities.Players
{
	/// <summary>
	/// Extra fields that are used for specific pieces of content per-player.
	/// </summary>
	// Token: 0x02000C7C RID: 3196
	[NullableContext(1)]
	[Nullable(0)]
	public class ExtraPlayerFields
	{
		/// <summary>
		/// The number of times the player has removed a card from their deck in the card shop.
		/// The card removal price increases each time, so we need to track this across the whole run.
		/// </summary>
		// Token: 0x170023AF RID: 9135
		// (get) Token: 0x060082A6 RID: 33446 RVA: 0x002C213D File Offset: 0x002C033D
		// (set) Token: 0x060082A7 RID: 33447 RVA: 0x002C2145 File Offset: 0x002C0345
		public int CardShopRemovalsUsed { get; set; }

		/// <summary>
		/// Points earned in the <see cref="T:MegaCrit.Sts2.Core.Models.Events.WelcomeToWongos" /> event.
		/// We can't save these immediately to SerializableProgress because, otherwise, the player would be able to exit the client,
		/// reload the event, and accumulate more points. We can't write to SerializableProgress and wait for something else to save
		/// it, because it only saves at the end of a run. So, we have to save this to the run data and let SaveManager
		/// accumulate the points into SerializableProgress at the end of the run.
		/// </summary>
		// Token: 0x170023B0 RID: 9136
		// (get) Token: 0x060082A8 RID: 33448 RVA: 0x002C214E File Offset: 0x002C034E
		// (set) Token: 0x060082A9 RID: 33449 RVA: 0x002C2156 File Offset: 0x002C0356
		public int WongoPoints { get; set; }

		/// <summary>
		/// Whether or not the player unlocked the CCCCombo badge.
		/// </summary>
		// Token: 0x170023B1 RID: 9137
		// (get) Token: 0x060082AA RID: 33450 RVA: 0x002C215F File Offset: 0x002C035F
		// (set) Token: 0x060082AB RID: 33451 RVA: 0x002C2167 File Offset: 0x002C0367
		public bool CccomboBadgeUnlocked { get; set; }

		/// <summary>
		/// How much damage the player dealt, aggregated among all combats.
		/// </summary>
		// Token: 0x170023B2 RID: 9138
		// (get) Token: 0x060082AC RID: 33452 RVA: 0x002C2170 File Offset: 0x002C0370
		// (set) Token: 0x060082AD RID: 33453 RVA: 0x002C2178 File Offset: 0x002C0378
		public int DamageDealt { get; set; }

		/// <summary>
		/// How many debuffs the player applied to enemies, aggregated among all combats.
		/// </summary>
		// Token: 0x170023B3 RID: 9139
		// (get) Token: 0x060082AE RID: 33454 RVA: 0x002C2181 File Offset: 0x002C0381
		// (set) Token: 0x060082AF RID: 33455 RVA: 0x002C2189 File Offset: 0x002C0389
		public int DebuffsApplied { get; set; }

		// Token: 0x060082B0 RID: 33456 RVA: 0x002C2194 File Offset: 0x002C0394
		public SerializableExtraPlayerFields ToSerializable()
		{
			return new SerializableExtraPlayerFields
			{
				CardShopRemovalsUsed = this.CardShopRemovalsUsed,
				WongoPoints = this.WongoPoints,
				CccomboBadgeUnlocked = this.CccomboBadgeUnlocked,
				DamageDealt = this.DamageDealt,
				DebuffsApplied = this.DebuffsApplied
			};
		}

		// Token: 0x060082B1 RID: 33457 RVA: 0x002C21E4 File Offset: 0x002C03E4
		public static ExtraPlayerFields FromSerializable(SerializableExtraPlayerFields save)
		{
			return new ExtraPlayerFields
			{
				CardShopRemovalsUsed = save.CardShopRemovalsUsed,
				WongoPoints = save.WongoPoints,
				CccomboBadgeUnlocked = save.CccomboBadgeUnlocked,
				DamageDealt = save.DamageDealt,
				DebuffsApplied = save.DebuffsApplied
			};
		}
	}
}
