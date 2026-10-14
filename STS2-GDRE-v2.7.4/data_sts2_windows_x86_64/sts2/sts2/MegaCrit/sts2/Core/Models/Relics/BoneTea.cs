using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000509 RID: 1289
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoneTea : RelicModel
	{
		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06004F52 RID: 20306 RVA: 0x0023D1C5 File Offset: 0x0023B3C5
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06004F53 RID: 20307 RVA: 0x0023D1C8 File Offset: 0x0023B3C8
		public override bool IsUsedUp
		{
			get
			{
				return this.CombatsLeft <= 0;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06004F54 RID: 20308 RVA: 0x0023D1D6 File Offset: 0x0023B3D6
		public override bool ShowCounter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06004F55 RID: 20309 RVA: 0x0023D1D9 File Offset: 0x0023B3D9
		public override int DisplayAmount
		{
			get
			{
				return Math.Max(0, this.CombatsLeft);
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06004F56 RID: 20310 RVA: 0x0023D1E7 File Offset: 0x0023B3E7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", this.CombatsLeft));
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06004F57 RID: 20311 RVA: 0x0023D203 File Offset: 0x0023B403
		// (set) Token: 0x06004F58 RID: 20312 RVA: 0x0023D20C File Offset: 0x0023B40C
		[SavedProperty]
		public int CombatsLeft
		{
			get
			{
				return this._combatsLeft;
			}
			set
			{
				base.AssertMutable();
				this._combatsLeft = value;
				base.DynamicVars["Combats"].BaseValue = this._combatsLeft;
				base.InvokeDisplayAmountChanged();
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x0023D25C File Offset: 0x0023B45C
		public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (this.IsUsedUp)
			{
				return Task.CompletedTask;
			}
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (base.Owner.PlayerCombatState.TurnNumber > 1)
			{
				return Task.CompletedTask;
			}
			foreach (CardModel cardModel in PileType.Hand.GetPile(base.Owner).Cards)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			int combatsLeft = this.CombatsLeft;
			this.CombatsLeft = combatsLeft - 1;
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x04002427 RID: 9255
		private const string _combatsKey = "Combats";

		// Token: 0x04002428 RID: 9256
		private int _combatsLeft = 1;
	}
}
