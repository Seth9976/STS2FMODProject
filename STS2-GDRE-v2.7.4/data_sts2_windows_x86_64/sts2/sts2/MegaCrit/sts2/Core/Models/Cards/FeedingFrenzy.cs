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
	// Token: 0x020009BC RID: 2492
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FeedingFrenzy : CardModel
	{
		// Token: 0x0600718F RID: 29071 RVA: 0x0028B864 File Offset: 0x00289A64
		public FeedingFrenzy()
			: base(0, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001DF4 RID: 7668
		// (get) Token: 0x06007190 RID: 29072 RVA: 0x0028B871 File Offset: 0x00289A71
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(5m));
			}
		}

		// Token: 0x17001DF5 RID: 7669
		// (get) Token: 0x06007191 RID: 29073 RVA: 0x0028B884 File Offset: 0x00289A84
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06007192 RID: 29074 RVA: 0x0028B8A4 File Offset: 0x00289AA4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			decimal baseValue = base.DynamicVars.Strength.BaseValue;
			await PowerCmd.Apply<FeedingFrenzyPower>(choiceContext, base.Owner.Creature, baseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x0028B8EF File Offset: 0x00289AEF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Strength.UpgradeValueBy(2m);
		}
	}
}
