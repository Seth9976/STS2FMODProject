using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020008F6 RID: 2294
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Adrenaline : CardModel
	{
		// Token: 0x06006D80 RID: 28032 RVA: 0x00283793 File Offset: 0x00281993
		public Adrenaline()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x06006D81 RID: 28033 RVA: 0x002837A0 File Offset: 0x002819A0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x06006D82 RID: 28034 RVA: 0x002837BF File Offset: 0x002819BF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x06006D83 RID: 28035 RVA: 0x002837CC File Offset: 0x002819CC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006D84 RID: 28036 RVA: 0x002837D4 File Offset: 0x002819D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (LocalContext.IsMe(base.Owner))
			{
				VfxCmd.PlayFullScreenInCombat("vfx/vfx_adrenaline", base.Owner.Creature);
			}
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06006D85 RID: 28037 RVA: 0x0028381F File Offset: 0x00281A1F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
