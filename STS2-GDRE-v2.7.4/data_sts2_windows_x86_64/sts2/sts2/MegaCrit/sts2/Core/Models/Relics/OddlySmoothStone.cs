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
	// Token: 0x0200059C RID: 1436
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OddlySmoothStone : RelicModel
	{
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x00243CE0 File Offset: 0x00241EE0
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060052EE RID: 21230 RVA: 0x00243CE3 File Offset: 0x00241EE3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(1m));
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060052EF RID: 21231 RVA: 0x00243CF4 File Offset: 0x00241EF4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x00243D14 File Offset: 0x00241F14
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				base.Flash();
				await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
			}
		}
	}
}
