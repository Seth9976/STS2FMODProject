using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ABD RID: 2749
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shadowmeld : CardModel
	{
		// Token: 0x060076F5 RID: 30453 RVA: 0x00296BBB File Offset: 0x00294DBB
		public Shadowmeld()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002030 RID: 8240
		// (get) Token: 0x060076F6 RID: 30454 RVA: 0x00296BC8 File Offset: 0x00294DC8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 1m));
			}
		}

		// Token: 0x17002031 RID: 8241
		// (get) Token: 0x060076F7 RID: 30455 RVA: 0x00296BDE File Offset: 0x00294DDE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060076F8 RID: 30456 RVA: 0x00296BF0 File Offset: 0x00294DF0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<ShadowmeldPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060076F9 RID: 30457 RVA: 0x00296C3B File Offset: 0x00294E3B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x040028BA RID: 10426
		private const string _powerKey = "Power";
	}
}
