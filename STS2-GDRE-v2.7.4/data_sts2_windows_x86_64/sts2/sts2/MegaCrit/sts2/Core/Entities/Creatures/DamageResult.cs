using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Entities.Creatures
{
	/// <summary>
	/// A class containing a bunch of meta-info about an instance of damage that was dealt to a creature.
	/// See comments on individual properties for more details and examples.
	/// </summary>
	// Token: 0x02000CA2 RID: 3234
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageResult
	{
		/// <summary>
		/// The creature that received the damage.
		/// </summary>
		// Token: 0x17002425 RID: 9253
		// (get) Token: 0x0600845C RID: 33884 RVA: 0x002C9CCD File Offset: 0x002C7ECD
		public Creature Receiver { get; }

		/// <summary>
		/// The value props of the damage that was dealt.
		/// </summary>
		// Token: 0x17002426 RID: 9254
		// (get) Token: 0x0600845D RID: 33885 RVA: 0x002C9CD5 File Offset: 0x002C7ED5
		public ValueProp Props { get; }

		/// <summary>
		/// The amount of damage that was blocked by the target.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 Block. BlockedDamage = 10.
		/// X deals 20 damage to Y, who has 15 Block. BlockedDamage = 15.
		/// X deals 20 damage to Y, who has 20 Block. BlockedDamage = 20.
		/// X deals 20 damage to Y, who has 0 Block.  BlockedDamage = 20.
		/// </example>
		// Token: 0x17002427 RID: 9255
		// (get) Token: 0x0600845E RID: 33886 RVA: 0x002C9CDD File Offset: 0x002C7EDD
		// (set) Token: 0x0600845F RID: 33887 RVA: 0x002C9CE5 File Offset: 0x002C7EE5
		public int BlockedDamage { get; set; }

		/// <summary>
		/// The amount of damage that the target received after blocking all that it could.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 Block. UnblockedDamage = 0.
		/// X deals 20 damage to Y, who has 15 Block. UnblockedDamage = 5.
		/// X deals 20 damage to Y, who has 20 Block. UnblockedDamage = 0.
		/// X deals 20 damage to Y, who has 0 Block.  UnblockedDamage = 20.
		/// </example>
		// Token: 0x17002428 RID: 9256
		// (get) Token: 0x06008460 RID: 33888 RVA: 0x002C9CEE File Offset: 0x002C7EEE
		// (set) Token: 0x06008461 RID: 33889 RVA: 0x002C9CF6 File Offset: 0x002C7EF6
		public int UnblockedDamage { get; set; }

		/// <summary>
		/// If the target was killed, this will be the amount of extra damage that the target received after their HP
		/// dropped to 0.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 HP. OverkillDamage = 0.
		/// X deals 10 damage to Y, who has 10 HP. OverkillDamage = 0.
		/// X deals 10 damage to Y, who has 5 HP.  OverkillDamage = 5.
		/// </example>
		// Token: 0x17002429 RID: 9257
		// (get) Token: 0x06008462 RID: 33890 RVA: 0x002C9CFF File Offset: 0x002C7EFF
		// (set) Token: 0x06008463 RID: 33891 RVA: 0x002C9D07 File Offset: 0x002C7F07
		public int OverkillDamage { get; set; }

		/// <summary>
		/// The total amount of damage that the target received, including both blocked and unblocked.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 Block. TotalDamage = 10.
		/// X deals 20 damage to Y, who has 15 Block. TotalDamage = 20.
		/// X deals 20 damage to Y, who has 20 Block. TotalDamage = 20.
		/// X deals 20 damage to Y, who has 0 Block.  TotalDamage = 20.
		/// </example>
		// Token: 0x1700242A RID: 9258
		// (get) Token: 0x06008464 RID: 33892 RVA: 0x002C9D10 File Offset: 0x002C7F10
		public int TotalDamage
		{
			get
			{
				return this.BlockedDamage + this.UnblockedDamage;
			}
		}

		/// <summary>
		/// Whether the creature's block was broken by this instance of damage.
		/// Note: If possible, you should prefer using the AfterBlockBroken hook instead of checking this, as non-damage
		/// sources like Expose can remove block.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 Block. WasBlockBroken = false.
		/// X deals 20 damage to Y, who has 15 Block. WasBlockBroken = true.
		/// X deals 20 damage to Y, who has 20 Block. WasBlockBroken = true.
		/// X deals 20 damage to Y, who has 0 Block.  WasBlockBroken = false.
		/// </example>
		// Token: 0x1700242B RID: 9259
		// (get) Token: 0x06008465 RID: 33893 RVA: 0x002C9D1F File Offset: 0x002C7F1F
		// (set) Token: 0x06008466 RID: 33894 RVA: 0x002C9D27 File Offset: 0x002C7F27
		public bool WasBlockBroken { get; set; }

		/// <summary>
		/// Whether the entirety of this instance of damage was blocked.
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 Block. WasFullyBlocked = true.
		/// X deals 20 damage to Y, who has 15 Block. WasFullyBlocked = false.
		/// X deals 20 damage to Y, who has 20 Block. WasFullyBlocked = true.
		/// X deals 20 damage to Y, who has 0 Block.  WasFullyBlocked = false.
		/// </example>
		// Token: 0x1700242C RID: 9260
		// (get) Token: 0x06008467 RID: 33895 RVA: 0x002C9D30 File Offset: 0x002C7F30
		// (set) Token: 0x06008468 RID: 33896 RVA: 0x002C9D38 File Offset: 0x002C7F38
		public bool WasFullyBlocked { get; set; }

		/// <summary>
		/// Was the creature that took damage killed by it?
		/// Note: This will return true even if the creature was resurrected afterwards (Fairy in a Bottle, etc.).
		/// </summary>
		/// <example>
		/// X deals 10 damage to Y, who has 15 HP. WasTargetKilled = false.
		/// X deals 20 damage to Y, who has 15 HP. WasTargetKilled = true.
		/// X deals 20 damage to Y, who has 20 HP. WasTargetKilled = true.
		/// X deals 20 damage to Y, who has 20 HP and Fairy in a Bottle. WasTargetKilled = true.
		/// </example>
		// Token: 0x1700242D RID: 9261
		// (get) Token: 0x06008469 RID: 33897 RVA: 0x002C9D41 File Offset: 0x002C7F41
		// (set) Token: 0x0600846A RID: 33898 RVA: 0x002C9D49 File Offset: 0x002C7F49
		public bool WasTargetKilled { get; set; }

		// Token: 0x0600846B RID: 33899 RVA: 0x002C9D52 File Offset: 0x002C7F52
		public DamageResult(Creature receiver, ValueProp props)
		{
			this.Receiver = receiver;
			this.Props = props;
		}
	}
}
