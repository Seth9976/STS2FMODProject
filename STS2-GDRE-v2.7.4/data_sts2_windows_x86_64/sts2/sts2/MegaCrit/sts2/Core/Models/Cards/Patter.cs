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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A6D RID: 2669
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Patter : CardModel
	{
		// Token: 0x06007554 RID: 30036 RVA: 0x0029353E File Offset: 0x0029173E
		public Patter()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001F87 RID: 8071
		// (get) Token: 0x06007555 RID: 30037 RVA: 0x0029354B File Offset: 0x0029174B
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F88 RID: 8072
		// (get) Token: 0x06007556 RID: 30038 RVA: 0x0029354E File Offset: 0x0029174E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(8m, ValueProp.Move),
					new PowerVar<VigorPower>(2m)
				});
			}
		}

		// Token: 0x17001F89 RID: 8073
		// (get) Token: 0x06007557 RID: 30039 RVA: 0x00293578 File Offset: 0x00291778
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VigorPower>(null));
			}
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x00293598 File Offset: 0x00291798
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<VigorPower>(choiceContext, base.Owner.Creature, base.DynamicVars["VigorPower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x002935EB File Offset: 0x002917EB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
			base.DynamicVars["VigorPower"].UpgradeValueBy(1m);
		}
	}
}
