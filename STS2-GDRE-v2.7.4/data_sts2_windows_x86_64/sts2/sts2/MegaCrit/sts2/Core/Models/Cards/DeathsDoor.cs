using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200097B RID: 2427
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeathsDoor : CardModel
	{
		// Token: 0x06007021 RID: 28705 RVA: 0x00288DC8 File Offset: 0x00286FC8
		public DeathsDoor()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D46 RID: 7494
		// (get) Token: 0x06007022 RID: 28706 RVA: 0x00288DD5 File Offset: 0x00286FD5
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x06007023 RID: 28707 RVA: 0x00288DD8 File Offset: 0x00286FD8
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.WasDoomAppliedThisTurn;
			}
		}

		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x06007024 RID: 28708 RVA: 0x00288DE0 File Offset: 0x00286FE0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(6m, ValueProp.Move),
					new RepeatVar(2)
				});
			}
		}

		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x06007025 RID: 28709 RVA: 0x00288E08 File Offset: 0x00287008
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06007026 RID: 28710 RVA: 0x00288E28 File Offset: 0x00287028
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int blockGains = 1;
			if (this.WasDoomAppliedThisTurn)
			{
				blockGains += base.DynamicVars.Repeat.IntValue;
			}
			for (int i = 0; i < blockGains; i++)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x00288E73 File Offset: 0x00287073
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(1m);
		}

		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x06007028 RID: 28712 RVA: 0x00288E8A File Offset: 0x0028708A
		private bool WasDoomAppliedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>().Any<PowerReceivedEntry>((PowerReceivedEntry e) => e.HappenedThisTurn(base.CombatState) && e.Power is DoomPower && e.Applier == base.Owner.Creature);
			}
		}
	}
}
