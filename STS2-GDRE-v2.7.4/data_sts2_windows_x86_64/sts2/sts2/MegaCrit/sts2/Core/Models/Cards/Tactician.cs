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
	// Token: 0x02000B04 RID: 2820
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tactician : CardModel
	{
		// Token: 0x0600787E RID: 30846 RVA: 0x00299F5F File Offset: 0x0029815F
		public Tactician()
			: base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020CF RID: 8399
		// (get) Token: 0x0600787F RID: 30847 RVA: 0x00299F6C File Offset: 0x0029816C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x170020D0 RID: 8400
		// (get) Token: 0x06007880 RID: 30848 RVA: 0x00299F79 File Offset: 0x00298179
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x170020D1 RID: 8401
		// (get) Token: 0x06007881 RID: 30849 RVA: 0x00299F81 File Offset: 0x00298181
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x00299F90 File Offset: 0x00298190
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x00299FD3 File Offset: 0x002981D3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
