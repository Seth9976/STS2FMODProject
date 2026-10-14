using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A2D RID: 2605
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Luminesce : CardModel
	{
		// Token: 0x060073EB RID: 29675 RVA: 0x00290583 File Offset: 0x0028E783
		public Luminesce()
			: base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true)
		{
		}

		// Token: 0x17001EF0 RID: 7920
		// (get) Token: 0x060073EC RID: 29676 RVA: 0x00290590 File Offset: 0x0028E790
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001EF1 RID: 7921
		// (get) Token: 0x060073ED RID: 29677 RVA: 0x0029059D File Offset: 0x0028E79D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001EF2 RID: 7922
		// (get) Token: 0x060073EE RID: 29678 RVA: 0x002905AA File Offset: 0x0028E7AA
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Exhaust,
					CardKeyword.Retain
				});
			}
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x002905C0 File Offset: 0x0028E7C0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x00290603 File Offset: 0x0028E803
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
