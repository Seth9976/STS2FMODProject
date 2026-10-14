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
	// Token: 0x02000513 RID: 1299
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BronzeScales : RelicModel
	{
		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06004FA0 RID: 20384 RVA: 0x0023DBEB File Offset: 0x0023BDEB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06004FA1 RID: 20385 RVA: 0x0023DBEE File Offset: 0x0023BDEE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ThornsPower>(3m));
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06004FA2 RID: 20386 RVA: 0x0023DC00 File Offset: 0x0023BE00
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ThornsPower>(null));
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x0023DC20 File Offset: 0x0023BE20
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				base.Flash();
				await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, null, false);
			}
		}
	}
}
