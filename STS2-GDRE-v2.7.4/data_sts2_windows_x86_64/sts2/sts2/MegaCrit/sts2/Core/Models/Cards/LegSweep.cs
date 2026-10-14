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
	// Token: 0x02000A28 RID: 2600
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LegSweep : CardModel
	{
		// Token: 0x060073D0 RID: 29648 RVA: 0x00290237 File Offset: 0x0028E437
		public LegSweep()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001EE4 RID: 7908
		// (get) Token: 0x060073D1 RID: 29649 RVA: 0x00290244 File Offset: 0x0028E444
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EE5 RID: 7909
		// (get) Token: 0x060073D2 RID: 29650 RVA: 0x00290248 File Offset: 0x0028E448
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x17001EE6 RID: 7910
		// (get) Token: 0x060073D3 RID: 29651 RVA: 0x00290268 File Offset: 0x0028E468
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(11m, ValueProp.Move),
					new PowerVar<WeakPower>(2m)
				});
			}
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x00290294 File Offset: 0x0028E494
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x002902E7 File Offset: 0x0028E4E7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
			base.DynamicVars.Weak.UpgradeValueBy(1m);
		}
	}
}
