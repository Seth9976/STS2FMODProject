using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B1D RID: 2845
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Transfigure : CardModel
	{
		// Token: 0x0600790C RID: 30988 RVA: 0x0029B155 File Offset: 0x00299355
		public Transfigure()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002107 RID: 8455
		// (get) Token: 0x0600790D RID: 30989 RVA: 0x0029B162 File Offset: 0x00299362
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002108 RID: 8456
		// (get) Token: 0x0600790E RID: 30990 RVA: 0x0029B165 File Offset: 0x00299365
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002109 RID: 8457
		// (get) Token: 0x0600790F RID: 30991 RVA: 0x0029B16D File Offset: 0x0029936D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x1700210A RID: 8458
		// (get) Token: 0x06007910 RID: 30992 RVA: 0x0029B17A File Offset: 0x0029937A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					base.EnergyHoverTip,
					HoverTipFactory.Static(StaticHoverTip.ReplayStatic, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x0029B1A0 File Offset: 0x002993A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1), null, this);
			IEnumerable<CardModel> cardToTransfigure = enumerable;
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (CardModel cardModel in cardToTransfigure)
			{
				if (!cardModel.EnergyCost.CostsX && cardModel.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
				{
					cardModel.EnergyCost.AddThisCombat(1, false);
				}
				cardModel.BaseReplayCount++;
			}
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x0029B1EB File Offset: 0x002993EB
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
