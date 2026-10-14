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
	// Token: 0x02000961 RID: 2401
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Convergence : CardModel
	{
		// Token: 0x06006FA3 RID: 28579 RVA: 0x00287CF2 File Offset: 0x00285EF2
		public Convergence()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x00287CFF File Offset: 0x00285EFF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new StarsVar(1)
				});
			}
		}

		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x00287D1E File Offset: 0x00285F1E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					base.EnergyHoverTip,
					HoverTipFactory.FromKeyword(CardKeyword.Retain)
				});
			}
		}

		// Token: 0x06006FA6 RID: 28582 RVA: 0x00287D40 File Offset: 0x00285F40
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<RetainHandPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<StarNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Stars.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x00287D8B File Offset: 0x00285F8B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Stars.UpgradeValueBy(1m);
		}
	}
}
