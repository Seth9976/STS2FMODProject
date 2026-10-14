using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0E RID: 2574
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Impervious : CardModel
	{
		// Token: 0x0600734F RID: 29519 RVA: 0x0028F21F File Offset: 0x0028D41F
		public Impervious()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001EB1 RID: 7857
		// (get) Token: 0x06007350 RID: 29520 RVA: 0x0028F22C File Offset: 0x0028D42C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EB2 RID: 7858
		// (get) Token: 0x06007351 RID: 29521 RVA: 0x0028F22F File Offset: 0x0028D42F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(30m, ValueProp.Move));
			}
		}

		// Token: 0x17001EB3 RID: 7859
		// (get) Token: 0x06007352 RID: 29522 RVA: 0x0028F243 File Offset: 0x0028D443
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007353 RID: 29523 RVA: 0x0028F24C File Offset: 0x0028D44C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007354 RID: 29524 RVA: 0x0028F297 File Offset: 0x0028D497
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(10m);
		}
	}
}
