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
	// Token: 0x020006A7 RID: 1703
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LightningRodPower : PowerModel
	{
		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06005963 RID: 22883 RVA: 0x00250753 File Offset: 0x0024E953
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06005964 RID: 22884 RVA: 0x00250756 File Offset: 0x0024E956
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06005965 RID: 22885 RVA: 0x00250759 File Offset: 0x0024E959
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		/// <remarks>
		/// We do this in AfterEnergyReset instead of BeforeSideTurnStart so the player will still get benefits from orbs
		/// that might be evoked to make room for the new Lightning Orb. Specifically:
		///
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Orbs.PlasmaOrb" />'s energy gain could be lost to energy reset if this triggered earlier.
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Orbs.FrostOrb" />'s block gain could be lost to block clear if this triggered earlier.
		/// </remarks>
		// Token: 0x06005966 RID: 22886 RVA: 0x0025077C File Offset: 0x0024E97C
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				await OrbCmd.Channel<LightningOrb>(new ThrowingPlayerChoiceContext(), base.Owner.Player);
				await PowerCmd.Decrement(this);
			}
		}
	}
}
