using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CC RID: 1484
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegalPillow : RelicModel
	{
		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x0600545A RID: 21594 RVA: 0x0024668F File Offset: 0x0024488F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x0600545B RID: 21595 RVA: 0x00246692 File Offset: 0x00244892
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(15m));
			}
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x002466A5 File Offset: 0x002448A5
		public override decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
		{
			if (creature.Player != base.Owner && creature.PetOwner != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Heal.BaseValue;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x002466DB File Offset: 0x002448DB
		public override Task AfterRestSiteHeal(Player player, bool isMimicked)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x00246700 File Offset: 0x00244900
		public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
		{
			if (!LocalContext.IsMe(base.Owner))
			{
				return currentExtraText;
			}
			int num = 0;
			LocString[] array = new LocString[1 + currentExtraText.Count];
			foreach (LocString locString in currentExtraText)
			{
				array[num] = locString;
				num++;
			}
			array[num] = base.AdditionalRestSiteHealText;
			return new <>z__ReadOnlyArray<LocString>(array);
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x0024677C File Offset: 0x0024497C
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			base.Status = ((room is RestSiteRoom) ? RelicStatus.Active : RelicStatus.Normal);
			return Task.CompletedTask;
		}
	}
}
