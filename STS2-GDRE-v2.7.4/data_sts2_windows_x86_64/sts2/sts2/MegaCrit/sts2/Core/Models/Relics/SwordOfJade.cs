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
	// Token: 0x020005F2 RID: 1522
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwordOfJade : RelicModel
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06005543 RID: 21827 RVA: 0x0024845B File Offset: 0x0024665B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06005544 RID: 21828 RVA: 0x0024845E File Offset: 0x0024665E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(3m));
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06005545 RID: 21829 RVA: 0x00248470 File Offset: 0x00246670
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00248490 File Offset: 0x00246690
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, null, null, false);
			}
		}
	}
}
