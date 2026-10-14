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
	// Token: 0x02000A9A RID: 2714
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reflect : CardModel
	{
		// Token: 0x06007637 RID: 30263 RVA: 0x00295403 File Offset: 0x00293603
		public Reflect()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FE0 RID: 8160
		// (get) Token: 0x06007638 RID: 30264 RVA: 0x00295410 File Offset: 0x00293610
		public override int CanonicalStarCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001FE1 RID: 8161
		// (get) Token: 0x06007639 RID: 30265 RVA: 0x00295413 File Offset: 0x00293613
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001FE2 RID: 8162
		// (get) Token: 0x0600763A RID: 30266 RVA: 0x00295416 File Offset: 0x00293616
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(15m, ValueProp.Move));
			}
		}

		// Token: 0x0600763B RID: 30267 RVA: 0x0029542C File Offset: 0x0029362C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<ReflectPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x0600763C RID: 30268 RVA: 0x0029547F File Offset: 0x0029367F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(5m);
		}
	}
}
