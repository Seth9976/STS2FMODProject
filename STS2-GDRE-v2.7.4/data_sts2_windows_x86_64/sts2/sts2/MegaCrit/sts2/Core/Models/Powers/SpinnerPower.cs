using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FE RID: 1790
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpinnerPower : PowerModel
	{
		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06005B6C RID: 23404 RVA: 0x0025432F File Offset: 0x0025252F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06005B6D RID: 23405 RVA: 0x00254332 File Offset: 0x00252532
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06005B6E RID: 23406 RVA: 0x00254335 File Offset: 0x00252535
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<GlassOrb>()
				});
			}
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x00254358 File Offset: 0x00252558
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				for (int i = 0; i < base.Amount; i++)
				{
					await OrbCmd.Channel<GlassOrb>(new ThrowingPlayerChoiceContext(), base.Owner.Player);
				}
			}
		}
	}
}
