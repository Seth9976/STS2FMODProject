using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D4 RID: 2260
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glam : EnchantmentModel
	{
		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x06006C53 RID: 27731 RVA: 0x00281BA4 File Offset: 0x0027FDA4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Times", 1m));
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x06006C54 RID: 27732 RVA: 0x00281BBA File Offset: 0x0027FDBA
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, new DynamicVar[] { base.DynamicVars["Times"] }));
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x06006C55 RID: 27733 RVA: 0x00281BE1 File Offset: 0x0027FDE1
		// (set) Token: 0x06006C56 RID: 27734 RVA: 0x00281BE9 File Offset: 0x0027FDE9
		private bool UsedThisCombat
		{
			get
			{
				return this._usedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._usedThisCombat = value;
			}
		}

		// Token: 0x06006C57 RID: 27735 RVA: 0x00281BF8 File Offset: 0x0027FDF8
		public override int EnchantPlayCount(int originalPlayCount)
		{
			if (this.UsedThisCombat)
			{
				return originalPlayCount;
			}
			return originalPlayCount + base.DynamicVars["Times"].IntValue;
		}

		// Token: 0x06006C58 RID: 27736 RVA: 0x00281C1B File Offset: 0x0027FE1B
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.UsedThisCombat)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card != base.Card)
			{
				return Task.CompletedTask;
			}
			this.UsedThisCombat = true;
			base.Status = EnchantmentStatus.Disabled;
			return Task.CompletedTask;
		}

		// Token: 0x04002833 RID: 10291
		private const string _timesKey = "Times";

		// Token: 0x04002834 RID: 10292
		private bool _usedThisCombat;
	}
}
