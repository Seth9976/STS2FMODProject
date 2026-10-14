using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000863 RID: 2147
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnrestSite : EventModel
	{
		// Token: 0x0600698E RID: 27022 RVA: 0x0027D08C File Offset: 0x0027B28C
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Creature.CurrentHp <= p.Creature.MaxHp * 0.70m);
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x0027D0B8 File Offset: 0x0027B2B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HealVar(0m),
					new DynamicVar("MaxHpLoss", 8m)
				});
			}
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x0027D0E8 File Offset: 0x0027B2E8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Rest), "UNREST_SITE.pages.INITIAL.options.REST", HoverTipFactory.FromCardWithCardHoverTips<PoorSleep>(false)),
				new EventOption(this, new Func<Task>(this.Kill), "UNREST_SITE.pages.INITIAL.options.KILL", Array.Empty<IHoverTip>()).ThatDecreasesMaxHp(base.DynamicVars["MaxHpLoss"].BaseValue)
			});
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x0027D159 File Offset: 0x0027B359
		public override void CalculateVars()
		{
			base.DynamicVars.Heal.BaseValue = base.Owner.Creature.MaxHp - base.Owner.Creature.CurrentHp;
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x0027D194 File Offset: 0x0027B394
		private async Task Rest()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
			await CardPileCmd.AddCursesToDeck(new <>z__ReadOnlySingleElementList<CardModel>(ModelDb.Card<PoorSleep>()), base.Owner);
			base.SetEventFinished(base.L10NLookup("UNREST_SITE.pages.REST.description"));
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x0027D1D8 File Offset: 0x0027B3D8
		private async Task Kill()
		{
			await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["MaxHpLoss"].BaseValue, false);
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("UNREST_SITE.pages.KILL.description"));
		}

		// Token: 0x040027E5 RID: 10213
		private const string _maxHpLossKey = "MaxHpLoss";
	}
}
