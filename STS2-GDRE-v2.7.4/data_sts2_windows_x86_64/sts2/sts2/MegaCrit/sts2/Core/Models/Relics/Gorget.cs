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
	// Token: 0x0200055D RID: 1373
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Gorget : RelicModel
	{
		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x00240C20 File Offset: 0x0023EE20
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06005154 RID: 20820 RVA: 0x00240C24 File Offset: 0x0023EE24
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromPowerWithPowerHoverTips<PlatingPower>(null);
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x00240C3F File Offset: 0x0023EE3F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PlatingPower>(4m));
			}
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00240C54 File Offset: 0x0023EE54
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				base.Flash();
				await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, null, false);
			}
		}
	}
}
