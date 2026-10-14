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
	/// <summary>
	/// This power doesn't actually do anything on its own. Instead, Frost orbs check for this power and change their
	/// behavior based off of that
	/// </summary>
	// Token: 0x02000695 RID: 1685
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HibernatePower : PowerModel
	{
		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x060058F2 RID: 22770 RVA: 0x0024F95F File Offset: 0x0024DB5F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x060058F3 RID: 22771 RVA: 0x0024F962 File Offset: 0x0024DB62
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x060058F4 RID: 22772 RVA: 0x0024F965 File Offset: 0x0024DB65
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromOrb<FrostOrb>(),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x0024F988 File Offset: 0x0024DB88
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player.Creature == base.Owner)
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
