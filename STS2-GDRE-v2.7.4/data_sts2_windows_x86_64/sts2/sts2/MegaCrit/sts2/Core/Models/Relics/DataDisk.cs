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
	// Token: 0x02000528 RID: 1320
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DataDisk : RelicModel
	{
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005015 RID: 20501 RVA: 0x0023E89F File Offset: 0x0023CA9F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x0023E8A2 File Offset: 0x0023CAA2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FocusPower>(1m));
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x0023E8B4 File Offset: 0x0023CAB4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x0023E8D4 File Offset: 0x0023CAD4
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				base.Flash();
				await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, null, false);
			}
		}
	}
}
