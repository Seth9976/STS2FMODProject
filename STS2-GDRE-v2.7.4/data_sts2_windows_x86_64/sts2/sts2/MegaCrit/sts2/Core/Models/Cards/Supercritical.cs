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
	// Token: 0x02000AFA RID: 2810
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Supercritical : CardModel
	{
		// Token: 0x0600784A RID: 30794 RVA: 0x0029981B File Offset: 0x00297A1B
		public Supercritical()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020BA RID: 8378
		// (get) Token: 0x0600784B RID: 30795 RVA: 0x00299828 File Offset: 0x00297A28
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x170020BB RID: 8379
		// (get) Token: 0x0600784C RID: 30796 RVA: 0x00299830 File Offset: 0x00297A30
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x170020BC RID: 8380
		// (get) Token: 0x0600784D RID: 30797 RVA: 0x0029983D File Offset: 0x00297A3D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(4));
			}
		}

		// Token: 0x0600784E RID: 30798 RVA: 0x0029984C File Offset: 0x00297A4C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x0029988F File Offset: 0x00297A8F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(2m);
		}
	}
}
