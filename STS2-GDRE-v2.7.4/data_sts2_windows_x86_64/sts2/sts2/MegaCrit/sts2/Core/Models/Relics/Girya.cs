using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000556 RID: 1366
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Girya : RelicModel
	{
		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x0600512B RID: 20779 RVA: 0x0024085D File Offset: 0x0023EA5D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00240860 File Offset: 0x0023EA60
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x0600512D RID: 20781 RVA: 0x00240868 File Offset: 0x0023EA68
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0024086B File Offset: 0x0023EA6B
		public override int DisplayAmount
		{
			get
			{
				return this.TimesLifted;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x00240873 File Offset: 0x0023EA73
		// (set) Token: 0x06005130 RID: 20784 RVA: 0x0024087B File Offset: 0x0023EA7B
		[SavedProperty]
		public int TimesLifted
		{
			get
			{
				return this._timesLifted;
			}
			set
			{
				base.AssertMutable();
				this._timesLifted = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005131 RID: 20785 RVA: 0x00240890 File Offset: 0x0023EA90
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x002408B0 File Offset: 0x0023EAB0
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (this.TimesLifted > 0)
			{
				if (room is CombatRoom)
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, this.TimesLifted, base.Owner.Creature, null, false);
				}
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x002408FB File Offset: 0x0023EAFB
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (this.TimesLifted >= 3)
			{
				return false;
			}
			options.Add(new LiftRestSiteOption(player));
			return true;
		}

		// Token: 0x04002454 RID: 9300
		private int _timesLifted;

		// Token: 0x04002455 RID: 9301
		public const int maxLifts = 3;
	}
}
