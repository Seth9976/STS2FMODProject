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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200095A RID: 2394
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Compact : CardModel
	{
		// Token: 0x06006F7C RID: 28540 RVA: 0x002877E4 File Offset: 0x002859E4
		public Compact()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D02 RID: 7426
		// (get) Token: 0x06006F7D RID: 28541 RVA: 0x002877F1 File Offset: 0x002859F1
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D03 RID: 7427
		// (get) Token: 0x06006F7E RID: 28542 RVA: 0x002877F4 File Offset: 0x002859F4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<Fuel>(base.IsUpgraded),
					HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()),
					HoverTipFactory.ForEnergy(this)
				});
			}
		}

		// Token: 0x17001D04 RID: 7428
		// (get) Token: 0x06006F7F RID: 28543 RVA: 0x00287826 File Offset: 0x00285A26
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06006F80 RID: 28544 RVA: 0x0028783C File Offset: 0x00285A3C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsTransformable && c.Type == CardType.Status).ToList<CardModel>();
			List<CardTransformation> list2 = new List<CardTransformation>();
			foreach (CardModel cardModel in list)
			{
				CardModel cardModel2 = base.CombatState.CreateCard<Fuel>(base.Owner);
				if (base.IsUpgraded)
				{
					CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
				}
				list2.Add(new CardTransformation(cardModel, cardModel2));
			}
			await CardCmd.Transform(list2, null, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06006F81 RID: 28545 RVA: 0x00287887 File Offset: 0x00285A87
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(1m);
		}
	}
}
