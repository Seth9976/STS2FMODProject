using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E4 RID: 1508
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlingOfCourage : RelicModel
	{
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x060054F2 RID: 21746 RVA: 0x00247AB4 File Offset: 0x00245CB4
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x060054F3 RID: 21747 RVA: 0x00247AB7 File Offset: 0x00245CB7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(2m));
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x060054F4 RID: 21748 RVA: 0x00247ACC File Offset: 0x00245CCC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x00247AEC File Offset: 0x00245CEC
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room.RoomType == RoomType.Elite)
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
			}
		}
	}
}
