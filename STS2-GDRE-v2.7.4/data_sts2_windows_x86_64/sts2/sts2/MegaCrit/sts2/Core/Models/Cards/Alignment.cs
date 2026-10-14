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
	// Token: 0x020008FB RID: 2299
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Alignment : CardModel
	{
		// Token: 0x06006D99 RID: 28057 RVA: 0x00283A35 File Offset: 0x00281C35
		public Alignment()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x06006D9A RID: 28058 RVA: 0x00283A42 File Offset: 0x00281C42
		public override int CanonicalStarCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x06006D9B RID: 28059 RVA: 0x00283A45 File Offset: 0x00281C45
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x06006D9C RID: 28060 RVA: 0x00283A52 File Offset: 0x00281C52
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x00283A60 File Offset: 0x00281C60
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x00283AA3 File Offset: 0x00281CA3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
