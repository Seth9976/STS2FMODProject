using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A68 RID: 2664
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Panache : CardModel
	{
		// Token: 0x06007539 RID: 30009 RVA: 0x00293216 File Offset: 0x00291416
		public Panache()
			: base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F7C RID: 8060
		// (get) Token: 0x0600753A RID: 30010 RVA: 0x00293223 File Offset: 0x00291423
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("PanacheDamage", 10m));
			}
		}

		// Token: 0x0600753B RID: 30011 RVA: 0x0029323C File Offset: 0x0029143C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PanachePower>(choiceContext, base.Owner.Creature, base.DynamicVars["PanacheDamage"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x00293287 File Offset: 0x00291487
		protected override void OnUpgrade()
		{
			base.DynamicVars["PanacheDamage"].UpgradeValueBy(4m);
		}

		// Token: 0x040028AC RID: 10412
		private const string _powerKey = "PanacheDamage";
	}
}
