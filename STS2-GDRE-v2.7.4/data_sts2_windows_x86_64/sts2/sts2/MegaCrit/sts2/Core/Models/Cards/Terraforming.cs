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
	// Token: 0x02000B0A RID: 2826
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Terraforming : CardModel
	{
		// Token: 0x0600789C RID: 30876 RVA: 0x0029A37B File Offset: 0x0029857B
		public Terraforming()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020DC RID: 8412
		// (get) Token: 0x0600789D RID: 30877 RVA: 0x0029A388 File Offset: 0x00298588
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VigorPower>(6m));
			}
		}

		// Token: 0x170020DD RID: 8413
		// (get) Token: 0x0600789E RID: 30878 RVA: 0x0029A39C File Offset: 0x0029859C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VigorPower>(null));
			}
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x0029A3BC File Offset: 0x002985BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<VigorPower>(choiceContext, base.Owner.Creature, base.DynamicVars["VigorPower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x0029A407 File Offset: 0x00298607
		protected override void OnUpgrade()
		{
			base.DynamicVars["VigorPower"].UpgradeValueBy(2m);
		}
	}
}
