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
	// Token: 0x02000B12 RID: 2834
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheSmith : CardModel
	{
		// Token: 0x060078D0 RID: 30928 RVA: 0x0029A9FD File Offset: 0x00298BFD
		public TheSmith()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020EF RID: 8431
		// (get) Token: 0x060078D1 RID: 30929 RVA: 0x0029AA0A File Offset: 0x00298C0A
		public override int CanonicalStarCost
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170020F0 RID: 8432
		// (get) Token: 0x060078D2 RID: 30930 RVA: 0x0029AA0D File Offset: 0x00298C0D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(30));
			}
		}

		// Token: 0x170020F1 RID: 8433
		// (get) Token: 0x060078D3 RID: 30931 RVA: 0x0029AA1B File Offset: 0x00298C1B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x0029AA24 File Offset: 0x00298C24
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x0029AA67 File Offset: 0x00298C67
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(10m);
		}
	}
}
