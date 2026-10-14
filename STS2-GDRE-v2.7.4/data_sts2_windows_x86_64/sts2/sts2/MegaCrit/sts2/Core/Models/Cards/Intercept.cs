using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A15 RID: 2581
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Intercept : CardModel
	{
		// Token: 0x06007372 RID: 29554 RVA: 0x0028F598 File Offset: 0x0028D798
		public Intercept()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001EC1 RID: 7873
		// (get) Token: 0x06007373 RID: 29555 RVA: 0x0028F5A5 File Offset: 0x0028D7A5
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001EC2 RID: 7874
		// (get) Token: 0x06007374 RID: 29556 RVA: 0x0028F5A8 File Offset: 0x0028D7A8
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EC3 RID: 7875
		// (get) Token: 0x06007375 RID: 29557 RVA: 0x0028F5AB File Offset: 0x0028D7AB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x06007376 RID: 29558 RVA: 0x0028F5C0 File Offset: 0x0028D7C0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<CoveredPower>(choiceContext, cardPlay.Target, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007377 RID: 29559 RVA: 0x0028F613 File Offset: 0x0028D813
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}
	}
}
