using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000991 RID: 2449
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dirge : CardModel
	{
		// Token: 0x0600709F RID: 28831 RVA: 0x00289B9B File Offset: 0x00287D9B
		public Dirge()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D85 RID: 7557
		// (get) Token: 0x060070A0 RID: 28832 RVA: 0x00289BA8 File Offset: 0x00287DA8
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D86 RID: 7558
		// (get) Token: 0x060070A1 RID: 28833 RVA: 0x00289BAB File Offset: 0x00287DAB
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001D87 RID: 7559
		// (get) Token: 0x060070A2 RID: 28834 RVA: 0x00289BB3 File Offset: 0x00287DB3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(3m));
			}
		}

		// Token: 0x17001D88 RID: 7560
		// (get) Token: 0x060070A3 RID: 28835 RVA: 0x00289BC5 File Offset: 0x00287DC5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }),
					HoverTipFactory.FromCard<Soul>(base.IsUpgraded)
				});
			}
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x00289C00 File Offset: 0x00287E00
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int xValue = base.ResolveEnergyXValue();
			for (int i = 0; i < xValue; i++)
			{
				await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			}
			List<Soul> list = Soul.Create(base.Owner, xValue, base.CombatState).ToList<Soul>();
			if (base.IsUpgraded)
			{
				foreach (Soul soul in list)
				{
					CardCmd.Upgrade(soul, CardPreviewStyle.HorizontalLayout);
				}
			}
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Draw, base.Owner, CardPilePosition.Random);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x00289C4B File Offset: 0x00287E4B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(1m);
		}
	}
}
