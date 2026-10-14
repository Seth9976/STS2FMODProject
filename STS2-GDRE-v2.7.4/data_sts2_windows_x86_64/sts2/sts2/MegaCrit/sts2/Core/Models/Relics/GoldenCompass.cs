using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200055A RID: 1370
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoldenCompass : RelicModel
	{
		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x00240AB7 File Offset: 0x0023ECB7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005143 RID: 20803 RVA: 0x00240ABA File Offset: 0x0023ECBA
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x00240ABD File Offset: 0x0023ECBD
		// (set) Token: 0x06005145 RID: 20805 RVA: 0x00240AC5 File Offset: 0x0023ECC5
		[SavedProperty]
		public int GoldenPathAct
		{
			get
			{
				return this._goldenPathAct;
			}
			set
			{
				base.AssertMutable();
				this._goldenPathAct = value;
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00240AD4 File Offset: 0x0023ECD4
		public override async Task AfterObtained()
		{
			this.GoldenPathAct = base.Owner.RunState.CurrentActIndex;
			await RunManager.Instance.GenerateMap();
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00240B17 File Offset: 0x0023ED17
		public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
		{
			if (this.GoldenPathAct != actIndex)
			{
				return map;
			}
			return new GoldenPathActMap(runState);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x00240B2C File Offset: 0x0023ED2C
		public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(IReadOnlySet<RoomType> roomTypes)
		{
			if (this.GoldenPathAct != base.Owner.RunState.CurrentActIndex)
			{
				return roomTypes;
			}
			return new HashSet<RoomType> { RoomType.Event };
		}

		// Token: 0x04002457 RID: 9303
		private int _goldenPathAct = -1;
	}
}
