using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F3 RID: 1523
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwordOfStone : RelicModel
	{
		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06005548 RID: 21832 RVA: 0x002484E3 File Offset: 0x002466E3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06005549 RID: 21833 RVA: 0x002484E6 File Offset: 0x002466E6
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x002484E9 File Offset: 0x002466E9
		public override int DisplayAmount
		{
			get
			{
				return this.ElitesDefeated;
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x002484F1 File Offset: 0x002466F1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Elites", 5m));
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x00248508 File Offset: 0x00246708
		// (set) Token: 0x0600554D RID: 21837 RVA: 0x00248510 File Offset: 0x00246710
		[SavedProperty]
		public int ElitesDefeated
		{
			get
			{
				return this._elitesDefeated;
			}
			set
			{
				base.AssertMutable();
				this._elitesDefeated = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x00248528 File Offset: 0x00246728
		public override async Task AfterCombatVictory(CombatRoom room)
		{
			if (room.RoomType == RoomType.Elite)
			{
				int elitesDefeated = this.ElitesDefeated;
				this.ElitesDefeated = elitesDefeated + 1;
				base.Flash();
				if (this.ElitesDefeated >= base.DynamicVars["Elites"].BaseValue)
				{
					await RelicCmd.Replace(this, ModelDb.Relic<SwordOfJade>().ToMutable());
				}
			}
		}

		// Token: 0x040024C5 RID: 9413
		private const string _elitesKey = "Elites";

		// Token: 0x040024C6 RID: 9414
		private int _elitesDefeated;
	}
}
