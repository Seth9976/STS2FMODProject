using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000831 RID: 2097
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrystalSphere : EventModel
	{
		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x060067A2 RID: 26530 RVA: 0x002736B8 File Offset: 0x002718B8
		public override bool IsDeterministic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060067A3 RID: 26531 RVA: 0x002736BB File Offset: 0x002718BB
		public override void CalculateVars()
		{
			base.DynamicVars["UncoverFutureCost"].BaseValue += base.Rng.NextInt(1, 50);
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x002736F0 File Offset: 0x002718F0
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= 100) && runState.CurrentActIndex > 0;
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x060067A5 RID: 26533 RVA: 0x0027372C File Offset: 0x0027192C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("UncoverFutureCost", 50m),
					new DynamicVar("UncoverFutureProphesizeCount", 3m),
					new DynamicVar("PaymentPlanCount", 6m),
					new StringVar("CurseTitle", ModelDb.Card<Debt>().Title)
				});
			}
		}

		// Token: 0x060067A6 RID: 26534 RVA: 0x00273798 File Offset: 0x00271998
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.UncoverFuture), "CRYSTAL_SPHERE.pages.INITIAL.options.UNCOVER_FUTURE", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.PaymentPlan), "CRYSTAL_SPHERE.pages.INITIAL.options.PAYMENT_PLAN", HoverTipFactory.FromCardWithCardHoverTips<Debt>(false))
			});
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x002737F0 File Offset: 0x002719F0
		private async Task UncoverFuture()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["UncoverFutureCost"].BaseValue, base.Owner, GoldLossType.Spent);
			CrystalSphereMinigame crystalSphereMinigame = new CrystalSphereMinigame(base.Owner, base.Rng, 3);
			await crystalSphereMinigame.PlayMinigame();
			base.SetEventFinished(base.L10NLookup("CRYSTAL_SPHERE.pages.FINISH.description"));
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x00273834 File Offset: 0x00271A34
		private async Task PaymentPlan()
		{
			await CardPileCmd.AddCurseToDeck<Debt>(base.Owner);
			CrystalSphereMinigame crystalSphereMinigame = new CrystalSphereMinigame(base.Owner, base.Rng, 6);
			await crystalSphereMinigame.PlayMinigame();
			base.SetEventFinished(base.L10NLookup("CRYSTAL_SPHERE.pages.FINISH.description"));
		}

		// Token: 0x04002762 RID: 10082
		private const string _uncoverFutureCostKey = "UncoverFutureCost";

		// Token: 0x04002763 RID: 10083
		private const string _uncoverFutureProphesizeKey = "UncoverFutureProphesizeCount";

		// Token: 0x04002764 RID: 10084
		private const string _paymentPlanKey = "PaymentPlanCount";

		// Token: 0x04002765 RID: 10085
		private const int _uncoverFutureCost = 50;

		// Token: 0x04002766 RID: 10086
		private const int _uncoverFutureRandomMin = 1;

		// Token: 0x04002767 RID: 10087
		private const int _uncoverFutureRandomMax = 50;

		// Token: 0x04002768 RID: 10088
		private const int _uncoverFutureProphesizeCount = 3;

		// Token: 0x04002769 RID: 10089
		private const int _paymentPlanCount = 6;
	}
}
